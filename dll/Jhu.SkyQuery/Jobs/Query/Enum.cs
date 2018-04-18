using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Jobs.Query
{
    public enum CreateZoneTableReason
    {
        None,
        NoZoneIndexFound,
        RegionFilterSpecified
    }
}
