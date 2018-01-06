using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core
{
    public class U
    {
        static readonly Random random = new Random();
        public static int Rand(int max) => random.Next(max);
        public static double Rand() => random.NextDouble();
        public static void RShuffle<T>(IList<T> array)
        {
            for (int i = array.Count - 1; i >= 0; i--)
            {
                int j = Rand(i + 1);

                var save = array[i];

                array[i] = array[j];
                array[j] = save;
            }
        }

        public static int InputSize = -1;

        public sealed class DoubleStream
        {
            public DoubleStream(int inputSize)
            {
                buffer = new double[inputSize];
            }
            double[] buffer;
            int pos = 0;

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void Write(double d) { buffer[pos++] = d; }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void WriteEmpty(int len) { pos += len; }

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            public void WriteOneHot(int len, int hotIndex)
            {
                buffer[pos + hotIndex] = 1;
                pos += len;
            }

            public double[] ToArray()
            {
                if (pos != buffer.Length) { throw new Exception("Bad stream size! Probably isn't finished writing"); }
                return buffer;
            }
        }

        public sealed class ModuloList<T>
        {
            public readonly T[] Buffer;
            int index = 0;

            int len = 0;
            public int Length => len;

            public ModuloList(int len)
            {
                Buffer = new T[len];
            }

            public void SetNext(T val)
            {
                Buffer[index] = val;

                if (len + 1 < Buffer.Length) { len++; }
                index = (index + 1) % Buffer.Length;
            }
        }
    }
}
