using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    [TestClass]
    public class TapDatasetTest
    {

        //vizier, gaia  http://tapvizier.u-strasbg.fr/TAPVizieR/tap/
        private const string connectionString = "Data SOurce = http://tapvizier.u-strasbg.fr/TAPVizieR/tap/";
        //private const string connectionString = "Data Source = http://dc.zah.uni-heidelberg.de/__system__/tap/run/tap/";
        //private const string connectionString = "Data Source=http://gaia.ari.uni-heidelberg.de/tap/";

        protected TapDataset CreateTestDataset()
        {
            var csb = new TapConnectionStringBuilder(connectionString);

            var ds = new TapDataset(Jhu.Graywulf.Test.Constants.TestDatasetName, csb.ConnectionString);

            return ds;
        }

        [TestMethod]
        public void ConnectionTapTest()
        {
            var cn = new TapConnection(connectionString);
        }

        [TestMethod]
        public void GetSingleTableTest()
        {
            var ds = CreateTestDataset();

            var t = ds.Tables["", "", "'J/AJ/144/129/refs'"];

            Assert.IsTrue(ds.Tables.Count == 1);
            Assert.AreEqual("viz7", t.SchemaName);
            Assert.AreEqual("J/AJ/144/129/refs", t.ObjectName);
            Assert.AreEqual("References ( Cho S.-H., Kim J.)", t.Metadata.Summary);
        }

        [TestMethod]
        public void GetTapDataTest()
        {
            var ds = CreateTestDataset();

            ds.Tables.LoadAll(false);

            Assert.IsTrue(ds.Tables.Count > 0);

        }

        [TestMethod]
        public void Columntest1()
        {
            var ds = CreateTestDataset();

            var t = ds.Tables["", "", "'J/AJ/144/129/refs'"];
            Assert.IsTrue(t.Columns.Count > 0);
            Assert.AreEqual(t.Columns.Count, 5);
            Assert.AreEqual(t.ObjectName, "J/AJ/144/129/refs");
            Assert.AreEqual(t.Metadata.Summary, "Comments");
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

        [TestMethod]
        public void ColumntestGaia()
        {
            var ds = CreateTestDataset();

            var t = ds.Tables["", "", "'twomass'"];
            Assert.IsTrue(t.Columns.Count > 0);
            Assert.AreEqual(t.Columns.Count, 34);
            //TODO test if it can read binary
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