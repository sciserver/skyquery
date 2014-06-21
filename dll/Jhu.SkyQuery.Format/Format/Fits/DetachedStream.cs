using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Jhu.SkyQuery.Format.Fits
{
    /// <summary>
    /// Implements a wrapper around any stream that prevents closing the
    /// underlying stream by automatic dispose calls.
    /// </summary>
    /// <remarks>
    /// When streams are wrapped into TextReader/TextWrite or BinaryReader/BinaryWriter,
    /// the underlying streams get automatically closed when the wrappers are disposed.
    /// To prevent this, DetachedStream class catches these close requests. This behaviour
    /// is fixed in .Net 4.5 where reader/writer wrappers have an additional constructor
    /// parameter that tells the class to leave the underlying streams open.
    /// </remarks>
    public class DetachedStream : Stream
    {
        private Stream baseStream;

        #region Properties

        public Stream BaseStream
        {
            get { return baseStream; }
        }

        public override bool CanRead
        {
            get { return baseStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return baseStream.CanSeek; }
        }

        public override bool CanTimeout
        {
            get
            {
                return baseStream.CanTimeout;
            }
        }

        public override bool CanWrite
        {
            get { return baseStream.CanWrite; }
        }

        public override long Length
        {
            get { return baseStream.Length; }
        }

        public override long Position
        {
            get
            {
                return baseStream.Position;
            }
            set
            {
                baseStream.Position = value;
            }
        }

        public override int ReadTimeout
        {
            get
            {
                return baseStream.ReadTimeout;
            }
            set
            {
                baseStream.ReadTimeout = value;
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return baseStream.WriteTimeout;
            }
            set
            {
                baseStream.WriteTimeout = value;
            }
        }

        #endregion
        #region Constructors and initializers

        public DetachedStream(Stream baseStream)
        {
            this.baseStream = baseStream;
        }

        protected override void Dispose(bool disposing)
        {
            Close();
            // Intentionally do not call it on baseStream
        }

        #endregion

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return baseStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return baseStream.BeginWrite(buffer, offset, count, callback, state);
        }
        
        public override void Close()
        {
            if (this.baseStream != null)
            {
                this.baseStream.Flush();
                this.baseStream = null;
            }

            // Intentionally do not call it on baseStream
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return baseStream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            baseStream.EndWrite(asyncResult);
        }

        public override void Flush()
        {
            baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return baseStream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            return baseStream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            baseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            baseStream.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            baseStream.WriteByte(value);
        }
    }
}
