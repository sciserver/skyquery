﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Parsing;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public partial class XMatchSelectStatement
    {
        public XMatchQuerySpecification XMatchQuerySpecification
        {
            get { return QueryExpression.FindDescendant<XMatchQuerySpecification>(); }
        }
    }
}
