namespace FluorineFx.Messaging.Rtmp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class RtmpQueuedWriteStream : Stream
    {
        private Stream _innerStream;
        private volatile bool _isWriting;
        private Queue<RtmpAsyncResult> _outgoingQueue = new Queue<RtmpAsyncResult>();

        public RtmpQueuedWriteStream(Stream innerStream)
        {
            this._innerStream = innerStream;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this._innerStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            RtmpAsyncResult item = new RtmpAsyncResult(callback, state, buffer, offset, count);
            lock (this._outgoingQueue)
            {
                this._outgoingQueue.Enqueue(item);
            }
            this.TryBeginWrite();
            return item;
        }

        private void BeginWriteCallback(IAsyncResult asyncResult)
        {
            RtmpAsyncResult asyncState = asyncResult.AsyncState as RtmpAsyncResult;
            if (asyncState != null)
            {
                Queue<RtmpAsyncResult> queue;
                try
                {
                    this.InternalEndWrite(asyncResult);
                    lock ((queue = this._outgoingQueue))
                    {
                        this._isWriting = false;
                    }
                    asyncState.SetComplete(null);
                }
                catch (Exception exception)
                {
                    lock ((queue = this._outgoingQueue))
                    {
                        this._isWriting = false;
                    }
                    asyncState.SetComplete(exception);
                }
                finally
                {
                    try
                    {
                        this.TryBeginWrite();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public override void Close()
        {
            this._innerStream.Close();
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this._outgoingQueue)
                {
                    this._outgoingQueue.Clear();
                }
            }
            base.Dispose(disposing);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return this._innerStream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            RtmpAsyncResult result = asyncResult as RtmpAsyncResult;
            try
            {
                if (!asyncResult.IsCompleted)
                {
                    asyncResult.AsyncWaitHandle.WaitOne();
                }
                if (result.HasException())
                {
                    throw result.Exception;
                }
            }
            finally
            {
                this.TryBeginWrite();
            }
        }

        public override void Flush()
        {
            this._innerStream.Flush();
        }

        private IAsyncResult InternalBeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this.InnerStream.BeginWrite(buffer, offset, count, callback, state);
        }

        private void InternalEndWrite(IAsyncResult asyncResult)
        {
            this.InnerStream.EndWrite(asyncResult);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._innerStream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            return this._innerStream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return this._innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            this._innerStream.SetLength(value);
        }

        private void TryBeginWrite()
        {
            if (!this._isWriting)
            {
                RtmpAsyncResult result;
                Queue<RtmpAsyncResult> queue;
                lock ((queue = this._outgoingQueue))
                {
                    if (this._isWriting)
                    {
                        return;
                    }
                    if (this._outgoingQueue.Count > 0)
                    {
                        result = this._outgoingQueue.Dequeue();
                        this._isWriting = true;
                    }
                    else
                    {
                        result = null;
                    }
                }
                try
                {
                    if (result != null)
                    {
                        this.InternalBeginWrite(result.Buffer, result.Offset, result.Count, new AsyncCallback(this.BeginWriteCallback), result);
                    }
                }
                catch (Exception exception)
                {
                    lock ((queue = this._outgoingQueue))
                    {
                        this._isWriting = false;
                    }
                    if (result != null)
                    {
                        result.SetComplete(exception);
                    }
                    throw;
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.EndWrite(this.BeginWrite(buffer, offset, count, null, null));
        }

        public override void WriteByte(byte value)
        {
            this._innerStream.WriteByte(value);
        }

        public override bool CanRead
        {
            get
            {
                return this._innerStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return this._innerStream.CanSeek;
            }
        }

        public override bool CanTimeout
        {
            get
            {
                return this._innerStream.CanTimeout;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return this._innerStream.CanWrite;
            }
        }

        public Stream InnerStream
        {
            get
            {
                return this._innerStream;
            }
        }

        public override long Length
        {
            get
            {
                return this._innerStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this._innerStream.Position;
            }
            set
            {
                this._innerStream.Position = value;
            }
        }

        public override int ReadTimeout
        {
            get
            {
                return this._innerStream.ReadTimeout;
            }
            set
            {
                this._innerStream.ReadTimeout = value;
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return this._innerStream.WriteTimeout;
            }
            set
            {
                this._innerStream.WriteTimeout = value;
            }
        }
    }
}

