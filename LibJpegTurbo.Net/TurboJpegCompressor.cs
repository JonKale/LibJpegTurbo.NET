﻿namespace LibJpegTurbo.Net
{
    #region Using Directives

    using System;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    /// TurboJPEG compressor
    /// </summary>
    public sealed class TurboJpegCompressor : TurboJpegBase
    {
        #region Constants and Fields

        private const string NoAssocError = "No source image is associated with this instance";

        private int jpegQuality = -1;

        private byte[] sourceBuffer;

        private int sourceHeight;

        private int sourcePitch;

        private PixelFormat sourcePixelFormat;

        private int sourceWidth;

        private int sourceX = -1;

        private int sourceY = -1;

        #endregion

        #region Constructors and Destructors

        /// <summary>Create a TurboJPEG compressor instance.</summary>
        public TurboJpegCompressor()
            : base(TurboJpegInterop.initCompressor())
        {
        }

        /// <summary>
        /// Create a TurboJPEG compressor instance and associate the uncompressed source image stored in
        /// <code>sourceImage</code> with the newly created instance.
        /// </summary>
        /// <param name="sourceImage">Image buffer containing RGB or grayscale pixels to be compressed or encoded.</param>
        /// <param name="x">X offset (in pixels) of the region in the source image from which the JPEG or YUV image 
        /// should be compressed/encoded.</param>
        /// <param name="y">Y offset (in pixels) of the region in the source image from which the JPEG or YUV image 
        /// should be compressed/encoded.</param>
        /// <param name="width">Width (in pixels) of the region in the source image from which the JPEG or YUV image 
        /// should be compressed/encoded.</param>
        /// <param name="pitch">Bytes per line of the source image. Normally, this should be 
        /// <code>width * <see cref="TurboJpegUtilities.GetPixelSize"/>(<paramref name="pixelFormat"/>)</code> if the 
        /// source image is unpadded, but you can use this parameter to, for instance, specify that the scanlines in 
        /// the source image are padded to a 4-byte boundary or to compress/encode a JPEG or YUV image from a region 
        /// of a larger source image. You can also be clever and use this parameter to skip lines, etc. Setting this 
        /// parameter to 0 is the equivalent of setting it to 
        /// <code>width * <see cref="TurboJpegUtilities.GetPixelSize"/>(<paramref name="pixelFormat"/>)</code>.
        /// </param>
        /// <param name="height">Height (in pixels) of the region in the source image from which the JPEG or YUV image 
        /// should be compressed/encoded.</param>
        /// <param name="pixelFormat">Pixel format of the source image. </param>
        public TurboJpegCompressor(byte[] sourceImage,
                                   int x,
                                   int y,
                                   int width,
                                   int pitch,
                                   int height,
                                   PixelFormat pixelFormat)
            : base(TurboJpegInterop.initCompressor())
        {
            this.SetSourceImage(sourceImage, x, y, width, pitch, height, pixelFormat);
        }

        #endregion

        #region Public Properties

        /// <summary>Returns the size of the image (in bytes) generated by the most recent compress/encode operation.</summary>
        /// <returns> the size of the image (in bytes) generated by the most recent compress/encode operation </returns>
        public int CompressedSize { get; private set; }

        /// <summary>Set the JPEG image quality level for subsequent compress operations (1 to 100, 1 = worst, 100 = best)</summary>
        public int JpegQuality
        {
            set
            {
                if (value < 1 || value > 100)
                {
                    throw new ArgumentOutOfRangeException("value", "JpegQuality must be between 1 and 100");
                }

                this.jpegQuality = value;
            }
        }

        /// <summary>Get or set the level of chrominance subsampling for subsequent compress/encode operations.</summary>
        public Subsampling Subsampling { get; set; }

        #endregion

        #region Public Methods

        /// <summary>Free the native structures associated with this compressor instance.</summary>
        public void Close()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Compress the uncompressed source image associated with this compressor instance and output a
        /// <see cref="TurboJpegBuffer" /> pointing to a JPEG image in memory.
        /// </summary>
        /// <param name="compressionOptions">Flags controlling compression.</param>
        /// <returns>A <see cref="TurboJpegBuffer"/> pointing to a JPEG image.</returns>
        /// <exception cref="System.InvalidOperationException">The source image has not been set.</exception>
        public TurboJpegBuffer Compress(TurboJpegFlags compressionOptions)
        {
            if (this.sourceBuffer == null)
            {
                throw new InvalidOperationException(NoAssocError);
            }

            var bufferSize =
                (ulong) TurboJpegInterop.bufSize(this.sourceWidth, this.sourceHeight, (int) this.Subsampling);
                
            // having allocated memory with the libjpeg-turbo allocator, we must ensure that we release it with the 
            // matching deallocator lest Bad Things happen
            var buffer = TurboJpegInterop.alloc((int) bufferSize);
            try
            {
                if (TurboJpegInterop.compress(this.Handle,
                                                this.sourceBuffer,
                                                this.sourceWidth,
                                                this.sourcePitch,
                                                this.sourceHeight,
                                                (int)this.sourcePixelFormat,
                                                ref buffer,
                                                ref bufferSize,
                                                (int)this.Subsampling,
                                                this.jpegQuality,
                                                (int)compressionOptions) != 0)
                {
                    throw new Exception(Marshal.PtrToStringAnsi(TurboJpegInterop.getErrorMessage()));
                }

                // we now have the result in a buffer on the unmanaged heap. It may have moved from the original, 
                // but libjpeg-turbo has handled the reallocation for us
                return new TurboJpegBuffer(buffer, (int)bufferSize);
            }
            catch
            {
                TurboJpegInterop.free(buffer);
                throw;
            }
        }

        /// <summary>
        /// Encode the uncompressed source image associated with this compressor instance and output a YUV planar 
        /// image to the given destination buffer. This method uses the accelerated color conversion routines in 
        /// TurboJPEG's underlying codec to produce a planar YUV image that is suitable for direct video display.  
        /// Specifically, if the chroma components are subsampled along the horizontal dimension, then the width of 
        /// the luma plane is padded to the nearest multiple of 2 in the output image (same goes for the height of the 
        /// luma plane, if the chroma components are subsampled along the vertical dimension.) Also, each line of each 
        /// plane in the output image is padded to 4 bytes. Although this will work with any subsampling option, it is 
        /// really only useful in combination with <see cref="P:Subsampling.Chroma420" />, which produces an image 
        /// compatible with the I420 (AKA "YUV420P") format.
        /// <para>
        /// NOTE: Technically, the JPEG format uses the YCbCr colorspace but following the convention of the digital 
        /// videocommunity (who, it's broadly admitted, didn't know anywhere near enough about video when they 
        /// invented digital video), the TurboJPEG API uses "YUV" to refer to an image format consisting of Y, Cb, 
        /// and Cr image planes.
        /// </para>
        /// </summary>
        /// <param name="destinationBuffer">A buffer that will receive the YUV planar image. Use
        /// <see cref="TurboJpegInterop.bufSizeYUV" /> to determine the appropriate size for this buffer based on the 
        /// image width, height and chroma subsampling.</param>
        /// <param name="compressionOptions">Set of flags controlling compression.</param>
        public void EncodeYuv(byte[] destinationBuffer, TurboJpegFlags compressionOptions)
        {
            if (destinationBuffer == null)
            {
                throw new ArgumentNullException("destinationBuffer");
            }

            if (this.sourceBuffer == null)
            {
                throw new InvalidOperationException(NoAssocError);
            }

            TurboJpegInterop.encodeYUV(this.Handle,
                                       this.sourceBuffer,
                                       this.sourceWidth,
                                       this.sourcePitch,
                                       this.sourceHeight,
                                       (int) this.sourcePixelFormat,
                                       destinationBuffer,
                                       (int) this.Subsampling,
                                       (int) compressionOptions);
            this.CompressedSize = TurboJpegInterop.bufSizeYUV(this.sourceWidth, this.sourceHeight, (int)this.Subsampling);
        }

        /// <summary>
        /// Encode the uncompressed source image associated with this compressor instance and return a buffer containing a
        /// YUV planar image. See <see cref="EncodeYuv(byte[], TurboJpegFlags)" /> for more detail.
        /// </summary>
        /// <param name="compressionOptions">Set of flags controlling compression.</param>
        /// <returns> a buffer containing a YUV planar image </returns>
        public byte[] EncodeYuv(TurboJpegFlags compressionOptions)
        {
            if (this.sourceWidth < 1 || this.sourceHeight < 1)
            {
                throw new InvalidOperationException(NoAssocError);
            }

            var buf = new byte[TurboJpegInterop.bufSizeYUV(this.sourceWidth, this.sourceHeight, (int)this.Subsampling)];
            this.EncodeYuv(buf, compressionOptions);
            return buf;
        }

        /// <summary>
        /// Associate an uncompressed source image with this compressor instance.
        /// </summary>
        /// <param name="sourceImage">Image buffer containing RGB or grayscale pixels to be compressed or encoded.</param>
        /// <param name="x">X offset (in pixels) of the region in the source image from which the JPEG or YUV image 
        /// should be compressed/encoded.</param>
        /// <param name="y">Y offset (in pixels) of the region in the source image from which the JPEG or YUV image 
        /// should be compressed/encoded.</param>
        /// <param name="width">Width (in pixels) of the region in the source image from which the JPEG or YUV image 
        /// should be compressed/encoded.</param>
        /// <param name="pitch">Bytes per line of the source image. Normally, this should be 
        /// <code>width * <see cref="TurboJpegUtilities.GetPixelSize"/>(<paramref name="pixelFormat"/>)</code> if the 
        /// source image is unpadded, but you can use this parameter to, for instance, specify that the scanlines in 
        /// the source image are padded to a 4-byte boundary or to compress/encode a JPEG or YUV image from a region 
        /// of a larger source image. You can also be clever and use this parameter to skip lines, etc. Setting this 
        /// parameter to 0 is the equivalent of setting it to 
        /// <code>width * <see cref="TurboJpegUtilities.GetPixelSize"/>(<paramref name="pixelFormat"/>)</code>.
        /// </param>
        /// <param name="height">Height (in pixels) of the region in the source image from which the JPEG or YUV image 
        /// should be compressed/encoded.</param>
        /// <param name="pixelFormat">Pixel format of the source image.</param>
        public void SetSourceImage(byte[] sourceImage,
                                   int x,
                                   int y,
                                   int width,
                                   int pitch,
                                   int height,
                                   PixelFormat pixelFormat)
        {
            if (sourceImage == null)
            {
                throw new ArgumentNullException("sourceImage");
            }

            if (x < 0)
            {
                throw new ArgumentOutOfRangeException("x", "x must be non-negative");
            }

            if (y < 0)
            {
                throw new ArgumentOutOfRangeException("y", "y must be non-negative");
            }

            if (width < 1)
            {
                throw new ArgumentOutOfRangeException("width", "width must be greater than zero");
            }

            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("height", "height must be greater than zero");
            }

            if (pitch < 0)
            {
                throw new ArgumentOutOfRangeException("pitch", "pitch must be non-negative");
            }

            this.sourceBuffer = sourceImage;
            this.sourceWidth = width;
            this.sourcePitch = pitch == 0 ? width * TurboJpegUtilities.GetPixelSize(pixelFormat) : pitch;
            this.sourceHeight = height;
            this.sourcePixelFormat = pixelFormat;
            this.sourceX = x;
            this.sourceY = y;
        }

        [Obsolete("Use SetSourceImage(sbyte[], int, int, int, int, int, PixelFormat)")]
        public void SetSourceImage(byte[] sourceImage, int width, int pitch, int height, PixelFormat pixelFormat)
        {
            this.SetSourceImage(sourceImage, 0, 0, width, pitch, height, pixelFormat);
            this.sourceX = this.sourceY = -1;
        }

        #endregion
    }
}