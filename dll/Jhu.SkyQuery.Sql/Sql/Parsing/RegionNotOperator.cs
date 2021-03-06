﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public partial class RegionNotOperator
    {
        private RegionOperatorType type;

        public override int Precedence
        {
            get { return 1; }
        }

        public override bool IsLeftAssociative
        {
            get { return false; }
        }

        protected override void OnInterpret()
        {
            base.OnInterpret();
            type = RegionOperatorType.Not;
        }
    }
}
