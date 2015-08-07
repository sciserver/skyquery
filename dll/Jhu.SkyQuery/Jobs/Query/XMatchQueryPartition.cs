using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.IO;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlCodeGen.SqlServer;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    public abstract class XMatchQueryPartition : QueryPartitionBase
    {
        protected const string regionParameterName = "@region";

        #region Property storage variables

        /// <summary>
        /// XMatch execution steps,
        /// initialized by <see cref="GenerateSteps"/>
        /// </summary>
        protected List<XMatchQueryStep> steps;

        [NonSerialized]
        protected Dictionary<string, XMatchTableSpecification> xmatchTableSpecifications;

        [NonSerialized]
        protected Jhu.Spherical.Region region;

        #endregion
        #region Properties

        /// <summary>
        /// Gets the list of XMatch steps
        /// </summary>
        public List<XMatchQueryStep> Steps
        {
            get { return steps; }
        }

        #endregion
        #region Constructors and initializers

        public XMatchQueryPartition()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public XMatchQueryPartition(XMatchQueryPartition old)
            : base(old)
        {
            CopyMembers(old);
        }

        public XMatchQueryPartition(XMatchQuery query, gw.Context context)
            : base(query, context)
        {
            InitializeMembers(new StreamingContext());
        }

        /// <summary>
        /// Initializes members variables.
        /// </summary>
        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            this.steps = new List<XMatchQueryStep>();
        }

        private void CopyMembers(XMatchQueryPartition old)
        {
            this.steps = new List<XMatchQueryStep>();   // TODO: do deep copy here?
        }

        /// <summary>
        /// When overriden in derived classes, initializes the
        /// <see cref="Steps"/> collection.
        /// </summary>
        /// <param name="tables"></param>
        public abstract void GenerateSteps(XMatchTableSpecification[] tables);

        #endregion
        #region Temporary table name generator functions

        protected Table GetZoneDefTable(int stepNumber)
        {
            return GetTemporaryTable(String.Format("ZoneDef_{0}", stepNumber));
        }

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
        protected Table GetZoneTable(TableReference table)
        {
            return GetTemporaryTable(String.Format("Zone_{0}_{1}_{2}_{3}",
                                                       table.DatasetName,
                                                       table.SchemaName,
                                                       table.DatabaseObjectName,
                                                       table.Alias));
        }

        protected Table GetHtmTable(int stepNumber, bool partial)
        {
            return GetTemporaryTable(String.Format(
                "Htm_{0}_{1}", 
                partial ? "Partial" : "Inner",
                stepNumber));
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
        protected Table GetZoneTable(int stepNumber)
        {
            return GetTemporaryTable(String.Format("Zone_Match_{0}", stepNumber));
        }

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
        protected Table GetLinkTable(int stepNumber)
        {
            return GetTemporaryTable(String.Format("Link_{0}", stepNumber));
        }

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
        protected Table GetPairTable(int stepNumber)
        {
            return GetTemporaryTable(String.Format("Pair_{0}", stepNumber));
        }

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
        protected Table GetMatchTable(int stepNumber)
        {
            return GetTemporaryTable(String.Format("Match_{0}", stepNumber));
        }

        protected string GetMatchTableZoneIndexName(int stepNumber)
        {
            return String.Format("IX_{0}_Zone", GetMatchTable(stepNumber).TableName);
        }

        /* TODO: delete
        /// <summary>
        /// Adds temporary schema name to the table name.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Quoted schema name and table name</returns>
        protected string QuoteSchemaAndTableName(Table table)
        {
            return String.Format("[{0}].[{1}]", table.SchemaName, table.TableName);
        }
        */

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
        #region Column propagator functions

        /// <summary>
        /// Column list nullable type.
        /// </summary>
        public enum ColumnListNullType
        {
            /// <summary>
            /// No 'NULL' or 'NOT NULL' added after columns.
            /// </summary>
            /// <remarks>
            /// For use with select lists, view definitions.
            /// </remarks>
            Nothing,

            /// <summary>
            /// 'NULL' added after each column.
            /// </summary>
            /// <remarks>
            /// Not used.
            /// </remarks>
            Null,

            /// <summary>
            /// 'NOT NULL' added after each column
            /// </summary>
            /// <remarks>
            /// From use with create table.
            /// </remarks>
            NotNull
        }

        /// <summary>
        /// Column list type.
        /// </summary>
        public enum ColumnListType
        {
            /// <summary>
            /// To use with 'CREATE TABLE' column list.
            /// </summary>
            /// <remarks>
            /// Escaped name used, column type is added.
            /// </remarks>
            ForCreateTable,

            /// <summary>
            /// To use with 'CREATE VIEW' column list.
            /// </summary>
            /// <remarks>
            /// Escaped name used, column type is not added.
            /// </remarks>
            ForCreateView,

            /// <summary>
            /// To use with 'INSERT' column list.
            /// </summary>
            /// <remarks>
            /// Escaped name used, column type is not added.
            /// </remarks>
            ForInsert,

            /// <summary>
            /// To use with 'SELECT'.
            /// </summary>
            /// <remarks>
            /// Original name used. To use with zone table create
            /// from source tables.
            /// </remarks>
            ForSelectWithOriginalName,

            /// <summary>
            /// To use with 'SELECT'.
            /// </summary>
            /// <remarks>
            /// Escaped name used. To use with anything but zone table
            /// create from source tables.
            /// </remarks>
            ForSelectWithEscapedName,

            /// <summary>
            /// To use with 'SELECT'.
            /// </summary>
            /// <remarks>
            /// Escaped name used, no column alias added. To use
            /// with 'INSERT' or 'CREATE INDEX'
            /// </remarks>
            ForSelectNoAlias
        }

        [Flags]
        public enum ColumnListInclude
        {
            None = 0,
            PrimaryKey = 1,
            Referenced = 2,
            All = PrimaryKey | Referenced
        }

        /// <summary>
        /// Returns a SQL snippet with the list of primary keys
        /// and propagated columns belonging to the table.
        /// </summary>
        /// <param name="table">Reference to the table.</param>
        /// <param name="type">Column list type.</param>
        /// <param name="nullType">Column nullable type.</param>
        /// <param name="tableAlias">Optional table alias prefix, specify null to omit.</param>
        /// <returns>A SQL snippet with the list of columns.</returns>
        protected string GetPropagatedColumnList(XMatchTableSpecification table, ColumnListType type, ColumnListInclude include, ColumnListNullType nullType, string tableAlias)
        {
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
        }

        #endregion
        #region Parsing function

        protected override void FinishInterpret(bool forceReinitialize)
        {
            // Collect cross match tables

            var xmqs = (XMatchQuerySpecification)SelectStatement.EnumerateQuerySpecifications().First();
            xmatchTableSpecifications = new Dictionary<string, XMatchTableSpecification>(SchemaManager.Comparer);

            foreach (var xt in xmqs.XMatchTableSource.EnumerateXMatchTableSpecifications())
            {
                xt.SetCodeDataset(CodeDataset);
                xmatchTableSpecifications.Add(xt.TableReference.UniqueName, xt);
            }

            // Process region clause
            var rc = xmqs.FindDescendant<RegionClause>();
            if (rc != null)
            {
                if (rc.IsUri)
                {
                    // TODO: implement region fetch
                    // need to do it once per query, not per partition?
                }
                if (rc.IsString)
                {
                    var p = new Jhu.Spherical.Parser.Parser(rc.RegionString);
                    region = p.ParseRegion();
                }
                else
                {
                    // TODO: implement direct region grammar
                    throw new NotImplementedException();
                }
            }

            base.FinishInterpret(forceReinitialize);
        }

        #endregion
        #region ZoneDef table function

        public void CreateZoneDefTable(XMatchQueryStep step)
        {
            if (step.StepNumber != 0)
            {
                var zonedeftable = GetZoneDefTable(step.StepNumber);

                // Drop table if it exists (unlikely, but might happen during debugging)
                zonedeftable.Drop();

                var sql = new StringBuilder(XMatchScripts.CreateZoneDefTable);

                sql.Replace("[$tablename]", CodeGenerator.GetResolvedTableName(zonedeftable));
                sql.Replace("[$indexname]", String.Format("[IXC_{0}_{1}]", zonedeftable.SchemaName, zonedeftable.TableName));

                ExecuteSqlCommand(sql.ToString(), CommandTarget.Temp);

                TemporaryTables.TryAdd(zonedeftable.TableName, zonedeftable);

                PopulateZoneDefTable(step);
            }
        }

        private void PopulateZoneDefTable(XMatchQueryStep step)
        {
            var zonedeftablename = GetZoneDefTable(step.StepNumber);

            var sql = new StringBuilder(XMatchScripts.PopulateZoneDefTable);

            sql.Replace("[$tablename]", CodeGenerator.GetResolvedTableName(zonedeftablename));
            sql.Replace("[$indexname]", String.Format("[IXC_{0}_{1}]", Query.TemporaryDataset.DefaultSchemaName, zonedeftablename));

            using (var cmd = new SqlCommand(sql.ToString()))
            {
                cmd.Parameters.Add("@ZoneHeight", SqlDbType.Float).Value = ((XMatchQuery)Query).ZoneHeight;
                cmd.Parameters.Add("@Theta", SqlDbType.Float).Value = step.SearchRadius;
                cmd.Parameters.Add("@PartitionMin", SqlDbType.Float).Value = Math.Max((double)PartitioningKeyFrom, -90);
                cmd.Parameters.Add("@PartitionMax", SqlDbType.Float).Value = Math.Min((double)PartitioningKeyTo, 90);

                ExecuteSqlCommand(cmd, CommandTarget.Code);
            }
        }

        #endregion
        #region Zone table functions

        /// <summary>
        /// Creates a zone table without populating it.
        /// </summary>
        /// <param name="step">XMatch query step.</param>
        /// <remarks>
        /// If <see cref="fromMatchTable"/> is true, zone table created
        /// according to a match table, otherwise for a source table.
        /// This is a slow operation and cannot be called inside a
        /// Graywulf context.
        /// Call <see cref="PrepareCreateZoneTable"/> before this.
        /// </remarks>
        public void CreateZoneTable(XMatchQueryStep step)
        {
            // Create zone table from match table
            if (step.StepNumber > 0)
            {
                CreateZoneTable(xmatchTableSpecifications[step.XMatchTable]);
                PopulateZoneTable(step);
            }
        }

        protected abstract string GetCreateZoneTableScript();

        private string GetCreateZoneTableScript(XMatchTableSpecification table, Table zonetable)
        {
            var sql = new StringBuilder(GetCreateZoneTableScript());

            sql.Replace("[$tablename]", CodeGenerator.GetResolvedTableName(zonetable));
            sql.Replace("[$indexname]", String.Format("[IXC_{0}_{1}]", zonetable.SchemaName, zonetable.TableName));
            sql.Replace("[$columnlist]", GetPropagatedColumnList(table, ColumnListType.ForCreateTable, ColumnListInclude.PrimaryKey, ColumnListNullType.NotNull, null));

            return sql.ToString();
        }

        /// <summary>
        /// Create an empty zone table for a source table.
        /// </summary>
        /// <param name="table">Reference to the source table.</param>
        private void CreateZoneTable(XMatchTableSpecification table)
        {
            var zonetable = GetZoneTable(table.TableReference);

            // Drop table if it exists (unlikely, but might happen during debugging)
            zonetable.Drop();

            var sql = GetCreateZoneTableScript(table, zonetable);
            ExecuteSqlCommand(sql, CommandTarget.Temp);

            TemporaryTables.TryAdd(zonetable.TableName, zonetable);
        }

        protected abstract string GetPopulateZoneTableScript(XMatchTableSpecification table);

        protected void SubstituteCoordinates(StringBuilder sql, TableCoordinates coords)
        {
            sql.Replace("[$ra]", coords.RA);
            sql.Replace("[$dec]", coords.Dec);
            sql.Replace("[$cx]", coords.X);
            sql.Replace("[$cy]", coords.Y);
            sql.Replace("[$cz]", coords.Z);
            sql.Replace("[$htmid]", coords.HtmId);
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
        private void PopulateZoneTable(XMatchQueryStep step)
        {
            var table = xmatchTableSpecifications[step.XMatchTable];
            var coords = table.Coordinates;

            // Check if table is remote and cached locally
            var zonetable = GetZoneTable(table.TableReference);
            var zonedeftable = GetZoneDefTable(step.StepNumber);

            // --- Build SQL query

            var sql = new StringBuilder(GetPopulateZoneTableScript(table));

            sql.Replace("[$zonetablename]", CodeGenerator.GetResolvedTableName(zonetable));
            sql.Replace("[$zonedeftable]", CodeGenerator.GetResolvedTableName(zonedeftable));
            sql.Replace("[$tablename]", SubstituteRemoteTableNameWithAlias(table.TableReference));
            sql.Replace("[$insertcolumnlist]", GetPropagatedColumnList(table, ColumnListType.ForInsert, ColumnListInclude.PrimaryKey, ColumnListNullType.Nothing, null));
            sql.Replace("[$selectcolumnlist]", GetPropagatedColumnList(table, ColumnListType.ForSelectWithOriginalName, ColumnListInclude.PrimaryKey, ColumnListNullType.Nothing, null));

            SubstituteCoordinates(sql, coords);

            if (region != null)
            {
                var htminner = GetHtmTable(step.StepNumber, false);
                var htmpartial = GetHtmTable(step.StepNumber, true);

                sql.Replace("[$htm_inner]", CodeGenerator.GetResolvedTableName(htminner));
                sql.Replace("[$htm_partial]", CodeGenerator.GetResolvedTableName(htmpartial));

                sql.Replace("[$where_inner]", GetPartitioningKeyWhereClause(step));
                sql.Replace("[$where_partial]", GetPartitioningKeyWhereClause(step));
            }
            else
            {
                sql.Replace("[$where]", GetPartitioningKeyWhereClause(step));
            }

            using (var cmd = new SqlCommand(sql.ToString()))
            {
                AppendPartitioningConditionParameters(cmd, 2 * ((XMatchQuery)Query).ZoneHeight);
                AppendRegionParameter(cmd);
                cmd.Parameters.Add("@H", SqlDbType.Float).Value = ((XMatchQuery)Query).ZoneHeight;

                ExecuteSqlCommand(cmd, CommandTarget.Code);
            }
        }

        protected override bool IsPartitioningKeyUnbound(object key)
        {
            return key == null || Double.IsInfinity((double)key) || Double.IsNaN((double)key);
        }

        protected void AppendPartitioningConditionParameters(SqlCommand cmd, double buffer)
        {
            if (!IsPartitioningKeyUnbound(PartitioningKeyFrom))
            {
                cmd.Parameters.Add(keyFromParameterName, SqlDbType.Float).Value = (double)PartitioningKeyFrom - buffer;
            }

            if (!IsPartitioningKeyUnbound(PartitioningKeyTo))
            {
                cmd.Parameters.Add(keyToParameterName, SqlDbType.Float).Value = (double)PartitioningKeyTo + buffer;
            }
        }

        protected void AppendRegionParameter(SqlCommand cmd)
        {
            if (region != null)
            {
                cmd.Parameters.Add(regionParameterName, SqlDbType.VarBinary).Value = region.ToSqlBytes();
            }
        }

        /// <summary>
        /// Drops a temporary zone table.
        /// </summary>
        /// <param name="step">Number of XMatch step.</param>
        public void DropZoneTable(XMatchQueryStep step)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Link table functions

        /// <summary>
        /// Creates a link table between two zone tables.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        /// <remarks>
        /// Call <see cref="PrepareCreateLinkTable"/> before calling
        /// this functon.
        /// This function calls <see cref="PopulateLinkTable"/> after
        /// the empty table is created.
        /// </remarks>
        public void CreateLinkTable(XMatchQueryStep step)
        {
            if (step.StepNumber != 0)
            {
                var linktable = GetLinkTable(step.StepNumber);

                // Drop table if it exists (unlikely, but might happen during debugging)
                linktable.Drop();

                var sql = new StringBuilder(XMatchScripts.CreateLinkTable);

                sql.Replace("[$tablename]", CodeGenerator.GetResolvedTableName(linktable));
                sql.Replace("[$indexname]", String.Format("PK_{0}", linktable.TableName));

                ExecuteSqlCommand(sql.ToString(), CommandTarget.Temp);

                TemporaryTables.TryAdd(linktable.TableName, linktable);

                PopulateLinkTable(step);
            }
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
        protected abstract void PopulateLinkTable(XMatchQueryStep step);

        /// <summary>
        /// Drops a link table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public void DropLinkTable(XMatchQueryStep step)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Pair table functions

        /// <summary>
        /// Creates a pair table from a link table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        /// <remarks>
        /// Call <see cref="PrepareCreatePairTable"/> before calling this function.
        /// Function calls <see cref="PopulatePairTable"/> to populate
        /// pair table form a link table.
        /// </remarks>
        public void CreatePairTable(XMatchQueryStep step)
        {
            if (step.StepNumber != 0)
            {
                var pairtable = GetPairTable(step.StepNumber);

                // Drop table if it exists (unlikely, but might happen during debugging)
                pairtable.Drop();

                var sql = new StringBuilder(XMatchScripts.CreatePairTable);

                sql.Replace("[$tablename]", CodeGenerator.GetResolvedTableName(pairtable));
                sql.Replace("[$createcolumnlist1]", String.Format("PK_Match_{0}_MatchID [bigint] NOT NULL", step.StepNumber - 1));
                sql.Replace("[$createcolumnlist2]", GetPropagatedColumnList(xmatchTableSpecifications[step.XMatchTable], ColumnListType.ForCreateTable, ColumnListInclude.PrimaryKey, ColumnListNullType.NotNull, null));

                ExecuteSqlCommand(sql.ToString(), CommandTarget.Temp);

                TemporaryTables.TryAdd(pairtable.TableName, pairtable);

                PopulatePairTable(step);
            }
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
        protected abstract void PopulatePairTable(XMatchQueryStep step);

        /// <summary>
        /// Drops a pair table.
        /// </summary>
        /// <param name="step"></param>
        public void DropPairTable(XMatchQueryStep step)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Match table functions

        protected abstract string GetCreateMatchTableScript(XMatchQueryStep step);

        /// <summary>
        /// Creates a match table from a pair table or, in the first
        /// iteration, created a view over an existing zone table to
        /// look like as a match table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step</param>
        /// <remarks>
        /// Call <see cref="PrepareCreateMatchTable"/> before this function.
        /// This function calls <see cref="CreateMatchTableSpecific"/> and
        /// <see cref="PopulateMatchTable"/> that must be implemented in
        /// derived classes.
        /// 
        /// 08/27/2010: Modified not to create a view in the 0th iteration as
        ///             the non-indexes view caused wrong query plan (non-parallel)
        /// </remarks>
        public void CreateMatchTable(XMatchQueryStep step)
        {
            // Create real match tables
            var matchtable = GetMatchTable(step.StepNumber);
            var indexname = String.Format("[IXC_{0}_{1}]", matchtable.SchemaName, matchtable.TableName);

            // Drop table if it exists (unlikely, but might happen during debugging)
            matchtable.Drop();

            ColumnListInclude include = ((XMatchQuery)Query).PropagateColumns ? ColumnListInclude.All : ColumnListInclude.PrimaryKey;

            // Add all propagated columns
            StringBuilder columnlist = new StringBuilder();
            for (int i = 0; i <= step.StepNumber; i++)
            {
                if (xmatchTableSpecifications[steps[i].XMatchTable].InclusionMethod != XMatchInclusionMethod.Drop)
                {
                    if (columnlist.Length != 0)
                    {
                        columnlist.Append(", ");
                    }
                    columnlist.AppendLine(GetPropagatedColumnList(xmatchTableSpecifications[steps[i].XMatchTable], ColumnListType.ForCreateTable, include, ColumnListNullType.NotNull, null));
                }
            }

            using (var cmd = new SqlCommand())
            {
                var sql = new StringBuilder(GetCreateMatchTableScript(step));

                sql.Replace("[$tablename]", matchtable.TableName);
                sql.Replace("[$indexname]", indexname);
                sql.Replace("[$columnlist]", columnlist.ToString());


                cmd.CommandText = sql.ToString();
                ExecuteSqlCommand(cmd, CommandTarget.Temp);
            }

            TemporaryTables.TryAdd(matchtable.TableName, matchtable);

            if (step.StepNumber == 0)
            {
                PopulateInitialMatchTable(step);
            }
            else
            {
                PopulateMatchTable(step);
            }

            BuildInitialMatchTableIndex(xmatchTableSpecifications[step.XMatchTable], step.StepNumber);
        }

        protected abstract string GetPopulateMatchTableScript(XMatchQueryStep step, SqlCommand cmd);

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
        private void PopulateInitialMatchTable(XMatchQueryStep step)
        {
            var table = xmatchTableSpecifications[step.XMatchTable];
            var coords = table.Coordinates;

            var include = ((XMatchQuery)Query).PropagateColumns ? ColumnListInclude.All : ColumnListInclude.PrimaryKey;

            using (SqlCommand cmd = new SqlCommand())
            {
                var sql = new StringBuilder(GetPopulateInitialMatchTableScript(step, cmd));

                sql.Replace("[$newtablename]", CodeGenerator.GetResolvedTableName(GetMatchTable(step.StepNumber)));
                sql.Replace("[$tablename]", SubstituteRemoteTableNameWithAlias(table.TableReference));
                sql.Replace("[$insertcolumnlist]", GetPropagatedColumnList(table, ColumnListType.ForInsert, include, ColumnListNullType.Nothing, null));
                sql.Replace("[$selectcolumnlist]", GetPropagatedColumnList(table, ColumnListType.ForSelectWithOriginalName, include, ColumnListNullType.Nothing, table.TableReference.Alias));

                SubstituteCoordinates(sql, coords);

                // No buffering when initial partitioning is done
                if (region != null)
                {
                    var htminner = GetHtmTable(step.StepNumber, false);
                    var htmpartial = GetHtmTable(step.StepNumber, true);

                    sql.Replace("[$htm_inner]", CodeGenerator.GetResolvedTableName(htminner));
                    sql.Replace("[$htm_partial]", CodeGenerator.GetResolvedTableName(htmpartial));

                    sql.Replace("[$where_inner]", GetPartitioningKeyWhereClause(step));
                    sql.Replace("[$where_partial]", GetPartitioningKeyWhereClause(step));
                }
                else
                {
                    sql.Replace("[$where]", GetPartitioningKeyWhereClause(step));
                }

                AppendPartitioningConditionParameters(cmd, 0);
                AppendRegionParameter(cmd);
                cmd.Parameters.Add("@H", SqlDbType.Float).Value = ((XMatchQuery)Query).ZoneHeight;

                cmd.CommandText = sql.ToString();

                ExecuteSqlCommand(cmd, CommandTarget.Code);
            }
        }

        protected abstract string GetBuildMatchTableIndexScript(XMatchTableSpecification table, int stepNumber);

        private void BuildInitialMatchTableIndex(XMatchTableSpecification table, int stepNumber)
        {
            var matchtable = GetMatchTable(stepNumber);
            var indexname = GetMatchTableZoneIndexName(stepNumber);

            StringBuilder sql = new StringBuilder(GetBuildMatchTableIndexScript(table, stepNumber));

            sql.Replace("[$indexname]", indexname);
            sql.Replace("[$tablename]", matchtable.TableName);
            sql.Replace("[$columnlist]", GetPropagatedColumnList(table, ColumnListType.ForInsert, ColumnListInclude.PrimaryKey, ColumnListNullType.Nothing, null));

            ExecuteSqlCommand(sql.ToString(), CommandTarget.Temp);
        }

        /// <summary>
        /// When overriden in a derived class, populates a match table
        /// from a pair table and an older match table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        private void PopulateMatchTable(XMatchQueryStep step)
        {
            var table = xmatchTableSpecifications[step.XMatchTable];

            var tablename = SubstituteRemoteTableName(table.TableReference);
            var newtablename = GetMatchTable(step.StepNumber);
            var schemaname = Query.TemporaryDataset.DefaultSchemaName;

            var include = ((XMatchQuery)Query).PropagateColumns ? ColumnListInclude.All : ColumnListInclude.PrimaryKey;

            // --- Propagated columns
            // Gather all columns from previous steps
            var insertcolumnlist = new StringBuilder();
            var selectcolumnlist = new StringBuilder();
            for (int i = 0; i <= step.StepNumber; i++)
            {
                if (xmatchTableSpecifications[steps[i].XMatchTable].InclusionMethod != XMatchInclusionMethod.Drop)
                {
                    if (insertcolumnlist.Length != 0)
                    {
                        insertcolumnlist.Append(", ");
                        selectcolumnlist.Append(", ");
                    }

                    var tablealias = (i < step.StepNumber) ? "tableA" : "tableB";
                    var listtype = (i < step.StepNumber) ? ColumnListType.ForSelectNoAlias : ColumnListType.ForSelectWithOriginalName;

                    // ForSelectNoalias -> ForInsert
                    insertcolumnlist.Append(GetPropagatedColumnList(xmatchTableSpecifications[steps[i].XMatchTable], ColumnListType.ForSelectNoAlias, include, ColumnListNullType.Nothing, null));
                    selectcolumnlist.Append(GetPropagatedColumnList(xmatchTableSpecifications[steps[i].XMatchTable], listtype, include, ColumnListNullType.Nothing, tablealias));
                }
            }

            // --- Zone table join conditions
            var join = new StringBuilder();
            var t = (TableOrView)xmatchTableSpecifications[step.XMatchTable].TableReference.DatabaseObject;

            foreach (var c in t.PrimaryKey.Columns.Values)
            {
                join.AppendLine(String.Format("[tableB].[{1}] = [pairtable].[{0}]",
                                              GetEscapedPropagatedColumnName(xmatchTableSpecifications[step.XMatchTable].TableReference, c.Name),
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
        }

        /// <summary>
        /// Drops a temporary match table.
        /// </summary>
        /// <param name="step">Reference to the XMatch step.</param>
        public void DropMatchTable(XMatchQueryStep step)
        {
            throw new NotImplementedException();
        }

        #endregion

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

        /* TODO: delete
        protected void SubstituteXMatchTableSources(Jhu.Graywulf.SqlParser.QuerySpecification qs, List<TableReference> xmatchTableReferences)
        {
            var from = qs.FindDescendant<Jhu.Graywulf.SqlParser.FromClause>();

            SubstituteXMatchTableSources(
                from.FindDescendant<Jhu.Graywulf.SqlParser.TableSourceExpression>().FindDescendant<Jhu.Graywulf.SqlParser.TableSource>(),
                xmatchTableReferences);
        }
         * */

        /* TODO: delete
        /// <summary>
        /// Replaces references to the xmatched table with the temporary table containing
        /// the xmatch results
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="xmatchTableReferences"></param>
        private void SubstituteXMatchTableSources(Jhu.Graywulf.SqlParser.TableSource table, List<TableReference> xmatchTableReferences)
        {
            // Get the next joined-in table
            // It is assumed here that xmatched tables take part only in CROSS JOIN
            // so the join operator can be eliminated without side-effects
            var jts = table.Parent.FindDescendant<Jhu.Graywulf.SqlParser.JoinedTable>();
            if (jts != null)
            {
                // Call recursively
                SubstituteXMatchTableSources(jts.FindDescendant<Jhu.Graywulf.SqlParser.TableSource>(), xmatchTableReferences);
            }

            // Look for references to the current table
            // The current table is part of an xmatch operation so if found, then
            // has to be removed from the list
            var found = false;
            foreach (var tr in xmatchTableReferences)
            {
                if (table.TableReference.Compare(tr))
                {
                    found = true;

                    xmatchTableReferences.Remove(tr);
                    break;
                }
            }

            // If the current table reference is a reference to an xmatched table
            // create a new table source node that points to the temporary,
            // already xmatched table
            if (found)
            {
                // If this is not the last xmatched table, simply remove it from
                // the FROM clause. If it is the last, a reference to the xmatch
                // output table has to be added to the query
                if (xmatchTableReferences.Count > 0)
                {
                    // Removing a table from the FROM clause requires removing
                    // a link from the join chain
                    jts = table.Parent.FindDescendant<Jhu.Graywulf.SqlParser.JoinedTable>();
                    if (jts != null)
                    {
                        // First move the joined table backward in the chain and
                        // then remove the unnecessary table from the chain

                        table.Parent.Parent.Stack.AddBefore(
                            table.Parent.Parent.Stack.Find(table.Parent),
                            jts);
                        jts.Parent = table.Parent.Parent;

                        var jtsp = table.Parent.Parent.Stack.Find(jts);
                        if (jtsp.Previous != null && !(jtsp.Previous.Value is CommentOrWhitespace))
                        {
                            table.Parent.Parent.Stack.AddBefore(
                                jtsp,
                                Whitespace.Create());
                        }
                    }

                    table.Parent.Parent.Stack.Remove(table.Parent);
                }
                else
                {
                    var matchtable = GetMatchTable(steps.Count - 1);

                    var nts = new Jhu.Graywulf.SqlParser.ComputedTableSource();
                    nts.TableReference = new TableReference();
                    nts.TableReference.DatabaseName = matchtable.DatabaseName;
                    nts.TableReference.SchemaName = matchtable.SchemaName;
                    nts.TableReference.DatabaseObjectName = matchtable.TableName;
                    nts.TableReference.Alias = "matchtable";

                    nts.Stack.AddLast(TableOrViewName.Create(nts.TableReference));
                    nts.Stack.AddLast(Whitespace.Create());
                    nts.Stack.AddLast(Jhu.Graywulf.ParserLib.Keyword.Create("AS"));
                    nts.Stack.AddLast(Whitespace.Create());
                    nts.Stack.AddLast(TableAlias.Create("matchtable"));

                    // Add the match table reference to the join chain
                    // First remove the xmatched table and then link in the newly created
                    // reference to
                    table.Parent.Stack.AddBefore(
                        table.Parent.Stack.Find(table),
                        Jhu.Graywulf.SqlParser.TableSource.Create(nts));
                    table.Parent.Stack.Remove(table);
                }
            }
        }*/

        protected override string GetExecuteQueryText()
        {
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
        }

        protected string GetWeightExpression(string sigmaExpression)
        {
            return String.Format(" 1 / POWER( CONVERT(float,{0}) / 3600 / 180*PI(), 2) ", sigmaExpression);
        }

        protected double GetWeight(double sigma)
        {
            return 1 / Math.Pow(sigma / 3600 / 180 * Math.PI, 2);
        }

        private double GetBufferZoneSize(XMatchQueryStep step)
        {
            return 2 * step.SearchRadius;
        }

        protected string GetPartitioningKeyWhereClause(XMatchQueryStep step)
        {
            var table = xmatchTableSpecifications[step.XMatchTable];

            // --- Find predicates only referencing this table

            var cn = new SearchConditionNormalizer();
            cn.Execute(SelectStatement);     // TODO: what if more than one QS? 
            var where = cn.GenerateWhereClauseSpecificToTable(table.TableReference);

            // --- Append partitioning key

            var sc = GetPartitioningConditions(table.Coordinates.Dec);

            if (sc != null)
            {
                if (where == null)
                {
                    where = Jhu.SkyQuery.Parser.WhereClause.Create(sc);
                }
                else
                {
                    where.AppendCondition(sc, "AND");
                }
            }

            if (where != null)
            {
                var wherestring = new StringWriter();
                var cg = new SqlServerCodeGenerator();
                cg.Execute(wherestring, where);

                return wherestring.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
