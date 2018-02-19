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
    abstract public class TapCommandTest
    {
        protected abstract string ConnectionString { get; }

        protected void ExecuteReaderTestHelper(string testQuery, int fieldCount, int rowCount)
        {
            using (var cn = new TapConnection(ConnectionString))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQuery, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        int q = 0;
                        while (dr.Read())
                        {
                            Assert.AreEqual(fieldCount, dr.FieldCount);
                            q++;
                        }

                        Assert.AreEqual(rowCount, q);
                    }
                }
            }
        }

        public void InterruptReaderTestHelper(string testQuery, int fieldCount)
        {
            using (var cn = new TapConnection(ConnectionString))
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
        
        public void FillDataSetTestHelper(string testQuery, string[] fieldNames, int rowCount)
        {
            using (var cn = new TapConnection(ConnectionString))
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
                    Assert.AreEqual(fieldNames.Length, dt.Columns.Count);
                    Assert.AreEqual(rowCount, dt.Rows.Count);

                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        Assert.AreEqual(fieldNames[i], dt.Columns[i].ColumnName);
                    }
                }
            }
        }

        public void SchemaTableTestHelper(string testQuery, string[] fieldNames, Type[] fieldTypes)
        {
            using (var cn = new TapConnection(ConnectionString))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQuery, cn))
                {
                    using (var dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        var dt = dr.GetSchemaTable();

                        Assert.AreEqual(fieldNames.Length, dt.Rows.Count);

                        for (int i = 0; i < fieldNames.Length; i++)
                        {
                            Assert.AreEqual(fieldNames[i], dt.Rows[i][SchemaTableColumn.ColumnName]);
                            Assert.AreEqual(fieldTypes[i], dt.Rows[i][SchemaTableColumn.DataType]);
                        }
                    }
                }
            }
        }
    }
}
