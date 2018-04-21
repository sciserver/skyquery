using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.CodeGeneration;
using Jhu.Graywulf.Sql.CodeGeneration.SqlServer;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.IO.Tasks;
using Jhu.SkyQuery.Parser;
using Jhu.Spherical;

namespace Jhu.SkyQuery.Jobs.Query
{
    public class RegionQueryCodeGenerator : SqlQueryCodeGenerator
    {
        #region Constants

        protected const string regionParameterName = "@region";
        protected const string regionUdtVariableName = "@r";

        #endregion

        private bool fallBackToDefaultColumns;

        #region Properties

        public bool FallBackToDefaultColumns
        {
            get { return fallBackToDefaultColumns; }
        }

        private RegionQueryPartition Partition
        {
            get { return queryObject as RegionQueryPartition; }
        }

        #endregion
        #region Constructors and initializers

        public RegionQueryCodeGenerator()
        {
            InitializeMembers();
        }

        public RegionQueryCodeGenerator(QueryObject queryObject)
            : base(queryObject)
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.fallBackToDefaultColumns = true;
        }

        #endregion
        #region Basic query rewrite functions

        /// <summary>
        /// Generates a SQL script based on the query by replacing the REGION clause
        /// with a standard SQL implementation based on HTM indexing and spherical functions
        /// </summary>
        /// <param name="selectStatement"></param>
        /// <returns></returns>
        protected override SourceQuery OnGetExecuteQuery(QueryDetails query)
        {
            var header = new StringBuilder();
            var regions = new List<Region>();

            foreach (var selectStatement in query.ParsingTree.EnumerateDescendantsRecursive<SelectStatement>())
            {
                if (selectStatement is RegionSelectStatement)
                {
                    AppendRegionJoinsAndConditions((RegionSelectStatement)selectStatement, regions);
                    RemoveNonStandardTokens(selectStatement);
                }
            }

            for (int i = 0; i < regions.Count; i++)
            {
                header.AppendLine(String.Format("DECLARE @r{0} dbo.Region = @region{0};", i));
            }

            var source = base.OnGetExecuteQuery(query);
            source.Header = header.ToString();

            // Return a table source query
            source.Dataset = queryObject.TemporaryDataset;
            AppendRegionParameters(source, regions);

            return source;
        }

        /// <summary>
        /// Removes non-standard SQL clauses from the parsing tree.
        /// </summary>
        /// <param name="qs"></param>
        protected override void RemoveNonStandardTokens(Graywulf.Sql.Parsing.QuerySpecification qs)
        {
            foreach (var ts in qs.EnumerateDescendantsRecursive<SkyQuery.Parser.CoordinatesTableSource>())
            {
                // TODO: update this to leave standard SQL Server hints?
                var hint = ts.FindDescendant<Jhu.Graywulf.Sql.Parsing.TableHintClause>();

                if (hint != null)
                {
                    hint.Parent.Stack.Remove(hint);
                }
            }

            // Strip off region clause
            var region = qs.FindDescendant<Jhu.SkyQuery.Parser.RegionClause>();
            if (region != null)
            {
                region.Parent.Stack.Remove(region);
            }

            base.RemoveNonStandardTokens(qs);
        }

        #endregion
        #region HTM joins and conditions

        private void AppendRegionJoinsAndConditions(RegionSelectStatement selectStatement, List<Region> regions)
        {
            int qsi = 0;
            int tsi = 0;

            // TODO: add support for WITH?

            var qe = (RegionQueryExpression)selectStatement.QueryExpression;
            AppendRegionJoinsAndConditions(qe, regions, ref qsi, ref tsi);
        }

        private void AppendRegionJoinsAndConditions(Graywulf.Sql.Parsing.QueryExpressionBrackets qeb, List<Region> regions, ref int qsi, ref int tsi)
        {
            var qee = qeb.FindDescendant<Jhu.Graywulf.Sql.Parsing.QueryExpression>();
            AppendRegionJoinsAndConditions(qee, regions, ref qsi, ref tsi);
        }

