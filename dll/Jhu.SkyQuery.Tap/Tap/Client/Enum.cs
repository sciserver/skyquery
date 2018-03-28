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

    public enum TapOutputFormat
    {
        None,
        VOTable,
        VOTableBinary,
        VOTableBinary2,
        VOTableFits,
        Json,
        Csv,
        Text,
        Fits,
        Html,
    }
    // application/x-votable+xml;serialization=TABLEDATA

    public enum TapJobAction
    {
        Run,
        Abort
    }

    public enum TapJobPhase
    {
        Unknown,
        Run,
        Pending,
        Queued,
        Executing,
        Completed,
        Aborted,
        Error,
    }
}
