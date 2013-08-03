using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using Jhu.Graywulf.CommandLineParser;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.Jobs.Query;
using Jhu.SkyQuery.Jobs.Query;

namespace Jhu.SkyQuery.CmdLineUtil
{
    [Verb(Name="Run", Description="Executes a query in single server mode.")]
    class RunParameters : Parameters
    {
        private string serverName;
        private string databaseName;

        [Parameter(Name = "ServerName", Description = "SQL Server to use.", Required = true)]
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        [Parameter(Name = "DatabaseName", Description = "MYDB database.", Required = true)]
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public override void Run()
        {
            InitializeQuery();

            System.Activities.Activity activity;
            Dictionary<string, object> parameters;

            query.GetAsWorkflow(out activity, out parameters);

            WorkflowHost.ExecuteWorkflow(activity, parameters);
        }

        private QueryBase InitializeQuery()
        {
            var qf = new XMatchQueryFactory();

            var q = qf.CreateQuery(SelectedQuery.Value, ExecutionMode.Graywulf, OutputTable.Text);
            q.Verify();

            var q = CreateQuery();
            if (q != null)
            {
                q.DestinationSchemaName = "dbo";
                q.DestinationTableName = "quickResults";
                q.DestinationTableOperation = DestinationTableOperation.Drop | DestinationTableOperation.Create;

                var ji = ScheduleQuery(queuename, q);

                ResultsFrame.Attributes.Add("src", String.Format("ResultsProgress.aspx?guid={0}", ji.Guid));
                ResultsDiv.Visible = true;
                CloseResults.Visible = true;
            }

            /*
            base.InitializeQuery();
            
            query.ExecutionMode = ExecutionMode.SingleServer;

            // Add MyDB as custom source
            SqlServerDataset mydbds = new SqlServerDataset();
            mydbds.Name = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingDefaultDatasetName];
            mydbds.IsOnLinkedServer = false;
            mydbds.ConnectionString = BuildConnectionString();
            mydbds.DefaultSchemaName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingDefaultSchema];
            query.CustomDatasets.Add(mydbds);

            // Set up MYDB for destination
            query.DestinationDataset = mydbds;
            query.DestinationSchemaName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingDefaultSchema];
            query.DestinationTableName = "outputtable";     ///******
            query.DestinationTableOperation = DestinationTableOperation.Create;

            // Set up single server mode specifics
            SqlServerDataset tempds = new SqlServerDataset();
            tempds.IsOnLinkedServer = false;
            tempds.ConnectionString = BuildConnectionString();
            tempds.DatabaseName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingTemporaryDatabaseName];
            query.TemporaryDataset = tempds;
            query.TemporarySchemaName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingTemporarySchemaName];


            query.ID = DateTime.Now.ToString("yyMMddHHmmssff");
            query.User = "user";        // ******
            query.QueryTimeout = 1200;*/
        }
    }
}
