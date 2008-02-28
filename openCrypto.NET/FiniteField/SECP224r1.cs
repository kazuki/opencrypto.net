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
	class SECP224r1 : GeneralizedMersennePrimeField
	{
		const uint P1 = 1;
		const uint P2_3 = 0;
		const uint P4_7 = 4294967295;
		public static Number PRIME = new Number (new uint[] {P1, P2_3, P2_3, P4_7, P4_7, P4_7, P4_7});

		public SECP224r1 () : base (PRIME)
		{
		}

		public unsafe override Number Multiply (Number x, Number y)
		{
			uint* z = stackalloc uint[14];
			uint* s1 = stackalloc uint[8];
			uint* s2 = stackalloc uint[8];
			ulong t;

			// z = x * y
			for (int i = 0; i < 14; i++) z[i] = 0;
			fixed (uint* px = x.data, py = y.data) {
				Number.Multiply (px, x.length, py, y.length, z);
			}

			// s1 = (z6,z5,z4,z3,z2,z1,z0) + (z10,z9,z8,z7,0,0,0) + (0,z13,z12,z11,0,0,0)
			s1[0] = z[0]; s1[1] = z[1]; s1[2] = z[2];
			t = ((ulong)z[3]) + ((ulong)z[7]) + ((ulong)z[11]);
			s1[3] = (uint)t; t >>= 32;
			t += ((ulong)z[4]) + ((ulong)z[8]) + ((ulong)z[12]);
			s1[4] = (uint)t; t >>= 32;
			t += ((ulong)z[5]) + ((ulong)z[9]) + ((ulong)z[13]);
			s1[5] = (uint)t; t >>= 32;
			t += ((ulong)z[6]) + ((ulong)z[10]);
			s1[6] = (uint)t; s1[7] = (uint)(t >> 32);

			// s2 = (z13,z12,z11,z10,z9,z8,z7) + (0,0,0,0,z13,z12,z11)
			t = ((ulong)z[7]) + ((ulong)z[11]);
			s2[0] = (uint)t; t >>= 32;
			t += ((ulong)z[8]) + ((ulong)z[12]);
			s2[1] = (uint)t; t >>= 32;
			t += ((ulong)z[9]) + ((ulong)z[13]);
			s2[2] = (uint)t; t >>= 32;
			s2[3] = (uint)(t += ((ulong)z[10])); t >>= 32;
			s2[4] = (uint)(t += ((ulong)z[11])); t >>= 32;
			s2[5] = (uint)(t += ((ulong)z[12])); t >>= 32;
			s2[6] = (uint)(t += ((ulong)z[13]));
			s2[7] = (uint)(t >> 32);

			int l1 = 8, l2 = 8;
			while (l1 > 0 && s1[l1 - 1] == 0) l1 --;
			while (l2 > 0 && s2[l2 - 1] == 0) l2 --;
			if (l1 == 0) l1 = 1;
			if (l2 == 0) l2 = 1;

			fixed (uint* pPrime = PRIME.data) {
				if (s1[7] != 0 || CompareToPrime (s1) >= 0)
					l1 = Number.SubtractInPlace (s1, l1, pPrime, 7);
				if (s2[7] != 0 || CompareToPrime (s2) >= 0)
					l2 = Number.SubtractInPlace (s2, l2, pPrime, 7);
				if (Compare (s1, s2) >= 0) {
					l1 = Number.SubtractInPlace (s1, l1, s2, l2);
				} else {
					l1 = Number.AddInPlace (s1, l1, pPrime, 7);
					l1 = Number.SubtractInPlace (s1, l1, s2, l2);
				}
				return new Number (s1, l1);
			}
		}

		static unsafe int Compare (uint* x, uint* y)
		{
			int ret;
			if ((ret = x[7].CompareTo (y[7])) != 0) return ret;
			if ((ret = x[6].CompareTo (y[6])) != 0) return ret;
			if ((ret = x[5].CompareTo (y[5])) != 0) return ret;
			if ((ret = x[4].CompareTo (y[4])) != 0) return ret;
			if ((ret = x[3].CompareTo (y[3])) != 0) return ret;
			if ((ret = x[2].CompareTo (y[2])) != 0) return ret;
			if ((ret = x[1].CompareTo (y[1])) != 0) return ret;
			return x[0].CompareTo (y[0]);
		}

		static unsafe int CompareToPrime (uint* x)
		{
			int ret;
			if ((ret = x[6].CompareTo (P4_7)) != 0) return ret;
			if ((ret = x[5].CompareTo (P4_7)) != 0) return ret;
			if ((ret = x[4].CompareTo (P4_7)) != 0) return ret;
			if ((ret = x[3].CompareTo (P4_7)) != 0) return ret;
			if ((ret = x[2].CompareTo (P2_3)) != 0) return ret;
			if ((ret = x[1].CompareTo (P2_3)) != 0) return ret;
			return x[0].CompareTo (P1);
		}
	}
}
