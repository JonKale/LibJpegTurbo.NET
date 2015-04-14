namespace LibJpegTurbo.Net
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A wrapper for a handle to a TurboJPEG compressor, decompressor or transformer instance.
    /// </summary>
    internal class TurboJpegSafeHandle : SafeHandle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Runtime.InteropServices.SafeHandle" /> class with the specified invalid handle value.
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
        /// When overridden in a derived class, executes the code required to free the handle.
        /// </summary>
        /// <returns>
        /// true if the handle is released successfully; otherwise, in the event of a catastrophic failure, false. 
        /// In this case, it generates a releaseHandleFailed MDA Managed Debugging Assistant.
        /// </returns>
        protected override bool ReleaseHandle()
        {
            TurboJpegInterop.destroy(this.handle);
            return true;
        }

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

        #endregion
    }
}