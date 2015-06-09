namespace LibJpegTurbo.Net
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>TurboJPEG utility class.</summary>
    public static class TurboJpegUtilities
    {
        /// <summary>
        /// The tile width for each chroma format (4:4:4, 4:2:2, 4:2:0, gray and 4:4:0).
        /// </summary>
        private static readonly int[] mcuWidth    = { 8, 16, 16, 8, 8 };

        /// <summary>
        /// The tile height for each chroma format (4:4:4, 4:2:2, 4:2:0, gray and 4:4:0).
        /// </summary>
        private static readonly int[] mcuHeight = { 8, 8, 16, 8, 16 };

        /// <summary>
        /// The pixel size for each pixel format (RGB, BGR, RGBx, BGRx, xBGR, xRGB, gray, RGBA, BGRA, ABGR, ARGB).
        /// </summary>
        private static readonly int[] pixelSize = { 3, 3, 4, 4, 4, 4, 1, 4, 4, 4, 4 };

        /// <summary>
        /// The offset of the red byte for each pixel format (as above).
        /// </summary>
        private static readonly int[] redOffset   = { 0, 2, 0, 2, 3, 1, 0, 0, 2, 3, 1 };

        /// <summary>
        /// The offset of the green byte for each pixel format (as above).
        /// </summary>
        private static readonly int[] greenOffset = { 1, 1, 1, 1, 2, 2, 0, 1, 1, 2, 2 };

        /// <summary>
        /// The offset of the blue byte for each pixel format (as above).
        /// </summary>
        private static readonly int[] blueOffset = { 2, 0, 2, 0, 1, 3, 0, 2, 0, 1, 3 };

        /// <summary>
        /// Returns the Minimum Coded Unit block ("tile") width associated with the specified chroma subsampling.
        /// </summary>
        /// <param name="subsampling">The chroma subsampling.</param>
        /// <returns>The MCU block width.</returns>
        public static int GetMcuWidth(this Subsampling subsampling)
        {
            Contract.Requires(Enum.IsDefined(typeof(Subsampling), subsampling));
            Contract.Assume((int)subsampling < LibJpegTurbo.Net.TurboJpegUtilities.mcuWidth.Length);

            return TurboJpegUtilities.mcuWidth[(int)subsampling];
        }

        /// <summary>Returns the MCU block height for the specified chroma subsampling.</summary>
        /// <param name="subsampling">The chroma subsampling.</param>
        /// <returns>The MCU block height.</returns>
        public static int GetMcuHeight(this Subsampling subsampling)
        {
            Contract.Requires(Enum.IsDefined(typeof(Subsampling), subsampling));
            Contract.Assume((int)subsampling < LibJpegTurbo.Net.TurboJpegUtilities.mcuHeight.Length);

            return TurboJpegUtilities.mcuHeight[(int)subsampling];
        }

        /// <summary>Returns the pixel size (in bytes) for the specified pixel format.</summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>The pixel size (in bytes).</returns>
        public static int GetPixelSize(this PixelFormat pixelFormat)
        {
            Contract.Requires(Enum.IsDefined(typeof(PixelFormat), pixelFormat));
            Contract.Assume((int)pixelFormat < LibJpegTurbo.Net.TurboJpegUtilities.pixelSize.Length);

            return TurboJpegUtilities.pixelSize[(int)pixelFormat];
        }

        /// <summary>
        /// For the specified pixel format, returns the number of bytes that the red component is offset from the 
        /// start of the pixel. For instance, if a pixel of format <see cref="PixelFormat.Bgrx"/> is stored in 
        /// <code>char pixel[]</code>, then the red component will be 
        /// <code>pixel[TurboJpegUtilities.GetRedOffset(TurboJpegUtilities.Bgrx)]</code>.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>The red offset for the pixel format </returns>
        public static int GetRedOffset(this PixelFormat pixelFormat)
        {
            Contract.Requires(Enum.IsDefined(typeof(PixelFormat), pixelFormat));
            Contract.Assume((int)pixelFormat < LibJpegTurbo.Net.TurboJpegUtilities.redOffset.Length);

            return TurboJpegUtilities.redOffset[(int)pixelFormat];
        }

        /// <summary>
        /// For the specified pixel format, returns the number of bytes that the green component is offset from the 
        /// start of the pixel. For instance, if a pixel of format <see cref="PixelFormat.Bgrx"/> is stored in 
        /// <code>char pixel[]</code>, then the green component will be 
        /// <code>pixel[TurboJpegUtilities.GetGreenOffset(TurboJpegUtilities.Bgrx)]</code>.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>The green offset for the pixel format </returns>
        public static int GetGreenOffset(this PixelFormat pixelFormat)
        {
            Contract.Requires(Enum.IsDefined(typeof(PixelFormat), pixelFormat));
            Contract.Assume((int)pixelFormat < LibJpegTurbo.Net.TurboJpegUtilities.greenOffset.Length);

            return TurboJpegUtilities.greenOffset[(int)pixelFormat];
        }

        /// <summary>
        /// For the specified pixel format, returns the number of bytes that the blue component is offset from the 
        /// start of the pixel. For instance, if a pixel of format <see cref="PixelFormat.Bgrx"/> is stored in 
        /// <code>char pixel[]</code>, then the blue component will be 
        /// <code>pixel[TurboJpegUtilities.GetBlueOffset(TurboJpegUtilities.Bgrx)]</code>.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>The blue offset for the pixel format </returns>
        public static int GetBlueOffset(this PixelFormat pixelFormat)
        {
            Contract.Requires(Enum.IsDefined(typeof(PixelFormat), pixelFormat));
            Contract.Assume((int) pixelFormat < LibJpegTurbo.Net.TurboJpegUtilities.blueOffset.Length);

            return TurboJpegUtilities.blueOffset[(int)pixelFormat];
        }

        /// <summary>
        /// Pads the specified value to the nearest 32-bit boundary.
        /// </summary>
        /// <param name="value">The value to pad.</param>
        /// <returns>The padded value.</returns>
        public static int Pad(this int value) 
        {
            return (value + 3) & (~3);
        }
    }
}