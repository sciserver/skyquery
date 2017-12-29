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
        private VO.VoTable.VoTableVersion version;

        [NonSerialized]
        private VO.VoTable.VoTableSerialization serialization;

        #endregion
        #region Properties

        internal VO.VoTable.VoTable VoTable
        {
            get { return votable; }
        }

        public VO.VoTable.VoTableVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        public VO.VoTable.VoTableSerialization Serialization
        {
            get { return serialization; }
            set { serialization = value; }
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
        public VoTableWrapper(Uri uri, DataFileMode fileMode)
            : base(uri, fileMode)
        {
            InitializeMembers(new StreamingContext());
            Open();
        }
        
        /// <summary>
        /// Initializes a FITS file by automatically wrapping and already open binary stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fileMode"></param>
        public VoTableWrapper(Stream stream, DataFileMode fileMode)
            : base(stream, fileMode)
        {
            InitializeMembers(new StreamingContext());
            Open();
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
            this.version = VO.VoTable.VoTableVersion.V1_3;
            this.serialization = VO.VoTable.VoTableSerialization.TableData;
        }

        private void CopyMembers(VoTableWrapper old)
        {
            this.votable = null;
            this.version = old.version;
            this.serialization = old.serialization;
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
                votable = new VO.VoTable.VoTable(BaseStream, FileAccess.Read, version);
            }
        }

        protected override async Task OpenForWriteAsync()
        {
            if (votable == null)
            {
                await base.OpenForWriteAsync();

                votable = new VO.VoTable.VoTable(BaseStream, FileAccess.Write, version);

                // TODO: initialize
                // Move initialization code from VoTable.OpenForWrite()
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
            return votable.ReadHeaderAsync();
        }

        protected override Task<DataFileBlockBase> OnReadNextBlockAsync(DataFileBlockBase block)
        {
            var resource = votable.ReadNextResource();

            if (resource != null)
            {
                return Task.FromResult(block ?? new VoTableResourceWrapper(this, votable.ReadNextResource()));
            }
            else
            {
                return Task.FromResult<DataFileBlockBase>(null);
            }
        }

        protected override Task<DataFileBlockBase> OnCreateNextBlockAsync(DataFileBlockBase block)
        {
            return Task.FromResult(block ?? new VoTableResourceWrapper(this, VO.VoTable.VoTableResource.Create(this.votable, serialization)));
        }

        protected override void OnBlockAppended(DataFileBlockBase block)
        {
            // Nothing to do here
        }

        protected override Task OnReadFooterAsync()
        {
            return votable.ReadFooterAsync();
        }

        protected override async Task OnWriteHeaderAsync()
        {
            await votable.WriteHeaderAsync();
        }

        protected override async Task OnWriteFooterAsync()
        {
            await votable.WriteFooterAsync();
        }

        #endregion
    }
}
