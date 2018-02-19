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
    public class TapDatasetVizierTest : TapDatasetTest
    {
        protected override string ConnectionString
        {
            get { return Constants.TapConnectionStringVizier; }
        }

        [TestMethod]
        public async Task OpenConnectionTest()
        {
            await CreateTestDataset().OpenConnectionAsync(CancellationToken.None);
        }

        [TestMethod]
        public void GetSingleTableTest()
        {
            var t = GetSingleTableTestHelper("", "", "J/AJ/144/129/refs");

            Assert.AreEqual("viz7", t.SchemaName);
            Assert.AreEqual("J/AJ/144/129/refs", t.ObjectName);
            Assert.AreEqual("References ( Cho S.-H., Kim J.)", t.Metadata.Summary);
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

            var t = ds.Tables["", "", "'J/AJ/144/129/refs'"];
            Assert.IsTrue(t.Columns.Count > 0);
            Assert.AreEqual(5, t.Columns.Count);
            Assert.AreEqual("J/AJ/144/129/refs", t.ObjectName);
            Assert.AreEqual("References ( Cho S.-H., Kim J.)", t.Metadata.Summary);
        }

        [TestMethod]
        public void ColumntestCoRoTBright()
        {
            var ds = CreateTestDataset();

            var t = ds.Tables["", "", "'J/ApJ/678/102/table4'"];
            Assert.IsTrue(t.Columns.Count > 0);
            Assert.AreEqual(t.Columns.Count, 16);
            Assert.AreEqual(t.Columns["Ref"].Metadata.Summary, "Reference code for XRT position (4)");
            Assert.AreEqual(t.Columns["Type"].Metadata.Summary, "Source type");
            //          Assert.AreEqual(t.Columns["Type"].Metadata.Unit, "" );
            //Assert.AreEqual(t.Columns["Type"].Metadata.Quantity ,  "src.class");
            //            Assert.AreEqual(t.Columns["Type"].Metadata.Class, "" );
            Assert.AreEqual(t.ObjectName, "Ref");
            Assert.AreEqual(t.Metadata.Summary, "Reference code for XRT position (4)");
        }

        /*
        [TestMethod]
        public void ColumntestGavo()
        {
            var ds = CreateTestDataset();

            var t = ds.Tables["", "", "'ohmaser.masers'"];
            Assert.IsTrue(t.Columns.Count > 0);

        }
        */
    }
}
