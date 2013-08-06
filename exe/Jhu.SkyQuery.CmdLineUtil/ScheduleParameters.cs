using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Jhu.Graywulf.CommandLineParser;
using Jhu.Graywulf.Registry;
using Jhu.SkyQuery;

namespace Jhu.SkyQuery.CmdLineUtil
{
    [Verb(Name = "Schedule", Description = "Executes a query in single server mode.")]
    class ScheduleParameters : Parameters
    {
        [Parameter(Name = "Queue", Description = "Queue to schedule in.")]
        public QueueType Queue
        {
            get { return queue; }
            set { queue = value; }
        }

        public override void Run()
        {
            InitializeQuery();

            using (Context context = ContextManager.Instance.CreateContext(true))
            {
                query.Context = context;
                string queuename;
                switch (queue)
                {
                    case QueueType.Quick:
                        queuename = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingQuickQueue];
                        break;
                    case QueueType.Long:
                        queuename = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingLongQueue];
                        break;
                    case QueueType.Admin:
                        queuename = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingAdminQueue];
                        break;
                    default:
                        throw new NotImplementedException();
                }
                JobInstance job = query.ScheduleAsJob(queuename);
                job.Save();
            }
        }

        protected override void InitializeQuery()
        {
            base.InitializeQuery();

            query.ExecutionMode = ExecutionMode.Graywulf;

            if (integratedSecurity)
            {
                ContextManager.Instance.Login();
            }
            else
            {
                ContextManager.Instance.Login(userName, password);
            }

            using (Context context = ContextManager.Instance.CreateContext(true))
            {
                User user = new User(context);
                user.Guid = context.UserGuid;
                user.Load();

                // Add MyDB as custom source
                GraywulfDataset mydbds = new GraywulfDataset();
                mydbds.Name = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingDefaultDatasetName];
                mydbds.DatabaseInstanceName = String.Format(ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingUserMYDB], user.Name);
                mydbds.DefaultSchemaName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingDefaultSchema];
                query.CustomDatasets.Add(mydbds);

                // Set up MYDB for destination
                query.DestinationDataset = mydbds;
                query.DestinationSchemaName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingDefaultSchema];
                query.DestinationTableName = "outputtable";     ///******
                query.DestinationTableOperation = DestinationTableOperation.Create;

                // Set up single server mode specifics
                GraywulfDataset tempds = new GraywulfDataset();
                tempds.IsOnLinkedServer = false;
                tempds.DatabaseDefinitionName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingTemporaryDatabaseDefinitionName];
                query.TemporaryDataset = tempds;
                query.TemporarySchemaName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingTemporarySchemaName];

                query.ID = DateTime.Now.ToString("yyMMddHHmmssff");
                query.User = user.Name;
                query.QueryTimeout = 1200;
            }
        }
    }
}
