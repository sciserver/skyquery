using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using Jhu.Graywulf.CommandLineParser;
using Jhu.Graywulf.Activities;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.CmdLineUtil
{
    [Verb(Name = "Query", Description = "Executes a query in single-server mode")]
    class Query : Verb
    {
        private string server;
        private bool integratedSecurity;
        private string userId;
        private string password;
        private string myDB;
        private string tempDB;
        private string input;

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

        [Parameter(Name = "MyDB", Description = "Database for results")]
        public string MyDB
        {
            get { return myDB; }
            set { myDB = value; }
        }

        [Parameter(Name = "TempDB", Description = "Temporary database")]
        public string TempDB
        {
            get { return tempDB; }
            set { tempDB = value; }
        }

        [Parameter(Name = "Input", Description = "Input file containing the query")]
        public string Input
        {
            get { return input; }
            set { input = value; }
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
            this.myDB = "SkyQuery_MYDB";
            this.myDB = "SkyQuery_TEMP";
            this.input = null;

            this.workflowCompleted = new AutoResetEvent(false);
        }

        private string GetConnectionString()
        {
            var csb = new SqlConnectionStringBuilder();

            csb.DataSource = this.server;

            if (this.integratedSecurity)
            {
                csb.IntegratedSecurity = true;
            }
            else
            {
                csb.IntegratedSecurity = false;
                csb.UserID = this.userId;
                csb.Password = this.password;
            }

            return csb.ConnectionString;
        }

        public override void Run()
        {
            // Load query string from file
            var query = System.IO.File.ReadAllText(input);

            // MyDB
            var mydbds = new SqlServerDataset();
            mydbds.Name = "MYDB";
            mydbds.DefaultSchemaName = "dbo";
            mydbds.ConnectionString = GetConnectionString();
            mydbds.DatabaseName = myDB;

            var tempds = new SqlServerDataset();
            tempds.IsOnLinkedServer = false;
            tempds.ConnectionString = GetConnectionString();
            tempds.DatabaseName = tempDB;

            // Create query and verify
            var f = new XMatchQueryFactory();
            var q = f.CreateQuery(query, ExecutionMode.SingleServer, null, mydbds, tempds);
            q.Verify();

            // Create a workflow
            var wf = f.GetAsWorkflow(q);
            var par = f.GetWorkflowParameters(q);

            // Submit for execution
            var wfhost = new WorkflowApplicationHost();
            wfhost.WorkflowEvent += new EventHandler<HostEventArgs>(wfhost_WorkflowEvent);

            wfhost.Start();

            var guid = wfhost.PrepareStartWorkflow(wf, par);
            wfhost.RunWorkflow(guid);

            // Wait for workflow to complete
            workflowCompleted.WaitOne();
        }

        void wfhost_WorkflowEvent(object sender, HostEventArgs e)
        {
            switch (e.EventType)
            {
                case WorkflowEventType.Completed:
                    Console.WriteLine("Query completed.");
                    break;
                case WorkflowEventType.Failed:
                    Console.WriteLine("Query failed.");
                    Console.WriteLine(e.ExceptionMessage);
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
