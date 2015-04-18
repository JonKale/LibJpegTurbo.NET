namespace LibJpegTurbo.Net.InteropTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Net;

    [TestClass]
    public class MemoryAllocatorTests
    {
        [TestMethod]
        public void AllocAndRelease()
        {
            var pointerToMemory = TurboJpegInterop.alloc(1048576);
            Assert.IsTrue(pointerToMemory != IntPtr.Zero);
            TurboJpegInterop.free(pointerToMemory);
        }

        [TestMethod]
        public void AllocAndCleanUp()
        {
            var pointerToMemory = TurboJpegInterop.alloc(1048576);
            using (var handle = new TurboJpegSafeHandle(pointerToMemory))
            {
                Assert.IsFalse(handle.IsInvalid);
                handle.Dispose();
                Assert.IsTrue(handle.IsClosed);
            }
        }
    }
}