        private void AppendRegionJoinsAndConditions(QueryExpression queryExpression, List<Region> regions, ref int qsi, ref int tsi)
        {
            // QueryExpression can have two children of type QueryExpression,
            // one in brackets and another one after a query operator.

            // Recursive calls on children

            // QueryExpression
            var qee = queryExpression.FindDescendant<Jhu.Graywulf.Sql.Parsing.QueryExpression>();
            if (qee != null)
            {
                AppendRegionJoinsAndConditions(qee, regions, ref qsi, ref tsi);
            }

            // QueryExpressionBrackets
            var qeb = queryExpression.FindDescendant<QueryExpressionBrackets>();
            if (qeb != null)
            {
                AppendRegionJoinsAndConditions(qeb, regions, ref qsi, ref tsi);
            }

            // Process query specification, generate inner and partial HTM query part
            var qs = queryExpression.FindDescendant<QuerySpecification>();

            if (qs is RegionQuerySpecification)
            {
                var rqs = (RegionQuerySpecification)qs;
                var from = rqs.FromClause;
                var region = rqs.Region;

                // XMatch queries might have no regions
                if (region != null)
                {
                    regions.Add(region);

                    // Descend on table sources and append htm constraint to each of them
                    var ts = from.FindDescendantRecursive<TableSource>();
                    if (ts != null)
                    {
                        AppendRegionJoinsAndConditions(ts, qsi, ref tsi);
                    }

                    qsi++;
                }
            }
        }

        /// <summary>
        /// Generates a UNION query to do htm-based filtering of the original table
        /// </summary>
        /// <param name="tableSource"></param>
        /// <param name="qsi"></param>
        /// <param name="tsi"></param>
        private void AppendRegionJoinsAndConditions(TableSource tableSource, int qsi, ref int tsi)
        {
            // Descend on table sources and append htm constraint to each of them
            var rts = tableSource.FindDescendant<TableSource>();
            if (rts != null)
            {
                AppendRegionJoinsAndConditions(rts, qsi, ref tsi);
            }

            if (tableSource.SpecificTableSource is CoordinatesTableSource)
            {
                // Get original table that will be filtered by region
                var ts = (CoordinatesTableSource)tableSource.SpecificTableSource;
                var coords = ts.Coordinates;

                // Check if it's possible to filter based on HTMID. If HTMID is not
                // available but the table is opted-in for region filtering by
                // specifying the coordinates then use row-by-row filtering
                if (coords.IsHtmIdHintSpecified || fallBackToDefaultColumns && coords.IsHtmIdColumnAvailable)
                {
                    // Make sure table references in the generated subquery
                    // remain pointed to the original table
                    // Copy table source because original will be mapped to new one
                    var sqname = String.Format("__htm_t{0}", tsi);
                    var htmts = (CoordinatesTableSource)ts.Clone();
                    var htmtr = htmts.TableReference = (TableReference)ts.TableReference.Clone();

                    // Generate HTM logic and wrap it up into a subquery
                    var qs1 = GenerateHtmJoinTableQuerySpecification(htmts, qsi, false);
                    var qs2 = GenerateHtmJoinTableQuerySpecification(htmts, qsi, true);
                    var qe = QueryExpression.Create(qs1, qs2, QueryOperator.CreateUnionAll());
                    var ss = SelectStatement.Create(qe);
                    var sqts = SubqueryTableSource.Create(ss, sqname);

                    // Make sure subquery has a new table reference so it doesn't get automatically
                    // mapped.
                    // TODO: verify this because it might break server assignment logic
                    SubstituteTableReference(ss, ts.TableReference, htmtr);

                    // Create a copy of the original table reference, substitute subquery with the new one
                    // and rewrite the old one to point to the subquery
                    var tr = ts.TableReference;
                    var ntr = new TableReference(tr)
                    {
                        Alias = sqname,
                        DatasetName = null,
                        DatabaseName = null,
                        SchemaName = null,
                        DatabaseObjectName = null
                    };

                    // Re-map table reference to point to the subquery
                    if (TableReferenceMap.ContainsKey(tr))
                    {
                        TableReferenceMap.Remove(tr);
                    }
                    TableReferenceMap.Add(tr, ntr);

                    // TODO: review this because ResultsTableReference logic
                    // has changed
                    ss.QueryExpression.ResultsTableReference = tr;
                    tr.InterpretTableSource(sqts);

                    // Replace table source with subquery
                    ts.ExchangeWith(sqts);

                    tsi++;
                }
                else if (coords.IsEqHintSpecified || coords.IsCartesianHintSpecified ||
                    fallBackToDefaultColumns && coords.IsEqColumnsAvailable ||
                    fallBackToDefaultColumns && coords.IsCartesianColumnsAvailable)
                {
                    var sc = GenerateRegionContainsCondition(ts, qsi);
                    var qs = ts.FindAscendant<Jhu.Graywulf.Sql.Parsing.QuerySpecification>();
                    qs.AppendSearchCondition(sc, "AND");
                }
            }
        }

