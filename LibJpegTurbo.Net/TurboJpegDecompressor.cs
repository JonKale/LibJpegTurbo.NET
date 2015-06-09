namespace LibJpegTurbo.Net
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    /// <summary>TurboJPEG decompressor</summary>
    internal class TurboJpegDecompressor : TurboJpegBase
    {
        private byte[] jpegBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegDecompressor" /> class with the specified object.
        /// </summary>
        /// <param name="handle">The handle.</param>
        protected TurboJpegDecompressor(IntPtr handle)
            : base(handle)
        {}

        /// <summary>Create a TurboJPEG decompresssor instance.</summary>
        public TurboJpegDecompressor()
            : base(NativeMethods.initDecompressor())
        {}

        /// <summary>
        /// Create a TurboJPEG decompressor instance and associate the JPEG image stored in <code>jpegImage</code> with
        /// the newly created instance.
        /// </summary>
        /// <param name="jpegImage"> JPEG image buffer (size of the JPEG image is assumed to be the length of the array) </param>
        public TurboJpegDecompressor(byte[] jpegImage)
            : base(NativeMethods.initDecompressor())
        {
            Contract.Requires(jpegImage != null);

            this.SetJpegImage(jpegImage);
        }

        /// <summary>
        /// The height of the image.
        /// </summary>
        private int jpegHeight;

        /// <summary>
        /// The chroma subsampling used by the image.
        /// </summary>
        private Subsampling jpegSubsampling;

        /// <summary>
        /// The width of the image.
        /// </summary>
        private int jpegWidth;

        protected TurboJpegDecompressor(byte[] destinationBuffer, int transformedSize)
            : base(NativeMethods.initDecompressor())
        {
            throw new NotImplementedException();
        }

        /// <summary>Returns the width of the JPEG image associated with this decompressor instance.</summary>
        public int Width
        {
            get
            {
                Contract.Assume(this.jpegWidth > 0, "No JPEG image is associated with this instance");

                return this.jpegWidth;
            }
        }

        /// <summary>Returns the height of the JPEG image associated with this decompressor instance.</summary>
        public int Height
        {
            get
            {
                Contract.Assume(this.jpegHeight > 0, "No JPEG image is associated with this instance");

                return this.jpegHeight;
            }
        }

        /// <summary>
        /// Returns the level of chrominance subsampling used in the JPEG image associated with this decompressor
        /// instance.  See <see cref="Subsampling" />.
        /// </summary>
        public Subsampling Subsampling
        {
            get { return this.jpegSubsampling; }
        }

        /// <summary>Returns the JPEG image buffer associated with this decompressor instance.</summary>
        public byte[] JpegBuffer
        {
            get
            {
                Contract.Assume(this.jpegBuffer != null, "No JPEG image is associated with this instance");

                return this.jpegBuffer;
            }
        }

        /// <summary>Returns the size of the JPEG image (in bytes) associated with this decompressor instance.</summary>
        public int JpegSize
        {
            get
            {
                Contract.Assume(this.jpegBuffer != null && this.jpegBuffer.Length < 1,
                                "No JPEG image is associated with this instance");

                return this.jpegBuffer.Length;
            }
        }

        /// <summary>
        /// Gets the JPEG colourspace.
        /// </summary>
        public Colourspace JpegColourspace { get; private set; }

        /// <summary>
        /// Associate the JPEG image of length <code>imageSize</code> bytes stored in  <code>jpegImage</code> with this
        /// decompressor instance.  This image will be used as the source image for subsequent decompress operations.
        /// </summary>
        /// <param name="jpegImage"> JPEG image buffer</param>
        public void SetJpegImage(byte[] jpegImage)
        {
            Contract.Requires(jpegImage != null);

            this.jpegBuffer = jpegImage;
            int width;
            int height;
            Subsampling chroma;
            Colourspace colourspace;

            if (NativeMethods.decompressHeader(this.Handle,
                                                  this.jpegBuffer,
                                                  this.jpegBuffer.Length,
                                                  out width,
                                                  out height,
                                                  out chroma,
                                                  out colourspace) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(NativeMethods.getErrorMessage()));
            }

            this.jpegWidth = width;
            this.jpegHeight = height;
            this.jpegSubsampling = chroma;
            this.JpegColourspace = colourspace;
        }

        /// <summary>
        /// Returns the width of the largest scaled-down image that the TurboJPEG decompressor can generate without
        /// exceeding the desired image width and height.
        /// </summary>
        /// <param name="desiredWidth">
        /// Desired width (in pixels) of the decompressed image. Setting this to 0 is the same as
        /// setting it to the width of the JPEG image (in other words, the width will not be considered when determining the scaled
        /// image size)
        /// </param>
        /// <param name="desiredHeight">
        /// Desired height (in pixels) of the decompressed image. Setting this to 0 is the same as
        /// setting it to the height of the JPEG image (in other words, the height will not be considered when determining the
        /// scaled image size)
        /// </param>
        /// <returns>
        /// The width of the largest scaled-down image that the TurboJPEG decompressor can generate without exceeding the
        /// desired image width and height
        /// </returns>
        public int GetScaledWidth(int desiredWidth, int desiredHeight)
        {
            Contract.Requires(desiredWidth >= 0, "desiredWidth must be non-negative");
            Contract.Requires(desiredHeight >= 0, "desiredHeight must be non-negative");

            Contract.Assume(this.jpegWidth > 0, "No JPEG image is associated with this instance");
            Contract.Assume(this.jpegHeight > 0, "No JPEG image is associated with this instance");

            var scalingFactors = TurboJpegInterop.ScalingFactors;

            if (desiredWidth == 0)
            {
                desiredWidth = this.jpegWidth;
            }

            if (desiredHeight == 0)
            {
                desiredHeight = this.jpegHeight;
            }

            var scaledWidth = this.jpegWidth;
            var scaledHeight = this.jpegHeight;
            foreach (var scalingFactor in scalingFactors)
            {
                scaledWidth = scalingFactor.GetScaled(this.jpegWidth);
                scaledHeight = scalingFactor.GetScaled(this.jpegHeight);
                if (scaledWidth <= desiredWidth && scaledHeight <= desiredHeight)
                {
                    break;
                }
            }

            if (scaledWidth > desiredWidth || scaledHeight > desiredHeight)
            {
                throw new Exception("Could not scale to desired image dimensions");
            }

            return scaledWidth;
        }

        /// <summary>
        /// Returns the height of the largest scaled-down image that the TurboJPEG decompressor can generate without
        /// exceeding the desired image width and height.
        /// </summary>
        /// <param name="desiredWidth">Desired width (in pixels) of the decompressed image. Setting this to 0 is the
        /// same as setting it to the width of the JPEG image (in other words, the width will not be considered when
        /// determining the scaled image size).</param>
        /// <param name="desiredHeight">Desired height (in pixels) of the decompressed image. Setting this to 0 is the
        /// same as setting it to the height of the JPEG image (in other words, the height will not be considered when
        /// determining the scaled image size).</param>
        /// <returns>
        /// The height of the largest scaled-down image that the TurboJPEG decompressor can generate without exceeding
        /// the desired image width and height.
        /// </returns>
        public virtual int GetScaledHeight(int desiredWidth, int desiredHeight)
        {
            Contract.Requires(desiredWidth >= 0, "desiredWidth must be non-negative");
            Contract.Requires(desiredHeight >= 0, "desiredHeight must be non-negative");

            Contract.Assume(this.jpegWidth > 0, "No JPEG image is associated with this instance");
            Contract.Assume(this.jpegHeight > 0, "No JPEG image is associated with this instance");

            var scalingFactors = TurboJpegInterop.ScalingFactors;
            if (desiredWidth == 0)
            {
                desiredWidth = this.jpegWidth;
            }

            if (desiredHeight == 0)
            {
                desiredHeight = this.jpegHeight;
            }

            int scaledWidth = this.jpegWidth, scaledHeight = this.jpegHeight;
            foreach (var scalingFactor in scalingFactors)
            {
                scaledWidth = scalingFactor.GetScaled(this.jpegWidth);
                scaledHeight = scalingFactor.GetScaled(this.jpegHeight);
                if (scaledWidth <= desiredWidth && scaledHeight <= desiredHeight)
                {
                    break;
                }
            }

            if (scaledWidth > desiredWidth || scaledHeight > desiredHeight)
            {
                throw new Exception("Could not scale to desired image dimensions");
            }
            return scaledHeight;
        }

        /// <summary>
        /// Decompress the JPEG source image associated with this decompressor instance and output a decompressed image to
        /// the given destination buffer.
        /// </summary>
        /// <param name="desiredWidth">The desired width (in pixels) of the decompressed image.  If the desired image
        /// dimensions are different than the dimensions of the JPEG image being decompressed, then TurboJPEG will use
        /// scaling in the JPEG decompressor to generate the largest possible image that will fit within the desired
        /// dimensions. Setting this to 0 is the same as setting it to the width of the JPEG image; in other words the
        /// width will not be considered when determining the scaled image size).
        /// </param>
        /// <param name="pitch">The number of bytes per line of the destination image.  Normally, this should be set to
        /// <code>scaledWidth * <see cref="TurboJpegUtilities.GetPixelSize" />(pixelFormat)</code> if the decompressed
        /// image is unpadded, but you can use this to, for instance, pad each line of the decompressed image to a
        /// 4-byte boundary or to decompress the JPEG image into a region of a larger image. NOTE: <code>scaledWidth</code>
        /// can be determined by calling
        /// <code>scalingFactor.<seealso cref="TurboJpegScalingFactor#getScaled getScaled" />(jpegWidth)</code> or by
        /// calling <seealso cref="GetScaledWidth" />. Setting this parameter to 0 is the equivalent of setting it to
        /// <code>scaledWidth * <see cref="TurboJpegUtilities.GetPixelSize" />(pixelFormat)</code>.</param>
        /// <param name="desiredHeight">
        /// The desired Height (in pixels) of the decompressed image (or image region.)  If the desired
        /// image dimensions are different than the dimensions of the JPEG image being decompressed, then TurboJPEG will use
        /// scaling in the JPEG decompressor to generate the largest possible image that will fit within the desired dimensions.
        /// Setting this to 0 is the same as setting it to the Height of the JPEG image (in other words, the Height will not be
        /// considered when determining the scaled image size.)
        /// </param>
        /// <param name="pixelFormat">
        /// pixel format of the decompressed/decoded image (one of
        /// <seealso cref="TurboJpegUtilities#Rgb TurboJpegUtilities.PF_*" />)
        /// </param>
        /// <param name="flags">
        /// the bitwise OR of one or more of
        /// <seealso cref="TurboJpegUtilities#BottomUp TurboJpegUtilities.FLAG_*" />
        /// </param>
        public TurboJpegBuffer Decompress(int desiredWidth,
                                          int pitch,
                                          int desiredHeight,
                                          PixelFormat pixelFormat,
                                          TurboJpegFlags flags)
        {
            Contract.Requires(desiredWidth >= 0, "desiredWidth must be non-negative");
            Contract.Requires(desiredHeight >= 0, "desiredHeight must be non-negative");
            Contract.Requires(pitch >= 0, "pitch must be non-negative");
            Contract.Requires(Enum.IsDefined(typeof(TurboJpegFlags), flags));
            Contract.Ensures(Contract.Result<TurboJpegBuffer>()
                                 .BufferSize > 0,
                             "output buffer must have non-zero size");

            Contract.Assume(this.jpegBuffer != null, "No JPEG image is associated with this instance");

            var pixelSize = TurboJpegUtilities.GetPixelSize(pixelFormat);
            var scaledWidth = this.GetScaledWidth(desiredWidth, desiredHeight);
            var scaledHeight = this.GetScaledHeight(desiredWidth, desiredHeight);
            if (pitch == 0)
            {
                pitch = scaledWidth * pixelSize;
            }

            var bufferSize = pitch * scaledHeight;

            // having allocated memory with the libjpeg-turbo allocator, we must ensure that we release it with the 
            // matching deallocator lest Bad Things happen. Unlike compress, where the initial buffer size is a best 
            // guess, we know the dimensions of the uncompressed image and the number of bits per pixel and so sizing 
            // it appropriately is trivial and the buffer will never be reallocated
            using(var buffer = new TurboJpegSafeHandle(NativeMethods.alloc(bufferSize)))
            {
                var ptr = buffer.DangerousGetHandle();
                if (NativeMethods.decompress(this.Handle,
                                                this.jpegBuffer,
                                                this.jpegBuffer.Length,
                                                ptr,
                                                desiredWidth,
                                                pitch,
                                                desiredHeight,
                                                pixelFormat,
                                                flags) != 0)
                {
                    throw new Exception(TurboJpegInterop.GetLastError());
                }

                // we now have the result in a buffer on the unmanaged heap. 
                return new TurboJpegBuffer(ptr, bufferSize);
            }
        }

        /// <summary>
        /// Decompress the JPEG source image associated with this decompressor instance and output a YUV planar image to the given
        /// destination buffer. This method performs JPEG decompression but leaves out the colour conversion step, so a planar YUV
        /// image is generated instead of an RGB image.  The padding of the planes in this image is the same as in the images
        /// generated by <seealso cref="TurboJpegCompressor#EncodeYuv(byte[], int)" />.
        /// <para>
        /// NOTE: Technically, the JPEG format uses the YCbCr colourspace, but per the convention of the digital video
        /// community, the TurboJPEG API uses "YUV" to refer to an image format consisting of Y, Cb, and Cr image planes.
        /// </para>
        /// </summary>
        /// <param name="flags">The <see cref="TurboJpegFlags" /> controlling decompression.</param>
        public TurboJpegBuffer DecompressToYuv(TurboJpegFlags flags)
        {
            Contract.Requires(Enum.IsDefined(typeof(TurboJpegFlags), flags));
            Contract.Ensures(Contract.Result<TurboJpegBuffer>()
                                 .BufferSize > 0,
                             "output buffer must have non-zero size");

            Contract.Assume(this.jpegBuffer != null, "No JPEG image is associated with this instance");

            var bufferSize = NativeMethods.bufSizeYUV(this.jpegWidth, 4, this.jpegHeight, this.jpegSubsampling);
            using(var buffer = new TurboJpegSafeHandle(NativeMethods.alloc(bufferSize)))
            {
                var ptr = buffer.DangerousGetHandle();
                if (NativeMethods.decompressToYUV(this.Handle,
                                                     this.jpegBuffer,
                                                     this.jpegBuffer.Length,
                                                     ref ptr,
                                                     this.jpegWidth,
                                                     4,
                                                     this.jpegHeight,
                                                     flags) != 0)
                {
                    throw new Exception(TurboJpegInterop.GetLastError());
                }

                // we now have the result in a buffer on the unmanaged heap. 
                return new TurboJpegBuffer(ptr, bufferSize);
            }
        }

        /// <summary>Free the native structures associated with this decompressor instance.</summary>
        public virtual void Close()
        {
            if (this.Handle != IntPtr.Zero)
            {
                this.Dispose();
            }
        }
    }
}