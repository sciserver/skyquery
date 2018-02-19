using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    [TestClass]
    public class TapClientHeasarcTest : TapClientTest
    {
        protected override Uri BaseUri
        {
            get
            {
                return new Uri(Constants.TapBaseUrlHeasarc);
            }
        }

        // TODO: implement test similar to vizier
    }
}
