namespace LibJpegTurbo.Net.InteropTests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Net;

    [TestClass]
    public class MemoryAllocatorAndBufferTests
    {
        [TestMethod]
        public void AllocAndRelease()
        {
            var pointerToMemory = NativeMethods.alloc(1048576);
            Assert.IsTrue(pointerToMemory != IntPtr.Zero);
            NativeMethods.free(pointerToMemory);
        }

        [TestMethod]
        public void AllocAndSaveInBuffer()
        {
            const int bufsize = 1024;
            var deadbeef = new byte[] { 0xde, 0xad, 0xbe, 0xef };
            var pointerToMemory = NativeMethods.alloc(bufsize);
            using (var buffer = new TurboJpegBuffer(pointerToMemory, bufsize))
            {
                Assert.IsTrue(buffer.BufferSize == bufsize);
                pointerToMemory.Initialise(bufsize);
                var bufferData = buffer.ToArray();
                for (var i = 0; i < bufsize; ++i)
                {
                    Assert.IsTrue(bufferData[i] == deadbeef[i % 4]);
                }
            }
        }


    }
}
