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
    public class TapDatasetGaiaTest : TapDatasetTest
    {
        protected override string ConnectionString
        {
            get { return Constants.TapConnectionStringGaia; }
        }

        [TestMethod]
        public async Task OpenConnectionTest()
        {
            await CreateTestDataset().OpenConnectionAsync(CancellationToken.None);
        }

        [TestMethod]
        public void GetSingleTableTest()
        {
            var ds = CreateTestDataset();
            var t = ds.Tables["", "extcat", "twomass"];
        }

        [TestMethod]
        public void LoadAllTablesTest()
        {
            LoadAllTablesTestHelper();
        }

        [TestMethod]
        public void LoadColumnsTest()
        {
            var ds = CreateTestDataset();
            var t = ds.Tables["", "extcat", "twomass"];
            Assert.IsTrue(t.Columns.Count > 0);
            Assert.AreEqual(t.Columns.Count, 34);
        }
    }
}
