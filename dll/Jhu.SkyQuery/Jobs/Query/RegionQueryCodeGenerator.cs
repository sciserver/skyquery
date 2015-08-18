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
        public override SourceTableQuery GetExecuteQuery(Jhu.Graywulf.SqlParser.SelectStatement selectStatement)
        {
            if (!(selectStatement is RegionSelectStatement))
            {
                return base.GetExecuteQuery(selectStatement);
            }
            
            var sql = new StringBuilder();
            var htmcreate = new StringBuilder();
            var htmdrop = new StringBuilder();

            /*
            int i = 0;
            foreach (var qs in selectStatement.EnumerateQuerySpecifications())
            {
                if (qs is RegionQuerySpecification)
                {
                    var htmInner = GetHtmTable(i, false);
                    var htmPartial = GetHtmTable(i, true);
                    string create, drop;

                    GenerateHtmTablesScript((RegionQuerySpecification)qs, i, htmInner, htmPartial, out create, out drop);

                    htmcreate.AppendLine(create);
                    htmdrop.AppendLine(drop);
                }
                else
                {
                }

                i++;
            }*/

            sql.AppendLine(htmcreate.ToString());



            sql.AppendLine(htmdrop.ToString());

            var source = new SourceTableQuery()
            {
                Dataset = queryObject.TemporaryDataset,
                Query = sql.ToString()
            };

            AppendPartitioningConditionParameters(source);

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
        #region Region and HTM query support functions

        private void GenerateHtmTablesScript(RegionQuerySpecification qs, int i, TableReference htmInner, TableReference htmPartial, out string create, out string drop)
        {
            /*var sql = new StringBuilder(RegionScripts.ExecuteQuery);

            // Take copies of the parsing tree
            var qsInner = new RegionQuerySpecification(qs);
            RewriteRegionQueryForExecute(qsInner, qs.Region, htmInner, false);
            sql.Replace("[$select_inner]", Execute(qsInner));

            var qsPartial = new RegionQuerySpecification(qs);
            RewriteRegionQueryForExecute(qsPartial, qs.Region, htmPartial, true);
            Execute(qsPartial);
            sql.Replace("[$select_partial]", Execute(qsInner));*/

            create = drop = null;
        }

        private void AppendRegionJoinsAndConditions(Jhu.SkyQuery.Parser.SelectStatement selectStatement, TableReference[] htmInner, TableReference[] htmPartial)
        {
            int qsi = 0;

            var qe = selectStatement.QueryExpression;
            AppendRegionJoinsAndConditions(qe, htmInner, htmPartial, ref qsi);
        }

        private void AppendRegionJoinsAndConditions(Graywulf.SqlParser.QueryExpression queryExpression, TableReference[] htmInner, TableReference[] htmPartial, ref int qsi)
        {
            // QueryExpression can have two children of type QueryExpression,
            // on in brackets and another one after a query operator.

            // Recursive calls on children

            // QueryExpression
            var qee = queryExpression.FindDescendant<Jhu.Graywulf.SqlParser.QueryExpression>();
            if (qee != null)
            {
                AppendRegionJoinsAndConditions(qee, htmInner, htmPartial, ref qsi);
            }

            // QueryExpressionBrackets
            var qeb = queryExpression.FindDescendant<Jhu.Graywulf.SqlParser.QueryExpressionBrackets>();
            if (qeb != null)
            {
                AppendRegionJoinsAndConditions(qeb, htmInner, htmPartial, ref qsi);
            }
            
            // Process query specification, generate inner and partial HTM query part
            var qs = queryExpression.FindDescendant<Jhu.Graywulf.SqlParser.QuerySpecification>();

            if (qs != null)
            {
                var qsinner = new Jhu.Graywulf.SqlParser.QuerySpecification(qs);
                var needinner = AppendRegionJoinsAndConditions(qsinner, htmInner[qsi], false);

                var qspartial = new Jhu.Graywulf.SqlParser.QuerySpecification(qs);
                var needpartial = AppendRegionJoinsAndConditions(qspartial, htmPartial[qsi], true);

                qsi++;

                if (needinner && !needpartial)
                {
                    qs.ExchangeWith(qsinner);

                }
                else if (!needinner && needpartial)
                {
                    qs.ExchangeWith(qspartial);
                }
                else if (needinner && needpartial)
                {
                    // Combine the two with UNION ALL
                    var qen = SkyQuery.Parser.QueryExpression.Create(qsinner, qspartial, QueryOperator.CreateUnionAll());
                    var qenb = SkyQuery.Parser.QueryExpressionBrackets.Create(qen);
                    qs.ExchangeWith(qenb);
                }
            }
        }

        private void AppendRegionJoinsAndConditions(Graywulf.SqlParser.QueryExpressionBrackets qeb, TableReference[] htmInner, TableReference[] htmPartial, ref int qsi)
        {
            var qee = qeb.FindDescendant<Jhu.Graywulf.SqlParser.QueryExpression>();
            AppendRegionJoinsAndConditions(qee, htmInner, htmPartial, ref qsi);
        }

        /// <summary>
        /// Appends HTM joins and filtering conditions to a query specification.
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="htmtable"></param>
        /// <param name="partial"></param>
        /// <returns>Returns true if the query specification needs to be rewritten.</returns>
        private bool AppendRegionJoinsAndConditions(Jhu.Graywulf.SqlParser.QuerySpecification qs, TableReference htmTable, bool partial)
        {
            Jhu.Graywulf.SqlParser.SearchCondition joinConditions = null;
            Jhu.Graywulf.SqlParser.SearchCondition whereConditions = null;

            foreach (var ts in qs.EnumerateDescendantsRecursive<SkyQuery.Parser.SimpleTableSource>())
            {
                var coords = ts.Coordinates;

                if (coords == null || coords.IsNoRegion)
                {
                    // The region contraint doesn't apply to the table
                    continue;
                }

                if (coords.IsHtmIdSpecified)
                {
                    // An HTM column is available, filter based on it
                    var sc = CreateHtmJoinCondition(ts, htmTable);

                    if (joinConditions == null)
                    {
                        joinConditions = sc;
                    }
                    else
                    {
                        joinConditions = Jhu.Graywulf.SqlParser.SearchCondition.Create(sc, joinConditions, LogicalOperator.CreateAnd());
                    }
                }

                if (partial)
                {
                    // No HTM column is avaiable, filter using region.Contains
                    // Also, points in partial HTM trixels need explicit filtering
                    var sc = CreateRegionContainsCondition(ts);

                    if (whereConditions == null)
                    {
                        whereConditions = sc;
                    }
                    else
                    {
                        whereConditions = Jhu.Graywulf.SqlParser.SearchCondition.Create(sc, whereConditions, LogicalOperator.CreateAnd());
                    }
                }
            }

            // Append where condition, if necessary
            if (whereConditions != null)
            {
                qs.AppendSearchCondition(whereConditions, "AND");
            }

            // Append htm table, if necessary
            if (joinConditions != null)
            {
                var jt = Jhu.Graywulf.SqlParser.JoinedTable.Create(
                    JoinType.CreateInner(),
                    Jhu.Graywulf.SqlParser.TableSource.Create(htmTable),
                    joinConditions);

                // Filter using HTM index, add a join clause
                qs.FromClause.AppendJoinedTable(jt);
            }

            return joinConditions != null || whereConditions != null;
        }

        #endregion
        #region HTM search and join conditions generation

        /// <summary>
        /// Create a join condition in the form of
        /// 'htmid BETWEEN htmtable.htmidstart AND htmtable.htmidend'
        /// to be used with tables with HTM index
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="htmTable"></param>
        /// <returns></returns>
        private Jhu.Graywulf.SqlParser.SearchCondition CreateHtmJoinCondition(SkyQuery.Parser.SimpleTableSource ts, TableReference htmTable)
        {
            var coords = ts.Coordinates;

            var htmidstart = new ColumnReference(htmTable, "HtmIDStart", DataTypes.SqlBigInt);
            var htmidend = new ColumnReference(htmTable, "HtmIDEnd", DataTypes.SqlBigInt);

            var p = Jhu.Graywulf.SqlParser.Predicate.CreateBetween(
                coords.GetHtmIdExpression(),
                Expression.Create(ColumnIdentifier.Create(htmidstart)),
                Expression.Create(ColumnIdentifier.Create(htmidend)));

            return Jhu.Graywulf.SqlParser.SearchCondition.Create(false, p);
        }

        private Jhu.Graywulf.SqlParser.SearchCondition CreateRegionContainsCondition(SkyQuery.Parser.SimpleTableSource ts)
        {
            var coords = ts.Coordinates;

            UdtFunctionCall udtf;

            if (coords.IsCartesianSpecified)
            {
                udtf = UdtFunctionCall.Create(regionUdtVariableName, "ContainsXyz", coords.GetXyzExpressions(CodeDataset));
            }
            else if (coords.IsEqSpecified)
            {
                udtf = UdtFunctionCall.Create(regionUdtVariableName, "ContainsEq", coords.GetEqExpressions(CodeDataset));
            }
            else
            {
                throw new InvalidOperationException();  // *** TODO
            }

            var p = Jhu.Graywulf.SqlParser.Predicate.CreateEquals(Expression.Create(udtf), Expression.CreateNumber("1"));
            var sc = Jhu.Graywulf.SqlParser.SearchCondition.Create(false, p);

            return sc;
        }

        public Table GetHtmTable(int i, bool partial)
        {
            return queryObject.GetTemporaryTable(String.Format("Htm_{0}_{1}", i, partial ? "Partial" : "Inner"));
        }

        #endregion
        #region Table statistics

        public override SqlCommand GetTableStatisticsCommand(ITableSource tableSource)
        {    
                var ts = (SkyQuery.Parser.SimpleTableSource)tableSource;
                var qs = ts.FindAscendant<SkyQuery.Parser.QuerySpecification>();

                if (qs is RegionQuerySpecification)
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

            // Get the region
            var ts = (SkyQuery.Parser.SimpleTableSource)tableSource;
            var qs = (RegionQuerySpecification)ts.FindAscendant<SkyQuery.Parser.QuerySpecification>();
            var region = qs.Region;

            // There are three options to generate a region-aware statistics query for a table
            // 1. the table has an HTM column
            // 2. the table doesn't have an HTM column but has coordinates
            // 3. the region constraint doesn't apply to the table

            var table = (TableOrView)tableSource.TableReference.DatabaseObject;
            var coords = ((SkyQuery.Parser.SimpleTableSource)tableSource).Coordinates;
            var tablename = GetEscapedUniqueName(tableSource.TableReference);
            var htmtable = new TableReference(queryObject.GetTemporaryTable("htm_" + tablename), "__htm");

            if (coords == null || coords.IsNoRegion)
            {
                // The region contraint doesn't apply to the table
                return base.GetTableStatisticsCommand(tableSource);
            }

            StringBuilder sql;

            if (coords.IsHtmIdSpecified)
            {
                sql = new StringBuilder(RegionScripts.TableStatistics);

                sql.Replace("[$htm]", GetResolvedTableName(htmtable));
                sql.Replace("[$htmid]", coords.GetHtmIdExpression().ToString());
            }
            else if (coords.IsEqSpecified || coords.IsCartesianSpecified)
            {
                // TODO
                throw new NotImplementedException();
            }
            else
            {
                sql = new StringBuilder(RegionScripts.TableStatisticsNoHtm);
            }

            SubstituteTableStatisticsQueryTokens(sql, tableSource);

            var cmd = new SqlCommand(sql.ToString());
            AppendRegionParameter(cmd, region);
            return cmd;
        }

        protected Jhu.Graywulf.SqlParser.WhereClause GetTableSpecificWhereClause(ITableSource tableSource, Spherical.Region region)
        {
            var where = base.GetTableSpecificWhereClause(tableSource);

            if (region == null || !(tableSource is SkyQuery.Parser.SimpleTableSource))
            {
                return where;
            }

            var coords = ((SkyQuery.Parser.SimpleTableSource)tableSource).Coordinates;

            if (coords != null && !coords.IsNoRegion && !coords.IsHtmIdSpecified)
            {
                // If coords are null we cannot filter the table by regions
                // If htmID is specified for the table, we use HTM-based filtering                

                // In this case, no HTM ID columns is specified so we have to use coordinates
                // and function calls to apply region filter

                var sc = CreateRegionContainsCondition((SkyQuery.Parser.SimpleTableSource)tableSource);

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

        public void AppendRegionParameter(SqlCommand cmd, Spherical.Region region)
        {
            if (region != null)
            {
                cmd.Parameters.Add(regionParameterName, SqlDbType.VarBinary).Value = region.ToSqlBytes();
            }
        }

        #endregion
    }
}
