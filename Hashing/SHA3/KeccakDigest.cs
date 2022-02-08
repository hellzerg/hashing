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
    ///     Implementation of Keccak based on following KeccakNISTInterface.c from http://keccak.noekeon.org/
    /// </summary>
    /// <remarks>
    ///     Following the naming conventions used in the C source code to enable easy review of the implementation.
    /// </remarks>
    internal class KeccakDigest
        : IDigest, IMemoable
    {
        private static readonly ulong[] KeccakRoundConstants =
        {
            0x0000000000000001UL, 0x0000000000008082UL, 0x800000000000808aUL, 0x8000000080008000UL,
            0x000000000000808bUL, 0x0000000080000001UL, 0x8000000080008081UL, 0x8000000000008009UL,
            0x000000000000008aUL, 0x0000000000000088UL, 0x0000000080008009UL, 0x000000008000000aUL,
            0x000000008000808bUL, 0x800000000000008bUL, 0x8000000000008089UL, 0x8000000000008003UL,
            0x8000000000008002UL, 0x8000000000000080UL, 0x000000000000800aUL, 0x800000008000000aUL,
            0x8000000080008081UL, 0x8000000000008080UL, 0x0000000080000001UL, 0x8000000080008008UL
        };

        private int _bitsInQueue;
        private readonly byte[] _dataQueue = new byte[192];
        protected int FixedOutputLength;
        private int _rate;
        private bool _squeezing;

        private readonly ulong[] _state = new ulong[25];

        public KeccakDigest()
            : this(288)
        {
        }

        public KeccakDigest(int bitLength)
        {
            Init(bitLength);
        }

        public KeccakDigest(KeccakDigest source)
        {
            CopyIn(source);
        }

        public virtual string AlgorithmName => "Keccak-" + FixedOutputLength;

        public virtual int GetDigestSize()
        {
            return FixedOutputLength >> 3;
        }

        public virtual void Update(byte input)
        {
            Absorb(input);
        }

        public virtual void BlockUpdate(byte[] input, int inOff, int len)
        {
            Absorb(input, inOff, len);
        }

        public virtual int DoFinal(byte[] output, int outOff)
        {
            Squeeze(output, outOff, FixedOutputLength);

            Reset();

            return GetDigestSize();
        }

        public virtual void Reset()
        {
            Init(FixedOutputLength);
        }

        /**
         * Return the size of block that the compression function is applied to in bytes.
         *
         * @return internal byte length of a block.
         */
        public virtual int GetByteLength()
        {
            return _rate >> 3;
        }

        public virtual IMemoable Copy()
        {
            return new KeccakDigest(this);
        }

        public virtual void Reset(IMemoable other)
        {
            CopyIn((KeccakDigest)other);
        }

        private void CopyIn(KeccakDigest source)
        {
            Array.Copy(source._state, 0, _state, 0, source._state.Length);
            Array.Copy(source._dataQueue, 0, _dataQueue, 0, source._dataQueue.Length);
            _rate = source._rate;
            _bitsInQueue = source._bitsInQueue;
            FixedOutputLength = source.FixedOutputLength;
            _squeezing = source._squeezing;
        }

        /*
         * TODO Possible API change to support partial-byte suffixes.
         */
        protected virtual int DoFinal(byte[] output, int outOff, byte partialByte, int partialBits)
        {
            if (partialBits > 0) AbsorbBits(partialByte, partialBits);

            Squeeze(output, outOff, FixedOutputLength);

            Reset();

            return GetDigestSize();
        }

        private void Init(int bitLength)
        {
            switch (bitLength)
            {
                case 128:
                case 224:
                case 256:
                case 288:
                case 384:
                case 512:
                    InitSponge(1600 - (bitLength << 1));
                    break;
                default:
                    throw new ArgumentException("must be one of 128, 224, 256, 288, 384, or 512.", nameof(bitLength));
            }
        }

        private void InitSponge(int rate)
        {
            if (rate <= 0 || rate >= 1600 || (rate & 63) != 0)
                throw new InvalidOperationException("invalid rate value");

            this._rate = rate;
            Array.Clear(_state, 0, _state.Length);
            Arrays.Fill(_dataQueue, 0);
            _bitsInQueue = 0;
            _squeezing = false;
            FixedOutputLength = (1600 - rate) >> 1;
        }

        protected void Absorb(byte data)
        {
            if ((_bitsInQueue & 7) != 0)
                throw new InvalidOperationException("attempt to absorb with odd length queue");
            if (_squeezing)
                throw new InvalidOperationException("attempt to absorb while squeezing");

            _dataQueue[_bitsInQueue >> 3] = data;
            if ((_bitsInQueue += 8) == _rate)
            {
                KeccakAbsorb(_dataQueue, 0);
                _bitsInQueue = 0;
            }
        }

        protected void Absorb(byte[] data, int off, int len)
        {
            if ((_bitsInQueue & 7) != 0)
                throw new InvalidOperationException("attempt to absorb with odd length queue");
            if (_squeezing)
                throw new InvalidOperationException("attempt to absorb while squeezing");

            var bytesInQueue = _bitsInQueue >> 3;
            var rateBytes = _rate >> 3;

            var available = rateBytes - bytesInQueue;
            if (len < available)
            {
                Array.Copy(data, off, _dataQueue, bytesInQueue, len);
                _bitsInQueue += len << 3;
                return;
            }

            var count = 0;
            if (bytesInQueue > 0)
            {
                Array.Copy(data, off, _dataQueue, bytesInQueue, available);
                count += available;
                KeccakAbsorb(_dataQueue, 0);
            }

            int remaining;
            while ((remaining = len - count) >= rateBytes)
            {
                KeccakAbsorb(data, off + count);
                count += rateBytes;
            }

            Array.Copy(data, off + count, _dataQueue, 0, remaining);
            _bitsInQueue = remaining << 3;
        }

        protected void AbsorbBits(int data, int bits)
        {
            if (bits < 1 || bits > 7)
                throw new ArgumentException("must be in the range 1 to 7", nameof(bits));
            if ((_bitsInQueue & 7) != 0)
                throw new InvalidOperationException("attempt to absorb with odd length queue");
            if (_squeezing)
                throw new InvalidOperationException("attempt to absorb while squeezing");

            var mask = (1 << bits) - 1;
            _dataQueue[_bitsInQueue >> 3] = (byte)(data & mask);

            // NOTE: After this, bitsInQueue is no longer a multiple of 8, so no more absorbs will work
            _bitsInQueue += bits;
        }

        private void PadAndSwitchToSqueezingPhase()
        {
            Debug.Assert(_bitsInQueue < _rate);

            _dataQueue[_bitsInQueue >> 3] |= (byte)(1 << (_bitsInQueue & 7));

            if (++_bitsInQueue == _rate)
            {
                KeccakAbsorb(_dataQueue, 0);
            }
            else
            {
                int full = _bitsInQueue >> 6, partial = _bitsInQueue & 63;
                var off = 0;
                for (var i = 0; i < full; ++i)
                {
                    _state[i] ^= Pack.LE_To_UInt64(_dataQueue, off);
                    off += 8;
                }

                if (partial > 0)
                {
                    var mask = (1UL << partial) - 1UL;
                    _state[full] ^= Pack.LE_To_UInt64(_dataQueue, off) & mask;
                }
            }

            _state[(_rate - 1) >> 6] ^= 1UL << 63;

            _bitsInQueue = 0;
            _squeezing = true;
        }

        protected void Squeeze(byte[] output, int offset, long outputLength)
        {
            if (!_squeezing) PadAndSwitchToSqueezingPhase();
            if ((outputLength & 7L) != 0L)
                throw new InvalidOperationException("outputLength not a multiple of 8");

            long i = 0;
            while (i < outputLength)
            {
                if (_bitsInQueue == 0) KeccakExtract();
                var partialBlock = (int)Math.Min(_bitsInQueue, outputLength - i);
                Array.Copy(_dataQueue, (_rate - _bitsInQueue) >> 3, output, offset + (int)(i >> 3), partialBlock >> 3);
                _bitsInQueue -= partialBlock;
                i += partialBlock;
            }
        }

        private void KeccakAbsorb(byte[] data, int off)
        {
            var count = _rate >> 6;
            for (var i = 0; i < count; ++i)
            {
                _state[i] ^= Pack.LE_To_UInt64(data, off);
                off += 8;
            }

            KeccakPermutation();
        }

        private void KeccakExtract()
        {
            KeccakPermutation();

            Pack.UInt64_To_LE(_state, 0, _rate >> 6, _dataQueue, 0);

            _bitsInQueue = _rate;
        }

        private void KeccakPermutation()
        {
            var a = _state;

            ulong a00 = a[0], a01 = a[1], a02 = a[2], a03 = a[3], a04 = a[4];
            ulong a05 = a[5], a06 = a[6], a07 = a[7], a08 = a[8], a09 = a[9];
            ulong a10 = a[10], a11 = a[11], a12 = a[12], a13 = a[13], a14 = a[14];
            ulong a15 = a[15], a16 = a[16], a17 = a[17], a18 = a[18], a19 = a[19];
            ulong a20 = a[20], a21 = a[21], a22 = a[22], a23 = a[23], a24 = a[24];

            for (var i = 0; i < 24; i++)
            {
                // theta
                var c0 = a00 ^ a05 ^ a10 ^ a15 ^ a20;
                var c1 = a01 ^ a06 ^ a11 ^ a16 ^ a21;
                var c2 = a02 ^ a07 ^ a12 ^ a17 ^ a22;
                var c3 = a03 ^ a08 ^ a13 ^ a18 ^ a23;
                var c4 = a04 ^ a09 ^ a14 ^ a19 ^ a24;

                var d1 = ((c1 << 1) | (c1 >> -1)) ^ c4;
                var d2 = ((c2 << 1) | (c2 >> -1)) ^ c0;
                var d3 = ((c3 << 1) | (c3 >> -1)) ^ c1;
                var d4 = ((c4 << 1) | (c4 >> -1)) ^ c2;
                var d0 = ((c0 << 1) | (c0 >> -1)) ^ c3;

                a00 ^= d1;
                a05 ^= d1;
                a10 ^= d1;
                a15 ^= d1;
                a20 ^= d1;
                a01 ^= d2;
                a06 ^= d2;
                a11 ^= d2;
                a16 ^= d2;
                a21 ^= d2;
                a02 ^= d3;
                a07 ^= d3;
                a12 ^= d3;
                a17 ^= d3;
                a22 ^= d3;
                a03 ^= d4;
                a08 ^= d4;
                a13 ^= d4;
                a18 ^= d4;
                a23 ^= d4;
                a04 ^= d0;
                a09 ^= d0;
                a14 ^= d0;
                a19 ^= d0;
                a24 ^= d0;

                // rho/pi
                c1 = (a01 << 1) | (a01 >> 63);
                a01 = (a06 << 44) | (a06 >> 20);
                a06 = (a09 << 20) | (a09 >> 44);
                a09 = (a22 << 61) | (a22 >> 3);
                a22 = (a14 << 39) | (a14 >> 25);
                a14 = (a20 << 18) | (a20 >> 46);
                a20 = (a02 << 62) | (a02 >> 2);
                a02 = (a12 << 43) | (a12 >> 21);
                a12 = (a13 << 25) | (a13 >> 39);
                a13 = (a19 << 8) | (a19 >> 56);
                a19 = (a23 << 56) | (a23 >> 8);
                a23 = (a15 << 41) | (a15 >> 23);
                a15 = (a04 << 27) | (a04 >> 37);
                a04 = (a24 << 14) | (a24 >> 50);
                a24 = (a21 << 2) | (a21 >> 62);
                a21 = (a08 << 55) | (a08 >> 9);
                a08 = (a16 << 45) | (a16 >> 19);
                a16 = (a05 << 36) | (a05 >> 28);
                a05 = (a03 << 28) | (a03 >> 36);
                a03 = (a18 << 21) | (a18 >> 43);
                a18 = (a17 << 15) | (a17 >> 49);
                a17 = (a11 << 10) | (a11 >> 54);
                a11 = (a07 << 6) | (a07 >> 58);
                a07 = (a10 << 3) | (a10 >> 61);
                a10 = c1;

                // chi
                c0 = a00 ^ (~a01 & a02);
                c1 = a01 ^ (~a02 & a03);
                a02 ^= ~a03 & a04;
                a03 ^= ~a04 & a00;
                a04 ^= ~a00 & a01;
                a00 = c0;
                a01 = c1;

                c0 = a05 ^ (~a06 & a07);
                c1 = a06 ^ (~a07 & a08);
                a07 ^= ~a08 & a09;
                a08 ^= ~a09 & a05;
                a09 ^= ~a05 & a06;
                a05 = c0;
                a06 = c1;

                c0 = a10 ^ (~a11 & a12);
                c1 = a11 ^ (~a12 & a13);
                a12 ^= ~a13 & a14;
                a13 ^= ~a14 & a10;
                a14 ^= ~a10 & a11;
                a10 = c0;
                a11 = c1;

                c0 = a15 ^ (~a16 & a17);
                c1 = a16 ^ (~a17 & a18);
                a17 ^= ~a18 & a19;
                a18 ^= ~a19 & a15;
                a19 ^= ~a15 & a16;
                a15 = c0;
                a16 = c1;

                c0 = a20 ^ (~a21 & a22);
                c1 = a21 ^ (~a22 & a23);
                a22 ^= ~a23 & a24;
                a23 ^= ~a24 & a20;
                a24 ^= ~a20 & a21;
                a20 = c0;
                a21 = c1;

                // iota
                a00 ^= KeccakRoundConstants[i];
            }

            a[0] = a00;
            a[1] = a01;
            a[2] = a02;
            a[3] = a03;
            a[4] = a04;
            a[5] = a05;
            a[6] = a06;
            a[7] = a07;
            a[8] = a08;
            a[9] = a09;
            a[10] = a10;
            a[11] = a11;
            a[12] = a12;
            a[13] = a13;
            a[14] = a14;
            a[15] = a15;
            a[16] = a16;
            a[17] = a17;
            a[18] = a18;
            a[19] = a19;
            a[20] = a20;
            a[21] = a21;
            a[22] = a22;
            a[23] = a23;
            a[24] = a24;
        }
    }
}