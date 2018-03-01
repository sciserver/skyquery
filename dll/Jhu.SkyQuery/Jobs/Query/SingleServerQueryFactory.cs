using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Jhu.Graywulf.Sql.Schema;
using Jhu.Graywulf.Sql.Schema.SqlServer;
using Jhu.Graywulf.IO.Tasks;
using Jhu.Graywulf.Sql.Jobs.Query;

namespace Jhu.SkyQuery.Jobs.Query
{
    /// <summary>
    /// Implements functions to initialize a query that runs in 
    /// single server mode.
    /// </summary>
    [Serializable]
    public class SingleServerQueryFactory : SkyQueryQueryFactory
    {
        private Dictionary<string, SqlServerDataset> customDatasets;

        public Dictionary<string, SqlServerDataset> CustomDatasets
        {
            get { return customDatasets; }
        }

        public SingleServerQueryFactory()
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
            ds.DefaultSchemaName = Jhu.Graywulf.Sql.Schema.SqlServer.Constants.DefaultSchemaName;
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
        protected override void InitializeQuery(SqlQuery query, string queryString)
        {
            // TODO: factor it out to a new class

            query.Parameters.ExecutionMode = ExecutionMode.SingleServer;
            query.Parameters.QueryString = queryString;

            query.Parameters.QueryTimeout = 7200;

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
                query.Parameters.DefaultSourceDataset = mydbds;
                query.Parameters.DefaultOutputDataset = mydbds;

                // Add MyDB as custom source
                query.Parameters.CustomDatasets.Add(mydbds);
            }

            // Set up temporary and code database
            query.TemporaryDataset = tempds;
            query.CodeDataset = codeds;

            query.Parameters.Destination = new DestinationTable()
            {
                Dataset = mydbds,
                Options = TableInitializationOptions.Create,
                SchemaName = Jhu.Graywulf.Sql.Schema.SqlServer.Constants.DefaultSchemaName,
                TableNamePattern = "outputtable"
            };
        }

        /// <summary>
        /// Creates a workflow job that can be used to execute the query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public System.Activities.Activity GetAsWorkflow(SqlQuery query)
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
        public Dictionary<string, object> GetWorkflowParameters(SqlQuery query)
        {
            return new Dictionary<string, object>()
            {
                { Jhu.Graywulf.Registry.Constants.JobParameterParameters, query.Parameters },
                { Jhu.Graywulf.Activities.Constants.ActivityParameterJobInfo, null },
            };
        }
    }
}
