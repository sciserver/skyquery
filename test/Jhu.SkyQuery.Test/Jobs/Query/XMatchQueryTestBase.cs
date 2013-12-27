using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Test;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    public class XMatchQueryTestBase : TestClassBase
    {
        protected Guid ScheduleQueryJob(string query, QueueType queueType)
        {
            var queue = String.Format("Graywulf.Controller.Controller.{0}", queueType.ToString());  // *** TODO

            using (var context = ContextManager.Instance.CreateContext(ConnectionMode.AutoOpen, TransactionMode.AutoCommit))
            {
                SignInTestUser(context);

                var f = new XMatchQueryFactory(context);

                var q = f.CreateQuery(query);
                var ji = f.ScheduleAsJob(q, queue, "testjob");

                ji.Save();

                return ji.Guid;
            }
        }
    }
}
