using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.IO.Test.IO
{
    [TestClass]
    public class ImportFromSciDriveTest : Jhu.Graywulf.SciDrive.ImportTest
    {
        [TestMethod]
        public void ImportSciDriveMyCatalogCsvTest()
        {
            ImportFileHelper("skyquery_io_test/mycatalog.txt", false);
        }
    }
}
