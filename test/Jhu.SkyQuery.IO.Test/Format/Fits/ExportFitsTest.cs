using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Data;
using Jhu.SkyQuery.Format.Fits;

namespace Jhu.SkyQuery.Format.Fits
{
    [TestClass]
    public class ExportFitsTest : TestClassBase
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            StartLogger();
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            StopLogger();
        }
        
        [TestMethod]
        public void EmptyTableTest()
        {
            var ms = new MemoryStream();

            using (var cn = IOTestDataset.OpenConnection())
            {
                using (var cmd = new SmartCommand(IOTestDataset, cn.CreateCommand()))
                {
                    cmd.CommandText = "SELECT * FROM EmptyTable";

                    using (var dr = cmd.ExecuteReader())
                    {
                        var fits = new FitsFileWrapper(ms, DataFileMode.Write);
                        fits.WriteFromDataReader(dr);
                    }
                }
            }

        }
        
    }
}
