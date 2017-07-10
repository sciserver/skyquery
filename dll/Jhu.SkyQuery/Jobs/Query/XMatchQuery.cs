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

        private XMatchQueryCodeGenerator CodeGenerator
        {
            get { return (XMatchQueryCodeGenerator)CreateCodeGenerator(); }
        }

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

        public XMatchQuery(gw::RegistryContext context)
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

        private Dictionary<string, XMatchTableSpecification> InterpretXMatchTables(XMatchQuerySpecification qs)
        {
            var res = new Dictionary<string, XMatchTableSpecification>(SchemaManager.Comparer);

            foreach (var xt in qs.XMatchTableSource.EnumerateXMatchTableSpecifications())
            {
                // Set partitioning key
                xt.TableSource.PartitioningKeyExpression = CodeGenerator.GetZoneIdExpression(xt.TableSource.Coordinates);
                xt.TableSource.PartitioningKeyDataType = DataTypes.SqlInt;

                res.Add(xt.TableReference.UniqueName, xt);
            }

            return res;
        }

        public override void CollectTablesForStatistics()
        {
            TableSourceStatistics.Clear();

            // Only collect statistics for xmatch tables
            foreach (var table in xmatchTables.Values)
            {
                var tr = table.TableReference;

                // Statistics is only gathered for table on known servers. Skip foreign
                // datasets here because we cannot make sure they support the necessary
                // CLR functions.
                if (tr.DatabaseObject.Dataset is GraywulfDataset)
                {
                    // Collect statistics for zoneID
                    tr.Statistics = new Graywulf.SqlParser.TableStatistics()
                    {
                        KeyColumn = CodeGenerator.GetZoneIdExpression(table.Coordinates),
                        KeyColumnDataType = DataTypes.SqlInt,
                    };

                    TableSourceStatistics.Add(table.TableSource);
                }
            }
        }

        private IEnumerable<XMatchTableSpecification> SortXMatchTables(IEnumerable<XMatchTableSpecification> tables)
        {
            // Use the comparer defined on SortXMatchTables to sort tables by increasing cardinality
            return xmatchTables.Values.OrderBy(i => i);
        }

        protected override void OnGeneratePartitions(int partitionCount, Jhu.Graywulf.SqlParser.TableStatistics stat)
        {
            // XmathTables might be reinitialized, so copy statistics
            foreach (var st in TableSourceStatistics)
            {
                xmatchTables[st.TableReference.UniqueName].TableReference.Statistics = st.TableReference.Statistics;
            }

            // We don't compute statistics for remote tables so we have to deal with them now
            foreach (var xt in xmatchTables.Values)
            {
                if (xt.TableReference.Statistics == null)
                {
                    xt.TableReference.Statistics = new Graywulf.SqlParser.TableStatistics();
                }
            }

            // Order tables based on incusion method and statistics
            var tables = SortXMatchTables(xmatchTables.Values).ToArray();

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

            base.OnGeneratePartitions(partitionCount, stat);

            // Create steps
            foreach (var qp in Partitions)
            {
                ((XMatchQueryPartition)qp).GenerateSteps(tables);
            }
        }
    }
}
