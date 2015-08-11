using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlCodeGen.SqlServer;
using Jhu.SkyQuery.Parser;


namespace Jhu.SkyQuery.Jobs.Query
{
    /// <summary>
    /// Implements functions to perform a bayesian cross-match operation
    /// over a partition of a table.
    /// </summary>
    [Serializable]
    public class BayesFactorXMatchQueryPartition : XMatchQueryPartition
    {
        #region Constructors and initializer functions

        public BayesFactorXMatchQueryPartition()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public BayesFactorXMatchQueryPartition(BayesFactorXMatchQueryPartition old)
            : base(old)
        {
            CopyMembers(old);
        }

        public BayesFactorXMatchQueryPartition(BayesFactorXMatchQuery query, gw.Context context)
            : base(query, context)
        {
            InitializeMembers(new StreamingContext());
        }

        /// <summary>
        /// Initializes private member variables
        /// </summary>
        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
        }

        private void CopyMembers(BayesFactorXMatchQueryPartition old)
        {
        }

        /// <summary>
        /// Generates cross-match steps.
        /// </summary>
        /// <param name="tables">Source tables for cross-matching</param>
        /// <remarks>
        /// This function generates n steps, where n is the number of the
        /// source tables. The first step will be slightly different, since
        /// we don't build the Match and Zone_Match tables, only views on
        /// the zone table of source table.
        /// </remarks>
        public override void GenerateSteps(XMatchTableSpecification[] tables)
        {
            System.Diagnostics.Debug.Assert(steps.Count == 0);

            for (int i = 0; i < tables.Length; i++)
            {
                var s = new BayesFactorXMatchQueryStep(this, Context);

                s.StepNumber = i;
                s.XMatchTable = tables[i].TableReference.UniqueName;

                steps.Add(s);
            }
        }

        #endregion
        #region Zone table functions

        protected override string GetCreateZoneTableScript()
        {
            return BayesFactorXMatchScripts.CreateZoneTable;
        }

        protected override string GetPopulateZoneTableScript(XMatchTableSpecification table)
        {
            if (region != null)
            {
                return BayesFactorXMatchScripts.PopulateZoneTableRegion;
            }
            else
            {
                return BayesFactorXMatchScripts.PopulateZoneTable;
            }
        }

        #endregion
        #region Compute search radius

        /// <summary>
        /// Calculates the search radius by taking the constant error value or error limits of each
        /// catalog into account.
        /// </summary>
        /// <param name="step"></param>
        public void ComputeSearchRadius(XMatchQueryStep step)
        {
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
        }

        /// <summary>
        /// Calculates parameters necessary to compute search radius
        /// </summary>
        /// <param name="step"></param>
        /// <param name="cmd"></param>
        private void CalculateSums(XMatchQueryStep step, SqlCommand cmd)
        {
            double lmax = 0;
            double amin = 0;
            double stepmax = 0;

            for (int i = step.StepNumber; i < steps.Count; i++)
            {
                var coords = xmatchTableSpecifications[steps[i].XMatchTable].Coordinates;

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
        }

        #endregion
        #region Link table functions

        protected override void PopulateLinkTable(XMatchQueryStep step)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.PopulateLinkTable);

                sql.Replace("[$tablename]", CodeGenerator.GetResolvedTableName(GetLinkTable(step.StepNumber)));        // new table name
                sql.Replace("[$zonetable1]", CodeGenerator.GetResolvedTableName(GetMatchTable(step.StepNumber - 1)));  // Match table always has the ZoneID index
                sql.Replace("[$zonetable2]", CodeGenerator.GetResolvedTableName(GetZoneTable(xmatchTableSpecifications[step.XMatchTable].TableReference)));
                sql.Replace("[$zonedeftable]", CodeGenerator.GetResolvedTableName(GetZoneDefTable(step.StepNumber)));

                cmd.Parameters.Add("@H", SqlDbType.Float).Value = ((BayesFactorXMatchQuery)Query).ZoneHeight;
                cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = ((BayesFactorXMatchQueryStep)step).SearchRadius;

