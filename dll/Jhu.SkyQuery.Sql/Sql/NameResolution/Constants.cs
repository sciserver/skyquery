using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Sql.NameResolution
{
    class Constants
    {
        public static readonly HashSet<string> SkyQuerySystemFunctionNames = new HashSet<string>(Jhu.Graywulf.Sql.Schema.SchemaManager.Comparer)
        {
            "POINT"
        };
    }
}
