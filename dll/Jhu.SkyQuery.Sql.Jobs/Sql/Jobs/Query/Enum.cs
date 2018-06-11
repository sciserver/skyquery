using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    public enum CreateZoneTableReason
    {
        None,
        NoZoneIndexFound,
        RegionFilterSpecified
    }
}
