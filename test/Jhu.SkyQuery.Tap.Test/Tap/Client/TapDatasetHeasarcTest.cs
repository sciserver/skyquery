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
    public class TapDatasetHeasarcTest : TapDatasetTest
    {
        protected override string ConnectionString
        {
            get { return Constants.TapConnectionStringHeasarc; }
        }

        [TestMethod]
        public async Task OpenConnectionTest()
        {
            // NASA service provides wrong URL for TAPRegExt schema

            await CreateTestDataset().OpenConnectionAsync(CancellationToken.None);
        }

        [TestMethod]
        public void LoadAllTablesTest()
        {
            LoadAllTablesTestHelper();
        }
    }
}
