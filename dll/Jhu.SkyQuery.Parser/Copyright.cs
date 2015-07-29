using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery
{
    public static class Copyright
    {
#if DEBUG
        public const string InfoConfiguration = "Debug";
#else
        public const string InfoConfiguration = "Release";
#endif
        public const string InfoProduct = "SkyQuery";
        public const string InfoCompany = "ELTE/JHU IDIES";
        public const string InfoCopyright = "Copyright (c) 2008-2015 László Dobos, IDIES, The Johns Hopkins University, Eötvös University";
        public const string InfoTrademark = "";
        public const string InfoCulture = "";
        public const string AssemblyVersion = "1.0.*";
    }
}
