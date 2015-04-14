namespace LibJpegTurbo.Net
{
    using System;

    public class TurboJpegBase 
    {
        /// <summary>
        /// The instance of a TurboJPEG compressor that we're wrapping.
        /// </summary>
        private readonly TurboJpegSafeHandle turboJpegObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegBase"/> class.
        /// </summary>
        /// <param name="turboJpegHandle">The TurboJPEG handle.</param>
        protected TurboJpegBase(IntPtr turboJpegHandle)
        {
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
        public void Dispose(bool disposing)
        {
            if (this.turboJpegObject != null && !this.turboJpegObject.IsInvalid)
            {
                this.turboJpegObject.Dispose();
            }
        }
    }
}