using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.Fits;
using Jhu.Graywulf.Test;

namespace Jhu.SkyQuery.Format.Fits.Test
{
    [TestClass]
    public class ImportFitsTest : Jhu.Graywulf.IO.Tasks.ImportTableTest
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

        protected override FileFormatFactory CreateFileFormatFactory()
        {
            return FileFormatFactory.Create(typeof(SkyQueryFileFormatFactory).AssemblyQualifiedName);
        }

        [TestMethod]
        public void ImportSdssSpecTest()
        {
            var path = GetTestFilePath(@"skyquery/test/files/sdssdr10_specsdss.fits");
            var it = GetImportTableTask(path, false, false);
            var t = ExecuteImportTableTask(it);
            Assert.AreEqual(8, t.Columns.Count);
            DropTable(t);
        }

        [TestMethod]
        public void ImportBossSpecTest()
        {
            var path = GetTestFilePath(@"skyquery/test/files/sdssdr10_specboss.fits");
            var it = GetImportTableTask(path, false, false);
            var t = ExecuteImportTableTask(it);
            Assert.AreEqual(8, t.Columns.Count);
            DropTable(t);
        }

    }
}
