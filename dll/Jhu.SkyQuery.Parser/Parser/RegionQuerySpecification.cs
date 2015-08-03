using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
{
    public class RegionQuerySpecification : QuerySpecification
    {
        #region Constructors and initializers

        public RegionQuerySpecification()
            : base()
        {
            InitializeMembers();
        }

        public RegionQuerySpecification(QuerySpecification old)
            :base(old)
        {
            InitializeMembers();
        }

        public RegionQuerySpecification(RegionQuerySpecification old)
            :base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(RegionQuerySpecification old)
        {
        }

        #endregion
    }
}
