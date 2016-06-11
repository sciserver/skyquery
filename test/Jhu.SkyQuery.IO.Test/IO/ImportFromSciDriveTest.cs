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
            ImportFileHelper("skyquery_io_test/mycatalog.txt", "ImportMyCatalogTxtTest");
        }

#if false
        [TestMethod]
        public void ImportCompressedFromSciDriveTest()
        {
            ImportFileHelper("graywulf_io_test/csv_numbers.csv.gz", "ImportCompressedFromSciDriveTest");
        }

        [TestMethod]
        public void ImportArchiveFromSciDriveTest()
        {
            ImportFileHelper("graywulf_io_test/csv_numbers.zip", "ImportArchiveFromSciDriveTest");
        }
#endif
    }
}
