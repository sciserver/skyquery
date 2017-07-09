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
    public class ComputeSearchRadius : GraywulfAsyncCodeActivity, IGraywulfActivity
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
                    using (Context context = ContextManager.Instance.CreateContext(this, activityContext, ConnectionMode.AutoOpen, TransactionMode.AutoCommit))
                    {
                        xmqp.InitializeQueryObject(context);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return delegate (AsyncJobContext asyncContext)
            {
                asyncContext.RegisterCancelable(xmqp);
                xmqp.ComputeSearchRadius(xmatchstep);
                asyncContext.UnregisterCancelable(xmqp);
            };
        }
    }
}
