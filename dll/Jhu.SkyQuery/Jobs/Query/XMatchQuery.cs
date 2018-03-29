﻿using System;
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

                res.Add(xt.TableSource.UniqueKey, xt);
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
                }
            }
        }
        
        protected override void OnGeneratePartitions(int partitionCount, Jhu.Graywulf.Sql.Jobs.Query.TableStatistics stat)
        {
            var tables = new List<XMatchTableSpecification>(); ;

            // Order tables based on incusion method and statistics
            foreach (var key in xmatchTables.Keys)
            {
                if (TableStatistics.ContainsKey(key))
                {
                    tables.Add(xmatchTables[key]);
                }
            }

            tables.Sort(new XMatchTableComparer(this));

            // Find stat histogram with most bins, that will be used to generate partitions
            // instead of the very first table
            int statmax = -1;
            for (int i = 0; i < tables.Count; i++)
            {
                if (statmax == -1 ||
                    TableStatistics[tables[i].TableSource.UniqueKey].RowCount > TableStatistics[tables[statmax].TableSource.UniqueKey].RowCount)
                {
                    statmax = i;
                }
            }

            stat = TableStatistics[tables[statmax].TableSource.UniqueKey];

            base.OnGeneratePartitions(partitionCount, stat);

            // Create steps
            foreach (var qp in Partitions)
            {
                ((XMatchQueryPartition)qp).GenerateSteps(tables);
            }
        }
    }
}
