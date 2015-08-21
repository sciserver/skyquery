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
using Jhu.SkyQuery.Parser;
using Jhu.Spherical;

namespace Jhu.SkyQuery.Jobs.Query
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

        protected string GetWeightExpressionString(string sigmaExpression)
        {
            return String.Format(" 1 / POWER(CONVERT(float,{0}) / 3600 / 180 * PI(), 2) ", sigmaExpression);
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

        protected void AppendZoneHeightParameter(SqlCommand cmd)
        {
            cmd.Parameters.Add(zoneHeightParameterName, SqlDbType.Float).Value = Query.ZoneHeight;
        }

        #region Search radius functions

        public SqlCommand GetComputeMinMaxErrorCommand(XMatchQueryStep step)
        {
            var sql = new StringBuilder(XMatchScripts.ComputeMinMaxError);
            var table = Query.XMatchTables[step.XMatchTable];
            var coords = table.Coordinates;

            var where = GetTableSpecificWhereClause(table.TableSource);
            var sc = GetPartitioningConditions(coords.GetZoneIdExpression(Partition.CodeDataset));

            if (where != null && sc != null)
            {
                where.AppendCondition(sc, "AND");
            }
            else if (where == null)
            {
                where = Graywulf.SqlParser.WhereClause.Create(sc);
            }

            sql.Replace("[$tablename]", GetResolvedTableName(table.TableReference));
            sql.Replace("[$error]", Execute(coords.ErrorExpression));
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

            AppendZoneHeightParameter(cmd);
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
            var sc = GetPartitioningConditions(cr);

            if (sc != null)
            {
                var where = Graywulf.SqlParser.WhereClause.Create(sc);
                sql.Replace("[$where]", Execute(where));
            }
            else
            {
                sql.Replace("[$where]", String.Empty);
            }

            var cmd = new SqlCommand(sql.ToString());

            cmd.Parameters.Add("@H", SqlDbType.Float).Value = Query.ZoneHeight;
            cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = Partition.Steps[step.StepNumber].SearchRadius;
            AppendPartitioningConditionParameters(cmd);

            return cmd;
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
            var table = Query.XMatchTables[step.XMatchTable];

            var sql = new StringBuilder(XMatchScripts.CreateZoneTable);

            sql.Replace("[$tablename]", GetResolvedTableName(zonetable));
            sql.Replace("[$indexname]", GeneratePrimaryKeyName(zonetable));
            sql.Replace("[$columnlist]", GeneratePropagatedColumnList(table.TableSource, null, ColumnListInclude.PrimaryKey, ColumnListType.ForCreateTable, ColumnListNullType.NotNull, false));

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
            var table = Query.XMatchTables[step.XMatchTable];
            var coords = table.Coordinates;
            var qs = (XMatchQuerySpecification)table.FindAscendant<SkyQuery.Parser.QuerySpecification>();
            var region = qs.Region;
            var hasregion = coords.IsHtmIdSpecified && region != null;

            StringBuilder sql;

            // Tables in xmatch queries are always filtered by coordinates if the
            // query contains a REGION cluse
            if (hasregion)
            {
                sql = new StringBuilder(XMatchScripts.PopulateZoneTableRegion);
            }
            else
            {
                sql = new StringBuilder(XMatchScripts.PopulateZoneTable);
            }

            // Build where clauses
            var where = GetTableSpecificWhereClause(table.TableSource);
            var sc = GetPartitioningConditions(coords.GetZoneIdExpression(queryObject.CodeDataset));

            // TODO: add HTM join if necessary

            if (sc != null && where == null)
            {
                where = Jhu.Graywulf.SqlParser.WhereClause.Create(sc);
            }
            else if (sc != null)
            {
                where.AppendCondition(sc, "AND");
            }
            var partwhere = sc == null ? null : SkyQuery.Parser.WhereClause.Create(sc);

            sql.Replace("[$zonetablename]", GetResolvedTableName(zonetable));
            sql.Replace("[$tablename]", GetResolvedTableNameWithAlias(table.TableReference));
            sql.Replace("[$where]", Execute(where));
            sql.Replace("[$selectcolumnlist]", GeneratePropagatedColumnList(table.TableSource, null, ColumnListInclude.PrimaryKey, ColumnListType.ForSelectWithOriginalName, ColumnListNullType.Nothing, false));

            SubstituteCoordinates(sql, coords, hasregion);

            if (hasregion)
            {
                var htminner = GetHtmTable(step.StepNumber, false);
                var htmpartial = GetHtmTable(step.StepNumber, true);

                sql.Replace("[$htm_inner]", GetResolvedTableName(htminner));
                sql.Replace("[$htm_partial]", GetResolvedTableName(htmpartial));

                sql.Replace("[$where_inner]", Execute(where));
                sql.Replace("[$where_partial]", Execute(where));
            }
            else
            {
                sql.Replace("[$where]", Execute(where));
            }

            var cmd = new SqlCommand(sql.ToString());

            AppendPartitioningConditionParameters(cmd);
            AppendRegionParameter(cmd, region);
            AppendZoneHeightParameter(cmd);

            return cmd;
        }

        protected void SubstituteCoordinates(StringBuilder sql, TableCoordinates coords, bool hasRegion)
        {
            sql.Replace("[$ra]", coords.GetRAExpression(queryObject.CodeDataset).ToString());
            sql.Replace("[$dec]", coords.GetDecExpression(queryObject.CodeDataset).ToString());
            sql.Replace("[$cx]", coords.GetXExpression(queryObject.CodeDataset).ToString());
            sql.Replace("[$cy]", coords.GetYExpression(queryObject.CodeDataset).ToString());
            sql.Replace("[$cz]", coords.GetZExpression(queryObject.CodeDataset).ToString());

            // HTMID is required for region queries only
            if (hasRegion)
            {
                sql.Replace("[$htmid]", coords.GetHtmIdExpression().ToString());
            }
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
        public SqlCommand GetCreatePairTableCommand(XMatchQueryStep step, XMatchTableSpecification table, Table pairtable)
        {
            var sql = new StringBuilder(XMatchScripts.CreatePairTable);

            sql.Replace("[$tablename]", GetResolvedTableName(pairtable));
            sql.Replace("[$createcolumnlist1]", String.Format("PK_Match_{0}_MatchID [bigint] NOT NULL", step.StepNumber - 1));
            sql.Replace("[$createcolumnlist2]", GeneratePropagatedColumnList(table.TableSource, null, ColumnListInclude.PrimaryKey, ColumnListType.ForCreateTable, ColumnListNullType.NotNull, false));

            return new SqlCommand(sql.ToString());
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
        public SqlCommand GetPopulatePairTableCommand(XMatchQueryStep step, XMatchTableSpecification table, Table pairtable)
        {
            // Table 1 is, with the exception of the very first step, the match table of a previous xmatch step.
            // In this case the match table contains all necessary columns and includes a zone index.
            // In the very first step Table 1 can either be a catalog table with a zone index that contains all
            // necessary columns, or a zone table built during the xmatch workflow.
            // Similarly, Table 2 is either a catalog table with the right index or a dynamically built zone table.
            // If we use an existing match table or a zone table, we know the column names. Otherwise it's a bit
            // tricky to get the list of columns

            var linktable = GetLinkTable(step);

            var step1 = Partition.Steps[step.StepNumber - 1];
            var step2 = step;

            var table1 = Query.XMatchTables[step1.XMatchTable];
            var table2 = Query.XMatchTables[step2.XMatchTable];

            StringBuilder sql = new StringBuilder(XMatchScripts.PopulatePairTable);

            sql.Replace("[$pairtable]", GetResolvedTableName(pairtable));
            sql.Replace("[$linktable]", GetResolvedTableName(linktable));

            if (step.StepNumber == 1 && !table1.IsZoneTableNecessary)
            {
                // Use catalog table for Table 1
                throw new NotImplementedException();
            }
            else if (step.StepNumber == 1)
            {
                // Use zone table for Table 1
                // PK needs to be figured out from catalog table
                sql.Replace("[$query1]", "SELECT * FROM " + GetResolvedTableName(GetZoneTable(step1)));
                sql.Replace("[$columnlist1]", GeneratePropagatedColumnList(table1.TableSource, "__t1", ColumnListInclude.PrimaryKey, ColumnListType.ForSelectNoAlias, ColumnListNullType.Nothing, false));
                sql.Replace("[$selectlist1]", GeneratePropagatedColumnList(table1.TableSource, null, ColumnListInclude.PrimaryKey, ColumnListType.ForSelectNoAlias, ColumnListNullType.Nothing, false));
            }
            else
            {
                // Use match table for Table 1
                // PK in match table is always MatchID
                sql.Replace("[$query1]", "SELECT * FROM " + GetResolvedTableName(GetMatchTable(step1)));
                sql.Replace("[$columnlist1]", "__t1.[MatchID]");
                sql.Replace("[$selectlist1]", "__t1_MatchID");
            }

            if (!table2.IsZoneTableNecessary)
            {
                // Use catalog table for Table 2
                throw new NotImplementedException();
            }
            else
            {
                // Use zone table for Table 2
                // PK needs to be figured out from catalog table
                sql.Replace("[$query2]", "SELECT * FROM " + GetResolvedTableName(GetZoneTable(step2)));
                sql.Replace("[$columnlist2]", GeneratePropagatedColumnList(table2.TableSource, "__t2", ColumnListInclude.PrimaryKey, ColumnListType.ForSelectNoAlias, ColumnListNullType.Nothing, false));
                sql.Replace("[$selectlist2]", GeneratePropagatedColumnList(table2.TableSource, null, ColumnListInclude.PrimaryKey, ColumnListType.ForSelectNoAlias, ColumnListNullType.Nothing, false));
            }

            var cmd = new SqlCommand(sql.ToString());

            cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = step.SearchRadius;

            return cmd;
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
            return String.Format("IX_{0}_Zone", GetMatchTable(step).TableName);
        }

        protected virtual string GetCreateMatchTableScript(XMatchQueryStep step)
        {
            throw new NotImplementedException();
        }

        public SqlCommand GetCreateMatchTableCommand(XMatchQueryStep step, Table matchtable)
        {
            var indexname = String.Format("[PK_{0}_{1}]", matchtable.SchemaName, matchtable.TableName);

            var sql = new StringBuilder(GetCreateMatchTableScript(step));

            sql.Replace("[$tablename]", GetResolvedTableName(matchtable));
            sql.Replace("[$indexname]", GeneratePrimaryKeyName(matchtable));
            sql.Replace("[$columnlist]", GetCreateMatchTableColumns(step));

            return new SqlCommand(sql.ToString());

        }

        private string GetCreateMatchTableColumns(XMatchQueryStep step)
        {
            // TODO: need to propagate primary key when lazy columns are being implemented

            // Add all propagated columns
            var columnlist = new StringBuilder();

            for (int i = 0; i <= step.StepNumber; i++)
            {
                // TODO: check this when implementing drop-outs

                if (Query.XMatchTables[Partition.Steps[i].XMatchTable].InclusionMethod != XMatchInclusionMethod.Drop)
                {
                    var columns = GeneratePropagatedColumnList(
                        Query.XMatchTables[Partition.Steps[i].XMatchTable].TableSource,
                        null,
                        ColumnListInclude.Referenced,
                        ColumnListType.ForCreateTable,
                        ColumnListNullType.NotNull,
                        true);

                    columnlist.AppendLine(columns);
                }
            }

            return columnlist.ToString();
        }

        protected virtual string GetPopulateMatchTableScript(XMatchQueryStep step)
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
        public SqlCommand GetPopulateMatchTableCommand(XMatchQueryStep step, Table matchtable)
        {
            if (step.StepNumber == 0)
            {
                throw new InvalidOperationException();
            }

            var table = Query.XMatchTables[step.XMatchTable];

            var tr = new TableReference(table.TableReference);
            SubstituteRemoteTableName(tr);

            var tablename = GetResolvedTableName(tr);
            var schemaname = Partition.TemporaryDataset.DefaultSchemaName;

            var include = ColumnListInclude.All;

            // --- Propagated columns
            // Gather all columns from previous steps
            var insertcolumnlist = new StringBuilder();
            var selectcolumnlist = new StringBuilder();
            for (int i = 0; i <= step.StepNumber; i++)
            {
                if (Query.XMatchTables[Partition.Steps[i].XMatchTable].InclusionMethod != XMatchInclusionMethod.Drop)
                {
                    if (insertcolumnlist.Length != 0)
                    {
                        insertcolumnlist.Append(", ");
                        selectcolumnlist.Append(", ");
                    }

                    var tablealias = (i < step.StepNumber) ? "tableA" : "tableB";
                    var listtype = (i < step.StepNumber) ? ColumnListType.ForSelectNoAlias : ColumnListType.ForSelectWithOriginalName;

                    // ForSelectNoalias -> ForInsert
                    insertcolumnlist.Append(GeneratePropagatedColumnList(Query.XMatchTables[Partition.Steps[i].XMatchTable].TableSource, null, include, ColumnListType.ForSelectNoAlias, ColumnListNullType.Nothing, false));
                    selectcolumnlist.Append(GeneratePropagatedColumnList(Query.XMatchTables[Partition.Steps[i].XMatchTable].TableSource, tablealias, include, listtype, ColumnListNullType.Nothing, false));
                }
            }

            // --- Zone table join conditions
            var join = new StringBuilder();
            var t = (TableOrView)Query.XMatchTables[step.XMatchTable].TableReference.DatabaseObject;

            foreach (var c in t.PrimaryKey.Columns.Values)
            {
                join.AppendLine(String.Format(
                    "[tableB].[{1}] = [pairtable].[{0}]",
                    EscapePropagatedColumnName(Query.XMatchTables[step.XMatchTable].TableReference, c.Name),
                    c.Name));
            }


            var sql = new StringBuilder(GetPopulateMatchTableScript(step));


            sql.Replace("[$newtablename]", GetResolvedTableName(matchtable));     // new match table
            sql.Replace("[$insertcolumnlist]", insertcolumnlist.ToString());
            sql.Replace("[$selectcolumnlist]", selectcolumnlist.ToString());
            sql.Replace("[$selectcolumnlist2]", insertcolumnlist.ToString());
            sql.Replace("[$pairtable]", GetResolvedTableName(GetPairTable(step)));
            sql.Replace("[$matchtable]", GetResolvedTableName((GetMatchTable(Partition.Steps[step.StepNumber - 1]))));        // tableA (old match table)
            sql.Replace("[$matchidcolumn]", String.Format("PK_Match_{0}_MatchID", step.StepNumber - 1));    // TODO
            sql.Replace("[$table]", tablename);        // tableB (source table)
            sql.Replace("[$tablejoinconditions]", join.ToString());

            var cmd = new SqlCommand(sql.ToString());

            AppendZoneHeightParameter(cmd);
            AppendPopulateMatchTableParameters(step, cmd);

            return cmd;
        }

        /// <summary>
        /// When overriden in a derived class, populates a match table from
        /// an original dataset (i.e. no real matching occures)
        /// </summary>
        /// <param name="table">Reference to the source table.</param>
        /// <param name="stepNumber">Number of XMatch step.</param>
        /// <remarks>
        /// This function is called only in the first iteration.
        /// </remarks>
        private SqlCommand GetPopulateInitialMatchTableCommand(XMatchQueryStep step)
        {
            // TODO: delete
            /*
            var table = xmatchTables[step.XMatchTable];
            var coords = table.Coordinates;

            var include = ColumnListInclude.All;

            using (SqlCommand cmd = new SqlCommand())
            {
                var sql = new StringBuilder(GetPopulateInitialMatchTableScript(step, cmd));
                var where = GetPartitioningKeyWhereClause(step);

                sql.Replace("[$newtablename]", CodeGenerator.GetResolvedTableName(GetMatchTable(step.StepNumber)));
                sql.Replace("[$tablename]", SubstituteRemoteTableNameWithAlias(table.TableReference));
                sql.Replace("[$insertcolumnlist]", GetPropagatedColumnList(table, ColumnListType.ForInsert, include, ColumnListNullType.Nothing, null));
                sql.Replace("[$selectcolumnlist]", GetPropagatedColumnList(table, ColumnListType.ForSelectWithOriginalName, include, ColumnListNullType.Nothing, table.TableReference.Alias));

                SubstituteCoordinates(sql, coords);

                // No buffering when initial partitioning is done
                if (Region != null)
                {
                    var htminner = GetHtmTable(step.StepNumber, false);
                    var htmpartial = GetHtmTable(step.StepNumber, true);

                    sql.Replace("[$htm_inner]", CodeGenerator.GetResolvedTableName(htminner));
                    sql.Replace("[$htm_partial]", CodeGenerator.GetResolvedTableName(htmpartial));
                    sql.Replace("[$where_inner]", where);
                    sql.Replace("[$where_partial]", where);
                }
                else
                {
                    sql.Replace("[$where]", where);
                }

                AppendPartitioningConditionParameters(cmd, 0);
                AppendRegionParameter(cmd);
                cmd.Parameters.Add("@H", SqlDbType.Float).Value = ((XMatchQuery)Query).ZoneHeight;

                cmd.CommandText = sql.ToString();

                ExecuteSqlCommand(cmd, CommandTarget.Code);
            }
             * */

            return null;
        }

        protected virtual string GetBuildMatchTableIndexScript(XMatchTableSpecification table, int stepNumber)
        {
            throw new NotImplementedException();
        }

        public SqlCommand GetBuildInitialMatchTableIndexCommand(XMatchQueryStep step, Table matchtable)
        {
            /*
            var indexname = GetMatchTableZoneIndexName(stepNumber);

            StringBuilder sql = new StringBuilder(GetBuildMatchTableIndexScript(table, stepNumber));

            sql.Replace("[$indexname]", indexname);
            sql.Replace("[$tablename]", matchtable.TableName);
            sql.Replace("[$columnlist]", GetPropagatedColumnList(table, ColumnListType.ForInsert, ColumnListInclude.PrimaryKey, ColumnListNullType.Nothing, null));

            ExecuteSqlCommand(sql.ToString(), CommandTarget.Temp);
             * */

            return null;
        }

        #endregion
        #region Final query execution

        protected override SourceTableQuery GetExecuteQueryImpl(Graywulf.SqlParser.SelectStatement selectStatement, CommandMethod method, Table destination)
        {
            // Collect tables that are part of the XMatch operation
            var qs = (XMatchQuerySpecification)selectStatement.EnumerateQuerySpecifications().First();

            ReplaceXMatchClause(qs);

            return base.GetExecuteQueryImpl(selectStatement, method, destination);
        }

        protected void ReplaceXMatchClause(XMatchQuerySpecification qs)
        {
            var xmts = qs.XMatchTableSource;
            var xmtstr = new List<TableReference>(xmts.EnumerateXMatchTableSpecifications().Select(ts => ts.TableReference));
            var matchtable = GetMatchTable(Partition.Steps[Partition.Steps.Count - 1]);

            SubstituteEscapedColumnNames(qs, xmtstr);
            SubstituteMatchTableName(qs, xmtstr);

            // Create match table expression tree
            var mtr = new TableReference(matchtable, "matchtable");

            var nts = Jhu.Graywulf.SqlParser.SimpleTableSource.Create(mtr);

            xmts.ExchangeWith(nts);
        }

        private void SubstituteMatchTableName(Jhu.Graywulf.SqlParser.QuerySpecification qs, List<TableReference> xmtstr)
        {
            // Replace table references
            // This must be a separate step to save original table aliases in escaped names
            // Itt minden oszlop kell, nem csak, ami a kimeneten van

            foreach (var ci in qs.EnumerateDescendantsRecursive<ColumnIdentifier>(typeof(Jhu.Graywulf.SqlParser.Subquery)))
            {
                var cr = ci.ColumnReference;

                if (cr.TableReference != null && cr.TableReference.IsComputed)
                {
                    // In case of a computed table (typically xmatch results table)
                    cr.TableReference.Alias = "matchtable";
                }
                else if (xmtstr.Where(tri => tri.Compare(cr.TableReference)).FirstOrDefault() != null)
                {
                    // In case of other tables
                    // See if it's an xmatched table or not
                    cr.TableReference.Alias = "matchtable";
                }
            }
        }

        private void SubstituteEscapedColumnNames(Jhu.Graywulf.SqlParser.QuerySpecification qs, List<TableReference> xmtstr)
        {
            // Replace column references to point to match table
            // also, change column names to the escaped names
            foreach (var ci in qs.EnumerateDescendantsRecursive<ColumnIdentifier>(typeof(Jhu.Graywulf.SqlParser.Subquery)))
            {
                var cr = ci.ColumnReference;

                if (xmtstr.Where(tri => tri.Compare(cr.TableReference)).FirstOrDefault() != null)
                {
                    cr.ColumnName = EscapePropagatedColumnName(cr.TableReference, cr.ColumnName);
                }
            }
        }

        #endregion
        #region Table statistics

        public override SqlCommand GetTableStatisticsCommand(ITableSource tableSource)
        {
            var cmd = base.GetTableStatisticsCommand(tableSource);

            AppendZoneHeightParameter(cmd);

            return cmd;
        }

        #endregion
    }
}
