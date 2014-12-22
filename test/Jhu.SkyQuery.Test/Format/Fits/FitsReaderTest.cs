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
        private FitsFileWrapper OpenFits(string path)
        {
            var f = new FitsFileWrapper(
                new Uri(String.Format("../../../../../skyquery/test/files/{0}", path), UriKind.Relative),
                DataFileMode.Read
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
                Assert.AreEqual(4, dr.RecordCount);
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
