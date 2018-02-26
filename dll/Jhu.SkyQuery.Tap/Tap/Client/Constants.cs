using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Tap.Client
{
    class Constants
    {
        public const int DefaultConnectionTimeout = 30;
        public const int DefaultCommandTimeout = 1200;      // Pumped up to support slow TAP services
        public const int DefaultHttpTimeout = 30;
        public const int DefaultPollTimeout = 120;

        public const string KeyDataSource = "Data Source";
        public const string KeyConnectionTimeout = "Connect Timeout";

        public const string TapCommandAvailability = "/availability";
        public const string TapCommandCapabilities = "/capabilities";

        public const string TapParamRequest = "REQUEST";
        public const string TapParamLang = "LANG";
        public const string TapParamFormat = "FORMAT";
        public const string TapParamSerialization = "SERIALIZATION";
        public const string TapParamQuery = "QUERY";
        public const string TapParamPhase = "PHASE";
        public const string TapParamTermination = "TERMINATION";
        public const string TapParamDestruction = "DESTRUCTION";

        public const string TapActionSync = "sync";
        public const string TapActionAsync = "async";
        public const string TapActionAsyncPhase = "phase";
        public const string TapActionAsyncQuote = "quote";
        public const string TapActionAsyncExecutionDuration = "executionduration";
        public const string TapActionAsyncDestruction = "destruction";
        public const string TapActionAsyncResults = "results/result";
        public const string TapActionAsyncError = "error";

        public const string TapRequestDoQuery = "doQuery";

        public const string TapMimeVoTable = "application/x-votable+xml";
        public const string TapMimeFits = "application/fits";
        public const string TapMimeJson = "application/json";
        public const string TapMimeCsv = "text/csv";
        public const string TapMimeText = "text/plain";
        public const string TapMimeHtml = "text/html";

        public const string TapSerializationVoTableTableData = "TABLEDATA";
        public const string TapSerializationVoTableBinary = "BINARY";
        public const string TapSerializationVoTableBinary2 = "BINARY2";
        public const string TapSerializationVoTableFits = "FITS";

        public const string TapDateFormat = "yyyy-MM-ddTHH:mm:ssZ";

        public const string TapProviderName = "System.Data.SqlClient";
    }
}
