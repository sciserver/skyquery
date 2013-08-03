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
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlParser.SqlCodeGen;
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
            :base()
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
                s.PreviousXMatchTable = (i == 0) ? null : tables[i - 1].TableReference.FullyQualifiedName;
                s.XMatchTable = tables[i].TableReference.FullyQualifiedName;

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
            return BayesFactorXMatchScripts.PopulateZoneTable;
        }

        #endregion
        #region Compute search radius

        public void ComputeSearchRadius(XMatchQueryStep step)
        {
            if (step.StepNumber > 0)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.ComputeRSquared);

                    CalculateSums(step, cmd);

                    sql.Replace("[$matchtable]", QuoteSchemaAndTableName(GetMatchTableName(step.StepNumber - 1)));
                    sql.Replace("[$tablename]", QuoteSchemaAndTableName(GetLinkTableName(step.StepNumber)));        // new table name
                    sql.Replace("[$zonetable1]", QuoteSchemaAndTableName(GetMatchTableName(step.StepNumber - 1)));  // Match table always has the ZoneID index
                    sql.Replace("[$zonetable2]", QuoteSchemaAndTableName(GetZoneTableName(xmatchTables[step.XMatchTable])));

                    cmd.Parameters.Add("@H", SqlDbType.Float).Value = ((BayesFactorXMatchQuery)Query).ZoneHeight;

                    cmd.CommandText = sql.ToString();
                    double theta = (double)ExecuteSqlCommandOnTemporaryDatabaseScalar(cmd);
                    theta = Math.Sqrt(theta) * 180.0 / Math.PI;
                    ((BayesFactorXMatchQueryStep)step).SearchRadius = theta;
                }
            }
        }

        #endregion
        #region Link table functions

        protected override void PopulateLinkTable(XMatchQueryStep step)
        {

            using (SqlCommand cmd = new SqlCommand())
            {
                StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.PopulateLinkTable);

                sql.Replace("[$matchtable]", QuoteSchemaAndTableName(GetMatchTableName(step.StepNumber - 1)));
                sql.Replace("[$tablename]", QuoteSchemaAndTableName(GetLinkTableName(step.StepNumber)));        // new table name
                sql.Replace("[$zonetable1]", QuoteSchemaAndTableName(GetMatchTableName(step.StepNumber - 1)));  // Match table always has the ZoneID index
                sql.Replace("[$zonetable2]", QuoteSchemaAndTableName(GetZoneTableName(xmatchTables[step.XMatchTable])));
                sql.Replace("[$zonedeftable]", QuoteSchemaAndTableName(GetZoneDefTableName(step.StepNumber)));

                cmd.Parameters.Add("@H", SqlDbType.Float).Value = ((BayesFactorXMatchQuery)Query).ZoneHeight;
                cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = ((BayesFactorXMatchQueryStep)step).SearchRadius;

                cmd.CommandText = sql.ToString();
                ExecuteSqlCommandOnTemporaryDatabase(cmd);
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

                    sql.Replace("[$pairtable]", QuoteSchemaAndTableName(GetPairTableName(step.StepNumber)));
                    sql.Replace("[$columnlist1]", "[tableA].[MatchID]");
                    sql.Replace("[$columnlist2]", GetPropagatedColumnList(xmatchTables[step.XMatchTable], ColumnListType.ForSelectNoAlias, ColumnListInclude.PrimaryKey, ColumnListNullType.Nothing, null));
                    sql.Replace("[$matchtable]", QuoteSchemaAndTableName(GetMatchTableName(step.StepNumber - 1)));
                    sql.Replace("[$matchzonetable]", QuoteSchemaAndTableName(GetMatchTableName(step.StepNumber - 1)));  // Match table always has the ZoneID index
                    sql.Replace("[$linktable]", QuoteSchemaAndTableName((GetLinkTableName(step.StepNumber))));
                    sql.Replace("[$zonetable]", QuoteSchemaAndTableName(GetZoneTableName(xmatchTables[step.XMatchTable])));

                    cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = ((BayesFactorXMatchQueryStep)step).SearchRadius;

                    cmd.CommandText = sql.ToString();
                    ExecuteSqlCommandOnTemporaryDatabase(cmd);
                }
            }
        }

        private void CalculateSums(XMatchQueryStep step, SqlCommand cmd)
        {
            double lmax = 0;
            double amin = 0;
            double stepmax = 0;

            for (int i = step.StepNumber; i < steps.Count; i++)
            {
                var xt = xmatchTables[steps[i].XMatchTable];

                double min, max;

                if (xt.IsConstantError)
                {
                    min = max = double.Parse(SqlServerCodeGenerator.GetCode(xt.ErrorExpression, false), System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    // Swap max/min because w = 1 / s^2
                    min = double.Parse(SqlServerCodeGenerator.GetCode(xt.MinErrorExpression, false), System.Globalization.CultureInfo.InvariantCulture);
                    max = double.Parse(SqlServerCodeGenerator.GetCode(xt.MaxErrorExpression, false), System.Globalization.CultureInfo.InvariantCulture);
                }

                if (i == step.StepNumber)
                {
                    stepmax = max;
                }
                
                lmax += Math.Log(GetWeight(min));
                amin += GetWeight(max);
            }

            //var stepmax = double.Parse(SqlServerCodeGenerator.GetCode(xmatchTables[step.XMatchTable].MaxErrorExpression, false), System.Globalization.CultureInfo.InvariantCulture);

            double factor = (steps.Count - 1) * Math.Log(2);
            double wmin = GetWeight(stepmax);

            cmd.Parameters.Add("@weightMin", SqlDbType.Float).Value = wmin;
            cmd.Parameters.Add("@factor", SqlDbType.Float).Value = factor;
            cmd.Parameters.Add("@lmax", SqlDbType.Float).Value = lmax;
            cmd.Parameters.Add("@amin", SqlDbType.Float).Value = amin;
            cmd.Parameters.Add("@limit", SqlDbType.Float).Value = Math.Log(((BayesFactorXMatchQuery)Query).Limit);
        }

        #endregion
        #region Match table functions

        protected override string GetCreateMatchTableScript(XMatchQueryStep step)
        {
            return BayesFactorXMatchScripts.CreateMatchTable;
        }

        protected override string GetPopulateInitialMatchTableScript(XMatchQueryStep step, SqlCommand cmd)
        {
            StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.PopulateInitialMatchTable);

            sql.Replace("[$weight]", GetWeightExpression(SqlServerCodeGenerator.GetCode(xmatchTables[step.XMatchTable].ErrorExpression, false)));
            sql.Replace("[$n]", steps.Count.ToString());

            return sql.ToString();
        }

        protected override string GetPopulateMatchTableScript(XMatchQueryStep step, SqlCommand cmd)
        {
            StringBuilder sql = new StringBuilder(BayesFactorXMatchScripts.PopulateMatchTable);

            // has to change alias to tableB and back to avoid side-effects
            string oldalias = xmatchTables[step.XMatchTable].TableReference.Alias;
            xmatchTables[step.XMatchTable].TableReference.Alias = "tableB";
            sql.Replace("[$weight]", GetWeightExpression(SqlServerCodeGenerator.GetCode(xmatchTables[step.XMatchTable].ErrorExpression, true)));
            xmatchTables[step.XMatchTable].TableReference.Alias = oldalias;

            double lmin = 0;
            double amax = 0;

            for (int i = step.StepNumber + 1; i < steps.Count; i++)
            {
                // Swap max/min because w = 1 / s^2
                var minexp = xmatchTables[steps[i].XMatchTable].MinErrorExpression;
                var maxexp = xmatchTables[steps[i].XMatchTable].MaxErrorExpression;

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


        public override void ExecuteQuery()
        {
            // Idáig egész jó, de itt valamiért már nem megy az xmatch query

            string sql = GetOutputSelectQuery();
            string temptable = GetTemporaryTableName(Query.TemporaryDestinationTableName);
            SqlConnectionStringBuilder cs;

            switch (Query.ExecutionMode)
            {
                case Graywulf.Jobs.Query.ExecutionMode.SingleServer:
                    cs = GetTemporaryDatabaseConnectionString();
                    break;
                case Graywulf.Jobs.Query.ExecutionMode.Graywulf:
                    cs = GetTemporaryDatabaseConnectionString();;
                    break;
                default:
                    throw new NotImplementedException();
            }

            ExecuteSelectInto(cs.ConnectionString,
                sql,
                cs.InitialCatalog, 
                Query.TemporarySchemaName,
                temptable,
                Query.QueryTimeout);

            if ((Query.ResultsetTarget & Jhu.Graywulf.Jobs.Query.ResultsetTarget.TemporaryTable) == 0)
            {
                TemporaryTables.TryAdd(temptable, temptable);
            }
        }

        public override DatasetBase GetDestinationTableSchemaSourceDataset()
        {
            return new SqlServerDataset()
            {
                ConnectionString = TemporaryDatabaseInstanceReference.Value.GetConnectionString().ConnectionString
            };
        }

        public override string GetDestinationTableSchemaSourceQuery()
        {
            String sql = String.Format("SELECT tablealias.* FROM {0} AS tablealias",
                QuoteSchemaAndTableName(GetTemporaryTableName(Query.TemporaryDestinationTableName)));

            return sql;
        }

#if false
        private string GetOutputSelectQuery()
        {
            // **** TODO: this disrupts the select statement
            // a copy could be made of the entire parsing tree
            // but reparsing is easier so it will be reset at the end

            var qs = SelectStatement.EnumerateQuerySpecifications().First<Jhu.Graywulf.SqlParser.QuerySpecification>();

            // Collect tables that are part of the XMatch operation
            var fc = qs.FindDescendant<Jhu.Graywulf.SqlParser.FromClause>();
            var xmc = qs.FindDescendant<XMatchClause>();
            var xmtstr = new List<TableReference>(xmc.EnumerateXMatchTableSpecifications().Select(ts => ts.TableReference));

            var matchtable = GetMatchTableName(steps.Count - 1);

            // Replace column references to point to match table
            foreach (ColumnReference cr in qs.ColumnReferences)
            {
                if (cr.TableReference != null && cr.TableReference.IsComputed)
                {
                    // In case of a computed table (typically xmatch results table)
                }
                else if (cr.TableReference != null && !cr.TableReference.IsComputed && !cr.TableReference.IsSubquery && !cr.TableReference.IsUdf)
                {
                    // In case of other tables
                    // See if it's an xmatched table or not
                    if (xmtstr.Where(tri => tri.Compare(cr.TableReference)).FirstOrDefault() == null)
                    {
                        continue;
                    }

                    cr.ColumnName = GetEscapedPropagatedColumnName(cr.TableReference, cr.ColumnName);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            // Replace table references
            // This must be a separate step to save original table aliases in escaped names
            foreach (ColumnReference cr in qs.ColumnReferences)
            {
                if (cr.TableReference != null && cr.TableReference.IsComputed)
                {
                    // In case of a computed table (typically xmatch results table)
                    cr.TableReference.Alias = "matchtable";
                }
                else if (cr.TableReference != null && !cr.TableReference.IsComputed && !cr.TableReference.IsSubquery && !cr.TableReference.IsUdf)
                {
                    // In case of other tables
                    // See if it's an xmatched table or not
                    if (xmtstr.Where(tri => tri.Compare(cr.TableReference)).FirstOrDefault() == null)
                    {
                        continue;
                    }
                    cr.TableReference.Alias = "matchtable";
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            // Remove table specifications used in xmatch
            ReplaceXMatchTableSources(fc, xmtstr);

            // Remove XMatch clause
            xmc.Parent.Stack.Remove(xmc);

            var code = SqlServerCodeGenerator.GetCode(SelectStatement, true);

            // Now zero out the selectStatement to force reparsing
            SelectStatement = null;

            return code;
        }
#endif

        #endregion

        public override object Clone()
        {
            return new BayesFactorXMatchQueryPartition(this);
        }
    }
}
