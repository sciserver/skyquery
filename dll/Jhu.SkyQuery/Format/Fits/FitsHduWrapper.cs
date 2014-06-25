using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SharpFitsIO;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.Fits
{
    [Serializable]
    public class FitsHduWrapper : DataFileBlockBase, ICloneable
    {
        private BinaryTableHdu hdu;

        private FitsFileWrapper File
        {
            get { return (FitsFileWrapper)file; }
        }

        private FitsFile Fits
        {
            get { return ((FitsFileWrapper)file).Fits; }
        }

        #region Constructors and initializers

        public FitsHduWrapper(FitsFileWrapper file, BinaryTableHdu hdu)
            : base(file)
        {
            InitializeMembers();

            this.hdu = hdu;
        }

        public FitsHduWrapper(FitsHduWrapper old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.hdu = null;
        }

        private void CopyMembers(FitsHduWrapper old)
        {
            this.hdu = (BinaryTableHdu)old.hdu.Clone();
        }

        public override object Clone()
        {
            return new FitsHduWrapper(this);
        }

        #endregion
        #region Column functions
        
        /// <summary>
        /// Converts FITS bintable columns into database columns
        /// </summary>
        private void ConvertColumnsToDatabase()
        {
            var cols = new Column[hdu.Columns.Count];

            for (int i = 0; i < cols.Length; i++)
            {
                var col = new Column()
                {
                    Name = hdu.Columns[i].Name,
                    DataType = ConvertDataTypeToDatabase(hdu.Columns[i]),
                    Metadata = ConvertMetadataToDatabase(hdu.Columns[i]),
                };

                cols[i] = col;
            }

            CreateColumns(cols);
        }

        /// <summary>
        /// Converts a FITS data type to a database data type
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private DataType ConvertDataTypeToDatabase(FitsTableColumn column)
        {
            // TODO: needs testing
            return DataType.Create(column.DataType.Type, column.DataType.Repeat);
        }

        /// <summary>
        /// Converts FITS column metadata into database column metadata
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private VariableMetadata ConvertMetadataToDatabase(FitsTableColumn column)
        {
            var metadata = new VariableMetadata()
            {
                Format = column.Format,
                Unit = column.Unit,
                //Summary TODO: figure out how to get comment from table column
            };

            return metadata;
        }

        /// <summary>
        /// Converts database columns to FITS columns
        /// </summary>
        private void ConvertColumnsToFits()
        {
            for (int i = 0; i < this.Columns.Count; i++)
            {
                hdu.Columns.Add(ConvertColumnToFits(Columns[i]));
            }
        }

        /// <summary>
        /// Converts a database column type to FITS data type
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private FitsTableColumn ConvertColumnToFits(Column column)
        {
            var c = new FitsTableColumn();

            c.Name = column.Name;
            c.Unit = column.Metadata.Unit;
            //c.Format          // TODO: convert formats

            c.DataType = ConvertDataTypeToFits(column);

            return c;
        }

        private FitsDataType ConvertDataTypeToFits(Column column)
        {
            FitsDataType dt;

            if (column.DataType.Type == typeof(Boolean))
            {
                dt = FitsDataTypes.Logical;
            }
            else if (column.DataType.Type == typeof(SByte))
            {
                dt = FitsDataTypes.Byte;
                dt.Zero = SByte.MaxValue;
            }
            else if (column.DataType.Type == typeof(Byte))
            {
                dt = FitsDataTypes.Byte;
            }
            else if (column.DataType.Type == typeof(Int16))
            {
                dt = FitsDataTypes.Int16;
                dt.Zero = Int16.MaxValue;
            }
            else if (column.DataType.Type == typeof(UInt16))
            {
                dt = FitsDataTypes.Int16;
            }
            else if (column.DataType.Type == typeof(Int32))
            {
                dt = FitsDataTypes.Int32;
                dt.Zero = Int32.MaxValue;
            }
            else if (column.DataType.Type == typeof(UInt32))
            {
                dt = FitsDataTypes.Int32;
            }
            else if (column.DataType.Type == typeof(Int64))
            {
                dt = FitsDataTypes.Int64;
                dt.Zero = Int64.MaxValue;
            }
            else if (column.DataType.Type == typeof(UInt64))
            {
                dt = FitsDataTypes.Int64;
            }
            else if (column.DataType.Type == typeof(Single))
            {
                dt = FitsDataTypes.Single;
            }
            else if (column.DataType.Type == typeof(Double))
            {
                dt = FitsDataTypes.Double;
            }
            else if (column.DataType.Type == typeof(Decimal))
            {
                dt = FitsDataTypes.Double;
            }
            else if (column.DataType.Type == typeof(DateTime))
            {
                dt = FitsDataTypes.Int64;
            }
            else if (column.DataType.Type == typeof(Guid))
            {
                dt = FitsDataTypes.Byte;
                dt.Repeat = 16;
            }
            else if (column.DataType.Type == typeof(char))
            {
                dt = FitsDataTypes.Char;
            }
            else if (column.DataType.Type == typeof(char[]))
            {
                dt = FitsDataTypes.Char;
                dt.Repeat = column.DataType.Length;
            }
            else if (column.DataType.Type == typeof(string))
            {
                dt = FitsDataTypes.Char;
                dt.Repeat = column.DataType.Length;
            }
            else if (column.DataType.Type == typeof(byte[]))
            {
                dt = FitsDataTypes.Byte;
                dt.Repeat = column.DataType.Length;
            }
            else
            {
                throw new NotImplementedException();
            }

            dt.NullValue = null;    // TODO
            //dt.Dimensions = // TODO

            return dt;
        }

        #endregion

        protected override void OnColumnsCreated()
        {
            // Nothing to do here in read mode
            // TODO: create columns in write mode

            switch (File.FileMode)
            {
                case Graywulf.IO.DataFileMode.Read:
                    break;
                case Graywulf.IO.DataFileMode.Write:
                    ConvertColumnsToFits();
                    break;
            }
        }

        protected override void OnReadHeader()
        {
            ConvertColumnsToDatabase();
        }

        protected override bool OnReadNextRow(object[] values)
        {
            return this.hdu.ReadNextRow(values);
        }

        protected override void OnReadFooter()
        {
            // No footers in HDUs
        }

        protected override void OnReadToFinish()
        {
            // HDUs can skip to the end
        }

        protected override void OnWriteHeader()
        {
            hdu.SetAxisLength(1, hdu.GetStrideLength());

            hdu.SetColumnCards();

            hdu.WriteHeader();
        }

        protected override void OnWriteNextRow(object[] values)
        {
            //
        }

        protected override void OnWriteFooter()
        {
            Fits.SkipBlock();
        }
    }
}
