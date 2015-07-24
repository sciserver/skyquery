using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery
{
    static class Copyright
    {
#if DEBUG
        public const string InfoConfiguration = "Debug";
#else
        public const string InfoConfiguration = "Release";
#endif
        public const string InfoProduct = "SkyQuery";
        public const string InfoCompany = "ELTE/JHU IDIES";
        public const string InfoCopyright = "Copyright ©  2008-2015 László Dobos, Eötvös University, The Johns Hopkins University";
        public const string InfoTrademark = "";
        public const string InfoCulture = "";
        public const string AssemblyVersion = "1.1.*";
    }
}
