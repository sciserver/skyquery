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
    public abstract class XMatchQueryCodeGenerator : RegionQueryCodeGenerator
    {
        protected const string zoneHeightParameterName = "@H";

        private XMatchQueryPartition Partition
        {
            get { return queryObject as XMatchQueryPartition; }
        }

        #region Constructors and initializers

        public XMatchQueryCodeGenerator()
        {
        }

        public XMatchQueryCodeGenerator(QueryObject queryObject)
            : base(queryObject)
        {
        }

        #endregion
        #region Basic query rewrite functions

        public override SourceTableQuery GetExecuteQuery(Graywulf.SqlParser.SelectStatement selectStatement, CommandMethod method, Table destination)
        {
            /*
            // **** TODO: this disrupts the select statement
            // a copy could be made of the entire parsing tree
            // but reparsing is easier so it will be reset at the end

            // Collect tables that are part of the XMatch operation
            var qs = (XMatchQuerySpecification)SelectStatement.EnumerateQuerySpecifications().First();
            var xm = qs.XMatchTableSource;
            var xmtstr = new List<TableReference>(xm.EnumerateXMatchTableSpecifications().Select(ts => ts.TableReference));
            var matchtable = GetMatchTable(steps.Count - 1);

            SubstituteEscapedColumnNames(qs, xmtstr);
            SubstituteMatchTableName(qs, xmtstr);

            // Create match table expression tree

            var nts = new Jhu.Graywulf.SqlParser.ComputedTableSource();
            nts.TableReference = new TableReference();
            nts.TableReference.DatabaseName = matchtable.DatabaseName;
            nts.TableReference.SchemaName = matchtable.SchemaName;
            nts.TableReference.DatabaseObjectName = matchtable.TableName;
            nts.TableReference.Alias = "matchtable";                            // *** TODO use constant

            nts.Stack.AddLast(TableOrViewName.Create(nts.TableReference));
            nts.Stack.AddLast(Whitespace.Create());
            nts.Stack.AddLast(Jhu.Graywulf.ParserLib.Keyword.Create("AS"));
            nts.Stack.AddLast(Whitespace.Create());
            nts.Stack.AddLast(TableAlias.Create("matchtable"));                 // *** TODO use constant

            // Replace XMATCH part with match table
            xm.Parent.Stack.AddBefore(
                xm.Parent.Stack.Find(xm),
                Jhu.Graywulf.SqlParser.TableSource.Create(nts));
            xm.Parent.Stack.Remove(xm);

            // Remove REGION clause

            var r = qs.FindDescendantRecursive<RegionClause>();

            if (r != null)
            {
                r.Parent.Stack.Remove(r);
            }

            var code = SqlServerCodeGenerator.GetCode(SelectStatement, true);

            // Now zero out the selectStatement to force reparsing
            SelectStatement = null;

            return code;
             * */

            return null;
        }

        #endregion
        #region Column propagator functions

        /// <summary>
        /// Returns a SQL snippet with the list of primary keys
        /// and propagated columns belonging to the table.
        /// </summary>
        /// <param name="table">Reference to the table.</param>
        /// <param name="type">Column list type.</param>
        /// <param name="nullType">Column nullable type.</param>
        /// <param name="tableAlias">Optional table alias prefix, specify null to omit.</param>
        /// <returns>A SQL snippet with the list of columns.</returns>
        public string GetPropagatedColumnList(XMatchTableSpecification table, ColumnListType type, ColumnListInclude include, ColumnListNullType nullType, string tableAlias)
        {
            /*
            // ---
            string nullstring = null;

            switch (nullType)
            {
                case ColumnListNullType.Nothing:
                    nullstring = String.Empty;
                    break;
                case ColumnListNullType.Null:
                    nullstring = "NULL";
                    break;
                case ColumnListNullType.NotNull:
                    nullstring = "NOT NULL";
                    break;
                default:
                    throw new NotImplementedException();
            }

            // ---
            string format = null;

            switch (type)
            {
                case ColumnListType.ForCreateTable:
                    format = "[{1}] {3} {4}";
                    break;
                case ColumnListType.ForCreateView:
                case ColumnListType.ForInsert:
                    format = "[{1}]";
                    break;
                case ColumnListType.ForSelectWithOriginalName:
                    format = "{0}[{2}] AS [{1}]";
                    break;
                case ColumnListType.ForSelectWithEscapedName:
                    format = "{0}[{1}] AS [{1}]";
                    break;
                case ColumnListType.ForSelectNoAlias:
                    format = "{0}[{1}]";
                    break;
                default:
                    throw new NotImplementedException();
            }

            StringBuilder columnlist = new StringBuilder();
            HashSet<string> referencedcolumns = new HashSet<string>(SchemaManager.Comparer);

            // Add primary key columns
            if ((include & ColumnListInclude.PrimaryKey) != 0)
            {
                var t = (TableOrView)table.TableReference.DatabaseObject;

                foreach (Column cd in t.PrimaryKey.Columns.Values)
                {
                    if (columnlist.Length != 0)
                    {
                        columnlist.Append(", ");
                    }

                    columnlist.AppendFormat(format,
                                            tableAlias == null ? String.Empty : String.Format("[{0}].", tableAlias),
                                            GetEscapedPropagatedColumnName(table.TableReference, cd.Name),
                                            cd.Name,
                                            cd.DataType.NameWithLength,
                                            nullstring);

                    referencedcolumns.Add(GetEscapedColumnName(table.TableReference, cd.Name));
                }
            }

            var tr = SelectStatement.EnumerateQuerySpecifications().First().EnumerateSourceTableReferences(false).ToArray();


            if ((include & ColumnListInclude.Referenced) != 0)
            {
                foreach (ColumnReference cr in table.TableReference.ColumnReferences)
                {
                    if (cr.IsReferenced)
                    {
                        string key = GetEscapedColumnName(table.TableReference, cr.ColumnName);
                        string escapedname = GetEscapedPropagatedColumnName(table.TableReference, cr.ColumnName);

                        if (!referencedcolumns.Contains(key))
                        {
                            if (columnlist.Length != 0)
                            {
                                columnlist.Append(", ");
                            }

                            columnlist.AppendFormat(format,
                                                tableAlias == null ? String.Empty : String.Format("[{0}].", tableAlias),
                                                escapedname,
                                                cr.ColumnName,
                                                cr.DataType.NameWithLength,
                                                nullstring);

                            referencedcolumns.Add(key);
                        }
                    }
                }
            }

            return columnlist.ToString();
             * */

            throw new NotImplementedException();
        }

        #endregion
        #region ZoneDef table function

        public Table GetZoneDefTable(int stepNumber)
        {
            return queryObject.GetTemporaryTable(String.Format("ZoneDef_{0}", stepNumber));
        }

        public SqlCommand GetCreateZoneDefTableCommand(XMatchQueryStep step, Table zonedeftable)
        {
            var sql = new StringBuilder(XMatchScripts.CreateZoneDefTable);

            sql.Replace("[$tablename]", GetResolvedTableName(zonedeftable));
            sql.Replace("[$indexname]", String.Format("[IXC_{0}_{1}]", zonedeftable.SchemaName, zonedeftable.TableName));

            return new SqlCommand(sql.ToString());
        }

        public SqlCommand GetPopulateZoneDefTableCommand(XMatchQueryStep step, Table zonedeftable)
        {
            // TODO: fix this, because zone def table should contain buffer zones when the
            // query is partitioned

            var sql = new StringBuilder(XMatchScripts.PopulateZoneDefTable);

            sql.Replace("[$tablename]", GetResolvedTableName(zonedeftable));

            var cmd = new SqlCommand(sql.ToString());
            AppendZoneHeightParameter(cmd);
            AppendPartitioningConditionParameters(cmd);
            cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = step.SearchRadius;

            return cmd;
        }

        #endregion
        #region Zone table functions

        /// <summary>
        /// Generates the name of a temporary zone table built
        /// from a source table.
        /// </summary>
        /// <remarks>
        /// Will generate a name like user_jobid_partition_Zone_DB_schema_table.
        /// This name is unique for the whole system.
        /// </remarks>
        /// <param name="table">Reference to the source table</param>
        /// <returns>The escaped name of a temporary table.</returns>
        public Table GetZoneTable(TableReference table)
        {
            return queryObject.GetTemporaryTable(String.Format("Zone_{0}_{1}_{2}_{3}",
                                                       table.DatasetName,
                                                       table.SchemaName,
                                                       table.DatabaseObjectName,
                                                       table.Alias));
        }

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
        public Table GetZoneTable(int stepNumber)
        {
            return queryObject.GetTemporaryTable(String.Format("Zone_Match_{0}", stepNumber));
        }

        protected abstract string GetCreateZoneTableScript();

        public SqlCommand GetCreateZoneTableCommand(XMatchTableSpecification table, Table zonetable)
        {
            var sql = new StringBuilder(GetCreateZoneTableScript());

            sql.Replace("[$tablename]", GetResolvedTableName(zonetable));
            sql.Replace("[$indexname]", String.Format("[IXC_{0}_{1}]", zonetable.SchemaName, zonetable.TableName));
            sql.Replace("[$columnlist]", GetPropagatedColumnList(table, ColumnListType.ForCreateTable, ColumnListInclude.PrimaryKey, ColumnListNullType.NotNull, null));

            return new SqlCommand(sql.ToString());
        }

        protected abstract string GetPopulateZoneTableScript(XMatchTableSpecification table);

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
        public SqlCommand GetPopulateZoneTableCommand(XMatchTableSpecification table, Table zonedeftable, Table zonetable)
        {
            /*var coords = table.Coordinates;

            var sql = new StringBuilder(GetPopulateZoneTableScript(table));
            
            // Build where clause
            var where = GetTableSpecificWhereClause(table.TableSource);
            var sc = GetPartitioningConditions(coords.GetZoneIdExpression(queryObject.CodeDataset));

            if (where != null && sc != null)
            {
                where.AppendCondition(sc, "AND");
            }
            else if (sc != null)
            {
                where = SkyQuery.Parser.WhereClause.Create(sc);
            }

            sql.Replace("[$zonetablename]", GetResolvedTableName(zonetable));
            sql.Replace("[$zonedeftable]", GetResolvedTableName(zonedeftable));
            sql.Replace("[$tablename]", SubstituteRemoteTableNameWithAlias(table.TableReference));
            sql.Replace("[$insertcolumnlist]", GetPropagatedColumnList(table, ColumnListType.ForInsert, ColumnListInclude.PrimaryKey, ColumnListNullType.Nothing, null));
            sql.Replace("[$selectcolumnlist]", GetPropagatedColumnList(table, ColumnListType.ForSelectWithOriginalName, ColumnListInclude.PrimaryKey, ColumnListNullType.Nothing, null));

            SubstituteCoordinates(sql, coords);

            if (Region != null)
            {
                var htminner = GetHtmTable(step.StepNumber, false);
                var htmpartial = GetHtmTable(step.StepNumber, true);

                sql.Replace("[$htm_inner]", GetResolvedTableName(htminner));
                sql.Replace("[$htm_partial]", GetResolvedTableName(htmpartial));

                sql.Replace("[$where_inner]", where  != null ? where.ToString() : "");
                sql.Replace("[$where_partial]", where != null ? where.ToString() : "");
            }
            else
            {
                sql.Replace("[$where]", where);
            }

            var cmd = new SqlCommand(sql.ToString());

            AppendPartitioningConditionParameters(cmd);
            AppendRegionParameter(cmd);
            AppendZoneHeightParameter(cmd);

            return cmd;*/

            return null;
        }

        protected void SubstituteCoordinates(StringBuilder sql, TableCoordinates coords)
        {
            /*
            sql.Replace("[$ra]", coords.GetRAExpression(queryObject.CodeDataset).ToString());
            sql.Replace("[$dec]", coords.GetDecExpression(queryObject.CodeDataset).ToString());
            sql.Replace("[$cx]", coords.GetXExpression(queryObject.CodeDataset).ToString());
            sql.Replace("[$cy]", coords.GetYExpression(queryObject.CodeDataset).ToString());
            sql.Replace("[$cz]", coords.GetZExpression(queryObject.CodeDataset).ToString());

            // HTMID is required for region queries only
            if (Region != null)
            {
                sql.Replace("[$htmid]", coords.GetHtmIdExpression().ToString());
            }
             * */
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
        public Table GetLinkTable(int stepNumber)
        {
            return queryObject.GetTemporaryTable(String.Format("Link_{0}", stepNumber));
        }

        /// <summary>
        /// Creates a link table between two zone tables.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public SqlCommand GetCreateLinkTableCommand(Table linktable)
        {
            var sql = new StringBuilder(XMatchScripts.CreateLinkTable);

            sql.Replace("[$tablename]", GetResolvedTableName(linktable));
            sql.Replace("[$indexname]", String.Format("PK_{0}", linktable.TableName));

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
        public abstract SqlCommand GetPopulateLinkTableCommand();

        #endregion
        #region Pair table

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
        public Table GetPairTable(int stepNumber)
        {
            return queryObject.GetTemporaryTable(String.Format("Pair_{0}", stepNumber));
        }

        /// <summary>
        /// Creates a pair table from a link table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public SqlCommand GetCreatePairTableCommand(XMatchTableSpecification table, int stepNumber, Table pairtable)
        {
            var sql = new StringBuilder(XMatchScripts.CreatePairTable);

            sql.Replace("[$tablename]", GetResolvedTableName(pairtable));
            sql.Replace("[$createcolumnlist1]", String.Format("PK_Match_{0}_MatchID [bigint] NOT NULL", stepNumber - 1));
            sql.Replace("[$createcolumnlist2]", GetPropagatedColumnList(table, ColumnListType.ForCreateTable, ColumnListInclude.PrimaryKey, ColumnListNullType.NotNull, null));

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
        public abstract SqlCommand GetPopulatePairTableCommand(Table pairtable);

        #endregion
        #region Match table

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
        public Table GetMatchTable(int stepNumber)
        {
            return queryObject.GetTemporaryTable(String.Format("Match_{0}", stepNumber));
        }

        protected abstract string GetCreateMatchTableScript(XMatchQueryStep step);

        public SqlCommand GetCreateMatchTableCommand(XMatchQueryStep step, Table matchtable)
        {
            // Create real match tables
            var indexname = String.Format("[IXC_{0}_{1}]", matchtable.SchemaName, matchtable.TableName);

            var include = ColumnListInclude.All;

            // Add all propagated columns
            var columnlist = new StringBuilder();
            for (int i = 0; i <= step.StepNumber; i++) 
            {
                if (Partition.Query.XMatchTables[Partition.Steps[i].XMatchTable].InclusionMethod != XMatchInclusionMethod.Drop)
                {
                    if (columnlist.Length != 0)
                    {
                        columnlist.Append(", ");
                    }
                    columnlist.AppendLine(GetPropagatedColumnList(Partition.Query.XMatchTables[Partition.Steps[i].XMatchTable], ColumnListType.ForCreateTable, include, ColumnListNullType.NotNull, null));
                }
            }
            
            var sql = new StringBuilder(GetCreateMatchTableScript(step));

            sql.Replace("[$tablename]", matchtable.TableName);
            sql.Replace("[$indexname]", indexname);
            sql.Replace("[$columnlist]", columnlist.ToString());

            return new SqlCommand(sql.ToString());
            
        }

        protected abstract string GetPopulateMatchTableScript(XMatchQueryStep step, SqlCommand cmd);

        /// <summary>
        /// When overriden in a derived class, populates a match table
        /// from a pair table and an older match table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public SqlCommand GetPopulateMatchTableCommand(XMatchQueryStep step, Table matchtable)
        {
            if (step.StepNumber == 0)
            {
                return GetPopulateInitialMatchTableCommand(step);
            }

            /*
            var table = xmatchTables[step.XMatchTable];

            var tablename = SubstituteRemoteTableName(table.TableReference);
            var newtablename = GetMatchTable(step.StepNumber);
            var schemaname = Query.TemporaryDataset.DefaultSchemaName;

            var include = ColumnListInclude.All;

            // --- Propagated columns
            // Gather all columns from previous steps
            var insertcolumnlist = new StringBuilder();
            var selectcolumnlist = new StringBuilder();
            for (int i = 0; i <= step.StepNumber; i++)
            {
                if (xmatchTables[steps[i].XMatchTable].InclusionMethod != XMatchInclusionMethod.Drop)
                {
                    if (insertcolumnlist.Length != 0)
                    {
                        insertcolumnlist.Append(", ");
                        selectcolumnlist.Append(", ");
                    }

                    var tablealias = (i < step.StepNumber) ? "tableA" : "tableB";
                    var listtype = (i < step.StepNumber) ? ColumnListType.ForSelectNoAlias : ColumnListType.ForSelectWithOriginalName;

                    // ForSelectNoalias -> ForInsert
                    insertcolumnlist.Append(GetPropagatedColumnList(xmatchTables[steps[i].XMatchTable], ColumnListType.ForSelectNoAlias, include, ColumnListNullType.Nothing, null));
                    selectcolumnlist.Append(GetPropagatedColumnList(xmatchTables[steps[i].XMatchTable], listtype, include, ColumnListNullType.Nothing, tablealias));
                }
            }

            // --- Zone table join conditions
            var join = new StringBuilder();
            var t = (TableOrView)xmatchTables[step.XMatchTable].TableReference.DatabaseObject;

            foreach (var c in t.PrimaryKey.Columns.Values)
            {
                join.AppendLine(String.Format("[tableB].[{1}] = [pairtable].[{0}]",
                                              GetEscapedPropagatedColumnName(xmatchTables[step.XMatchTable].TableReference, c.Name),
                                              c.Name));
            }

            //

            using (var cmd = new SqlCommand())
            {
                var sql = new StringBuilder(GetPopulateMatchTableScript(step, cmd));

                sql.Replace("[$newtablename]", CodeGenerator.GetResolvedTableName(newtablename));     // new match table
                sql.Replace("[$insertcolumnlist]", insertcolumnlist.ToString());
                sql.Replace("[$selectcolumnlist]", selectcolumnlist.ToString());
                sql.Replace("[$selectcolumnlist2]", insertcolumnlist.ToString());
                sql.Replace("[$pairtable]", CodeGenerator.GetResolvedTableName(GetPairTable(step.StepNumber)));
                sql.Replace("[$matchtable]", CodeGenerator.GetResolvedTableName((GetMatchTable(step.StepNumber - 1))));        // tableA (old match table)
                sql.Replace("[$matchidcolumn]", String.Format("PK_Match_{0}_MatchID", step.StepNumber - 1));
                sql.Replace("[$table]", tablename);        // tableB (source table)
                sql.Replace("[$tablejoinconditions]", join.ToString());

                cmd.CommandText = sql.ToString();

                cmd.Parameters.Add("@H", SqlDbType.Float).Value = ((XMatchQuery)Query).ZoneHeight;

                ExecuteSqlCommand(cmd, CommandTarget.Code);
            }
             * */

            return null;
        }

        protected abstract string GetPopulateInitialMatchTableScript(XMatchQueryStep step, SqlCommand cmd);

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

        protected abstract string GetBuildMatchTableIndexScript(XMatchTableSpecification table, int stepNumber);

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
        #region Column name escaping

        /// <summary>
        /// Generates and escaped name for a column that should be
        /// propagated.
        /// </summary>
        /// <remarks>
        /// Will generate a name like DS_schema_table_column that
        /// is unique in a table.
        /// </remarks>
        /// <param name="table">Reference to the source table.</param>
        /// <param name="column">Reference to the column.</param>
        /// <returns>The excaped name of the temporary table column.</returns>
        protected string GetEscapedColumnName(TableReference table, string columnName)
        {
            return String.Format("{0}_{1}_{2}_{3}_{4}",
                                 table.DatasetName,
                                 table.SchemaName,
                                 table.DatabaseObjectName,
                                 table.Alias,
                                 columnName);
        }


        protected string GetEscapedPropagatedColumnName(TableReference table, string columnName)
        {
            return String.Format("_{0}", GetEscapedColumnName(table, columnName));
        }

        #endregion
        #region Temporary table name generator functions
        

        protected string GetMatchTableZoneIndexName(int stepNumber)
        {
            return String.Format("IX_{0}_Zone", GetMatchTable(stepNumber).TableName);
        }

        #endregion

        public void AppendZoneHeightParameter(SqlCommand cmd)
        {
            /*
            cmd.Parameters.Add(zoneHeightParameterName, SqlDbType.Float).Value = zoneHeight;
             * */
        }

        public Table GetHtmTable(int stepNumber, bool partial)
        {
            return queryObject.GetTemporaryTable(String.Format("Htm_{0}_{1}", partial ? "Partial" : "Inner", stepNumber));
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
                    cr.ColumnName = GetEscapedPropagatedColumnName(cr.TableReference, cr.ColumnName);
                }
            }
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

        protected string GetWeightExpression(string sigmaExpression)
        {
            return String.Format(" 1 / POWER( CONVERT(float,{0}) / 3600 / 180*PI(), 2) ", sigmaExpression);
        }
    }
}
