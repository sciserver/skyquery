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

        public const string TapBaseUrlHeasarc = "https://heasarc.gsfc.nasa.gov/xamin/vo/tap/";
        public const string TapConnectionStringHeasarc = "Data Source=" + TapBaseUrlHeasarc;

        public const string TapBaseUrlGavo = "http://dc.zah.uni-heidelberg.de/__system__/tap/run/tap/";
        public const string TapConnectionStringGavo = "Data Source=" + TapBaseUrlGavo;

        public const string TapBaseUrlCadc = "http://www.cadc-ccda.hia-iha.nrc-cnrc.gc.ca/tap/";
        public const string TapConnectionStringCadc = "Data Source=" + TapBaseUrlCadc;

        public const string TapBaseUrlAadc = "https://vo.phys.au.dk/__system__/tap/run/tap/";
        public const string TapConnectionStringAadc = "Data Source=" + TapBaseUrlAadc;

        public const string TapBaseUrlSkymapper = "http://skymappertap.asvo.nci.org.au/ncitap/tap/";
        public const string TapConnectionStringSkymapper = "Data Source=" + TapBaseUrlSkymapper;

        public const string TapBaseUrlNoao = "http://datalab.noao.edu/tap";
        public const string TapConnectionStringNoao = "Data Source=" + TapBaseUrlNoao;
    }
}
