using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Registry;

namespace Jhu.Graywulf.Install
{
    public class XMatchQueryJobInstaller : Jhu.Graywulf.Jobs.Query.SqlQueryJobInstaller
    {
        public XMatchQueryJobInstaller(Federation federation)
            : base(federation)
        {
        }

        public override JobDefinition Install()
        {
            return GenerateJobDefinition(typeof(Jhu.SkyQuery.Jobs.Query.XMatchQueryJob));
        }

    }
}
