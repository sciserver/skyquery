using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Schema;
using Jhu.SkyQuery.Parser;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
	public class BayesFactorXMatchQueryStep : XMatchQueryStep
	{
        public BayesFactorXMatchQueryStep()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public BayesFactorXMatchQueryStep(XMatchQueryPartition queryPartition, gw.Context context)
            : base(queryPartition, context)
        {
            InitializeMembers(new StreamingContext());
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
        }
	}
}
