using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.ParserLib;

namespace Jhu.SkyQuery.Parser
{
    public partial class RegionSelectStatement
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
        
        public RegionClause RegionClause
        {
            get { return FindDescendantRecursive<RegionClause>(); }
        }

        #region Constructors and initializers

        protected override void OnInitializeMembers()
        {
            base.OnInitializeMembers();

            this.region = null;
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
