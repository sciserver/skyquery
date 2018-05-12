using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.Tasks;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    public class CreateMatchTable : JobAsyncCodeActivity, IJobActivity
    {
        [RequiredArgument]
        public InArgument<XMatchQueryStep> XMatchStep { get; set; }

        protected override async Task OnExecuteAsync(AsyncCodeActivityContext activityContext, CancellationContext cancellationContext)
        {
            var workflowInstanceId = activityContext.WorkflowInstanceId;
            var activityInstanceId = activityContext.ActivityInstanceId;
            XMatchQueryStep xmatchstep = XMatchStep.Get(activityContext);
            XMatchQueryPartition xmqp = (XMatchQueryPartition)xmatchstep.QueryPartition;
            Table pairTable = null;
            Table matchTable = null;
            SqlCommand createMatchTableCommand = null;
            SqlCommand populateMatchTableCommand = null;
            SqlCommand buildMatchTableIndexCommand = null;

            switch (xmqp.Parameters.ExecutionMode)
            {
                case ExecutionMode.SingleServer:
                    xmqp.InitializeQueryObject(cancellationContext, null);
                    break;
                case ExecutionMode.Graywulf:
                    using (RegistryContext registryContext = ContextManager.Instance.CreateReadOnlyContext())
                    {
                        xmqp.InitializeQueryObject(cancellationContext, registryContext);
                        xmqp.PrepareCreateMatchTable(xmatchstep, out pairTable, out matchTable, out createMatchTableCommand, out populateMatchTableCommand, out buildMatchTableIndexCommand);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            await xmqp.CreateMatchTableAsync(xmatchstep, pairTable, matchTable, createMatchTableCommand, populateMatchTableCommand, buildMatchTableIndexCommand);
        }
    }
}
