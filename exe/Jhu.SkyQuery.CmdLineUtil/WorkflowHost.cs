using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Activities;
using Jhu.Graywulf.Registry;

namespace Jhu.SkyQuery.CmdLineUtil
{
    class WorkflowHost
    {
        public static void ExecuteWorkflow(Activity activity, Dictionary<string, object> parameters)
        {
            // Set up logging to write to the console
            Jhu.Graywulf.Logging.Logger.Instance.Writers.Clear();

            Jhu.Graywulf.Logging.Logger.Instance.Writers.Add(
                new Jhu.Graywulf.Logging.StreamLogWriter(Console.Out));

            // Set up workflow host
            WorkflowInvoker wi = new WorkflowInvoker(activity);
            wi.Extensions.Add(new Jhu.Graywulf.Activities.Tracking.GraywulfTrackingParticipant());

            // Run workflow
            try
            {
                wi.Invoke(parameters);

                Console.WriteLine();
                Console.WriteLine("Job completed successfully.");
            }
            catch (Exception)
            {
                Console.WriteLine();
                Console.WriteLine("Job completed with errors, see log for details.");
            }
        }

        public static void ExecuteJob(JobInstance job)
        {

            // Load assembly and create workflow instance
            Type wftype = Type.GetType(job.WorkflowTypeName);

            // Deserialize parameters
            Dictionary<string, object> par = new Dictionary<string, object>();
            foreach (string key in job.Parameters.Keys)
            {
                XmlSerializer ser = new XmlSerializer(wftype.GetProperty(key).PropertyType.GetGenericArguments()[0]);
                par.Add(key, ser.Deserialize(new StringReader(job.Parameters[key])));
            }

            par.Add("UserGuid", job.UserGuidOwner);
            par.Add("JobGuid", job.Guid);

            ExecuteWorkflow((Activity)Activator.CreateInstance(wftype), par);
        }
    }
}
