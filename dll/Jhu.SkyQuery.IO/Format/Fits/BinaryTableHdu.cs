using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.Fits
{
    public class BinaryTableHdu : HduBase, ICloneable
    {
        private static readonly Regex FormatRegex = new Regex(@"([0-9]*)([LXBIJAEDCMP]+)");

        private delegate int ByteReaderDelegate(BitConverterBase converter, byte[] buffer, int startIndex, int count, out object value);

        [NonSerialized]
        private ByteReaderDelegate[] columnByteReaders;

        internal BinaryTableHdu(Fits fits)
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
        }

        private void CopyMembers(BinaryTableHdu old)
        {
        }

        public override object Clone()
        {
            return new BinaryTableHdu(this);
        }

        protected int GroupCount
        {
            get { return Cards[Constants.FitsKeywordGCount].GetInt32(); }
        }

        protected int ParameterCount
        {
            get { return Cards[Constants.FitsKeywordPCount].GetInt32(); }
        }

        protected int FieldCount
        {
            get { return Cards[Constants.FitsKeywordTFields].GetInt32(); }
        }

        #region Column functions

        protected override void OnColumnsCreated()
        {
            if ((file.FileMode & DataFileMode.Read) != 0)
            {
                InitializeColumnByteReader();
            }
        }

        private void InitializeColumnByteReader()
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

            for (int i = 0; i < Columns.Count; i++)
            {
                res += GetColumnBinarySize(Columns[i]);
            }

            return res;
        }

        private void DetectColumns()
        {
            // Loop though header cards and 
            Card card;
            var cols = new Column[FieldCount];

            for (int i = 0; i < cols.Length; i++)
            {
                // Get data type
                if (!TryGetCard(Constants.FitsKeywordTForm, i, out card))
                {
                    throw new FileFormatException();    // *** TODO
                }
                var tform = card.GetString();

                char type;
                int repeat;
                GetFitsDataTypeDetails(tform, out type, out repeat);
                var dt = GetFitsDataType(type, repeat);

                // Create column
                cols[i] = new Column(String.Format("Col_{0}", i + 1), dt);
                cols[i].ID = i;

                // Set optional parameters

                // - Column name
                if (TryGetCard(Constants.FitsKeywordTType, i, out card))
                {
                    cols[i].Name = card.GetString();
                }
                
                // Unit
                if (TryGetCard(Constants.FitsKeywordTUnit, i, out card))
                {
                    cols[i].Metadata.Unit = card.GetString();
                }

                // Null value equivalent
                if (TryGetCard(Constants.FitsKeywordTNull, i, out card))
                {
                    // *** TODO
                    //cols[i].Metadata.Null = card.GetDouble();
                }

                // Scale
                if (TryGetCard(Constants.FitsKeywordTScal, i, out card))
                {
                    // *** TODO
                    //cols[i].Metadata.Scale = card.GetDouble();
                }

                // Zero offset
                if (TryGetCard(Constants.FitsKeywordTZero, i, out card))
                {
                    // *** TODO
                    //cols[i].Metadata.Zero = card.GetDouble();
                }

                // Format
                if (TryGetCard(Constants.FitsKeywordTDisp, i, out card))
                {
                    // TODO: convert to .Net style
                    cols[i].Metadata.Format = card.GetString();
                }
            }

            // -

            // TODO: move to on columns created
            // getValueConverter = CreateGetValueConverterDelegate();

            CreateColumns(cols);

            // Verify size
            if (GetAxisLength(0) != GetStrideLength())
            {
                throw new Exception();  // *** TODO
            }
        }

        private void GetFitsDataTypeDetails(string tform, out char type, out int repeat)
        {
            var m = FormatRegex.Match(tform);

            if (!m.Success)
            {
                throw new Exception();      // *** TODO
            }

            type = m.Groups[2].Value.ToUpper()[0];

            // Item repeated a number of times
            var repeatstr = m.Groups[1].Value;
            if (!String.IsNullOrEmpty(repeatstr))
            {
                repeat = int.Parse(repeatstr, Fits.Culture);
            }
            else
            {
                repeat = 1;
            }
        }

        private DataType GetFitsDataType(char type, int repeat)
        {
            DataType dt;
            switch (type)
            {
                case 'L':
                    dt = DataType.Boolean;
                    break;
                case 'X':
                    dt = DataType.Boolean;           // *** This is union bit!
                    break;
                case 'B':
                    if (repeat == 1)
                    {
                        dt = DataType.Byte;
                    }
                    else
                    {
                        dt = DataType.SqlVarBinary;
                        dt.Length = repeat;
                    }
                    break;
                case 'I':
                    dt = DataType.Int16;
                    break;
                case 'J':
                    dt = DataType.Int32;
                    break;
                case 'A':
                    if (repeat == 1)
                    {
                        dt = DataType.SqlChar;
                    }
                    else
                    {
                        dt = DataType.SqlChar;
                        dt.Length = repeat;
                    }
                    break;
                case 'E':
                    dt = DataType.Single;
                    break;
                case 'D':
                    dt = DataType.Double;
                    break;
                case 'C':
                    dt = DataType.SingleComplex;
                    break;
                case 'M':
                    dt = DataType.DoubleComplex;
                    break;
                case 'P':
                    // Array, not implemented
                default:
                    throw new NotImplementedException();
            }

            if (!dt.HasLength && repeat > 1)
            {
                // Array, not implemented
                throw new NotImplementedException();
            }

            return dt;
        }

        private bool TryGetCard(string key, int id, out Card card)
        {
            key += (id + 1).ToString();
            return Cards.TryGetValue(key, out card);
        }

        #endregion

        protected override bool OnReadNextRow(object[] values)
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
                                Columns[i].DataType.Length,
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

        public bool ReadRowValues(object[] values)
        {
            return OnReadNextRow(values);
        }

        public object[] ReadRowValues()
        {
            var values = new object[Columns.Count];
            if (OnReadNextRow(values))
            {
                return values;
            }
            else
            {
                return null;
            }
        }

        public int GetColumnBinarySize(Column column)
        {
            if (column.DataType.IsMaxLength)
            {
                // varchar(max), varbinary(max) - no known size
                return -1;
            }
            else if (column.DataType.HasLength)
            {
                return column.DataType.Length * column.DataType.ByteSize;
            }
            else if (column.DataType.IsSqlArray)
            {
                return column.DataType.ArrayLength * column.DataType.ByteSize;
            }
            else
            {
                return column.DataType.ByteSize;
            }
        }

        private ByteReaderDelegate GetByteReaderDelegate(Column column)
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
            else if (!column.DataType.HasLength && !column.DataType.IsSqlArray)
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

                // TODO: date time etc.
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

                // TODO: implement, maybe, string
            }
        }
    }
}
