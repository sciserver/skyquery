using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    [TestClass]
    public class TapCommandGaiaTest : TapCommandTest
    {
        protected override string ConnectionString
        {
            get { return Constants.TapConnectionStringGaia; }
        }

        [TestMethod]
        public void ExecuteReaderTest()
        {
            ExecuteReaderTestHelper("SELECT TOP 10 ra, dec FROM gaiadr1.tgas_source", 2, 10);
        }

        [TestMethod]
        public void InterruptReaderTest()
        {
            InterruptReaderTestHelper("SELECT TOP 10 ra, dec FROM gaiadr1.tgas_source", 2);
        }

        [TestMethod]
        public void FillDataSetTest()
        {
            FillDataSetTestHelper(
                "SELECT TOP 10 ra, dec FROM gaiadr1.tgas_source",
                new string[] { "ra", "dec" },
                10);
        }

        [TestMethod]
        public void SchemaTableTest()
        {
            SchemaTableTestHelper(
                "SELECT TOP 10 ra, dec FROM gaiadr1.tgas_source",
                new string[] { "ra", "dec" },
                new Type[] { typeof(double), typeof(double) });
        }

        [TestMethod]
        public void EmptyElementReaderTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void AllTypeDataTest()
        {
            throw new NotImplementedException();
        }

        // TODO: implement all test with different queries as in vizier
    }
}
