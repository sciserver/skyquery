using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Threading.Tasks;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Activities;

namespace Jhu.SkyQuery.Jobs.Query
{
    public class ComputeSearchRadius : JobAsyncCodeActivity, IJobActivity
    {
        [RequiredArgument]
        public InArgument<XMatchQueryStep> XMatchStep { get; set; }

        protected override AsyncActivityWorker OnBeginExecute(AsyncCodeActivityContext activityContext)
        {
            XMatchQueryStep xmatchstep = XMatchStep.Get(activityContext);

            var xmqp = (XMatchQueryPartition)xmatchstep.QueryPartition;

            switch (xmqp.Query.ExecutionMode)
            {
                case Jhu.Graywulf.Jobs.Query.ExecutionMode.SingleServer:
                    xmqp.InitializeQueryObject(null);
                    break;
                case Jhu.Graywulf.Jobs.Query.ExecutionMode.Graywulf:
                    using (RegistryContext context = ContextManager.Instance.CreateContext(ConnectionMode.AutoOpen, TransactionMode.AutoCommit))
                    {
                        xmqp.InitializeQueryObject(context);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return delegate (JobContext asyncContext)
            {
                asyncContext.RegisterCancelable(xmqp);
                xmqp.ComputeSearchRadius(xmatchstep);
                asyncContext.UnregisterCancelable(xmqp);
            };
        }
    }
}
