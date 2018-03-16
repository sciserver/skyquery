using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization;
using gw = Jhu.Graywulf.Registry;
using Jhu.Graywulf.Tasks;
using Jhu.Graywulf.Sql.Jobs.Query;
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

        public BayesFactorXMatchQuery()
        {
            // overload
        }

        protected BayesFactorXMatchQuery(CancellationContext cancellationContext)
            : base(cancellationContext)
        {
            InitializeMembers(new StreamingContext());
        }

        public BayesFactorXMatchQuery(CancellationContext cancellationContext, gw.RegistryContext context)
            : base(cancellationContext, context)
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
