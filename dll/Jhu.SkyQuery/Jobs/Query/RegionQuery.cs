using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    [DataContract(Name = "Query", Namespace = "")]
    public class RegionQuery : SqlQuery
    {
        #region Private member variables

        [NonSerialized]
        protected Jhu.Spherical.Region region;

        #endregion
        #region Properties

        [IgnoreDataMember]
        protected new RegionQueryCodeGenerator CodeGenerator
        {
            get
            {
                return new RegionQueryCodeGenerator()
                {
                    ResolveNames = true
                };
            }
        }

        #endregion
        #region Constructors and initializer

        protected RegionQuery()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public RegionQuery(RegionQuery old)
            : base(old)
        {
            CopyMembers(old);
        }

        public RegionQuery(Context context)
            : base(context)
        {
            InitializeMembers(new StreamingContext());
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
        }

        private void CopyMembers(RegionQuery old)
        {
        }

        public override object Clone()
        {
            return new RegionQuery(this);
        }

        #endregion

        protected override void FinishInterpret(bool forceReinitialize)
        {
            region = InterpretRegion(SelectStatement);

            base.FinishInterpret(forceReinitialize);
        }

        internal static Spherical.Region InterpretRegion(Jhu.Graywulf.SqlParser.SelectStatement selectStatement)
        {
            var qs = (RegionQuerySpecification)selectStatement.EnumerateQuerySpecifications().First();
            var rc = qs.FindDescendant<RegionClause>();

            if (rc != null)
            {
                if (rc.IsUri)
                {
                    // TODO: implement region fetch
                    // need to do it once per query, not per partition?
                    throw new NotImplementedException();
                }
                if (rc.IsString)
                {
                    var p = new Jhu.Spherical.Parser.Parser(rc.RegionString);
                    return p.ParseRegion();
                }
                else
                {
                    // TODO: implement direct region grammar
                    throw new NotImplementedException();
                }
            }
            else
            {
                return null;
            }
        }

        protected override SqlCommand GetTableStatisticsCommand(ITableSource tableSource)
        {
            if (region == null)
            {
                return base.GetTableStatisticsCommand(tableSource);
            }
            else
            {
                return GetTableStatisticsWithRegionCommand(tableSource);
            }
        }

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

            // There are three options to generate a region-aware statistics query for a table
            // 1. the table has an HTM column
            // 2. the table doesn't have an HTM column but has coordinates
            // 3. the region constraint doesn't apply to the table

            var table = (TableOrView)tableSource.TableReference.DatabaseObject;
            var coords = ((SkyQuery.Parser.SimpleTableSource)tableSource).Coordinates;
            
            var keycol = tableSource.TableReference.Statistics.KeyColumn;
            var keytype = tableSource.TableReference.Statistics.KeyColumnDataType.NameWithLength;
            var tablename = CodeGenerator.GetEscapedUniqueName(tableSource.TableReference);
            var temptable = GetTemporaryTable("stat_" + tablename);
            
            
            StringBuilder sql;

            if (coords == null || coords.IsNoRegion)
            {
                // The region contraint doesn't apply to the table
                return base.GetTableStatisticsCommand(tableSource);
            }
            else if (coords.IsHtmIdSpecified)
            {
                var htmtable = GetTemporaryTable("htm_" + tablename);

                sql = new StringBuilder(RegionScripts.TableStatistics);

                sql.Replace("[$htm]", CodeGenerator.GetResolvedTableName(htmtable));
                sql.Replace("[$htmid]", ((SkyQuery.Parser.SimpleTableSource)tableSource).Coordinates.GetHtmIdString());
            }
            else
            {
                sql = new StringBuilder(RegionScripts.TableStatisticsNoHtm);
            }

            sql.Replace("[$temptable]", CodeGenerator.GetResolvedTableName(temptable));
            sql.Replace("[$keytype]", keytype);
            sql.Replace("[$keycol]", keycol);
            sql.Replace("[$tablename]", CodeGenerator.GetResolvedTableNameWithAlias(tableSource.TableReference));
            sql.Replace("[$where]", GetTableStatisticsWhereClause(tableSource));

            var cmd = new SqlCommand(sql.ToString());

            var par = cmd.Parameters.Add("@region", SqlDbType.VarBinary).Value = region.ToSqlBytes();

            return cmd;
        }

        protected override string GetTableStatisticsWhereClause(ITableSource tableSource)
        {
            var where = base.GetTableStatisticsWhereClause(tableSource);

            if (region == null || !(tableSource is SkyQuery.Parser.SimpleTableSource))
            {
                return where;
            }

            var coords = ((SkyQuery.Parser.SimpleTableSource)tableSource).Coordinates;

            // Append search criterium to filter for containment
            string htmwhere;

            if (coords == null || coords.IsNoRegion || coords.IsHtmIdSpecified)
            {
                // If coords are null we cannot filter the table by regions
                // If htmID is specified for the table, we use HTM-based filtering                
                return where;
            }
            else
            {
                htmwhere = CodeGenerator.GetRegionConditionText((SkyQuery.Parser.SimpleTableSource)tableSource, CodeDataset);
            }

            if (!String.IsNullOrWhiteSpace(where))
            {
                where += " AND " + htmwhere;
            }
            else
            {
                where = "WHERE " + htmwhere;
            }

            return where;
        }
    }
}
