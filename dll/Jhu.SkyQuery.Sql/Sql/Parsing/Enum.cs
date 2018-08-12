using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Sql.Parsing
{
    public enum XMatchInclusionMethod : int
    {
        Unknown = -1,
        Must = 0,
        May = 1,
        Drop = 2,
    }

    public enum RegionOperatorType
    {
        Not,
        Union,
        Intersect,
        Except
    }
}
