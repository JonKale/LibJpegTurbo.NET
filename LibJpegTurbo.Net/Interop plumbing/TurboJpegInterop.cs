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
            var buffer = NativeMethods.getScalingFactors(out count);
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
            var messageBuffer = NativeMethods.getErrorMessage();
            return Marshal.PtrToStringAnsi(messageBuffer);
        }
    }
}