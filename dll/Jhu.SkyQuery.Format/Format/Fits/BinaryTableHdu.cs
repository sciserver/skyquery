using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Jhu.Graywulf.Types;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.Fits
{
    public class BinaryTableHdu : HduBase
    {
        private static readonly Regex FormatRegex = new Regex(@"([0-9]*)([LXBIJAEDCMP]+)");

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

        private void InitializeMembers()
        {
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

        public override int GetStrideLength()
        {
            int res = 0;

            for (int i = 0; i < Columns.Count; i++)
            {

                res += Columns[i].DataType.GetBinarySize();
            }

            return res;
        }

        private void DetectColumns()
        {
            // Loop though header cards and 
            Card card;
            var cols = new DataFileColumn[FieldCount];

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
                cols[i] = new DataFileColumn(String.Format("Col_{0}", i + 1), dt);
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

#if false
        private void ProcessField(Card card)
        {
            int index = GetFieldIndex(card);
            fields[index].ReadFromCard(card);
        }

        private int GetFieldIndex(Card card)
        {
            return int.Parse(card.Keyword.Substring(5)) - 1;
        }
#endif

        protected override bool OnReadNextRow(object[] values)
        {
            if (HasMoreStrides)
            {
                ReadStride();

                int startIndex = 0;
                for (int i = 0; i < Columns.Count; i++)
                {
                    var res = Columns[i].DataType.ReadFromBytes(
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

    }
}
