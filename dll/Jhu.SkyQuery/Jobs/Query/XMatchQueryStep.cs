using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    public abstract class XMatchQueryStep
    {
        protected QueryPartitionBase queryPartition;

        protected int stepNumber;

        protected string previousXMatchTable;

        protected string xMatchTable;
        
        protected double searchRadius;

        public QueryPartitionBase QueryPartition
        {
            get { return queryPartition; }
        }

        public int StepNumber
        {
            get { return stepNumber; }
            set { stepNumber = value; }
        }

        public string PreviousXMatchTable
        {
            get { return previousXMatchTable; }
            set { previousXMatchTable = value; }
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

        public XMatchQueryStep()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public XMatchQueryStep(XMatchQueryPartition queryPartition, gw.Context context)
        {
            InitializeMembers(new StreamingContext());

            this.queryPartition = queryPartition;
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            this.queryPartition = null;

            this.stepNumber = 0;
            this.previousXMatchTable = null;
            this.xMatchTable = null;
        }
    }
}
