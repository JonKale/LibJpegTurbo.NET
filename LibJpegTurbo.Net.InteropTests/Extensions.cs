using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibJpegTurbo.Net.InteropTests
{
    using System.CodeDom;
    using System.Runtime.InteropServices;

    internal static class Extensions
    {
        public static void Initialise(this IntPtr buffer, int length)
        {
            var deadbeef = new byte[] { 0xDE, 0xAD, 0xBE, 0xEF, 0xDE, 0xAD, 0xBE, 0xEF };
            var copied = 0;
            while (copied < length)
            {
                Marshal.Copy(deadbeef, copied % 8, buffer, 1);
                ++copied;
                buffer = buffer + 1;
            }
        }
    }
}
