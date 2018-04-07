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
        public static readonly StringComparer Comparer = StringComparer.InvariantCultureIgnoreCase;

        #region Private member variables

        // TAP service base URI
        private Uri baseAddress;

        // Timeout of individual HTTP requests
        private TimeSpan httpTimeout;

        // Timeout of polling process
        private TimeSpan pollTimeout;

        private HttpClient httpClient;

        private VO.Vosi.Availability.Common.IAvailability availability;
        private VO.Vosi.Capabilities.Common.ICapabilities capabilities;

        #endregion
        #region Properties

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

        #endregion
        #region Constructors and initializers

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

        #endregion
        #region Generic HTTP methods

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

        private async Task ValidateResponseCodeAsync(HttpResponseMessage response, IList<HttpStatusCode> expectedStatus)
        {
            if (500 <= (int)response.StatusCode && (int)response.StatusCode < 600)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw Error.ServiceError(response.StatusCode, response.ReasonPhrase, body);
            }
            else if (expectedStatus.IndexOf(response.StatusCode) < 0)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw Error.UnexpectedHttpResponse(response.StatusCode, response.ReasonPhrase, body);
            }
        }

        private async Task<HttpResponseMessage> HttpPostAsync(Uri address, HttpContent content, IList<HttpStatusCode> expectedStatus, CancellationToken cancellationToken)
        {
            EnsureHttpClientCreated();

            var response = await httpClient.PostAsync(address, content, cancellationToken);
            await ValidateResponseCodeAsync(response, expectedStatus);
            return response;
        }

        private async Task<HttpResponseMessage> HttpGetAsync(Uri address, IList<HttpStatusCode> expectedStatus, bool allowRedirect, CancellationToken cancellationToken)
        {
            EnsureHttpClientCreated();
            
            // Allow a few 303 redirects here but not too many
            HttpResponseMessage response = null;
            int q = 0;
            while (q < 5)
            {
                response = await httpClient.GetAsync(address, cancellationToken);

                if (allowRedirect && 
                    (response.StatusCode == HttpStatusCode.RedirectMethod ||
                     response.StatusCode == HttpStatusCode.Redirect))
                {
                    address = response.Headers.Location;
                    q++;
                }
                else
                {
                    break;
                }
            }

            if (response == null)
            {
                throw Error.TooManyRedirects();
            }

            await ValidateResponseCodeAsync(response, expectedStatus);
            return response;
        }

        private async Task<string> HttpGetStringAsync(Uri address, CancellationToken cancellationToken)
        {
            var response = await HttpGetAsync(address, new[] { HttpStatusCode.OK }, true, cancellationToken);
            return await response.Content.ReadAsStringAsync();
        }

        private async Task<System.IO.Stream> HttpGetStreamAsync(Uri address, IList<HttpStatusCode> expectedStatus, CancellationToken cancellationToken)
        {
            var response = await HttpGetAsync(address, expectedStatus, false, cancellationToken);
            return await response.Content.ReadAsStreamAsync();
        }

        private async Task<XmlReader> HttpGetXmlReaderAsync(Uri address, CancellationToken cancellationToken)
        {
            var stream = await HttpGetStreamAsync(address, new[] { HttpStatusCode.OK }, cancellationToken);
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
            using (var reader = await HttpGetXmlReaderAsync(address, cancellationToken))
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

#endregion

        public async Task<VO.Vosi.Availability.Common.IAvailability> GetAvailabilityAsync(CancellationToken cancellationToken)
        {
            if (availability == null)
            {
                var address = UriConverter.Combine(baseAddress, Constants.TapCommandAvailability);
                availability = await HttpGetObject<VO.Vosi.Availability.V1_0.Availability>(address, cancellationToken);
            }

            return availability;
        }

        public async Task<VO.Vosi.Capabilities.Common.ICapabilities> GetCapabilitiesAsync(CancellationToken cancellationToken)
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
            return await HttpGetStringAsync(address, cancellationToken);
        }

        private async Task<HttpResponseMessage> SetParameterAsync(TapJob job, string action, string parameter, string value, CancellationToken cancellationToken)
        {
            var address = UriConverter.Combine(job.Uri, action);
            var parameters = new Dictionary<string, string>()
            {
                {parameter, value }
            };
            var content = new FormUrlEncodedContent(parameters);
            return await HttpPostAsync(address, content, new[] { HttpStatusCode.RedirectMethod, HttpStatusCode.Redirect, HttpStatusCode.OK }, cancellationToken);
        }

        private async Task<HttpResponseMessage> SubmitAsync(TapJob job, CancellationToken cancellationToken)
        {
            EnsureHttpClientCreated();

            string action;
            HttpStatusCode[] expectedStatus;

            if (job.IsAsync)
            {
                action = Constants.TapActionAsync;
                expectedStatus = new[] { HttpStatusCode.RedirectMethod };
            }
            else
            {
                action = Constants.TapActionSync;
                expectedStatus = new[] { HttpStatusCode.RedirectMethod, HttpStatusCode.OK };
            }

            var parameters = job.GetSubmitRequestParameters();
            var content = new FormUrlEncodedContent(parameters);
            var address = UriConverter.Combine(baseAddress, action);
            var result = await HttpPostAsync(address, content, expectedStatus, cancellationToken);

            if (result.StatusCode == HttpStatusCode.RedirectMethod)
            {
                job.Uri = result.Headers.Location;
            }

            return result;
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


        private async Task SetPhaseAsync(TapJob job, CancellationToken cancellationToken)
        {
            await SetParameterAsync(job, Constants.TapActionAsyncPhase, Constants.TapParamPhase, job.Phase.ToString().ToUpperInvariant(), cancellationToken);
        }

        private async Task GetQuoteAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        private async Task GetExecutionDurationAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task SetExecutionDurationAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task GetDestructionAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task SetDestructionAsync(TapJob job, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<System.IO.Stream> GetResultsAsync(TapJob job, CancellationToken cancellationToken)
        {
            Uri address;

            if (!job.IsAsync)
            {
                address = job.Uri;
            }
            else
            {
                address = UriConverter.Combine(job.Uri, Constants.TapActionAsyncResults);
            }

            var response = await HttpGetAsync(address, new[] { HttpStatusCode.RedirectMethod, HttpStatusCode.OK }, true, cancellationToken);
            
            return await response.Content.ReadAsStreamAsync();
        }

        private async Task<System.IO.Stream> GetErrorAsync(TapJob job, CancellationToken cancellationToken)
        {
            var address = UriConverter.Combine(job.Uri, Constants.TapActionAsyncError);
            return await HttpGetStreamAsync(address, new[] { HttpStatusCode.OK }, cancellationToken);
        }

        private async Task PollAsync(TapJob job, IList<TapJobPhase> expectedPhase, IList<TapJobPhase> errorPhase, CancellationToken cancellationToken)
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

            throw Error.CommandTimeout();
        }

        public async Task<System.IO.Stream> ExecuteJobAsync(TapJob job, CancellationToken cancellationToken)
        {
            System.IO.Stream stream;

            // 1. Submit job
            var result = await SubmitAsync(job, cancellationToken);

            if (!job.IsAsync)
            {
                // Allow a single redirect
                if (result.StatusCode == System.Net.HttpStatusCode.RedirectMethod)
                {
                    stream = await GetResultsAsync(job, cancellationToken);
                }
                else
                {
                    stream = await result.Content.ReadAsStreamAsync();
                }
            }
            else
            {
                // TODO: this gets into an endless loop here for AADC

                while (true)
                {
                    // 2. Poll status and commit if pending
                    await PollAsync(job, new[] { TapJobPhase.Pending, TapJobPhase.Completed, TapJobPhase.Aborted, TapJobPhase.Error }, null, cancellationToken);

                    // 3. Process output
                    if (job.Phase == TapJobPhase.Pending)
                    {
                        job.Phase = TapJobPhase.Run;
                        await SetPhaseAsync(job, cancellationToken);
                        continue;
                    }
                    else if (job.Phase == TapJobPhase.Completed)
                    {
                        stream = await GetResultsAsync(job, cancellationToken);
                        break;
                    }
                    else if (job.Phase == TapJobPhase.Aborted)
                    {
                        throw Error.CommandCancelled();
                    }
                    else if (job.Phase == TapJobPhase.Error)
                    {
                        stream = await GetErrorAsync(job, cancellationToken);
                        var votable = new Jhu.SkyQuery.Format.VoTable.VoTableWrapper(stream, Graywulf.IO.DataFileMode.Read);

                        // TODO: read error document, parse VOTable and report error
                        // TODO: throw TAP exception with message from server

                        throw new TapException();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }

            return stream;
        }

        #region Feature support logic

        private VO.VoResource.Common.ICapability GetCapability(string standardID)
        {
            var cap = capabilities.CapabilityList.FirstOrDefault(i => Comparer.Compare(i.StandardID, standardID) == 0);
            return cap;
        }

        private VO.VoResource.Common.ICapability GetTableAccessCapability()
        {
            // TODO: we always pick the first matching capability though there could be more entry points
            var cap = GetCapability(VO.Constants.StandardIDTap);
            return cap;
        }

        private string GetTapUri()
        {
            // TODO: how to select the right access URL
            //       - prefer one matching connection string
            //       - prefer https
            var cap = GetTableAccessCapability();
            var iface = cap.InterfaceList.FirstOrDefault(i => i is VO.VoDataService.Common.IParamHttp);
            return iface.AccessUrlList.FirstOrDefault().Value;
        }

        public bool IsLanguageSupported(string name)
        {
            var cap = GetTableAccessCapability() as VO.TapRegExt.Common.ITableAccess;
            if (cap != null)
            {
                var lang = cap.LanguageList.FirstOrDefault(i => Comparer.Compare(name, i) == 0);
                if (lang != null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsSchemaInfoSupported()
        {
            var cap = GetCapability(VO.Constants.StandardIDVosiTables);
            return cap != null;
        }

        #endregion
        #region Output format logic

        private string GetFormatMimeType(string mime, string alias, string serialization)
        {
            var tableAccess = GetTableAccessCapability() as VO.TapRegExt.Common.ITableAccess;
            VO.TapRegExt.Common.IOutputFormat outputFormat = null;

            if (tableAccess != null)
            {
                var fq = from f in tableAccess.OutputFormatList
                         where
                            (f.Mime.IndexOf(mime, StringComparison.InvariantCultureIgnoreCase) >= 0 ||
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

            // Fall-back to default if format list is not available
            format = TapOutputFormat.VOTable;
            mime = Constants.TapMimeVoTable;
            return false;
        }

        #endregion
    }
}
