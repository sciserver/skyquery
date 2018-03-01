using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.CodeGeneration.SqlServer;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.IO.Tasks;
using Jhu.SkyQuery.Parser;
using Jhu.Spherical;


namespace Jhu.SkyQuery.Jobs.Query
{
    public class BayesFactorXMatchQueryCodeGenerator : XMatchQueryCodeGenerator
    {

        #region Constructors and initializers

        private BayesFactorXMatchQueryPartition Partition
        {
            get { return queryObject as BayesFactorXMatchQueryPartition; }
        }

        public BayesFactorXMatchQueryCodeGenerator()
        {
        }

        public BayesFactorXMatchQueryCodeGenerator(QueryObject queryObject)
            : base(queryObject)
        {
        }

        #endregion
        #region Source table functions

        protected override StringBuilder GetSelectAugmentedTableTemplate()
        {
            return new StringBuilder(BayesFactorXMatchScripts.SelectAugmentedTable);
        }

        protected override StringBuilder GetSelectAugmentedTableHtmTemplate()
        {
            return new StringBuilder(BayesFactorXMatchScripts.SelectAugmentedTableHtm);
        }

        protected override StringBuilder GenerateAugmentedTableQuery(AugmentedTableQueryOptions options)
        {
            var coords = options.Table.Coordinates;
            var error = GetCoordinateErrorExpression(coords);
            var weight = GetWeightExpressionString(Execute(error));

            var sql = base.GenerateAugmentedTableQuery(options);

            sql.Replace("[$zoneid]", Execute(GetZoneIdExpression(coords)));
            sql.Replace("[$weight]", weight);

            // When called from HTM table statistics, there is no partition yet
            // This is a simple hack here, should do it for now
            if (Partition != null)
            {
                sql.Replace("[$n]", Partition.Steps.Count.ToString());
            }
            else
            {
                sql.Replace("[$n]", "1");
            }

            return sql;
        }

        protected override void SubstituteAugmentedTableColumns(StringBuilder sql, AugmentedTableQueryOptions options)
        {
            base.SubstituteAugmentedTableColumns(sql, options);

            var coords = options.Table.Coordinates;
            SubstituteCoordinates(sql, coords);
            SubstituteHtmId(sql, coords);
        }

        #endregion
        #region Search radius functions

        public SqlCommand GetComputeSearchRadiusCommand(XMatchQueryStep step)
        {
            // Partitioning is applied on the very first table only. The rest of steps
            // uses the previous match table.

            var table = Partition.Query.XMatchTables[step.XMatchTable];
            var coords = table.Coordinates;
            var region = table.Region;
            var matchtable = GetMatchTable(step);
            var sql = new StringBuilder(BayesFactorXMatchScripts.ComputeRSquared);

            // Generate the augmented table query
            if (step.StepNumber == 0)
            {
                var options = new AugmentedTableQueryOptions(table.TableSource, region)
                {
                    ColumnContext = ColumnContext.All
                };

                sql.Replace("[$query]", GenerateAugmentedTableQuery(options).ToString());
            }
            else
            {
                var query = GenerateSelectStarQuery(matchtable, -1);

                sql.Replace("[$query]", query);
            }

            var cmd = new SqlCommand(sql.ToString());

            AppendComputeSearchRadiusCommandParameters(step, cmd);
            AppendPartitioningConditionParameters(cmd);
            AppendRegionParameter(cmd, region);

            return cmd;
        }

