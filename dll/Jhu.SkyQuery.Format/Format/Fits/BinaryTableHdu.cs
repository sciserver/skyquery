using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Jhu.SkyQuery.Format.Fits
{
    public class BinaryTableHdu : HduBase, ICloneable
    {
        private delegate int ByteReaderDelegate(BitConverterBase converter, byte[] buffer, int startIndex, int count, out object value);

        #region Private member variables

        [NonSerialized]
        private ByteReaderDelegate[] columnByteReaders;

        /// <summary>
        /// Collection of table columns
        /// </summary>
        [NonSerialized]
        private List<FitsTableColumn> columns;

        #endregion
        #region Properties

        /// <summary>
        /// Gets the collection containing columns of the data file
        /// </summary>
        [IgnoreDataMember]
        public List<FitsTableColumn> Columns
        {
            get { return columns; }
        }

        #endregion
        #region Keyword accessor properties

        public int ColumnCount
        {
            get { return Cards[Constants.FitsKeywordTFields].GetInt32(); }
        }

        public int HeapOffset
        {
            get { return Cards[Constants.FitsKeywordTHeap].GetInt32(); }
        }

        public int GroupCount
        {
            get { return Cards[Constants.FitsKeywordGCount].GetInt32(); }
        }

        public int ParameterCount
        {
            get { return Cards[Constants.FitsKeywordPCount].GetInt32(); }
        }

        #endregion
        #region Constructors and initializers

        internal BinaryTableHdu(FitsFile fits)
            : base(fits)
        {
            InitializeMembers();
        }

        internal BinaryTableHdu(HduBase hdu)
            : base(hdu)
        {
            InitializeMembers();

            DetectColumns();
        }

        private BinaryTableHdu(BinaryTableHdu old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.columnByteReaders = null;
            this.columns = new List<FitsTableColumn>();
        }

        private void CopyMembers(BinaryTableHdu old)
        {
            this.columnByteReaders = null;
            this.columns = new List<FitsTableColumn>();
            foreach (var columns in old.columns)
            {
                this.columns.Add((FitsTableColumn)columns.Clone());
            }
        }

        public object Clone()
        {
            return new BinaryTableHdu(this);
        }

        #endregion
        #region Column functions

        private void DetectColumns()
        {
            columns.Clear();

            // Loop though header cards and 
            for (int i = 0; i < ColumnCount; i++)
            {
                // FITS column indexes are 1-based!
                columns.Add(FitsTableColumn.CreateFromHeader(this, i + 1));
            }

            InitializeColumnByteReaders();

            // Verify size
            if (GetAxisLength(0) != GetStrideLength())
            {
                throw new Exception();  // *** TODO
            }
        }

        private void InitializeColumnByteReaders()
        {
            columnByteReaders = new ByteReaderDelegate[Columns.Count];

            for (int i = 0; i < columnByteReaders.Length; i++)
            {
                columnByteReaders[i] = GetByteReaderDelegate(Columns[i]);
            }
        }

        public override int GetStrideLength()
        {
            int res = 0;

            for (int i = 0; i < columns.Count; i++)
            {
                res += columns[i].DataType.TotalBytes;
            }

            return res;
        }

        #endregion

        public bool ReadNextRow(object[] values)
        {
            if (HasMoreStrides)
            {
                ReadStride();

                int startIndex = 0;
                for (int i = 0; i < Columns.Count; i++)
                {
                    var res = columnByteReaders[i](
                                Fits.BitConverter,
                                StrideBuffer,
                                startIndex,
                                columns[i].DataType.TotalBytes,
                                out values[i]);

                    startIndex += res;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a delegate to read the specific column into a boxed 
        /// strongly typed variable
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private ByteReaderDelegate GetByteReaderDelegate(FitsTableColumn column)
        {
            // Complex types firts, then scalar and arrays
            if (column.DataType.Type == typeof(String))
            {
                return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                {
                    value = Encoding.ASCII.GetString(bytes, startIndex, count);
                    return count;
                };
            }
            else if (column.DataType.Repeat == 1)
            {
                // Scalars
                if (column.DataType.Type == typeof(Boolean))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Boolean val;
                        var res = converter.ToBoolean(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(SByte))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        SByte val;
                        var res = converter.ToSByte(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Byte))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Byte val;
                        var res = converter.ToByte(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Char))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Char val;
                        var res = converter.ToChar(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Int16))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Int16 val;
                        var res = converter.ToInt16(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(UInt16))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        UInt16 val;
                        var res = converter.ToUInt16(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Int32))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Int32 val;
                        var res = converter.ToInt32(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(UInt32))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        UInt32 val;
                        var res = converter.ToUInt32(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Int64))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Int64 val;
                        var res = converter.ToInt64(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(UInt64))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        UInt64 val;
                        var res = converter.ToUInt64(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Single))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Single val;
                        var res = converter.ToSingle(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Double))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Double val;
                        var res = converter.ToDouble(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(SingleComplex))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        SingleComplex val;
                        var res = converter.ToSingleComplex(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(DoubleComplex))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        DoubleComplex val;
                        var res = converter.ToDoubleComplex(bytes, startIndex, out val);
                        value = val;
                        return res;
                    };
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                // Arrays
                if (column.DataType.Type == typeof(Boolean))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Boolean[] val;
                        var res = converter.ToBoolean(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(SByte))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        SByte[] val;
                        var res = converter.ToSByte(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Byte))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Byte[] val;
                        var res = converter.ToByte(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Char))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Char[] val;
                        var res = converter.ToChar(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Int16))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Int16[] val;
                        var res = converter.ToInt16(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(UInt16))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        UInt16[] val;
                        var res = converter.ToUInt16(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Int32))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Int32[] val;
                        var res = converter.ToInt32(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(UInt32))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        UInt32[] val;
                        var res = converter.ToUInt32(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Int64))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Int64[] val;
                        var res = converter.ToInt64(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(UInt64))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        UInt64[] val;
                        var res = converter.ToUInt64(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Single))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Single[] val;
                        var res = converter.ToSingle(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(Double))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        Double[] val;
                        var res = converter.ToDouble(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(SingleComplex))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        SingleComplex[] val;
                        var res = converter.ToSingleComplex(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else if (column.DataType.Type == typeof(DoubleComplex))
                {
                    return delegate(BitConverterBase converter, byte[] bytes, int startIndex, int count, out object value)
                    {
                        DoubleComplex[] val;
                        var res = converter.ToDoubleComplex(bytes, startIndex, count, out val);
                        value = val;
                        return res;
                    };
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
