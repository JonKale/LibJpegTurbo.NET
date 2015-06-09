namespace LibJpegTurbo.Net
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Base class for the compressor and decompressor.
    /// </summary>
    internal abstract class TurboJpegBase : IDisposable
    {
        /// <summary>
        /// The instance of a TurboJPEG compressor or decompressor that we're wrapping.
        /// </summary>
        private readonly TurboJpegSafeHandle turboJpegObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegBase"/> class.
        /// </summary>
        /// <param name="turboJpegHandle">The TurboJPEG handle.</param>
        protected TurboJpegBase(IntPtr turboJpegHandle)
        {
            Contract.Requires(turboJpegHandle != null);

            this.turboJpegObject = new TurboJpegSafeHandle(turboJpegHandle);
        }

        /// <summary>
        /// Gets the handle.
        /// </summary>
        public IntPtr Handle
        {
            get { return this.turboJpegObject.DangerousGetHandle(); }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            try
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to 
        /// release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            // once Dispose() has been called on a SafeHandle it is no longer valid
            // if we get called by the finaliser then all bets are off; bail immediately
            if (disposing && this.turboJpegObject != null && !this.turboJpegObject.IsInvalid)
            {
                this.turboJpegObject.Dispose();
            }
        }
    }
}