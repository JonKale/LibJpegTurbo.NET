namespace LibJpegTurbo.Net
{
    #region

    using System;
    using System.Diagnostics.Contracts;
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
            Contract.Requires(handle != IntPtr.Zero);

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
            if (this.IsInvalid)
            {
                return true;
            }
            
            var success = NativeMethods.destroy(this.handle);
            this.handle = IntPtr.Zero;
            return success == 0;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="TurboJpegSafeHandle"/> to <see cref="IntPtr"/>.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">handle is invalid</exception>
        public static explicit operator IntPtr(TurboJpegSafeHandle handle)
        {
            Contract.Requires(handle != null);

            if (handle.IsInvalid)
            {
                throw new InvalidOperationException("handle is invalid");
            }

            return handle.DangerousGetHandle();
        }

        #region Overrides of SafeHandle

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.Runtime.InteropServices.SafeHandle"/> class specifying whether to perform a normal dispose operation.
        /// </summary>
        /// <param name="disposing">true for a normal dispose operation; false to finalize the handle.</param>
        protected override void Dispose(bool disposing)
        {
            this.ReleaseHandle();
            base.Dispose(disposing);
        }

        #endregion

        #endregion
    }
}