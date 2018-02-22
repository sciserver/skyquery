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
using System.Xml.Schema;
using Jhu.Graywulf.Util;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapClient : IDisposable
    {
        private Uri baseAddress;
        private TimeSpan httpTimeout;
        private TimeSpan pollTimeout;

        private HttpClient httpClient;

        private VO.Vosi.Availability.V1_0.Availability availability;
        private VO.Vosi.Capabilities.V1_0.Capabilities capabilities;

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

            if (500 <= (int)result.StatusCode && (int)result.StatusCode < 600)
            {
                var body = await result.Content.ReadAsStringAsync();
                throw Error.ServiceError(result.StatusCode, result.ReasonPhrase, body);
            }
            if (result.StatusCode != expectedStatus)
            {
                var body = await result.Content.ReadAsStringAsync();
                throw Error.UnexpectedHttpResponse(result.StatusCode, result.ReasonPhrase, body);
            }

            return result;
        }

        private async Task<string> HttpGetAsync(Uri address, HttpStatusCode expectedStatus, CancellationToken cancellationToken)
        {
            EnsureHttpClientCreated();

            var result = await httpClient.GetAsync(address, cancellationToken);

            if (result.StatusCode != expectedStatus)
            {
                var body = await result.Content.ReadAsStringAsync();
                throw Error.UnexpectedHttpResponse(result.StatusCode, result.ReasonPhrase, body);
            }

            return await result.Content.ReadAsStringAsync();
        }

        private async Task<System.IO.Stream> HttpGetStreamAsync(Uri address, HttpStatusCode expectedStatus, CancellationToken cancellationToken)
        {
            EnsureHttpClientCreated();

            var result = await httpClient.GetAsync(address, cancellationToken);

            if (result.StatusCode != expectedStatus)
            {
                var body = await result.Content.ReadAsStringAsync();
                throw Error.UnexpectedHttpResponse(result.StatusCode, result.ReasonPhrase, body);
            }

            return await result.Content.ReadAsStreamAsync();
        }

        private async Task<XmlReader> HttpGetXmlReaderAsync(Uri address, HttpStatusCode expectedStatus, CancellationToken cancellationToken)
        {
            var stream = await HttpGetStreamAsync(address, HttpStatusCode.OK, cancellationToken);
            var settings = new XmlReaderSettings()
            {
                Async = true,
                // Schemas  ... TODO: add schema cache?
                // XmlResolver 
            };

            var xml = XmlReader.Create(stream, settings);
            return new VO.VoXmlReader(xml);
        }

        private async Task<T> HttpGetObject<T>(Uri address, CancellationToken cancellationToken)
        {
            using (var reader = await HttpGetXmlReaderAsync(address, HttpStatusCode.OK, cancellationToken))
            {
                try
                {
                    var s = new XmlSerializer(typeof(T));
                    var obj = (T)s.Deserialize(reader);
                    return obj;
                }
                catch (XmlException ex)
                {
                    throw Error.DeserializationException(ex);
                }
            }
        }
        
        public async Task<VO.Vosi.Availability.V1_0.Availability> GetAvailabilityAsync(CancellationToken cancellationToken)
        {
            if (availability == null)
            {
                var address = UriConverter.Combine(baseAddress, Constants.TapCommandAvailability);
                availability = await HttpGetObject<VO.Vosi.Availability.V1_0.Availability>(address, cancellationToken);
            }

            return availability;
        }

        public async Task<VO.Vosi.Capabilities.V1_0.Capabilities> GetCapabilitiesAsync(CancellationToken cancellationToken)
        {
            if (capabilities == null)
            {
                var address = UriConverter.Combine(baseAddress, Constants.TapCommandCapabilities);
                capabilities = await HttpGetObject<VO.Vosi.Capabilities.V1_0.Capabilities>(address, cancellationToken);
            }

            return capabilities;
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

        #region Output format logic

        private string GetFormatMimeType(string mime, string alias, string serialization)
        {
            VO.TapRegExt.V1_0.TableAccess tableAccess = null;
            VO.TapRegExt.V1_0.OutputFormat outputFormat = null;

            var cq = from c in capabilities.CapabilityList
                     where c is VO.TapRegExt.V1_0.TableAccess
                     select (VO.TapRegExt.V1_0.TableAccess)c;
            tableAccess = cq.FirstOrDefault();

            if (tableAccess != null)
            {
                var fq = from f in tableAccess.OutputFormatList
                         where
                            (f.Mime.IndexOf(mime, StringComparison.InvariantCultureIgnoreCase) >= 0  ||
                             f.Alias.FirstOrDefault(a => StringComparer.InvariantCultureIgnoreCase.Compare(alias, a) == 0) != null) &&
                            (serialization == null || f.Mime.IndexOf(serialization, StringComparison.InvariantCultureIgnoreCase) >= 0)
                         select f;
                outputFormat = fq.FirstOrDefault();
            }

            if (outputFormat != null)
            {
                return outputFormat.Mime;
            }
            else
            {
                return null;
            }
        }

        public string GetFormatMimeType(TapOutputFormat format)
        {
            string mime = null;

            switch (format)
            {
                case TapOutputFormat.VOTable:
                    mime = GetFormatMimeType(Constants.TapMimeVoTable, TapOutputFormat.VOTable.ToString(), null);
                    break;
                case TapOutputFormat.VOTableBinary:
                    mime = GetFormatMimeType(Constants.TapMimeVoTable, TapOutputFormat.VOTable.ToString(), Constants.TapSerializationVoTableBinary);
                    break;
                case TapOutputFormat.VOTableBinary2:
                    mime = GetFormatMimeType(Constants.TapMimeVoTable, TapOutputFormat.VOTable.ToString(), Constants.TapSerializationVoTableBinary2);
                    break;
                case TapOutputFormat.VOTableFits:
                    mime = GetFormatMimeType(Constants.TapMimeVoTable, TapOutputFormat.VOTable.ToString(), Constants.TapSerializationVoTableFits);
                    break;
                case TapOutputFormat.Json:
                    mime = GetFormatMimeType(Constants.TapMimeJson, TapOutputFormat.Json.ToString(), null);
                    break;
                case TapOutputFormat.Csv:
                    mime = GetFormatMimeType(Constants.TapMimeCsv, TapOutputFormat.Csv.ToString(), null);
                    break;
                case TapOutputFormat.Text:
                    mime = GetFormatMimeType(Constants.TapMimeText, TapOutputFormat.Text.ToString(), null);
                    break;
                case TapOutputFormat.Fits:
                    mime = GetFormatMimeType(Constants.TapMimeFits, TapOutputFormat.Fits.ToString(), null);
                    break;
                case TapOutputFormat.Html:
                    mime = GetFormatMimeType(Constants.TapMimeHtml, TapOutputFormat.Html.ToString(), null);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return mime;
        }

        public bool GetBestFormat(out TapOutputFormat format, out string mime)
        {
            var formats = new[] 
            {
                TapOutputFormat.VOTableBinary2,
                TapOutputFormat.VOTableBinary,
                TapOutputFormat.VOTable,
                TapOutputFormat.Fits,
                TapOutputFormat.Csv
            };

            for (int i = 0; i < formats.Length; i++)
            {
                mime = GetFormatMimeType(formats[i]);

                if (mime != null)
                {
                    format = formats[i];
                    return true;
                }
            }

            format = TapOutputFormat.None;
            mime = null;
            return false;
        }

        #endregion
    }
}
