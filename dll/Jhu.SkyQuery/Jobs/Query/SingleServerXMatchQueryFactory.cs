using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Schema.SqlServer;
using Jhu.Graywulf.IO.Tasks;
using Jhu.Graywulf.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query
{
    /// <summary>
    /// Implements function to initialize a query that runs in 
    /// single server mode.
    /// </summary>
    [Serializable]
    public class SingleServerXMatchQueryFactory : XMatchQueryFactory
    {
        private Dictionary<string, SqlServerDataset> customDatasets;

        public Dictionary<string, SqlServerDataset> CustomDatasets
        {
            get { return customDatasets; }
        }

        public SingleServerXMatchQueryFactory()
        {
            InitializeMembers();
        }

        private void InitializeMembers()
        {
            this.customDatasets = new Dictionary<string, SqlServerDataset>(StringComparer.InvariantCultureIgnoreCase);
        }

        private string GetConnectionString(string server, string userId, string password, bool integratedSecurity)
        {
            var csb = new SqlConnectionStringBuilder();

            csb.DataSource = server;

            if (integratedSecurity)
            {
                csb.IntegratedSecurity = true;
            }
            else
            {
                csb.IntegratedSecurity = false;
                csb.UserID = userId;
                csb.Password = password;
            }

            return csb.ConnectionString;
        }

        private SqlServerDataset CreateDataset(string name, string connectionString, string server, string userId, string password, bool integratedSecurity)
        {
            // Take connection string from command-line arguments but replace database name
            var dscsb = new SqlConnectionStringBuilder(connectionString);
            var ds = new SqlServerDataset();

            ds.Name = name;
            ds.DefaultSchemaName = Jhu.Graywulf.Schema.SqlServer.Constants.DefaultSchemaName;
            ds.ConnectionString = GetConnectionString(server, userId, password, integratedSecurity);
            ds.DatabaseName = dscsb.InitialCatalog;

            return ds;
        }

        public void LoadCustomDatasets(string server, string userId, string password, bool integratedSecurity)
        {
            var customds = new Dictionary<string, SqlServerDataset>(StringComparer.InvariantCultureIgnoreCase);
            foreach (ConnectionStringSettings cstr in ConfigurationManager.ConnectionStrings)
            {
                if (cstr.Name.StartsWith(SqlServerSchemaManager.ConnectionStringNamePrefix, StringComparison.InvariantCultureIgnoreCase))
                {
                    var name = cstr.Name.Substring(SqlServerSchemaManager.ConnectionStringNamePrefix.Length + 1);
                    customds.Add(name, CreateDataset(name, cstr.ConnectionString, server, userId, password, integratedSecurity));
                }
            }
        }

        /// <summary>
        /// Initializes a query object for execution outside the Graywulf framework.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryString"></param>
        /// <param name="outputTable"></param>
        /// <param name="mydbds"></param>
        /// <param name="tempds"></param>
        /// <param name="codeds"></param>
        protected override void InitializeQuery(QueryBase query, string queryString)
        {
            // TODO: factor it out to a new class

            query.ExecutionMode = ExecutionMode.SingleServer;
            query.QueryString = queryString;

            query.QueryTimeout = 7200;

            SqlServerDataset mydbds = null;
            SqlServerDataset tempds = null;
            SqlServerDataset codeds = null;

            // MyDB
            if (customDatasets.ContainsKey(Jhu.Graywulf.Registry.Constants.UserDbName))
            {
                mydbds = customDatasets[Jhu.Graywulf.Registry.Constants.UserDbName];
                mydbds.IsMutable = true;
            }

            // TempDB
            if (customDatasets.ContainsKey(Jhu.Graywulf.Registry.Constants.TempDbName))
            {
                tempds = customDatasets[Jhu.Graywulf.Registry.Constants.TempDbName];
                tempds.IsMutable = true;
            }

            // CodeDB
            if (customDatasets.ContainsKey(Jhu.Graywulf.Registry.Constants.CodeDbName))
            {
                codeds = customDatasets[Jhu.Graywulf.Registry.Constants.CodeDbName];
            }

            if (mydbds != null)
            {
                query.DefaultDataset = mydbds;
                // Add MyDB as custom source
                query.CustomDatasets.Add(mydbds);
            }

            // Set up temporary and code database
            query.TemporaryDataset = tempds;
            query.CodeDataset = codeds;

            query.Destination = new DestinationTable()
            {
                Dataset = mydbds,
                Options = TableInitializationOptions.Create,
                SchemaName = Jhu.Graywulf.Schema.SqlServer.Constants.DefaultSchemaName,
                TableNamePattern = "outputtable"
            };
        }

        /// <summary>
        /// Creates a workflow job that can be used to execute the query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public System.Activities.Activity GetAsWorkflow(QueryBase query)
        {
            if (!(query is XMatchQuery))
            {
                return new SqlQueryJob();
            }
            else
            {
                return new XMatchQueryJob();
            }
        }

        /// <summary>
        /// Returns a disctionary of parameters used to configure a query job.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <remarks>
        /// This function is used by the single-server mode command-line utility only.
        /// </remarks>
        public Dictionary<string, object> GetWorkflowParameters(QueryBase query)
        {
            return new Dictionary<string, object>()
            {
                { Jhu.Graywulf.Jobs.Constants.JobParameterQuery, query },
                { Jhu.Graywulf.Jobs.Constants.JobParameterUserGuid, Guid.Empty },
                { Jhu.Graywulf.Jobs.Constants.JobParameterJobGuid, Guid.Empty },
            };
        }
    }
}
