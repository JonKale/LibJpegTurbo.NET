namespace LibJpegTurbo.Net
{
    using System;

    /// <summary>
    /// Represents a buffer allocated by libjpeg-turbo on its heap.
    /// </summary>
    public class TurboJpegBuffer : IDisposable
    {
        /// <summary>
        /// Gets the buffer.
        /// </summary>
        public IntPtr Buffer { get; private set; }

        /// <summary>
        /// Gets the size of the buffer (in bytes).
        /// </summary>
        public int BufferSize { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegBuffer"/> class.
        /// </summary>
        /// <param name="buffer">The start of the buffer.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public TurboJpegBuffer(IntPtr buffer, int bufferSize)
        {
            this.Buffer = buffer;
            this.BufferSize = bufferSize;
        }

        /// <summary>
        /// Gets the buffer as an array of bte on the managed heap.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return this.Buffer.ToByteArray(this.BufferSize);
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to 
        /// release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (this.Buffer != IntPtr.Zero)
            {
                TurboJpegInterop.free(this.Buffer);
                this.Buffer = IntPtr.Zero;
            }
        }

        #endregion
    }
}