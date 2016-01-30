using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.SqlCodeGen;
using Jhu.Graywulf.SqlCodeGen.SqlServer;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.IO.Tasks;
using Jhu.SkyQuery.Parser;
using Jhu.Spherical;

namespace Jhu.SkyQuery.Jobs.Query
{
    public class RegionQueryCodeGenerator : Jhu.Graywulf.Jobs.Query.SqlQueryCodeGenerator
    {
        #region Constants

        protected const string regionParameterName = "@region";
        protected const string regionUdtVariableName = "@r";

        #endregion
        #region Properties

        private RegionQueryPartition Partition
        {
            get { return queryObject as RegionQueryPartition; }
        }

        #endregion
        #region Constructors and initializers

        public RegionQueryCodeGenerator()
        {
        }

        public RegionQueryCodeGenerator(QueryObject queryObject)
            : base(queryObject)
        {
        }

        #endregion
        #region Basic query rewrite functions

        /// <summary>
        /// Generates a SQL script based on the query by replacing the REGION clause
        /// with a standard SQL implementation based on HTM indexing and spherical functions
        /// </summary>
        /// <param name="selectStatement"></param>
        /// <returns></returns>
        protected override SourceTableQuery OnGetExecuteQuery(Graywulf.SqlParser.SelectStatement selectStatement)
        {
            if (!(selectStatement is RegionSelectStatement))
            {
                return base.OnGetExecuteQuery(selectStatement);
            }

            var regions = new List<Region>();

            // Rewrite query
            AppendRegionJoinsAndConditions(selectStatement, regions);
            RemoveNonStandardTokens(selectStatement);

            if (queryObject != null)
            {
                SubstituteServerSpecificDatabaseNames(selectStatement);
                SubstituteRemoteTableNames(selectStatement);
            }

            // Compose final script
            var header = new StringBuilder();

            for (int i = 0; i < regions.Count; i++)
            {
                header.AppendLine(String.Format("DECLARE @r{0} dbo.Region = @region{0};", i));
            }

            var source = new SourceTableQuery()
            {
                Header = header.ToString(),
                Query = Execute(selectStatement),
            };

            if (queryObject != null)
            {
                // Return a table source query
                source.Dataset = queryObject.TemporaryDataset;

                AppendPartitioningConditionParameters(source);
                AppendRegionParameters(source, regions);
            }

            return source;
        }

        /// <summary>
        /// Removes non-standard SQL clauses from the parsing tree.
        /// </summary>
        /// <param name="qs"></param>
        protected override void RemoveNonStandardTokens(Graywulf.SqlParser.QuerySpecification qs)
        {
            foreach (var ts in qs.EnumerateDescendantsRecursive<SkyQuery.Parser.SimpleTableSource>())
            {
                // TODO: update this to leave standard SQL Server hints?
                var hint = ts.FindDescendant<Jhu.Graywulf.SqlParser.TableHintClause>();

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

        private void AppendRegionJoinsAndConditions(Graywulf.SqlParser.SelectStatement selectStatement, List<Region> regions)
        {
            int qsi = 0;
            int tsi = 0;

            var qe = selectStatement.QueryExpression;
            AppendRegionJoinsAndConditions(qe, regions, ref qsi, ref tsi);
        }

        private void AppendRegionJoinsAndConditions(Graywulf.SqlParser.QueryExpressionBrackets qeb, List<Region> regions, ref int qsi, ref int tsi)
        {
            var qee = qeb.FindDescendant<Jhu.Graywulf.SqlParser.QueryExpression>();
            AppendRegionJoinsAndConditions(qee, regions, ref qsi, ref tsi);
        }

        private void AppendRegionJoinsAndConditions(Graywulf.SqlParser.QueryExpression queryExpression, List<Region> regions, ref int qsi, ref int tsi)
        {
            // QueryExpression can have two children of type QueryExpression,
            // on in brackets and another one after a query operator.

            // Recursive calls on children

            // QueryExpression
            var qee = queryExpression.FindDescendant<Jhu.Graywulf.SqlParser.QueryExpression>();
            if (qee != null)
            {
                AppendRegionJoinsAndConditions(qee, regions, ref qsi, ref tsi);
            }

            // QueryExpressionBrackets
            var qeb = queryExpression.FindDescendant<Jhu.Graywulf.SqlParser.QueryExpressionBrackets>();
            if (qeb != null)
            {
                AppendRegionJoinsAndConditions(qeb, regions, ref qsi, ref tsi);
            }

            // Process query specification, generate inner and partial HTM query part
            var qs = queryExpression.FindDescendant<Jhu.Graywulf.SqlParser.QuerySpecification>();

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
                    var ts = from.FindDescendantRecursive<SkyQuery.Parser.TableSource>();
                    if (ts != null)
                    {
                        AppendRegionJoinsAndConditions(ts, qsi, ref tsi);
                    }

                    qsi++;
                }
            }
        }

