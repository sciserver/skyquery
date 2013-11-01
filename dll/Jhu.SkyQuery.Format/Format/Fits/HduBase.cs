using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using Jhu.Graywulf.Types;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.Fits
{
    public class HduBase : DataFileBlockBase
    {
        private bool headerRead;
        private long headerPosition;
        private long dataPosition;

        private CardCollection cards;

        private byte[] strideBuffer;
        private int totalStrides;
        private int strideCounter;

        internal Fits Fits
        {
            get { return (Fits)file; }
            set { file = value; }
        }

        public long HeaderPosition
        {
            get { return headerPosition; }
        }

        public long DataPosition
        {
            get { return dataPosition; }
        }

        public CardCollection Cards
        {
            get { return cards; }
        }

        protected byte[] StrideBuffer
        {
            get { return strideBuffer; }
        }

        public int TotalStrides
        {
            get { return totalStrides; }
        }

        #region Keyword accessors

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

        public int BitsPerPixel
        {
            get
            {
                return cards[Constants.FitsKeywordBitPix].GetInt32();
            }
        }

        public int AxisCount
        {
            get
            {
                return cards[Constants.FitsKeywordNAxis].GetInt32();
            }
        }

        public int GetAxisLength(int i)
        {
            return cards[Constants.FitsKeywordNAxis + (i + 1).ToString(CultureInfo.InvariantCulture)].GetInt32();
        }

        #endregion

        internal HduBase(Fits fits)
            : base(fits)
        {
            InitializeMembers();
        }

        internal HduBase(HduBase old)
            : base(old)
        {
            CopyMembers(old);
        }

        private void InitializeMembers()
        {
            this.headerRead = false;
            this.headerPosition = -1;
            this.dataPosition = -1;

            this.cards = new CardCollection();

            this.strideBuffer = null;
            this.totalStrides = 0;
            this.strideCounter = 0;
        }

        private void CopyMembers(HduBase old)
        {
            this.headerRead = old.headerRead;
            this.headerPosition = old.headerPosition;
            this.dataPosition = old.dataPosition;

            this.cards = new CardCollection(old.cards);

            this.strideBuffer = null;
            this.totalStrides = 0;
            this.strideCounter = 0;
        }

        #region Column functions

        protected override void OnColumnsCreated()
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Read functions

        public void ReadHeader()
        {
            OnReadHeader();
        }

        protected override void OnReadHeader()
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

        protected override bool OnReadNextRow(object[] values)
        {
            throw new NotImplementedException();
        }

        protected override void OnReadToFinish()
        {
            var sl = GetStrideLength();
            var sc = GetTotalStrides();

            long offset = sl * (sc - strideCounter);
            Fits.ForwardStream.Seek(offset, SeekOrigin.Current);

            Fits.SkipBlock();
        }

        protected override void OnReadFooter()
        {
            // No HDU footer, skip is done in OnReadToFinish
        }

        #endregion
        #region Write functions

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
                throw new FileFormatException("Unexpected end of stream.");  // *** TODO
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
