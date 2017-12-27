using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Jhu.Graywulf.IO;
using Jhu.Graywulf.Format;
using System.Threading.Tasks;

namespace Jhu.SkyQuery.Format.VoTable
{
    /// <summary>
    /// Implements a wrapper around VOTable files.
    /// </summary>
    [Serializable]
    public class VoTableWrapper : DataFileBase, ICloneable, IDisposable
    {
        #region Private member variables

        [NonSerialized]
        private VO.VoTable.VoTable votable;

        [NonSerialized]
        private SharpFitsIO.Endianness endianness;

        #endregion
        #region Properties

        internal VO.VoTable.VoTable VoTable
        {
            get { return votable; }
        }

        public SharpFitsIO.Endianness Endianness
        {
            get { return endianness; }
            set { endianness = value; }
        }

        #endregion
        #region Constructors and initializers

        public VoTableWrapper()
            : base()
        {
            InitializeMembers(new StreamingContext());
        }

        public VoTableWrapper(VoTableWrapper old)
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
        public VoTableWrapper(Uri uri, DataFileMode fileMode, SharpFitsIO.Endianness endianness)
            : base(uri, fileMode)
        {
            InitializeMembers(new StreamingContext());

            this.endianness = endianness;

            Open();
        }

        public VoTableWrapper(Uri uri, DataFileMode fileMode)
            : this(uri, fileMode, SharpFitsIO.Endianness.BigEndian)
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
        public VoTableWrapper(Stream stream, DataFileMode fileMode, SharpFitsIO.Endianness endianness)
            : base(stream, fileMode)
        {
            InitializeMembers(new StreamingContext());

            this.endianness = endianness;

            Open();
        }

        public VoTableWrapper(Stream stream, DataFileMode fileMode)
            : this(stream, fileMode, SharpFitsIO.Endianness.BigEndian)
        {
            // Overload
        }

        [OnDeserializing]
        private void InitializeMembers(StreamingContext context)
        {
            Description = new FileFormatDescription()
            {
                DisplayName = FileFormatNames.VOTable,
                MimeType = Constants.MimeTypeVoTable,
                Extension = Constants.FileExtensionVoTable,
                CanRead = true,
                CanWrite = true,
                CanDetectColumnNames = true,
                CanHoldMultipleDatasets = true,
                RequiresArchive = false,
                IsCompressed = false,
                KnowsRecordCount = false,
                RequiresRecordCount = false,
            };

            this.votable = null;
            this.endianness = SharpFitsIO.Endianness.BigEndian;
        }

        private void CopyMembers(VoTableWrapper old)
        {
            this.votable = null;
            this.endianness = old.endianness;
        }

        public override void Dispose()
        {
            Close();
            base.Dispose();
        }

        public override object Clone()
        {
            return new VoTableWrapper(this);
        }

        #endregion
        #region File open and close

        protected override void EnsureNotOpen()
        {
            if (votable != null)
            {
                throw new InvalidOperationException();
            }
        }

        protected override async Task OpenForReadAsync()
        {
            if (votable == null)
            {
                await base.OpenForReadAsync();

                // TODO: make it async if necessary
                votable = new VO.VoTable.VoTable(BaseStream, FileAccess.Read, endianness);
            }
        }

        protected override async Task OpenForWriteAsync()
        {
            if (votable == null)
            {
                await base.OpenForWriteAsync();

                // TODO: make it async if necessary
                votable = new VO.VoTable.VoTable(BaseStream, FileAccess.Write, endianness);
            }
        }

        public override void Close()
        {
            if (votable != null)
            {
                votable.Close();
            }

            base.Close();
        }

        public override bool IsClosed
        {
            get
            {
                return votable == null || votable.IsClosed;
            }
        }

        #endregion
        #region Resource read and write functions

        protected override Task OnReadHeaderAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task<DataFileBlockBase> OnReadNextBlockAsync(DataFileBlockBase block)
        {
            //return block ?? new FitsBinaryTableWrapper(this, (BinaryTableHdu)hdu);
            throw new NotImplementedException();
        }

        protected override Task<DataFileBlockBase> OnCreateNextBlockAsync(DataFileBlockBase block)
        {
            // return Task.FromResult(block ?? new FitsBinaryTableWrapper(this, BinaryTableHdu.Create(votable, true)));
            throw new NotImplementedException();
        }

        protected override void OnBlockAppended(DataFileBlockBase block)
        {
            // Nothing to do here
        }

        protected override Task OnReadFooterAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task OnWriteHeaderAsync()
        {
            // As a header, we write the first HDU now,
            // because bintables are all just extensions

            // TODO: rewrite to async FITS

            //var hdu = SimpleHdu.Create(votable, true, true, true);
            //await hdu.WriteHeaderAsync();

            throw new NotImplementedException();
        }

        protected override Task OnWriteFooterAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
