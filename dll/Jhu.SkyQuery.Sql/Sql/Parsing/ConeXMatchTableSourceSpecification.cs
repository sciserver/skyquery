using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public class ConeXMatchTableSourceSpecification : XMatchTableSourceSpecification
    {
        #region Constructors and initialiters

        public ConeXMatchTableSourceSpecification()
        {
        }

        public ConeXMatchTableSourceSpecification(XMatchTableSourceSpecification old)
            :base(old)
        {
        }

        public ConeXMatchTableSourceSpecification(ConeXMatchTableSourceSpecification old)
            : base(old)
        {
        }

        public override object Clone()
        {
            return new ConeXMatchTableSourceSpecification(this);
        }

        #endregion

        public override void Interpret()
        {
 	        base.Interpret();

            TableReference = new ConeXMatchTableReference()
            {
                Alias = this.Alias
            };
        }
    }
}
