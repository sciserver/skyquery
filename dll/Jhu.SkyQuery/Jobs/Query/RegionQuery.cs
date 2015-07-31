using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query
{
    [Serializable]
    [DataContract(Name = "Query", Namespace = "")]
    public class RegionQuery : SqlQuery 
    {
        #region Constructors and initializer

        protected RegionQuery()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public RegionQuery(RegionQuery old)
            : base(old)
        {
            CopyMembers(old);
        }

        public RegionQuery(Context context)
            : base(context)
        {
            InitializeMembers(new StreamingContext());
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
        }

        private void CopyMembers(RegionQuery old)
        {
        }

        public override object Clone()
        {
            return new RegionQuery(this);
        }

        #endregion
    }
}
