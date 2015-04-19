﻿namespace LibJpegTurbo.Net.InteropTests
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TjPixelFormat = LibJpegTurbo.Net.PixelFormat;
    using SdiPixelFormat = System.Drawing.Imaging.PixelFormat;

    [TestClass]
    public class DecompressorTests
    {
        // 64x64 square coloured 000000 ff0000 ffff00 00ff00
        //                       00ffff 0000ff ff00ff ffffff
        //                       404040 800000 808000 008000
        //                       008080 000080 800080 808080
        // in 16x16 blocks
        private static readonly byte[] imageData =
        {
            0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01, 0x01, 0x01, 0x00, 0x60, 
            0x00, 0x60, 0x00, 0x00, 0xFF, 0xE1, 0x00, 0x66, 0x45, 0x78, 0x69, 0x66, 0x00, 0x00, 0x4D, 0x4D, 
            0x00, 0x2A, 0x00, 0x00, 0x00, 0x08, 0x00, 0x04, 0x01, 0x1A, 0x00, 0x05, 0x00, 0x00, 0x00, 0x01, 
            0x00, 0x00, 0x00, 0x3E, 0x01, 0x1B, 0x00, 0x05, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x46, 
            0x01, 0x28, 0x00, 0x03, 0x00, 0x00, 0x00, 0x01, 0x00, 0x02, 0x00, 0x00, 0x01, 0x31, 0x00, 0x02, 
            0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x4E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x60, 
            0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x60, 0x00, 0x00, 0x00, 0x01, 0x70, 0x61, 0x69, 0x6E, 
            0x74, 0x2E, 0x6E, 0x65, 0x74, 0x20, 0x34, 0x2E, 0x30, 0x2E, 0x35, 0x00, 0xFF, 0xDB, 0x00, 0x43, 
            0x00, 0x02, 0x01, 0x01, 0x02, 0x01, 0x01, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x03, 
            0x05, 0x03, 0x03, 0x03, 0x03, 0x03, 0x06, 0x04, 0x04, 0x03, 0x05, 0x07, 0x06, 0x07, 0x07, 0x07, 
            0x06, 0x07, 0x07, 0x08, 0x09, 0x0B, 0x09, 0x08, 0x08, 0x0A, 0x08, 0x07, 0x07, 0x0A, 0x0D, 0x0A, 
            0x0A, 0x0B, 0x0C, 0x0C, 0x0C, 0x0C, 0x07, 0x09, 0x0E, 0x0F, 0x0D, 0x0C, 0x0E, 0x0B, 0x0C, 0x0C, 
            0x0C, 0xFF, 0xDB, 0x00, 0x43, 0x01, 0x02, 0x02, 0x02, 0x03, 0x03, 0x03, 0x06, 0x03, 0x03, 0x06, 
            0x0C, 0x08, 0x07, 0x08, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 
            0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 
            0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 
            0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0x0C, 0xFF, 0xC0, 0x00, 0x11, 0x08, 0x00, 0x40, 0x00, 0x40, 0x03, 
            0x01, 0x22, 0x00, 0x02, 0x11, 0x01, 0x03, 0x11, 0x01, 0xFF, 0xC4, 0x00, 0x1F, 0x00, 0x00, 0x01, 
            0x05, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 
            0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0xFF, 0xC4, 0x00, 0xB5, 0x10, 0x00, 
            0x02, 0x01, 0x03, 0x03, 0x02, 0x04, 0x03, 0x05, 0x05, 0x04, 0x04, 0x00, 0x00, 0x01, 0x7D, 0x01, 
            0x02, 0x03, 0x00, 0x04, 0x11, 0x05, 0x12, 0x21, 0x31, 0x41, 0x06, 0x13, 0x51, 0x61, 0x07, 0x22, 
            0x71, 0x14, 0x32, 0x81, 0x91, 0xA1, 0x08, 0x23, 0x42, 0xB1, 0xC1, 0x15, 0x52, 0xD1, 0xF0, 0x24, 
            0x33, 0x62, 0x72, 0x82, 0x09, 0x0A, 0x16, 0x17, 0x18, 0x19, 0x1A, 0x25, 0x26, 0x27, 0x28, 0x29, 
            0x2A, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3A, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 
            0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5A, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 
            0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 
            0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9A, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 
            0xA9, 0xAA, 0xB2, 0xB3, 0xB4, 0xB5, 0xB6, 0xB7, 0xB8, 0xB9, 0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xC6, 
            0xC7, 0xC8, 0xC9, 0xCA, 0xD2, 0xD3, 0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA, 0xE1, 0xE2, 0xE3, 
            0xE4, 0xE5, 0xE6, 0xE7, 0xE8, 0xE9, 0xEA, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 
            0xFA, 0xFF, 0xC4, 0x00, 0x1F, 0x01, 0x00, 0x03, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 
            0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 
            0x0A, 0x0B, 0xFF, 0xC4, 0x00, 0xB5, 0x11, 0x00, 0x02, 0x01, 0x02, 0x04, 0x04, 0x03, 0x04, 0x07, 
            0x05, 0x04, 0x04, 0x00, 0x01, 0x02, 0x77, 0x00, 0x01, 0x02, 0x03, 0x11, 0x04, 0x05, 0x21, 0x31, 
            0x06, 0x12, 0x41, 0x51, 0x07, 0x61, 0x71, 0x13, 0x22, 0x32, 0x81, 0x08, 0x14, 0x42, 0x91, 0xA1, 
            0xB1, 0xC1, 0x09, 0x23, 0x33, 0x52, 0xF0, 0x15, 0x62, 0x72, 0xD1, 0x0A, 0x16, 0x24, 0x34, 0xE1, 
            0x25, 0xF1, 0x17, 0x18, 0x19, 0x1A, 0x26, 0x27, 0x28, 0x29, 0x2A, 0x35, 0x36, 0x37, 0x38, 0x39, 
            0x3A, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4A, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 
            0x5A, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6A, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 
            0x7A, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8A, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 
            0x98, 0x99, 0x9A, 0xA2, 0xA3, 0xA4, 0xA5, 0xA6, 0xA7, 0xA8, 0xA9, 0xAA, 0xB2, 0xB3, 0xB4, 0xB5, 
            0xB6, 0xB7, 0xB8, 0xB9, 0xBA, 0xC2, 0xC3, 0xC4, 0xC5, 0xC6, 0xC7, 0xC8, 0xC9, 0xCA, 0xD2, 0xD3, 
            0xD4, 0xD5, 0xD6, 0xD7, 0xD8, 0xD9, 0xDA, 0xE2, 0xE3, 0xE4, 0xE5, 0xE6, 0xE7, 0xE8, 0xE9, 0xEA, 
            0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7, 0xF8, 0xF9, 0xFA, 0xFF, 0xDA, 0x00, 0x0C, 0x03, 0x01, 0x00, 
            0x02, 0x11, 0x03, 0x11, 0x00, 0x3F, 0x00, 0xFE, 0x7F, 0xE8, 0xA2, 0x8A, 0x00, 0xFD, 0x30, 0xA2, 
            0x8A, 0x2B, 0xF9, 0x4C, 0xFF, 0x00, 0x7F, 0x0F, 0xE9, 0x62, 0x8A, 0x28, 0xAF, 0xF2, 0xBC, 0xFF, 
            0x00, 0x15, 0xCF, 0xCC, 0xFA, 0x28, 0xA2, 0xBF, 0xAB, 0x0F, 0xF9, 0xFF, 0x00, 0x3D, 0xD2, 0x8A, 
            0x28, 0xAF, 0xF7, 0xF0, 0xFC, 0xBC, 0xFE, 0x69, 0xE8, 0xA2, 0x8A, 0xFF, 0x00, 0x54, 0x0F, 0xF6, 
            0xA0, 0xFD, 0x30, 0xA2, 0x8A, 0x2B, 0xF9, 0x4C, 0xFF, 0x00, 0xA0, 0x03, 0xFA, 0x58, 0xA2, 0x8A, 
            0x2B, 0xFC, 0xAF, 0x3F, 0xC5, 0x33, 0xF9, 0x03, 0xA2, 0x8A, 0x28, 0x03, 0xC5, 0xE8, 0xA2, 0x8A, 
            0xFC, 0x9C, 0xFF, 0x00, 0x40, 0x0F, 0xD2, 0xCA, 0x28, 0xA2, 0xBF, 0x95, 0xCF, 0xE2, 0x73, 0xE6, 
            0x7A, 0x28, 0xA2, 0xBF, 0x58, 0x3F, 0xCF, 0xF3, 0xBC, 0xA2, 0x8A, 0x2B, 0xFD, 0x00, 0x3E, 0x5C, 
            0xFC, 0xD3, 0xA2, 0x8A, 0x2B, 0xFA, 0xA0, 0xFE, 0xD8, 0x3E, 0x98, 0xA2, 0x8A, 0x2B, 0xF2, 0x73, 
            0xFD, 0x00, 0x3F, 0x4B, 0x28, 0xA2, 0x8A, 0xFE, 0x57, 0x3F, 0x89, 0xCF, 0xFF, 0xD9
        };
        
        private TurboJpegSafeHandle handle;

        [TestInitialize]
        public void CreateDecompressor()
        {
            this.handle = new TurboJpegSafeHandle(TurboJpegInterop.initDecompressor());
        }

        [TestCleanup]
        public void CleanupDecompressor()
        {
            this.handle.Dispose();
        }

        [TestMethod]
        public void DecompressHeader()
        {
            int width;
            int height;
            Subsampling chroma;
            Colourspace colourspace;
            var success = TurboJpegInterop.decompressHeader((IntPtr)this.handle,
                                                            imageData,
                                                            imageData.Length,
                                                            out width,
                                                            out height,
                                                            out chroma,
                                                            out colourspace);
            Assert.IsTrue(success == 0);
            Assert.IsTrue(width == 64);
            Assert.IsTrue(height == 64);
            Assert.IsTrue(chroma == Subsampling.Chroma420);
            Assert.IsTrue(colourspace == Colourspace.YCbCr);
        }

        [TestMethod]
        public void DecompressImage()
        {
            const int width = 64;
            const int height = 64;
            var pixelSize = TurboJpegInterop.PixelSize[TjPixelFormat.Rgba];
            var pitch = width * pixelSize;
            var outputBufferSize = pitch * height;
            var outputBuffer = Marshal.AllocHGlobal(outputBufferSize);
            outputBuffer.Initialise(outputBufferSize);
            var success = TurboJpegInterop.decompress((IntPtr)this.handle,
                                                      imageData,
                                                      imageData.Length,
                                                      outputBuffer,
                                                      width,
                                                      pitch, 
                                                      height,
                                                      TjPixelFormat.Rgba,
                                                      TurboJpegFlags.None);
            Assert.IsTrue(success == 0);
            var results = new byte[outputBufferSize];
            Marshal.Copy(outputBuffer, results, 0, outputBufferSize);
            var image = new Bitmap(width, height, SdiPixelFormat.Format32bppArgb);
            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    var baseOffset = y * pitch + x * pixelSize;
                    var colour = Color.FromArgb(results[baseOffset + TurboJpegInterop.RedOffset[TjPixelFormat.Rgba]],
                                                results[baseOffset + TurboJpegInterop.GreenOffset[TjPixelFormat.Rgba]],
                                                results[baseOffset + TurboJpegInterop.BlueOffset[TjPixelFormat.Rgba]]);
                    image.SetPixel(x, y, colour);
                }
            }

            bool isOk;
            using(var form = new Form
                             {
                                 Text = String.Format("Width: {0}, Height: {1}", image.Width, image.Height),
                                 ClientSize = new Size(Math.Max(image.Width, 64), Math.Max(image.Height, 64)),
                                 FormBorderStyle = FormBorderStyle.FixedToolWindow
                             })
            {
                using(
                    var pictureBox = new PictureBox
                                     {
                                         Image = image,
                                         Parent = form,
                                         Dock = DockStyle.Fill,
                                         SizeMode = PictureBoxSizeMode.Zoom
                                     })
                {
                    form.Show();
                    isOk =
                        MessageBox.Show("Does the image look reasonable?",
                                        "Eyeballing…",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes;
                }

                form.Close();
            }

            Assert.IsTrue(isOk);
        }

        [TestMethod]
        public void DecompressAndScaleImageBySupportedScaleFactor()
        {
            const int width = 96;
            const int height = 96;
            var pixelSize = TurboJpegInterop.PixelSize[TjPixelFormat.Rgba];
            var pitch = width * pixelSize;
            var outputBufferSize = pitch * height;
            var outputBuffer = new TurboJpegBuffer(outputBufferSize);
            outputBuffer.Buffer.Initialise(outputBufferSize);
            var success = TurboJpegInterop.decompress((IntPtr)this.handle,
                                                      imageData,
                                                      imageData.Length,
                                                      (IntPtr)outputBuffer,
                                                      width,
                                                      pitch,
                                                      height,
                                                      TjPixelFormat.Rgba,
                                                      TurboJpegFlags.None);
            Assert.IsTrue(success == 0);
            Assert.IsTrue(this.ImageOutputIsOk(outputBufferSize, outputBuffer, width, height, pitch, pixelSize));
        }

        private bool ImageOutputIsOk(int outputBufferSize, TurboJpegBuffer outputBuffer, int width, int height, int pitch, int pixelSize)
        {

            var results = new byte[outputBufferSize];
            Marshal.Copy((IntPtr)outputBuffer, results, 0, outputBufferSize);
            var image = new Bitmap(width, height, SdiPixelFormat.Format32bppArgb);
            for (var y = 0; y < height; ++y)
            {
                for (var x = 0; x < width; ++x)
                {
                    var baseOffset = y * pitch + x * pixelSize;
                    var colour = Color.FromArgb(results[baseOffset + TurboJpegInterop.RedOffset[TjPixelFormat.Rgba]],
                                                results[baseOffset + TurboJpegInterop.GreenOffset[TjPixelFormat.Rgba]],
                                                results[baseOffset + TurboJpegInterop.BlueOffset[TjPixelFormat.Rgba]]);
                    image.SetPixel(x, y, colour);
                }
            }

            bool isOk;
            using(var form = new Form
                             {
                                 Text = String.Format("Width: {0}, Height: {1}", image.Width, image.Height),
                                 ClientSize = new Size(Math.Max(image.Width, 64), Math.Max(image.Height, 64)),
                                 FormBorderStyle = FormBorderStyle.FixedToolWindow
                             })
            {
                using(
                    var pictureBox = new PictureBox
                                     {
                                         Image = image,
                                         Parent = form,
                                         Dock = DockStyle.Fill,
                                         SizeMode = PictureBoxSizeMode.Zoom
                                     })
                {
                    form.Show();
                    isOk =
                        MessageBox.Show("Does the image look reasonable?",
                                        "Eyeballing…",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes;
                }

                form.Close();
            }
            return isOk;
        }
    }
}
