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
        protected string tableAlias;

        // --- cross-match parameters
        protected double zoneHeight;
        protected bool propagateColumns;

        protected double limit;

        // --- cache for table specifications
        [NonSerialized]
        protected List<XMatchTableSpecification> xmatchTables;

        [IgnoreDataMember]
        public override bool IsPartitioned
        {
            get
            {
                return true;
            }
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
        public bool PropagateColumns
        {
            get { return propagateColumns; }
            set { propagateColumns = value; }
        }

        [DataMember]
        public double Limit
        {
            get { return limit; }
            set { limit = value; }
        }

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
            this.propagateColumns = true;

            this.limit = -1;
        }

        private void CopyMembers(XMatchQuery old)
        {
            this.tableAlias = old.tableAlias;

            this.zoneHeight = old.zoneHeight;
            this.propagateColumns = old.propagateColumns;

            this.limit = old.limit;
        }

        protected override void FinishInterpret(bool forceReinitialize)
        {
            this.zoneHeight = Constants.DefaultZoneHeight;

            // Interpret xmatch parameters
            var qs = (XMatchQuerySpecification)SelectStatement.EnumerateQuerySpecifications().First();
            var xts = qs.XMatchTableSource;
           
            // Bayes factor or probability limit
            this.limit = xts.XMatchLimit;

            // Find xmatch tables
            xmatchTables = new List<XMatchTableSpecification>(xts.EnumerateXMatchTableSpecifications());

            base.FinishInterpret(forceReinitialize);
        }
    }
}
