using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    public class RegionQueryRewriter : PartitionedSqlQueryRewriter
    {

        #region Properties

        private RegionQueryPartition Partition
        {
            get { return queryObject as RegionQueryPartition; }
        }

        #endregion
        #region Constructors and initializers

        public RegionQueryRewriter(QueryObject queryObject)
            :base(queryObject)
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
        }

        #endregion

        /// <summary>
        /// Removes non-standard SQL clauses from the parsing tree.
        /// </summary>
        /// <param name="qs"></param>
        protected override void RemoveNonStandardTokens(QuerySpecification qs)
        {
            foreach (var ts in qs.EnumerateDescendantsRecursive<CoordinatesTableSource>())
            {
                // TODO: update this to leave standard SQL Server hints?
                var hint = ts.FindDescendant<Jhu.Graywulf.Sql.Parsing.TableHintClause>();

                if (hint != null)
                {
                    hint.Parent.Stack.Remove(hint);
                }
            }

            // Strip off region clause
            var region = qs.FindDescendant<RegionClause>();
            if (region != null)
            {
                region.Parent.Stack.Remove(region);
            }

            base.RemoveNonStandardTokens(qs);
        }

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
                    var ts = from.FindDescendantRecursive<TableSourceSpecification>();
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
        private void AppendRegionJoinsAndConditions(TableSourceSpecification tableSource, int qsi, ref int tsi)
        {
            // Descend on table sources and append htm constraint to each of them
            var rts = tableSource.FindDescendant<TableSourceSpecification>();
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

                    throw new NotImplementedException();
                    // TODO: review
                    /*
                    tr.InterpretTableSource(sqts);

                    // Replace table source with subquery
                    ts.ExchangeWith(sqts);
                    */

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
            var ts2 = TableSourceSpecification.Create(ts);
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

        private Jhu.Graywulf.Sql.Parsing.TableSourceSpecification GenerateHtmCoverFunctionCall(int qsi)
        {
            var fr = new FunctionReference()
            {
                DatabaseName = CodeDataset.DatabaseName,
                SchemaName = "htm",
                DatabaseObjectName = "Cover"
            };
            var udt = regionUdtVariableName + qsi.ToString();
            var var = Graywulf.Sql.Parsing.UserVariable.Create(udt);
            var exp = Expression.Create(var);
            var fc = TableValuedFunctionCall.Create(fr, exp);
            var fts = FunctionTableSource.Create(fc, "__htm" + qsi.ToString());
            var ts = TableSourceSpecification.Create(fts);

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

            var htmidstart = new ColumnReference(null, htmTable, "HtmIDStart", new DataTypeReference(DataTypes.SqlBigInt));
            var htmidend = new ColumnReference(null, htmTable, "HtmIDEnd", new DataTypeReference(DataTypes.SqlBigInt));

            var limitsp = Jhu.Graywulf.Sql.Parsing.Predicate.CreateBetween(
                GetHtmIdExpression(coords),
                Expression.Create(ColumnIdentifier.Create(htmidstart)),
                Expression.Create(ColumnIdentifier.Create(htmidend)));
            var limitssc = Jhu.Graywulf.Sql.Parsing.BooleanExpression.Create(false, limitsp);

            var htmpartial = new ColumnReference(null, htmTable, "Partial", new DataTypeReference(DataTypes.SqlBit));
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

        protected BooleanExpression GenerateRegionContainsCondition(CoordinatesTableSource ts, int qsi)
        {
            var vr = new VariableReference();
            var mr = new MethodReference();
            Expression[] args;
            var coords = ts.Coordinates;

            if (qsi >= 0)
            {
                vr.VariableName = regionUdtVariableName + qsi.ToString();
            }
            else
            {
                vr.VariableName = regionUdtVariableName;
            }

            if (coords.IsCartesianHintSpecified ||
                fallBackToDefaultColumns && !coords.IsEqHintSpecified && coords.IsCartesianColumnsAvailable)
            {
                mr.MethodName = "ContainsXyz";
                args = GetXyzExpressions(coords);
            }
            else if (coords.IsEqHintSpecified ||
                fallBackToDefaultColumns && !coords.IsCartesianHintSpecified && coords.IsEqColumnsAvailable)
            {
                mr.MethodName = "ContainsEq";
                args = GetEqExpressions(coords);
            }
            else
            {
                // TODO: use metadata?
                throw Error.NoCoordinateColumnsFound(coords);
            }

            var mc = Expression.Create(vr, mr, args);
            var p = Predicate.CreateEquals(mc, Expression.CreateNumber("1"));
            var sc = BooleanExpression.Create(false, p);

            return sc;
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

            return Expression.Create(ScalarFunctionCall.Create(fr, args));
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
    }
}
