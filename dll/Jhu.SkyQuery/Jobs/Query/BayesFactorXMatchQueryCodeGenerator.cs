using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlCodeGen.SqlServer;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.IO.Tasks;
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

        public override SqlCommand GetPopulateLinkTableCommand()
        {
            /*
             * using (SqlCommand cmd = new SqlCommand())
            {
                StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.PopulateLinkTable);

                sql.Replace("[$tablename]", CodeGenerator.GetResolvedTableName(GetLinkTable(step.StepNumber)));        // new table name
                sql.Replace("[$zonetable1]", CodeGenerator.GetResolvedTableName(GetMatchTable(step.StepNumber - 1)));  // Match table always has the ZoneID index
                sql.Replace("[$zonetable2]", CodeGenerator.GetResolvedTableName(GetZoneTable(xmatchTables[step.XMatchTable].TableReference)));
                sql.Replace("[$zonedeftable]", CodeGenerator.GetResolvedTableName(GetZoneDefTable(step.StepNumber)));

                cmd.Parameters.Add("@H", SqlDbType.Float).Value = ((BayesFactorXMatchQuery)Query).ZoneHeight;
                cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = ((BayesFactorXMatchQueryStep)step).SearchRadius;

                cmd.CommandText = sql.ToString();
                ExecuteSqlCommand(cmd, CommandTarget.Code);
            }*/

            throw new NotImplementedException();
        }

        public override SqlCommand GetPopulatePairTableCommand(Table pairtable)
        {
            /*
             * if (step.StepNumber != 0)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.PopulatePairTable);

                    sql.Replace("[$pairtable]", CodeGenerator.GetResolvedTableName(GetPairTable(step.StepNumber)));
                    sql.Replace("[$columnlist1]", "[tableA].[MatchID]");
                    sql.Replace("[$columnlist2]", GetPropagatedColumnList(xmatchTables[step.XMatchTable], ColumnListType.ForSelectNoAlias, ColumnListInclude.PrimaryKey, ColumnListNullType.Nothing, null));
                    sql.Replace("[$matchzonetable]", CodeGenerator.GetResolvedTableName(GetMatchTable(step.StepNumber - 1)));  // Match table always has the ZoneID index
                    sql.Replace("[$linktable]", CodeGenerator.GetResolvedTableName((GetLinkTable(step.StepNumber))));
                    sql.Replace("[$zonetable]", CodeGenerator.GetResolvedTableName(GetZoneTable(xmatchTables[step.XMatchTable].TableReference)));

                    cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = ((BayesFactorXMatchQueryStep)step).SearchRadius;

                    cmd.CommandText = sql.ToString();
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }
            }
             * */

            throw new NotImplementedException();
        }
        
        protected override string GetCreateMatchTableScript(XMatchQueryStep step)
        {
            return BayesFactorXMatchScripts.CreateMatchTable;
        }

        protected override string GetPopulateInitialMatchTableScript(XMatchQueryStep step, SqlCommand cmd)
        {
            /*var sql = new StringBuilder();

            if (Region != null)
            {
                sql.Append(BayesFactorXMatchScripts.PopulateInitialMatchTableRegion);
            }
            else
            {
                sql.Append(BayesFactorXMatchScripts.PopulateInitialMatchTable);
            }

            sql.Replace("[$weight]", GetWeightExpression(SqlServerCodeGenerator.GetCode(xmatchTables[step.XMatchTable].Coordinates.ErrorExpression, false)));
            sql.Replace("[$n]", steps.Count.ToString());

            return sql.ToString();*/

            throw new NotImplementedException();
        }

        protected override string GetPopulateMatchTableScript(XMatchQueryStep step, SqlCommand cmd)
        {
            /*
             * 
             * StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.PopulateMatchTable);

            // has to change alias to tableB and back to avoid side-effects
            string oldalias = xmatchTables[step.XMatchTable].TableReference.Alias;
            xmatchTables[step.XMatchTable].TableReference.Alias = "tableB";         // *** TODO
            sql.Replace("[$weight]", GetWeightExpression(SqlServerCodeGenerator.GetCode(xmatchTables[step.XMatchTable].Coordinates.ErrorExpression, true)));
            xmatchTables[step.XMatchTable].TableReference.Alias = oldalias;

            double lmin = 0;
            double amax = 0;

            // TODO: this fails here if only a single constant error is given instead of constant and limits
            for (int i = step.StepNumber + 1; i < steps.Count; i++)
            {
                // Swap max/min because w = 1 / s^2
                var minexp = xmatchTables[steps[i].XMatchTable].Coordinates.ErrorMinExpression;
                var maxexp = xmatchTables[steps[i].XMatchTable].Coordinates.ErrorMaxExpression;

                // TODO: add logic to handle case when no min/max specified

                lmin += Math.Log(GetWeight(double.Parse(SqlServerCodeGenerator.GetCode(maxexp, false))));
                amax += GetWeight(double.Parse(SqlServerCodeGenerator.GetCode(minexp, false)));
            }

            double factor = (steps.Count - 1) * Math.Log(2);

            cmd.Parameters.Add("@factor", SqlDbType.Float).Value = factor;
            cmd.Parameters.Add("@lmin", SqlDbType.Float).Value = lmin;
            cmd.Parameters.Add("@amax", SqlDbType.Float).Value = amax;
            cmd.Parameters.Add("@limit", SqlDbType.Float).Value = Math.Log(((BayesFactorXMatchQuery)Query).Limit);

            return sql.ToString();
             * */

            throw new NotImplementedException();
        }

        protected override string GetBuildMatchTableIndexScript(Parser.XMatchTableSpecification table, int stepNumber)
        {
            return BayesFactorXMatchScripts.BuildMatchTableIndex;
        }

        protected override string GetCreateZoneTableScript()
        {
            return BayesFactorXMatchScripts.CreateZoneTable;
        }

        protected override string GetPopulateZoneTableScript(Parser.XMatchTableSpecification table)
        {
            /*
            if (Region != null)
            {
                return BayesFactorXMatchScripts.PopulateZoneTableRegion;
            }
            else
            {
                return BayesFactorXMatchScripts.PopulateZoneTable;
            }*/

            throw new NotImplementedException();
        }

        public SqlCommand GetComputeSearchRadiusCommand()
        {
            /*
            // This is done only for the second catalog and on, based on the
            // output match table of the previous step
            if (step.StepNumber > 0)
            {
                var sql = new StringBuilder(BayesFactorXMatchScripts.ComputeRSquared);
                
                sql.Replace("[$matchtable]", CodeGenerator.GetResolvedTableName(GetMatchTable(step.StepNumber - 1)));
                sql.Replace("[$a]", "a");
                sql.Replace("[$l]", "l");
                sql.Replace("[$q]", "q");

                using (SqlCommand cmd = new SqlCommand(sql.ToString()))
                {
                    cmd.Parameters.Add("@H", SqlDbType.Float).Value = ((BayesFactorXMatchQuery)Query).ZoneHeight;
                    
                    CalculateSums(step, cmd);

                    var theta = (double)ExecuteSqlCommandScalar(cmd, CommandTarget.Code);
                    theta = Math.Sqrt(theta) * 180.0 / Math.PI;
                    ((BayesFactorXMatchQueryStep)step).SearchRadius = theta;
                }
            }
             * */

            return null;
        }

        /// <summary>
        /// Calculates parameters necessary to compute search radius
        /// </summary>
        /// <param name="step"></param>
        /// <param name="cmd"></param>
        private void CalculateSums(XMatchQueryStep step, SqlCommand cmd)
        {
            /*double lmax = 0;
            double amin = 0;
            double stepmax = 0;

            for (int i = step.StepNumber; i < steps.Count; i++)
            {
                var coords = xmatchTables[steps[i].XMatchTable].Coordinates;

                double min, max;

                if (coords.IsConstantError)
                {
                    min = max = double.Parse(SqlServerCodeGenerator.GetCode(coords.ErrorExpression, false), System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    // Swap max/min because w = 1 / s^2
                    min = double.Parse(SqlServerCodeGenerator.GetCode(coords.ErrorMinExpression, false), System.Globalization.CultureInfo.InvariantCulture);
                    max = double.Parse(SqlServerCodeGenerator.GetCode(coords.ErrorMaxExpression, false), System.Globalization.CultureInfo.InvariantCulture);
                }

                if (i == step.StepNumber)
                {
                    stepmax = max;
                }

                lmax += Math.Log(GetWeight(min));
                amin += GetWeight(max);
            }

            double factor = (steps.Count - 1) * Math.Log(2);
            double wmin = GetWeight(stepmax);

            cmd.Parameters.Add("@weightMin", SqlDbType.Float).Value = wmin;
            cmd.Parameters.Add("@factor", SqlDbType.Float).Value = factor;
            cmd.Parameters.Add("@lmax", SqlDbType.Float).Value = lmax;
            cmd.Parameters.Add("@amin", SqlDbType.Float).Value = amin;
            cmd.Parameters.Add("@limit", SqlDbType.Float).Value = Math.Log(((BayesFactorXMatchQuery)Query).Limit);
             * */
        }

        public override SqlCommand GetTableStatisticsCommand(ITableSource tableSource)
        {
            /*
            var cmd = base.GetTableStatisticsCommand(tableSource);

            cmd.Parameters.Add("@H", SqlDbType.Float).Value = ZoneHeight;

            return cmd;
             * */

            throw new NotImplementedException();
        }
    }
}
