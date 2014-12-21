using System;
using System.IO;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace IF.Common.Metro.Structures
{
    public class MemoryRandomAccessStream : IRandomAccessStream
    {
        private readonly Stream m_InternalStream;

        public MemoryRandomAccessStream(Stream stream)
        {
            this.m_InternalStream = stream;
        }

        public MemoryRandomAccessStream(byte[] bytes)
        {
            this.m_InternalStream = new MemoryStream(bytes);
        }

        public IInputStream GetInputStreamAt(ulong position)
        {
            this.m_InternalStream.Seek((long)position, SeekOrigin.Current);

            return this.m_InternalStream.AsInputStream();
        }

        public IOutputStream GetOutputStreamAt(ulong position)
        {
            this.m_InternalStream.Seek((long)position, SeekOrigin.Current);

            return this.m_InternalStream.AsOutputStream();
        }

        public ulong Size
        {
            get { return (ulong)this.m_InternalStream.Length; }
            set { this.m_InternalStream.SetLength((long)value); }
        }

        public bool CanRead
        {
            get { return true; }
        }

        public bool CanWrite
        {
            get { return true; }
        }

        public IRandomAccessStream CloneStream()
        {
            throw new NotSupportedException();
        }

        public ulong Position
        {
            get { return (ulong)this.m_InternalStream.Position; }
        }

        public void Seek(ulong position)
        {
            this.m_InternalStream.Seek((long)position, 0);
        }

        public void Dispose()
        {
            this.m_InternalStream.Dispose();
        }

        public IAsyncOperationWithProgress<IBuffer, uint> ReadAsync(IBuffer buffer, uint count, InputStreamOptions options)
        {
            var inputStream = this.GetInputStreamAt(0);
            return inputStream.ReadAsync(buffer, count, options);
        }

        public IAsyncOperation<bool> FlushAsync()
        {
            var outputStream = this.GetOutputStreamAt(0);
            return outputStream.FlushAsync();
        }

        public IAsyncOperationWithProgress<uint, uint> WriteAsync(IBuffer buffer)
        {
            var outputStream = this.GetOutputStreamAt(0);
            return outputStream.WriteAsync(buffer);
        }
    }
}