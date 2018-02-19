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
        private const string connectionStringVizier = "Data Source=http://tapvizier.u-strasbg.fr/TAPVizieR/tap/";
        private const string testQuery = "SELECT TOP 10 ra, dec FROM \"I/337/gaia\"";
        private const string testQueryAllTypeParameterKepler = "SELECT TOP 10 \"J/AJ/151/68/catalog\".V1,  \"J/AJ/151/68/catalog\".KIC, \"J/AJ/151/68/catalog\".DV, \"J/AJ/151/68/catalog\".Per FROM \"J/AJ/151/68/catalog\"";
        private const string testQueryAllTypeParameterGaia = "SELECT TOP 10 \"I/337/gaia\".ra,  \"I/337/gaia\".ra_error,  \"I/337/gaia\".source_id, \"I/337/gaia\".duplicated_source FROM \"I/337/gaia\"";

        private const string connectionStringGaia = "Data Source=http://gaia.ari.uni-heidelberg.de/tap/";
        private const string testQueryGaia = " SELECT TOP 10 ra, dec FROM gaiadr1.tgas_source";
        private const string testQueryEmpty = " SELECT TOP 20  \"J/ApJ/678/102/table4\".\"S/N\",  \"J/ApJ/678/102/table4\".OID, \"J/ApJ/678/102/table4\".f_OID,  \"J/ApJ/678/102/table4\".Off,  \"J/ApJ/678/102/table4\".Type,  \"J/ApJ/678/102/table4\".Cat,  \"J/ApJ/678/102/table4\".n_Cat,  \"J/ApJ/678/102/table4\".Ref FROM \"J/ApJ/678/102/table4\"" ;

        private const string connectionStringAbellClusters = "Data Source: https://heasarc.gsfc.nasa.gov/cgi-bin/W3Browse/getvotable.pl?name=abell";
        private const string testQueryAbellClusters = " SELECT TOP 10";

        private const string connectionStringVstars = "Data Source=https://heasarc.gsfc.nasa.gov/cgi-bin/W3Browse/getvotable.pl?name=abell";
        private const string testQueryVstars = "";

        [TestMethod]
        public void ExecuteReaderTest()
        {
            using (var cn = new TapConnection(connectionStringVizier))
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
            using (var cn = new TapConnection(connectionStringVizier))
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
        public void InterruptReaderTestVstars()
        {
            using (var cn = new TapConnection(connectionStringVstars))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQueryVstars, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        // Read one row only, then dispose
                        dr.Read();
//                        Assert.AreEqual(2, dr.FieldCount);
                    }
                }
            }
        }

        [TestMethod]
        public void FillDataSetTest()
        {
            using (var cn = new TapConnection(connectionStringVizier))
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
            using (var cn = new TapConnection(connectionStringVizier))
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

        [TestMethod]
        public void InterruptReaderGaiaTest()
        {
            using (var cn = new TapConnection(connectionStringGaia))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQueryGaia, cn))
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
        public void EmptyElementReaderTest()
        {
            //Can't read empty element
            using (var cn = new TapConnection(connectionStringVizier))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQueryEmpty, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        int q = 0;
                        while (dr.Read())
                        {
                            Assert.AreEqual(8, dr.FieldCount);
                            q++;
                        }

                        Assert.AreEqual(20, q);
                    }
                }
            }
        }


        [TestMethod]
        public void AllTypeDataTestKeplerEB()
        {
            using (var cn = new TapConnection(connectionStringVizier))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQueryAllTypeParameterKepler, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        Assert.AreEqual(4, dr.FieldCount);
                    }
                }
            }

            using (var cn = new TapConnection(connectionStringVizier))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQueryAllTypeParameterKepler, cn))
                {
                    using (var dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        var dt = dr.GetSchemaTable();
                        Assert.AreEqual(typeof(short), dt.Rows[0][SchemaTableColumn.DataType]);
                        Assert.AreEqual(typeof(int), dt.Rows[1][SchemaTableColumn.DataType]);
                        Assert.AreEqual(typeof(string), dt.Rows[2][SchemaTableColumn.DataType]);
                        Assert.AreEqual(typeof(double), dt.Rows[3][SchemaTableColumn.DataType]);
                    }
                }
            }
        }

        [TestMethod]
        public void AllTypeDataTestGaia()
        {
            using (var cn = new TapConnection(connectionStringVizier))
            {
                cn.Open();

                using (var cmd = new TapCommand(testQueryAllTypeParameterGaia, cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        dr.Read();
                        Assert.AreEqual(4, dr.FieldCount);
                    }
                }
            }
        }

        [TestMethod]
        public void AllColumnTypeTest()
        {
            using (var cn =  new TapConnection(connectionStringVizier))
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
        // int, short,long, float, double, char(-string) already tested
        // - read data with nulls in all possible column types
        // - test error behavior (e.g. syntax error in query, etc.)
    }
}
