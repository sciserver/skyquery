﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Parser
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
