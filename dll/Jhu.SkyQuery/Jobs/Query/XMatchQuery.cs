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

        public Dictionary<string, XMatchTableSpecification> XMatchTables
        {
            get { return xmatchTables; }
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
            this.zoneHeight = Constants.DefaultZoneHeight;
            this.limit = -1;

            this.xmatchTables = null;
        }

        private void CopyMembers(XMatchQuery old)
        {
            this.zoneHeight = old.zoneHeight;
            this.limit = old.limit;

            this.xmatchTables = old.xmatchTables;
        }
        
        #endregion

        protected override void FinishInterpret(bool forceReinitialize)
        {
            if (xmatchTables == null || forceReinitialize)
            {
                InterpretXMatchQuery((XMatchSelectStatement)SelectStatement);
            }

            base.FinishInterpret(forceReinitialize);
        }

        private void InterpretXMatchQuery(XMatchSelectStatement selectStatement)
        {
            // Interpret xmatch parameters
            // Bayes factor or probability limit
            var qs = selectStatement.XMatchQuerySpecification;
            var xts = qs.XMatchTableSource;

            limit = xts.XMatchLimit;
            xmatchTables = InterpretXMatchTables(qs);
        }

        internal static Dictionary<string, XMatchTableSpecification> InterpretXMatchTables(XMatchQuerySpecification qs)
        {
            var res = new Dictionary<string, XMatchTableSpecification>(SchemaManager.Comparer);

            foreach (var xt in qs.XMatchTableSource.EnumerateXMatchTableSpecifications())
            {
                res.Add(xt.TableReference.UniqueName, xt);
            }

            return res;
        }

        protected override void OnGeneratePartitions(int partitionCount, Jhu.Graywulf.SqlParser.TableStatistics stat)
        {
            // XmathTables might be reinitialized, so copy statistics
            foreach (var st in TableSourceStatistics)
            {
                xmatchTables[st.TableReference.UniqueName].TableReference.Statistics = st.TableReference.Statistics;
            }

            // Order tables based on incusion method and statistics
            var tables = xmatchTables.Values.OrderBy(i => i).ToArray();

            // Find stat histogram with most bins, that will be used to generate partitions
            // instead of the very first table
            int statmax = -1;
            for (int i = 0; i < tables.Length; i++)
            {
                if (statmax == -1 ||
                    tables[i].TableReference.Statistics.RowCount > tables[statmax].TableReference.Statistics.RowCount)
                {
                    statmax = i;
                }
            }

            stat = tables[statmax].TableReference.Statistics;

            // Generate partitions the standard way
            base.OnGeneratePartitions(partitionCount, stat);

            // Create steps
            foreach (var qp in Partitions)
            {
                ((XMatchQueryPartition)qp).GenerateSteps(tables);
            }
        }
    }
}
