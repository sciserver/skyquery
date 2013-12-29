using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Format.Fits
{
    public class ImageHdu : HduBase, ICloneable
    {
        internal ImageHdu(Fits fits)
            : base(fits)
        {
            InitializeMembers();
        }

        internal ImageHdu(HduBase hdu)
            :base(hdu)
        {
            InitializeMembers();
        }

        private ImageHdu(ImageHdu old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
        }

        private void CopyMembers(ImageHdu old)
        {
        }

        public override object Clone()
        {
            return new ImageHdu(this);
        }

        #region Column functions

        protected override void OnColumnsCreated()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Read functions

        protected override bool OnReadNextRow(object[] values)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Write functions

        protected override void OnWriteHeader()
        {
            throw new NotImplementedException();
        }

        protected override void OnWriteNextRow(object[] values)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Stride functions

        public Int16[] ReadStrideInt16()
        {
            ReadStride();

            Int16[] values;
            Fits.BitConverter.ToInt16(StrideBuffer, 0, StrideBuffer.Length / 2, out values);
            return values;
        }

        public Int32[] ReadStrideInt32()
        {
            ReadStride();

            Int32[] values;
            Fits.BitConverter.ToInt32(StrideBuffer, 0, StrideBuffer.Length / 4, out values);
            return values;
        }

        public Single[] ReadStrideSingle()
        {
            ReadStride();

            Single[] values;
            Fits.BitConverter.ToSingle(StrideBuffer, 0, StrideBuffer.Length / 4, out values);
            return values;
        }

        public Double[] ReadStrideDouble()
        {
            ReadStride();

            Double[] values;
            Fits.BitConverter.ToDouble(StrideBuffer, 0, StrideBuffer.Length / 8, out values);
            return values;
        }

        #endregion
    }
}
