using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Jobs.Query;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Tasks;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    [Serializable]
    [DataContract(Name = "Query", Namespace = "")]
    public abstract class XMatchQuery : RegionQuery
    {
        #region Private member variables

        // --- cross-match parameters
        private double zoneHeight;

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

        public Dictionary<string, XMatchTableSpecification> XMatchTables
        {
            get { return xmatchTables; }
        }

        #endregion
        #region Constructors and initializers

        public XMatchQuery()
        {
            InitializeMembers(new StreamingContext());
        }

        public XMatchQuery(CancellationContext cancellationContext)
            : base(cancellationContext)
        {
            InitializeMembers(new StreamingContext());
        }

        public XMatchQuery(CancellationContext cancellationContext, gw::RegistryContext context)
            : base(cancellationContext, context)
        {
            InitializeMembers(new StreamingContext());
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            this.zoneHeight = Constants.DefaultZoneHeight;
            this.xmatchTables = null;
        }

        private void CopyMembers(XMatchQuery old)
        {
            this.zoneHeight = old.zoneHeight;
            this.xmatchTables = old.xmatchTables;
        }

        #endregion

        protected override void OnNamesResolved(bool forceReinitialize)
        {
            if (xmatchTables == null || forceReinitialize)
            {
                var xmss = QueryDetails.ParsingTree.FindDescendantRecursive<XMatchSelectStatement>();
                InterpretXMatchQuery(xmss);
            }

            base.OnNamesResolved(forceReinitialize);
        }

        private void InterpretXMatchQuery(XMatchSelectStatement selectStatement)
        {
            // Interpret xmatch parameters
            // Bayes factor or probability limit
            var qs = selectStatement.XMatchQuerySpecification;
            var xmts = qs.XMatchTableSource;

            xmatchTables = IdentifyXMatchTables(qs);

            InterpretLimit(xmts);

            LogOperation(LogMessages.XMatchInterpreted, xmatchTables.Count, this.GetType().Name);
        }

        protected abstract void InterpretLimit(XMatchTableSource xmts);

        private Dictionary<string, XMatchTableSpecification> IdentifyXMatchTables(XMatchQuerySpecification qs)
        {
            var res = new Dictionary<string, XMatchTableSpecification>(SchemaManager.Comparer);

            foreach (var xt in qs.XMatchTableSource.EnumerateXMatchTableSpecifications())
            {
                // Set partitioning key
                xt.TableSource.PartitioningKeyExpression = CodeGenerator.GetZoneIdExpression(xt.TableSource.Coordinates);
                xt.TableSource.PartitioningKeyDataType = DataTypes.SqlInt;

                res.Add(xt.TableSource.UniqueKey, xt);

                LogOperation(LogMessages.XMatchTableIdentified, xt.TableSource.TableReference.DatabaseObject.FullyResolvedName);

                if (CodeGenerator.IsZoneTableNecessary(xt, out CreateZoneTableReason reason))
                {
                    LogWarning(LogMessages.ZoneTableNecessary, xt.TableSource.TableReference.DatabaseObject.FullyResolvedName, reason);
                }
            }

            return res;
        }

        public override void IdentifyTablesForStatistics()
        {
            TableStatistics.Clear();

            // Only collect statistics for xmatch tables
            // TODO: how to deal with remote tables?

            foreach (var table in xmatchTables.Values)
            {
                var tr = table.TableReference;
                var ts = table.TableSource;

                if (tr.DatabaseObject.Dataset is GraywulfDataset)
                {
                    // Collect statistics for zoneID
                    var stat = new Graywulf.Sql.Jobs.Query.TableStatistics(ts)
                    {
                        KeyColumn = CodeGenerator.GetZoneIdExpression(table.Coordinates),
                        KeyColumnDataType = DataTypes.SqlInt,
                    };

                    TableStatistics.Add(ts.UniqueKey, stat);

                    LogOperation(Jhu.Graywulf.Sql.Jobs.Query.LogMessages.StatisticsTableIdentified, ts.TableReference.DatabaseObject.FullyResolvedName);
                }
            }
        }

        protected override void OnGeneratePartitions(int partitionCount, Jhu.Graywulf.Sql.Jobs.Query.TableStatistics stat)
        {
            var tables = SortXMatchTableSpecifications();

            // Find stat histogram with most bins, that will be used to generate partitions
            // instead of the very first table
            int statmax = -1;
            for (int i = 0; i < tables.Count; i++)
            {
                if (TableStatistics.ContainsKey(tables[i].TableSource.UniqueKey) &&
                   (statmax == -1 || TableStatistics[tables[i].TableSource.UniqueKey].RowCount > TableStatistics[tables[statmax].TableSource.UniqueKey].RowCount))
                {
                    statmax = i;
                }
            }

            if (statmax >= 0)
            {
                stat = TableStatistics[tables[statmax].TableSource.UniqueKey];
            }
            else
            {
                stat = null;
            }

            base.OnGeneratePartitions(partitionCount, stat);

            // Create steps
            foreach (var qp in Partitions)
            {
                ((XMatchQueryPartition)qp).GenerateSteps(tables);
            }

            LogOperation(LogMessages.XMatchStepsGenerated, ((XMatchQueryPartition)Partitions[0]).Steps.Count);
        }

        protected abstract List<XMatchTableSpecification> SortXMatchTableSpecifications();
    }
}
