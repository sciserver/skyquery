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
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.VOTable;

namespace Jhu.SkyQuery.Format.VOTable.Test
{
    [TestClass]
    public class VOTableWriterTest : Jhu.Graywulf.Test.TestClassBase
    {
        [TestMethod]
        public void SimpleWriterTest()
        {
            var uri = GetTestFilename(".votable");

            using (var nat = new VOTable(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = IOTestDataset.CreateCommand("SELECT * FROM SampleData_NumericTypes", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleWriterNullsTest()
        {
            var uri = GetTestFilename(".votable");

            using (var nat = new VOTable(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = IOTestDataset.CreateCommand("SELECT * FROM SampleData_NumericTypes_Null", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleWriterAllTypesTest()
        {
            var uri = GetTestFilename(".votable");

            using (var nat = new VOTable(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = IOTestDataset.CreateCommand("SELECT * FROM SampleData_AllTypes", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleWriterAllTypesNullableTest()
        {
            var uri = GetTestFilename(".votable");

            using (var nat = new VOTable(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = IOTestDataset.CreateCommand("SELECT * FROM SampleData_AllTypes_Nullable", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }


    }
}
