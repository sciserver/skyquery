using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.SqlParser;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    public class RegionQueryPartition : SqlQueryPartition, ICloneable
    {
        public new RegionQuery Query
        {
            get { return (RegionQuery)base.Query; }
        }

        private RegionQueryCodeGenerator CodeGenerator
        {
            get { return (RegionQueryCodeGenerator)CreateCodeGenerator(); }
        }

        #region Constructors and initializers

        public RegionQueryPartition()
            : base()
        {
        }

        public RegionQueryPartition(RegionQuery query)
            : base(query)
        {
        }

        public RegionQueryPartition(RegionQueryPartition old)
            : base(old)
        {
        }

        public override object Clone()
        {
            return new RegionQueryPartition(this);
        }

        #endregion

        protected override Graywulf.SqlCodeGen.SqlServer.SqlServerCodeGenerator CreateCodeGenerator()
        {
            return new RegionQueryCodeGenerator(this);
        }
    }
}
