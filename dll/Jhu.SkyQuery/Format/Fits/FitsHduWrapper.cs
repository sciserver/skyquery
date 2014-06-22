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
        
        private void DetectColumns()
        {
            var cols = new Column[hdu.Columns.Count];

            for (int i = 0; i < cols.Length; i++)
            {
                var col = new Column()
                {
                    Name = hdu.Columns[i].Name,
                    DataType = GetFitsDataType(hdu.Columns[i]),
                    Metadata = GetFitsMetadata(hdu.Columns[i]),
                };

                cols[i] = col;
            }

            CreateColumns(cols);
        }

        /// <summary>
        /// Converts a FITS data type to Graywulf data type
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private DataType GetFitsDataType(FitsTableColumn column)
        {
            // TODO: needs testing
            return DataType.Create(column.DataType.Type, column.DataType.Repeat);
        }

        private VariableMetadata GetFitsMetadata(FitsTableColumn column)
        {
            var metadata = new VariableMetadata()
            {
                Format = column.Format,
                Unit = column.Unit,
                //Summary TODO: figure out how to get comment from table column
            };

            return metadata;
        }

        #endregion

        protected override void OnColumnsCreated()
        {
            // Nothing to do here in read mode
            // TODO: create columns in write mode
        }

        protected override void OnReadHeader()
        {
            DetectColumns();
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
            throw new NotImplementedException();
        }

        protected override void OnWriteNextRow(object[] values)
        {
            throw new NotImplementedException();
        }

        protected override void OnWriteFooter()
        {
            throw new NotImplementedException();
        }
    }
}
