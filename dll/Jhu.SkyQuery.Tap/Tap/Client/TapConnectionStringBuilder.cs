using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapConnectionStringBuilder : DbConnectionStringBuilder
    {
        public string Url
        {
            get { return (string)this["Url"]; }
            set { this["Url"] = value; }
        }
    }
}
