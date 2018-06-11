using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Registry;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    public class XMatchQueryJobInstaller : Jhu.Graywulf.Sql.Jobs.Query.SqlQueryJobInstaller
    {
        protected override Type JobType
        {
            get { return typeof(Jhu.SkyQuery.Sql.Jobs.Query.XMatchQueryJob); }
        }

        protected override string DisplayName
        {
            get { return "XMatch"; }
        }

        protected override bool IsSystem
        {
            get { return false; }
        }

        public XMatchQueryJobInstaller(Federation federation)
            : base(federation)
        {
        }
    }
}
