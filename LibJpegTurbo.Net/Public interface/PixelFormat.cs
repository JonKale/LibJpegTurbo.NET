namespace LibJpegTurbo.Net
{
    /// <summary>
    /// Specifies the allowable pixel formats.
    /// </summary>
    public enum PixelFormat
    {
        /// <summary>
        /// RGB pixel format.
        /// </summary>
        /// <remarks>The red green and blue components in the image are stored in 3-byte pixels in the order R G B from
        /// lowest to highest byte address within each pixel.</remarks>
        Rgb = 0,

        /// <summary>
        /// BGR pixel format.
        /// </summary>
        /// <remarks>The red green and blue components in the image are stored in 3-byte pixels in the order B G R from
        /// lowest to highest byte address within each pixel.</remarks>
        Bgr = 1,

        /// <summary>
        /// RGBX pixel format.
        /// </summary>
        /// <remarks>The red green and blue components in the image are stored in 4-byte pixels in the order
        /// R G B from lowest to highest byte address within each pixel. The X component is ignored when compressing
        /// and undefined when decompressing.</remarks>
        Rgbx = 2,

        /// <summary>
        /// BGRX pixel format.
        /// </summary>
        /// <remarks>The red green and blue components in the image are stored in 4-byte pixels in the order
        /// B G R from lowest to highest byte address within each pixel. The X component is ignored when compressing
        /// and undefined when decompressing.</remarks>
        Bgrx = 3,

        /// <summary>
        /// XBGR pixel format.
        /// </summary>
        /// <remarks>The red green and blue components in the image are stored in 4-byte pixels in the order
        /// R G B from highest to lowest byte address within each pixel. The X component is ignored when compressing
        /// and undefined when decompressing.</remarks>
        Xbgr = 4,

        /// <summary>
        /// XRGB pixel format.
        /// </summary>
        /// <remarks>The red green and blue components in the image are stored in 4-byte pixels in the order
        /// B G R from highest to lowest byte address within each pixel. The X component is ignored when compressing
        /// and undefined when decompressing.</remarks>
        Xrgb = 5,

        /// <summary>
        /// Grayscale pixel format.
        /// </summary>
        /// <remarks>Each 1-byte pixel represents a luminance (brightness) level from 0 to 255.</remarks>
        Gray = 6,

        /// <summary>
        /// RGBA pixel format.
        /// </summary>
        /// <remarks>This is the same as <see cref="Rgbx" /> except that when decompressing the X byte is guaranteed to 
        /// be 0xFF which can be interpreted as an opaque alpha channel.</remarks>
        Rgba = 7,

        /// <summary>
        /// BGRA pixel format.
        /// </summary>
        /// <remarks>This is the same as <see cref="Bgrx" /> except that when decompressing the X byte is guaranteed to 
        /// be 0xFF which can be interpreted as an opaque alpha channel.</remarks>
        Bgra = 8,

        /// <summary>
        /// ABGR pixel format.
        /// </summary>
        /// <remarks>This is the same as <see cref="Xbgr" /> except that when decompressing the X byte is guaranteed to 
        /// be 0xFF which can be interpreted as an opaque alpha channel.</remarks>
        Abgr = 9,

        /// <summary>
        /// ARGB pixel format.
        /// </summary>
        /// <remarks>This is the same as <see cref="Xrgb" /> except that when decompressing the X byte is guaranteed to be
        /// 0xFF which can be interpreted as an opaque alpha channel.</remarks>
        Argb = 10,

        /// <summary>
        /// CMYK pixel format.
        /// </summary>
        /// <remarks>Unlike RGB, which is an additive colour model used primarily for display, CMYK 
        /// (Cyan/Magenta/Yellow/Key) is a subtractive colour model used primarily for printing. In the CMYK colour 
        /// model, the value of each colour component typically corresponds to an amount of cyan, magenta, yellow, or 
        /// black ink that is applied to a white background. In order to convert between CMYK and RGB, it is necessary 
        /// to use a colour management system (CMS.) A CMS will attempt to map colours within the printer's gamut to 
        /// perceptually similar colours in the display's gamut and vice versa, but the mapping is typically not 1:1 or 
        /// reversible, nor can it be defined with a simple formula. Thus, such a conversion is out of scope for a 
        /// codec library. However, the TurboJPEG API allows for compressing CMYK pixels into a YCbCrK JPEG image 
        /// <see cref="Colourspace.YCbCrK"/> and decompressing YCbCrK JPEG images into CMYK pixels.</remarks>
        Cmyk = 11
    }
}