        /// <summary>
        /// Generates the HTM-based join query for table filtering
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="qsi"></param>
        /// <param name="partial"></param>
        /// <returns></returns>
        private Jhu.Graywulf.Sql.Parsing.QuerySpecification GenerateHtmJoinTableQuerySpecification(CoordinatesTableSource ts, int qsi, bool partial)
        {
            var coords = ts.Coordinates;

            var fts = GenerateHtmCoverFunctionCall(qsi);
            var ts2 = TableSource.Create(ts);
            var jc = GenerateHtmJoinCondition(ts, fts.TableReference, partial, qsi);
            var jt = JoinedTable.Create(JoinType.CreateInnerLoop(), ts2, jc);
            var tse = TableSourceExpression.Create(fts, jt);
            var fro = FromClause.Create(tse);
            var tr = MapTableReference(ts.TableReference);
            var qs = QuerySpecification.Create(SelectList.CreateStar(tr), fro);

            return qs;
        }

        #endregion
        #region HTM search and join conditions generation

        private Jhu.Graywulf.Sql.Parsing.TableSource GenerateHtmCoverFunctionCall(int qsi)
        {
            var fr = new FunctionReference()
            {
                DatabaseName = CodeDataset.DatabaseName,
                SchemaName = "htm",
                DatabaseObjectName = "Cover"
            };
            var udt = regionUdtVariableName + qsi.ToString();
            var var = Graywulf.Sql.Parsing.Variable.Create(udt);
            var exp = Expression.Create(var);
            var fc = TableValuedFunctionCall.Create(fr, exp);
            var fts = FunctionTableSource.Create(fc, "__htm" + qsi.ToString());
            var ts = TableSource.Create(fts);

            return ts;
        }

        /// <summary>
        /// Create a join condition in the form of
        /// 'htmid BETWEEN htmtable.htmidstart AND htmtable.htmidend'
        /// to be used with tables with HTM index
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="htmTable"></param>
        /// <returns></returns>
        protected Jhu.Graywulf.Sql.Parsing.BooleanExpression GenerateHtmJoinCondition(CoordinatesTableSource ts, TableReference htmTable, bool partial, int qsi)
        {
            var coords = ts.Coordinates;

            var htmidstart = new ColumnReference(htmTable, "HtmIDStart", DataTypes.SqlBigInt);
            var htmidend = new ColumnReference(htmTable, "HtmIDEnd", DataTypes.SqlBigInt);

            var limitsp = Jhu.Graywulf.Sql.Parsing.Predicate.CreateBetween(
                GetHtmIdExpression(coords),
                Expression.Create(ColumnIdentifier.Create(htmidstart)),
                Expression.Create(ColumnIdentifier.Create(htmidend)));
            var limitssc = Jhu.Graywulf.Sql.Parsing.BooleanExpression.Create(false, limitsp);

            var htmpartial = new ColumnReference(htmTable, "Partial", DataTypes.SqlBit);
            var partialp = Jhu.Graywulf.Sql.Parsing.Predicate.CreateEquals(
                Expression.Create(htmpartial),
                Expression.CreateNumber(partial ? "1" : "0"));
            var partialsc = Jhu.Graywulf.Sql.Parsing.BooleanExpression.Create(false, partialp);

            var sc = Jhu.Graywulf.Sql.Parsing.BooleanExpression.Create(limitssc, partialsc, LogicalOperator.CreateAnd());

            // Add precise filtering if necessary
            if (partial &&
                (coords.IsEqHintSpecified || coords.IsCartesianHintSpecified ||
                fallBackToDefaultColumns && coords.IsEqColumnsAvailable ||
                fallBackToDefaultColumns && coords.IsCartesianColumnsAvailable))
            {
                var rcc = GenerateRegionContainsCondition(ts, qsi);
                sc = Jhu.Graywulf.Sql.Parsing.BooleanExpression.Create(sc, rcc, LogicalOperator.CreateAnd());
            }

            return sc;
        }

