using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SharpFitsIO;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.Fits
{
    /// <summary>
    /// This class wraps a FITS file binary table into an interface compatible with
    /// the Graywulf IO and Format libraries.
    /// </summary>
    /// <remarks>
    /// FITS files do not support complete streaming during write because the
    /// binary table header must contain the number of rows.
    /// </remarks>
    [Serializable]
    public class FitsBinaryTableWrapper : DataFileBlockBase, ICloneable
    {
        private BinaryTableHdu hdu;

        /// <summary>
        /// Gets a reference to the underlying FITS file wrapper.
        /// </summary>
        private FitsFileWrapper File
        {
            get { return (FitsFileWrapper)file; }
        }

        /// <summary>
        /// Gets a reference to the underlying FITS file object.
        /// </summary>
        private FitsFile Fits
        {
            get { return ((FitsFileWrapper)file).Fits; }
        }

        #region Constructors and initializers

        public FitsBinaryTableWrapper(FitsFileWrapper file, BinaryTableHdu hdu)
            : base(file)
        {
            InitializeMembers();

            this.hdu = hdu;
        }

        public FitsBinaryTableWrapper(FitsBinaryTableWrapper old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.hdu = null;
        }

        private void CopyMembers(FitsBinaryTableWrapper old)
        {
            this.hdu = (BinaryTableHdu)old.hdu.Clone();
        }

        public override object Clone()
        {
            return new FitsBinaryTableWrapper(this);
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
            var columns = new FitsTableColumn[this.Columns.Count];

            for (int i = 0; i < this.Columns.Count; i++)
            {
                columns[i] = ConvertColumnToFits(Columns[i]);
            }

            hdu.CreateColumns(columns);
        }

        /// <summary>
        /// Converts a database column type to FITS data type
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private FitsTableColumn ConvertColumnToFits(Column column)
        {
            var c = FitsTableColumn.Create(column.Name, ConvertDataTypeToFits(column));

            c.Name = column.Name;
            c.Unit = column.Metadata.Unit;
            //c.Format          // TODO: convert formats

            return c;
        }

        /// <summary>
        /// Converts data
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private FitsDataType ConvertDataTypeToFits(Column column)
        {
            /* TODO: delete if works
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

            return dt;*/

            // TODO: fix this to work with any data type

            Type type;
            int repeat;

            // Match data types based on .Net equivalent
            type = column.DataType.Type;

            // TODO: this logic needs to be extended if arrays are supported
            if (column.DataType.HasLength)
            {
                repeat = column.DataType.Length;
            }
            else
            {
                repeat = 1;
            }

            return FitsDataType.Create(type, repeat, column.DataType.IsNullable);
        }

        #endregion

        protected override void OnColumnsCreated()
        {
            // Nothing to do here in read mode
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

            Name = hdu.ExtensionName;
            // TODO: Metadata = ...
            RecordCount = hdu.GetAxisLength(2);
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
            // HDUs skip to the end automatically
            // TODO: this needs to be tested by partially reading the file!
        }

        protected override void OnWriteHeader()
        {
            if (RecordCount > int.MaxValue)
            {
                throw new FitsException("Too big file");    //**** TODO
            }

            hdu.SetAxisLength(2, (int)RecordCount);

            // TODO: what else to be set here before writing the header?

            hdu.WriteHeader();
        }

        protected override void OnWriteNextRow(object[] values)
        {
            // Replace DBNulls with object nulls, as required by the FITS library
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == DBNull.Value)
                {
                    values[i] = null;
                }
            }

            hdu.WriteNextRow(values);
        }

        protected override void OnWriteFooter()
        {
            // FITS HDUs have no footers
        }
    }
}
