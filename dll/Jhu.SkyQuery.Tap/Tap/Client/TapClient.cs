using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;
using Jhu.Graywulf.Util;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapClient : IDisposable
    {
        private Uri baseAddress;
        private TimeSpan httpTimeout;
        private TimeSpan pollTimeout;

        private HttpClient httpClient;

        public Uri BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }

        public TimeSpan HttpTimeout
        {
            get { return httpTimeout; }
            set { httpTimeout = value; }
        }

        public TimeSpan PollTimeout
        {
            get { return pollTimeout; }
            set { pollTimeout = value; }
        }

        public TapClient()
        {
            InitializeMembers();
        }

        public TapClient(Uri baseAddress)
        {
            InitializeMembers();

            this.baseAddress = baseAddress;
        }

        private void InitializeMembers()
        {
            this.baseAddress = null;
            this.httpTimeout = TimeSpan.FromSeconds(Constants.DefaultHttpTimeout);
            this.pollTimeout = TimeSpan.FromSeconds(Constants.DefaultPollTimeout);

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

        private async Task<HttpResponseMessage> HttpPostAsync(Uri address, HttpContent content, HttpStatusCode expectedStatus, CancellationToken cancellationToken)
        {
            EnsureHttpClientCreated();

            var result = await httpClient.PostAsync(address, content, cancellationToken);

            if (result.StatusCode != expectedStatus)
            {
                throw Error.UnexpectedHttpResponse(result.StatusCode, result.ReasonPhrase);
            }

            return result;
        }

        private async Task<string> HttpGetAsync(Uri address, HttpStatusCode expectedStatus, CancellationToken cancellationToken)
        {
            EnsureHttpClientCreated();

            var result = await httpClient.GetAsync(address, cancellationToken);

            if (result.StatusCode != expectedStatus)
            {
                throw Error.UnexpectedHttpResponse(result.StatusCode, result.ReasonPhrase);
            }

            return await result.Content.ReadAsStringAsync();
        }

        private async Task<System.IO.Stream> HttpGetStreamAsync(Uri address, HttpStatusCode expectedStatus, CancellationToken cancellationToken)
        {
            EnsureHttpClientCreated();

            var result = await httpClient.GetAsync(address, cancellationToken);

            if (result.StatusCode != expectedStatus)
            {
                throw Error.UnexpectedHttpResponse(result.StatusCode, result.ReasonPhrase);
            }

            return await result.Content.ReadAsStreamAsync();
        }

        private async Task<XmlReader> HttpGetXmlReaderAsync(Uri address, HttpStatusCode expectedStatus, CancellationToken cancellationToken)
        {
            var stream = await HttpGetStreamAsync(address, HttpStatusCode.OK, cancellationToken);
            var xml = XmlReader.Create(stream);
            return xml;
        }

        private async Task<T> HttpGetObject<T>(Uri address, CancellationToken cancellationToken)
        {
            using (var reader = await HttpGetXmlReaderAsync(address, HttpStatusCode.OK, cancellationToken))
            {
                var s = new XmlSerializer(typeof(T));
                return (T)s.Deserialize(reader);
            }
        }

        public async Task<Vosi.Availability.V1_0.Availability> GetAvailabilityAsync(CancellationToken cancellationToken)
        {
            var address = UriConverter.Combine(baseAddress, Constants.TapCommandAvailability);
            return await HttpGetObject<Vosi.Availability.V1_0.Availability>(address, cancellationToken);
        }

        public async Task<Vosi.Capabilities.V1_0.Capabilities> GetCapabilitiesAsync(CancellationToken cancellationToken)
        {
            var address = UriConverter.Combine(baseAddress, Constants.TapCommandCapabilities);
            return await HttpGetObject<Vosi.Capabilities.V1_0.Capabilities>(address, cancellationToken);
        }

        private async Task<string> GetParameterAsync(TapJob job, string action, string parameter, CancellationToken cancellationToken)
        {
            var address = UriConverter.Combine(job.Uri, action);
            return await HttpGetAsync(address, HttpStatusCode.OK, cancellationToken);
        }

        private async Task SetParameterAsync(TapJob job, string action, string parameter, string value, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SubmitAsync(TapJob job, CancellationToken cancellationToken)
        {
            EnsureHttpClientCreated();

            var parameters = job.GetSubmitRequestParameters();
            var content = new FormUrlEncodedContent(parameters);
            var address = UriConverter.Combine(baseAddress, Constants.TapActionAsync);
            var result = await HttpPostAsync(address, content, HttpStatusCode.RedirectMethod, cancellationToken);

            job.Uri = result.Headers.Location;
        }

        private async Task GetPhaseAsync(TapJob job, CancellationToken cancellationToken)
        {
            var phasestr = await GetParameterAsync(job, Constants.TapActionAsyncPhase, Constants.TapParamPhase, cancellationToken);

            TapJobPhase phase;
            if (!Enum.TryParse(phasestr, true, out phase))
            {
                throw Error.UnexpectedPhase(phasestr);
            }

            job.Phase = phase;
        }

        
        public async Task SetPhaseAsync(TapJob job, CancellationToken cancellationToken)
        {
            await SetParameterAsync(job, Constants.TapActionAsyncPhase, Constants.TapParamPhase, job.Phase.ToString().ToUpperInvariant(), cancellationToken);
        }

        public async Task GetQuoteAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        public async Task GetExecutionDurationAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetExecutionDurationAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task GetDestructionAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetDestructionAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<System.IO.Stream> GetResultsAsync(TapJob job, CancellationToken cancellationToken)
        {
            var address = UriConverter.Combine(job.Uri, Constants.TapActionAsyncResults);
            return await HttpGetStreamAsync(address, HttpStatusCode.OK, cancellationToken);
        }

        public async Task<System.IO.Stream> GetErrorAsync(TapJob job, CancellationToken cancellationToken)
        {
            var address = UriConverter.Combine(job.Uri, Constants.TapActionAsyncError);
            return await HttpGetStreamAsync(address, HttpStatusCode.OK, cancellationToken);
        }

        public async Task PollAsync(TapJob job, IList<TapJobPhase> expectedPhase, IList<TapJobPhase> errorPhase, CancellationToken cancellationToken)
        {
            var limit = DateTime.Now + pollTimeout;
            var interval = 1000; // start from one second polling interval

            // Poll job until status is the expected
            while (DateTime.Now < limit)
            {
                await GetPhaseAsync(job, cancellationToken);

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
