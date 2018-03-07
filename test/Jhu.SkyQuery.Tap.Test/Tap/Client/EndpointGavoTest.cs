using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    [TestClass]
    public class EndpointGavoTest : TapTestBase
    {
        protected override Uri BaseUri
        {
            get { return new Uri("http://dc.zah.uni-heidelberg.de/__system__/tap/run/tap/"); }
        }
    }
}
