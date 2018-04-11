using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Activities;
using System.Threading.Tasks;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Tasks;

namespace Jhu.SkyQuery.Jobs.Query
{
    public class CreatePairTable : JobAsyncCodeActivity, IJobActivity
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
            Table linkTable = null;
            SqlCommand createPairTableCommand = null;

            switch (xmqp.Parameters.ExecutionMode)
            {
                case ExecutionMode.SingleServer:
                    xmqp.InitializeQueryObject(cancellationContext, null);
                    break;
                case ExecutionMode.Graywulf:
                    using (RegistryContext registryContext = ContextManager.Instance.CreateReadOnlyContext())
                    {
                        xmqp.InitializeQueryObject(cancellationContext, registryContext);
                        xmqp.PrepareCreatePairTable(xmatchstep, out linkTable, out pairTable, out createPairTableCommand);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            await xmqp.CreatePairTableAsync(xmatchstep, linkTable, pairTable, createPairTableCommand);
        }
    }
}