        protected Jhu.Graywulf.Sql.Parsing.BooleanExpression GenerateRegionContainsCondition(CoordinatesTableSource ts, int qsi)
        {
            string udt;
            var coords = ts.Coordinates;

            if (qsi >= 0)
            {
                udt = regionUdtVariableName + qsi.ToString();
            }
            else
            {
                udt = regionUdtVariableName;
            }

            UdtFunctionCall udtf;

            if (coords.IsCartesianHintSpecified)
            {
                udtf = UdtFunctionCall.Create(udt, "ContainsXyz", GetXyzExpressions(coords));
            }
            else if (coords.IsEqHintSpecified)
            {
                udtf = UdtFunctionCall.Create(udt, "ContainsEq", GetEqExpressions(coords));
            }
            else if (fallBackToDefaultColumns && coords.IsEqColumnsAvailable)
            {
                udtf = UdtFunctionCall.Create(udt, "ContainsXyz", GetXyzExpressions(coords));
            }
            else if (fallBackToDefaultColumns && coords.IsEqColumnsAvailable)
            {
                udtf = UdtFunctionCall.Create(udt, "ContainsEq", GetEqExpressions(coords));
            }
            else
            {
                // TODO: use metadata?
                throw Error.NoCoordinateColumnsFound(coords);
            }

            var p = Jhu.Graywulf.Sql.Parsing.Predicate.CreateEquals(Expression.Create(udtf), Expression.CreateNumber("1"));
            var sc = Jhu.Graywulf.Sql.Parsing.BooleanExpression.Create(false, p);

            return sc;
        }

        #endregion
        #region Augmented table query generation

        protected virtual StringBuilder GetSelectAugmentedTableTemplate()
        {
            return new StringBuilder(RegionScripts.SelectAugmentedTable);
        }

        protected virtual StringBuilder GetSelectAugmentedTableHtmTemplate()
        {
            return new StringBuilder(RegionScripts.SelectAugmentedTableHtm);
        }

