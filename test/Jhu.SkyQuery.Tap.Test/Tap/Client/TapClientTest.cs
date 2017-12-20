using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    public abstract class TapClientTest
    {
        protected abstract Uri BaseUri { get; }

        protected VO.Vosi.Availability.V1_0.Availability GetAvailabilityTestHelper()
        {
            using (var tap = new TapClient(BaseUri))
            {
                return tap.GetAvailabilityAsync(CancellationToken.None).Result;
            }
        }

        protected VO.Vosi.Capabilities.V1_0.Capabilities GetCapabilitiesTestHelper()
        {
            using (var tap = new TapClient(BaseUri))
            {
                return tap.GetCapabilitiesAsync(CancellationToken.None).Result;
            }
        }

        protected void SubmitQueryAsyncTestHelper(string query, string format)
        {
            using (var tap = new TapClient(BaseUri))
            {
                var job = new TapJob()
                {
                    Query = query,
                    Format = format,
                };

                tap.SubmitAsync(job, CancellationToken.None).Wait();
                tap.PollAsync(job, new[] { TapJobPhase.Completed }, null, CancellationToken.None).Wait();

                var stream = tap.GetResultsAsync(job, CancellationToken.None).Result;
                var reader = new System.IO.StreamReader(stream);
                var data = reader.ReadToEnd();
            }
        }

        protected string GetBestFormatMimeTypeTestHelper()
        {
            using (var tap = new TapClient(BaseUri))
            {
                tap.GetCapabilitiesAsync(CancellationToken.None).Wait();
                tap.GetBestFormat(out TapOutputFormat format, out string mime);

                return mime;
            }
        }
    }
}
