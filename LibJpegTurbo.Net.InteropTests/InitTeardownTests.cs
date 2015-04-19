using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibJpegTurbo.Net.InteropTests
{
    [TestClass]
    public class InitTeardownTests
    {
        [TestMethod]
        public void CreateDestroyCompressor()
        {
            var handle = new TurboJpegSafeHandle(TurboJpegInterop.initCompressor());
            Assert.IsFalse(handle.IsInvalid);
            handle.Dispose();
            Assert.IsTrue(handle.IsClosed);
        }

        [TestMethod]
        public void CreateDestroyDecompressor()
        {
            var handle = new TurboJpegSafeHandle(TurboJpegInterop.initDecompressor());
            Assert.IsFalse(handle.IsInvalid);
            handle.Dispose();
            Assert.IsTrue(handle.IsClosed);
        }

        [TestMethod]
        public void CreateDestroyTranmsformer()
        {
            var handle = new TurboJpegSafeHandle(TurboJpegInterop.initTransformer());
            Assert.IsFalse(handle.IsInvalid);
            handle.Dispose();
            Assert.IsTrue(handle.IsClosed);
        }
    }
}
