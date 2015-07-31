using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    public class RegionQueryPartition : SqlQueryPartition, ICloneable
    {
        #region Constructors and initializers

        public RegionQueryPartition()
            : base()
        {
            InitializeMembers();
        }

        public RegionQueryPartition(RegionQuery query, Context context)
            : base(query, context)
        {
            InitializeMembers();
        }

        public RegionQueryPartition(RegionQueryPartition old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(RegionQueryPartition old)
        {
        }

        public override object Clone()
        {
            return new RegionQueryPartition(this);
        }

        #endregion
    }
}