        private void AppendRegionJoinsAndConditions(SkyQuery.Parser.TableSource tableSource, int qsi, ref int tsi)
        {
            // Descend on table sources and append htm constraint to each of them
            var rts = tableSource.FindDescendant<SkyQuery.Parser.TableSource>();
            if (rts != null)
            {
                AppendRegionJoinsAndConditions(rts, qsi, ref tsi);
            }

            if (tableSource.SpecificTableSource is SkyQuery.Parser.SimpleTableSource)
            {
                var ts = (SkyQuery.Parser.SimpleTableSource)tableSource.SpecificTableSource;
                var coords = ts.Coordinates;

                if (coords.IsHtmIdSpecified)
                {
                    var qs1 = GenerateHtmJoinTableQuerySpecification(ts, qsi, false);
                    var qs2 = GenerateHtmJoinTableQuerySpecification(ts, qsi, true);

                    var qe = Jhu.Graywulf.SqlParser.QueryExpression.Create(qs1, qs2, QueryOperator.CreateUnionAll());

                    // Wrap is up into a subquery
                    var ss = Jhu.Graywulf.SqlParser.SelectStatement.Create(qe);
                    var sqts = Jhu.Graywulf.SqlParser.SubqueryTableSource.Create(ss, "__htm_t" + tsi.ToString());

                    // Create a copy of the original table reference, substitute subquery with the new one
                    // and rewrite the old one to point to the subquery
                    var tr = ts.TableReference;
                    var ntr = new TableReference(tr);

                    // Replace table reference on subtree to a new one
                    // that will point to the original table
                    SubstituteTableReference(ss, tr, ntr);

                    // Rewrite original table reference to point to
                    // the subquery
                    ss.QueryExpression.TableReference = tr;
                    tr.InterpretTableSource(sqts);

                    // Replace table source with subquery
                    ts.ExchangeWith(sqts);

                    tsi++;
                }
                else if (coords.IsEqSpecified || coords.IsCartesianSpecified)
                {
                    var sc = GenerateRegionContainsCondition(ts, qsi);
                    var qs = ts.FindAscendant<Jhu.Graywulf.SqlParser.QuerySpecification>();
                    qs.AppendSearchCondition(sc, "AND");
                }
            }
        }

        private Jhu.Graywulf.SqlParser.QuerySpecification GenerateHtmJoinTableQuerySpecification(SkyQuery.Parser.SimpleTableSource ts, int qsi, bool partial)
        {
            var coords = ts.Coordinates;

            var fts = GenerateHtmCoverFunctionCall(qsi);
            var ts2 = SkyQuery.Parser.TableSource.Create(ts);
            var jc = GenerateHtmJoinCondition(ts, fts.TableReference, partial, qsi);
            var jt = Graywulf.SqlParser.JoinedTable.Create(JoinType.CreateInnerLoop(), ts2, jc);
            var tse = Jhu.Graywulf.SqlParser.TableSourceExpression.Create(fts, jt);
            var fro = Jhu.Graywulf.SqlParser.FromClause.Create(tse);
            var qs = Jhu.Graywulf.SqlParser.QuerySpecification.Create(SelectList.CreateStar(ts.TableReference), fro);

            return qs;
        }