        /// <summary>
        /// Generates a query that returns all primary key columns and propagated columns,
        /// augmented with columns necessary for xmatching.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        protected StringBuilder GenerateAugmentedTableQuery(AugmentedTableQueryOptions options)
        {
            StringBuilder sql;

            var coords = options.Table.Coordinates;
            WhereClause where = null;
            WhereClause whereregion = null;

            // 1. Generate column list

            var columnlist = new SqlServerColumnListGenerator(
                MapTableReference(options.Table.TableReference),
                options.ColumnContext,
                options.EscapeColumnNames ?
                    ColumnListType.SelectWithOriginalName :
                    ColumnListType.SelectWithOriginalNameNoAlias)
            {
                TableAlias = null,
                SeparatorRendering = ColumnListSeparatorRendering.Leading,
            };

            // 2. Load template

            var hasRegion =
                options.Region != null && options.UseRegion && options.UseHtm &&
                (options.Table.Coordinates.IsHtmIdHintSpecified ||
                fallBackToDefaultColumns && options.Table.Coordinates.IsHtmIdColumnAvailable);

            if (hasRegion)
            {
                sql = GetSelectAugmentedTableHtmTemplate();
            }
            else
            {
                sql = GetSelectAugmentedTableTemplate();
            }

            // 2. Generate where clause

            OnGenerateAugmentedTableQuery(options, sql, ref where);

            // 3. Add explicit region filter, if necessary

            if (options.Region != null && options.UseRegion)
            {
                // Filter on region containment
                var rc = GenerateRegionContainsCondition(options.Table, -1);

                if (rc != null)
                {
                    if (where != null)
                    {
                        whereregion = (WhereClause)where.Clone();
                        whereregion.AppendCondition(rc, "AND");
                    }
                    else
                    {
                        whereregion = WhereClause.Create(rc);
                    }
                }
            }
            else
            {
                whereregion = where;
            }

            // 4. Rewrite SQL template by substituting tokens

            if (hasRegion)
            {
                SubstituteHtmId(sql, coords);
                sql.Replace("[$where_inner]", Execute(where));
                sql.Replace("[$where_partial]", Execute(whereregion));
            }
            else
            {

                sql.Replace("[$where]", Execute(whereregion));
            }

            // TODO: test
            var table = MapTableReference(options.Table.TableReference);
            sql.Replace("[$tablename]", GetResolvedTableNameWithAlias(table));
            sql.Replace("[$columnlist]", columnlist.Execute());

            SubstituteAugmentedTableColumns(sql, options);

            return sql;
        }

        protected virtual void OnGenerateAugmentedTableQuery(AugmentedTableQueryOptions options, StringBuilder sql, ref WhereClause where)
        {
            if (options.UseWhereConditions)
            {
                // Take the most restrictive where clause from the table
                // The region condition will be appended later as HTM-based joins
                // require different handling
                where = GetTableSpecificWhereClause(options.Table, false);
            }

            if (options.UsePartitioning && Partition != null)
            {
                var pc = GetPartitioningConditions(options.Table.PartitioningKeyExpression);

                if (pc != null)
                {
                    if (where != null)
                    {
                        where.AppendCondition(pc, "AND");
                    }
                    else
                    {
                        where = WhereClause.Create(pc);
                    }
                }
            }
        }

        protected virtual void SubstituteAugmentedTableColumns(StringBuilder sql, AugmentedTableQueryOptions options)
        {
        }

        protected void SubstituteHtmId(StringBuilder sql, TableCoordinates coords)
        {
            var htmex = GetHtmIdExpression(coords);
            sql.Replace("[$htmid]", Execute(htmex));
        }

        #endregion
        #region Coordinate column functions

        protected Expression CreateFunction(string name, params Expression[] args)
        {
            return CreateFunction(CodeDataset.DatabaseName, Constants.SkyQueryFunctionSchema, name, args);
        }

        private Expression CreateFunction(string schema, string name, params Expression[] args)
        {
            return CreateFunction(CodeDataset.DatabaseName, schema, name, args);
        }

        private Expression CreateFunction(string database, string schema, string name, params Expression[] args)
        {
            var fr = new FunctionReference()
            {
                DatabaseName = database,
                SchemaName = schema,
                DatabaseObjectName = name
            };

            return Expression.Create(FunctionCall.Create(fr, args));
        }

        public Expression GetRAExpression(TableCoordinates coords)
        {
            if (coords.IsEqHintSpecified)
            {
                return coords.RAHintExpression;
            }
            else if (coords.IsCartesianHintSpecified)
            {
                return CreateFunction("CartesianToEqRa", coords.XHintExpression, coords.YHintExpression, coords.ZHintExpression);
            }
            else if (fallBackToDefaultColumns && coords.IsEqColumnsAvailable)
            {
                return coords.RAColumnExpression;
            }
            else if (fallBackToDefaultColumns && coords.IsCartesianColumnsAvailable)
            {
                return CreateFunction("CartesianToEqRa", coords.XColumnExpression, coords.YColumnExpression, coords.ZColumnExpression);
            }
            else
            {
                // TODO: Figure out from metadata
                throw Error.NoCoordinateColumnFound(coords, "ra");
            }
        }

