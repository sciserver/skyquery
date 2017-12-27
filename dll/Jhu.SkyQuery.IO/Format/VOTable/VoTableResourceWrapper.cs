using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jhu.Graywulf.Schema;
using Jhu.Graywulf.Data;
using Jhu.Graywulf.Format;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Format.VoTable
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
    public class VoTableResourceWrapper : DataFileBlockBase, ICloneable
    {
        private VO.VoTable.VoTableResource resource;

        /// <summary>
        /// Gets a reference to the underlying FITS file wrapper.
        /// </summary>
        private VoTableWrapper File
        {
            get { return (VoTableWrapper)file; }
        }

        /// <summary>
        /// Gets a reference to the underlying FITS file object.
        /// </summary>
        private VO.VoTable.VoTable VoTable
        {
            get { return ((VoTableWrapper)file).VoTable; }
        }

        #region Constructors and initializers

        public VoTableResourceWrapper(VoTableWrapper file, VO.VoTable.VoTableResource resource)
            : base(file)
        {
            InitializeMembers();

            this.resource = resource;
        }

        public VoTableResourceWrapper(VoTableResourceWrapper old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.resource = null;
        }

        private void CopyMembers(VoTableResourceWrapper old)
        {
            this.resource = (VO.VoTable.VoTableResource)old.resource.Clone();
        }

        public override object Clone()
        {
            return new VoTableResourceWrapper(this);
        }

        #endregion
        #region Column conversion from FITS to database

        /// <summary>
        /// Converts FITS bintable columns into database columns
        /// </summary>
        private void ConvertColumnsToDatabase()
        {
            /*var cols = new Column[hdu.Columns.Count];

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
            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts a FITS data type to a database data type
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private DataType ConvertDataTypeToDatabase(VO.VoTable.VoTableColumn column)
        {
            /*
            // TODO: needs testing
            // TODO: what to do with varchar(max) etc?
            var dt = DataType.Create(column.DataType.Type, column.DataType.Repeat);

            dt.IsNullable = column.DataType.IsNullable;

            return dt;*/

            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts FITS column metadata into database column metadata
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private VariableMetadata ConvertMetadataToDatabase(VO.VoTable.VoTableColumn column)
        {
            /*
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
            */

            throw new NotImplementedException();
        }

        #endregion
        #region Column conversion from database to VOTable

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
                    Mapping = delegate (object value)
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
                    Mapping = delegate (object value)
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
                    Mapping = delegate (object value)
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
                    Mapping = delegate (object value)
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
                    Mapping = delegate (object value)
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
                    Mapping = delegate (object value)
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
                    Mapping = delegate (object value)
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
        /// Converts database columns to VOTable columns
        /// </summary>
        private void ConvertColumnsToVoTable()
        {
            TypeMapping mapping;
            var columns = new VO.VoTable.VoTableColumn[this.Columns.Count];

            for (int i = 0; i < this.Columns.Count; i++)
            {
                ConvertColumnToVoTable(this.Columns[i], out columns[i], out mapping);
                this.ColumnTypeMappings[i] = mapping;
            }

            resource.CreateColumns(columns);
        }

        /// <summary>
        /// Converts a database column type to FITS data type
        /// </summary>
        /// <param name="databaseColumn"></param>
        /// <returns></returns>
        private void ConvertColumnToVoTable(Column databaseColumn, out VO.VoTable.VoTableColumn votableColumn, out TypeMapping typeMapping)
        {
            VO.VoTable.VoTableDataType votabletype;

            ConvertDataTypeToVoTable(databaseColumn, out votabletype, out typeMapping);

            votableColumn = VO.VoTable.VoTableColumn.Create(databaseColumn.ID.ToString(), databaseColumn.Name, votabletype);

            votableColumn.Description = databaseColumn.Metadata.Summary;
            votableColumn.Ucd = databaseColumn.Metadata.Quantity.ToString();
            votableColumn.UType = databaseColumn.Metadata.Class;
            // TODO: use unit converter function
            votableColumn.Unit = databaseColumn.Metadata.Unit.ToString();

            // TODO: create form format string
            // votableColumn.Precision =
            // votableColumn.Width =
        }

        /// <summary>
        /// Converts database columns to VOTable columns
        /// </summary>
        /// <param name="databaseColumn"></param>
        /// <returns></returns>
        private void ConvertDataTypeToVoTable(Column databaseColumn, out VO.VoTable.VoTableDataType votableType, out TypeMapping typeMapping)
        {
            throw new NotImplementedException();

            /*
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

            votableType = VO.VoTable.VoTableDataType.Create(type, repeat, databaseColumn.DataType.IsNullable);
            */
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
                    ConvertColumnsToVoTable();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected override Task OnReadHeaderAsync()
        {
            ConvertColumnsToDatabase();
            // TODO: delete RecordCount = hdu.GetAxisLength(2);
            return Task.CompletedTask;
        }

        protected override void OnSetMetadata(int blockCounter)
        {
            base.OnSetMetadata(blockCounter);

            /// Name = resource.ExtensionName;
            // TODO: Metadata = ...
        }

        protected override Task<bool> OnReadNextRowAsync(object[] values)
        {
            return this.resource.ReadNextRowAsync(values, File.GenerateIdentityColumn ? 1 : 0);
        }

        protected override Task OnReadFooterAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task OnReadToFinishAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task OnWriteHeaderAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task OnWriteNextRowAsync(object[] values)
        {
            // Replace DBNulls with object nulls, as required by the FITS library
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == DBNull.Value)
                {
                    values[i] = null;
                }
            }

            await resource.WriteNextRowAsync(values);
        }

        protected override Task OnWriteFooterAsync()
        {
            throw new NotImplementedException();
        }
    }
}
