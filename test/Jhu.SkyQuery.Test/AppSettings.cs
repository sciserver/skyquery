using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Jhu.SkyQuery.Test
{
    public static class AppSettings
    {
        public static string SkyQueryTestConnectionString
        {
            get
            {
                var cs = ConfigurationManager.ConnectionStrings["Jhu.SkyQuery..Test"];
                return cs != null ? cs.ConnectionString : null;
            }
        }
    }
}
