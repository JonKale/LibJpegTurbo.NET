namespace LibJpegTurbo.Net
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    /// Wraps all the interop goop - p/invoke declarations and associated faffle.
    /// </summary>
    internal static class TurboJpegInterop
    {
        /// <summary>
        /// The scaling factors. We grab these eagerly and cache them.
        /// </summary>
        private static readonly TurboJpegScalingFactor[] scalingFactors;

        /// <summary>
        /// The array of pixel sizes, one per pixel format.
        /// </summary>
        private static readonly Dictionary<PixelFormat, int> pixelSizes;

        /// <summary>
        /// The array of pixel sizes, one per pixel format.
        /// </summary>
        private static readonly Dictionary<PixelFormat, int> redOffsets;

        /// <summary>
        /// The array of pixel sizes, one per pixel format.
        /// </summary>
        private static readonly Dictionary<PixelFormat, int> greenOffsets;

        /// <summary>
        /// The array of pixel sizes, one per pixel format.
        /// </summary>
        private static readonly Dictionary<PixelFormat, int> blueOffsets;

        /// <summary>
        /// The array of MCU block widths, one per level of chrominance subsampling.
        /// </summary>
        private static readonly Dictionary<Subsampling, int> mcuWidths;

        /// <summary>
        /// The array of MCU block heights, one per level of chrominance subsampling.
        /// </summary>
        private static readonly Dictionary<Subsampling, int> mcuHeights;

        /// <summary>
        /// Initializes the <see cref="TurboJpegInterop" /> class.
        /// </summary>
        static TurboJpegInterop()
        {
            int count;
            var buffer = getScalingFactors(out count);
            scalingFactors = buffer.ToTurboJpegScalingFactorArray(count);
            pixelSizes = new Dictionary<PixelFormat, int>
                         {
                             { PixelFormat.Rgb, 3 },
                             { PixelFormat.Bgr, 3 },
                             { PixelFormat.Rgbx, 4 },
                             { PixelFormat.Bgrx, 4 },
                             { PixelFormat.Xbgr, 4 },
                             { PixelFormat.Xrgb, 4 },
                             { PixelFormat.Gray, 1 },
                             { PixelFormat.Rgba, 4 },
                             { PixelFormat.Bgra, 4 },
                             { PixelFormat.Abgr, 4 },
                             { PixelFormat.Argb, 4 },
                             { PixelFormat.Cmyk, 4 },
                         };
            redOffsets =  new Dictionary<PixelFormat, int>
                         {
                             { PixelFormat.Rgb,  0 },
                             { PixelFormat.Bgr,  2 },
                             { PixelFormat.Rgbx, 0 },
                             { PixelFormat.Bgrx, 2 },
                             { PixelFormat.Xbgr, 3 },
                             { PixelFormat.Xrgb, 1 },
                             { PixelFormat.Gray, 0 },
                             { PixelFormat.Rgba, 0 },
                             { PixelFormat.Bgra, 2 },
                             { PixelFormat.Abgr, 3 },
                             { PixelFormat.Argb, 1 },
                             { PixelFormat.Cmyk, -1 },
                         };
            greenOffsets = new Dictionary<PixelFormat, int>
                         {
                             { PixelFormat.Rgb,  1 },
                             { PixelFormat.Bgr,  1 },
                             { PixelFormat.Rgbx, 1 },
                             { PixelFormat.Bgrx, 1 },
                             { PixelFormat.Xbgr, 2 },
                             { PixelFormat.Xrgb, 2 },
                             { PixelFormat.Gray, 0 },
                             { PixelFormat.Rgba, 1 },
                             { PixelFormat.Bgra, 1 },
                             { PixelFormat.Abgr, 2 },
                             { PixelFormat.Argb, 2 },
                             { PixelFormat.Cmyk, -1 },
                         };
            blueOffsets = new Dictionary<PixelFormat, int>
                         {
                             { PixelFormat.Rgb,  2 },
                             { PixelFormat.Bgr,  0 },
                             { PixelFormat.Rgbx, 2 },
                             { PixelFormat.Bgrx, 0 },
                             { PixelFormat.Xbgr, 1 },
                             { PixelFormat.Xrgb, 3 },
                             { PixelFormat.Gray, 0 },
                             { PixelFormat.Rgba, 2 },
                             { PixelFormat.Bgra, 0 },
                             { PixelFormat.Abgr, 1 },
                             { PixelFormat.Argb, 3 },
                             { PixelFormat.Cmyk, -1 },
                         };
            mcuWidths = new Dictionary<Subsampling, int>
                        {
                            { Subsampling.Chroma444, 8 },
                            { Subsampling.Chroma422, 16 },
                            { Subsampling.Chroma420, 16 },
                            { Subsampling.Gray, 8 },
                            { Subsampling.Chroma440, 8 },
                            { Subsampling.Chroma411, 32 }
                        };
            mcuHeights = new Dictionary<Subsampling, int>
                        {
                            { Subsampling.Chroma444, 8 },
                            { Subsampling.Chroma422, 8 },
                            { Subsampling.Chroma420, 16 },
                            { Subsampling.Gray, 8 },
                            { Subsampling.Chroma440, 16 },
                            { Subsampling.Chroma411, 8 }
                        };
        }

        /// <summary>
        /// Gets the pixel size (in bytes) for a given pixel format. 
        /// </summary>
        /// <returns>The pixel size dictionary.</returns>
        public static ReadOnlyDictionary<PixelFormat, int> PixelSize
        {
            get { return new ReadOnlyDictionary<PixelFormat, int>(pixelSizes);}
        }

        /// <summary>
        /// Gets the red offset (in bytes) for a given pixel format. 
        /// </summary>
        /// <remarks>
        /// This specifies the number of bytes that the red component is offset from the start of the pixel. For 
        /// instance, if a pixel of format <see cref="PixelFormat.Bgrx"/> is stored in byte[] pixel, then the red 
        /// component will be pixel[<see cref="RedOffset"/>[<see cref="PixelFormat.Bgrx"/>]]. 
        /// </remarks>
        /// <returns>The red offset.</returns>
        public static ReadOnlyDictionary<PixelFormat, int> RedOffset
        {
            get { return new ReadOnlyDictionary<PixelFormat, int>(redOffsets); }
        }

        /// <summary>
        /// Gets the green offset (in bytes) for a given pixel format. 
        /// </summary>
        /// <remarks>
        /// This specifies the number of bytes that the green component is offset from the start of the pixel. For 
        /// instance, if a pixel of format <see cref="PixelFormat.Bgrx"/> is stored in byte[] pixel, then the green 
        /// component will be pixel[<see cref="GreenOffset"/>[<see cref="PixelFormat.Bgrx"/>]]. 
        /// </remarks>
        /// <returns>The green offset.</returns>
        public static ReadOnlyDictionary<PixelFormat, int> GreenOffset
        {
            get { return new ReadOnlyDictionary<PixelFormat, int>(greenOffsets); }
        }

        /// <summary>
        /// Gets the blue offset (in bytes) for a given pixel format. 
        /// </summary>
        /// <remarks>
        /// This specifies the number of bytes that the blue component is offset from the start of the pixel. For 
        /// instance, if a pixel of format <see cref="PixelFormat.Bgrx"/> is stored in byte[] pixel, then the blue 
        /// component will be pixel[<see cref="BlueOffset"/>[<see cref="PixelFormat.Bgrx"/>]]. 
        /// </remarks>
        /// <returns>The blue offset.</returns>
        public static ReadOnlyDictionary<PixelFormat, int> BlueOffset
        {
            get { return new ReadOnlyDictionary<PixelFormat, int>(blueOffsets); }
        }

        /// <summary>
        /// Gets the MCU block width (in pixels) for a given level of chrominance subsampling.
        /// </summary>
        public static ReadOnlyDictionary<Subsampling, int> McuWidth
        {
            get {  return new ReadOnlyDictionary<Subsampling, int>(mcuWidths); }
        }

        /// <summary>
        /// Gets the MCU block height (in pixels) for a given level of chrominance subsampling.
        /// </summary>
        public static ReadOnlyDictionary<Subsampling, int> McuHeight
        {
            get { return new ReadOnlyDictionary<Subsampling, int>(mcuHeights); }
        }

        /// <summary>
        /// Gets the scaling factors.
        /// </summary>
        public static TurboJpegScalingFactor[] ScalingFactors
        {
            get { return (TurboJpegScalingFactor[]) scalingFactors.Clone(); }
        }

        /// <summary>
        /// Gets the last error message.
        /// </summary>
        /// <returns>A descriptive error message explaining why the last command failed.</returns>
        public static string GetLastError()
        {
            var messageBuffer = getErrorMessage();
            return Marshal.PtrToStringAnsi(messageBuffer);
        }

        #region Methods

        /// <summary>
        /// Allocate an image buffer for use with TurboJPEG.
        /// <para>You should always use this function to allocate the JPEG destination buffer(s) for <see cref="compress" />
        /// and <see cref="transform" /> unless you are disabling automatic buffer (re)allocation (by setting
        /// <see cref="TurboJpegFlags.NoReallocation" />).</para>
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>A pointer to the buffer.</returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjAlloc", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr alloc(int size);

        /// <summary>
        /// Returns the maximum size of the buffer (in bytes) required to hold a JPEG image with the specified width,
        /// height and chroma subsampling.
        /// <para>The number of bytes returned by this function is larger than the size of the uncompressed source
        /// image. The reason for this is that the JPEG format uses 16-bit coefficients, and it is thus possible for a very
        /// high-quality JPEG image with very high-frequency content to expand rather than compress when converted to
        /// the JPEG format. Such images represent a very rare corner case, but since there is no way to predict the
        /// size of a JPEG image prior to compression, the corner case has to be handled.</para>
        /// </summary>
        /// <param name="width">The width (in pixels) of the JPEG image.</param>
        /// <param name="height">The height (in pixels) of the JPEG image.</param>
        /// <param name="jpegSubsamp">The chroma subsampling to be used when generating the JPEG image.</param>
        /// <returns>
        /// The maximum size of the buffer (in bytes) required to hold a JPEG image with the given width, height, and
        /// chroma subsampling.
        /// </returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjBufSize", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int bufSize(int width, int height, [MarshalAs(UnmanagedType.I4)]Subsampling jpegSubsamp);

        /// <summary>
        /// Returns the size of the buffer (in bytes) required to hold a YUV planar image with the specified width,
        /// height and chroma subsampling.
        /// </summary>
        /// <param name="width">The width of the YUV image in pixels.</param>
        /// <param name="pad">The width of each line in each plane of the image is padded to the nearest multiple of
        /// this number of bytes (must be a power of 2).</param>
        /// <param name="height">The height of the YUV image in pixels.</param>
        /// <param name="subsamp">The chroma subsampling used in the YUV image.</param>
        /// <returns>
        /// The size of the buffer (in bytes) required to hold a YUV planar image with the given width, height and
        /// chroma subsampling.
        /// </returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjBufSizeYUV", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int bufSizeYUV(int width,
                                              int pad,
                                              int height,
                                              [MarshalAs(UnmanagedType.I4)] Subsampling subsamp);

        /// <summary>
        /// Compresses an RGB or grayscale image into a JPEG image.
        /// </summary>
        /// <param name="handle">A handle to a TurboJPEG compressor or transformer instance.</param>
        /// <param name="sourceBuffer">An image buffer containing RGB or grayscale pixels to be compressed.</param>
        /// <param name="width">The width (in pixels) of the source image.</param>
        /// <param name="pitch">The bytes per line of the source image. Normally, this should be
        /// <paramref name="width" /> * <see cref="TurboJpegUtilities.GetPixelSize" />(<paramref name="pixelFormat" />)
        /// if the image is unpadded, or <see cref="TurboJpegUtilities.Pad" />(<paramref name="width" /> *
        /// <see cref="TurboJpegUtilities.GetPixelSize" />(<paramref name="pixelFormat" />)) if each line of the image is
        /// padded to the nearest 32-bit boundary, as is the case for Windows bitmaps. You can also be clever and use
        /// this parameter to skip lines, etc. Setting this parameter to 0 is the equivalent of setting it to
        /// <paramref name="width" /> * <see cref="TurboJpegUtilities.GetPixelSize" />(<paramref name="pixelFormat" />).
        /// </param>
        /// <param name="height">The height (in pixels) of the source image.</param>
        /// <param name="pixelFormat">The pixel format of the source image.</param>
        /// <param name="destinationBuffer">A pointer to an image buffer that will receive the JPEG image. TurboJPEG
        /// has the ability to reallocate the JPEG buffer to accommodate the size of the JPEG image. Thus, you can
        /// choose to:
        /// <list type="number">
        ///     <item>pre-allocate the JPEG buffer with an arbitrary size using <see cref="alloc" /> and let TurboJPEG
        ///     grow the buffer as needed, or</item>
        ///     <item>set <paramref name="destinationBuffer" /> to NULL to tell TurboJPEG to allocate the buffer for you,
        ///     or</item>
        ///     <item>pre-allocate the buffer to a "worst case" size determined by calling <see cref="bufSize" />. The
        ///     buffer should never be re-allocated; to ensure that it is not use <see cref="TurboJpegFlags.NoReallocation" />
        ///     and if the buffer is invalid or too small an error will be generated.</item>
        /// </list>
        /// If you choose option 1, <paramref name="bufferSize" /> should be set to the size of your pre-allocated
        /// buffer. Unless you have set <see cref="TurboJpegFlags.NoReallocation" />, you should always check
        /// <paramref name="destinationBuffer" /> upon return from this function, as it may have changed.</param>
        /// <param name="bufferSize">Size of the compressed image. If <paramref name="destinationBuffer" /> points to a
        /// pre-allocated buffer, then <paramref name="bufferSize" /> should be set to the size of the buffer. Upon
        /// return, <paramref name="bufferSize" /> will contain the size of the JPEG image (in bytes).</param>
        /// <param name="jpegSubsamp">The chroma subsampling to be used when generating the JPEG image.</param>
        /// <param name="jpegQual">The image quality of the generated JPEG image (1 = worst, 100 = best).</param>
        /// <param name="flags">The flags.</param>
        /// <returns>0 on success, or -1 if an error occurred.</returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjCompress2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int compress(IntPtr handle,
                                            byte[] sourceBuffer,
                                            int width,
                                            int pitch,
                                            int height,
                                            [MarshalAs(UnmanagedType.I4)] PixelFormat pixelFormat,
                                            ref IntPtr destinationBuffer,
                                            ref ulong bufferSize,
                                            [MarshalAs(UnmanagedType.I4)] Subsampling jpegSubsamp,
                                            int jpegQual,
                                            [MarshalAs(UnmanagedType.I4)] TurboJpegFlags flags);


        /// <summary>
        /// Decompresses a JPEG image to an RGB or grayscale image.
        /// </summary>
        /// <param name="handle">A handle to a TurboJPEG decompressor or transformer instance.</param>
        /// <param name="sourceBuffer">The buffer containing the JPEG image to decompress.</param>
        /// <param name="size">The size of the JPEG image (in bytes).</param>
        /// <param name="destinationBuffer">An image buffer that will receive the decompressed image. This buffer
        /// should normally be <paramref name="pitch" /> * <c>scaledHeight</c> bytes in size, where <c>scaledHeight</c>
        /// can be determined by calling <see cref="TurboJpegScalingFactor.GetScaled" /> with the JPEG image height and
        /// one of the scaling factors in <see cref="TurboJpegScalingFactor.ScalingFactors" />.
        /// <paramref name="destinationBuffer" /> may also be used to decompress into a specific region of a larger
        /// buffer.</param>
        /// <param name="desiredWidth">The desired width (in pixels) of the destination image. If this is different
        /// than the width of the JPEG image being decompressed, then TurboJPEG will use scaling in the JPEG
        /// decompressor to generate the largest possible image that will fit within the desired width. If width
        /// <paramref name="desiredWidth" /> is set to 0, then only the height will be considered when determining the
        /// scaled image size.</param>
        /// <param name="pitch">The number of bytes per line of the destination image. Normally, this is
        /// <c>scaledWidth</c> * <see cref="TurboJpegUtilities.GetPixelSize" />(<paramref name="pixelFormat" />) if the
        /// decompressed image is unpadded, else <see cref="TurboJpegUtilities.Pad" />(<c>scaledWidth</c> *
        /// <see cref="TurboJpegUtilities.GetPixelSize" />(<paramref name="pixelFormat" />)) if each line of the
        /// decompressed image is padded to the nearest 32-bit boundary, as is the case for Windows bitmaps.
        /// (NOTE: <c>scaledWidth</c> can be determined by calling <see cref="TurboJpegScalingFactor.GetScaled" /> with
        /// the JPEG image width and one of the scaling factors in <see cref="TurboJpegScalingFactor.ScalingFactors" />).
        /// You can also be clever and use the pitch parameter to skip lines, etc. Setting this parameter to 0 is the
        /// equivalent of setting it to <c>scaledWidth</c> * <see cref="TurboJpegUtilities.GetPixelSize" />(
        /// <paramref name="pixelFormat" />).</param>
        /// <param name="desiredHeight">The desired height (in pixels) of the destination image. If this is different
        /// than the height of the JPEG image being decompressed, then TurboJPEG will use scaling in the JPEG
        /// decompressor to generate the largest possible image that will fit within the desired height. If height is
        /// set to 0, then only the width will be considered when determining the scaled image size.</param>
        /// <param name="pixelFormat">The pixel format of the destination image.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>0 on success, or -1 if an error occurred.</returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjDecompress2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int decompress(IntPtr handle,
                                              [In] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] sourceBuffer,
                                              int size,
                                              IntPtr destinationBuffer,
                                              int desiredWidth,
                                              int pitch,
                                              int desiredHeight,
                                              [MarshalAs(UnmanagedType.I4)] PixelFormat pixelFormat,
                                              [MarshalAs(UnmanagedType.I4)] TurboJpegFlags flags);

        /// <summary>
        /// Retrieve information about a JPEG image without decompressing it.
        /// </summary>
        /// <param name="handle">A handle to a TurboJPEG decompressor or transformer instance.</param>
        /// <param name="sourceBuffer">A buffer containing a JPEG image.</param>
        /// <param name="size">The size of the image in bytes.</param>
        /// <param name="width">On return, contains the width of the image.</param>
        /// <param name="height">On return, contains the height of the image.</param>
        /// <param name="chroma">On return, contains the chroma subsampling format used when compressing the image.</param>
        /// <param name="colourspace">On return, contains the colourspace of the JPEG image.</param>
        /// <returns>
        /// 0 on success, or -1 if an error occurred.
        /// </returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjDecompressHeader3", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int decompressHeader(IntPtr handle,
                                                    [In] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] sourceBuffer,
                                                    int size,
                                                    out int width,
                                                    out int height,
                                                    [MarshalAs(UnmanagedType.I4)] out Subsampling chroma,
                                                    [MarshalAs(UnmanagedType.I4)] out Colourspace colourspace);

        /// <summary>
        /// Decompresses a JPEG image to a YUV planar image.
        /// </summary>
        /// <param name="handle">A handle to a TurboJPEG decompressor or transformer instance.</param>
        /// <param name="sourceBuffer">The source buffer containing the JPEG image to decompress.</param>
        /// <param name="size">The size of the JPEG image (in bytes).</param>
        /// <param name="destinationBuffer">The destination buffer that will receive the YUV image. Use
        /// <see cref="bufSizeYUV" /> to determine the appropriate size for this buffer based on the image width,
        /// height and chroma subsampling.</param>
        /// <param name="width">The desired width (in pixels) of the YUV image. If this is different than the width of 
        /// the JPEG image being decompressed, then TurboJPEG will use scaling in the JPEG decompressor to generate the 
        /// largest possible image that will fit within the desired width. If width is set to 0, then only the height 
        /// will be considered when determining the scaled image size. If the scaled width is not an even multiple of 
        /// the MCU block width (see tjMCUWidth), then an intermediate buffer copy will be performed within TurboJPEG.</param>
        /// <param name="pad">The width of each line in each plane of the YUV image will be padded to the nearest 
        /// multiple of this number of bytes (must be a power of 2.) To generate images suitable for X Video, <param name="pad" /> 
        /// should be set to 4.</param>
        /// <param name="height">The desired height (in pixels) of the YUV image. If this is different than the height 
        /// of the JPEG image being decompressed, then TurboJPEG will use scaling in the JPEG decompressor to generate 
        /// the largest possible image that will fit within the desired height. If height is set to 0, then only the 
        /// width will be considered when determining the scaled image size. If the scaled height is not an even 
        /// multiple of the MCU block height (see tjMCUHeight), then an intermediate buffer copy will be performed 
        /// within TurboJPEG.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>
        /// 0 on success, or -1 if an error occurred.
        /// </returns>
        /// <remarks>
        /// This function performs JPEG decompression but leaves out the colour conversion step, so a planar YUV image
        /// is generated instead of an RGB image. The padding of the planes in this image is the same as in the images
        /// generated by <see cref="encodeYUV" />. Note that if the width or height of the image is not an even multiple
        /// of the MCU block size (see <see cref="TurboJpegUtilities.GetMcuWidth" /> and
        /// <see cref="TurboJpegUtilities.GetMcuHeight" />), then an intermediate buffer copy will be performed within
        /// TurboJPEG.
        /// <para>NOTE: Technically, the JPEG format uses the YCbCr colourspace, but per the convention of the digital
        /// video community, the TurboJPEG API uses "YUV" to refer to an image format consisting of Y, Cb, and Cr image
        /// planes.</para>
        /// </remarks>
        [DllImport("turbojpeg.dll", EntryPoint = "tjDecompressToYUV2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int decompressToYUV(IntPtr handle,
                                                   byte[] sourceBuffer,
                                                   int size,
                                                   ref IntPtr destinationBuffer,
                                                   int width,
                                                   int pad,
                                                   int height,
                                                   [MarshalAs(UnmanagedType.I4)] TurboJpegFlags flags);

        /// <summary>
        /// Destroys a TurboJPEG compressor, decompressor, or transformer instance.
        /// </summary>
        /// <param name="handle">The handle to a TurboJPEG compressor, decompressor or transformer instance.</param>
        /// <returns>0 on success, or -1 if an error occurred.</returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjDestroy", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int destroy(IntPtr handle);

        /// <summary>
        /// Encodes an RGB or grayscale image into a YUV planar image.
        /// </summary>
        /// <remarks>
        /// This function uses the accelerated colour conversion routines in TurboJPEG's underlying codec to produce a
        /// planar YUV image that is suitable for X Video. Specifically, if the chrominance components are subsampled
        /// along the horizontal dimension, then the width of the luminance plane is padded to the nearest multiple of
        /// 2 in the output image (same goes for the height of the luminance plane, if the chrominance components are
        /// subsampled along the vertical dimension.) Also, each line of each plane in the output image is padded to 4
        /// bytes. Although this will work with any subsampling option, it is really only useful in combination with
        /// <see cref="Subsampling.Chroma420" />, which produces an image compatible with the I420 (AKA "YUV420P")
        /// format.
        /// <para>NOTE: Technically, the JPEG format uses the YCbCr colourspace, but per the convention of the digital
        /// video community, the TurboJPEG API uses "YUV" to refer to an image format consisting of Y, Cb, and Cr image
        /// planes.</para>
        /// </remarks>
        /// <param name="handle">A handle to a TurboJPEG compressor or transformer instance.</param>
        /// <param name="sourceBuffer">The source buffer containing RGB or grayscale pixels to be encoded.</param>
        /// <param name="width">The width (in pixels) of the source image.</param>
        /// <param name="pitch">The number of bytes per line of the destination image. Normally, this is
        /// <c>scaledWidth</c> * <see cref="TurboJpegUtilities.GetPixelSize" />(<paramref name="pixelFormat" />) if the
        /// decompressed image is unpadded, else <see cref="TurboJpegUtilities.Pad" />(<c>scaledWidth</c> *
        /// <see cref="TurboJpegUtilities.GetPixelSize" />(<paramref name="pixelFormat" />)) if each line of the
        /// decompressed image is padded to the nearest 32-bit boundary, as is the case for Windows bitmaps.
        /// You can also be clever and use the pitch parameter to skip lines, etc. Setting this parameter to 0 is the
        /// equivalent of setting it to <c>scaledWidth</c> * <see cref="TurboJpegUtilities.GetPixelSize" />(
        /// <paramref name="pixelFormat" />).</param>
        /// <param name="height">The height (in pixels) of the source image.</param>
        /// <param name="pixelFormat">The pixel format of the source image.</param>
        /// <param name="destinationBuffer">An image buffer that will receive the YUV image. Use <see cref="bufSizeYUV" />
        /// to determine the appropriate size for this buffer based on the image width, height, and chroma subsampling.
        /// </param>
        /// <param name="subsamp">The chroma subsampling to be used when generating the YUV image.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>0 on success, or -1 if an error occurred.</returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjEncodeYUV2", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int encodeYUV(IntPtr handle,
                                             byte[] sourceBuffer,
                                             int width,
                                             int pitch,
                                             int height,
                                             [MarshalAs(UnmanagedType.I4)] PixelFormat pixelFormat,
                                             byte[] destinationBuffer,
                                             [MarshalAs(UnmanagedType.I4)] Subsampling subsamp,
                                             [MarshalAs(UnmanagedType.I4)] TurboJpegFlags flags);

        /// <summary>
        /// Frees an image buffer previously allocated by TurboJPEG.
        /// <para>You should always use this function to free JPEG destination buffer(s) that were automatically
        /// (re)allocated by <see cref="compress(IntPtr, byte[], int, int, int, int, ref IntPtr, ref ulong, int, int, int)" />
        /// or <see cref="transform" /> or that were manually allocated using <see cref="alloc" />.</para>
        /// </summary>
        /// <param name="buffer">The address of the buffer to free.</param>
        [DllImport("turbojpeg.dll", EntryPoint = "tjFree", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void free(IntPtr buffer);

        /// <summary>
        /// Gets a descriptive error message explaining why the last command failed.
        /// </summary>
        /// <returns>A descriptive error message explaining why the last command failed.</returns>
        /// <remarks>The buffer is owned by TurboJpegLib and should not be released, freed or otherwise tampered with.</remarks>
        [DllImport("turbojpeg.dll", EntryPoint = "tjGetErrorStr", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr getErrorMessage();

        /// <summary>
        /// Returns a list of fractional scaling factors that the JPEG decompressor in this implementation of TurboJPEG
        /// supports.
        /// </summary>
        /// <param name="numScalingFactors">The number of scaling factors.</param>
        /// <returns>
        /// A list of the fractional scaling factors that the JPEG decompressor in this implementation of
        /// TurboJPEG supports.
        /// </returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjGetScalingFactors", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr getScalingFactors(out int numScalingFactors);

        /// <summary>
        /// Creates a TurboJPEG compressor instance.
        /// </summary>
        /// <returns>A handle to the newly-created instance, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjInitCompress", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr initCompressor();

        /// <summary>
        /// Creates a TurboJPEG decompressor instance.
        /// </summary>
        /// <returns>A handle to the newly-created instance, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjInitDecompress", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr initDecompressor();

        /// <summary>
        /// Creates a TurboJPEG transformer instance.
        /// </summary>
        /// <returns>A handle to the newly-created instance, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjInitTransform", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr initTransformer();

        /// <summary>
        /// Transforms the JPEG contained in the supplied source buffer.
        /// </summary>
        /// <param name="handle">A handle to a TurboJPEG transformer instance.</param>
        /// <param name="sourceBuffer">The source buffer containing the JPEG image to transform.</param>
        /// <param name="sourceSize">Size of the JPEG image (in bytes).</param>
        /// <param name="count">The number of transformed JPEG images to generate.</param>
        /// <param name="destinationBuffers">An array of <paramref name="count" /> image buffers.
        /// <paramref name="destinationBuffers" />[i] will receive a JPEG image that has been transformed using the
        /// parameters in <paramref name="transforms" />[i]. TurboJPEG has the ability to reallocate the JPEG buffer to
        /// accommodate the size of the JPEG image. Thus, you can choose to:
        /// <list type="number">
        ///     <item>pre-allocate the JPEG buffer with an arbitrary size using tjAlloc() and let TurboJPEG grow the buffer
        ///     as needed, or</item>
        ///     <item>set <paramref name="destinationBuffers" />[i] to <see cref="IntPtr.Zero" /> to tell TurboJPEG to
        ///     allocate the buffer for you, or</item>
        ///     <item>pre-allocate the buffers to a "worst case" size determined by calling <see cref="bufSize" />. The
        ///     buffers should never be re-allocated; to ensure that they are not use <see cref="TurboJpegFlags.NoReallocation" />
        ///     and if a buffer is invalid or too small an error will be generated.</item>
        /// </list>
        /// If you choose option 1, <paramref name="sizes" />[i] should be set to the size of your pre-allocated buffer.
        /// You should always check <paramref name="destinationBuffers" />[i] upon return from this function, as it may
        /// have changed.
        /// </param>
        /// <param name="sizes">An array of <paramref name="count" /> unsigned long that will receive the actual sizes
        /// (in bytes) of each transformed JPEG image. If <paramref name="destinationBuffers" />[i] points to a
        /// pre-allocated buffer, then <paramref name="sizes" />[i] should be set to the size of the buffer. Upon
        /// return, <paramref name="sizes" />[i] will contain the size of the JPEG image (in bytes).</param>
        /// <param name="transforms">An array of <paramref name="count" /> <see cref="TurboJpegTransform" /> structures,
        /// each of which specifies the transform parameters and/or cropping region for the corresponding transformed
        /// output image.
        /// </param>
        /// <param name="flags">The flags.</param>
        /// <returns>0 on success, or -1 if an error occurred.</returns>
        [DllImport("turbojpeg.dll", EntryPoint = "tjTransform", CallingConvention = CallingConvention.Cdecl)]
        internal static extern int transform(IntPtr handle,
                                             byte[] sourceBuffer,
                                             int sourceSize,
                                             int count,
                                             ref IntPtr[] destinationBuffers,
                                             ref ulong[] sizes,
                                             TurboJpegTransform[] transforms,
                                             [MarshalAs(UnmanagedType.I4)] TurboJpegFlags flags);

        #endregion
    }
}