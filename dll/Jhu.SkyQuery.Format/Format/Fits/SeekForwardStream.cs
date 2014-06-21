using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Jhu.SkyQuery.Format.Fits
{
    /// <summary>
    /// Implements a wrapper around a stream that allows positioning inside the
    /// stream and supports seeking forward, even if the stream is sequential
    /// read only.
    /// </summary>
    public class SeekForwardStream : Stream
    {
        #region Private members

        private Stream baseStream;
        private long position;

        #endregion
        #region Properties

        public override bool CanRead
        {
            get { return baseStream.CanRead; }
        }

        public override bool CanWrite
        {
            get { return baseStream.CanWrite; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanTimeout
        {
            get { return baseStream.CanTimeout; }
        }

        public override int ReadTimeout
        {
            get { return baseStream.ReadTimeout; }
            set { baseStream.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { return baseStream.WriteTimeout; }
            set { baseStream.WriteTimeout = value; }
        }

        public override long Length
        {
            get { return baseStream.Length; }
        }

        public override long Position
        {
            get
            {
                if (baseStream.CanSeek)
                {
                    return baseStream.Position;
                }
                else
                {
                    return position;
                }
            }
            set
            {
                if (baseStream.CanSeek)
                {
                    baseStream.Position = value;
                }
                else if (value >= position)
                {
                    Skip(value - position);
                }
                else
                {
                    throw new NotSupportedException("Seeking backwards is not supported");
                }
            }
        }

        #endregion
        #region Constructors and initializers

        public SeekForwardStream(Stream baseStream)
        {
            this.baseStream = baseStream;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion
        #region Stream overriden functions

        public override void Close()
        {
            baseStream.Close();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return baseStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            var res = baseStream.EndRead(asyncResult);
            position += res;
            return res;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var res = baseStream.Read(buffer, offset, count);
            position += res;
            return res;
        }

        public override int ReadByte()
        {
            var res = baseStream.ReadByte();
            position++;
            return res;
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            var res = baseStream.BeginWrite(buffer, offset, count, callback, state);
            position += count;
            return res;
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            baseStream.EndWrite(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            baseStream.Write(buffer, offset, count);
            position += count;
        }

        public override void WriteByte(byte value)
        {
            baseStream.WriteByte(value);
            position++;
        }

        public override void Flush()
        {
            baseStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (baseStream.CanSeek)
            {
                var res = baseStream.Seek(offset, origin);
                position = res;
                return position;
            }
            else if (origin == SeekOrigin.Begin && offset >= position)
            {
                Skip(offset - position);
                return position;
            }
            else if (origin == SeekOrigin.Current && offset >= 0)
            {
                Skip(offset);
                return position;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public override void SetLength(long value)
        {
            baseStream.SetLength(value);
        }

        #endregion

        private void Skip(long count)
        {
            var buffer = new byte[0x10000]; // *** TODO: move to member

            var remaining = count;

            while (remaining > 0)
            {
                var block = (int)Math.Min(buffer.Length, remaining);

                if (block != Read(buffer, 0, block))
                {
                    throw new EndOfStreamException("Unexpected end of stream.");  // *** TODO
                }

                remaining -= block;
            }
        }
    }
}