        public Expression GetDecExpression(TableCoordinates coords)
        {
            if (coords.IsEqHintSpecified)
            {
                return coords.DecHintExpression;
            }
            else if (coords.IsCartesianHintSpecified)
            {
                return CreateFunction("CartesianToEqDec", coords.XHintExpression, coords.YHintExpression, coords.ZHintExpression);
            }
            else if (fallBackToDefaultColumns && coords.IsEqColumnsAvailable)
            {
                return coords.DecColumnExpression;
            }
            else if (fallBackToDefaultColumns && coords.IsCartesianColumnsAvailable)
            {
                return CreateFunction("CartesianToEqDec", coords.XColumnExpression, coords.YColumnExpression, coords.ZColumnExpression);
            }
            else
            {
                // TODO: Figure out from metadata
                throw Error.NoCoordinateColumnFound(coords, "dec");
            }
        }

        public Expression[] GetEqExpressions(TableCoordinates coords)
        {
            return new Expression[]
            {
                GetRAExpression(coords),
                GetDecExpression(coords)
            };
        }

        public Expression GetXExpression(TableCoordinates coords)
        {
            if (coords.IsCartesianHintSpecified)
            {
                return coords.XHintExpression;
            }
            else if (coords.IsEqHintSpecified)
            {
                return CreateFunction("EqToCartesianX", coords.RAHintExpression, coords.DecHintExpression);
            }
            else if (fallBackToDefaultColumns && coords.IsCartesianColumnsAvailable)
            {
                return coords.XColumnExpression;
            }
            else if (fallBackToDefaultColumns && coords.IsEqColumnsAvailable)
            {
                return CreateFunction("EqToCartesianX", coords.RAColumnExpression, coords.DecColumnExpression);
            }
            else
            {
                // TODO: Figure out from metadata
                throw Error.NoCoordinateColumnFound(coords, "cx");
            }
        }

        public Expression GetYExpression(TableCoordinates coords)
        {
            if (coords.IsCartesianHintSpecified)
            {
                return coords.YHintExpression;
            }
            else if (coords.IsEqHintSpecified)
            {
                return CreateFunction("EqToCartesianY", coords.RAHintExpression, coords.DecHintExpression);
            }
            else if (fallBackToDefaultColumns && coords.IsCartesianColumnsAvailable)
            {
                return coords.YColumnExpression;
            }
            else if (fallBackToDefaultColumns && coords.IsEqColumnsAvailable)
            {
                return CreateFunction("EqToCartesianY", coords.RAColumnExpression, coords.DecColumnExpression);
            }
            else
            {
                // TODO: Figure out from metadata
                throw Error.NoCoordinateColumnFound(coords, "cy");
            }
        }

        public Expression GetZExpression(TableCoordinates coords)
        {
            if (coords.IsCartesianHintSpecified)
            {
                return coords.ZHintExpression;
            }
            else if (coords.IsEqHintSpecified)
            {
                return CreateFunction("EqToCartesianZ", coords.RAHintExpression, coords.DecHintExpression);
            }
            else if (fallBackToDefaultColumns && coords.IsCartesianColumnsAvailable)
            {
                return coords.ZColumnExpression;
            }
            else if (fallBackToDefaultColumns && coords.IsEqColumnsAvailable)
            {
                return CreateFunction("EqToCartesianZ", coords.RAColumnExpression, coords.DecColumnExpression);
            }
            else
            {
                // TODO: Figure out from metadata
                throw Error.NoCoordinateColumnFound(coords, "cz");
            }
        }