                cmd.CommandText = sql.ToString();
                ExecuteSqlCommand(cmd, CommandTarget.Code);
            }
        }

        #endregion
        #region Pair table functions

        protected override void PopulatePairTable(XMatchQueryStep step)
        {
            if (step.StepNumber != 0)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.PopulatePairTable);

                    sql.Replace("[$pairtable]", CodeGenerator.GetResolvedTableName(GetPairTable(step.StepNumber)));
                    sql.Replace("[$columnlist1]", "[tableA].[MatchID]");
                    sql.Replace("[$columnlist2]", GetPropagatedColumnList(xmatchTableSpecifications[step.XMatchTable], ColumnListType.ForSelectNoAlias, ColumnListInclude.PrimaryKey, ColumnListNullType.Nothing, null));
                    sql.Replace("[$matchzonetable]", CodeGenerator.GetResolvedTableName(GetMatchTable(step.StepNumber - 1)));  // Match table always has the ZoneID index
                    sql.Replace("[$linktable]", CodeGenerator.GetResolvedTableName((GetLinkTable(step.StepNumber))));
                    sql.Replace("[$zonetable]", CodeGenerator.GetResolvedTableName(GetZoneTable(xmatchTableSpecifications[step.XMatchTable].TableReference)));

                    cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = ((BayesFactorXMatchQueryStep)step).SearchRadius;

                    cmd.CommandText = sql.ToString();
                    ExecuteSqlCommand(cmd, CommandTarget.Code);
                }
            }
        }

        #endregion
        #region Match table functions

        protected override string GetCreateMatchTableScript(XMatchQueryStep step)
        {
            return BayesFactorXMatchScripts.CreateMatchTable;
        }

        protected override string GetPopulateInitialMatchTableScript(XMatchQueryStep step, SqlCommand cmd)
        {
            var sql = new StringBuilder();

            if (region != null)
            {
                sql.Append(BayesFactorXMatchScripts.PopulateInitialMatchTableRegion);
            }
            else
            {
                sql.Append(BayesFactorXMatchScripts.PopulateInitialMatchTable);
            }

            sql.Replace("[$weight]", GetWeightExpression(SqlServerCodeGenerator.GetCode(xmatchTableSpecifications[step.XMatchTable].Coordinates.ErrorExpression, false)));
            sql.Replace("[$n]", steps.Count.ToString());

            return sql.ToString();
        }

        protected override string GetPopulateMatchTableScript(XMatchQueryStep step, SqlCommand cmd)
        {
            StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.PopulateMatchTable);

            // has to change alias to tableB and back to avoid side-effects
            string oldalias = xmatchTableSpecifications[step.XMatchTable].TableReference.Alias;
            xmatchTableSpecifications[step.XMatchTable].TableReference.Alias = "tableB";         // *** TODO
            sql.Replace("[$weight]", GetWeightExpression(SqlServerCodeGenerator.GetCode(xmatchTableSpecifications[step.XMatchTable].Coordinates.ErrorExpression, true)));
            xmatchTableSpecifications[step.XMatchTable].TableReference.Alias = oldalias;

            double lmin = 0;
            double amax = 0;

            // TODO: this fails here if only a single constant error is given instead of constant and limits
            for (int i = step.StepNumber + 1; i < steps.Count; i++)
            {
                // Swap max/min because w = 1 / s^2
                var minexp = xmatchTableSpecifications[steps[i].XMatchTable].Coordinates.ErrorMinExpression;
                var maxexp = xmatchTableSpecifications[steps[i].XMatchTable].Coordinates.ErrorMaxExpression;

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
        }

        protected override string GetBuildMatchTableIndexScript(XMatchTableSpecification table, int stepNumber)
        {
            return BayesFactorXMatchScripts.BuildMatchTableIndex;
        }

        #endregion

        public override object Clone()
        {
            return new BayesFactorXMatchQueryPartition(this);
        }
    }
}