        #endregion
        #region HTM search and join conditions generation

        private Jhu.Graywulf.SqlParser.TableSource GenerateHtmCoverFunctionCall(int qsi)
        {
            var fr = new FunctionReference()
            {
                DatabaseName = CodeDataset.DatabaseName,
                SchemaName = "htm",
                DatabaseObjectName = "Cover"
            };
            var udt = regionUdtVariableName + qsi.ToString();
            var var = Graywulf.SqlParser.Variable.Create(udt);
            var exp = Expression.Create(var);
            var fc = TableValuedFunctionCall.Create(fr, exp);
            var fts = Graywulf.SqlParser.FunctionTableSource.Create(fc, "__htm" + qsi.ToString());
            var ts = SkyQuery.Parser.TableSource.Create(fts);

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
        protected Jhu.Graywulf.SqlParser.SearchCondition GenerateHtmJoinCondition(SkyQuery.Parser.SimpleTableSource ts, TableReference htmTable, bool partial, int qsi)
        {
            var coords = ts.Coordinates;

            var htmidstart = new ColumnReference(htmTable, "HtmIDStart", DataTypes.SqlBigInt);
            var htmidend = new ColumnReference(htmTable, "HtmIDEnd", DataTypes.SqlBigInt);

            var limitsp = Jhu.Graywulf.SqlParser.Predicate.CreateBetween(
                GetHtmIdExpression(coords),
                Expression.Create(ColumnIdentifier.Create(htmidstart)),
                Expression.Create(ColumnIdentifier.Create(htmidend)));
            var limitssc = Jhu.Graywulf.SqlParser.SearchCondition.Create(false, limitsp);

            var htmpartial = new ColumnReference(htmTable, "Partial", DataTypes.SqlBit);
            var partialp = Jhu.Graywulf.SqlParser.Predicate.CreateEquals(
                Expression.Create(htmpartial),
                Expression.CreateNumber(partial ? "1" : "0"));
            var partialsc = Jhu.Graywulf.SqlParser.SearchCondition.Create(false, partialp);

            var sc = Jhu.Graywulf.SqlParser.SearchCondition.Create(limitssc, partialsc, LogicalOperator.CreateAnd());

            // Add precise filtering if necessary
            if (partial && (coords.IsEqSpecified || coords.IsCartesianSpecified))
            {
                var rcc = GenerateRegionContainsCondition(ts, qsi);
                sc = Jhu.Graywulf.SqlParser.SearchCondition.Create(sc, rcc, LogicalOperator.CreateAnd());
            }

            return sc;
        }

        protected Jhu.Graywulf.SqlParser.SearchCondition GenerateRegionContainsCondition(SkyQuery.Parser.SimpleTableSource ts, int qsi)
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

            if (coords.IsCartesianSpecified)
            {
                udtf = UdtFunctionCall.Create(udt, "ContainsXyz", GetXyzExpressions(coords));
            }
            else if (coords.IsEqSpecified)
            {
                udtf = UdtFunctionCall.Create(udt, "ContainsEq", GetEqExpressions(coords));
            }
            else
            {
                throw new InvalidOperationException();  // *** TODO
            }

            var p = Jhu.Graywulf.SqlParser.Predicate.CreateEquals(Expression.Create(udtf), Expression.CreateNumber("1"));
            var sc = Jhu.Graywulf.SqlParser.SearchCondition.Create(false, p);

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
        protected virtual StringBuilder GenerateAugmentedTableQuery(AugmentedTableQueryOptions options)
        {
            StringBuilder sql;

            var coords = options.Table.Coordinates;
            Jhu.Graywulf.SqlParser.WhereClause where = null;
            Jhu.Graywulf.SqlParser.WhereClause whereregion = null;

            // 1. Generate column list

            var columnlist = new SqlServerColumnListGenerator(
                options.Table.TableReference,
                options.ColumnContext,
                options.EscapeColumnNames ?
                    ColumnListType.ForSelectWithOriginalName :
                    ColumnListType.ForSelectWithOriginalNameNoAlias)
            {
                TableAlias = null,
                ColumnContext = options.ColumnContext,
                LeadingComma = true,
            };

            // 2. Figure out where clause

            if (options.UseConditions)
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
                        where = Jhu.Graywulf.SqlParser.WhereClause.Create(pc);
                    }
                }
            }

