using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Jhu.Graywulf.Util;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapClient : IDisposable
    {
        private Uri baseAddress;
        private TimeSpan httpTimeout;
        private TimeSpan pollTimeout;

        private HttpClient httpClient;
        private CancellationToken cancellationToken;

        public Uri BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }

        public TapClient()
        {
            InitializeMembers();
        }

        public TapClient(Uri baseAddress)
        {
            InitializeMembers();

            this.baseAddress = baseAddress;
            this.httpTimeout = TimeSpan.FromSeconds(Constants.DefaultHttpTimeout);
            this.pollTimeout = TimeSpan.FromSeconds(Constants.DefaultPollTimeout);

            this.httpClient = null;
            this.cancellationToken = CancellationToken.None;
        }

        private void InitializeMembers()
        {
            this.baseAddress = null;

            this.httpClient = null;
        }

        public void Dispose()
        {
            if (httpClient != null)
            {
                httpClient.Dispose();
                httpClient = null;
            }
        }

        private void CreateHttpClient()
        {
            var h = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
            };

            httpClient = new HttpClient(h, true);
            httpClient.BaseAddress = baseAddress;
            httpClient.Timeout = httpTimeout;
        }

        private void EnsureHttpClientCreated()
        {
            if (httpClient == null)
            {
                CreateHttpClient();
            }
        }

        private async Task<HttpResponseMessage> HttpPostAsync(Uri address, HttpContent content, HttpStatusCode expectedStatus)
        {
            EnsureHttpClientCreated();

            var result = await httpClient.PostAsync(address, content, cancellationToken);

            if (result.StatusCode != expectedStatus)
            {
                throw Error.UnexpectedHttpResponse(result.StatusCode, result.ReasonPhrase);
            }

            return result;
        }

        private async Task<string> HttpGetAsync(Uri address, HttpStatusCode expectedStatus = HttpStatusCode.OK)
        {
            EnsureHttpClientCreated();

            var result = await httpClient.GetAsync(address, cancellationToken);

            if (result.StatusCode != expectedStatus)
            {
                throw Error.UnexpectedHttpResponse(result.StatusCode, result.ReasonPhrase);
            }

            return await result.Content.ReadAsStringAsync();
        }

        private async Task<System.IO.Stream> HttpGetStreamAsync(Uri address, HttpStatusCode expectedStatus = HttpStatusCode.OK)
        {
            EnsureHttpClientCreated();

            var result = await httpClient.GetAsync(address, cancellationToken);

            if (result.StatusCode != expectedStatus)
            {
                throw Error.UnexpectedHttpResponse(result.StatusCode, result.ReasonPhrase);
            }

            return await result.Content.ReadAsStreamAsync();
        }

        public async Task SubmitAsync(TapJob job)
        {
            EnsureHttpClientCreated();

            var parameters = job.GetSubmitRequestParameters();
            var content = new FormUrlEncodedContent(parameters);
            var address = UriConverter.Combine(baseAddress, Constants.TapActionAsync);
            var result = await HttpPostAsync(address, content, HttpStatusCode.RedirectMethod);

            job.Uri = result.Headers.Location;
        }

        public async Task GetPhaseAsync(TapJob job)
        {
            var address = UriConverter.Combine(job.Uri, Constants.TapActionAsyncPhase);
            var phasestr = await HttpGetAsync(address);

            TapJobPhase phase;
            if (!Enum.TryParse(phasestr, true, out phase))
            {
                throw Error.UnexpectedPhase(phasestr);
            }

            job.Phase = phase;
        }

        public async Task SetPhaseAsync(TapJob job)
        {
            throw new NotImplementedException();
        }

        public async Task GetQuoteAsync(TapJob job)
        {
            throw new NotImplementedException();
        }

        public async Task GetExecutionDurationAsync(TapJob job)
        {
            throw new NotImplementedException();
        }

        public async Task SetExecutionDurationAsync(TapJob job)
        {
            throw new NotImplementedException();
        }

        public async Task GetDesctructionAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SetDescrtructionAsync(TapJob job)
        {
            throw new NotImplementedException();
        }

        public async Task GetErrorAsync(TapJob job)
        {
            throw new NotImplementedException();
        }

        public async Task<System.IO.Stream> GetResultsAsync(TapJob job)
        {
            var address = UriConverter.Combine(job.Uri, Constants.TapActionAsyncResults);
            return await HttpGetStreamAsync(address);
        }

        public async Task PollAsync(TapJob job, IList<TapJobPhase> expectedPhase, IList<TapJobPhase> errorPhase = null)
        {
            var limit = DateTime.Now + pollTimeout;
            var interval = 1000; // start from one second polling interval

            // Poll job until status is the expected
            while (DateTime.Now < limit)
            {
                await GetPhaseAsync(job);

                if (expectedPhase.Contains(job.Phase))
                {
                    return;
                }
                else if ((errorPhase == null && job.Phase == TapJobPhase.Error) ||
                         (errorPhase != null && errorPhase.Contains(job.Phase)))
                {
                    throw Error.UnexpectedPhase(job.Phase.ToString());
                }

                // Wait and back-off logarithmically until ~30 sec polling time is reached
                await Task.Delay(interval, cancellationToken);
                
                if (interval < 20000)
                {
                    interval = (int)Math.Floor(1.4142 * interval);
                }
            }
        }
    }
}
