using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Data;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.VoTable
{
    [TestClass]
    public class VOTableWriterTest : Jhu.Graywulf.Test.TestClassBase
    {
        [TestMethod]
        public void SimpleWriterTest()
        {
            var uri = GetTestUniqueFileUri(".votable");

            using (var nat = new VoTableWrapper(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = new SmartCommand(IOTestDataset, cn.CreateCommand()))
                    {
                        cmd.CommandText = "SELECT * FROM SampleData_NumericTypes";

                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReaderAsync(dr).Wait();
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleWriterNullsTest()
        {
            var uri = GetTestUniqueFileUri(".votable");

            using (var nat = new VoTableWrapper(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = new SmartCommand(IOTestDataset, cn.CreateCommand()))
                    {
                        cmd.CommandText = "SELECT * FROM SampleData_NumericTypes_Null";

                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReaderAsync(dr).Wait();
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleWriterAllTypesTest()
        {
            var uri = GetTestUniqueFileUri(".votable");

            using (var nat = new VoTableWrapper(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = new SmartCommand(IOTestDataset, cn.CreateCommand()))
                    {
                        cmd.CommandText = "SELECT * FROM SampleData_AllTypes";

                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReaderAsync(dr).Wait();
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleWriterAllTypesNullableTest()
        {
            var uri = GetTestUniqueFileUri(".votable");

            using (var nat = new VoTableWrapper(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = new SmartCommand(IOTestDataset, cn.CreateCommand()))
                    {
                        cmd.CommandText = "SELECT * FROM SampleData_AllTypes_Nullable";

                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReaderAsync(dr).Wait();
                        }
                    }
                }
            }
        }


    }
}
