namespace LibJpegTurbo.Net
{
    /// <summary>
    /// Specifies the allowable pixel formats.
    /// </summary>
    public enum PixelFormat 
    {
        /// <summary>
        /// RGB pixel format. The red green and blue components in the image are stored in 3-byte pixels in the order
        /// R G B from lowest to highest byte address within each pixel.
        /// </summary>
        Rgb = 0,

        /// <summary>
        /// BGR pixel format. The red green and blue components in the image are stored in 3-byte pixels in the order
        /// B G R from lowest to highest byte address within each pixel.
        /// </summary>
        Bgr = 1,

        /// <summary>
        /// RGBX pixel format. The red green and blue components in the image are stored in 4-byte pixels in the order
        /// R G B from lowest to highest byte address within each pixel. The X component is ignored when compressing 
        /// and undefined when decompressing.
        /// </summary>
        Rgbx = 2,

        /// <summary>
        /// BGRX pixel format. The red green and blue components in the image are stored in 4-byte pixels in the order
        /// B G R from lowest to highest byte address within each pixel. The X component is ignored when compressing 
        /// and undefined when decompressing.
        /// </summary>
        Bgrx = 3,

        /// <summary>
        /// XBGR pixel format. The red green and blue components in the image are stored in 4-byte pixels in the order
        /// R G B from highest to lowest byte address within each pixel. The X component is ignored when compressing 
        /// and undefined when decompressing.
        /// </summary>
        Xbgr = 4,

        /// <summary>
        /// XRGB pixel format. The red green and blue components in the image are stored in 4-byte pixels in the order
        /// B G R from highest to lowest byte address within each pixel. The X component is ignored when compressing 
        /// and undefined when decompressing.
        /// </summary>
        Xrgb = 5,

        /// <summary>Grayscale pixel format. Each 1-byte pixel represents a luminance (brightness) level from 0 to 255.</summary>
        Gray = 6,

        /// <summary>
        /// RGBA pixel format. This is the same as <see cref="Rgbx" /> except that when decompressing the X byte is 
        /// guaranteed to be 0xFF which can be interpreted as an opaque alpha channel.
        /// </summary>
        Rgba = 7,

        /// <summary>
        /// BGRA pixel format. This is the same as <see cref="Bgrx" /> except that when decompressing the X byte is 
        /// guaranteed to be 0xFF which can be interpreted as an opaque alpha channel.
        /// </summary>
        Bgra = 8,

        /// <summary>
        /// ABGR pixel format. This is the same as <see cref="Xbgr" /> except that when decompressing the X byte is 
        /// guaranteed to be 0xFF which can be interpreted as an opaque alpha channel.
        /// </summary>
        Abgr = 9,

        /// <summary>
        /// ARGB pixel format. This is the same as <see cref="Xrgb" /> except that when decompressing the X byte is 
        /// guaranteed to be 0xFF which can be interpreted as an opaque alpha channel.
        /// </summary>
        Argb = 10
    }
}