using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Format.Fits
{
    abstract class BitConverterBase
    {
        public abstract bool IsLittleEndian { get; }

        unsafe protected abstract void PutBytes(byte* dst, byte[] src, int startIndex, int count);

        unsafe protected abstract void GetBytes(byte* src, byte[] dst, int startIndex, int count);

        #region Boolean

        public Boolean ToBoolean(byte[] bytes, int startIndex)
        {
            return bytes[startIndex] != 0;
        }

        public int ToBoolean(byte[] bytes, int startIndex, out Boolean value)
        {
            value = ToBoolean(bytes, startIndex);
            return 1;
        }

        public int ToBoolean(byte[] bytes, int startIndex, int count, out Boolean[] values)
        {
            values = new Boolean[count];

            for (int i = 0; i < count; i++)
            {
                ToBoolean(bytes, startIndex + i, out values[i]);
            }

            return count;
        }

        #endregion
        #region Byte

        public Byte ToByte(byte[] bytes, int startIndex)
        {
            return bytes[startIndex];
        }

        public int ToByte(byte[] bytes, int startIndex, out Byte value)
        {
            value = ToByte(bytes, startIndex);
            return 1;
        }

        public int ToByte(byte[] bytes, int startIndex, int count, out byte[] values)
        {
            values = new byte[count];

            Array.Copy(bytes, startIndex, values, 0, count);

            return count;
        }

        #endregion
        #region SByte

        unsafe public SByte ToSByte(byte[] bytes, int startIndex)
        {
            SByte res;
            PutBytes((byte*)&res, bytes, startIndex, 1);
            return res;
        }

        public int ToSByte(byte[] bytes, int startIndex, out SByte value)
        {
            value = ToSByte(bytes, startIndex);
            return 1;
        }

        public int ToSByte(byte[] bytes, int startIndex, int count, out SByte[] values)
        {
            values = new SByte[count];

            for (int i = 0; i < count; i++)
            {
                ToSByte(bytes, startIndex + i, out values[i]);
            }

            return count;
        }

        #endregion
        #region Char

        public Char ToChar(byte[] bytes, int startIndex)
        {
            return Convert.ToChar(bytes[startIndex]);
        }

        public int ToChar(byte[] bytes, int startIndex, out Char value)
        {
            value = ToChar(bytes, startIndex);
            return 1;
        }

        public int ToChar(byte[] bytes, int startIndex, int count, out Char[] values)
        {
            values = ASCIIEncoding.ASCII.GetChars(bytes, startIndex, count);
            return count;
        }

        #endregion
        #region Int16

        unsafe public Int16 ToInt16(byte[] bytes, int startIndex)
        {
            Int16 res;
            PutBytes((byte*)&res, bytes, startIndex, 2);
            return res;
        }

        unsafe public int ToInt16(byte[] bytes, int startIndex, out Int16 value)
        {
            value = ToInt16(bytes, startIndex);
            return 2;
        }

        public int ToInt16(byte[] bytes, int startIndex, int count, out Int16[] values)
        {
            values = new Int16[count];

            for (int i = 0; i < count; i++)
            {
                ToInt16(bytes, startIndex + i * 2, out values[i]);
            }

            return count * 2;
        }

        #endregion
        #region UInt16

        unsafe public UInt16 ToUInt16(byte[] bytes, int startIndex)
        {
            UInt16 res;
            PutBytes((byte*)&res, bytes, startIndex, 2);
            return res;
        }

        public int ToUInt16(byte[] bytes, int startIndex, out UInt16 value)
        {
            value = ToUInt16(bytes, startIndex);
            return 2;
        }

        public int ToUInt16(byte[] bytes, int startIndex, int count, out UInt16[] values)
        {
            values = new UInt16[count];

            for (int i = 0; i < count; i++)
            {
                ToUInt16(bytes, startIndex + i * 2, out values[i]);
            }

            return count * 2;
        }

        #endregion
        #region Int32

        unsafe public Int32 ToInt32(byte[] bytes, int startIndex)
        {
            Int32 res;
            PutBytes((byte*)&res, bytes, startIndex, 4);
            return res;
        }

        public int ToInt32(byte[] bytes, int startIndex, out Int32 value)
        {
            value = ToInt32(bytes, startIndex);
            return 4;
        }

        public int ToInt32(byte[] bytes, int startIndex, int count, out Int32[] values)
        {
            values = new Int32[count];

            for (int i = 0; i < count; i++)
            {
                ToInt32(bytes, startIndex + i * 4, out values[i]);
            }

            return count * 4;
        }

        #endregion
        #region UInt32

        unsafe public UInt32 ToUInt32(byte[] bytes, int startIndex)
        {
            UInt32 res;
            PutBytes((byte*)&res, bytes, startIndex, 4);
            return res;
        }

        public int ToUInt32(byte[] bytes, int startIndex, out UInt32 value)
        {
            value = ToUInt32(bytes, startIndex);
            return 4;
        }

        public int ToUInt32(byte[] bytes, int startIndex, int count, out UInt32[] values)
        {
            values = new UInt32[count];

            for (int i = 0; i < count; i++)
            {
                ToUInt32(bytes, startIndex + i * 4, out values[i]);
            }

            return count * 4;
        }

        #endregion
        #region Int64

        unsafe public Int64 ToInt64(byte[] bytes, int startIndex)
        {
            Int64 res;
            PutBytes((byte*)&res, bytes, startIndex, 8);
            return res;
        }

        public int ToInt64(byte[] bytes, int startIndex, out Int64 value)
        {
            value = ToInt64(bytes, startIndex);
            return 8;
        }

        public int ToInt64(byte[] bytes, int startIndex, int count, out Int64[] values)
        {
            values = new Int64[count];

            for (int i = 0; i < count; i++)
            {
                ToInt64(bytes, startIndex + i * 8, out values[i]);
            }

            return count * 8;
        }

        #endregion
        #region UInt64

        unsafe public UInt64 ToUInt64(byte[] bytes, int startIndex)
        {
            UInt64 res;
            PutBytes((byte*)&res, bytes, startIndex, 8);
            return res;
        }

        public int ToUInt64(byte[] bytes, int startIndex, out UInt64 value)
        {
            value = ToUInt64(bytes, startIndex);
            return 8;
        }

        public int ToUInt64(byte[] bytes, int startIndex, int count, out UInt64[] values)
        {
            values = new UInt64[count];

            for (int i = 0; i < count; i++)
            {
                ToUInt64(bytes, startIndex + i * 8, out values[i]);
            }

            return count * 8;
        }

        #endregion
        #region Single

        unsafe public Single ToSingle(byte[] bytes, int startIndex)
        {
            Single res;
            PutBytes((byte*)&res, bytes, startIndex, 4);
            return res;
        }

        public int ToSingle(byte[] bytes, int startIndex, out Single value)
        {
            value = ToSingle(bytes, startIndex);
            return 4;
        }

        public int ToSingle(byte[] bytes, int startIndex, int count, out Single[] values)
        {
            values = new Single[count];

            for (int i = 0; i < count; i++)
            {
                ToSingle(bytes, startIndex + i * 4, out values[i]);
            }

            return count * 4;
        }

        #endregion
        #region Double

        unsafe public Double ToDouble(byte[] bytes, int startIndex)
        {
            Double res;
            PutBytes((byte*)&res, bytes, startIndex, 8);
            return res;
        }

        unsafe public int ToDouble(byte[] bytes, int startIndex, out Double value)
        {
            value = ToDouble(bytes, startIndex);
            return 8;
        }

        public int ToDouble(byte[] bytes, int startIndex, int count, out Double[] values)
        {
            values = new Double[count];

            for (int i = 0; i < count; i++)
            {
                ToDouble(bytes, startIndex + i * 8, out values[i]);
            }

            return count * 8;
        }

        #endregion
        #region SingleComplex

        unsafe public SingleComplex ToSingleComplex(byte[] bytes, int startIndex)
        {
            SingleComplex res;
            PutBytes((byte*)&res, bytes, startIndex, 8);
            return res;
        }

        public int ToSingleComplex(byte[] bytes, int startIndex, out SingleComplex value)
        {
            value = ToSingleComplex(bytes, startIndex);
            return 8;
        }

        public int ToSingleComplex(byte[] bytes, int startIndex, int count, out SingleComplex[] values)
        {
            values = new SingleComplex[count];

            for (int i = 0; i < count; i++)
            {
                ToSingleComplex(bytes, startIndex + i * 8, out values[i]);
            }

            return count * 8;
        }

        #endregion
        #region DoubleComplex

        unsafe public DoubleComplex ToDoubleComplex(byte[] bytes, int startIndex)
        {
            DoubleComplex res;
            PutBytes((byte*)&res, bytes, startIndex, 16);
            return res;
        }

        public int ToDoubleComplex(byte[] bytes, int startIndex, out DoubleComplex value)
        {
            value = ToDoubleComplex(bytes, startIndex);
            return 16;
        }

        public int ToDoubleComplex(byte[] bytes, int startIndex, int count, out DoubleComplex[] values)
        {
            values = new DoubleComplex[count];

            for (int i = 0; i < count; i++)
            {
                ToDoubleComplex(bytes, startIndex + i * 16, out values[i]);
            }

            return count * 16;
        }

        #endregion

        // ---

        unsafe public void GetBytes(Boolean value, byte[] bytes, int startIndex)
        {
            bytes[startIndex] = value ? (Byte)0x01 : (Byte)0x00;
        }

        unsafe public void GetBytes(SByte value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 1);
        }

        public void GetBytes(Char value, byte[] bytes, int startIndex)
        {
            throw new NotImplementedException();
        }

        unsafe public void GetBytes(Int16 value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 2);
        }

        unsafe public void GetBytes(UInt16 value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 2);
        }

        unsafe public void GetBytes(Int32 value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 4);
        }

        unsafe public void GetBytes(UInt32 value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 4);
        }

        unsafe public void GetBytes(Int64 value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 8);
        }

        unsafe public void GetBytes(UInt64 value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 8);
        }

        unsafe public void GetBytes(Single value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 4);
        }

        unsafe public void GetBytes(Double value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 8);
        }

        unsafe public void GetBytes(SingleComplex value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 8);
        }

        unsafe public void GetBytes(DoubleComplex value, byte[] bytes, int startIndex)
        {
            GetBytes((byte*)&value, bytes, startIndex, 16);
        }
    }
}
