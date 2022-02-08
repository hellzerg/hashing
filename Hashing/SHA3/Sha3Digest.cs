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


using System;
using System.Diagnostics;

namespace Hashing
{
    /// <summary>
    ///     Implementation of SHA-3 based on following KeccakNISTInterface.c from http://keccak.noekeon.org/
    /// </summary>
    /// <remarks>
    ///     Following the naming conventions used in the C source code to enable easy review of the implementation.
    /// </remarks>
    internal class Sha3Digest
        : KeccakDigest
    {
        public Sha3Digest()
            : this(256)
        {
        }

        public Sha3Digest(int bitLength)
            : base(CheckBitLength(bitLength))
        {
        }

        public Sha3Digest(Sha3Digest source)
            : base(source)
        {
        }

        public override string AlgorithmName => "SHA3-" + FixedOutputLength;

        private static int CheckBitLength(int bitLength)
        {
            switch (bitLength)
            {
                case 224:
                case 256:
                case 384:
                case 512:
                    return bitLength;
                default:
                    throw new ArgumentException(bitLength + " not supported for SHA-3", "bitLength");
            }
        }

        public override int DoFinal(byte[] output, int outOff)
        {
            AbsorbBits(0x02, 2);

            return base.DoFinal(output, outOff);
        }

        /*
         * TODO Possible API change to support partial-byte suffixes.
         */
        protected override int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
        {
            if (partialBits < 0 || partialBits > 7)
                throw new ArgumentException("must be in the range [0,7]", "partialBits");

            var finalInput = (partialByte & ((1 << partialBits) - 1)) | (0x02 << partialBits);
            Debug.Assert(finalInput >= 0);
            var finalBits = partialBits + 2;

            if (finalBits >= 8)
            {
                Absorb((byte)finalInput);
                finalBits -= 8;
                finalInput >>= 8;
            }

            return base.DoFinal(output, outOff, (byte)finalInput, finalBits);
        }

        public override IMemoable Copy()
        {
            return new Sha3Digest(this);
        }
    }
}