        /// <summary>
        /// Calculates parameters necessary to compute search radius
        /// </summary>
        /// <param name="step"></param>
        /// <param name="cmd"></param>
        private void AppendComputeSearchRadiusCommandParameters(XMatchQueryStep step, SqlCommand cmd)
        {
            double lmax = 0;
            double amin = 0;
            double stepmax = 0;

            // Compute search radius based on the error limits specified for
            // all xmatch tables that have been processed so far

            // Error is measured in arc sec
            // Weight is calculated as w = 1 / s^2, where
            // s^2 is expressed in rad^2

            // TODO: add logic to compute min/max error

            
            for (int i = step.StepNumber; i < Partition.Steps.Count; i++)
            {
                var table = Partition.Query.XMatchTables[Partition.Steps[i].XMatchTable];
                var coords = table.Coordinates;

                double min, max;

                if (!coords.IsErrorHintSpecified)
                {
                    min = max = Constants.DefaultError;
                }
                else if (coords.IsConstantError)
                {
                    var error = GetCoordinateErrorExpression(coords);
                    min = max = double.Parse(Execute(error), System.Globalization.CultureInfo.InvariantCulture);
                }
                else if (!coords.IsErrorLimitsHintSpecified)
                {
                    // TODO: something needs to be done with it
                    min = max = Constants.DefaultError;
                }
                else
                {
                    // TODO: instead of rendering the expression, try to find the number in it
                    min = double.Parse(Execute(coords.ErrorHintMinExpression), System.Globalization.CultureInfo.InvariantCulture);
                    max = double.Parse(Execute(coords.ErrorHintMaxExpression), System.Globalization.CultureInfo.InvariantCulture);
                }

                if (i == step.StepNumber)
                {
                    stepmax = max;
                }

                // Swap max/min because w = 1 / s^2
                lmax += Math.Log(GetWeight(min));
                amin += GetWeight(max);
            }

            double wmin = GetWeight(stepmax);

            AppendZoneHeightParameter(cmd);
            cmd.Parameters.Add("@weightMin", SqlDbType.Float).Value = wmin;
            cmd.Parameters.Add("@lmax", SqlDbType.Float).Value = lmax;
            cmd.Parameters.Add("@amin", SqlDbType.Float).Value = amin;
            cmd.Parameters.Add("@stepCount", SqlDbType.SmallInt).Value = Partition.Steps.Count;
            cmd.Parameters.Add("@limit", SqlDbType.Float).Value = Math.Log(Partition.Query.Limit);
        }

        #endregion
        #region Match table functions

        protected override string GetCreateMatchTableScript()
        {
            return BayesFactorXMatchScripts.CreateMatchTable;
        }

        protected override string GetPopulateMatchTableScript()
        {
            return BayesFactorXMatchScripts.PopulateMatchTable;
        }

        protected override void AppendPopulateMatchTableParameters(XMatchQueryStep step, SqlCommand cmd)
        {
            double lmin = 0;
            double amax = 0;

            // Compute the sum and log sum of weights of the _remaining_ catalogs
            // This will give us a lower limit on the Bayes factor, so we can
            // apply a cut reliably
            for (int i = step.StepNumber + 1; i < Partition.Steps.Count; i++)
            {
                Expression minexp, maxexp;
                var coords = Partition.Query.XMatchTables[Partition.Steps[i].XMatchTable].Coordinates;

                if (coords.IsErrorHintSpecified && coords.IsConstantError)
                {
                    minexp = maxexp = coords.ErrorHintExpression;
                }
                else if (coords.IsErrorHintSpecified)
                {
                    // Swap max/min because w = 1 / s^2 <--- TODO: verify this
                    minexp = coords.ErrorHintMinExpression;
                    maxexp = coords.ErrorHintMaxExpression;
                }
                else
                {
                    // TODO: return maybe 1?
                    throw new NotImplementedException();
                }
                
                // TODO: add logic to handle case when no min/max specified

                lmin += Math.Log(GetWeight(double.Parse(SqlServerCodeGenerator.GetCode(maxexp, false))));
                amax += GetWeight(double.Parse(SqlServerCodeGenerator.GetCode(minexp, false)));
            }

            cmd.Parameters.Add("@stepNumber", SqlDbType.SmallInt).Value = step.StepNumber + 1;
            cmd.Parameters.Add("@stepCount", SqlDbType.SmallInt).Value = Partition.Steps.Count;
            cmd.Parameters.Add("@lmin", SqlDbType.Float).Value = lmin;
            cmd.Parameters.Add("@amax", SqlDbType.Float).Value = amax;
            cmd.Parameters.Add("@limit", SqlDbType.Float).Value = Math.Log(Partition.Query.Limit);
        }

        protected override string GetBuildMatchTableIndexScript()
        {
            return BayesFactorXMatchScripts.BuildMatchTableIndex;
        }

        #endregion
    }
}
