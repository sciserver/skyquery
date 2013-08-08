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
            TableStatistics.Clear();
            decimal binsize = (decimal)(5 * ZoneHeight);   // histogram bin size in Dec

            foreach (var table in xmatchTables)
            {
                table.TableReference.Statistics = new Graywulf.SqlParser.TableStatistics()
                {
                    Table = table.TableReference,
                    KeyColumn = "Dec",  // *** TODO: figure this out from query
                };

                TableStatistics.Add(table.TableReference);
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
                        BayesFactorXMatchQueryPartition qp = new BayesFactorXMatchQueryPartition(this, null);
                        qp.ID = 0;
                        qp.GenerateSteps(xmatchTables.ToArray());
                        qp.PartitioningKeyFrom = double.NegativeInfinity;
                        qp.PartitioningKeyTo = double.PositiveInfinity;
                        AppendPartition(qp);
                    }
                    break;
                case ExecutionMode.Graywulf:
                    {
                        // Create a copy of xmatchTables that will be reordered based on stats
                        XMatchTableSpecification[] tables = xmatchTables.ToArray();

                        // --- sort tables by count
                        // *** this must be changed for MAY and DROP!
                        // Order of xmatch execution: MUST, MAY, DROP (inclusion method)
                        //    first sort by InclusionMethod, then by count, smallest table first
                        for (int i = 0; i < tables.Length; i++)
                        {
                            for (int j = tables.Length - 1; j > i; j--)
                            {
                                // swap
                                if (
                                    tables[j].InclusionMethod == XMatchInclusionMethod.Must && tables[i].InclusionMethod == XMatchInclusionMethod.Drop ||
                                    tables[j].InclusionMethod == XMatchInclusionMethod.Must && tables[i].InclusionMethod == XMatchInclusionMethod.May ||
                                    tables[j].InclusionMethod == XMatchInclusionMethod.Drop && tables[i].InclusionMethod == XMatchInclusionMethod.May ||
                                    tables[j].InclusionMethod == tables[i].InclusionMethod && TableStatistics[j].Statistics.RowCount < TableStatistics[i].Statistics.RowCount)
                                {
                                    var xmts = tables[i];
                                    tables[i] = tables[j];
                                    tables[j] = xmts;

                                    var st = TableStatistics[i];
                                    TableStatistics[i] = TableStatistics[j];
                                    TableStatistics[j] = st;
                                }
                            }
                        }

                        // Update partitioning table to be the first
                        partitioningTable = tables[0].TableReference.FullyQualifiedName;
                        var stat = TableStatistics[0].Statistics;

                        GeneratePartitions(partitionCount, stat, tables);

#if false
                        // --- determine partition limits based on the first table's statistics
                        BayesFactorXMatchQueryPartition qp = null;
                        int cnt = 0;
                        int bin = 0;
                        while (bin < tables[0].TableReference.Statistics.KeyCount.Count)
                        {
                            if (qp == null)
                            {
                                cnt = 0;
                                qp = new BayesFactorXMatchQueryPartition(this, this.Context);
                                qp.ID = Partitions.Count;
                                qp.GenerateSteps(tables);
                                qp.PartitioningKeyFrom = (double)tables[0].TableReference.Statistics.BinMin[bin];
                            }

                            cnt += tables[0].TableReference.Statistics.KeyCount[bin];

                            if (cnt >= (int)(tables[0].TableReference.Statistics.RowCount / (double)partitionCount) || (bin == tables[0].TableReference.Statistics.KeyCount.Count - 1))
                            {
                                qp.PartitioningKeyTo = (double)tables[0].TableReference.Statistics.BinMax[bin];
                                AppendPartition(qp);
                                qp = null;
                            }

                            bin++;
                        }
#endif

                        AlignPartitionLimitsWithZone(Partitions);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void GeneratePartitions(int partitionCount, Jhu.Graywulf.SqlParser.TableStatistics stat, XMatchTableSpecification[] tables)
        {
            BayesFactorXMatchQueryPartition qp = null;
            int s = stat.KeyValue.Count / partitionCount;
            
            if (s == 0)
            {
                qp = new BayesFactorXMatchQueryPartition(this, this.Context);
                qp.ID = Partitions.Count;
                qp.GenerateSteps(tables);

                qp.PartitioningKeyFrom = double.NegativeInfinity;
                qp.PartitioningKeyTo = double.PositiveInfinity;

                AppendPartition(qp);
            }
            else
            {
                for (int i = 0; i < partitionCount; i++)
                {
                    qp = new BayesFactorXMatchQueryPartition(this, this.Context);
                    qp.ID = Partitions.Count;
                    qp.GenerateSteps(tables);

                    // *** TODO: verify bounds
                    qp.PartitioningKeyTo = stat.KeyValue[Math.Min((i + 1) * s, stat.KeyValue.Count -1)];
                    if (i == 0)
                    {
                        qp.PartitioningKeyFrom = double.NegativeInfinity;
                    }
                    else
                    {
                        qp.PartitioningKeyFrom = Partitions[i - 1].PartitioningKeyTo;
                    }

                    AppendPartition(qp);
                }

                Partitions[Partitions.Count - 1].PartitioningKeyTo = double.PositiveInfinity;
            }
        }

#if false
        public override void GeneratePartitions(int partitionCount)
        {
            switch (ExecutionMode)
            {
                case ExecutionMode.SingleServer:
                    {
                        // No partitioning for single server
                        // Use this for debugging purposes too
                        BayesFactorXMatchQueryPartition qp = new BayesFactorXMatchQueryPartition(this, null);
                        qp.ID = 0;
                        qp.GenerateSteps(xmatchTables.ToArray());
                        qp.PartitioningKeyFrom = double.NegativeInfinity;
                        qp.PartitioningKeyTo = double.PositiveInfinity;
                        AppendPartition(qp);
                    }
                    break;
                case ExecutionMode.Graywulf:
                    {
                        List<TableStatistics> stats = new List<TableStatistics>();

                        // Create a histogram with appropriate resolution for the number of servers available
                        foreach (var table in xmatchTables)
                        {
                            decimal binsize = (decimal)(5 * ZoneHeight);   // histogram bin size in Dec
                            TableStatistics ts;

                            var qs = (XMatchQuerySpecification)SelectStatement.EnumerateQuerySpecifications().FirstOrDefault();
                            // **** tweak this to get appropriate histogram resolution and be compatible with small search areas with few object
                            /*do
                            {
                                ts = ComputeTableStatistics(qs, table, binsize);

                                if (ts.BinCount.Count == 0) break;

                                binsize /= 10.0m;
                            }
                            while (ts.BinCount.Count / partitionCount < 10);*/

                            ts = ComputeTableStatistics(qs, table, binsize);

                            stats.Add(ts);
                        }

                        // Create a copy of xmatchTables that will be reordered based on stats
                        XMatchTableSpecification[] tables = xmatchTables.ToArray();

                        // --- sort tables by count
                        // *** this must be changed for MAY and DROP!
                        // Order of xmatch execution: MUST, MAY, DROP (inclusion method)
                        //    first sort by InclusionMethod, then by count, smallest table first
                        for (int i = 0; i < tables.Length; i++)
                        {
                            for (int j = tables.Length - 1; j > i; j--)
                            {
                                // swap
                                if (
                                    tables[j].InclusionMethod == XMatchTableSpecification.XMatchInclusionMethod.Must && tables[i].InclusionMethod == XMatchTableSpecification.XMatchInclusionMethod.Drop ||
                                    tables[j].InclusionMethod == XMatchTableSpecification.XMatchInclusionMethod.Must && tables[i].InclusionMethod == XMatchTableSpecification.XMatchInclusionMethod.May ||
                                    tables[j].InclusionMethod == XMatchTableSpecification.XMatchInclusionMethod.Drop && tables[i].InclusionMethod == XMatchTableSpecification.XMatchInclusionMethod.May ||
                                    tables[j].InclusionMethod == tables[i].InclusionMethod && stats[j].Count < stats[i].Count)
                                {
                                    TableStatistics ts = stats[i];
                                    stats[i] = stats[j];
                                    stats[j] = ts;

                                    XMatchTableSpecification xmts = tables[i];
                                    tables[i] = tables[j];
                                    tables[j] = xmts;
                                }
                            }
                        }

                        // Update partitioning table to be the first
                        partitioningTable = tables[0].TableReference.FullyQualifiedName;

                        // --- determine partition limits based on the first table's statistics
                        BayesFactorXMatchQueryPartition qp = null;
                        int cnt = 0;
                        int bin = 0;
                        while (bin < stats[0].BinCount.Count)
                        {
                            if (qp == null)
                            {
                                cnt = 0;
                                qp = new BayesFactorXMatchQueryPartition(this, this.Context);
                                qp.ID = Partitions.Count;
                                qp.GenerateSteps(tables);
                                qp.PartitioningKeyFrom = (double)stats[0].BinMin[bin];
                            }

                            cnt += stats[0].BinCount[bin];

                            if (cnt >= (int)(stats[0].Count / (double)partitionCount) || (bin == stats[0].BinCount.Count - 1))
                            {
                                qp.PartitioningKeyTo = (double)stats[0].BinMax[bin];
                                AppendPartition(qp);
                                qp = null;
                            }

                            bin++;
                        }

                        AlignPartitionLimitsWithZone(Partitions);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
#endif

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// First and last partitions will go to infinity, not to miss anything.
        /// </remarks>
        /// <param name="partitions"></param>
        private void AlignPartitionLimitsWithZone(List<QueryPartitionBase> partitions)
        {
            for (int i = 0; i < partitions.Count; i++)
            {
                BayesFactorXMatchQueryPartition qp = (BayesFactorXMatchQueryPartition)partitions[i];

                if (i == 0)
                {
                    qp.PartitioningKeyFrom = double.NegativeInfinity;
                }
                else
                {
                    qp.PartitioningKeyFrom = Math.Floor((qp.PartitioningKeyFrom + 90.0) / ZoneHeight) * ZoneHeight - 90;
                }

                if (i == partitions.Count - 1)
                {
                    qp.PartitioningKeyTo = double.PositiveInfinity;
                }
                else
                {
                    qp.PartitioningKeyTo = Math.Floor((qp.PartitioningKeyTo + 90.0) / ZoneHeight) * ZoneHeight - 90;
                }
            }
        }
    }
}
