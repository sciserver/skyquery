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
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.Fits;

namespace Jhu.SkyQuery.Format.Fits.Test
{
    [TestClass]
    public class FitsWriterTest : Jhu.Graywulf.Test.TestClassBase
    {
        [TestMethod]
        public void SimpleWriterTest()
        {
            var uri = GetTestFilename(".fits");

            using (var fits = new FitsFileWrapper(uri, DataFileMode.Write))
            {
                using (var cn = IOTestDataset.OpenConnection())
                {
                    using (var cmd = IOTestDataset.CreateCommand("SELECT * FROM SampleData_NumericTypes", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            fits.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }
    }
}
