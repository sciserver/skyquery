using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Tasks;

namespace Jhu.SkyQuery.Sql.Jobs.Query
{
    public class ComputeSearchRadius : JobAsyncCodeActivity, IJobActivity
    {
        [RequiredArgument]
        public InArgument<XMatchQueryStep> XMatchStep { get; set; }

        protected override async Task OnExecuteAsync(AsyncCodeActivityContext activityContext, CancellationContext cancellationContext)
        {
            var workflowInstanceId = activityContext.WorkflowInstanceId;
            var activityInstanceId = activityContext.ActivityInstanceId;
            XMatchQueryStep xmatchstep = XMatchStep.Get(activityContext);
            SqlCommand computeSearchRadiusCommand = null;

            var xmqp = (XMatchQueryPartition)xmatchstep.QueryPartition;

            switch (xmqp.Parameters.ExecutionMode)
            {
                case Jhu.Graywulf.Sql.Jobs.Query.ExecutionMode.SingleServer:
                    xmqp.InitializeQueryObject(cancellationContext, null);
                    break;
                case Jhu.Graywulf.Sql.Jobs.Query.ExecutionMode.Graywulf:
                    using (RegistryContext registryContext = ContextManager.Instance.CreateReadOnlyContext())
                    {
                        xmqp.InitializeQueryObject(cancellationContext, registryContext);
                        xmqp.PrepareComputeSearchRadius(xmatchstep, out computeSearchRadiusCommand);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            await xmqp.ComputeSearchRadiusAsync(xmatchstep, computeSearchRadiusCommand);
        }
    }
}
