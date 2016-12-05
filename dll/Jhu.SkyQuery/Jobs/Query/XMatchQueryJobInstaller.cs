using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Registry;

namespace Jhu.Graywulf.Install
{
    public class XMatchQueryJobInstaller : Jhu.Graywulf.Jobs.Query.SqlQueryJobInstaller
    {
        protected override Type JobType
        {
            get { return typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryJob); }
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
