namespace LibJpegTurbo.Net.InteropTests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetScalingFactorTests
    {
        [TestMethod]
        public void GetScalingFactors()
        {
            int count;
            var scalingFactorsPointer = TurboJpegInterop.getScalingFactors(out count);
            var scalingFactors = scalingFactorsPointer.ToTurboJpegScalingFactorArray(count);

            Assert.IsTrue(scalingFactors.Length == 16);
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(2, 1)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(15, 8)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(7, 4)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(13, 8)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(3, 2)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(11, 8)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(5, 4)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(9, 8)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(1, 1)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(7, 8)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(3, 4)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(5, 8)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(1, 2)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(3, 8)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(1, 4)));
            Assert.IsTrue(scalingFactors.Contains(new TurboJpegScalingFactor(1, 8)));
        }
    }
}