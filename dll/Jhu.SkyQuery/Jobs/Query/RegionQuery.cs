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
            var xmqs = (XMatchQuerySpecification)selectStatement.EnumerateQuerySpecifications().First();
            var rc = xmqs.FindDescendant<RegionClause>();

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

        protected override SqlCommand GetTableStatisticsCommand(TableReference tr)
        {
            if (region == null)
            {
                return base.GetTableStatisticsCommand(tr);
            }
            else
            {
                return GetTableStatisticsWithRegionCommand(tr);
            }
        }

        private SqlCommand GetTableStatisticsWithRegionCommand(TableReference tr)
        {
            if (tr.Statistics == null)
            {
                throw new ArgumentNullException();
            }

            if (!(tr.DatabaseObject is TableOrView))
            {
                throw new ArgumentException();
            }

            var table = (TableOrView)tr.DatabaseObject;
            var keycol = tr.Statistics.KeyColumn;
            var keytype = tr.Statistics.KeyColumnDataType.NameWithLength;

            var tablename = CodeGenerator.GetEscapedUniqueName(tr);
            var temptable = GetTemporaryTable("stat_" + tablename);
            var htmtable = GetTemporaryTable("htm_" + tablename);

            var sql = new StringBuilder(RegionScripts.TableStatistics);

            sql.Replace("[$temptable]", CodeGenerator.GetResolvedTableName(temptable));
            sql.Replace("[$htm]", CodeGenerator.GetResolvedTableName(htmtable));
            sql.Replace("[$htmid]", tr....
            sql.Replace("[$keytype]", keytype);
            sql.Replace("[$keycol]", keycol);
            sql.Replace("[$tablename]", CodeGenerator.GetResolvedTableNameWithAlias(tr));
            sql.Replace("[$where]", GetTableStatisticsWhereClause(tr));

            var cmd = new SqlCommand(sql.ToString());

            cmd.Parameters.Add("@region", SqlDbType.VarBinary).Value = region.ToSqlBytes();
            
            return cmd;
        }
    }
}
