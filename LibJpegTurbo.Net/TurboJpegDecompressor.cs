namespace LibJpegTurbo.Net
{
    #region

    using System;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>TurboJPEG decompressor</summary>
    public class TurboJpegDecompressor : TurboJpegBase
    {
        private const string NoAssocError = "No JPEG image is associated with this instance";

        private byte[] jpegBuffer;
        public int JpegHeight { get; protected set; }
        public Subsampling JpegSubsampling { get; protected set; }
        public int JpegWidth { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegDecompressor"/> class with the specified object.
        /// </summary>
        /// <param name="handle">The handle.</param>
        protected TurboJpegDecompressor(IntPtr handle)
            : base(handle)
        {
        }
        
        /// <summary>Create a TurboJPEG decompresssor instance.</summary>
        public TurboJpegDecompressor()
            : base(TurboJpegInterop.initDecompressor())
        {
        }

        /// <summary>
        /// Create a TurboJPEG decompressor instance and associate the JPEG image stored in <code>jpegImage</code> with
        /// the newly created instance.
        /// </summary>
        /// <param name="jpegImage"> JPEG image buffer (size of the JPEG image is assumed to be the length of the array) </param>
        public TurboJpegDecompressor(byte[] jpegImage)
            : base(TurboJpegInterop.initDecompressor())
        {
            this.SetJpegImage(jpegImage);
        }

        /// <summary>Returns the width of the JPEG image associated with this decompressor instance.</summary>
        public int Width
        {
            get
            {
                if (this.JpegWidth < 1)
                {
                    throw new InvalidOperationException(NoAssocError);
                }

                return this.JpegWidth;
            }
        }

        /// <summary>Returns the height of the JPEG image associated with this decompressor instance.</summary>
        public int Height
        {
            get
            {
                if (this.JpegHeight < 1)
                {
                    throw new InvalidOperationException(NoAssocError);
                }

                return this.JpegHeight;
            }
        }

        /// <summary>
        /// Returns the level of chrominance subsampling used in the JPEG image associated with this decompressor
        /// instance.  See <see cref="Subsampling" />.
        /// </summary>
        public Subsampling Subsampling
        {
            get { return this.JpegSubsampling; }
        }

        /// <summary>Returns the JPEG image buffer associated with this decompressor instance.</summary>
        public byte[] JpegBuffer
        {
            get
            {
                if (this.jpegBuffer == null)
                {
                    throw new InvalidOperationException(NoAssocError);
                }

                return this.jpegBuffer;
            }
        }

        /// <summary>Returns the size of the JPEG image (in bytes) associated with this decompressor instance.</summary>
        public int JpegSize
        {
            get
            {
                if (this.jpegBuffer.Length < 1)
                {
                    throw new InvalidOperationException(NoAssocError);
                }

                return this.jpegBuffer.Length;
            }
        }

        /// <summary>
        /// Associate the JPEG image of length <code>imageSize</code> bytes stored in
        /// <code>jpegImage</code> with this decompressor instance.  This image will be used as the source image for subsequent
        /// decompress operations.
        /// </summary>
        /// <param name="jpegImage"> JPEG image buffer</param>
        public void SetJpegImage(byte[] jpegImage)
        {
            if (jpegImage == null)
            {
                throw new  ArgumentNullException("jpegImage");
            }

            this.jpegBuffer = jpegImage;
            int width;
            int height;
            int chroma;

            if (TurboJpegInterop.decompressHeader(this.Handle,
                                                  this.jpegBuffer,
                                                  this.jpegBuffer.Length,
                                                  out width,
                                                  out height,
                                                  out chroma) != 0)
            {
                throw new Exception(Marshal.PtrToStringAnsi(TurboJpegInterop.getErrorMessage()));
            }

            this.JpegWidth = width;
            this.JpegHeight = height;
            this.JpegSubsampling = (Subsampling) chroma;
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
            if (this.JpegWidth < 1 || this.JpegHeight < 1)
            {
                throw new InvalidOperationException(NoAssocError);
            }

            if (desiredWidth < 0 || desiredHeight < 0)
            {
                throw new Exception("Invalid argument in getScaledWidth()");
            }

            var scalingFactors = TurboJpegInterop.ScalingFactors;

            if (desiredWidth == 0)
            {
                desiredWidth = this.JpegWidth;
            }

            if (desiredHeight == 0)
            {
                desiredHeight = this.JpegHeight;
            }

            var scaledWidth = this.JpegWidth;
            var scaledHeight = this.JpegHeight;
            foreach (var scalingFactor in scalingFactors)
            {
                scaledWidth = scalingFactor.GetScaled(this.JpegWidth);
                scaledHeight = scalingFactor.GetScaled(this.JpegHeight);
                if (scaledWidth <= desiredWidth && scaledHeight <= desiredHeight)
                {
                    break;
                }
            }
            
            if (scaledWidth > desiredWidth || scaledHeight > desiredHeight)
            {
                throw new Exception("Could not scale down to desired image dimensions");
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
            if (this.JpegWidth < 1 || this.JpegHeight < 1)
            {
                throw new InvalidOperationException(NoAssocError);
            }

            if (desiredWidth < 0 || desiredHeight < 0)
            {
                throw new Exception("Invalid argument in GetScaledHeight()");
            }

            var scalingFactors = TurboJpegInterop.ScalingFactors;
            if (desiredWidth == 0)
            {
                desiredWidth = this.JpegWidth;
            }

            if (desiredHeight == 0)
            {
                desiredHeight = this.JpegHeight;
            }

            int scaledWidth = this.JpegWidth, scaledHeight = this.JpegHeight;
            foreach (var scalingFactor in scalingFactors)
            {
                scaledWidth = scalingFactor.GetScaled(this.JpegWidth);
                scaledHeight = scalingFactor.GetScaled(this.JpegHeight);
                if (scaledWidth <= desiredWidth && scaledHeight <= desiredHeight)
                {
                    break;
                }
            }
            
            if (scaledWidth > desiredWidth || scaledHeight > desiredHeight)
            {
                throw new Exception("Could not scale down to desired image dimensions");
            }
            return scaledHeight;
        }

        /// <summary>
        /// Decompress the JPEG source image associated with this decompressor instance and output a decompressed image to
        /// the given destination buffer.
        /// </summary>
        /// <param name="desiredWidth">The desired width (in pixels) of the decompressed image.  If the desired image
        ///  dimensions are different than the dimensions of the JPEG image being decompressed, then TurboJPEG will use 
        /// scaling in the JPEG decompressor to generate the largest possible image that will fit within the desired 
        /// dimensions. Setting this to 0 is the same as setting it to the width of the JPEG image; in other words the 
        /// width will not be considered when determining the scaled image size).
        /// </param>
        /// <param name="pitch">The number of bytes per line of the destination image.  Normally, this should be set to
        /// <code>scaledWidth * <see cref="TurboJpegUtilities.GetPixelSize"/>(pixelFormat)</code> if the decompressed 
        /// image is unpadded, but you can use this to, for instance, pad each line of the decompressed image to a 
        /// 4-byte boundary or to decompress the JPEG image into a region of a larger image. NOTE: <code>scaledWidth</code> 
        /// can be determined by calling 
        /// <code>scalingFactor.<seealso cref="TurboJpegScalingFactor#getScaled getScaled" />(jpegWidth)</code> or by 
        /// calling <seealso cref="GetScaledWidth" />. Setting this parameter to 0 is the equivalent of setting it to
        /// <code>scaledWidth * <see cref="TurboJpegUtilities.GetPixelSize"/>(pixelFormat)</code>.</param>
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
            if (this.jpegBuffer == null)
            {
                throw new InvalidOperationException(NoAssocError);
            }
            if (desiredWidth < 0 || pitch < 0 || desiredHeight < 0 || flags < 0)
            {
                throw new Exception("Invalid argument in Decompress()");
            }

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
            var buffer = TurboJpegInterop.alloc((int) bufferSize);
            try
            {
                if (TurboJpegInterop.decompress(this.Handle,
                                                this.jpegBuffer,
                                                this.jpegBuffer.Length,
                                                ref buffer,
                                                desiredWidth,
                                                pitch,
                                                desiredHeight,
                                                (int) pixelFormat,
                                                (int) flags) != 0)
                {
                    throw new Exception(Marshal.PtrToStringAnsi(TurboJpegInterop.getErrorMessage()));
                }

                // we now have the result in a buffer on the unmanaged heap. 
                return new TurboJpegBuffer(buffer, (int) bufferSize);
            }
            catch
            {
                TurboJpegInterop.free(buffer);
                throw;
            }
        }

        /// <summary>
        /// Decompress the JPEG source image associated with this decompressor instance and output a YUV planar image to the given
        /// destination buffer. This method performs JPEG decompression but leaves out the color conversion step, so a planar YUV
        /// image is generated instead of an RGB image.  The padding of the planes in this image is the same as in the images
        /// generated by <seealso cref="TurboJpegCompressor#EncodeYuv(byte[], int)" />.
        /// <para>
        /// NOTE: Technically, the JPEG format uses the YCbCr colorspace, but per the convention of the digital video
        /// community, the TurboJPEG API uses "YUV" to refer to an image format consisting of Y, Cb, and Cr image planes.
        /// </para>
        /// </summary>
        /// <param name="flags">The <see cref="TurboJpegFlags"/> controlling decompression.</param>
        public TurboJpegBuffer DecompressToYuv(TurboJpegFlags flags)
        {
            if (this.jpegBuffer == null)
            {
                throw new InvalidOperationException(NoAssocError);
            }

            if (flags < 0)
            {
                throw new Exception("Invalid argument in DecompressToYuv()");
            }

            var bufferSize = TurboJpegInterop.bufSizeYUV(this.JpegWidth, this.JpegHeight, (int) this.JpegSubsampling);
            var buffer = TurboJpegInterop.alloc(bufferSize);
            try
            {
                if (
                    TurboJpegInterop.decompressToYUV(this.Handle,
                                                     this.jpegBuffer,
                                                     this.jpegBuffer.Length,
                                                     ref buffer,
                                                     (int) flags) != 0)
                {
                    throw new Exception(Marshal.PtrToStringAnsi(TurboJpegInterop.getErrorMessage()));
                }

                // we now have the result in a buffer on the unmanaged heap. 
                return new TurboJpegBuffer(buffer, bufferSize);
            }
            catch
            {
                TurboJpegInterop.free(buffer);
                throw;
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