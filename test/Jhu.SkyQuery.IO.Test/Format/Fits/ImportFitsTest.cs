using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.Fits;

namespace Jhu.SkyQuery.Format.Fits.Test
{
    [TestClass]
    public class ImportFitsTest : Jhu.Graywulf.IO.Tasks.ImportTableTest
    {
        protected override FileFormatFactory CreateFileFormatFactory()
        {
            return FileFormatFactory.Create(typeof(SkyQueryFileFormatFactory).AssemblyQualifiedName);
        }

        [TestMethod]
        public void ImportSdssSpecTest()
        {
            var path = @"..\..\..\files\sdssdr10_specsdss.fits";
            var table = "ImportFitsTest_ImportSdssSpecTest";
            var it = GetImportTableTask(path, table, false);

            it.Execute();

            it.Destination.GetTable().Drop();
        }

        [TestMethod]
        public void ImportBossSpecTest()
        {
            var path = @"..\..\..\files\sdssdr10_specboss.fits";
            var table = "ImportFitsTest_ImportBossSpecTest";
            var it = GetImportTableTask(path, table, false);

            it.Execute();

            it.Destination.GetTable().Drop();
        }

    }
}
