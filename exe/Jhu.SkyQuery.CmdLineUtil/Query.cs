using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
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

        private SqlServerDataset CreateDataset(string name, string connectionString)
        {
            // Take connection string from command-line arguments but replace database name
            var dscsb = new SqlConnectionStringBuilder(connectionString);
            var ds = new SqlServerDataset();

            ds.Name = name;
            ds.DefaultSchemaName = Jhu.Graywulf.Schema.SqlServer.Constants.DefaultSchemaName;
            ds.ConnectionString = GetConnectionString();
            ds.DatabaseName = dscsb.InitialCatalog;

            return ds;
        }

        private Dictionary<string, SqlServerDataset> GetCustomDatasets()
        {
            var customds = new Dictionary<string, SqlServerDataset>(StringComparer.InvariantCultureIgnoreCase);
            foreach (ConnectionStringSettings cstr in ConfigurationManager.ConnectionStrings)
            {
                if (cstr.Name.StartsWith(SqlServerSchemaManager.ConnectionStringNamePrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    var name = cstr.Name.Substring(SqlServerSchemaManager.ConnectionStringNamePrefix.Length + 1);
                    customds.Add(name, CreateDataset(name, cstr.ConnectionString));
                }
            }

            return customds;
        }

        public override void Run()
        {
            // Load query string from file
            var query = System.IO.File.ReadAllText(input);

            // Read connection strings from config
            var f = new SingleServerXMatchQueryFactory();
            f.CustomDatasets = GetCustomDatasets();

            // Create query and verify
            var q = f.CreateQuery(query);
            q.Verify();

            q.DumpSql = dumpSql;

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
