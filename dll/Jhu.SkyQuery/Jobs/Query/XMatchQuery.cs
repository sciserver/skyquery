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

        [NonSerialized]
        protected string partitioningTable;
        [NonSerialized]
        protected string partitioningKey;

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
            this.zoneHeight = 4.0 / 3600.0;     //****** TODO: remove hardcoding

            // Interpret xmatch parameters
            XMatchClause xmc = SelectStatement.EnumerateQuerySpecifications().First<Jhu.Graywulf.SqlParser.QuerySpecification>().FindDescendant<XMatchClause>();
            XMatchHavingClause xmhc = xmc.FindDescendant<XMatchHavingClause>();

            // Bayes factor or probability limit
            this.limit = double.Parse(xmhc.FindDescendant<Number>().Value, System.Globalization.CultureInfo.InvariantCulture);    // *** TODO: test this

            // Find xmatch tables
            xmatchTables = new List<XMatchTableSpecification>(SelectStatement.EnumerateQuerySpecifications().First<Jhu.Graywulf.SqlParser.QuerySpecification>().FindDescendant<XMatchClause>().EnumerateXMatchTableSpecifications());

            // Partitioning table - pick the first one
            // But this might be updated later based on statistics
            this.partitioningTable = xmatchTables[0].TableReference.FullyQualifiedName;
            this.partitioningKey = "Dec";        // ****** TODO use metadata

            base.FinishInterpret(forceReinitialize);
        }

        /* TODO: delete
        protected TableStatistics ComputeTableStatistics(XMatchQuerySpecification qs, XMatchTableSpecification table, decimal binSize)
        {
            // Partitioning key is overriden here! Always partition on Dec
            return ComputeTableStatistics(qs, table.TableReference, table.Position.Dec, binSize);
        }
        */
    }
}
