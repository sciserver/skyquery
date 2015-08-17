using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.SqlParser;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.SkyQuery.Parser;

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

        protected RegionQuery()
            : base()
        {
        }

        public RegionQuery(RegionQuery old)
            : base(old)
        {
        }

        public RegionQuery(Context context)
            : base(context)
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
