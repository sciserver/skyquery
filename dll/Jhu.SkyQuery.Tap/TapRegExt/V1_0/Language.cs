using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.TapRegExt.V1_0
{
    public class Language
    {
        public string Name { get; set; }
        public Version[] VersionList { get; set; }
        public string Description { get; set; }
        public LanguageFeatureList LanguageFeatures { get; set; }
    }
}
