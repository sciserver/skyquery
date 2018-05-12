using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Schema.SqlServer;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.CodeGeneration;
using Jhu.Graywulf.Sql.CodeGeneration.SqlServer;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.IO.Tasks;
using Jhu.SkyQuery.Sql.Parsing;
using Jhu.Spherical;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    public class XMatchQueryCodeGenerator : RegionQueryCodeGenerator
    {
        #region Constants

        protected const string zoneHeightParameterName = "@H";

        #endregion
        #region Properties

        private XMatchQueryPartition Partition
        {
            get { return queryObject as XMatchQueryPartition; }
        }

        private XMatchQuery Query
        {
            get
            {
                if (queryObject is XMatchQuery)
                {
                    return (XMatchQuery)queryObject;
                }
                else if (queryObject is XMatchQueryPartition)
                {
                    return Partition.Query;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion
        #region Constructors and initializers

        public XMatchQueryCodeGenerator()
        {
        }

        public XMatchQueryCodeGenerator(QueryObject queryObject)
            : base(queryObject)
        {
        }

        #endregion

        protected Expression CreateZoneHeightParameter()
        {
            return Expression.Create(Jhu.Graywulf.Sql.Parsing.Variable.Create("@H"));
        }

        public Expression GetCoordinateErrorExpression(TableCoordinates coords)
        {
            if (coords.IsErrorHintSpecified)
            {
                return coords.ErrorHintExpression;
            }
            else
            {
                return Expression.CreateNumber(Constants.DefaultError.ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            // TODO: Figure out from metadata
        }

        protected BooleanExpression GenerateExcludeZeroWeightCondition(CoordinatesTableSource table)
        {
            var a = Expression.CreateNumber("0");
            var b = GetCoordinateErrorExpression(table.Coordinates);
            var p = Predicate.CreateGreaterThan(b, a);
            return BooleanExpression.Create(false, p);
        }

        public Expression GetZoneIdExpression(TableCoordinates coords)
        {
            if (coords.IsZoneIdHintSpecified)
            {
                return coords.ZoneIdHintExpression;
            }
            else if (coords.IsEqHintSpecified)
            {
                return CreateFunction("ZoneIDFromDec", coords.DecHintExpression, CreateZoneHeightParameter());
            }
            else if (coords.IsCartesianHintSpecified)
            {
                return CreateFunction("ZoneIDFromZ", coords.ZHintExpression, CreateZoneHeightParameter());
            }
            else if (FallBackToDefaultColumns && coords.IsZoneIdColumnAvailable)
            {
                return coords.ZoneIdColumnExpression;
            }
            else if (FallBackToDefaultColumns && coords.IsEqColumnsAvailable)
            {
                return CreateFunction("ZoneIDFromDec", coords.DecColumnExpression, CreateZoneHeightParameter());
            }
            else if (FallBackToDefaultColumns && coords.IsCartesianColumnsAvailable)
            {
                return CreateFunction("ZoneIDFromZ", coords.ZColumnExpression, CreateZoneHeightParameter());
            }
            else
            {
                // TODO: Figure out from metadata
                throw Error.NoCoordinateColumnFound(coords, "zoneid");
            }
        }

        public Index FindZoneIndex(TableCoordinates coords)
        {
            Index idx = null;

            if (coords.IsZoneIdHintSpecified)
            {
                var cr = coords.ZoneIdHintExpression.FindDescendant<AnyVariable>().FindDescendant<ColumnIdentifier>().ColumnReference;
                idx = FindIndexWithFirstKey(coords.Table, cr.ColumnName);
            }
            else if (coords.IsEqHintSpecified || coords.IsCartesianHintSpecified)
            {
                // Coordinates might be overriden, do not use fallback
            }
            else if (FallBackToDefaultColumns && coords.IsZoneIdColumnAvailable)
            {
                idx = FindIndexWithFirstKey(coords.Table, TableCoordinates.ZoneIdColumnName);
            }

            return idx;
        }

        /// <summary>
        /// Returns true if building a zone table on the fly is necessary
        /// </summary>
        /// <returns></returns>
        public bool IsZoneTableNecessary(XMatchTableSpecification table, out CreateZoneTableReason reason)
        {
            // It might be worth building a zone table if:
            // - the doesn't have a zoneID or it's not indexed
            // - the region constraint is small
            // - a region constraint is specified but the table has no HTMID

            // Always build a zone table if not zoneid is available or it is
            // not properly indexed

            if (FindZoneIndex(table.Coordinates) == null)
            {
                reason = CreateZoneTableReason.NoZoneIndexFound;
                return true;
            }

            // TODO: we could decide on omitting a zone table if the region is
            // large but filtering for htmid and join by zoneid cannot be done
            // at the same time
            if (table.Region != null)
            {
                reason = CreateZoneTableReason.RegionFilterSpecified;
                return true;
            }

            // TODO: compare statistics with total row count to find tables with strong
            // where clause conditions and consider building a zone table for them

            reason = CreateZoneTableReason.None;
            return false;
        }

        protected string GetWeightExpressionString(string sigmaExpression)
        {
            return String.Format("POWER(RADIANS(CONVERT(float, {0}) / 3600.00000000), -2)", sigmaExpression);
        }

        protected string GetZoneIDExpressionString(string dec)
        {
            return String.Format("CONVERT(INT,FLOOR((({0}) + 90.0) / @H)) as [ZoneID]", dec);
        }

        protected double GetWeight(double sigma)
        {
            var a = sigma / 3600.0 / 180.0 * Math.PI;
            return 1.0 / (a * a);
        }
        
        #region Search radius functions

        public SqlCommand GetComputeMinMaxErrorCommand(XMatchQueryStep step)
        {
            var sql = new StringBuilder(XMatchScripts.ComputeMinMaxError);
            var table = Query.XMatchTables[step.XMatchTable];
            var coords = table.Coordinates;

            var where = GetTableSpecificWhereClause(table.TableSource);
            var sc = GetPartitioningConditions(GetZoneIdExpression(coords));

            if (where != null && sc != null)
            {
                where.AppendCondition(sc, "AND");
            }
            else if (where == null)
            {
                where = Graywulf.Sql.Parsing.WhereClause.Create(sc);
            }

            sql.Replace("[$tablename]", GetResolvedTableName(table.TableReference));
            sql.Replace("[$error]", Execute(coords.ErrorHintExpression));
            sql.Replace("[$where]", Execute(where));

            var cmd = new SqlCommand(sql.ToString());

            AppendPartitioningConditionParameters(cmd);

            return cmd;
        }

        #endregion
        #region ZoneDef table function

        public Table GetZoneDefTable(XMatchQueryStep step)
        {
            return queryObject.GetTemporaryTable(String.Format("ZoneDef_{0}", step.StepNumber));
        }

        public SqlCommand GetCreateZoneDefTableCommand(XMatchQueryStep step, Table zonedeftable)
        {
            var sql = new StringBuilder(XMatchScripts.CreateZoneDefTable);

            sql.Replace("[$tablename]", GetResolvedTableName(zonedeftable));
            sql.Replace("[$indexname]", GeneratePrimaryKeyName(zonedeftable));

            return new SqlCommand(sql.ToString());
        }

        public SqlCommand GetPopulateZoneDefTableCommand(XMatchQueryStep step, Table zonedeftable)
        {
            // It is not worth imposing partitioning conditions here as buffering would
            // be needed. Partitioning will be taken care of at link table construction.

            var sql = new StringBuilder(XMatchScripts.PopulateZoneDefTable);

            sql.Replace("[$tablename]", GetResolvedTableName(zonedeftable));

            var cmd = new SqlCommand(sql.ToString());

            AppendZoneParameters(cmd);
            cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = step.SearchRadius;

            return cmd;
        }

        #endregion
        #region Link table

        /// <summary>
        /// Generates the name of a temporary link table built
        /// from two zone tables.
        /// </summary>
        /// <remarks>
        /// Will generate a name like user_jobid_partition_Link_step.
        /// This name is unique for the whole system.
        /// </remarks>
        /// <param name="stepNumber">Number of the XMatch step.</param>
        /// <returns>The escaped name of a temporary table.</returns>
        public Table GetLinkTable(XMatchQueryStep step)
        {
            return queryObject.GetTemporaryTable(String.Format("Link_{0}", step.StepNumber));
        }

        /// <summary>
        /// Creates a link table between two zone tables.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public SqlCommand GetCreateLinkTableCommand(XMatchQueryStep step, Table linktable)
        {
            var sql = new StringBuilder(XMatchScripts.CreateLinkTable);

            sql.Replace("[$tablename]", GetResolvedTableName(linktable));
            sql.Replace("[$indexname]", GeneratePrimaryKeyName(linktable));

            return new SqlCommand(sql.ToString());
        }

        /// <summary>
        /// When overriden in derived classes, populates a link
        /// table from two zone tables.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        /// <summary>
        /// Populates a link table using limits determined
        /// by the bayesian cross-match algoritm
        /// </summary>
        /// <param name="step"></param>
        public virtual SqlCommand GetPopulateLinkTableCommand(XMatchQueryStep step, Table zoneDefTable, Table linkTable)
        {
            var sql = new StringBuilder(XMatchScripts.PopulateLinkTable);

            sql.Replace("[$tablename]", GetResolvedTableName(linkTable));        // new table name
            sql.Replace("[$zonedeftable]", GetResolvedTableName(zoneDefTable));

            /* TODO: consider adding these back
             * could be used to pre-filter zones based on objects but it's unlikely to be
             * worth it at this step
            sql.Replace("[$zonetable1]", GetResolvedTableName(GetMatchTable(step.StepNumber - 1)));  // Match table always has the ZoneID index
            sql.Replace("[$zonetable2]", GetResolvedTableName(GetZoneTable(xmatchTables[step.XMatchTable].TableReference)));
             * 
             -- TODO: Optionally join in source tables to reduce number of zone links
             INNER JOIN (SELECT DISTINCT [ZoneID] FROM [$zonetable1]) AS [Z1]
	            ON [Z1].[ZoneID] = [D1].[ZoneID]
             INNER JOIN (SELECT DISTINCT [ZoneID] FROM [$zonetable2]) AS [Z2]
	            ON [Z2].[ZoneID] = [D2].[ZoneID]
             * */

            // Partitioning is done on the ZoneID of the first table
            var tr = new TableReference("D1");
            var cr = new ColumnReference(tr, "ZoneID", DataTypes.SqlInt);
            var exp = Expression.Create(ColumnIdentifier.Create(cr));
            var sc = GetPartitioningConditions(exp);

            if (sc != null)
            {
                var where = Graywulf.Sql.Parsing.WhereClause.Create(sc);
                sql.Replace("[$where]", Execute(where));
            }
            else
            {
                sql.Replace("[$where]", String.Empty);
            }

            var cmd = new SqlCommand(sql.ToString());
            
            cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = Partition.Steps[step.StepNumber].SearchRadius;
            AppendZoneParameters(cmd);
            AppendPartitioningConditionParameters(cmd);

            return cmd;
        }

        protected void AppendZoneParameters(SqlCommand cmd)
        {
            cmd.Parameters.Add(zoneHeightParameterName, SqlDbType.Float).Value = Query.ZoneHeight;
        }

        #endregion
        #region Zone table functions

        /// <summary>
        /// Generates the name of a temporary zone table built
        /// from a match table.
        /// </summary>
        /// <remarks>
        /// Will generate a name like user_jobid_partition_Zone_Match_step.
        /// This name is unique for the whole system.
        /// </remarks>
        /// <param name="stepNumber">Number of the XMatch step</param>
        /// <returns>The escaped name of a temporary table.</returns>
        public Table GetZoneTable(XMatchQueryStep step)
        {
            return queryObject.GetTemporaryTable(String.Format("Zone_{0}", step.StepNumber));
        }

        public SqlCommand GetCreateZoneTableCommand(XMatchQueryStep step, Table zonetable)
        {
            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings();
                AddSourceTableMappings(Partition.Parameters.SourceDatabaseVersionName, null);
            }

            var table = Query.XMatchTables[step.XMatchTable];
            var tr = MapTableReference(table.TableReference);
            var columnlist = new SqlServerColumnListGenerator(tr, ColumnContext.PrimaryKey, ColumnListType.CreateTableWithEscapedName)
            {
                IdentityRendering = ColumnListIdentityRendering.Never,
            };

            var sql = new StringBuilder(XMatchScripts.CreateZoneTable);

            sql.Replace("[$tablename]", GetResolvedTableName(zonetable));
            sql.Replace("[$indexname]", GeneratePrimaryKeyName(zonetable));
            sql.Replace("[$columnlist]", columnlist.Execute());

            return new SqlCommand(sql.ToString());
        }

        /// <summary>
        /// Populates a zone table from a source table.
        /// </summary>
        /// <param name="table">Reference to the source table.</param>
        /// <remarks>
        /// This function propagates primary key and data columns
        /// to the zone table. It also applies filters in the where
        /// clause to the source table to reduce zone table size.
        /// Partitioning conditions also applied here.
        /// </remarks>
        public SqlCommand GetPopulateZoneTableCommand(XMatchQueryStep step, Table zonetable)
        {
            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings();
                AddSourceTableMappings(Partition.Parameters.SourceDatabaseVersionName, null);
            }

            var table = Query.XMatchTables[step.XMatchTable];
            var coords = table.Coordinates;
            var tr = MapTableReference(table.TableReference);

            // Strict partitioning conditions are applied on the very first table and buffering
            // is used for the rest of the tables. Buffering depends on the search radius.
            // When the partition limit is near the poles, buffering should extend to the
            // entire pole region
            var keymin = (IComparable)Convert.ToInt64(Partition.PartitioningKeyMin);
            var keymax = (IComparable)Convert.ToInt64(Partition.PartitioningKeyMax);

            if (step.StepNumber > 0)
            {
                long buffer = step.ZoneBuffer;

                if (!IsPartitioningKeyUnbound(Partition.PartitioningKeyMin))
                {
                    var decmin = ((long)keymin * Query.ZoneHeight) - 90;

                    if (decmin - step.SearchRadius < -89.9)
                    {
                        keymin = null;
                    }
                    else
                    {
                        keymin = (long)keymin - buffer;
                    }
                }
                else
                {
                    keymin = null;
                }

                if (!IsPartitioningKeyUnbound(keymax))
                {
                    var decmax = ((long)keymax * Query.ZoneHeight) - 90;

                    if (decmax + step.SearchRadius > 89.9)
                    {
                        keymax = null;
                    }
                    else
                    {
                        keymax = (long)keymax + buffer;
                    }
                }
                else
                {
                    keymax = null;
                }
            }

            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings();
                AddSourceTableMappings(Partition.Parameters.SourceDatabaseVersionName, null);
            }

            // Generate augmented query for select
            var query = new AugmentedTableQueryOptions(step, table.TableSource, table.Region)
            {
                ColumnContext = ColumnContext.PrimaryKey,
            };

            // Generate primary key list for insert
            var columnlist = new SqlServerColumnListGenerator(tr, ColumnContext.PrimaryKey, ColumnListType.SelectWithEscapedNameNoAlias)
            {
                TableAlias = "__t",
                SeparatorRendering = ColumnListSeparatorRendering.Leading
            };

            var sql = new StringBuilder(XMatchScripts.PopulateZoneTable);

            sql.Replace("[$query]", GenerateAugmentedTableQuery(query).ToString());
            sql.Replace("[$zonetablename]", GetResolvedTableName(zonetable));
            sql.Replace("[$selectcolumnlist]", columnlist.Execute());

            var cmd = new SqlCommand(sql.ToString());

            AppendPartitioningConditionParameters(cmd, keymin, keymax);
            AppendRegionParameter(cmd, table.Region);
            AppendZoneParameters(cmd);

            return cmd;
        }

        protected void SubstituteCoordinates(StringBuilder sql, TableCoordinates coords)
        {
            sql.Replace("[$ra]", Execute(GetRAExpression(coords)));
            sql.Replace("[$dec]", Execute(GetDecExpression(coords)));
            sql.Replace("[$cx]", Execute(GetXExpression(coords)));
            sql.Replace("[$cy]", Execute(GetYExpression(coords)));
            sql.Replace("[$cz]", Execute(GetZExpression(coords)));
        }

        protected void SubstituteZoneId(StringBuilder sql, TableCoordinates coords)
        {
            sql.Replace("[$zoneid]", Execute(GetZoneIdExpression(coords)));
        }

        #endregion
        #region Pair table functions

        /// <summary>
        /// Generates the name of a temporary pair table built
        /// from two zone tables and the link table.
        /// </summary>
        /// <remarks>
        /// Will generate a name like user_jobid_partition_Pair_step.
        /// This name is unique for the whole system.
        /// </remarks>
        /// <param name="stepNumber">Number of the XMatch step.</param>
        /// <returns>The escaped name of a temporary table.</returns>
        public Table GetPairTable(XMatchQueryStep step)
        {
            return queryObject.GetTemporaryTable(String.Format("Pair_{0}", step.StepNumber));
        }

        /// <summary>
        /// Creates a pair table from a link table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public SqlCommand GetCreatePairTableCommand(XMatchQueryStep step, Table pairtable)
        {
            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings();
                AddSourceTableMappings(Partition.Parameters.SourceDatabaseVersionName, null);
            }

            var pstep = Partition.Steps[step.StepNumber - 1];

            string columnlist1, columnlist2;

            GenerateColumnListForCreatePairTable(pstep, 1, out columnlist1);
            GenerateColumnListForCreatePairTable(step, 2, out columnlist2);

            var sql = new StringBuilder(XMatchScripts.CreatePairTable);

            sql.Replace("[$tablename]", GetResolvedTableName(pairtable));
            sql.Replace("[$createcolumnlist1]", columnlist1);
            sql.Replace("[$createcolumnlist2]", columnlist2);

            return new SqlCommand(sql.ToString());
        }

        private void GenerateColumnListForCreatePairTable(XMatchQueryStep step, int i, out string columnlist)
        {
            var table = Query.XMatchTables[step.XMatchTable];

            if (i == 1)
            {
                if (step.StepNumber == 0)
                {
                    // Directly use a source table or zone table for pair table building
                    GenerateColumnListFromSourceTableForCreatePairTable(step, out columnlist);
                }
                else
                {
                    // Use match table computed earlier for pair table building
                    GenerateColumnListFromMatchTableForCreatePairTable(step, out columnlist);
                }
            }
            else if (i == 2)
            {
                // Directly use a source table or zone table for pair table building
                GenerateColumnListFromSourceTableForCreatePairTable(step, out columnlist);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void GenerateColumnListFromSourceTableForCreatePairTable(XMatchQueryStep step, out string columnlist)
        {
            var table = Query.XMatchTables[step.XMatchTable];

            // Primary key of source table or zone table
            // To be used in the pairs select
            var clg = new SqlServerColumnListGenerator(table.TableReference, ColumnContext.PrimaryKey, ColumnListType.CreateTableWithEscapedName)
            {
                NullRendering = ColumnListNullRendering.NotNull,
            };

            columnlist = clg.Execute();
        }

        private void GenerateColumnListFromMatchTableForCreatePairTable(XMatchQueryStep step, out string columnlist)
        {
            // Primary key of previous match table
            columnlist = String.Format("{0} [bigint] NOT NULL", GetEscapedMatchIDString(step.StepNumber));
        }

        /// <summary>
        /// When overriden in derived classes, populates a pair table
        /// from a link table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        public SqlCommand GetPopulatePairTableCommand(XMatchQueryStep step, Table linktable, Table pairtable)
        {
            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings();
                AddSourceTableMappings(Partition.Parameters.SourceDatabaseVersionName, null);
            }

            // Table 1 is, with the exception of the very first step, the match table of a previous xmatch step.
            // In this case the match table contains all necessary columns and includes a zone index.
            // In the very first step Table 1 can either be a catalog table with a zone index that contains all
            // necessary columns, or a zone table built during the xmatch workflow.
            // Similarly, Table 2 is either a catalog table with the right index or a dynamically built zone table.
            // If we use an existing match table or a zone table, we know the column names. Otherwise it's a bit
            // tricky to get the list of columns
            var pstep = Partition.Steps[step.StepNumber - 1];
            var table1 = Query.XMatchTables[pstep.XMatchTable];
            var table2 = Query.XMatchTables[step.XMatchTable];

            // Generate source table queries
            string query1, query2;
            string columnlist1, columnlist2;
            string selectlist1, selectlist2;

            GenerateSourceQueryForPopulatePairTable(pstep, 1, out query1, out columnlist1, out selectlist1);
            GenerateSourceQueryForPopulatePairTable(step, 2, out query2, out columnlist2, out selectlist2);

            StringBuilder sql = new StringBuilder(XMatchScripts.PopulatePairTable);

            sql.Replace("[$query1]", query1);
            sql.Replace("[$query2]", query2);
            sql.Replace("[$columnlist1]", columnlist1);
            sql.Replace("[$columnlist2]", columnlist2);
            sql.Replace("[$selectlist1]", selectlist1);
            sql.Replace("[$selectlist2]", selectlist2);
            sql.Replace("[$pairtable]", GetResolvedTableName(pairtable));
            sql.Replace("[$linktable]", GetResolvedTableName(linktable));

            var cmd = new SqlCommand(sql.ToString());

            // Region and partitioning is same for all tables
            AppendPartitioningConditionParameters(cmd);
            AppendRegionParameter(cmd, table1.Region);
            cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = step.SearchRadius;

            return cmd;
        }

        private void GenerateSourceQueryForPopulatePairTable(XMatchQueryStep step, int i, out string query, out string columnlist, out string selectlist)
        {
            var table = Query.XMatchTables[step.XMatchTable];

            if (i == 1)
            {
                var alias = "__t1";

                if (step.StepNumber == 0 && !IsZoneTableNecessary(table, out CreateZoneTableReason reason))
                {
                    // Partitioning constraints need to be applied on the very first MUST catalog only, as further
                    // matches might cross the partition boundary. On the other hand, in case of MAY catalogs,
                    // outer rows need to be filtered for partition constraints.

                    // Directly use a source table for pair table building
                    GenerateSourceQueryFromSourceTableForPopulatePairTable(step, true, alias, out query, out columnlist, out selectlist);
                }
                else if (step.StepNumber == 0)
                {
                    // Zone tables are already build with partitioning constraints applied but
                    // with some buffering, so outer rows of MAY catalogs will need further
                    // filtering

                    // Use a zone table computed earlier for pair table building
                    GenerateSourceQueryFromZoneTableForPopulatePairTable(step, alias, out query, out columnlist, out selectlist);
                }
                else
                {
                    // Use match table computed earlier for pair table building
                    GenerateSourceQueryFromMatchTableForPopulatePairTable(step, alias, out query, out columnlist, out selectlist);
                }
            }
            else if (i == 2)
            {
                var alias = "__t2";

                if (!IsZoneTableNecessary(table, out CreateZoneTableReason reason))
                {
                    // Directly use a source table for pair table building
                    GenerateSourceQueryFromSourceTableForPopulatePairTable(step, false, alias, out query, out columnlist, out selectlist);
                }
                else
                {
                    // Use a zone table computed earlier for pair table building
                    GenerateSourceQueryFromZoneTableForPopulatePairTable(step, alias, out query, out columnlist, out selectlist);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void GenerateSourceQueryFromSourceTableForPopulatePairTable(XMatchQueryStep step, bool usePartitioning, string alias, out string query, out string columnlist, out string selectlist)
        {
            var table = Query.XMatchTables[step.XMatchTable];
            var tr = MapTableReference(table.TableReference);

            // Directly use a source table for pair table building
            var tqoptions = new AugmentedTableQueryOptions(step, table.TableSource, table.Region)
            {
                ColumnContext = ColumnContext.PrimaryKey,
                EscapeColumnNames = true,
                UsePartitioning = usePartitioning,
            };
            query = GenerateAugmentedTableQuery(tqoptions).ToString();

            // To be used in the pairs select
            var clg = new SqlServerColumnListGenerator(tr, ColumnContext.PrimaryKey, ColumnListType.SelectWithEscapedNameNoAlias)
            {
                TableAlias = alias,
            };

            columnlist = clg.Execute();

            // To be used in the final select that goes into the insert
            var slg = new SqlServerColumnListGenerator(tr, ColumnContext.PrimaryKey, ColumnListType.SelectWithEscapedNameNoAlias)
            {
                TableAlias = ""
            };

            selectlist = slg.Execute();
        }

        private void GenerateSourceQueryFromZoneTableForPopulatePairTable(XMatchQueryStep step, string alias, out string query, out string columnlist, out string selectlist)
        {
            var table = Query.XMatchTables[step.XMatchTable];
            var tr = MapTableReference(table.TableReference);

            // Use a zone table computed earlier for pair table building
            query = GenerateSelectStarQuery(GetZoneTable(step), -1);

            // PK needs to be figured out from catalog table
            var clg = new SqlServerColumnListGenerator(tr, ColumnContext.PrimaryKey, ColumnListType.SelectWithEscapedNameNoAlias)
            {
                TableAlias = alias,
            };

            columnlist = clg.Execute();

            var slg = new SqlServerColumnListGenerator(tr, ColumnContext.PrimaryKey, ColumnListType.SelectWithEscapedNameNoAlias)
            {
                TableAlias = ""
            };

            selectlist = slg.Execute();
        }

        private void GenerateSourceQueryFromMatchTableForPopulatePairTable(XMatchQueryStep step, string alias, out string query, out string columnlist, out string selectlist)
        {
            // Use match table computed earlier for pair table building
            query = GenerateSelectStarQuery(GetMatchTable(step), -1);

            // PK in match table is always MatchID
            columnlist = String.Format("{0}.[MatchID] AS {1}", alias, GetEscapedMatchIDString(step.StepNumber));
            selectlist = GetEscapedMatchIDString(step.StepNumber);
        }

        #endregion
        #region Match table functions

        /// <summary>
        /// Generates the name of a temporary match table built
        /// from the pair table.
        /// </summary>
        /// <remarks>
        /// Will generate a name like user_jobid_partition_Match_step.
        /// This name is unique for the whole system.
        /// </remarks>
        /// <param name="stepNumber">Number of the XMatch step.</param>
        /// <returns>The escaped name of a temporary table.</returns>
        public Table GetMatchTable(XMatchQueryStep step)
        {
            return queryObject.GetTemporaryTable(String.Format("Match_{0}", step.StepNumber));
        }

        protected string GetMatchTableZoneIndexName(XMatchQueryStep step)
        {
            var indexname =  String.Format("IX_{0}_Zone", GetMatchTable(step).TableName);
            return QuoteIdentifier(indexname);
        }

        protected string GetEscapedMatchIDString(int stepNumber)
        {
            return String.Format("_Match_{0}_MatchID", stepNumber);
        }

        protected virtual string GetCreateMatchTableScript()
        {
            throw new NotImplementedException();
        }

        public SqlCommand GetCreateMatchTableCommand(XMatchQueryStep step, Table matchtable)
        {
            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings();
                AddSourceTableMappings(Partition.Parameters.SourceDatabaseVersionName, null);
            }

            var indexname = String.Format("[PK_{0}_{1}]", matchtable.SchemaName, matchtable.TableName);
            var columnlist = GenerateMatchTableColumns(step, ColumnListType.CreateTableWithEscapedName, false);

            var sql = new StringBuilder(GetCreateMatchTableScript());

            sql.Replace("[$idseed]", (((long)Partition.ID + 1L) * 1000000000000L).ToString());
            sql.Replace("[$tablename]", GetResolvedTableName(matchtable));
            sql.Replace("[$indexname]", GeneratePrimaryKeyName(matchtable));
            sql.Replace("[$columnlist]", columnlist);

            return new SqlCommand(sql.ToString());
        }

        protected string GenerateMatchTableColumns(XMatchQueryStep step, ColumnListType listType, bool useTableAlias)
        {
            // Add all primary keys and referenced columns from all source tables
            // that have been processed so far

            // TODO: check this when implementing drop-outs
            // there's no point in propagating anything other than PKs

            var columnlist = new StringBuilder();

            for (int i = 0; i <= step.StepNumber; i++)
            {
                if (Query.XMatchTables[Partition.Steps[i].XMatchTable].InclusionMethod != XMatchInclusionMethod.Drop)
                {
                    var tr = MapTableReference(Query.XMatchTables[Partition.Steps[i].XMatchTable].TableReference);
                    var columns = new SqlServerColumnListGenerator(tr, ColumnContext.Default, listType)
                    {
                        SeparatorRendering = ColumnListSeparatorRendering.Leading,
                        IdentityRendering = ColumnListIdentityRendering.Never,
                    };

                    if (useTableAlias)
                    {
                        if (i < step.StepNumber)
                        {
                            columns.TableAlias = "__t1";
                        }
                        else
                        {
                            columns.TableAlias = "__t2";
                        }
                    }
                    else
                    {
                        columns.TableAlias = "";
                    }

                    columnlist.AppendLine(columns.Execute());
                }
            }

            return columnlist.ToString();
        }

        protected virtual string GetPopulateMatchTableScript()
        {
            throw new NotImplementedException();
        }

        protected virtual void AppendPopulateMatchTableParameters(XMatchQueryStep step, SqlCommand cmd)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overriden in a derived class, populates a match table
        /// from a pair table and an older match table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public SqlCommand GetPopulateMatchTableCommand(XMatchQueryStep step, Table pairtable, Table matchtable)
        {
            if (step.StepNumber == 0)
            {
                throw new InvalidOperationException();
            }

            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings();
                AddSourceTableMappings(Partition.Parameters.SourceDatabaseVersionName, null);
            }

            var pstep = Partition.Steps[step.StepNumber - 1];
            var table1 = Partition.Query.XMatchTables[pstep.XMatchTable];
            var table2 = Partition.Query.XMatchTables[step.XMatchTable];
            var tr1 = MapTableReference(table1.TableReference);
            var tr2 = MapTableReference(table2.TableReference);

            // 1. Generate augmented table queries
            string query1, query2;

            if (step.StepNumber == 1)
            {
                // The first table is a source table. We don't apply conditions here, that
                // has been done at creating the pair table already
                var options = new AugmentedTableQueryOptions(pstep, table1.TableSource, table1.Region)
                {
                    ColumnContext = ColumnContext.Default | ColumnContext.PrimaryKey,
                    UseRegion = false,
                    UsePartitioning = false,
                    UseWhereConditions = false,
                };
                query1 = GenerateAugmentedTableQuery(options).ToString();
            }
            else
            {
                // The first table is a match table
                query1 = GenerateSelectStarQuery(GetMatchTable(pstep), -1);
            }

            // The second table is always the next source table
            var options2 = new AugmentedTableQueryOptions(step, table2.TableSource, table2.Region)
            {
                ColumnContext = ColumnContext.Default | ColumnContext.PrimaryKey,
                UseRegion = false,
                UsePartitioning = false,
                UseWhereConditions = false,
            };
            query2 = GenerateAugmentedTableQuery(options2).ToString();

            // 2. Generate column lists, both must include propagated primary keys and
            // other referenced columns (default)
            // these are all referenced by escaped names, so generated names are unique by default
            var columnlist1 = GenerateMatchTableColumns(step, ColumnListType.SelectWithEscapedNameNoAlias, true);
            var columnlist2 = GenerateMatchTableColumns(step, ColumnListType.SelectWithEscapedNameNoAlias, false);

            // 3. Generate join conditions between the pair table and source tables
            // on primary keys
            string tablejoin1;

            if (step.StepNumber == 1)
            {
                // The first table is a source table.
                var jlg1 = new SqlServerColumnListGenerator(tr1, ColumnContext.PrimaryKey, ColumnListType.JoinConditionWithEscapedName)
                {
                    TableAlias = "__t1",
                    JoinedTableAlias = "__pair",
                };
                tablejoin1 = jlg1.Execute();
            }
            else
            {
                // The first table is a match table
                tablejoin1 = "__t1.MatchID = __pair." + GetEscapedMatchIDString(pstep.StepNumber);
            }

            // The second table is always the next source table
            var jlg2 = new SqlServerColumnListGenerator(tr2, ColumnContext.PrimaryKey, ColumnListType.JoinConditionWithEscapedName)
            {
                TableAlias = "__t2",
                JoinedTableAlias = "__pair",
            };
            var tablejoin2 = jlg2.Execute();

            // Replace tokens in SQL script template

            var sql = new StringBuilder(GetPopulateMatchTableScript());

            sql.Replace("[$query1]", query1);
            sql.Replace("[$query2]", query2);
            sql.Replace("[$columnlist1]", columnlist1);
            sql.Replace("[$columnlist2]", columnlist2);
            sql.Replace("[$tablejoin1]", tablejoin1);
            sql.Replace("[$tablejoin2]", tablejoin2);
            sql.Replace("[$pairtable]", GetResolvedTableName(pairtable));
            sql.Replace("[$matchtable]", GetResolvedTableName(matchtable));

            var cmd = new SqlCommand(sql.ToString());

            AppendZoneParameters(cmd);
            AppendRegionParameter(cmd, table1.Region);
            AppendPopulateMatchTableParameters(step, cmd);

            return cmd;
        }

        protected virtual string GetBuildMatchTableIndexScript()
        {
            throw new NotImplementedException();
        }

        public SqlCommand GetBuildMatchTableIndexCommand(XMatchQueryStep step, Table matchtable)
        {
            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings();
                AddSourceTableMappings(Partition.Parameters.SourceDatabaseVersionName, null);
            }

            // The zone index has to contain the primary key columns of all previously
            // matched tables
            var columnlist = GenerateMatchTableColumns(step, ColumnListType.SelectWithEscapedNameNoAlias, false);

            StringBuilder sql = new StringBuilder(GetBuildMatchTableIndexScript());

            sql.Replace("[$indexname]", GetMatchTableZoneIndexName(step));
            sql.Replace("[$tablename]", GetResolvedTableName(matchtable));
            sql.Replace("[$columnlist]", columnlist);

            return new SqlCommand(sql.ToString());
        }


        #endregion
        #region Final query execution

        protected override SourceQuery OnGetExecuteQuery(QueryDetails query)
        {
            var xmss = query.ParsingTree.FindDescendantRecursive<XMatchSelectStatement>();
            var qs = (XMatchQuerySpecification)xmss.QueryExpression.EnumerateQuerySpecifications().First();

            ReplaceXMatchClause(qs);

            return base.OnGetExecuteQuery(query);
        }

        protected void ReplaceXMatchClause(XMatchQuerySpecification qs)
        {
            var xmts = qs.XMatchTableSource;
            var xmtstr = new List<TableReference>(xmts.EnumerateXMatchTableSpecifications().Select(ts => ts.TableReference));

            var matchtable = GetMatchTable(Partition.Steps[Partition.Steps.Count - 1]);
            var mtr = new TableReference(matchtable, "__match", false);

            SubstituteMatchTableName(qs, xmtstr, mtr);

            var nts = TableSource.Create(SimpleTableSource.Create(mtr));
            xmts.ExchangeWith(nts);
        }

        private void SubstituteMatchTableName(Jhu.Graywulf.Sql.Parsing.QuerySpecification qs, List<TableReference> xmtstr, TableReference matchtr)
        {
            var cg = new SqlServerColumnListGenerator();

            // Replace table references and substitute column names with escaped version
            foreach (var ci in qs.EnumerateDescendantsRecursive<ColumnIdentifier>(typeof(Subquery)))
            {
                var cr = ci.ColumnReference;

                if (cr.TableReference != null && cr.TableReference.IsComputed)
                {
                    // In case of a computed table (typically xmatch results table)
                    cr.TableReference = matchtr;
                }
                else if (xmtstr.Where(tri => tri.Compare(cr.TableReference)).FirstOrDefault() != null)
                {
                    // In case of other tables
                    var tr = MapTableReference(cr.TableReference);
                    cr.ColumnName = cg.EscapePropagatedColumnName(tr, cr.ColumnName);
                    cr.TableReference = matchtr;
                }
            }
        }

        #endregion
        #region Table statistics

        public override SqlCommand GetTableStatisticsCommand(ITableSource tableSource, DatasetBase statisticsDataset)
        {
            var cmd = base.GetTableStatisticsCommand(tableSource, statisticsDataset);

            AppendZoneParameters(cmd);

            return cmd;
        }

        #endregion
    }
}
