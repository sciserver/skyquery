using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Tap.Client
{
    public class TapJob
    {
        private string query;
        private TapQueryLanguage language;
        private TapResultsFormat format;
        private int maxRec;
        private string runId;
        private TapJobPhase phase;
        private TimeSpan quote;
        private TimeSpan duration;
        private DateTime destruction;
        private string error;
        private Uri uri;

        public string Query
        {
            get { return query; }
            set { query = value; }
        }

        public TapQueryLanguage Language
        {
            get { return language; }
            set { language = value; }
        }

        public TapResultsFormat Format
        {
            get { return format; }
            set { format = value; }
        }

        public int MaxRec
        {
            get { return maxRec; }
            set { maxRec = value; }
        }

        private string RunId
        {
            get { return RunId; }
            set { runId = value; }
        }

        public TapJobPhase Phase
        {
            get { return phase; }
            set { phase = value; }
        }

        public TimeSpan Quote
        {
            get { return quote; }
            set { quote = value; }
        }

        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public DateTime Destruction
        {
            get { return destruction; }
            set { destruction = value; }
        }

        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        public Uri Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        public TapJob()
        {
            InitializeMembers();
        }
        
        public TapJob(TapJob old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.query = null;
            this.language = TapQueryLanguage.Adql;
            this.format = TapResultsFormat.VOTable;
            this.maxRec = -1;
            this.runId = null;
            this.phase = TapJobPhase.Unknown;
            this.quote = TimeSpan.Zero;
            this.duration = TimeSpan.Zero;
            this.destruction = DateTime.MinValue;
            this.error = null;
            this.uri = null;
            
        }

        private void CopyMembers(TapJob old)
        {
            this.query = old.query;
            this.language = old.language;
            this.format = old.format;
            this.maxRec = old.maxRec;
            this.runId = old.runId;
            this.phase = old.phase;
            this.quote = old.quote;
            this.duration = old.duration;
            this.destruction = old.destruction;
            this.error = old.error;
            this.uri = old.uri;
        }

        public Dictionary<string, string> GetSubmitRequestParameters()
        {
            TapResultsFormat fmt;
            string ser = null;

            switch (format)
            {
                case TapResultsFormat.VOTable:
                    fmt = TapResultsFormat.VOTable;
                    ser = Constants.VoTableTableData;
                    break;
                case TapResultsFormat.VOTableBinary:
                    fmt = TapResultsFormat.VOTable;
                    ser = Constants.VoTableBinary;
                    break;
                case TapResultsFormat.VOTableBinary2:
                    fmt = TapResultsFormat.VOTable;
                    ser = Constants.VoTableBinary2;
                    break;
                case TapResultsFormat.VOTableFits:
                    fmt = TapResultsFormat.VOTable;
                    ser = Constants.VoTableFits;
                    break;
                default:
                    fmt = format;
                    break;
            }

            var parameters = new Dictionary<string, string>()
            {
                { Constants.TapParamRequest, Constants.TapRequestDoQuery },
                { Constants.TapParamLang, language.ToString().ToUpperInvariant() },
                { Constants.TapParamFormat, fmt.ToString().ToUpperInvariant() },
                { Constants.TapParamQuery, query },
                { Constants.TapParamPhase, TapJobAction.Run.ToString().ToUpperInvariant() }
            };

            if (ser != null)
            {
                parameters.Add(Constants.TapParamSerialization, ser);
            }

            if (duration != TimeSpan.Zero)
            {
                parameters.Add(Constants.TapParamTermination, ((int)duration.TotalSeconds).ToString());
            }

            if (destruction != DateTime.MinValue)
            {
                parameters.Add(Constants.TapParamDestruction, destruction.ToString(Constants.DateFormat));
            }

            return parameters;
        }
    }
}
