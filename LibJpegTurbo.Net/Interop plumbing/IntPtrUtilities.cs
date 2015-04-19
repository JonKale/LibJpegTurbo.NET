namespace LibJpegTurbo.Net
{
    #region

    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    /// A bunch of utility functions for marshalling unmanaged blobs to managed arrays.
    /// </summary>
    internal static class IntPtrUtilities
    {
        /// <summary>
        /// Copies data from an unmanaged memory pointer to a managed 8-bit unsigned integer array.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="elements">The number of array elements to copy.</param>
        /// <returns>An array of the specified size populated from <paramref name="pointer" />.</returns>
        public static byte[] ToByteArray(this IntPtr pointer, int elements)
        {
            var array = new byte[elements];
            Marshal.Copy(pointer, array, 0, elements);
            return array;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to a managed char array.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="elements">The number of array elements to copy.</param>
        /// <returns>An array of the specified size populated from <paramref name="pointer" />.</returns>
        public static char[] ToCharArray(this IntPtr pointer, int elements)
        {
            var array = new char[elements];
            Marshal.Copy(pointer, array, 0, elements);
            return array;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to a managed double array.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="elements">The number of array elements to copy.</param>
        /// <returns>An array of the specified size populated from <paramref name="pointer" />.</returns>
        public static double[] ToDoubleArray(this IntPtr pointer, int elements)
        {
            var array = new double[elements];
            Marshal.Copy(pointer, array, 0, elements);
            return array;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to a managed short array.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="elements">The number of array elements to copy.</param>
        /// <returns>An array of the specified size populated from <paramref name="pointer" />.</returns>
        public static short[] ToShortArray(this IntPtr pointer, int elements)
        {
            var array = new short[elements];
            Marshal.Copy(pointer, array, 0, elements);
            return array;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to a managed int array.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="elements">The number of array elements to copy.</param>
        /// <returns>An array of the specified size populated from <paramref name="pointer" />.</returns>
        public static int[] ToIntArray(this IntPtr pointer, int elements)
        {
            var array = new int[elements];
            Marshal.Copy(pointer, array, 0, elements);
            return array;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to a managed long array.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="elements">The number of array elements to copy.</param>
        /// <returns>An array of the specified size populated from <paramref name="pointer" />.</returns>
        public static long[] ToLongArray(this IntPtr pointer, int elements)
        {
            var array = new long[elements];
            Marshal.Copy(pointer, array, 0, elements);
            return array;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to a managed IntPtr array.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="elements">The number of array elements to copy.</param>
        /// <returns>An array of the specified size populated from <paramref name="pointer" />.</returns>
        public static IntPtr[] ToIntPtrArray(this IntPtr pointer, int elements)
        {
            var array = new IntPtr[elements];
            Marshal.Copy(pointer, array, 0, elements);
            return array;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to a managed float array.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="elements">The number of array elements to copy.</param>
        /// <returns>An array of the specified size populated from <paramref name="pointer" />.</returns>
        public static float[] ToFloatArray(this IntPtr pointer, int elements)
        {
            var array = new float[elements];
            Marshal.Copy(pointer, array, 0, elements);
            return array;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to a managed <see cref="TurboJpegScalingFactor"/> array.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="elements">The number of array elements to copy.</param>
        /// <returns>An array of the specified size populated from <paramref name="pointer" />.</returns>
        public static TurboJpegScalingFactor[] ToTurboJpegScalingFactorArray(this IntPtr pointer, int elements)
        {
            var array = new TurboJpegScalingFactor[elements];
            var structSize = Marshal.SizeOf(typeof(TurboJpegScalingFactor));
            var j = pointer;
            for (var i = 0; i < elements; i += 1, j += structSize)
            {
                array[i] = (TurboJpegScalingFactor) Marshal.PtrToStructure(j, typeof(TurboJpegScalingFactor));
            }

            return array;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed byte arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>This method does not support ragged arrays. Instead, use <see cref="ToByteArrays(IntPtr, int, int[])" />.</remarks>
        public static byte[][] ToByteArrays(this IntPtr pointer, int arrays, int elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            var output = new byte[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToByteArray(elements);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed byte arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>This method should be used for ragged arrays.</remarks>
        public static byte[][] ToByteArrays(this IntPtr pointer, int arrays, int[] elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            Debug.Assert(arrayStarts.Length != elements.Length, "arrayStarts.Length != elements.Length");
            var output = new byte[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToByteArray(elements[i]);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed char arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>
        /// This method does not support ragged arrays. Instead, use
        /// <see cref="ToCharArrays(System.IntPtr, System.Int32, System.Int32[])" />.
        /// </remarks>
        public static char[][] ToCharArrays(this IntPtr pointer, int arrays, int elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            var output = new char[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToCharArray(elements);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed char arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>This method should be used for ragged arrays.</remarks>
        public static char[][] ToCharArrays(this IntPtr pointer, int arrays, int[] elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            Debug.Assert(arrayStarts.Length != elements.Length, "arrayStarts.Length != elements.Length");
            var output = new char[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToCharArray(elements[i]);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed double arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>
        /// This method does not support ragged arrays. Instead, use
        /// <see cref="ToDoubleArrays(System.IntPtr, System.Int32, System.Int32[])" />.
        /// </remarks>
        public static double[][] ToDoubleArrays(this IntPtr pointer, int arrays, int elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            var output = new double[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToDoubleArray(elements);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed double arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>This method should be used for ragged arrays.</remarks>
        public static double[][] ToDoubleArrays(this IntPtr pointer, int arrays, int[] elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            Debug.Assert(arrayStarts.Length != elements.Length, "arrayStarts.Length != elements.Length");
            var output = new double[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToDoubleArray(elements[i]);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed short arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>
        /// This method does not support ragged arrays. Instead, use
        /// <see cref="ToShortArrays(System.IntPtr, System.Int32, System.Int32[])" />.
        /// </remarks>
        public static short[][] ToShortArrays(this IntPtr pointer, int arrays, int elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            var output = new short[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToShortArray(elements);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed short arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>This method should be used for ragged arrays.</remarks>
        public static short[][] ToShortArrays(this IntPtr pointer, int arrays, int[] elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            Debug.Assert(arrayStarts.Length != elements.Length, "arrayStarts.Length != elements.Length");
            var output = new short[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToShortArray(elements[i]);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed int arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>
        /// This method does not support ragged arrays. Instead, use
        /// <see cref="ToIntArrays(System.IntPtr, System.Int32, System.Int32[])" />.
        /// </remarks>
        public static int[][] ToIntArrays(this IntPtr pointer, int arrays, int elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            var output = new int[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToIntArray(elements);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed int arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>This method should be used for ragged arrays.</remarks>
        public static int[][] ToIntArrays(this IntPtr pointer, int arrays, int[] elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            Debug.Assert(arrayStarts.Length != elements.Length, "arrayStarts.Length != elements.Length");
            var output = new int[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToIntArray(elements[i]);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed long arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>
        /// This method does not support ragged arrays. Instead, use
        /// <see cref="ToLongArrays(System.IntPtr, System.Int32, System.Int32[])" />.
        /// </remarks>
        public static long[][] ToLongArrays(this IntPtr pointer, int arrays, int elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            var output = new long[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToLongArray(elements);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed long arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>This method should be used for ragged arrays.</remarks>
        public static long[][] ToLongArrays(this IntPtr pointer, int arrays, int[] elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            Debug.Assert(arrayStarts.Length != elements.Length, "arrayStarts.Length != elements.Length");
            var output = new long[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToLongArray(elements[i]);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed IntPtr arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>
        /// This method does not support ragged arrays. Instead, use
        /// <see cref="ToIntPtrArrays(System.IntPtr, System.Int32, System.Int32[])" />.
        /// </remarks>
        public static IntPtr[][] ToIntPtrArrays(this IntPtr pointer, int arrays, int elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            var output = new IntPtr[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToIntPtrArray(elements);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed IntPtr arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>This method should be used for ragged arrays.</remarks>
        public static IntPtr[][] ToIntPtrArrays(this IntPtr pointer, int arrays, int[] elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            Debug.Assert(arrayStarts.Length != elements.Length, "arrayStarts.Length != elements.Length");
            var output = new IntPtr[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToIntPtrArray(elements[i]);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed float arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>
        /// This method does not support ragged arrays. Instead, use
        /// <see cref="ToFloatArrays(System.IntPtr, System.Int32, System.Int32[])" />.
        /// </remarks>
        public static float[][] ToFloatArrays(this IntPtr pointer, int arrays, int elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            var output = new float[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToFloatArray(elements);
            }

            return output;
        }

        /// <summary>
        /// Copies data from an unmanaged memory pointer to an array of managed float arrays.
        /// </summary>
        /// <param name="pointer">The memory pointer to copy from.</param>
        /// <param name="arrays">The number of arrays to copy.</param>
        /// <param name="elements">The number of elements to copy into each array.</param>
        /// <returns>An array of arrays of the specified size populated from <paramref name="pointer" />.</returns>
        /// <remarks>This method should be used for ragged arrays.</remarks>
        public static float[][] ToFloatArrays(this IntPtr pointer, int arrays, int[] elements)
        {
            var arrayStarts = pointer.ToIntPtrArray(arrays);
            Debug.Assert(arrayStarts.Length != elements.Length, "arrayStarts.Length != elements.Length");
            var output = new float[arrays][];
            for (var i = 0; i < arrays; ++i)
            {
                output[i] = arrayStarts[i].ToFloatArray(elements[i]);
            }

            return output;
        }
    }
}