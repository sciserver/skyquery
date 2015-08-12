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
    public abstract class XMatchQuery : RegionQuery
    {
        #region Private member variables

        protected string tableAlias;

        // --- cross-match parameters
        protected double zoneHeight;
        protected double limit;

        // --- cache for table specifications
        [NonSerialized]
        protected Dictionary<string, XMatchTableSpecification> xmatchTables;

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

        protected override SqlCommand GetTableStatisticsCommand(TableReference tr)
        {
            var cmd = base.GetTableStatisticsCommand(tr);

            cmd.Parameters.Add("@H", SqlDbType.Float).Value = zoneHeight;

            return cmd;
        }
    }
}
