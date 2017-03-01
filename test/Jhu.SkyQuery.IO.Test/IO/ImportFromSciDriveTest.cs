using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.IO.Test.IO
{
    [TestClass]
    public class ImportFromSciDriveTest : Jhu.Graywulf.SciDrive.ImportTest
    {
        [ClassInitialize]
        public new static void Initialize(TestContext context)
        {
            Jhu.Graywulf.Web.Api.V1.ImportTest.Initialize(context);
        }

        [ClassCleanup]
        public new static void CleanUp()
        {
            Jhu.Graywulf.Web.Api.V1.ImportTest.CleanUp();
        }

        [TestMethod]
        public void ImportSciDriveMyCatalogCsvTest()
        {
            ImportFileHelper("skyquery_io_test/mycatalog.txt", false);
        }
    }
}
