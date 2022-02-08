/*
 * LICENSE
Copyright (c) 2000 - 2018 The Legion of the Bouncy Castle Inc. (https://www.bouncycastle.org)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

namespace Hashing
{
    internal static class Pack
    {
        internal static void UInt32_To_LE(uint n, byte[] bs, int off)
        {
            bs[off] = (byte)n;
            bs[off + 1] = (byte)(n >> 8);
            bs[off + 2] = (byte)(n >> 16);
            bs[off + 3] = (byte)(n >> 24);
        }

        internal static uint LE_To_UInt32(byte[] bs, int off)
        {
            return bs[off]
                   | ((uint)bs[off + 1] << 8)
                   | ((uint)bs[off + 2] << 16)
                   | ((uint)bs[off + 3] << 24);
        }

        internal static void UInt64_To_LE(ulong n, byte[] bs, int off)
        {
            UInt32_To_LE((uint)n, bs, off);
            UInt32_To_LE((uint)(n >> 32), bs, off + 4);
        }

        internal static void UInt64_To_LE(ulong[] ns, int nsOff, int nsLen, byte[] bs, int bsOff)
        {
            for (var i = 0; i < nsLen; ++i)
            {
                UInt64_To_LE(ns[nsOff + i], bs, bsOff);
                bsOff += 8;
            }
        }

        internal static ulong LE_To_UInt64(byte[] bs, int off)
        {
            var lo = LE_To_UInt32(bs, off);
            var hi = LE_To_UInt32(bs, off + 4);
            return ((ulong)hi << 32) | lo;
        }
    }
}