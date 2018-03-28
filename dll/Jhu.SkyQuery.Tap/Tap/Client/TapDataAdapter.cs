using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapDataAdapter : DbDataAdapter
    {
        public TapDataAdapter()
        {
        }

        public TapDataAdapter(TapDataAdapter da)
            : base(da)
        {
        }
    }
}
