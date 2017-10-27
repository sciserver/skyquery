using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Tap.Client
{
    public enum TapQueryLanguage
    {
        Adql,
        Pql,
        Sql,
        SkyQuery
    }

    public enum TapResultsFormat
    {
        VOTable,
        Json,
        Csv,
        Text
    }

    public enum TapJobPhase
    {
        Run,
        Abort,
        Queued,
        Executing,
        Completed,
        Aborted,
        Error,
    }
}
