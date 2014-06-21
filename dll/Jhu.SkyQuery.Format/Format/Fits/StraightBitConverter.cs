using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Format.Fits
{
    class StraightBitConverter : BitConverterBase
    {
        public override bool IsLittleEndian
        {
            get { return System.BitConverter.IsLittleEndian; }
        }

        #region Helper functions

        unsafe protected override void PutBytes(byte* dst, byte[] src, int startIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                dst[i] = src[i + startIndex];
            }
        }

        unsafe protected override void GetBytes(byte* src, byte[] dst, int startIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                dst[i + startIndex] = src[i];
            }
        }

        #endregion


    }
}
