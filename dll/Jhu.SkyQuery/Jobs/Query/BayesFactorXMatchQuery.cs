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

                        var stat = TableStatistics[0].Statistics;

                        GeneratePartitions(partitionCount, stat, tables);
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
                    qp.GenerateSteps(tables);

                    // *** TODO: verify bounds
                    qp.PartitioningKeyTo = stat.KeyValue[Math.Min((i + 1) * s, stat.KeyValue.Count - 1)];
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
