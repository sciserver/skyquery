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

        protected VO.Vosi.Availability.Common.IAvailability GetAvailabilityTestHelper()
        {
            using (var tap = new TapClient(BaseUri))
            {
                return Jhu.Graywulf.Util.TaskHelper.Wait(tap.GetAvailabilityAsync(CancellationToken.None));
            }
        }

        protected VO.Vosi.Capabilities.Common.ICapabilities GetCapabilitiesTestHelper()
        {
            using (var tap = new TapClient(BaseUri))
            {
                return Jhu.Graywulf.Util.TaskHelper.Wait(tap.GetCapabilitiesAsync(CancellationToken.None));
            }
        }

        protected async Task SubmitQueryAsyncTestHelper(string query, string format)
        {
            using (var tap = new TapClient(BaseUri))
            {
                var job = new TapJob()
                {
                    Query = query,
                    Format = format,
                };

                await tap.SubmitAsync(job, CancellationToken.None);
                await tap.PollAsync(job, new[] { TapJobPhase.Completed }, null, CancellationToken.None);
                
                var stream = await tap.GetResultsAsync(job, CancellationToken.None);
                var reader = new System.IO.StreamReader(stream);
                var data = reader.ReadToEnd();
            }
        }

        protected async Task SubmitQuerySyncTestHelper(string query, string format)
        {
            using (var tap = new TapClient(BaseUri))
            {
                var job = new TapJob()
                {
                    Query = query,
                    Format = format,
                    IsAsync = false,
                };

                var result = await tap.SubmitAsync(job, CancellationToken.None);
                var stream = await result.Content.ReadAsStreamAsync();
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
