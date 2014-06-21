using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SkyQuery.Format.Fits
{
    /// <summary>
    /// Represents a binary table HDU column
    /// </summary>
    [Serializable]
    public class FitsTableColumn : ICloneable
    {
        private int id;
        private string name;
        private FitsDataType dataType;
        private Int32? nullValue;
        private string unit;
        private double? scale;
        private double? zero;
        private string format;
        private string dimensions;

        #region Properties

        /// <summary>
        /// Gets or sets the ID of the columns
        /// </summary>
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <remarks>
        /// Corresponds to the TTYPEn keyword
        /// </remarks>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Get or sets the data type of the column.
        /// </summary>
        /// <remarks>
        /// Corresponds to the TFORMn keyword
        /// </remarks>
        public FitsDataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating null.
        /// </summary>
        /// <remarks>
        /// Corresponds to the TNULLn keyword
        /// </remarks>
        public Int32? NullValue
        {
            get { return nullValue; }
            set { nullValue = value; }
        }

        /// <summary>
        /// Gets or sets the unit of the column.
        /// </summary>
        /// <remarks>
        /// Corresponds to the TUNITn keyword
        /// </remarks>
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        /// <summary>
        /// Gets or sets the scale of the column.
        /// </summary>
        /// <remarks>
        /// Corresponds to the TSCALn keyword
        /// </remarks>
        public double? Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// Gets or sets the zero offset of the column.
        /// </summary>
        /// <remarks>
        /// Corresponds to the TZEROn keyword
        /// </remarks>
        public double? Zero
        {
            get { return zero; }
            set { zero = value; }
        }

        /// <summary>
        /// Gets or sets the display format of the column.
        /// </summary>
        /// <remarks>
        /// Corresponds to the TFORMn keyword
        /// </remarks>
        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        #endregion
        #region Constructors and initializers

        public FitsTableColumn()
        {
            InitializeMembers();
        }

        public FitsTableColumn(FitsTableColumn old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.id = 0;
            this.name = null;
            this.dataType = null;
            this.nullValue = null;
            this.unit = null;
            this.scale = null;
            this.zero = null;
            this.format = null;
            this.dimensions = null;
        }

        private void CopyMembers(FitsTableColumn old)
        {
            this.id = old.id;
            this.name = old.name;
            this.dataType = new FitsDataType(old.dataType);
            this.nullValue = old.nullValue;
            this.unit = old.unit;
            this.scale = old.scale;
            this.zero = old.zero;
            this.format = old.format;
            this.dimensions = old.dimensions;
        }

        public object Clone()
        {
            return new FitsTableColumn(this);
        }

        #endregion

        internal static FitsTableColumn CreateFromHeader(HduBase hdu, int index)
        {
            Card card;

            // Get data type
            if (!hdu.Cards.TryGetValue(Constants.FitsKeywordTForm, index, out card))
            {
                throw new FitsException("Keyword expected but not found:"); // TODO
            }

            var tform = card.GetString();
            var dt = FitsDataType.CreateFromTForm(tform);

            // Create column

            var column = new FitsTableColumn()
            {
                ID = index,
            };

            // Set optional parameters

            // --- Column name
            if (hdu.Cards.TryGetValue(Constants.FitsKeywordTType, index, out card))
            {
                column.Name = card.GetString();
            }

            // Unit
            if (hdu.Cards.TryGetValue(Constants.FitsKeywordTUnit, index, out card))
            {
                column.Unit = card.GetString();
            }

            // Null value equivalent
            if (hdu.Cards.TryGetValue(Constants.FitsKeywordTNull, index, out card))
            {
                column.NullValue = card.GetInt32();
            }

            // Scale
            if (hdu.Cards.TryGetValue(Constants.FitsKeywordTScal, index, out card))
            {
                column.Scale = card.GetDouble();
            }

            // Zero offset
            if (hdu.Cards.TryGetValue(Constants.FitsKeywordTZero, index, out card))
            {
                column.Zero = card.GetDouble();
            }

            // Format
            if (hdu.Cards.TryGetValue(Constants.FitsKeywordTDisp, index, out card))
            {
                column.Format = card.GetString();
            }

            return column;
        }
    }
}
