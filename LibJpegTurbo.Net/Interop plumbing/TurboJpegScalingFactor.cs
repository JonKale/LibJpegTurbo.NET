namespace LibJpegTurbo.Net
{
    #region

    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>Fractional scaling factor.</summary>
    /// <remarks>Because System.Rational isn't a thing.</remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [DebuggerDisplay("{DebugString}")]
    internal struct TurboJpegScalingFactor
    {
        /// <summary>
        /// The numerator.
        /// </summary>
        private readonly int numerator;
        
        /// <summary>
        /// The denominator.
        /// </summary>
        private readonly int denominator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegScalingFactor" /> class.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="numerator"/> must be greater than 1 or
        /// <paramref name="denominator"/> must be greater than 1.</exception>
        public TurboJpegScalingFactor(int numerator, int denominator)
        {
            Contract.Requires(numerator > 0, "Numerator must be greater than 0");
            Contract.Requires(denominator > 0, "Denominator must be greater than 0");

            this.numerator = numerator;
            this.denominator = denominator;
        }

        /// <summary>
        /// Gets the scaling factors.
        /// </summary>
        public TurboJpegScalingFactor[] ScalingFactors 
        { 
            get { return (TurboJpegScalingFactor[]) TurboJpegInterop.ScalingFactors.Clone(); } 
        }

        /// <summary>Returns numerator </summary>
        public int Numerator
        {
            get { return this.numerator; }
        }

        /// <summary>Returns denominator </summary>
        public int Denominator
        {
            get { return this.denominator; }
        }

        /// <summary>Returns true or false, depending on whether this instance is equal to 1/1. </summary>
        public bool One
        {
            get { return this.numerator == 1 && this.denominator == 1; }
        }

        /// <summary>
        /// Gets the debugger display string.
        /// </summary>
        private string DebugString
        {
            get { return String.Format("{0}:{1}", this.numerator, this.denominator); }
        }

        /// <summary>
        /// Returns the scaled value of <paramref name="dimension"/>. This function performs the integer equivalent of
        /// <code>ceil(<paramref name="dimension"/> * <see cref="TurboJpegScalingFactor"/>)</code>. 
        /// </summary>
        public int GetScaled(int dimension)
        {
            Contract.Assume(dimension * this.numerator + this.denominator - 1 != Int32.MinValue ||
                            this.denominator != -1);

            return (dimension * this.numerator + this.denominator - 1) / this.denominator;
        }

        /// <summary>
        /// Returns true or false, depending on whether this instance and <paramref name="other"/> have the same 
        /// numerator and denominator. 
        /// </summary>
        public bool Equals(TurboJpegScalingFactor other)
        {
            return this.numerator == other.numerator && this.denominator == other.denominator;
        }
    }
}