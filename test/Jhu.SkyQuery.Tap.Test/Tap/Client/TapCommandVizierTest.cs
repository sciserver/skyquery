﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    [TestClass]
    public class TapCommandVizierTest : TapCommandTest
    {
        protected override string ConnectionString
        {
            get { return Constants.TapConnectionStringVizier; }
        }

        [TestMethod]
        public void ExecuteReaderTest()
        {
            ExecuteReaderTestHelper("SELECT TOP 10 ra, dec FROM \"I/337/gaia\"", 2, 10);
        }

        [TestMethod]
        public void InterruptReaderTest()
        {
            InterruptReaderTestHelper("SELECT TOP 10 ra, dec FROM \"I/337/gaia\"", 2);
        }

        [TestMethod]
        public void FillDataSetTest()
        {
            FillDataSetTestHelper(
                "SELECT TOP 10 ra, dec FROM \"I/337/gaia\"",
                new string[] { "ra", "dec" },
                10);
        }

        [TestMethod]
        public void SchemaTableTest()
        {
            SchemaTableTestHelper(
                "SELECT TOP 10 ra, dec FROM \"I/337/gaia\"",
                new string[] { "ra", "dec" },
                new Type[] { typeof(double), typeof(double) });
        }

        [TestMethod]
        public void EmptyElementReaderTest()
        {
            ExecuteReaderTestHelper(
@"SELECT TOP 20  ""J/ApJ/678/102/table4"".""S/N"",  ""J/ApJ/678/102/table4"".OID, ""J/ApJ/678/102/table4"".f_OID,
""J/ApJ/678/102/table4"".Off,  ""J/ApJ/678/102/table4"".Type,  ""J/ApJ/678/102/table4"".Cat,  
""J/ApJ/678/102/table4"".n_Cat,  ""J/ApJ/678/102/table4"".Ref FROM ""J/ApJ/678/102/table4"" ",
                8,
                20);
            //Van egy üres sor, ahol nincsenek elemek és ott elvágódik
        }

        [TestMethod]
        public void AllTypeDataTest()
        {
            InterruptReaderTestHelper(
@"SELECT TOP 100 ""I/239/hip_main"".""(V-I)red"", ""I/239/hip_main"".""r_B-V"", ""I/239/hip_main"".BD, ""I/239/hip_main"".F1,
""I/239/hip_main"".HD, ""I/239/hip_main"".SpType
 FROM ""I/239/hip_main""", 6);
            // Only returns double, char(1), char(10), smallint, int, varchar;
        }

        // TODO: add test:
        // - read data with all possible column types 
        // int, short,long, float, double, char(-string) already tested
        // - read data with nulls in all possible column types
        // - test error behavior (e.g. syntax error in query, etc.)
    }
}