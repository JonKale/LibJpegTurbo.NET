namespace LibJpegTurbo.Net
{
    // ReSharper disable once InconsistentNaming

    /// <summary>
    /// Specifies the JPEG colourspaces
    /// </summary>
    public enum Colourspace
    {
        /// <summary>
        /// RGB colourspace.
        /// </summary>
        /// <remarks>
        /// When compressing the JPEG image, the R, G, and B components in the source image are reordered into image 
        /// planes, but no colourspace conversion or subsampling is performed.  RGB JPEG images can be decompressed to 
        /// any of the extended RGB pixel formats or grayscale, but they cannot be decompressed to YUV images.
        /// </remarks>
        Rgb,

        /// <summary>
        /// YCbCr colourspace.
        /// </summary>
        /// <remarks>
        /// YCbCr is not an absolute colourspace but rather a mathematical transformation of RGB designed solely for 
        /// storage and transmission. YCbCr images must be converted to RGB before they can actually be displayed. In 
        /// the YCbCr colourspace, the Y (luminance) component represents the black & white portion of the original 
        /// image, and the Cb and Cr (chrominance) components represent the colour portion of the original image. 
        /// Originally, the analogue equivalent of this transformation allowed the same signal to drive both black & 
        /// white and colour televisions, but JPEG images use YCbCr primarily because it allows the colour data to be 
        /// optionally subsampled for the purposes of reducing bandwidth or disk space. YCbCr is the most common JPEG 
        /// colourspace, and YCbCr JPEG images can be compressed from and decompressed to any of the extended RGB pixel 
        /// formats or grayscale, or they can be decompressed to YUV planar images.
        /// </remarks>
        YCbCr,

        /// <summary>
        /// The grayscale colourspace.
        /// </summary>
        /// <remarks>
        /// The JPEG image retains only the luminance data (Y component), and any colour data from the source image is 
        /// discarded. Grayscale JPEG images can be compressed from and decompressed to any of the extended RGB pixel 
        /// formats or grayscale, or they can be decompressed to YUV planar images.
        /// </remarks>
        Grayscale,

        /// <summary>
        /// The CMYK colourspace.
        /// </summary>
        /// <remarks>
        /// When compressing the JPEG image, the C, M, Y, and K components in the source image are reordered into image 
        /// planes, but no colourspace conversion or subsampling is performed. CMYK JPEG images can only be 
        /// decompressed to CMYK pixels.
        /// </remarks>
        Cmyk,

        /// <summary>
        /// The YCbCrK (aka YCCK) colourspace.
        /// </summary>
        /// <remarks>
        /// YCbCrK is not an absolute colourspace but rather a mathematical transformation of CMYK designed solely for 
        /// storage and transmission. It is to CMYK as YCbCr is to RGB. CMYK pixels can be reversibly transformed into 
        /// YCbCrK, and as with YCbCr, the chrominance components in the YCCK pixels can be subsampled without 
        /// incurring major perceptual loss. YCbCrK JPEG images can only be compressed from and decompressed to CMYK 
        /// pixels.
        /// </remarks>
        YCbCrK
    }
}