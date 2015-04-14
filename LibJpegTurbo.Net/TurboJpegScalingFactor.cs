namespace LibJpegTurbo.Net
{
    #region

    using System;

    #endregion

    /// <summary>Fractional scaling factor</summary>
    public struct TurboJpegScalingFactor
    {
        private readonly int num;
        private readonly int denom;

        /// <summary>
        /// Initializes a new instance of the <see cref="TurboJpegScalingFactor" /> class.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="numerator"/> must be greater than 1 or
        /// <paramref name="denominator"/> must be greater than 1.</exception>
        public TurboJpegScalingFactor(int numerator, int denominator)
        {
            if (numerator < 1)
            {
                throw new ArgumentOutOfRangeException("numerator", "Numerator must be greater than 1");
            }

            if (denominator < 1)
            {
                throw new ArgumentOutOfRangeException("denominator", "Denominator must be greater than 1");
            }

            num = numerator;
            denom = denominator;
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
            get { return this.num; }
        }

        /// <summary>Returns denominator </summary>
        public int Denominator
        {
            get { return this.denom; }
        }

        /// <summary>Returns true or false, depending on whether this instance is equal to 1/1. </summary>
        public bool One
        {
            get { return (this.Numerator == 1 && this.Denominator == 1); }
        }

        /// <summary>
        /// Returns the scaled value of <paramref name="dimension"/>. This function performs the integer equivalent of
        /// <code>ceil(<paramref name="dimension"/> * <see cref="TurboJpegScalingFactor"/>)</code>. 
        /// </summary>
        public int GetScaled(int dimension)
        {
            return (dimension * this.Numerator + this.Denominator - 1)/this.Denominator;
        }

        /// <summary>
        /// Returns true or false, depending on whether this instance and <paramref name="other"/> have the same 
        /// numerator and denominator. 
        /// </summary>
        public bool Equals(TurboJpegScalingFactor other)
        {
            return (this.Numerator == other.Numerator && this.Denominator == other.Denominator);
        }
    }
}