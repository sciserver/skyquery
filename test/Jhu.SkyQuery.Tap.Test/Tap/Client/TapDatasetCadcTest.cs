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
        public class TapDatasetCadcTest : TapDatasetTest
        {
            protected override string ConnectionString
            {
                get { return Constants.TapConnectionStringCadc; }
            }

            [TestMethod]
            public async Task OpenConnectionTest()
            {
                await CreateTestDataset().OpenConnectionAsync(CancellationToken.None);
            }

            [TestMethod]
            public void LoadAllTablesTest()
            {
                LoadAllTablesTestHelper();
            }

        }
    }