        public Expression[] GetXyzExpressions(TableCoordinates coords)
        {
            return new Expression[]
            {
                GetXExpression(coords),
                GetYExpression(coords),
                GetZExpression(coords)
            };
        }

        public Expression GetHtmIdExpression(TableCoordinates coords)
        {
            if (coords.IsHtmIdHintSpecified)
            {
                return coords.HtmIdHintExpression;
            }
            else if (fallBackToDefaultColumns && coords.IsHtmIdColumnAvailable)
            {
                return coords.HtmIdColumnExpression;
            }
            else if (coords.IsEqHintSpecified)
            {
                return CreateFunction("htmid", "FromEq", coords.RAHintExpression, coords.DecHintExpression);
            }
            else if (coords.IsCartesianHintSpecified)
            {
                return CreateFunction("htmid", "FromXyz", coords.XHintExpression, coords.YHintExpression, coords.ZHintExpression);
            }
            else if (fallBackToDefaultColumns && coords.IsEqColumnsAvailable)
            {
                return CreateFunction("htmid", "FromEq", coords.RAColumnExpression, coords.DecColumnExpression);
            }
            else if (fallBackToDefaultColumns && coords.IsCartesianColumnsAvailable)
            {
                return CreateFunction("htmid", "FromXyz", coords.XColumnExpression, coords.YColumnExpression, coords.ZColumnExpression);
            }
            else
            {
                // TODO: Figure out from metadata
                throw Error.NoCoordinateColumnFound(coords, "htmid");
            }
        }

        #endregion
        #region Index selector functions

        /// <summary>
        /// Attempt to find an index that has an HTM ID in is as the first key column
        /// </summary>
        /// <returns></returns>
        public Index FindHtmIndex(TableCoordinates coords)
        {
            Index idx = null;

            if (coords.IsHtmIdHintSpecified)
            {
                var cr = coords.HtmIdHintExpression.FindDescendant<AnyVariable>().FindDescendant<ColumnIdentifier>().ColumnReference;
                idx = FindIndexWithFirstKey(coords.Table, cr.ColumnName);
            }
            else if (fallBackToDefaultColumns && coords.IsHtmIdColumnAvailable)
            {
                idx = FindIndexWithFirstKey(coords.Table, TableCoordinates.HtmIdColumnName);
            }

            return idx;
        }

        protected Index FindIndexWithFirstKey(ITableSource table, string columnName)
        {
            var tr = table.TableReference;

            if (tr.DatabaseObject == null)
            {
                return null;
            }

            var t = (TableOrView)tr.DatabaseObject;
            return t.FindIndexWithFirstKey(columnName);
        }

        #endregion
        #region Table statistics

        public override SqlCommand GetTableStatisticsCommand(ITableSource tableSource, DatasetBase statisticsDataset)
        {
            SqlCommand cmd;
            var ts = (CoordinatesTableSource)tableSource;
            var coords = ts.Coordinates;
            var qs = ts.FindAscendant<RegionQuerySpecification>();

            if (qs.Region != null && coords != null && !coords.IsNoRegion)
            {
                cmd = this.GetTableStatisticsWithRegionCommand(tableSource, statisticsDataset);
            }
            else
            {
                cmd = base.GetTableStatisticsCommand(tableSource, statisticsDataset);
            }

            return cmd;
        }

        /// <summary>
        /// Returns a command to gather table statistics with region
        /// </summary>
        /// <param name="tableSource"></param>
        /// <returns></returns>
        private SqlCommand GetTableStatisticsWithRegionCommand(ITableSource tableSource, DatasetBase statisticsDataset)
        {
            if (!queryObject.TableStatistics.ContainsKey(tableSource.UniqueKey))
            {
                throw new InvalidOperationException();
            }

            if (!(tableSource.TableReference.DatabaseObject is TableOrView))
            {
                throw new ArgumentException();
            }

            var ts = (CoordinatesTableSource)tableSource;
            var qs = ts.FindAscendant<RegionQuerySpecification>();
            var region = qs.Region;

            // There are three options to generate a region-aware statistics query for a table
            // 1. the table has an HTM column
            // 2. the table doesn't have an HTM column but has coordinates
            // 3. the region constraint doesn't apply to the table

            var table = (TableOrView)tableSource.TableReference.DatabaseObject;
            var coords = ((SkyQuery.Parser.CoordinatesTableSource)tableSource).Coordinates;

            if (coords == null || coords.IsNoRegion)
            {
                // The region contraint doesn't apply to the table
                return base.GetTableStatisticsCommand(tableSource, statisticsDataset);
            }

            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings(tableSource);
                AddSourceTableMappings(queryObject.Parameters.StatDatabaseVersionName, queryObject.Parameters.SourceDatabaseVersionName);
                AddOutputTableMappings();
            }

