using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;
using Jhu.Graywulf.CommandLineParser;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Scheduler;
using Jhu.SkyQuery.Sql.Jobs.Query;

namespace Jhu.SkyQuery.CmdLineUtil
{
    [Verb(Name = "Query", Description = "Executes a query in single-server mode")]
    class Query : Verb
    {
        private string server;
        private bool integratedSecurity;
        private string userId;
        private string password;
        private string input;
        private bool dumpSql;

        AutoResetEvent workflowCompleted;

        [Parameter(Name = "Server", Description = "Database server.")]
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        [Option(Name = "EnableIntegratedSecurity", Description = "Use Windows login.")]
        public bool IntegratedSecurity
        {
            get { return integratedSecurity; }
            set { integratedSecurity = value; }
        }

        [Parameter(Name = "UserId", Description = "User ID.")]
        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        [Parameter(Name = "Password", Description = "Password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [Parameter(Name = "Input", Description = "Input file containing the query", Required = true)]
        public string Input
        {
            get { return input; }
            set { input = value; }
        }

        [Option(Name = "DumpSQL", Description = "Dump SQL queries to a file")]
        public bool DumpSql
        {
            get { return dumpSql; }
            set { dumpSql = value; }
        }

        public Query()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.server = "localhost";
            this.integratedSecurity = true;
            this.userId = null;
            this.password = null;
            this.input = null;

            this.workflowCompleted = new AutoResetEvent(false);
        }

        public override void Run()
        {
            // Load query string from file
            var query = System.IO.File.ReadAllText(input);

            // Read connection strings from config
            var f = new SingleServerQueryFactory();
            f.LoadCustomDatasets(this.server, this.userId, this.password, this.integratedSecurity);

            // Create query and verify
            var q = f.CreateQuery(query);
            q.Verify();

            q.Parameters.DumpSql = dumpSql;

            // Create a workflow
            var wf = f.GetAsWorkflow(q);
            var par = f.GetWorkflowParameters(q);

            // Submit for execution
            var wfhost = new StandaloneWorkflowApplicationHost();
            wfhost.WorkflowEvent += new EventHandler<WorkflowApplicationHostEventArgs>(wfhost_WorkflowEvent);

            wfhost.Start(Jhu.Graywulf.Logging.LoggingContext.Current.GetLogger());

            var guid = wfhost.PrepareStartWorkflow(wf, par);
            wfhost.RunWorkflow(guid);

            // Wait for workflow to complete
            workflowCompleted.WaitOne();
        }

        void wfhost_WorkflowEvent(object sender, WorkflowApplicationHostEventArgs e)
        {
            switch (e.EventType)
            {
                case WorkflowEventType.Completed:
                    Console.WriteLine("Query completed.");
                    break;
                case WorkflowEventType.Failed:
                    Console.WriteLine("Query failed.");
                    Console.WriteLine(e.Exception.Message);
                    break;
                case WorkflowEventType.Cancelled:
                    Console.WriteLine("Workflow cancelled.");
                    break;
                default:
                    throw new NotImplementedException();
            }

            workflowCompleted.Set();
        }
    }
}
