using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.ParserLib;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    [DataContract(Name = "Query", Namespace = "")]
    public class BayesFactorXMatchQuery : XMatchQuery
    {
        protected BayesFactorXMatchQuery()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public BayesFactorXMatchQuery(gw.Context context)
            : base(context)
        {
            InitializeMembers(new StreamingContext());
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
        }

        public override void CollectTablesForStatistics()
        {
            TableSourceStatistics.Clear();
            decimal binsize = (decimal)(5 * ZoneHeight);   // histogram bin size in Dec

            foreach (var table in xmatchTables.Values)
            {
                var tr = table.TableReference;

                // Collect statistics for zoneID
                tr.Statistics = new Graywulf.SqlParser.TableStatistics();
                tr.Statistics.KeyColumnDataType = DataTypes.Int32;
                tr.Statistics.KeyColumn = table.Coordinates.GetZoneIdString(CodeDataset);

                TableSourceStatistics.Add(table.TableSource);
            }
        }

        public override void GeneratePartitions(int partitionCount)
        {
            switch (ExecutionMode)
            {
                case ExecutionMode.SingleServer:
                    {
                        // No partitioning for single server
                        // Use this for debugging purposes too
                        var qp = new BayesFactorXMatchQueryPartition(this, null);
                        qp.GenerateSteps(xmatchTables.Values.ToArray());
                        AppendPartition(qp);
                    }
                    break;
                case ExecutionMode.Graywulf:
                    {
                        // xmathTables might be reinitialized, so copy statistics
                        foreach (var st in TableSourceStatistics)
                        {
                            xmatchTables[st.TableReference.UniqueName].TableReference.Statistics = st.TableReference.Statistics;
                        }

                        // Order tables based on incusion method and statistics
                        var tables = xmatchTables.Values.OrderBy(i => i).ToArray();

                        // Find stat histogram with most bins, that will be used to generate partitions
                        int statmax = -1;
                        for (int i = 0; i < tables.Length; i++)
                        {
                            if (statmax == -1 ||
                                tables[i].TableReference.Statistics.RowCount > tables[statmax].TableReference.Statistics.RowCount)
                            {
                                statmax = i;
                            }
                        }

                        var stat = tables[statmax].TableReference.Statistics;
                        GeneratePartitions(partitionCount, stat, tables);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void GeneratePartitions(int partitionCount, Jhu.Graywulf.SqlParser.TableStatistics stat, XMatchTableSpecification[] tables)
        {
            // TODO: verify with repeating keys!

            BayesFactorXMatchQueryPartition qp = null;
            int s = stat.KeyValue.Count / partitionCount;

            if (s == 0)
            {
                qp = new BayesFactorXMatchQueryPartition(this, this.Context);
                qp.GenerateSteps(tables);
                AppendPartition(qp);
            }
            else
            {
                for (int i = 0; i < partitionCount; i++)
                {
                    qp = new BayesFactorXMatchQueryPartition(this, this.Context);
                    qp.GenerateSteps(tables);

                    // *** TODO: verify bounds
                    qp.PartitioningKeyTo = stat.KeyValue[Math.Min((i + 1) * s, stat.KeyValue.Count - 1)];
                    if (i == 0)
                    {
                        qp.PartitioningKeyFrom = null;
                    }
                    else
                    {
                        qp.PartitioningKeyFrom = Partitions[i - 1].PartitioningKeyTo;
                    }

                    AppendPartition(qp);
                }

                Partitions[Partitions.Count - 1].PartitioningKeyTo = null;
            }
        }
    }
}
