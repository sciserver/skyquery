﻿using System;
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

namespace Jhu.SkyQuery.Format.VoTable
{
    [TestClass]
    public class VOTableReaderTest : VOTableTestBase
    {
        private void VoTableReaderTestHelper(string filename, int rows, int resultsets, object[][] gt)
        {
            using (var dr = OpenSimpleReader(filename))
            {
                int q = 0;
                int r = 0;

                do
                {
                    var values = new object[dr.FieldCount];

                    while (dr.Read())
                    {
                        dr.GetValues(values);

                        if (gt != null && gt.Length > q)
                        {
                            for (int i = 0; i < values.Length; i++)
                            {
                                Assert.AreEqual(gt[q][i], values[i]);
                            }
                        }

                        q++;
                    }
                    r++;
                }
                while (dr.NextResult());

                Assert.AreEqual(rows, q);
                Assert.AreEqual(resultsets, r);
            }
        }

        private void VoTableInterruptReaderTestHelper(string filename)
        {
            using (var dr = OpenSimpleReader(filename))
            {
                dr.Read();
            }
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

        /// <summary>
        /// 
        /// 
        [TestMethod]
        public void WithNullValuesReadTest()
        {
            var dr = OpenSimpleReader("votable_nulls2.xml");

            int q = 0;
            int r = 0;

            do
            {
                q = 0;
                while (dr.Read())
                {
                    q++;
                }
                r++;
            }
            while (dr.NextResult());

            Assert.AreEqual(1, q);
            Assert.AreEqual(2, r);
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
        public void VizierSampleDataReadTest()
        {
            var dr = OpenSimpleReader("vizier_corot_with_date.xml");

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

            Assert.AreEqual(2, q);
            Assert.AreEqual(1, r);
        }

        [TestMethod]
        [ExpectedException(typeof(Jhu.VO.VoTable.VoTableException))]
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

        [TestMethod]
        public void ReadVOTableNullsTest()
        {
            var testfile = @"tap_votable\votable_nulls.xml";
            var gt = new object[][]
            {
                new object[] { "twomass", "edgeew", "Distance from the source to the nearest East or West scan edge", "deg", "pos;arith.diff", "", "REAL", -1, 0, 0,  0 }
            };

            VoTableReaderTestHelper(testfile, 34, 1, gt);
            VoTableInterruptReaderTestHelper(testfile);
        }

        [TestMethod]
        public void ReadVOTableBinaryTest()
        {
            var testfile = @"tap_votable\votable_binary.xml";
            var gt = new object[][]
            {
                new object[] { 122.7034865286146, 0.91527890151003144 }
            };

            VoTableReaderTestHelper(testfile, 10, 1, gt);
            VoTableInterruptReaderTestHelper(testfile);
        }

        [TestMethod]
        public void ReadVOTableBinary2Test()
        {
            var testfile = @"tap_votable\votable_binary2.xml";
            var gt = new object[][]
            {
                new object[] { 122.7034865286146, 0.91527890151003144 }
            };

            VoTableReaderTestHelper(testfile, 10, 1, gt);
            VoTableInterruptReaderTestHelper(testfile);
        }

        [TestMethod]
        public void ReadVOTableBinary2NullsTest()
        {
            var testfile = @"tap_votable\votable_binary2_nulls.xml";
            var gt = new object[][]
            {
                new object[] { "twomass", "edgeew", "Distance from the source to the nearest East or West scan edge", "deg", "pos;arith.diff", DBNull.Value, "REAL", -1, 0, 0,  0 }
            };

            VoTableReaderTestHelper(testfile, 34, 1, gt);
            VoTableInterruptReaderTestHelper(testfile);
        }
    }
}
