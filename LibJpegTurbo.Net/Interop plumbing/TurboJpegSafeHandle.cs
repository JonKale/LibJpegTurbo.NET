namespace LibJpegTurbo.Net
{
    #region

    using System;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    /// A wrapper for a handle to a TurboJPEG compressor, decompressor or transformer instance.
    /// </summary>
    internal class TurboJpegSafeHandle : SafeHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegSafeHandle" /> class with the specified handle value.
        /// </summary>
        /// <param name="handle">The handle.</param>
        public TurboJpegSafeHandle(IntPtr handle)
            : base(IntPtr.Zero, true)
        {
            if (handle == IntPtr.Zero)
            {
                throw new ArgumentNullException("handle");
            }

            this.handle = handle;
        }

        #region Overrides of SafeHandle

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the handle value is invalid.
        /// </summary>
        /// <returns>
        /// true if the handle value is invalid; otherwise, false.
        /// </returns>
        public override bool IsInvalid
        {
            get { return this.handle == IntPtr.Zero; }
        }

        /// <summary>
        /// When overridden in a derived class, executes the code required to free the handle.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the handle is released successfully; otherwise, in the event of a catastrophic failure, <c>false.</c>
        /// </returns>
        protected override bool ReleaseHandle()
        {
            return TurboJpegInterop.destroy(this.handle) ==  0;
        }

        #endregion
    }
}