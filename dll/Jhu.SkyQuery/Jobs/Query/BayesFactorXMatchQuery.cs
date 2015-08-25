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
        private BayesFactorXMatchQueryCodeGenerator CodeGenerator
        {
            get { return (BayesFactorXMatchQueryCodeGenerator)CreateCodeGenerator(); }
        }

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

            foreach (var table in xmatchTables.Values)
            {
                var tr = table.TableReference;

                // Collect statistics for zoneID
                tr.Statistics = new Graywulf.SqlParser.TableStatistics();
                tr.Statistics.KeyColumn = table.Coordinates.ZoneIdExpression;
                tr.Statistics.KeyColumnDataType = DataTypes.SqlInt;

                TableSourceStatistics.Add(table.TableSource);
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
                        var qp = new BayesFactorXMatchQueryPartition(this);
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
                        GeneratePartitionsInternal(partitionCount, stat, tables);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
#endif

        

        protected override SqlQueryCodeGenerator CreateCodeGenerator()
        {
            return new BayesFactorXMatchQueryCodeGenerator(this);
        }

        protected override SqlQueryPartition CreatePartition()
        {
            return new BayesFactorXMatchQueryPartition(this);
        }
    }
}
