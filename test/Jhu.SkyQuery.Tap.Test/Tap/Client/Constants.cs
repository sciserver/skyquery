using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Tap.Client
{
    public class Constants
    {
        public const string TapBaseUrlVizier = "http://tapvizier.u-strasbg.fr/TAPVizieR/tap/";
        public const string TapConnectionStringVizier = "Data Source=" + TapBaseUrlVizier;

        public const string TapBaseUrlGaia = "http://gaia.ari.uni-heidelberg.de/tap/";
        public const string TapConnectionStringGaia = "Data Source=" + TapBaseUrlGaia;

        public const string TapBaseUrlHeasarc = "https://heasarc.gsfc.nasa.gov/cgi-bin/W3Browse/";
        public const string TapConnectionStringHeasarc = "Data Source=" + TapBaseUrlHeasarc;

        //private const string connectionString = "Data Source = http://dc.zah.uni-heidelberg.de/__system__/tap/run/tap/";
        //private const string connectionString = "Data Source=http://gaia.ari.uni-heidelberg.de/tap/";
    }
}
