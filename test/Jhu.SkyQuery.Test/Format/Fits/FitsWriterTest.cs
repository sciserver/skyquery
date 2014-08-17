using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Data;
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.Fits;

namespace Jhu.SkyQuery.Format.Fits.Test
{
    [TestClass]
    public class FitsWriterTest : Jhu.Graywulf.Test.TestClassBase
    {
        private void WriteTestHelper(string table)
        {
            var uri = GetTestFilename(".fits");

            using (var fits = new FitsFileWrapper(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = new SmartCommand(IOTestDataset, cn.CreateCommand()))
                    {
                        cmd.CommandText = "SELECT * FROM " + table;
                        
                        using (var dr = cmd.ExecuteReader())
                        {
                            fits.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void WriteNumericTypes()
        {
            WriteTestHelper("SampleData_NumericTypes");
        }

        [TestMethod]
        public void WriteNumericTypesNull()
        {
            WriteTestHelper("SampleData_NumericTypes_Null");
        }

        [TestMethod]
        public void WriteAllTypes()
        {
            WriteTestHelper("SampleData_AllTypes");
        }

        [TestMethod]
        public void WriteAllTypesNull()
        {
            WriteTestHelper("SampleData_AllTypes_Nullable");
        }

        [TestMethod]
        public void WriteAllPrecision()
        {
            WriteTestHelper("SampleData_AllPrecision");
        }
    }
}
