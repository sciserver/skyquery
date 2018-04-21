﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Sql.Parsing;

namespace Jhu.SkyQuery.Parser
{
    public class BayesFactorXMatchQuerySpecification : XMatchQuerySpecification
    {
        #region Constructors and initializers

        public BayesFactorXMatchQuerySpecification()
            : base()
        {
        }

        public BayesFactorXMatchQuerySpecification(XMatchQuerySpecification xqs)
            :base(xqs)
        {
        }

        public BayesFactorXMatchQuerySpecification(BayesFactorXMatchQuerySpecification old)
            :base(old)
        {
        }

        public override object Clone()
        {
            return new BayesFactorXMatchQuerySpecification(this);
        }

        #endregion
    }
}
