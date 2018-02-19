using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Sql.Schema;

namespace Jhu.SkyQuery.Tap.Client
{
    public abstract class TapDatasetTest
    {
        protected abstract string ConnectionString { get; }
        
        protected TapDataset CreateTestDataset()
        {
            var csb = new TapConnectionStringBuilder(ConnectionString);

            var ds = new TapDataset(Jhu.Graywulf.Test.Constants.TestDatasetName, csb.ConnectionString);

            return ds;
        }

        public Table GetSingleTableTestHelper(string databaseName, string schemaName, string tableName)
        {
            var ds = CreateTestDataset();
            return ds.Tables[databaseName, schemaName, tableName];
        }

        public void LoadAllTablesTestHelper()
        {
            var ds = CreateTestDataset();

            ds.Tables.LoadAll(false);

            Assert.IsTrue(ds.Tables.Count > 0);
        }
    }
}