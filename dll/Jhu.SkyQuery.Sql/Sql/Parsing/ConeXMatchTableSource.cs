using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public class ConeXMatchTableSource : XMatchTableSource
    {
        #region Constructors and initialiters

        public ConeXMatchTableSource()
        {
        }

        public ConeXMatchTableSource(XMatchTableSource old)
            :base(old)
        {
        }

        public ConeXMatchTableSource(ConeXMatchTableSource old)
            : base(old)
        {
        }

        public override object Clone()
        {
            return new ConeXMatchTableSource(this);
        }

        #endregion

        protected override void OnInterpret()
        {
            TableReference = new NameResolution.ConeXMatchTableReference()
            {
                Alias = this.Alias
            };
        }
    }
}
