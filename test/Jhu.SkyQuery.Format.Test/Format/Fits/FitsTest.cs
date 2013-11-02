using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jhu.Graywulf.Format;
using Jhu.SkyQuery.Format.Fits;

namespace Jhu.SkyQuery.Format.Fits.Test
{
    [TestClass]
    public class FitsTest
    {
        private Fits OpenFits(string path)
        {
            var f = new Fits(
                new Uri(String.Format("../../../test/files/{0}", path), UriKind.Relative),
                DataFileMode.Read
                );

            return f;
        }

        [TestMethod]
        public void ReadFitsTest()
        {
            var f = OpenFits("sdssdr7_fpC.fit.gz");

            f.Close();
        }

        [TestMethod]
        public void ReadSdssDR7ImageTest()
        {
            var f = OpenFits("sdssdr7_fpC.fit.gz");

            var img = (ImageHdu)f.ReadNextBlock();

            Assert.AreEqual(2, img.AxisCount);
            Assert.AreEqual(2048, img.GetAxisLength(0));
            Assert.AreEqual(1489, img.GetAxisLength(1));

            while (img.HasMoreStrides)
            {
                var row = img.ReadStrideInt16();
            }

            int q = 0;
            HduBase hdu;
            while ((hdu = (HduBase)f.ReadNextBlock()) != null)
            {
                q++;
            }

            Assert.AreEqual(0, q);

            f.Close();
        }

        [TestMethod]
        public void ReadSdssDR7SpectrumTest()
        {
            var f = OpenFits("sdssdr7_spSpec.fit");

            var img = (ImageHdu)f.ReadNextBlock();

            Assert.AreEqual(2, img.AxisCount);
            Assert.AreEqual(3857, img.GetAxisLength(0));
            Assert.AreEqual(5, img.GetAxisLength(1));

            while (img.HasMoreStrides)
            {
                var row = img.ReadStrideSingle();
            }

            var tab = (BinaryTableHdu)f.ReadNextBlock();

            var values = new object[tab.Columns.Count];

            while (tab.HasMoreStrides)
            {
                tab.ReadRowValues(values);
            }

            tab = (BinaryTableHdu)f.ReadNextBlock();

            values = new object[tab.Columns.Count];

            while (tab.HasMoreStrides)
            {
                tab.ReadRowValues(values);
            }

            //
            HduBase hdu;
            while ((hdu = (HduBase)f.ReadNextBlock()) != null)
            {
            }

            f.Close();
        }

        [TestMethod]
        public void ReadSdssDR7SpectrumTest2()
        {
            var f = OpenFits("sdssdr7_spSpec.fit");

            HduBase hdu;
            while ((hdu = (HduBase)f.ReadNextBlock()) != null)
            {
            }

            f.Close();
        }

    }
}
