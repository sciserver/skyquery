using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Threading.Tasks;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Tasks;

namespace Jhu.SkyQuery.Jobs.Query
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

            var xmqp = (XMatchQueryPartition)xmatchstep.QueryPartition;

            switch (xmqp.Query.ExecutionMode)
            {
                case Jhu.Graywulf.Jobs.Query.ExecutionMode.SingleServer:
                    xmqp.InitializeQueryObject(cancellationContext, null);
                    break;
                case Jhu.Graywulf.Jobs.Query.ExecutionMode.Graywulf:
                    using (RegistryContext registryContext = ContextManager.Instance.CreateReadOnlyContext())
                    {
                        xmqp.InitializeQueryObject(cancellationContext, registryContext);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            await xmqp.ComputeSearchRadiusAsync(xmatchstep);
        }
    }
}