            // 3. Add explicit region filter, if necessary

            if (options.Region != null && options.UseRegion)
            {
                // Filter on region containment
                var rc = GenerateRegionContainsCondition(options.Table, -1);

                if (rc != null)
                {
                    if (where != null)
                    {
                        whereregion = (Jhu.Graywulf.SqlParser.WhereClause)where.Clone();
                        whereregion.AppendCondition(rc, "AND");
                    }
                    else
                    {
                        whereregion = Jhu.Graywulf.SqlParser.WhereClause.Create(rc);
                    }
                }
            }
            else
            {
                whereregion = where;
            }

            // 4. Load and rewrite SQL template

            if (options.Region != null && options.UseRegion && options.Table.Coordinates.IsHtmIdSpecified && options.UseHtm)
            {
                sql = GetSelectAugmentedTableHtmTemplate();

                SubstituteHtmId(sql, coords);

                sql.Replace("[$where_inner]", Execute(where));
                sql.Replace("[$where_partial]", Execute(whereregion));
            }
            else
            {
                sql = GetSelectAugmentedTableTemplate();

                sql.Replace("[$where]", Execute(whereregion));
            }

            // 5. Substitute remote table name, if necessary
            var tr = new TableReference(options.Table.TableReference);
            SubstituteRemoteTableName(tr);

            // Substitute additional tokens
            sql.Replace("[$tablename]", GetResolvedTableNameWithAlias(tr));
            sql.Replace("[$columnlist]", columnlist.GetColumnListString());

            SubstituteAugmentedTableColumns(sql, options);

