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
