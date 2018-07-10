using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Jhu.Graywulf.Sql.Jobs.Query;

namespace Jhu.SkyQuery.Sql.Jobs.Query
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

        protected override SqlQueryCodeGenerator CreateCodeGenerator()
        {
            return new RegionQueryCodeGenerator(this)
            {
                TableNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified,
                TableAliasRendering = Graywulf.Sql.QueryGeneration.AliasRendering.Default,
                ColumnNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified,
                ColumnAliasRendering = Graywulf.Sql.QueryGeneration.AliasRendering.Always,
                DataTypeNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified,
                FunctionNameRendering = Graywulf.Sql.QueryGeneration.NameRendering.FullyQualified
            };
        }
    }
}
