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
    public class EndpointSkymapperTest : TapTestBase
    {
        protected override Uri BaseUri
        {
            get { return new Uri("http://skymappertap.asvo.nci.org.au/ncitap/tap/"); }
        }
    }
}
