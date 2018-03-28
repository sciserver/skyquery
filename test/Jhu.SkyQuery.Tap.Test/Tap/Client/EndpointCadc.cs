using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    [TestClass]
    public class EndpointCadcTest : TapTestBase
    {
        // Server implementation available at https://github.com/opencadc/uws

        protected override Uri BaseUri
        {
            get { return new Uri("http://www.cadc-ccda.hia-iha.nrc-cnrc.gc.ca/tap/"); }
        }
    }
}
