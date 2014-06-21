using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;

namespace Jhu.SkyQuery.Format.Fits
{
    public class HduBase : ICloneable
    {
        #region Private member variables

        /// <summary>
        /// Holds a reference to the underlying file
        /// </summary>
        /// <remarks>
        /// This value is set by the constructor when a new data file block
        /// is created based on a data file.
        /// </remarks>
        [NonSerialized]
        protected FitsFile file;

        [NonSerialized]
        private bool headerRead;

        [NonSerialized]
        private long headerPosition;

        [NonSerialized]
        private long dataPosition;

        [NonSerialized]
        private CardCollection cards;

        [NonSerialized]
        private byte[] strideBuffer;

        [NonSerialized]
        private int totalStrides;

        [NonSerialized]
        private int strideCounter;

        #endregion
        #region Properties

        [IgnoreDataMember]
        internal FitsFile Fits
        {
            get { return file; }
            set { file = value; }
        }

        [IgnoreDataMember]
        public long HeaderPosition
        {
            get { return headerPosition; }
        }

        [IgnoreDataMember]
        public long DataPosition
        {
            get { return dataPosition; }
        }

        [IgnoreDataMember]
        public CardCollection Cards
        {
            get { return cards; }
        }

        [IgnoreDataMember]
        protected byte[] StrideBuffer
        {
            get { return strideBuffer; }
        }

        [IgnoreDataMember]
        public int TotalStrides
        {
            get { return totalStrides; }
        }

        #endregion
        #region Keyword accessor properties

        [IgnoreDataMember]
        public bool IsSimple
        {
            get
            {
                Card card;
                if (cards.TryGetValue(Constants.FitsKeywordSimple, out card))
                {
                    return card.GetBoolean();
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets if this HDU has any extensions.
        /// </summary>
        /// <remarks>
        /// This is typically used in the primary header only.
        /// </remarks>
        [IgnoreDataMember]
        public bool HasExtension
        {
            get
            {
                Card card;
                if (cards.TryGetValue(Constants.FitsKeywordExtend, out card))
                {
                    return card.GetBoolean();
                }
                else if (AxisCount == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [IgnoreDataMember]
        public string Extension
        {
            get
            {
                Card card;
                if (cards.TryGetValue(Constants.FitsKeywordXtension, out card))
                {
                    return card.GetString();
                }
                else
                {
                    return null;
                }
            }
        }

        [IgnoreDataMember]
        public int BitsPerPixel
        {
            get
            {
                return cards[Constants.FitsKeywordBitPix].GetInt32();
            }
        }

        [IgnoreDataMember]
        public int AxisCount
        {
            get
            {
                return cards[Constants.FitsKeywordNAxis].GetInt32();
            }
        }

        #endregion
        #region Constructors and initializers

        internal HduBase(FitsFile fits)
        {
            InitializeMembers(new StreamingContext());
        }

        internal HduBase(HduBase old)
        {
            CopyMembers(old);
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            this.file = null;

            this.headerRead = false;
            this.headerPosition = -1;
            this.dataPosition = -1;

            this.cards = new CardCollection();
            //this.columns = 

            this.strideBuffer = null;
            this.totalStrides = 0;
            this.strideCounter = 0;
        }

        private void CopyMembers(HduBase old)
        {
            this.file = old.file;

            this.headerRead = old.headerRead;
            this.headerPosition = old.headerPosition;
            this.dataPosition = old.dataPosition;

            this.cards = new CardCollection(old.cards);
            //this.columns = 

            this.strideBuffer = null;
            this.totalStrides = 0;
            this.strideCounter = 0;
        }

        public virtual object Clone()
        {
            return new HduBase(this);
        }

#endregion

        public int GetAxisLength(int i)
        {
            return cards[Constants.FitsKeywordNAxis + (i + 1).ToString(CultureInfo.InvariantCulture)].GetInt32();
        }

        #region Read functions

        public void ReadHeader()
        {
            // Make sure header is read only once
            if (!headerRead)
            {
                // Save start position
                headerPosition = Fits.ForwardStream.Position;

                Card card;

                do
                {
                    card = new Card();
                    card.Read(Fits.ForwardStream);

                    if (!cards.ContainsKey(card.Keyword))
                    {
                        // *** TODO: handle comments
                        if (!card.IsComment)
                        {
                            cards.Add(card.Keyword.ToUpper(), card);
                        }
                    }
                    else
                    {
                        // *** TODO: Duplicate keys, 
                        throw new Exception();
                    }
                }
                while (!card.IsEnd);

                // Skip block
                Fits.SkipBlock();
                dataPosition = Fits.ForwardStream.Position;

                headerRead = true;
            }
        }

        internal void ReadToFinish()
        {
            var sl = GetStrideLength();
            var sc = GetTotalStrides();

            long offset = sl * (sc - strideCounter);
            Fits.ForwardStream.Seek(offset, SeekOrigin.Current);

            Fits.SkipBlock();
        }

        #endregion
        #region Stride functions

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Last axis length determines stride length
        /// </remarks>
        public virtual int GetStrideLength()
        {
            return Math.Abs(BitsPerPixel) / 8 * GetAxisLength(0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Last axis length determines stride length
        /// </remarks>
        public virtual int GetTotalStrides()
        {
            int total = 1;

            for (int i = 0; i < AxisCount - 1; i++)
            {
                total *= GetAxisLength(i + 1);
            }

            return total;
        }

        public long GetTotalSize()
        {
            return GetStrideLength() * GetTotalStrides();
        }

        public bool HasMoreStrides
        {
            get { return strideBuffer == null || strideCounter < totalStrides; }
        }

        public byte[] ReadStride()
        {
            if (strideBuffer == null)
            {
                strideBuffer = new byte[GetStrideLength()];
                totalStrides = GetTotalStrides();
                strideCounter = 0;
            }

            if (strideBuffer.Length != Fits.ForwardStream.Read(strideBuffer, 0, strideBuffer.Length))
            {
                throw new FitsException("Unexpected end of stream.");  // *** TODO
            }

            strideCounter++;

            if (!HasMoreStrides)
            {
                Fits.SkipBlock();
            }

            return strideBuffer;
        }

        #endregion
    }
}
