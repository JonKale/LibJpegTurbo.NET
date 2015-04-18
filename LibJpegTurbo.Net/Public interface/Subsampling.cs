namespace LibJpegTurbo.Net
{
    /// <summary>
    /// Specifies the allowed chroma subsampling formats.
    /// </summary>
    public enum Subsampling {
        /// <summary>
        /// 4:4:4 chrominance subsampling (no chrominance subsampling).
        /// <para>The JPEG or YUV image will contain one chrominance component for every pixel in the source image.</para>
        /// </summary>
        Chroma444 = 0,

        /// <summary>
        /// 4:2:2 chrominance subsampling.
        /// <para>The JPEG or YUV image will contain one chrominance component for every 2x1 block of pixels in the 
        /// source image.</para>
        /// </summary>
        Chroma422 = 1,

        /// <summary>
        /// 4:2:0 chrominance subsampling.
        /// <para>The JPEG or YUV image will contain one chrominance component for every 2x2 block of pixels in the 
        /// source image.</para>
        /// </summary>
        Chroma420 = 2,

        /// <summary>
        /// Grayscale. 
        /// <para>The JPEG or YUV image will contain no chrominance components.</para>
        /// </summary>
        Gray = 3,

        /// <summary>
        /// 4:4:0 chrominance subsampling. 
        /// <para>The JPEG or YUV image will contain one chrominance component for every 1x2 block of pixels in the 
        /// source image.</para>
        /// </summary>
        /// <remarks>Note that 4:4:0 subsampling is not fully accelerated in libjpeg-turbo.</remarks>
        Chroma440 = 4,

        /// <summary>
        /// 4:1:1 chrominance subsampling. 
        /// <para>The JPEG or YUV image will contain one chrominance component for every 4x1 block of pixels in the 
        /// source image. JPEG images compressed with 4:1:1 subsampling will be almost exactly the same size as those 
        /// compressed with 4:2:0 subsampling, and in the aggregate, both subsampling methods produce approximately the 
        /// same perceptual quality. However, 4:1:1 is better able to reproduce sharp horizontal features.</para>
        /// </summary>
        /// <remarks>Note that 4:1:1 subsampling is not fully accelerated in libjpeg-turbo.</remarks>
        Chroma411 = 5
    }
}