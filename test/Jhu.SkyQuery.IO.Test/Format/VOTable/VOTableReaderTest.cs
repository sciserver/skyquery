using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Jhu.Graywulf.Test;
using Jhu.Graywulf.Util;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.VOTable
{
    [TestClass]
    public class VOTableReaderTest : VOTableTestBase
    {
        [TestMethod]
        public void ValidationTest()
        {
            var xml = File.ReadAllText(GetTestFilePath(@"modules\skyquery\test\files\votable_everything.xml"));
            ValidateVOTable(xml);
        }

        [TestMethod]
        public void SimpleReadTest()
        {
            var dr = OpenSimpleReader("votable_simple.xml");

            int q = 0;

            while (dr.Read())
            {
                if (q == 0)
                {
                    int o = -1;
                    Assert.AreEqual(1237651758284999317L, dr.GetInt64(++o));
                    Assert.AreEqual(270.000683875077, dr.GetDouble(++o));
                    Assert.AreEqual(0.00379192855675463, dr.GetDouble(++o));
                    Assert.AreEqual(19.12505f, dr.GetFloat(++o));
                    Assert.AreEqual(17.33891f, dr.GetFloat(++o));
                    Assert.AreEqual(16.45144f, dr.GetFloat(++o));
                    Assert.AreEqual(16.02683f, dr.GetFloat(++o));
                    Assert.AreEqual(15.72931f, dr.GetFloat(++o));
                }

                q++;
            }

            Assert.AreEqual(10, q);
        }

        [TestMethod]
        public void MultipleResourceReadTest()
        {
            var dr = OpenSimpleReader("votable_multiresource.xml");

            int q = 0;

            do
            {
                while (dr.Read())
                {
                    if (q == 0)
                    {
                        Assert.AreEqual(8, dr.FieldCount);

                        int o = -1;
                        Assert.AreEqual(1237651758284999317L, dr.GetInt64(++o));
                        Assert.AreEqual(270.000683875077, dr.GetDouble(++o));
                        Assert.AreEqual(0.00379192855675463, dr.GetDouble(++o));
                        Assert.AreEqual(19.12505f, dr.GetFloat(++o));
                        Assert.AreEqual(17.33891f, dr.GetFloat(++o));
                        Assert.AreEqual(16.45144f, dr.GetFloat(++o));
                        Assert.AreEqual(16.02683f, dr.GetFloat(++o));
                        Assert.AreEqual(15.72931f, dr.GetFloat(++o));
                    }

                    if (q == 10)
                    {
                        Assert.AreEqual(2, dr.FieldCount);

                        int o = -1;
                        Assert.AreEqual(270.000683875077, dr.GetDouble(++o));
                        Assert.AreEqual(0.00379192855675463, dr.GetDouble(++o));
                    }

                    q++;
                }
            }
            while (dr.NextResult());

            Assert.AreEqual(12, q);
        }

        [TestMethod]
        public void NullValuesReadTest()
        {
            var dr = OpenSimpleReader("votable_nulls.xml");

            int q = 0;

            while (dr.Read())
            {
                q++;
            }

            Assert.AreEqual(3, q);
        }

        [TestMethod]
        public void EmptyDataReadTest()
        {
            var dr = OpenSimpleReader("votable_emptydata.xml");

            int q = 0;
            int r = 0;

            do
            {
                while (dr.Read())
                {
                    q++;
                }
                r++;
            }
            while (dr.NextResult());

            Assert.AreEqual(0, q);
            Assert.AreEqual(2, r);
        }

        [TestMethod]
        public void IvoaSampleDataReadTest()
        {
            var dr = OpenSimpleReader("votable_ivoa.xml");

            int q = 0;
            int r = 0;

            do
            {
                while (dr.Read())
                {
                    q++;
                }
                r++;
            }
            while (dr.NextResult());

            Assert.AreEqual(3, q);
            Assert.AreEqual(1, r);
        }

        [TestMethod]
        public void EverythingTest()
        {
            var dr = OpenSimpleReader("votable_everything.xml");

            int q = 0;
            int r = 0;

            do
            {
                while (dr.Read())
                {
                    q++;
                }
                r++;
            }
            while (dr.NextResult());
        }

        [TestMethod]
        public void ReadTap_VOTableTest()
        {
            //var dr = OpenSimpleReader(@"tap_votable\viziercorotvotable.xml");
            var dr = OpenSimpleReader(@"tap_votable\votable_ivoa.xml");
            int q = 0;
            int r = 0;
            do
            {
                while (dr.Read())
                {
                    q++;
                }
                r++;
            }
            while (dr.NextResult());

            Assert.AreEqual(3, q);
            Assert.AreEqual(1, r);
        }
    }
}
