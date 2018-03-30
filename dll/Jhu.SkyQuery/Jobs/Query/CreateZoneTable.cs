using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Sql.Jobs.Query;
using Jhu.Graywulf.Tasks;

namespace Jhu.SkyQuery.Jobs.Query
{
    public class CreateZoneTable : JobAsyncCodeActivity, IJobActivity
    {
        [RequiredArgument]
        public InArgument<XMatchQueryStep> XMatchStep { get; set; }

        protected override async Task OnExecuteAsync(AsyncCodeActivityContext activityContext, CancellationContext cancellationContext)
        {
            var workflowInstanceId = activityContext.WorkflowInstanceId;
            var activityInstanceId = activityContext.ActivityInstanceId;
            XMatchQueryStep xmatchstep = XMatchStep.Get(activityContext);
            XMatchQueryPartition xmqp = (XMatchQueryPartition)xmatchstep.QueryPartition;
            Jhu.Graywulf.Sql.Schema.Table zoneTable = null;
            SqlCommand createZoneTableCommand = null;
            SqlCommand populateZoneTableCommand = null;
            
            switch (xmqp.Parameters.ExecutionMode)
            {
                case ExecutionMode.SingleServer:
                    xmqp.InitializeQueryObject(cancellationContext, null);
                    break;
                case ExecutionMode.Graywulf:
                    using (RegistryContext registryContext = ContextManager.Instance.CreateReadOnlyContext())
                    {
                        xmqp.InitializeQueryObject(cancellationContext, registryContext);
                        xmqp.PrepareCreateZoneTable(xmatchstep, out zoneTable, out createZoneTableCommand, out populateZoneTableCommand);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            await xmqp.CreateZoneTableAsync(xmatchstep, zoneTable, createZoneTableCommand, populateZoneTableCommand);
        }
    }
}
