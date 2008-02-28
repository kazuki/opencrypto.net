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
	class SECP384r1 : GeneralizedMersennePrimeField
	{
		const uint P1 = 4294967295;
		const uint P2 = 0;
		const uint P3 = P2;
		const uint P4 = P1;
		const uint P5 = 4294967294;
		const uint P6 = P1;
		const uint P7 = P1;
		const uint P8 = P1;
		const uint P9 = P1;
		const uint P10 = P1;
		const uint P11 = P1;
		const uint P12 = P1;
		public static Number PRIME = new Number (new uint[] {P1, P2, P3, P4, P5, P6, P7, P8, P9, P10, P11, P12});

		public SECP384r1 () : base (PRIME)
		{
		}

		public unsafe override Number Multiply (Number x, Number y)
		{
			uint* z = stackalloc uint[24];
			uint* s1 = stackalloc uint[13];
			uint* s2 = stackalloc uint[13];
			ulong t;

			// z = x * y
			for (int i = x.length + y.length; i < 24; i++) z[i] = 0;
			fixed (uint* px = x.data, py = y.data) {
				Number.Multiply (px, x.length, py, y.length, z);
			}

			// s1 = (z11, z10,  z9,  z8,  z7,  z6,  z5,  z4,  z3,  z2,  z1,  z0)
			//    + (  0,   0,   0,   0,   0, z23, z22, z21,   0,   0,   0,   0)
			//    + (z23, z22, z21, z20, z19, z18, z17, z16, z15, z14, z13, z12)
			//    + (z20, z19, z18, z17, z16, z15, z14, z13, z12, z23, z22, z21)
			//    + (z19, z18, z17, z16, z15, z14, z13, z12, z20,   0, z23,   0)
			//    + (  0,   0,   0,   0, z23, z22, z21, z20,   0,   0,   0,   0)
			//    + (  0,   0,   0,   0,   0,   0, z23, z22, z21,   0,   0, z20)
			//
			// s2 = (z22, z21, z20, z19, z18, z17, z16, z15, z14, z13, z12, z23)
			//    + (  0,   0,   0,   0,   0,   0,   0, z23, z22, z21, z20,   0)
			//    + (  0,   0,   0,   0,   0,   0,   0, z23, z23,   0,   0,   0)
			#region Compute s1
			t = ((ulong)z[0]) + ((ulong)z[12]) + ((ulong)z[21]) + ((ulong)z[20]);
			s1[0] = (uint)t; t >>= 32;
			t += ((ulong)z[1]) + ((ulong)z[13]) + ((ulong)z[22]) + ((ulong)z[23]);
			s1[1] = (uint)t; t >>= 32;
			t += ((ulong)z[2]) + ((ulong)z[14]) + ((ulong)z[23]);
			s1[2] = (uint)t; t >>= 32;
			t += ((ulong)z[3]) + ((ulong)z[15]) + ((ulong)z[12]) + ((ulong)z[20]) + ((ulong)z[21]);
			s1[3] = (uint)t; t >>= 32;
			t += ((ulong)z[4]) + (((ulong)z[21]) << 1) + ((ulong)z[16]) + ((ulong)z[13]) + ((ulong)z[12]) + ((ulong)z[20]) + ((ulong)z[22]);
			s1[4] = (uint)t; t >>= 32;
			t += ((ulong)z[5]) + (((ulong)z[22]) << 1) + ((ulong)z[17]) + ((ulong)z[14]) + ((ulong)z[13]) + ((ulong)z[21]) + ((ulong)z[23]);
			s1[5] = (uint)t; t >>= 32;
			t += ((ulong)z[6]) + (((ulong)z[23]) << 1) + ((ulong)z[18]) + ((ulong)z[15]) + ((ulong)z[14]) + ((ulong)z[22]);
			s1[6] = (uint)t; t >>= 32;
			t += ((ulong)z[7]) + ((ulong)z[19]) + ((ulong)z[16]) + ((ulong)z[15]) + ((ulong)z[23]);
			s1[7] = (uint)t; t >>= 32;
			t += ((ulong)z[8]) + ((ulong)z[20]) + ((ulong)z[17]) + ((ulong)z[16]);
			s1[8] = (uint)t; t >>= 32;
			t += ((ulong)z[9]) + ((ulong)z[21]) + ((ulong)z[18]) + ((ulong)z[17]);
			s1[9] = (uint)t; t >>= 32;
			t += ((ulong)z[10]) + ((ulong)z[22]) + ((ulong)z[19]) + ((ulong)z[18]);
			s1[10] = (uint)t; t >>= 32;
			t += ((ulong)z[11]) + ((ulong)z[23]) + ((ulong)z[20]) + ((ulong)z[19]);
			s1[11] = (uint)t; s1[12] = (uint)(t >> 32);
			#endregion

			#region Compute s2
			s2[0] = z[23];
			t = ((ulong)z[12]) + ((ulong)z[20]);
			s2[1] = (uint)t; t >>= 32;
			t += ((ulong)z[13]) + ((ulong)z[21]);
			s2[2] = (uint)t; t >>= 32;
			t += ((ulong)z[14]) + ((ulong)z[22]) + ((ulong)z[23]);
			s2[3] = (uint)t; t >>= 32;
			t += ((ulong)z[15]) + ((ulong)z[23]) + ((ulong)z[23]);
			s2[4] = (uint)t; t >>= 32;
			t += ((ulong)z[16]); s2[5] = (uint)t; t >>= 32;
			t += ((ulong)z[17]); s2[6] = (uint)t; t >>= 32;
			t += ((ulong)z[18]); s2[7] = (uint)t; t >>= 32;
			t += ((ulong)z[19]); s2[8] = (uint)t; t >>= 32;
			t += ((ulong)z[20]); s2[9] = (uint)t; t >>= 32;
			t += ((ulong)z[21]); s2[10] = (uint)t; t >>= 32;
			t += ((ulong)z[22]); s2[11] = (uint)t; s2[12] = (uint)(t >> 32);
			#endregion

			int l1 = 13, l2 = 13;
			while (l1 > 0 && s1[l1 - 1] == 0) l1 --;
			while (l2 > 0 && s2[l2 - 1] == 0) l2 --;
			if (l1 == 0) l1 = 1;
			if (l2 == 0) l2 = 1;

			// s1 -= s2
			fixed (uint* pPrime = PRIME.data) {
				if (Compare (s1, s2) < 0)
					l1 = Number.AddInPlace (s1, l1, pPrime, PRIME.length);
				l1 = Number.SubtractInPlace (s1, l1, s2, l2);

				while (s1[12] != 0 || CompareToPrime (s1) >= 0)
					l1 = Number.SubtractInPlace (s1, l1, pPrime, PRIME.length);
			}

			return new Number (s1, l1);
		}

		static unsafe int CompareToPrime (uint* x)
		{
			int ret;
			if ((ret = x[11].CompareTo (P12)) != 0) return ret;
			if ((ret = x[10].CompareTo (P11)) != 0) return ret;
			if ((ret = x[9].CompareTo (P10)) != 0) return ret;
			if ((ret = x[8].CompareTo (P9)) != 0) return ret;
			if ((ret = x[7].CompareTo (P8)) != 0) return ret;
			if ((ret = x[6].CompareTo (P7)) != 0) return ret;
			if ((ret = x[5].CompareTo (P6)) != 0) return ret;
			if ((ret = x[4].CompareTo (P5)) != 0) return ret;
			if ((ret = x[3].CompareTo (P4)) != 0) return ret;
			if ((ret = x[2].CompareTo (P3)) != 0) return ret;
			if ((ret = x[1].CompareTo (P2)) != 0) return ret;
			return x[0].CompareTo (P1);
		}

		static unsafe int Compare (uint* x, uint* y)
		{
			int ret;
			if ((ret = x[12].CompareTo (y[12])) != 0) return ret;
			if ((ret = x[11].CompareTo (y[11])) != 0) return ret;
			if ((ret = x[10].CompareTo (y[10])) != 0) return ret;
			if ((ret = x[9].CompareTo (y[9])) != 0) return ret;
			if ((ret = x[8].CompareTo (y[8])) != 0) return ret;
			if ((ret = x[7].CompareTo (y[7])) != 0) return ret;
			if ((ret = x[6].CompareTo (y[6])) != 0) return ret;
			if ((ret = x[5].CompareTo (y[5])) != 0) return ret;
			if ((ret = x[4].CompareTo (y[4])) != 0) return ret;
			if ((ret = x[3].CompareTo (y[3])) != 0) return ret;
			if ((ret = x[2].CompareTo (y[2])) != 0) return ret;
			if ((ret = x[1].CompareTo (y[1])) != 0) return ret;
			return x[0].CompareTo (y[0]);
		}
	}
}
