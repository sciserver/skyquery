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
    public class TapDatasetGavoTest : TapDatasetTest
    {
        protected override string ConnectionString
        {
            get { return Constants.TapConnectionStringGavo; }
            //get { return Constants.TapConnectionStringNoao; }
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

        [TestMethod]
        public void ColumntestGavo()
        {
            var ds = CreateTestDataset();

            var t = ds.Tables["", "", "ohmaser.masers"];
            Assert.IsTrue(t.Columns.Count > 0);
        }
    }
}
