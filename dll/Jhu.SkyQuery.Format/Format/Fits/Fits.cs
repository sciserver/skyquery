using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Jhu.Graywulf.Types;
using Jhu.Graywulf.Format;

namespace Jhu.SkyQuery.Format.Fits
{
    public class Fits : DataFileBase, IDisposable
    {
        public static readonly StringComparison Comparision = StringComparison.InvariantCultureIgnoreCase;
        public static readonly StringComparer Comparer = StringComparer.InvariantCultureIgnoreCase;
        public static readonly System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture;

        #region Private members

        private SeekForwardStream forwardStream;

        /// <summary>
        /// Endianness. Many FITS files are big-endian
        /// </summary>
        private Endianness endianness;

        /// <summary>
        /// Little-endian or big-endian bit converter.
        /// </summary>
        private BitConverterBase bitConverter;

        #endregion
        #region Properties

        public override FileFormatDescription Description
        {
            get
            {
                return new FileFormatDescription()
                {
                    DisplayName = FileFormatNames.Fits,
                    DefaultExtension = Constants.FileExtensionFits,
                    CanRead = true,
                    CanWrite = false,
                    CanDetectColumnNames = false,
                    MultipleDatasets = true,
                    IsCompressed = false
                };
            }
        }

        /// <summary>
        /// Gets the stream data is read from or written to. Used internally.
        /// </summary>
        /// <remarks>
        /// Depending whether compression is turned on or not, we need to use
        /// the baseStream or the wrapper stream.
        /// </remarks>
        internal SeekForwardStream ForwardStream
        {
            get { return forwardStream; }
        }

        /// <summary>
        /// Gets the BitConverter for byte order swapping. Used internally.
        /// </summary>
        internal BitConverterBase BitConverter
        {
            get { return bitConverter; }
        }

        /// <summary>
        /// Gets or sets the endianness of the file.
        /// </summary>
        public Endianness Endianness
        {
            get { return endianness; }
            set
            {
                EnsureNotOpen();
                endianness = value;
            }
        }

        #endregion
        #region Constructors and initializers

        public Fits()
        {
            InitializeMembers();
        }

        public Fits(Uri uri, DataFileMode fileMode, CompressionMethod compression, Endianness endianness)
            : base(uri, fileMode, compression)
        {
            InitializeMembers();

            this.endianness = endianness;

            Open();
        }

        public Fits(Uri uri, DataFileMode fileMode)
            : this(uri, fileMode, CompressionMethod.None, Endianness.LittleEndian)
        {
            // Overload
        }

        public Fits(Stream stream, DataFileMode fileMode, CompressionMethod compression, Endianness endianness)
            : base(stream, fileMode, compression)
        {
            InitializeMembers();

            this.endianness = endianness;

            Open();
        }

        public Fits(Stream stream, DataFileMode fileMode)
            : this(stream, fileMode, CompressionMethod.None, Endianness.LittleEndian)
        {
            // Overload
        }

        private void InitializeMembers()
        {
            this.forwardStream = null;
            this.bitConverter = null;

            this.endianness = Endianness.LittleEndian;
        }

        #endregion
        #region Stream open/close

        public override void Open(Stream stream, DataFileMode fileMode)
        {
            base.Open(stream, fileMode);

            Open();
        }

        protected override void OpenForRead()
        {
            base.OpenForRead();

            if (forwardStream == null)
            {
                forwardStream = new SeekForwardStream(base.Stream);

                CreateBitConverter();
            }
        }

        protected override void OpenForWrite()
        {
            base.OpenForWrite();

            CreateBitConverter();
        }

        /// <summary>
        /// When overloaded in derived classes, closes the data file
        /// </summary>
        public override void Close()
        {
            if (forwardStream != null)
            {
                forwardStream.Flush();
                forwardStream.Close();
                forwardStream.Dispose();
                forwardStream = null;
            }

            base.Close();
        }

        private void CreateBitConverter()
        {
            // Create bit converter
            switch (endianness)
            {
                case Endianness.LittleEndian:
                    bitConverter = new StraightBitConverter();
                    break;
                case Endianness.BigEndian:
                    bitConverter = new SwapBitConverter();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        protected override void OnReadHeader()
        {
            // No separate fits header, only those of the HDUs
        }

        protected override DataFileBlockBase OnReadNextBlock(DataFileBlockBase block)
        {
            HduBase hdu;

            if (block != null)
            {
                hdu = (HduBase)block;
                hdu.Fits = this;
            }
            else
            {
                hdu = new HduBase(this);
            }

            hdu.ReadHeader();

            if (hdu.IsSimple)
            {
                return new ImageHdu(hdu);
            }
            else
            {
                switch (hdu.Extension)
                {
                    case Constants.FitsKeywordBinTable:
                        return new BinaryTableHdu(hdu);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        protected override void OnBlockAppended(DataFileBlockBase block)
        {
            throw new NotImplementedException();
        }

        protected override void OnReadFooter()
        {
            // No separate fits footer
        }

        protected override void OnWriteHeader()
        {
            throw new NotImplementedException();
        }

        protected override DataFileBlockBase OnWriteNextBlock(DataFileBlockBase block, System.Data.IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override void OnWriteFooter()
        {
            throw new NotImplementedException();
        }

        internal void SkipBlock()
        {
            var offset = 2880 * ((forwardStream.Position + 2879) / 2880) - forwardStream.Position;
            forwardStream.Seek(offset, SeekOrigin.Current);
        }
    }
}