            var options = new AugmentedTableQueryOptions(null, (SkyQuery.Parser.CoordinatesTableSource)tableSource, region)
            {
                UseHtm = coords.IsHtmIdHintSpecified || fallBackToDefaultColumns && coords.IsHtmIdColumnAvailable,
                UsePartitioning = false,
                EscapeColumnNames = false,
                ColumnContext = ColumnContext.None,
            };
            var query = GenerateAugmentedTableQuery(options);

            // Statistics key needs to be changed because table is aliased
            var stattable = MapTableReference(tableSource.TableReference);
            var tr = new TableReference(stattable)
            {
                Alias = "__t",
                DatasetName = null,
                DatabaseName = null,
                SchemaName = null,
                DatabaseObjectName = null,
            };
            TableReferenceMap.Add(stattable, tr);

            var keycol = queryObject.TableStatistics[tableSource.UniqueKey].KeyColumn;
            SubstituteSystemDatabaseNames(keycol);
            AddColumnTableReferenceMappings(keycol, tableSource.TableReference, tr);

            var sql = new StringBuilder(RegionScripts.TableStatistics);

            sql.Replace("[$query]", query.ToString());
            sql.Replace("[$keycol]", Execute(keycol));
            SubstituteTableStatisticsQueryTokens(sql, tableSource);

            var cmd = new SqlCommand(sql.ToString());
            AppendRegionParameter(cmd, region);
            AppendTableStatisticsCommandParameters(tableSource, cmd);

            return cmd;
        }

        protected override WhereClause GetTableSpecificWhereClause(ITableSource tableSource)
        {
            return GetTableSpecificWhereClause(tableSource, true);
        }

        protected WhereClause GetTableSpecificWhereClause(ITableSource tableSource, bool useRegion)
        {
            var ts = (CoordinatesTableSource)tableSource;
            var coords = ts.Coordinates;
            var qs = ts.FindAscendant<RegionQuerySpecification>();
            var region = qs.Region;
            var where = base.GetTableSpecificWhereClause(ts);

            if (region == null)
            {
                return where;
            }

            if (useRegion && coords != null && !coords.IsNoRegion && !coords.IsHtmIdHintSpecified)
            {
                // If coords are null we cannot filter the table by regions
                // If htmID is specified for the table, we use HTM-based filtering                

                // In this case, no HTM ID columns is specified so we have to use coordinates
                // and function calls to apply region filter

                var sc = GenerateRegionContainsCondition(ts, -1);

                if (where == null)
                {
                    where = WhereClause.Create(sc);
                }
                else
                {
                    where.AppendCondition(sc, "AND");
                }
            }

            return where;
        }

        protected void AppendRegionParameter(SqlCommand cmd, Spherical.Region region)
        {
            cmd.Parameters.Add(regionParameterName, SqlDbType.VarBinary).Value = region == null ? System.Data.SqlTypes.SqlBytes.Null : region.ToSqlBytes();
        }

        protected void AppendRegionParameters(SourceQuery source, List<Spherical.Region> regions)
        {
            for (int i = 0; i < regions.Count; i++)
            {
                source.Parameters.Add(regionParameterName + i.ToString(), regions[i].ToSqlBytes());
            }
        }

        #endregion
    }
}
