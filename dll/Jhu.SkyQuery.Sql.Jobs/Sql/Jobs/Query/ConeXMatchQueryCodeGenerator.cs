using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Sql.Parsing;
using Jhu.Graywulf.Sql.NameResolution;
using Jhu.Graywulf.Sql.CodeGeneration.SqlServer;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.IO.Tasks;
using Jhu.SkyQuery.Sql.Parsing;
using Jhu.Spherical;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    public class ConeXMatchQueryCodeGenerator : XMatchQueryCodeGenerator
    {
        #region Constructors and initializers

        private ConeXMatchQueryPartition Partition
        {
            get { return queryObject as ConeXMatchQueryPartition; }
        }

        public ConeXMatchQueryCodeGenerator()
        {
        }

        public ConeXMatchQueryCodeGenerator(QueryObject queryObject)
            : base(queryObject)
        {
        }

        #endregion
        #region Source table functions

        protected override StringBuilder GetSelectAugmentedTableTemplate()
        {
            return new StringBuilder(ConeXMatchScripts.SelectAugmentedTable);
        }

        protected override StringBuilder GetSelectAugmentedTableHtmTemplate()
        {
            return new StringBuilder(ConeXMatchScripts.SelectAugmentedTableHtm);
        }

        protected override void OnGenerateAugmentedTableQuery(AugmentedTableQueryOptions options, StringBuilder sql, ref WhereClause where)
        {
            // TODO try to merge as much as possible with bayesian

            base.OnGenerateAugmentedTableQuery(options, sql, ref where);

            var coords = options.Table.Coordinates;

            if (options.Step == null || options.Step.StepNumber > 0)
            {
                sql.Replace("[$radius]", "0");
            }
            else
            {
                var radius = Partition.Query.RegionHintArguments[2];
                sql.Replace("[$radius]", Execute(radius));
            }

            SubstituteZoneId(sql, coords);



            /*
             * // TODO: exclude zero size regions
             * // var error = GetCoordinateErrorExpression(coords);
            if (options.ExcludeZeroWeight && !options.Table.Coordinates.IsConstantError)
            {
                var wc = GenerateExcludeZeroWeightCondition(options.Table);

                if (wc != null)
                {
                    if (where != null)
                    {
                        where.AppendCondition(wc, "AND");
                    }
                    else
                    {
                        where = WhereClause.Create(wc);
                    }
                }
            }*/
        }

        protected override void SubstituteAugmentedTableColumns(StringBuilder sql, AugmentedTableQueryOptions options)
        {
            // TODO try to merge as much as possible with bayesian

            base.SubstituteAugmentedTableColumns(sql, options);

            var coords = options.Table.Coordinates;
            SubstituteCoordinates(sql, coords);
            SubstituteHtmId(sql, coords);
        }

        #endregion
        #region Search radius functions

        public SqlCommand GetComputeSearchRadiusCommand(XMatchQueryStep step)
        {
            var table = Partition.Query.XMatchTables[step.XMatchTable];
            var region = table.Region;
            var radius = Partition.Query.RegionHintArguments[2];
            var sql = new StringBuilder(ConeXMatchScripts.ComputeSearchRadius);

            // TODO: try to merge these calls with the rest
            if (queryObject.Parameters.ExecutionMode == ExecutionMode.Graywulf)
            {
                AddSystemDatabaseMappings();
                AddSourceTableMappings(Partition.Parameters.SourceDatabaseVersionName, null);
            }

            var options = new AugmentedTableQueryOptions(step, table.TableSource, region)
            {
                ColumnContext = ColumnContext.None
            };

            sql.Replace("[$query]", GenerateAugmentedTableQuery(options).ToString());

            var cmd = new SqlCommand(sql.ToString());

            AppendZoneParameters(cmd);
            AppendPartitioningConditionParameters(cmd);
            AppendRegionParameter(cmd, region);

            return cmd;
        }

        #endregion
        #region Match table functions

        protected override string GetCreateMatchTableScript()
        {
            return ConeXMatchScripts.CreateMatchTable;
        }

        protected override string GetPopulateMatchTableScript()
        {
            return ConeXMatchScripts.PopulateMatchTable;
        }

        protected override void AppendPopulateMatchTableParameters(XMatchQueryStep step, SqlCommand cmd)
        {
            
        }

        protected override string GetBuildMatchTableIndexScript()
        {
            return ConeXMatchScripts.BuildMatchTableIndex;
        }

        #endregion
    }
}
