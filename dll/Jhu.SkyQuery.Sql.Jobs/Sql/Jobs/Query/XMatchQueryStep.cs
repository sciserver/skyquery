using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.SkyQuery.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    [Serializable]
    public abstract class XMatchQueryStep
    {
        private SqlQueryPartition queryPartition;

        private int stepNumber;
        private string xMatchTable;
        private double searchRadius;

        public SqlQueryPartition QueryPartition
        {
            get { return queryPartition; }
        }

        public int StepNumber
        {
            get { return stepNumber; }
            set { stepNumber = value; }
        }

        public string XMatchTable
        {
            get { return xMatchTable; }
            set { xMatchTable = value; }
        }

        public double SearchRadius
        {
            get { return searchRadius; }
            set { searchRadius = value; }
        }

        public long ZoneBuffer
        {
            get
            {
                return (long)Math.Ceiling(searchRadius / ((XMatchQuery)queryPartition.Query).ZoneHeight);
            }
        }

        public XMatchQueryStep()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public XMatchQueryStep(XMatchQueryPartition queryPartition, gw.RegistryContext context)
        {
            InitializeMembers(new StreamingContext());

            this.queryPartition = queryPartition;
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            this.queryPartition = null;

            this.stepNumber = 0;
            this.xMatchTable = null;
            this.searchRadius = 0;
        }
    }
}
