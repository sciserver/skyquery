using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.ServiceModel.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;
using Jhu.Graywulf.Tasks;
using Jhu.Graywulf.Web.Services;
using Jhu.Graywulf.Web.Api.V1;
using Jhu.SkyQuery.Format.Fits;

namespace Jhu.SkyQuery.Format.Fits
{
    [TestClass]
    public class UploadFitsTest : DataServiceTest
    {
        [TestMethod]
        public void UploadSimpleFitsTest()
        {
            // This test also demonstrates how to call a WCF REST service from a standard
            // WCF client with custom headers. To access WebOperationContext from a client an
            // OperationContextScope needs to be created

            var path = GetTestFilePath(@"modules\skyquery\test\files\XMM_xray_errors.fits");

            using (var session = new RestClientSession())
            {
                var client = CreateClient(session);

                // Try to drop the table before uploading a new one
                try
                {
                    client.DropTable("MYDB", "XMM_xray_errors");
                }
                catch (Exception) { }

                using (var scope = new OperationContextScope((IContextChannel)client))
                {
                    WebOperationContext.Current.OutgoingRequest.ContentType = Jhu.SkyQuery.Format.Fits.Constants.MimeTypeFits;

                    using (var infile = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        client.UploadTable("MYDB", "XMM_xray_errors", infile);
                    }
                }

                // Drop the table that just has been created
                client.DropTable("MYDB", "XMM_xray_errors");
            }
        }
    }
}
