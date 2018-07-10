using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.QueryGeneration;
using Jhu.Graywulf.Sql.QueryGeneration.SqlServer;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.IO.Tasks;
using Jhu.SkyQuery.Sql.Parsing;
using Jhu.Spherical;

namespace Jhu.SkyQuery.Sql.Jobs.Query
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
                var cr = coords.HtmIdHintExpression.FindDescendant<ColumnIdentifier>().ColumnReference;
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
            var coords = ((CoordinatesTableSource)tableSource).Coordinates;

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

            var options = new AugmentedTableQueryOptions(null, (CoordinatesTableSource)tableSource, region)
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
