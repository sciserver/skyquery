using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using Jhu.Graywulf.Tasks;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.Registry;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    [DataContract(Name = "Query", Namespace = "")]
    public class RegionQuery : SqlQuery
    {
        #region Private member variables

        #endregion
        #region Properties

        private RegionQueryCodeGenerator CodeGenerator
        {
            get { return (RegionQueryCodeGenerator)CreateCodeGenerator(); }
        }

        #endregion
        #region Constructors and initializers

        public RegionQuery()
        {
        }

        protected RegionQuery(CancellationContext cancellationContext)
            : base(cancellationContext)
        {
        }

        public RegionQuery(RegionQuery old)
            : base(old)
        {
        }

        public RegionQuery(CancellationContext cancellationContext, RegistryContext context)
            : base(cancellationContext, context)
        {
        }

        public override object Clone()
        {
            return new RegionQuery(this);
        }

        #endregion
        #region Query interpretation and validation



        #endregion

        protected override SqlQueryCodeGenerator CreateCodeGenerator()
        {
            return new RegionQueryCodeGenerator(this);
        }

        protected override SqlQueryPartition CreatePartition()
        {
            return new RegionQueryPartition(this);
        }
    }
}
