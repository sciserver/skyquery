using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.SharpFitsIO;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Data;
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
        #region Column conversion from FITS to database

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
            // TODO: what to do with varchar(max) etc?
            var dt = DataType.Create(column.DataType.Type, column.DataType.Repeat);

            dt.IsNullable = column.DataType.IsNullable;

            return dt;
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
                //Summary TODO: figure out how to get comment from table column
            };

            if (!String.IsNullOrWhiteSpace(column.Unit))
            {
                // *** TODO: user unit converter function!
                metadata.Unit = Unit.Parse(column.Unit);
            }

            return metadata;
        }

        #endregion
        #region Column conversion from database to FITS

        /// <summary>
        /// Returns a type mapping from .Net types to FITS types
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private TypeMapping CreateTypeMappingToFits(Column column)
        {
            if (column.DataType.Type == typeof(Decimal))
            {
                return new TypeMapping()
                {
                    From = typeof(Decimal),
                    To = typeof(Double),
                    Mapping = delegate(object value)
                    {
                        return (Double)(Decimal)value;
                    }
                };
            }
            else if (column.DataType.Type == typeof(DateTime))
            {
                return new TypeMapping()
                {
                    From = typeof(DateTime),
                    To = typeof(Int64),
                    Mapping = delegate(object value)
                    {
                        return ((DateTime)value).ToFileTime();
                    }
                };
            }
            else if (column.DataType.Type == typeof(DateTimeOffset))
            {
                return new TypeMapping()
                {
                    From = typeof(Decimal),
                    To = typeof(Int64),
                    Mapping = delegate(object value)
                    {
                        return ((DateTimeOffset)value).ToFileTime();
                    }
                };
            }
            else if (column.DataType.Type == typeof(TimeSpan))
            {
                return new TypeMapping()
                {
                    From = typeof(TimeSpan),
                    To = typeof(Double),
                    Mapping = delegate(object value)
                    {
                        return ((TimeSpan)value).TotalMilliseconds;
                    }
                };
            }
            else if (column.DataType.Type == typeof(Guid))
            {
                return new TypeMapping()
                {
                    From = typeof(Guid),
                    To = typeof(Byte[]),
                    Mapping = delegate(object value)
                    {
                        return ((Guid)value).ToByteArray();
                    }
                };
            }
            else if (column.DataType.Type == typeof(Jhu.Graywulf.Schema.SingleComplex))
            {
                return new TypeMapping()
                {
                    From = typeof(Jhu.Graywulf.Schema.SingleComplex),
                    To = typeof(Jhu.SharpFitsIO.SingleComplex),
                    Mapping = delegate(object value)
                    {
                        var s = (Jhu.Graywulf.Schema.SingleComplex)value;
                        return new Jhu.SharpFitsIO.SingleComplex(s.A, s.B);
                    }
                };
            }
            else if (column.DataType.Type == typeof(Jhu.Graywulf.Schema.DoubleComplex))
            {
                return new TypeMapping()
                {
                    From = typeof(Jhu.Graywulf.Schema.DoubleComplex),
                    To = typeof(Jhu.SharpFitsIO.DoubleComplex),
                    Mapping = delegate(object value)
                    {
                        var s = (Jhu.Graywulf.Schema.DoubleComplex)value;
                        return new Jhu.SharpFitsIO.DoubleComplex(s.A, s.B);
                    }
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts database columns to FITS columns
        /// </summary>
        private void ConvertColumnsToFits()
        {
            TypeMapping mapping;
            var columns = new FitsTableColumn[this.Columns.Count];

            for (int i = 0; i < this.Columns.Count; i++)
            {
                ConvertColumnToFits(this.Columns[i], out columns[i], out mapping);
                this.ColumnTypeMappings[i] = mapping;
            }

            hdu.CreateColumns(columns);
        }

        /// <summary>
        /// Converts a database column type to FITS data type
        /// </summary>
        /// <param name="databaseColumn"></param>
        /// <returns></returns>
        private void ConvertColumnToFits(Column databaseColumn, out FitsTableColumn fitsColumn, out TypeMapping typeMapping)
        {
            FitsDataType fitstype;

            ConvertDataTypeToFits(databaseColumn, out fitstype, out typeMapping);

            fitsColumn = FitsTableColumn.Create(databaseColumn.Name, fitstype);

            fitsColumn.Name = databaseColumn.Name;
            fitsColumn.Unit = databaseColumn.Metadata.Unit.ToString();
            // *** TODO: use unit converter function
            //fitsColumn.Format          // TODO: convert formats
        }

        /// <summary>
        /// Converts database columns to fits columns
        /// </summary>
        /// <param name="databaseColumn"></param>
        /// <returns></returns>
        private void ConvertDataTypeToFits(Column databaseColumn, out FitsDataType fitsType, out TypeMapping typeMapping)
        {
            // Fits get type mapping
            typeMapping = CreateTypeMappingToFits(databaseColumn);

            Type type;
            int repeat = 1;

            if (typeMapping == null)
            {
                type = databaseColumn.DataType.Type;
            }
            else
            {
                type = typeMapping.To;
            }

            if (type.IsArray)
            {
                type = type.GetElementType();
                repeat = databaseColumn.DataType.ByteSize;    
            }

            // TODO: this logic needs to be extended if arrays are supported
            if (databaseColumn.DataType.IsMaxLength)
            {
                // This cannot be exported, so limit to a few characters or bytes
                repeat = 128;   // *** TODO
            }
            else if (databaseColumn.DataType.HasLength)
            {
                repeat = databaseColumn.DataType.Length;
            }

            fitsType = FitsDataType.Create(type, repeat, databaseColumn.DataType.IsNullable);
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
                default:
                    throw new NotImplementedException();
            }
        }

        protected override void OnReadHeader()
        {
            ConvertColumnsToDatabase();

            RecordCount = hdu.GetAxisLength(2);
        }

        protected override void OnSetMetadata(int blockCounter)
        {
            base.OnSetMetadata(blockCounter);

            Name = hdu.ExtensionName;
            // TODO: Metadata = ...
        }

        protected override bool OnReadNextRow(object[] values)
        {
            return this.hdu.ReadNextRow(values, File.GenerateIdentityColumn ? 1 : 0);
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
            hdu.MarkEnd();
        }
    }
}
