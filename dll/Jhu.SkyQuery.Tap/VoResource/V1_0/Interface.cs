using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.VoResource.V1_0
{
    public class Interface
    {
        public AccessUrl[] AccessUrlList { get; set; }
        public SecurityMethod[] SecurityMethodList { get; set; }
        public string Version { get; set; }
        public string Role { get; set; }
    }
}
