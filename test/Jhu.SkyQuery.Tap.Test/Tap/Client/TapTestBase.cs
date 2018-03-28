using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Sql.Schema;

namespace Jhu.SkyQuery.Tap.Client
{
    public abstract class TapTestBase : IDisposable
    {
        private static TapConnection connection;
        private static TapDataset dataset;

        protected abstract Uri BaseUri { get; }

        protected string ConnectionString
        {
            get
            {
                return "Data Source=" + BaseUri.ToString();
            }
        }

        protected virtual string TestQuery
        {
            get
            {
                return "SELECT TOP 10 schema_name, table_name FROM TAP_SCHEMA.tables";
            }
        }

        public TapTestBase()
        {
            connection = new TapConnection(ConnectionString);
            connection.Open();

            var csb = new TapConnectionStringBuilder(ConnectionString);
            dataset = new TapDataset(Jhu.Graywulf.Test.Constants.TestDatasetName, csb.ConnectionString);
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();

            dataset = null;
        }

        /*
        [TestMethod]
        public async Task SubmitQueryAsyncTest()
        {
            using (var tap = new TapClient(BaseUri))
            {
                await tap.GetCapabilitiesAsync(CancellationToken.None);
                tap.GetBestFormat(out var format, out var mime);

                var job = new TapJob()
                {
                    Query = TestQuery,
                    Format = mime
                };

                var stream = await tap.ExecuteJobAsync(job, CancellationToken.None);
                var reader = new System.IO.StreamReader(stream);
                var data = reader.ReadToEnd();
            }
        }

        [TestMethod]
        protected async Task SubmitQuerySyncTest()
        {
            using (var tap = new TapClient(BaseUri))
            {
                tap.GetBestFormat(out var format, out var mime);

                var job = new TapJob()
                {
                    Query = TestQuery,
                    Format = mime,
                    IsAsync = false,
                };

                var stream = await tap.ExecuteJobAsync(job, CancellationToken.None);
                var reader = new System.IO.StreamReader(stream);
                var data = reader.ReadToEnd();
            }
        }
        */

        /*
    protected string GetBestFormatMimeTypeTestHelper()
    {
        connection.

        using (var tap = new TapClient(BaseUri))
        {
            tap.GetCapabilitiesAsync(CancellationToken.None).Wait();
            tap.GetBestFormat(out TapOutputFormat format, out string mime);

            return mime;
        }
    }
    */

        [TestMethod]
        public void GetSingleTableTest()
        {
            var table = dataset.Tables[null, "TAP_SCHEMA", "tables"];
        }

        [TestMethod]
        public void LoadAllTablesTest()
        {
            dataset.Tables.Clear();
            dataset.Tables.LoadAll(false);
            Assert.IsTrue(dataset.Tables.Count > 0);
        }

        [TestMethod]
        public async Task ExecuteReaderTest()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            using (var cmd = new TapCommand(TestQuery, connection))
            {
                cmd.CommandTimeout = 20;

                using (var dr = await cmd.ExecuteReaderAsync(cts.Token))
                {
                    int q = 0;
                    while (await dr.ReadAsync())
                    {
                        Assert.AreEqual(2, dr.FieldCount);
                        q++;
                    }

                    Assert.IsTrue(q > 0);
                }
            }
        }

        [TestMethod]
        public async Task InterruptReaderTest()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            using (var cmd = new TapCommand(TestQuery, connection))
            {
                cmd.CommandTimeout = 20;

                using (var dr = await cmd.ExecuteReaderAsync(cts.Token))
                {
                    // Read one row only, then dispose
                    await dr.ReadAsync();
                    Assert.AreEqual(2, dr.FieldCount);
                }

                // Only read one row and stop
            }
        }

        [TestMethod]
        public async Task FillDataSetTest()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            using (var cmd = new TapCommand(TestQuery, connection))
            {
                cmd.CommandTimeout = 20;

                var ds = new DataSet();
                var da = new TapDataAdapter();
                da.SelectCommand = cmd;

                await Task.Run(() => da.Fill(ds), cts.Token);

                // TODO: Add more asserts
                var dt = ds.Tables[0];
                Assert.AreEqual(2, dt.Columns.Count);
                Assert.IsTrue(dt.Rows.Count > 0);
            }
        }

        [TestMethod]
        public async Task SchemaTableTest()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));

            using (var cmd = new TapCommand(TestQuery, connection))
            {
                using (var dr = await cmd.ExecuteReaderAsync(CommandBehavior.SchemaOnly, cts.Token))
                {
                    var dt = dr.GetSchemaTable();

                    Assert.AreEqual(2, dt.Rows.Count);
                }
            }
        }
    }
}
