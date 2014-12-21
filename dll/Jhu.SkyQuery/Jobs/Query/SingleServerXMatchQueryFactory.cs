using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
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
        private SqlServerDataset mydbds;
        private SqlServerDataset tempds;
        private SqlServerDataset codeds;

        public SqlServerDataset UserDatabaseDataSet
        {
            get { return mydbds; }
            set { mydbds = value; }
        }

        public SqlServerDataset TempDatabaseDataset
        {
            get { return tempds; }
            set { tempds = value; }
        }

        public SqlServerDataset CodeDatabaseDataset
        {
            get { return codeds; }
            set { codeds = value; }
        }

        public SingleServerXMatchQueryFactory()
        {
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

            if (mydbds != null)
            {
                query.DefaultDataset = mydbds;
                // Add MyDB as custom source
                query.CustomDatasets.Add(mydbds);
            }

            // Set up temporary and code database
            query.TemporaryDataset = tempds;
            query.CodeDataset = codeds;
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
