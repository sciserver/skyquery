using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Jhu.Graywulf.Registry;
using Jhu.Graywulf.CommandLineParser;
using Jhu.SkyQuery.Lib;
using Jhu.SkyQuery.Schema;


namespace Jhu.SkyQuery.CmdLineUtil
{
    abstract class Parameters
    {
        //protected string serverName;
        //protected string databaseName;
        protected string userName;
        protected string password;
        protected bool integratedSecurity;
        protected string inputFile;
        //protected QueueType queue;

        //protected QueryBase query;

        [Parameter(Name = "UserName", Description = "User name.")]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [Parameter(Name = "Password", Description = "Password.")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [Option(Name = "EnableIntegratedSecurity", Description = "Use Windows login.")]
        public bool IntegratedSecurity
        {
            get { return integratedSecurity; }
            set { integratedSecurity = value; }
        }

        [Parameter(Name = "InputFile", Description = "File containing query.", Required = true)]
        public string InputFile
        {
            get { return inputFile; }
            set { inputFile = value; }
        }

        public abstract void Run();

#if false // TODO delete
        protected virtual void InitializeQuery()
        {
            // Read input file to load the query
            query = QueryBase.Create(null, File.ReadAllText(inputFile));

            query.SourceDatabaseRedundancyState = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingSourceRedundancyState];
            query.MiniDatabaseRedundancyState = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingStatRedundancyState]; ;

            query.DefaultSchemaName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingDefaultSchema];
            query.DefaultDatasetName = ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingDefaultDatasetName];

            query.CacheRemoteTables = true;
            query.QueryTimeout = Int32.Parse(ConfigurationManager.AppSettings[Jhu.SkyQuery.Lib.Constants.AppSettingLongQueryTimeout]);

            query.ResultsetTarget = ResultsetTarget.DestinationTable;
            query.TemporaryDestinationTableName = "output";
            query.KeepTemporaryDestinationTable = true;
        }

        public string BuildConnectionString()
        {
            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
            csb.DataSource = serverName;
            csb.InitialCatalog = databaseName;

            if (!integratedSecurity)
            {
                csb.UserID = userName;
                csb.Password = password;
            }
            csb.IntegratedSecurity = integratedSecurity;

            return csb.ConnectionString;
        }
#endif
    }
}
