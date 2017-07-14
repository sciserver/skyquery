using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Threading.Tasks;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query
{
    public class CreatePairTable : JobAsyncCodeActivity, IJobActivity
    {
        [RequiredArgument]
        public InArgument<XMatchQueryStep> XMatchStep { get; set; }

        protected override AsyncActivityWorker OnBeginExecute(AsyncCodeActivityContext activityContext)
        {
            var workflowInstanceId = activityContext.WorkflowInstanceId;
            var activityInstanceId = activityContext.ActivityInstanceId;
            XMatchQueryStep xmatchstep = XMatchStep.Get(activityContext);
            XMatchQueryPartition xmqp = (XMatchQueryPartition)xmatchstep.QueryPartition;

            switch (xmqp.Query.ExecutionMode)
            {
                case ExecutionMode.SingleServer:
                    xmqp.InitializeQueryObject(null);
                    break;
                case ExecutionMode.Graywulf:
                    using (RegistryContext context = ContextManager.Instance.CreateContext(ConnectionMode.AutoOpen, TransactionMode.AutoCommit))
                    {
                        xmqp.InitializeQueryObject(context);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return delegate ()
            {
                RegisterCancelable(workflowInstanceId, activityInstanceId, xmqp);
                xmqp.CreatePairTable(xmatchstep);
                UnregisterCancelable(workflowInstanceId, activityInstanceId, xmqp);
            };
        }
    }
}
