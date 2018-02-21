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
            ExecuteReaderTestHelper(
@"SELECT TOP 20 ""source_id"", ""tycho2_id""
FROM gaiadr1.tgas_source ",
                2,
                20);
        }

        [TestMethod]
        public void AllTypeDataTest()
        {
            InterruptReaderTestHelper(
@"SELECT TOP 10 ""source_id"", ""ra"",""astrometric_delta_q"", ""astrometric_n_bad_obs_al"",  ""matched_observations"" ,""tycho2_id""
FROM gaiadr1.tgas_source", 6);
        }

        // TODO: implement all test with different queries as in vizier
    }
}
