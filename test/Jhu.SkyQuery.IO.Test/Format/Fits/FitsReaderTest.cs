using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.Fits;

namespace Jhu.SkyQuery.Format.Fits.Test
{
    [TestClass]
    public class FitsReaderTest
    {
        private Fits OpenFits(string path)
        {
            var f = new Fits(
                new Uri(String.Format("../../../test/files/{0}", path), UriKind.Relative),
                DataFileMode.Read,
                Graywulf.Types.Endianness.BigEndian
                );

            return f;
        }

        [TestMethod]
        public void ReadSdssDR7SpectrumTest()
        {
            var f = OpenFits("sdssdr7_spSpec.fit");
            var cmd = new FileCommand(f);

            using (var dr = cmd.ExecuteReader())
            {
                Assert.AreEqual(0, dr.FieldCount);

                dr.NextResult();

                Assert.AreEqual(23, dr.FieldCount);

                var values = new object[dr.FieldCount];
                int q = 0;
                while (dr.Read())
                {
                    dr.GetValues(values);
                    q++;
                }

                Assert.AreEqual(4, q);
            }
            

            f.Close();
        }

    }
}
