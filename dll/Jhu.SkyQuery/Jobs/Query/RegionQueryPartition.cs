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
        protected const string regionParameterName = "@region";

        #region Private member variables

        [NonSerialized]
        private Jhu.Spherical.Region region;

        #endregion
        #region Properties

        public Spherical.Region Region
        {
            get { return region; }
        }

        #endregion
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
            this.region = null;
        }

        private void CopyMembers(RegionQueryPartition old)
        {
            this.region = old.region;
        }

        public override object Clone()
        {
            return new RegionQueryPartition(this);
        }

        #endregion

        protected override void FinishInterpret(bool forceReinitialize)
        {
            region = RegionQuery.InterpretRegion(SelectStatement);

            base.FinishInterpret(forceReinitialize);
        }
    }
}
