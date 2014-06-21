using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;

namespace Jhu.SkyQuery.Format.Fits
{
    [Serializable]
    [DataContract(Namespace = "")]
    public class FitsFile : IDisposable, ICloneable
    {
        public static readonly StringComparison Comparision = StringComparison.InvariantCultureIgnoreCase;
        public static readonly StringComparer Comparer = StringComparer.InvariantCultureIgnoreCase;
        public static readonly System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture;

        #region Private member variables

        /// <summary>
        /// Base stream to read from/write to
        /// </summary>
        /// <remarks>
        /// Either set by the constructor (in this case the stream is not owned)
        /// or opened internally (owned)
        /// </remarks>
        [NonSerialized]
        private Stream baseStream;

        /// <summary>
        /// If true, baseStream was opened by the object and will need
        /// to be closed when disposing.
        /// </summary>
        [NonSerialized]
        private bool ownsBaseStream;

        /// <summary>
        /// A seek forward stream that is used to wrap sequential streams.
        /// </summary>
        [NonSerialized]
        private SeekForwardStream forwardStream;

        /// <summary>
        /// Read or write
        /// </summary>
        [NonSerialized]
        private FitsFileMode fileMode;

        /// <summary>
        /// Path to the file. If set, the class can open it internally.
        /// </summary>
        [NonSerialized]
        private string path;

        /// <summary>
        /// Endianness. Many FITS files are big-endian
        /// </summary>
        private Endianness endianness;

        /// <summary>
        /// Little-endian or big-endian bit converter.
        /// </summary>
        private BitConverterBase bitConverter;

        /// <summary>
        /// Stores the hdus read/written so far.
        /// </summary>
        [NonSerialized]
        private List<HduBase> hdus;

        /// <summary>
        /// Points to the current block in the blocks collection
        /// </summary>
        /// <remarks>
        /// This can be different from hdus.Count as blocks can be
        /// predefined by the user or automatically generated as new
        /// hdus are discovered while reading the file.
        /// </remarks>
        [NonSerialized]
        private int hduCounter;

        #endregion
        #region Properties

