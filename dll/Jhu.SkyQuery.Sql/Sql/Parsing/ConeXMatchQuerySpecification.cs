using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.Parsing;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public class ConeXMatchQuerySpecification : XMatchQuerySpecification
    {
        #region Constructors and initializers

        public ConeXMatchQuerySpecification()
            : base()
        {
        }

        public ConeXMatchQuerySpecification(XMatchQuerySpecification xqs)
            :base(xqs)
        {
        }

        public ConeXMatchQuerySpecification(ConeXMatchQuerySpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new ConeXMatchQuerySpecification(this);
        }

        #endregion
    }
}
