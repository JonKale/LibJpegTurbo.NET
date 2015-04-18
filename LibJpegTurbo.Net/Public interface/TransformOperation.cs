namespace LibJpegTurbo.Net
{
    /// <summary>
    /// Specifies the available transform operations.
    /// </summary>
    public enum TransformOperation
    {
        /// <summary>
        /// Do not transform the position of the image pixels.
        /// </summary>
        None = 0,

        /// <summary>
        /// Flip (mirror) image horizontally. This transform is imperfect if there are any partial MCU blocks on the
        /// right edge.
        /// </summary>
        /// <seealso cref="TransformOptions" />
        HorizontalFlip = 1,

        /// <summary>
        /// Flip (mirror) image vertically. This transform is imperfect if there are any partial MCU blocks on the 
        /// bottom edge.
        /// </summary>
        /// <seealso cref="TransformOptions" />
        VerticalFlip = 2,

        /// <summary>
        /// Transpose image (flip/mirror along upper left to lower right axis). This transform is always perfect.
        /// </summary>
        /// <seealso cref="TransformOptions" />
        Transpose = 3,

        /// <summary>
        /// Transverse transpose image (flip/mirror along upper right to lower left axis). This transform is imperfect 
        /// if there are any partial MCU blocks in the image.
        /// </summary>
        /// <seealso cref="TransformOptions" />
        Transverse = 4,

        /// <summary>
        /// Rotate image clockwise by 90 degrees. This transform is imperfect if there are any partial MCU blocks on 
        /// the bottom edge.
        /// </summary>
        /// <seealso cref="TransformOptions" />
        Rotate90Clockwise = 5,

        /// <summary>
        /// Rotate image 180 degrees. This transform is imperfect if there are any partial MCU blocks in the image.
        /// </summary>
        /// <seealso cref="TransformOptions" />
        Rotate180 = 6,

        /// <summary>
        /// Rotate image anticlockwise by 90 degrees. This transform is imperfect if there are any partial MCU blocks
        /// on the right edge.
        /// </summary>
        /// <seealso cref="TransformOptions" />
        Rotate90Anticlockwise = 7,
    }
}