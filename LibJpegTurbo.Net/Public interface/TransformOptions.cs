namespace LibJpegTurbo.Net
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>Specifies the options available when using a <see cref="TurboJpegTransform" />.</summary>
    [Flags]
    public enum TransformOptions
    {
        /// <summary>
        /// This option will cause <see name="TurboJpegInterop.transform()" /> to throw an exception if the transform 
        /// is not perfect. Lossless transforms operate on Minimum Coded Unit blocks ("tiles") whose size depends on 
        /// the chroma subsampling used. If the image's width or height is not evenly divisible by the MCU block size 
        /// (see <see cref="TurboJpegUtilities.GetMcuWidth" /> and <see cref="TurboJpegUtilities.GetMcuHeight" />) 
        /// then there will be partial MCU blocks on the right and/or bottom edges. It is not possible to move these 
        /// partial MCU blocks to the top or left of the image so any transform that would require that is "imperfect."  
        /// If this option is not specified then any partial MCU blocks that cannot be transformed will be left in 
        /// place which will create odd-looking strips on the right or bottom edge of the image.
        /// </summary>
        Perfect = 1,

        /// <summary>
        /// This option will discard any partial MCU blocks that cannot be transformed.
        /// </summary>
        Trim = 2,

        /// <summary>
        /// This option will enable lossless cropping.
        /// </summary>
        Crop = 4,

        /// <summary>
        /// This option will discard the colour data in the input image and produce a grayscale output image.
        /// </summary>
        Gray = 8,

        /// <summary>
        /// This option will prevent <see name="TurboJpegInterop.transform()" /> from outputting a JPEG image for 
        /// this particular transform. This can be used in conjunction with a custom filter to capture the transformed 
        /// DCT coefficients without transcoding them.
        /// </summary>
        NoOutput = 16
    }
}