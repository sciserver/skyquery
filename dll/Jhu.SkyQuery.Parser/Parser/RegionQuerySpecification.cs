using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public class RegionQuerySpecification : QuerySpecification
    {
        private Spherical.Region region;

        public Spherical.Region Region
        {
            get
            {
                if (region == null)
                {
                    InterpretRegion();
                }

                return region;
            }
        }

        #region Constructors and initializers

        public RegionQuerySpecification()
            : base()
        {
        }

        public RegionQuerySpecification(QuerySpecification old)
            : base(old)
        {
        }

        public RegionQuerySpecification(RegionQuerySpecification old)
            : base(old)
        {
        }

        protected override void OnInitializeMembers()
        {
            base.OnInitializeMembers();

            this.region = null;
        }

        public override object Clone()
        {
            return new RegionQuerySpecification(this);
        }

        #endregion

        private void InterpretRegion()
        {
            var rc = FindDescendant<RegionClause>();

            if (rc != null)
            {
                if (rc.IsUri)
                {
                    // TODO: implement region fetch
                    // need to do it once per query, not per partition?
                    throw new NotImplementedException();
                }
                if (rc.IsString)
                {
                    var p = new Jhu.Spherical.Parser.Parser(rc.RegionString);
                    region = p.ParseRegion();
                }
                else
                {
                    // TODO: implement direct region grammar
                    throw new NotImplementedException();
                }

                if (!region.IsSimplified)
                {
                    region.Simplify();
                }
            }
        }
    }
}
