using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jhu.SkyQuery.Format.Test
{
    [TestClass]
    public class VOTableTest
    {
        [TestMethod]
        public void CheckVOTable()
        {
            var file = "H:\\AllTAP\\test1.xml";
            XmlReader xReader = XmlReader.Create(file);

            VOTable vot = new VOTable(xReader, culture);
            vot.DetectColumns();
            var dr = vot.OpenDataReader();

            Assert.AreEqual("objid", dr.GetName(0));
            Assert.AreEqual("ra", dr.GetName(1));
            Assert.AreEqual("dec", dr.GetName(2));

            //first
            dr.Read();
            //second
            dr.Read();
        }

        [TestMethod]
        public void SimpleVOTableWriter()
        {
            var w = new StringWriter();
            var wr = new XmlTextWriter(w);
            using (var cn = new SqlConnection(Jhu.Graywulf.Test.Constants.TestConnectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand(" SELECT top 2 ra,dec from Frame ", cn))
                {
                    using (var dr = cmd.ExecuteReader())
                    {
                        var vot = new VOTable(wr, culture);
                        vot.WriteFromDataReader(dr);
                    }
                }
            }
            Assert.AreEqual("<VOTABLE><RESOURCE><TABLE><FIELD name=\"ra\" datatype=\"float\" /><FIELD name=\"dec\" datatype=\"float\" /><Data><TABLEDATA><TR><TD>336.514598443829</TD><TD>-0.931640678911959</TD></TR><TR><TD>336.514598443829</TD><TD>-0.931640678911959</TD></TR></TABLEDATA></Data></TABLE></RESOURCE></VOTABLE>", w.ToString());
        }

        [TestMethod]
        public void CompressedVOTableWriterTest()
        {
            //var path = "C:\\Deoyani\\temp\\VOTableTest_CompressedWriter.vot.gz";
            var path = "C:\\Deoyani\\temp\\VOTableTest_CompressedWriter.vot.zip";
            using (var vot = new VOTable(path, DataFileMode.Write))
            {
                //vot.Compression = CompressionMethod.GZip;
                vot.Compression = CompressionMethod.Zip;

                using (var cn = new SqlConnection(Jhu.Graywulf.Test.Constants.TestConnectionString))
                {
                    cn.Open();

                    using (var cmd = new SqlCommand("SELECT top 2 ra,dec from Field", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            vot.WriteFromDataReader(dr);
                        }
                    }
                }
            }

            Assert.IsTrue(File.Exists(path));
            //File.Delete(path);
        }

        [TestMethod]
        public void VOTableWriterTest()
        {
            var path = "C:\\Deoyani\\temp\\VOTableTest_Writer.xml";

            using (var vot = new VOTable(path, DataFileMode.Write))
            {
                vot.Compression = CompressionMethod.None;

                using (var cn = new SqlConnection(Jhu.Graywulf.Test.Constants.TestConnectionString))
                {
                    cn.Open();

                    using (var cmd = new SqlCommand("SELECT top 2 ra,dec from Field", cn))
                    {
                        using (var dr = cmd.ExecuteReader())
                        {
                            vot.WriteFromDataReader(dr);
                        }
                    }
                }
            }

            Assert.IsTrue(File.Exists(path));
            //File.Delete(path);
        }
    }
}