        /// <summary>
        /// Gets the stream that can be used to read data
        /// </summary>
        [IgnoreDataMember]
        public virtual Stream BaseStream
        {
            get { return baseStream; }
            set { baseStream = value; }
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
        /// Gets or sets file mode (read or write)
        /// </summary>
        [IgnoreDataMember]
        public FitsFileMode FileMode
        {
            get { return fileMode; }
            set
            {
                EnsureNotOpen();
                fileMode = value;
            }
        }

        /// <summary>
        /// Gets or sets the location of the file
        /// </summary>
        [DataMember]
        public string Path
        {
            get { return path; }
            set
            {
                EnsureNotOpen();
                path = value;
            }
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

        /// <summary>
        /// Gets the BitConverter for byte order swapping. Used internally.
        /// </summary>
        internal BitConverterBase BitConverter
        {
            get { return bitConverter; }
        }

        /// <summary>
        /// Gets a collection of HDUs blocks.
        /// </summary>
        [IgnoreDataMember]
        protected List<HduBase> Hdus
        {
            get { return hdus; }
        }

        /// <summary>
        /// Gets if the underlying data file is closed
        /// </summary>
        [IgnoreDataMember]
        public virtual bool IsClosed
        {
            get { return baseStream == null; }
        }

        #endregion
        #region Constructors and initializers

        public FitsFile()
        {
            InitializeMembers(new StreamingContext());
        }

        public FitsFile(FitsFile old)
        {
            CopyMembers(old);
        }

        public FitsFile(string path, FitsFileMode fileMode, Endianness endianness)
        {
            InitializeMembers(new StreamingContext());

            this.path = path;
            this.fileMode = fileMode;
            this.endianness = endianness;

            Open();
        }

        public FitsFile(string path, FitsFileMode fileMode)
            : this(path, fileMode, Endianness.LittleEndian)
        {
            // Overload
        }

        public FitsFile(Stream stream, FitsFileMode fileMode, Endianness endianness)
        {
            InitializeMembers(new StreamingContext());

            OpenExternalStream(stream, fileMode, endianness);
        }

        public FitsFile(Stream stream, FitsFileMode fileMode)
            : this(stream, fileMode, Endianness.LittleEndian)
        {
            // Overload
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            this.baseStream = null;
            this.ownsBaseStream = false;
            this.forwardStream = null;

            this.fileMode = FitsFileMode.Unknown;
            this.path = null;

            this.endianness = Endianness.LittleEndian;
            this.bitConverter = null;

            this.hdus = new List<HduBase>();
        }

        private void CopyMembers(FitsFile old)
        {
            this.baseStream = null;
            this.ownsBaseStream = false;
            this.forwardStream = null;

            this.fileMode = old.fileMode;
            this.path = old.path;

            this.endianness = old.endianness;
            this.bitConverter = old.bitConverter;

            // Deep copy HDUs
            this.hdus = new List<HduBase>();
            foreach (var hdu in old.hdus)
            {
                this.hdus.Add((HduBase)hdu.Clone());
            }
        }

        public void Dispose()
        {
            Close();
        }

        public object Clone()
        {
            return new FitsFile(this);
        }

        #endregion
        #region Stream open/close

        /// <summary>
        /// Makes sure that the base stream is not open, if
        /// stream is owned by the class.
        /// </summary>
        protected virtual void EnsureNotOpen()
        {
            if (ownsBaseStream && baseStream != null)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Opens the file by opening a stream to the resource
        /// identified by the Uri property.
        /// </summary>
        public void Open()
        {
            EnsureNotOpen();

            switch (fileMode)
            {
                case FitsFileMode.Read:
                    OpenForRead();
                    break;
                case FitsFileMode.Write:
                    OpenForWrite();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Opens a file by wrapping an external file stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileMode"></param>
        public void Open(Stream stream, FitsFileMode fileMode, Endianness endianness)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");  // TODO
            }

            OpenExternalStream(stream, fileMode, endianness);
            Open();
        }

        /// <summary>
        /// Opens a file by opening a new stream.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="fileMode"></param>
        public void Open(string path, FitsFileMode fileMode, Endianness endianness)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path"); // TODO
            }

            this.path = path;
            this.fileMode = fileMode;
            this.endianness = endianness;

            Open();
        }

        /// <summary>
        /// Opens a file by wrapping a stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="mode"></param>
        /// <param name="compression"></param>
        protected void OpenExternalStream(Stream stream, FitsFileMode fileMode, Endianness endianness)
        {
            this.baseStream = stream;
            this.ownsBaseStream = false;

            this.fileMode = fileMode;
            this.endianness = endianness;
        }

        /// <summary>
        /// Opens the underlying stream, if it is not set externally via
        /// a constructor or the OpenStream method.
        /// </summary>
        private void OpenOwnStream()
        {
            switch (fileMode)
            {
                case FitsFileMode.Read:
                    baseStream = new FileStream(path, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read);
                    break;
                case FitsFileMode.Write:
                    baseStream = new FileStream(path, System.IO.FileMode.Create, FileAccess.Write, FileShare.Read);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            ownsBaseStream = true;
        }

        protected void OpenForRead()
        {
            if (FileMode != FitsFileMode.Read)
            {
                throw new InvalidOperationException();
            }

            if (baseStream == null)
            {
                OpenOwnStream();
            }

            forwardStream = new SeekForwardStream(new DetachedStream(baseStream));

            CreateBitConverter();
        }

        protected void OpenForWrite()
        {
            if (FileMode != FitsFileMode.Write)
            {
                throw new InvalidOperationException();
            }

            if (baseStream == null)
            {
                OpenOwnStream();
            }

            forwardStream = new SeekForwardStream(new DetachedStream(baseStream));

            CreateBitConverter();
        }

        /// <summary>
        /// Closes the data file
        /// </summary>
        public void Close()
        {
            if (forwardStream != null)
            {
                forwardStream.Flush();
                forwardStream.Close();
                forwardStream.Dispose();
                forwardStream = null;
            }

            if (ownsBaseStream && baseStream != null)
            {
                baseStream.Flush();
                baseStream.Close();
                baseStream.Dispose();
                baseStream = null;
                ownsBaseStream = false;
            }
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

        public HduBase ReadNextHdu()
        {
            if (hduCounter != -1)
            {
                // If we are not at the beginning of the file, read to the end of the
                // block, read the block footer and position stream on the beginning
                // of the next file block
                hdus[hduCounter].ReadToFinish();
            }

            try
            {
                hduCounter++;

                HduBase nextHdu;

                // If blocks are created manually, the blocks collection might already
                // contain an object for the next file block. In this case, use the
                // manually created object, otherwise create one automatically.
                if (hduCounter < hdus.Count)
                {
                    nextHdu = ReadNextHdu(hdus[hduCounter]);
                }
                else
                {
                    // Create a new block automatically, if collection is not predefined
                    nextHdu = ReadNextHdu(null);
                    if (nextHdu != null)
                    {
                        hdus.Add(nextHdu);
                    }
                }

                // See if there's a new block in the file.
                if (nextHdu != null)
                {
                    nextHdu.ReadHeader();
                    return nextHdu;
                }

                // FITS files don't have footers, so nothing to do here
            }
            catch (EndOfStreamException)
            {
                // Some data formats cannot detect end of blocks and will
                // throw exception at the end of the file instead
                // Eat this exception now. Note, that this behaviour won't
                // occur when block contents are read, so the integrity of
                // reading a block will be kept anyway.
            }

            // No additional blocks found, return with null
            hduCounter = -1;
            return null;
        }

        private HduBase ReadNextHdu(HduBase prototype)
        {
            HduBase hdu;

            if (prototype != null)
            {
                hdu = prototype;
                hdu.Fits = this;
            }
            else
            {
                hdu = new HduBase(this);
            }

            hdu.ReadHeader();

            // Dispatch different types of FITS HDUs
            if (prototype != null)
            {
                return hdu;
            }
            else if (hdu.IsSimple)
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

        internal void SkipBlock()
        {
            var offset = 2880 * ((forwardStream.Position + 2879) / 2880) - forwardStream.Position;
            forwardStream.Seek(offset, SeekOrigin.Current);
        }
    }
}
