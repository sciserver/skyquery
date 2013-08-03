using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Jhu.SkyQuery.Jobs.Query
{
    public static class AppSettings
    {
        private static string GetValue(string key)
        {
            return (string)((NameValueCollection)ConfigurationManager.GetSection("Jhu.SkyQuery/Jobs/Query"))[key];
        }

        public static string XMatchQueryJob
        {
            get { return GetValue("XMatchQueryJob"); }
        }

        public static int ZoneHeight
        {
            get { return int.Parse(GetValue("ZoneHeight")); }
        }

    }
}
