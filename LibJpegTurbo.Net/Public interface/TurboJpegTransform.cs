﻿namespace LibJpegTurbo.Net
{
    #region Using Directives

    using System.Drawing;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>Lossless transform parameters</summary>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct TurboJpegTransform
    {
        #region Constants and Fields

        /// <summary>
        /// The rectangle the transform applies to.
        /// </summary>
        private Rectangle rect;

        [MarshalAs(UnmanagedType.I4)]
        private TransformOperation operation;

        [MarshalAs(UnmanagedType.I4)]
        private TransformOptions options;

        private ICustomFilter customFilter;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Create a new lossless transform instance with the given parameters.
        /// </summary>
        /// <param name="x">The left boundary of the cropping region. This must be evenly divisible by the MCU block 
        /// width (see <see cref="TurboJpegUtilities.GetMcuWidth" />).</param>
        /// <param name="y">The upper boundary of the cropping region. This must be evenly divisible by the MCU block 
        /// height (see <see cref="TurboJpegUtilities.GetMcuHeight" />).</param>
        /// <param name="w">The width of the cropping region. Setting this to 0 is the equivalent of setting it to 
        /// (width of the source JPEG image - <paramref name="x"/>).</param>
        /// <param name="h">The height of the cropping region. Setting this to 0 is the equivalent of setting it to 
        /// (height of the source JPEG image - <paramref name="y"/>).</param>
        /// <param name="operation">The transform operation.</param>
        /// <param name="options">One or more of the transform options.</param>
        /// <param name="customFilter">An instance of an object that implements the <see cref="ICustomFilter"/> 
        /// interface, or <c>null</c> if no custom filter is needed.</param>
        public TurboJpegTransform(int x,
                                  int y,
                                  int w,
                                  int h,
                                  TransformOperation operation,
                                  TransformOptions options,
                                  ICustomFilter customFilter = null)
            : this(new Rectangle(x, y, w, h), operation, options, customFilter)
        {
        }

        /// <summary>Create a new lossless transform instance with the given parameters.</summary>
        /// <param name="r">A <see cref="Rectangle"/>> instance that specifies the cropping region. See 
        /// <see cref="TurboJpegTransform (int, int, int, int, TransformOperation, TransformOptions, ICustomFilter)"/> 
        /// for more detail.</param>
        /// <param name="operation">The transform operation.</param>
        /// <param name="options">One or more of the transform options.</param>
        /// <param name="customFilter">An instance of an object that implements the <see cref="ICustomFilter"/> 
        /// interface, or <c>null</c> if no custom filter is needed.</param>
        public TurboJpegTransform(Rectangle r,
                                  TransformOperation operation,
                                  TransformOptions options,
                                  ICustomFilter customFilter = null)
        {
            this.rect = r;
            this.operation = operation;
            this.options = options;
            this.customFilter = customFilter;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the custom filter instance</summary>
        public ICustomFilter CustomFilter
        {
            get { return this.customFilter; }
            private set { this.customFilter = value; }
        }

        /// <summary>Gets the height.</summary>
        public int Height { get { return this.rect.Height; } }

        /// <summary>Gets the transform operation.</summary>
        public TransformOperation Operation
        {
            get { return this.operation; }
            private set { this.operation = value; }
        }

        /// <summary>Gets the transform options.</summary>
        public TransformOptions Options
        {
            get { return this.options; }
            private set { this.options = value; }
        }

        /// <summary>Gets the width.</summary>
        public int Width { get { return this.rect.Width; } }

        #endregion
    }
}