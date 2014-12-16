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
using Jhu.Graywulf.Web.Api.V1;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query.Test
{
    public class XMatchQueryTestBase : TestClassBase
    {
        protected Guid ScheduleQueryJob(string query, QueueType queueType)
        {
            var queue = String.Format("QueueInstance:Graywulf.Controller.Controller.{0}", queueType.ToString());  // *** TODO

            using (var context = ContextManager.Instance.CreateContext(ConnectionMode.AutoOpen, TransactionMode.AutoCommit))
            {
                var user = SignInTestUser(context);

                var udf = UserDatabaseFactory.Create(context.Federation);
                var mydb = udf.GetUserDatabase(user);

                var qf = QueryFactory.Create(context.Federation);

                var q = qf.CreateQuery(query);

                q.Destination = new Jhu.Graywulf.IO.Tasks.DestinationTable()
                {
                    Dataset = mydb,
                    DatabaseName = mydb.DatabaseName,
                    SchemaName = mydb.DefaultSchemaName,
                    TableNamePattern = "testtable",     // will be overwritten by INTO queries
                    Options = TableInitializationOptions.Create | TableInitializationOptions.Drop
                };

                var ji = qf.ScheduleAsJob(null, q, queue, "testjob");

                ji.Save();

                return ji.Guid;
            }
        }
    }
}