            return sql;
        }

        protected virtual void SubstituteAugmentedTableColumns(StringBuilder sql, AugmentedTableQueryOptions options)
        {
        }

        protected void SubstituteHtmId(StringBuilder sql, TableCoordinates coords)
        {
            var htmex = GetHtmIdExpression(coords);
            SubstituteSystemDatabaseNames(htmex);
            sql.Replace("[$htmid]", Execute(htmex));
        }

        #endregion
        #region Coordinate column functions

        public Expression GetRAExpression(TableCoordinates coords)
        {
            if (coords.IsEqSpecified)
            {
                return coords.RAExpression;
            }
            else if (coords.IsCartesianSpecified)
            {
                var fr = new FunctionReference()
                {
                    DatabaseName = CodeDataset.DatabaseName,
                    SchemaName = Constants.SkyQueryFunctionSchema,
                    DatabaseObjectName = "CartesianToEqRa"
                };

                return Expression.Create(FunctionCall.Create(fr, coords.XExpression, coords.YExpression, coords.ZExpression));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        public Expression GetDecExpression(TableCoordinates coords)
        {
            if (coords.IsEqSpecified)
            {
                return coords.DecExpression;
            }
            else if (coords.IsCartesianSpecified)
            {
                var fr = new FunctionReference()
                {
                    DatabaseName = CodeDataset.DatabaseName,
                    SchemaName = Constants.SkyQueryFunctionSchema,
                    DatabaseObjectName = "CartesianToEqDec"
                };

                return Expression.Create(FunctionCall.Create(fr, coords.XExpression, coords.YExpression, coords.ZExpression));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
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
            if (coords.IsCartesianSpecified)
            {
                return coords.XExpression;
            }
            else if (coords.IsEqSpecified)
            {
                var fr = new FunctionReference()
                {
                    DatabaseName = CodeDataset.DatabaseName,
                    SchemaName = Constants.SkyQueryFunctionSchema,
                    DatabaseObjectName = "EqToCartesianX"
                };

                return Expression.Create(FunctionCall.Create(fr, coords.RAExpression, coords.DecExpression));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        public Expression GetYExpression(TableCoordinates coords)
        {
            if (coords.IsCartesianSpecified)
            {
                return coords.YExpression;
            }
            else if (coords.IsEqSpecified)
            {
                var fr = new FunctionReference()
                {
                    DatabaseName = CodeDataset.DatabaseName,
                    SchemaName = Constants.SkyQueryFunctionSchema,
                    DatabaseObjectName = "EqToCartesianY"
                };

                return Expression.Create(FunctionCall.Create(fr, coords.RAExpression, coords.DecExpression));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        public Expression GetZExpression(TableCoordinates coords)
        {
            if (coords.IsCartesianSpecified)
            {
                return coords.ZExpression;
            }
            else if (coords.IsEqSpecified)
            {
                var fr = new FunctionReference()
                {
                    DatabaseName = CodeDataset.DatabaseName,
                    SchemaName = Constants.SkyQueryFunctionSchema,
                    DatabaseObjectName = "EqToCartesianZ"
                };

                return Expression.Create(FunctionCall.Create(fr, coords.RAExpression, coords.DecExpression));
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
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
            if (coords.IsHtmIdSpecified)
            {
                return coords.HtmIdExpression;
            }
            else if (coords.IsEqSpecified)
            {
                var fr = new FunctionReference()
                {
                    DatabaseName = CodeDataset.DatabaseName,
                    SchemaName = "htmid",
                    DatabaseObjectName = "FromEq"
                };

                var fc = FunctionCall.Create(
                    fr,
                    coords.RAExpression,
                    coords.DecExpression);

                return Expression.Create(fc);
            }
            else if (coords.IsCartesianSpecified)
            {
                var fr = new FunctionReference()
                {
                    DatabaseName = CodeDataset.DatabaseName,
                    SchemaName = "htmid",
                    DatabaseObjectName = "FromXyz"
                };

                var fc = FunctionCall.Create(
                    fr,
                    coords.XExpression,
                    coords.YExpression,
                    coords.ZExpression);

                return Expression.Create(fc);
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        public Expression GetZoneIdExpression(TableCoordinates coords)
        {
            if (coords.IsZoneIdSpecified)
            {
                return coords.ZoneIdExpression;
            }
            else if (coords.IsEqSpecified)
            {
                var fr = new FunctionReference()
                {
                    DatabaseName = CodeDataset.DatabaseName,
                    SchemaName = Constants.SkyQueryFunctionSchema,
                    DatabaseObjectName = "ZoneIDFromDec"
                };

                var fc = FunctionCall.Create(
                    fr,
                    coords.DecExpression,
                    Expression.Create(Jhu.Graywulf.SqlParser.Variable.Create("@H")));

                return Expression.Create(fc);
            }
            else if (coords.IsCartesianSpecified)
            {
                var fr = new FunctionReference()
                {
                    DatabaseName = CodeDataset.DatabaseName,
                    SchemaName = Constants.SkyQueryFunctionSchema,
                    DatabaseObjectName = "ZoneIDFromZ"
                };

                var fc = FunctionCall.Create(
                    fr,
                    coords.ZExpression,
                    Expression.Create(Jhu.Graywulf.SqlParser.Variable.Create("@H")));

                return Expression.Create(fc);
            }
            else
            {
                // TODO: Figure out from metadata
                throw new NotImplementedException();
            }
        }

        #endregion
        #region Table statistics

        public override SqlCommand GetTableStatisticsCommand(ITableSource tableSource)
        {
            var ts = (SkyQuery.Parser.SimpleTableSource)tableSource;
            var coords = ts.Coordinates;
            var qs = ts.FindAscendant<SkyQuery.Parser.QuerySpecification>();

            if (qs is RegionQuerySpecification && ((RegionQuerySpecification)qs).Region != null &&
                coords != null && !coords.IsNoRegion)
            {
                return this.GetTableStatisticsWithRegionCommand(tableSource);
            }
            else
            {
                return base.GetTableStatisticsCommand(tableSource);
            }
        }

        /// <summary>
        /// Returns a command to gather table statistics with region
        /// </summary>
        /// <param name="tableSource"></param>
        /// <returns></returns>
        private SqlCommand GetTableStatisticsWithRegionCommand(ITableSource tableSource)
        {
            if (tableSource.TableReference.Statistics == null)
            {
                throw new ArgumentNullException();
            }

            if (!(tableSource.TableReference.DatabaseObject is TableOrView))
            {
                throw new ArgumentException();
            }

            var ts = (SkyQuery.Parser.SimpleTableSource)tableSource;
            var qs = (RegionQuerySpecification)ts.FindAscendant<SkyQuery.Parser.QuerySpecification>();
            var region = qs.Region;

            // There are three options to generate a region-aware statistics query for a table
            // 1. the table has an HTM column
            // 2. the table doesn't have an HTM column but has coordinates
            // 3. the region constraint doesn't apply to the table

            var table = (TableOrView)tableSource.TableReference.DatabaseObject;
            var coords = ((SkyQuery.Parser.SimpleTableSource)tableSource).Coordinates;

            if (coords == null || coords.IsNoRegion)
            {
                // The region contraint doesn't apply to the table
                return base.GetTableStatisticsCommand(tableSource);
            }

            var options = new AugmentedTableQueryOptions((SkyQuery.Parser.SimpleTableSource)tableSource, region)
            {
                UseHtm = coords.IsHtmIdSpecified,
                UsePartitioning = false,
                EscapeColumnNames = false,
                ColumnContext = ColumnContext.None,
            };
            var query = GenerateAugmentedTableQuery(options);

            // Statistics key needs to be changed because table is aliased
            var tr = new TableReference("__t");
            var keycol = SubstituteTableName(tableSource.TableReference.Statistics.KeyColumn, tableSource.TableReference, tr);
            SubstituteSystemDatabaseNames(keycol);

            var sql = new StringBuilder(RegionScripts.TableStatistics);

            sql.Replace("[$query]", query.ToString());
            sql.Replace("[$keycol]", Execute(keycol));
            SubstituteTableStatisticsQueryTokens(sql, tableSource);

            var cmd = new SqlCommand(sql.ToString());
            AppendRegionParameter(cmd, region);
            AppendTableStatisticsCommandParameters(tableSource, cmd);
            return cmd;
        }

        protected override Jhu.Graywulf.SqlParser.WhereClause GetTableSpecificWhereClause(ITableSource tableSource)
        {
            return GetTableSpecificWhereClause(tableSource, true);
        }

        protected Jhu.Graywulf.SqlParser.WhereClause GetTableSpecificWhereClause(ITableSource tableSource, bool useRegion)
        {
            var ts = (SkyQuery.Parser.SimpleTableSource)tableSource;
            var qs = (RegionQuerySpecification)ts.FindAscendant<SkyQuery.Parser.QuerySpecification>();
            var region = qs.Region;
            var where = base.GetTableSpecificWhereClause(tableSource);

            if (region == null || !(tableSource is SkyQuery.Parser.SimpleTableSource))
            {
                return where;
            }

            var coords = ((SkyQuery.Parser.SimpleTableSource)tableSource).Coordinates;

            if (useRegion && coords != null && !coords.IsNoRegion && !coords.IsHtmIdSpecified)
            {
                // If coords are null we cannot filter the table by regions
                // If htmID is specified for the table, we use HTM-based filtering                

                // In this case, no HTM ID columns is specified so we have to use coordinates
                // and function calls to apply region filter

                var sc = GenerateRegionContainsCondition((SkyQuery.Parser.SimpleTableSource)tableSource, -1);

                if (where == null)
                {
                    where = Jhu.Graywulf.SqlParser.WhereClause.Create(sc);
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

        protected void AppendRegionParameters(SourceTableQuery source, List<Spherical.Region> regions)
        {
            for (int i = 0; i < regions.Count; i++)
            {
                source.Parameters.Add(regionParameterName + i.ToString(), regions[i].ToSqlBytes());
            }
        }

        #endregion
    }
}
