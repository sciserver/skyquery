using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Jobs.Query;
using Jhu.Graywulf.Schema;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    public class XMatchQueryTestBase : SqlQueryTestBase
    {
        protected override UserDatabaseFactory CreateUserDatabaseFactory(Context context)
        {
            return UserDatabaseFactory.Create(
                typeof(Jhu.Graywulf.CasJobs.CasJobsUserDatabaseFactory).AssemblyQualifiedName,
                context);
        }

        protected override QueryFactory CreateQueryFactory(Context context)
        {
            return QueryFactory.Create(
                typeof(XMatchQueryFactory).AssemblyQualifiedName,
                context);
        }
    }
}
