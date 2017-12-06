using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Tap.Client
{
    [TestClass]
    public class TapCommandTest
    {
        private const string connectionString = "Data Source=http://tapvizier.u-strasbg.fr/TAPVizieR/tap/";
        private const string testQuery = "SELECT TOP 10 ra, dec FROM \"I/337/gaia\"";

        [TestMethod]
        public void ExecuteReaderTest()
        {
            using (var cn = new TapConnection(connectionString))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQuery, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        int q = 0;
                        while (dr.Read())
                        {
                            Assert.AreEqual(2, dr.FieldCount);
                            q++;
                        }

                        Assert.AreEqual(10, q);
                    }
                }
            }
        }

        [TestMethod]
        public void InterruptReaderTest()
        {
            using (var cn = new TapConnection(connectionString))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQuery, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        // Read one row only, then dispose
                        dr.Read();
                        Assert.AreEqual(2, dr.FieldCount);
                    }
                }
            }
        }

        [TestMethod]
        public void FillDataSetTest()
        {
            using (var cn = new TapConnection(connectionString))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQuery, cn))
                {
                    var ds = new DataSet();
                    var da = new TapDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(ds);

                    // TODO: Add more asserts
                    var dt = ds.Tables[0];
                    Assert.AreEqual(2, dt.Columns.Count);
                    Assert.AreEqual("ra", dt.Columns[0].ColumnName);
                    Assert.AreEqual("dec", dt.Columns[1].ColumnName);
                    Assert.AreEqual(10, dt.Rows.Count);
                }
            }
        }

        [TestMethod]
        public void SchemaTableTest()
        {
            using (var cn = new TapConnection(connectionString))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQuery, cn))
                {
                    using (var dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        var dt = dr.GetSchemaTable();

                        Assert.AreEqual(2, dt.Rows.Count);
                        Assert.AreEqual("ra", dt.Rows[0][SchemaTableColumn.ColumnName]);
                        Assert.AreEqual(typeof(double), dt.Rows[0][SchemaTableColumn.DataType]);
                    }
                }
            }
        }

        // TODO: add test:
        // - read data with all possible column types
        // - read data with nulls in all possible column types
        // - test error behavior (e.g. syntax error in query, etc.)
    }
}
