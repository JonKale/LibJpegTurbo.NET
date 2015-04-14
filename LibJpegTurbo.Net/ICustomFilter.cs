namespace LibJpegTurbo.Net
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Drawing;

    #endregion

    /// <summary>Custom filter callback interface</summary>
    public interface ICustomFilter
    {
        #region Public Methods

        /// <summary>
        /// A callback function that can be used to modify the DCT coefficients after they are losslessly transformed 
        /// but before they are transcoded to a new JPEG image. This allows for custom filters or other 
        /// transformations to be applied in the frequency domain.
        /// </summary>
        /// <param name="coeffBuffer">
        /// A buffer containing transformed DCT coefficients. (NOTE: this buffer is not guaranteed to be valid once 
        /// the callback returns, so applications wishing to hand off the DCT coefficients to another function or 
        /// library should make a copy of them within the body of the callback).
        /// </param>
        /// <param name="bufferRegion">
        /// A <see cref="Rectangle"/> containing the width and height of <paramref name="coeffBuffer"/> as well as its 
        /// offset relative to the component plane. TurboJPEG implementations may choose to split each component plane 
        /// into multiple DCT coefficient buffers and call the callback function once for each buffer.
        /// </param>
        /// <param name="planeRegion">
        /// A <see cref="Rectangle"/> containing the width and height of the component plane to which 
        /// <paramref name="coeffBuffer"/> belongs.
        /// </param>
        /// <param name="componentId">
        /// ID number of the component plane to which <paramref name="coeffBuffer"/> belongs (Y, Cb, and Cr have, 
        /// respectively, ID's of 0, 1, and 2 in typical JPEG images).
        /// </param>
        /// <param name="transformId">
        /// ID number of the transformed image to which <paramref name="coeffBuffer"/> belongs. This is the same as 
        /// the index of the transform in the <code>transforms</code> array that was passed to 
        /// <see cref="TurboJpegTransformer.transform"/>.
        /// </param>
        /// <param name="transform">
        /// A <see cref="TurboJpegTransform" /> instance that specifies the parameters and/or cropping region for this 
        /// transform.
        /// </param>
        void CustomFilter(List<short> coeffBuffer,
                          Rectangle bufferRegion,
                          Rectangle planeRegion,
                          int componentId,
                          int transformId,
                          TurboJpegTransform transform);

        #endregion
    }
}