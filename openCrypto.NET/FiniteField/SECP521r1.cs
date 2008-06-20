// 
// Copyright (c) 2008, Kazuki Oikawa
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using openCrypto.EllipticCurve;

namespace openCrypto.FiniteField
{
	class SECP521r1 : GeneralizedMersennePrimeField
	{
		const uint P1_16 = 4294967295;
		const uint P17 = 511;
		public static Number PRIME = new Number (new uint[] {P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P1_16, P17});

		public SECP521r1 () : base (PRIME)
		{
		}

		public override unsafe Number Multiply (Number x, Number y)
		{
			uint* pz = stackalloc uint[33];
			uint* high = stackalloc uint[17];
			uint* low = pz;

			// pz = x * y
			for (int i = 0; i < 33; i ++) pz[i] = 0;
			fixed (uint* px = x.data, py = y.data) {
				Number.Multiply (px, x.length, py, y.length, pz);
			}

			// low = pz & (2^521 - 1)
			// high = pz >> 521
			//for (int i = 0; i < 16; i ++) high[i] = (pz[i + 16] >> 9) | (pz[i + 17] << 23);
			high[0] = (pz[16] >> 9) | (pz[17] << 23);
			high[1] = (pz[17] >> 9) | (pz[18] << 23);
			high[2] = (pz[18] >> 9) | (pz[19] << 23);
			high[3] = (pz[19] >> 9) | (pz[20] << 23);
			high[4] = (pz[20] >> 9) | (pz[21] << 23);
			high[5] = (pz[21] >> 9) | (pz[22] << 23);
			high[6] = (pz[22] >> 9) | (pz[23] << 23);
			high[7] = (pz[23] >> 9) | (pz[24] << 23);
			high[8] = (pz[24] >> 9) | (pz[25] << 23);
			high[9] = (pz[25] >> 9) | (pz[26] << 23);
			high[10] = (pz[26] >> 9) | (pz[27] << 23);
			high[11] = (pz[27] >> 9) | (pz[28] << 23);
			high[12] = (pz[28] >> 9) | (pz[29] << 23);
			high[13] = (pz[29] >> 9) | (pz[30] << 23);
			high[14] = (pz[30] >> 9) | (pz[31] << 23);
			high[15] = (pz[31] >> 9) | (pz[32] << 23);
			high[16] = pz[32] >> 9;
			low[16] &= 0x1FF;
			low[17] = low[18] = 0;

			fixed (uint* pPrime = PRIME.data) {
				// ret = low + high
				Number.AddInPlace (low, 17, high, 17);

				// compute length of ret
				int retLen = 17;
				while (retLen > 0 && low[retLen - 1] == 0) retLen--;

				// ret -= PRIME if ret is greater than PRIME
				while (retLen == 17 && CompareToPrime (low) > 0)
					retLen = Number.SubtractInPlace (low, 17, pPrime, 17);

				return new Number (low, retLen);
			}
		}

		static unsafe int CompareToPrime (uint* x)
		{
			if (x[16] != P17) {
				if (x[16] < P17)
					return -1;
				return 1;
			}
			int idx = 15;
			while (idx != 0 && x[idx] == P1_16) idx--;
			if (x[idx] < P1_16)
				return -1;
			else if (x[idx] > P1_16)
				return 1;
			return 0;
		}
	}
}
