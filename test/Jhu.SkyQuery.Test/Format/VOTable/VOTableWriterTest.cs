using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.VOTable;

namespace Jhu.SkyQuery.Format.VOTable.Test
{
    [TestClass]
    public class VOTableWriterTest
    {
        
        /*
        [TestMethod]
        public void SimpleWriterTest()
        {
            var w = new StringWriter();

            var s = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "\t"
            };
            var xw = XmlWriter.Create(w, s);

            using (var cn = new SqlConnection(Jhu.Graywulf.Test.AppSettings.SqlServerSchemaTestConnectionString))
            {
                cn.Open();

                using (var cmd = new SqlCommand("SELECT SampleData.* FROM SampleData", cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        var votable = new VOTable(xw);
                        votable.WriteFromDataReader(dr);
                    }
                }
            }

            xw.Flush();

            Assert.AreEqual(
"<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<VOTABLE version=\"1.2\">\r\n\t<RESOURCE>\r\n\t\t<TABLE>\r\n\t\t\t<FIELD name=\"float\" />\r\n\t\t\t<FIELD name=\"double\" />\r\n\t\t\t<FIELD name=\"decimal\" />\r\n\t\t\t<FIELD name=\"nvarchar(50)\" />\r\n\t\t\t<FIELD name=\"bigint\" />\r\n\t\t\t<FIELD name=\"int\" />\r\n\t\t\t<FIELD name=\"tinyint\" />\r\n\t\t\t<FIELD name=\"smallint\" />\r\n\t\t\t<FIELD name=\"bit\" />\r\n\t\t\t<FIELD name=\"ntext\" />\r\n\t\t\t<FIELD name=\"char\" />\r\n\t\t\t<FIELD name=\"datetime\" />\r\n\t\t\t<FIELD name=\"guid\" />\r\n\t\t\t<DATA>\r\n\t\t\t\t<TABLEDATA>\r\n\t\t\t\t\t<TR>\r\n\t\t\t\t\t\t<TD>1.234568</TD>\r\n\t\t\t\t\t\t<TD>1.23456789</TD>\r\n\t\t\t\t\t\t<TD>1.2346</TD>\r\n\t\t\t\t\t\t<TD>this is text</TD>\r\n\t\t\t\t\t\t<TD>123456789</TD>\r\n\t\t\t\t\t\t<TD>123456</TD>\r\n\t\t\t\t\t\t<TD>123</TD>\r\n\t\t\t\t\t\t<TD>12345</TD>\r\n\t\t\t\t\t\t<TD>True</TD>\r\n\t\t\t\t\t\t<TD>this is unicode text ő</TD>\r\n\t\t\t\t\t\t<TD>A</TD>\r\n\t\t\t\t\t\t<TD>08/17/2012 00:00:00</TD>\r\n\t\t\t\t\t\t<TD>68652251-c9e4-4630-80be-88b96d3258ce</TD>\r\n\t\t\t\t\t</TR>\r\n\t\t\t\t</TABLEDATA>\r\n\t\t\t</DATA>\r\n\t\t</TABLE>\r\n\t</RESOURCE>\r\n</VOTABLE>",
                w.ToString());

        }*/

        [TestMethod]
        public void SimpleWriterTest()
        {
            var uri = new Uri("VOTableWriterTest_SimpleWriterTest.votable", UriKind.Relative);

            using (var nat = new VOTable(uri, DataFileMode.Write))
            {
                using (var cn = new SqlConnection(Jhu.Graywulf.Test.AppSettings.IOTestConnectionString))
                {
                    cn.Open();

                    using (var cmd = new SqlCommand("SELECT * FROM SampleData_NumericTypes", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleWriterNullsTest()
        {
            var uri = new Uri("VOTableWriterTest_SimpleWriterNullsTest.votable", UriKind.Relative);

            using (var nat = new VOTable(uri, DataFileMode.Write))
            {
                using (var cn = new SqlConnection(Jhu.Graywulf.Test.AppSettings.IOTestConnectionString))
                {
                    cn.Open();

                    using (var cmd = new SqlCommand("SELECT * FROM SampleData_NumericTypes_Null", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleWriterAllTypesTest()
        {
            var uri = new Uri("VOTableWriterTest_SimpleWriterAllTypesTest.votable", UriKind.Relative);

            using (var nat = new VOTable(uri, DataFileMode.Write))
            {
                using (var cn = new SqlConnection(Jhu.Graywulf.Test.AppSettings.IOTestConnectionString))
                {
                    cn.Open();

                    using (var cmd = new SqlCommand("SELECT * FROM SampleData_AllTypes", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }

        [TestMethod]
        public void SimpleWriterAllTypesNullableTest()
        {
            var uri = new Uri("VOTableWriterTest_SimpleWriterAllTypesNullableTest.votable", UriKind.Relative);

            using (var nat = new VOTable(uri, DataFileMode.Write))
            {
                using (var cn = new SqlConnection(Jhu.Graywulf.Test.AppSettings.IOTestConnectionString))
                {
                    cn.Open();

                    using (var cmd = new SqlCommand("SELECT * FROM SampleData_AllTypes_Nullable", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            nat.WriteFromDataReader(dr);
                        }
                    }
                }
            }
        }
        
        
    }
}
