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
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.SqlParser;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    [DataContract(Name = "Query", Namespace = "")]
    public abstract class XMatchQuery : SqlQuery
    {
        #region Private member variables

        protected string tableAlias;

        // --- cross-match parameters
        protected double zoneHeight;
        protected double limit;

        // --- cache for table specifications
        [NonSerialized]
        protected Dictionary<string, XMatchTableSpecification> xmatchTables;

        [NonSerialized]
        protected Jhu.Spherical.Region region;

        #endregion
        #region Properties

        [IgnoreDataMember]
        public override bool IsPartitioned
        {
            get { return true; }
        }

        /// <summary>
        /// In degrees
        /// </summary>
        [DataMember]
        public double ZoneHeight
        {
            get { return zoneHeight; }
            set { zoneHeight = value; }
        }

        [DataMember]
        public double Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        #endregion
        #region Constructors and initializers

        public XMatchQuery()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public XMatchQuery(gw::Context context)
            : base(context)
        {
            InitializeMembers(new StreamingContext());
        }

        private void InitializeMembers(StreamingContext context)
        {
            this.tableAlias = String.Empty;

            this.zoneHeight = -1;
            this.limit = -1;

            this.xmatchTables = null;
            this.region = null;
        }

        private void CopyMembers(XMatchQuery old)
        {
            this.tableAlias = old.tableAlias;

            this.zoneHeight = old.zoneHeight;
            this.limit = old.limit;

            this.xmatchTables = old.xmatchTables;
            this.region = old.region;
        }

        #endregion

        protected override void FinishInterpret(bool forceReinitialize)
        {
            this.zoneHeight = Constants.DefaultZoneHeight;

            // Interpret xmatch parameters
            var qs = (XMatchQuerySpecification)SelectStatement.EnumerateQuerySpecifications().First();
            var xts = qs.XMatchTableSource;

            // Bayes factor or probability limit
            this.limit = xts.XMatchLimit;

            xmatchTables = InterpretXMatchTables(SelectStatement);
            region = InterpretRegion(SelectStatement);

            base.FinishInterpret(forceReinitialize);
        }

        internal static Dictionary<string, XMatchTableSpecification> InterpretXMatchTables(Jhu.Graywulf.SqlParser.SelectStatement selectStatement)
        {
            var xmqs = (XMatchQuerySpecification)selectStatement.EnumerateQuerySpecifications().First();
            var res = new Dictionary<string, XMatchTableSpecification>(SchemaManager.Comparer);

            foreach (var xt in xmqs.XMatchTableSource.EnumerateXMatchTableSpecifications())
            {
                res.Add(xt.TableReference.UniqueName, xt);
            }

            return res;
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

        protected override SqlCommand GetComputeTableStatisticsCommand(TableReference tr)
        {
            SqlCommand cmd;

            if (region == null)
            {
                cmd = base.GetComputeTableStatisticsCommand(tr);
                
            }
            else
            {
                // TODO: implement table statistics with region constraint here
                throw new NotImplementedException();
            }

            cmd.Parameters.Add("@H", SqlDbType.Float).Value = zoneHeight;

            return cmd;
        }
    }
}
