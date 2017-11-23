using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Jhu.SharpFitsIO;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Format.Fits
{
    /// <summary>
    /// Implements a wrapper around FITS file to read and write binary tables.
    /// </summary>
    [Serializable]
    public class FitsFileWrapper : DataFileBase, ICloneable, IDisposable
    {
        #region Private member variables

        [NonSerialized]
        private FitsFile fits;

        [NonSerialized]
        private Endianness endianness;

        #endregion
        #region Properties

        internal FitsFile Fits
        {
            get { return fits; }
        }

        public Endianness Endianness
        {
            get { return endianness; }
            set { endianness = value; }
        }

        #endregion
        #region Constructors and initializers

        public FitsFileWrapper()
            :base()
        {
            InitializeMembers(new StreamingContext());
        }

        public FitsFileWrapper(FitsFileWrapper old)
            :base(old)
        {
            CopyMembers(old);
        }

        /// <summary>
        /// Initializes a FITS file by automatically opening an underlying stream
        /// identified by an URI.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="fileMode"></param>
        /// <param name="compression"></param>
        /// <param name="encoding"></param>
        public FitsFileWrapper(Uri uri, DataFileMode fileMode, Endianness endianness)
            : base(uri, fileMode)
        {
            InitializeMembers(new StreamingContext());

            this.endianness = endianness;

            Open();
        }

        public FitsFileWrapper(Uri uri, DataFileMode fileMode)
            : this(uri, fileMode, Endianness.BigEndian)
        {
            // Overload
        }

        /// <summary>
        /// Initializes a FITS file by automatically wrapping and already open binary stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileMode"></param>
        /// <param name="compression"></param>
        /// <param name="encoding"></param>
        public FitsFileWrapper(Stream stream, DataFileMode fileMode, Endianness endianness)
            : base(stream, fileMode)
        {
            InitializeMembers(new StreamingContext());

            this.endianness = endianness;

            Open();
        }

        public FitsFileWrapper(Stream stream, DataFileMode fileMode)
            : this(stream, fileMode, Endianness.BigEndian)
        {
            // Overload
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            Description = new FileFormatDescription()
            {
                DisplayName = FileFormatNames.Fits,
                MimeType = Constants.MimeTypeFits,
                Extension = Constants.FileExtensionFits,
                CanRead = true,
                CanWrite = true,
                CanDetectColumnNames = false,
                CanHoldMultipleDatasets = true,
                RequiresArchive = false,
                IsCompressed = false,
                KnowsRecordCount = true,
                RequiresRecordCount = true,
            };

            this.fits = null;
            this.endianness = SharpFitsIO.Endianness.BigEndian;
        }

        private void CopyMembers(FitsFileWrapper old)
        {
            this.fits = null;
            this.endianness = old.endianness;
        }

        public override void Dispose()
        {
            Close();
            base.Dispose();
        }

        public override object Clone()
        {
            return new FitsFileWrapper(this);
        }

        #endregion
        #region File open and close

        protected override void EnsureNotOpen()
        {
            if (fits != null)
            {
                throw new InvalidOperationException();
            }
        }

        protected override async Task OpenForReadAsync()
        {
            if (fits == null)
            {
                await base.OpenForReadAsync();

                // TODO: make it async if necessary
                fits = new FitsFile(BaseStream, FitsFileMode.Read, endianness);
            }
        }

        protected override async Task OpenForWriteAsync()
        {
            if (fits == null)
            {
                await base.OpenForWriteAsync();

                // TODO: make it async if necessary
                fits = new FitsFile(BaseStream, FitsFileMode.Write, endianness);
            }
        }

        public override void Close()
        {
            if (fits != null)
            {
                fits.Close();
            }

            base.Close();
        }

        public override bool IsClosed
        {
            get
            {
                return fits == null || fits.IsClosed;
            }
        }

        #endregion
        #region HDU read and write functions

        protected override async Task<DataFileBlockBase> OnReadNextBlockAsync(DataFileBlockBase block)
        {
            // TODO: rewrite FITS io to async
            // Read until the next binary table extension is found
            SimpleHdu hdu;
            while ((hdu = await Fits.ReadNextHduAsync()) != null && !(hdu is BinaryTableHdu))
            {
            }

            if (hdu == null)
            {
                return null;
            }
            else
            {
                return block ?? new FitsBinaryTableWrapper(this, (BinaryTableHdu)hdu);
            }
        }

        protected override Task<DataFileBlockBase> OnCreateNextBlockAsync(DataFileBlockBase block)
        {
            return Task.FromResult(block ?? new FitsBinaryTableWrapper(this, BinaryTableHdu.Create(fits, true)));
        }

        protected override void OnBlockAppended(DataFileBlockBase block)
        {
            // Nothing to do here
        }

        protected override Task OnReadHeaderAsync()
        {
            // FITS files don't have a separate header, they start with the first HDU
            return Task.CompletedTask;
        }

        protected override Task OnReadFooterAsync()
        {
            // FITS files don't have footers
            return Task.CompletedTask;
        }

        protected override async Task OnWriteHeaderAsync()
        {
            // As a header, we write the first HDU now,
            // because bintables are all just extensions

            // TODO: rewrite to async FITS

            var hdu = SimpleHdu.Create(fits, true, true, true);
            await hdu.WriteHeaderAsync();
        }

        protected override Task OnWriteFooterAsync()
        {
            // FITS files don't have footers
            return Task.CompletedTask;
        }

        #endregion
    }
}
