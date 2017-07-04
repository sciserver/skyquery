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
    public class CreateZoneTable : GraywulfAsyncCodeActivity, IGraywulfActivity
    {
        [RequiredArgument]
        public InArgument<JobContext> JobContext { get; set; }

        [RequiredArgument]
        public InArgument<XMatchQueryStep> XMatchStep { get; set; }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext activityContext, AsyncCallback callback, object state)
        {
            XMatchQueryStep xmatchstep = XMatchStep.Get(activityContext);
            XMatchQueryPartition xmqp = (XMatchQueryPartition)xmatchstep.QueryPartition;

            switch (xmqp.Query.ExecutionMode)
            {
                case ExecutionMode.SingleServer:
                    xmqp.InitializeQueryObject(null);
                    break;
                case ExecutionMode.Graywulf:
                    using (Context context = ContextManager.Instance.CreateContext(this, activityContext, ConnectionMode.AutoOpen, TransactionMode.AutoCommit))
                    {
                        xmqp.InitializeQueryObject(context);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            Guid workflowInstanceGuid = activityContext.WorkflowInstanceId;
            string activityInstanceId = activityContext.ActivityInstanceId;
            return EnqueueAsync(_ => OnAsyncExecute(workflowInstanceGuid, activityInstanceId, xmatchstep), callback, state);
        }

        private void OnAsyncExecute(Guid workflowInstanceGuid, string activityInstanceId, XMatchQueryStep xmatchstep)
        {
            XMatchQueryPartition xmqp = (XMatchQueryPartition)xmatchstep.QueryPartition;
            RegisterCancelable(workflowInstanceGuid, activityInstanceId, xmqp);
            xmqp.CreateZoneTable(xmatchstep);
            UnregisterCancelable(workflowInstanceGuid, activityInstanceId, xmqp);
        }
    }
}
