namespace LibJpegTurbo.Net.InteropTests
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TjPixelFormat = LibJpegTurbo.Net.PixelFormat;
    using SdiPixelFormat = System.Drawing.Imaging.PixelFormat;

    /// <summary>
    /// Summary description for RawCompressorTests
    /// </summary>
    [TestClass]
    public class RawCompressorTests
    {
        private static readonly int[][] colours = { new [] { 0x000000, 0xff0000, 0xffff00, 0x00ff00 },
                                                    new [] { 0x00ffff, 0x0000ff, 0xff00ff, 0xffffff },
                                                    new [] { 0x404040, 0x800000, 0x808000, 0x008000 },
                                                    new [] { 0x008080, 0x000080, 0x800080, 0x808080 } };

        private TurboJpegSafeHandle handle;

        [TestInitialize]
        public void CreateCompressor()
        {
            this.handle = new TurboJpegSafeHandle(NativeMethods.initCompressor());
        }

        [TestCleanup]
        public void CleanupCompressor()
        {
            this.handle.Dispose();
        }

        [TestMethod]
        public void CompressImageWithPreallocatedBuffer()
        {
            const int width = 64;
            const int height = 64;
            var pixelSize = TurboJpegInterop.PixelSize[TjPixelFormat.Rgb];
            var pitch = width * pixelSize;
            var imageBytes = new byte[width * height * pixelSize];
            var i = 0;
            for (var y = 0; y < 64; ++y)
            {
                for (var x = 0; x < 64; ++x)
                {
                    var colour = colours[y / 16][x / 16];
                    imageBytes[i + TurboJpegInterop.RedOffset[TjPixelFormat.Rgb]] = (byte) (colour >> 16);
                    imageBytes[i + TurboJpegInterop.GreenOffset[TjPixelFormat.Rgb]] = (byte)((colour >> 8) & 0xff);
                    imageBytes[i + TurboJpegInterop.BlueOffset[TjPixelFormat.Rgb]] = (byte)(colour & 0xff);
                    i += pixelSize;
                }
            }

            var worstCaseSize = NativeMethods.bufSize(width, height, Subsampling.Chroma420);
            var outputBuffer = NativeMethods.alloc(worstCaseSize);
            var originalOutputBufferPointer = outputBuffer;
            var outputBufferSize = (ulong)worstCaseSize;
            var success = NativeMethods.compress((IntPtr) this.handle,
                                                    imageBytes,
                                                    width,
                                                    pitch,
                                                    height,
                                                    TjPixelFormat.Rgb,
                                                    ref outputBuffer,
                                                    ref outputBufferSize,
                                                    Subsampling.Chroma420,
                                                    95,
                                                    TurboJpegFlags.None);
            Assert.IsTrue(success == 0);
            var image = this.ConstructImageFromBuffer(outputBufferSize, outputBuffer);
            Assert.IsTrue(image.Width == width);
            Assert.IsTrue(image.Height == height);
            Assert.IsTrue(image.PixelFormat == SdiPixelFormat.Format24bppRgb);
            Assert.IsTrue(image.RawFormat.Equals(ImageFormat.Jpeg));
            Assert.IsTrue(worstCaseSize > (int)outputBufferSize);
            Assert.IsTrue(originalOutputBufferPointer == outputBuffer);
        }

        [TestMethod]
        public void CompressImageWithInsufficientPreallocatedBuffer()
        {
            const int width = 64;
            const int height = 64;
            var pixelSize = TurboJpegInterop.PixelSize[TjPixelFormat.Rgb];
            var pitch = width * pixelSize;
            var imageBytes = new byte[width * height * pixelSize];
            var i = 0;
            for (var y = 0; y < 64; ++y)
            {
                for (var x = 0; x < 64; ++x)
                {
                    var colour = colours[y / 16][x / 16];
                    imageBytes[i + TurboJpegInterop.RedOffset[TjPixelFormat.Rgb]] = (byte)(colour >> 16);
                    imageBytes[i + TurboJpegInterop.GreenOffset[TjPixelFormat.Rgb]] = (byte)((colour >> 8) & 0xff);
                    imageBytes[i + TurboJpegInterop.BlueOffset[TjPixelFormat.Rgb]] = (byte)(colour & 0xff);
                    i += pixelSize;
                }
            }

            const int preallocatedSize = 256;
            var outputBuffer = NativeMethods.alloc(preallocatedSize);
            var originalOutputBufferPointer = outputBuffer;
            var outputBufferSize = (ulong)preallocatedSize;
            var success = NativeMethods.compress((IntPtr)this.handle,
                                                    imageBytes,
                                                    width,
                                                    pitch,
                                                    height,
                                                    TjPixelFormat.Rgb,
                                                    ref outputBuffer,
                                                    ref outputBufferSize,
                                                    Subsampling.Chroma420,
                                                    95,
                                                    TurboJpegFlags.None);
            Assert.IsTrue(success == 0);
            var image = this.ConstructImageFromBuffer(outputBufferSize, outputBuffer);
            Assert.IsTrue(image.Width == width);
            Assert.IsTrue(image.Height == height);
            Assert.IsTrue(image.PixelFormat == SdiPixelFormat.Format24bppRgb);
            Assert.IsTrue(image.RawFormat.Equals(ImageFormat.Jpeg));
            Assert.IsTrue(preallocatedSize < outputBufferSize);
            Assert.IsTrue(originalOutputBufferPointer != outputBuffer);
        }

        [TestMethod]
        public void CompressImageNoPreallocatedBuffer()
        {
            const int width = 64;
            const int height = 64;
            var pixelSize = TurboJpegInterop.PixelSize[TjPixelFormat.Rgb];
            var pitch = width * pixelSize;
            var imageBytes = new byte[width * height * pixelSize];
            var i = 0;
            for (var y = 0; y < 64; ++y)
            {
                for (var x = 0; x < 64; ++x)
                {
                    var colour = colours[y / 16][x / 16];
                    imageBytes[i + TurboJpegInterop.RedOffset[TjPixelFormat.Rgb]] = (byte)(colour >> 16);
                    imageBytes[i + TurboJpegInterop.GreenOffset[TjPixelFormat.Rgb]] = (byte)((colour >> 8) & 0xff);
                    imageBytes[i + TurboJpegInterop.BlueOffset[TjPixelFormat.Rgb]] = (byte)(colour & 0xff);
                    i += pixelSize;
                }
            }

            var outputBuffer = IntPtr.Zero;
            var outputBufferSize = 0UL;
            var success = NativeMethods.compress((IntPtr)this.handle,
                                                    imageBytes,
                                                    width,
                                                    pitch,
                                                    height,
                                                    TjPixelFormat.Rgb,
                                                    ref outputBuffer,
                                                    ref outputBufferSize,
                                                    Subsampling.Chroma420,
                                                    95,
                                                    TurboJpegFlags.None);
            Assert.IsTrue(success == 0);
            var image = this.ConstructImageFromBuffer(outputBufferSize, outputBuffer);
            Assert.IsTrue(image.Width == width);
            Assert.IsTrue(image.Height == height);
            Assert.IsTrue(image.PixelFormat == SdiPixelFormat.Format24bppRgb);
            Assert.IsTrue(image.RawFormat.Equals(ImageFormat.Jpeg));
        }

        /// <summary>
        /// Reads an image from a buffer.
        /// </summary>
        /// <param name="outputBufferSize">Size of the output buffer.</param>
        /// <param name="outputBuffer">The output buffer.</param>
        /// <returns></returns>
        private Image ConstructImageFromBuffer(ulong outputBufferSize, IntPtr outputBuffer)
        {
            var results = new byte[outputBufferSize];
            Marshal.Copy(outputBuffer, results, 0, (int) outputBufferSize);

            var memstream = new MemoryStream(results);
            var jpegImage = Image.FromStream(memstream);
            return jpegImage;
        }
    }
}
