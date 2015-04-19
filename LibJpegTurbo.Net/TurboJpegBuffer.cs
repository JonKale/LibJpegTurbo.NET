namespace LibJpegTurbo.Net
{
    #region

    using System;

    #endregion

    /// <summary>
    /// Represents a buffer allocated by libjpeg-turbo on its heap.
    /// </summary>
    public class TurboJpegBuffer : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegBuffer" /> class.
        /// </summary>
        /// <param name="buffer">The start of the buffer.</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        public TurboJpegBuffer(IntPtr buffer, int bufferSize)
        {
            if (buffer == IntPtr.Zero)
            {
                throw new ArgumentOutOfRangeException("buffer", "buffer must not be the NUL pointer");
            }

            this.Buffer = buffer;
            this.BufferSize = bufferSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegBuffer" /> class and allocates a buffer of the
        /// specified size.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer.</param>
        public TurboJpegBuffer(int bufferSize)
        {
            this.Buffer = TurboJpegInterop.alloc(bufferSize);
            this.BufferSize = bufferSize;
        }

        /// <summary>
        /// Gets the buffer.
        /// </summary>
        public IntPtr Buffer { get; private set; }

        /// <summary>
        /// Gets the size of the buffer (in bytes).
        /// </summary>
        public int BufferSize { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is invalid.
        /// </summary>
        public bool IsInvalid
        {
            get { return this.Buffer == IntPtr.Zero; }
        }

        /// <summary>
        /// Gets the buffer as an array of byte on the managed heap.
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return this.Buffer.ToByteArray(this.BufferSize);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="TurboJpegBuffer" /> to <see cref="IntPtr" />.
        /// </summary>
        /// <param name="buffer">The handle.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">handle is invalid</exception>
        public static explicit operator IntPtr(TurboJpegBuffer buffer)
        {
            if (buffer.IsInvalid)
            {
                throw new InvalidOperationException("handle is invalid");
            }

            return buffer.Buffer;
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