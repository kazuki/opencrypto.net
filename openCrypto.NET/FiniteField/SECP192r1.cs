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
	class SECP192r1 : GeneralizedMersennePrimeField
	{
		const uint P1 = 4294967295;
		const uint P2 = 4294967295;
		const uint P3 = 4294967294;
		const uint P4 = 4294967295;
		const uint P5 = 4294967295;
		const uint P6 = 4294967295;
		public static Number PRIME = new Number (new uint[] {P1, P2, P3, P4, P5, P6});
		static Number PADDED_ZERO = new Number (new uint[6], 1);

		public SECP192r1 () : base (PRIME)
		{
		}

		public override Number Add (Number x, Number y)
		{
			if (x.data.Length < 6 || y.data.Length < 6) return ToElement (base.Add (x, y));

			ulong sum;
			uint[] pz = new uint[7], px = x.data, py = y.data;
			uint tmp, carry;

			pz[0] = (uint)(sum = ((ulong)px[0]) + ((ulong)py[0])); sum >>= 32;
			pz[1] = (uint)(sum += ((ulong)px[1]) + ((ulong)py[1])); sum >>= 32;
			pz[2] = (uint)(sum += ((ulong)px[2]) + ((ulong)py[2])); sum >>= 32;
			pz[3] = (uint)(sum += ((ulong)px[3]) + ((ulong)py[3])); sum >>= 32;
			pz[4] = (uint)(sum += ((ulong)px[4]) + ((ulong)py[4])); sum >>= 32;
			pz[5] = (uint)(sum += ((ulong)px[5]) + ((ulong)py[5])); sum >>= 32;
			pz[6] = (uint)sum;

			if (pz[6] != 0 || CompareTo (pz[0], pz[1], pz[2], pz[3], pz[4], pz[5]) <= 0) {
				carry = ((pz[0] -= P1) > ~P1 ? 1U : 0U);
				tmp = P2 + carry; carry = (tmp < carry | (pz[1] -= tmp) > ~tmp ? 1U : 0U);
				tmp = P3 + carry; carry = (tmp < carry | (pz[2] -= tmp) > ~tmp ? 1U : 0U);
				tmp = P4 + carry; carry = (tmp < carry | (pz[3] -= tmp) > ~tmp ? 1U : 0U);
				tmp = P5 + carry; carry = (tmp < carry | (pz[4] -= tmp) > ~tmp ? 1U : 0U);
				pz[5] -= P6 + carry;
			}
			pz[6] = 0;
			return new Number (pz);
		}

		public override Number Subtract (Number x, Number y)
		{
			if (x.data.Length < 6 || y.data.Length < 6)
				return ToElement (base.Subtract (x, y));

			int cmp = x.CompareTo (y);
			if (cmp == 0)
				return PADDED_ZERO;
			uint[] pz = new uint[6], px = x.data, py = y.data;
			if (cmp > 0) {
				uint tmp, carry;
				tmp = py[0]; carry = ((pz[0] = px[0] - tmp) > ~tmp ? 1U : 0U);
				tmp = py[1] + carry; carry = (tmp < carry | (pz[1] = px[1] - tmp) > ~tmp ? 1U : 0U);
				tmp = py[2] + carry; carry = (tmp < carry | (pz[2] = px[2] - tmp) > ~tmp ? 1U : 0U);
				tmp = py[3] + carry; carry = (tmp < carry | (pz[3] = px[3] - tmp) > ~tmp ? 1U : 0U);
				tmp = py[4] + carry; carry = (tmp < carry | (pz[4] = px[4] - tmp) > ~tmp ? 1U : 0U);
				pz[5] = px[5] - py[5] - carry;
			} else {
				long tmp;
				int carry = 0;
				pz[0] = (uint)(tmp = ((long)px[0]) + ((long)P1) - ((long)py[0]) - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[1] = (uint)(tmp = ((long)px[1]) + ((long)P2) - ((long)py[1]) - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[2] = (uint)(tmp = ((long)px[2]) + ((long)P3) - ((long)py[2]) - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[3] = (uint)(tmp = ((long)px[3]) + ((long)P4) - ((long)py[3]) - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[4] = (uint)(tmp = ((long)px[4]) + ((long)P5) - ((long)py[4]) - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[5] = (uint)(tmp = ((long)px[5]) + ((long)P6) - ((long)py[5]) - carry);
			}
			return new Number (pz);
		}

		public override Number Multiply (Number x, Number y)
		{
			return Multiply_with_Modulo (x, y);
			//return Multiply_Safe (x, y);
		}

		public unsafe Number Multiply_with_Modulo (Number x, Number y)
		{
#if false
			uint* px = stackalloc uint[6];
			uint* py = stackalloc uint[6];
			for (int i = 0; i < x.length; i++) px[i] = x.data[i];
			for (int i = x.length; i < 6; i++) px[i] = 0;
			for (int i = 0; i < y.length; i++) py[i] = y.data[i];
			for (int i = y.length; i < 6; i++) py[i] = 0;
#else
			uint[] px = x.data, py = y.data;
			if (x.data.Length < 6 || y.data.Length < 6) throw new ArgumentException ();
#endif
			ulong tmp;
			uint* r = stackalloc uint[6];
			ulong tmp1, tmp2;
			uint r6, r7, z1;

			r[0] = (uint)(tmp =  px[0] * ((ulong)py[0])); tmp >>= 32;
			r[1] = (uint)(tmp += px[1] * ((ulong)py[0])); tmp >>= 32;
			r[2] = (uint)(tmp += px[2] * ((ulong)py[0])); tmp >>= 32;
			r[3] = (uint)(tmp += px[3] * ((ulong)py[0])); tmp >>= 32;
			r[4] = (uint)(tmp += px[4] * ((ulong)py[0])); tmp >>= 32;
			r[5] = (uint)(tmp += px[5] * ((ulong)py[0]));
			r6   = (uint)(tmp >> 32);

			tmp1 = px[5] * ((ulong)py[1]);
			r[0] = (uint)(tmp =  tmp1 +                   ((ulong)r[0])); tmp >>= 32;
			r[1] = (uint)(tmp += px[0] * ((ulong)py[1]) + ((ulong)r[1])); tmp >>= 32;
			r[2] = (uint)(tmp += tmp1 +                   ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[2] * ((ulong)py[1]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[3] * ((ulong)py[1]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[4] * ((ulong)py[1]) + ((ulong)r[5]));
			z1   = (uint)(tmp >> 32);

			tmp1 = px[4] * ((ulong)py[2]);
			tmp2 = px[5] * ((ulong)py[2]);
			r[0] = (uint)(tmp =  tmp1 +                   ((ulong)r[0])); tmp >>= 32;
			r[1] = (uint)(tmp += tmp2 +                   ((ulong)r[1])); tmp >>= 32;
			r[2] = (uint)(tmp += tmp1 +                   ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += tmp2 +                   ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[2] * ((ulong)py[2]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[3] * ((ulong)py[2]) + ((ulong)r[5])); tmp >>= 32;
			r6   = (uint)(tmp += z1 +                     ((ulong)r6  ));
			r7   = (uint)(tmp >> 32);

			tmp1 = px[4] * ((ulong)py[3]);
			tmp2 = px[5] * ((ulong)py[3]);
			r[0] = (uint)(tmp =  px[3] * ((ulong)py[3]) + ((ulong)r[0])); tmp >>= 32;
			r[1] = (uint)(tmp += tmp1 +                   ((ulong)r[1])); tmp >>= 32;
			r[2] = (uint)(tmp += tmp2 +                   ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += tmp1 +                   ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += tmp2 +                   ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[2] * ((ulong)py[3]) + ((ulong)r[5]));
			z1   = (uint)(tmp >> 32);

			tmp1 = px[4] * ((ulong)py[4]);
			tmp2 = px[5] * ((ulong)py[4]);
			r[0] = (uint)(tmp =  px[2] * ((ulong)py[4]) + ((ulong)r[0])); tmp >>= 32;
			r[1] = (uint)(tmp += px[3] * ((ulong)py[4]) + ((ulong)r[1])); tmp >>= 32;
			r[2] = (uint)(tmp += tmp1 +                   ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += tmp2 +                   ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += tmp1 +                   ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += tmp2 +                   ((ulong)r[5])); tmp >>= 32;
			r6   = (uint)(tmp += z1 +                     ((ulong)r6  ));
			r7  += (uint)(tmp >> 32);

			tmp1 = px[5] * ((ulong)py[5]);
			tmp2 = px[4] * ((ulong)py[5]);
			r[0] = (uint)(tmp =  tmp1                   + ((ulong)r[0])); tmp >>= 32;
			r[1] = (uint)(tmp += px[2] * ((ulong)py[5]) + ((ulong)r[1])); tmp >>= 32;
			r[2] = (uint)(tmp += tmp1 +                   ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += tmp2 +                   ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += tmp1 +                   ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += tmp2 +                   ((ulong)r[5]));
			z1   = (uint)(tmp >> 32);
			
			r[2] = (uint)(tmp =  px[1] * ((ulong)py[1]) + ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[1] * ((ulong)py[2]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[1] * ((ulong)py[3]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[1] * ((ulong)py[4]) + ((ulong)r[5])); tmp >>= 32;
			r6   = (uint)(tmp += z1 +                     ((ulong)r6  ));
			r7  += (uint)(tmp >> 32);

			r[2] = (uint)(tmp =  px[0] * ((ulong)py[2]) + ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[0] * ((ulong)py[3]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[0] * ((ulong)py[4]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[0] * ((ulong)py[5]) + ((ulong)r[5]));
			z1   = (uint)(tmp >> 32);

			r[2] = (uint)(tmp =  px[3] * ((ulong)py[3]) + ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[3] * ((ulong)py[4]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[3] * ((ulong)py[5]) + ((ulong)r[4]));
			tmp2 = tmp >> 32;

			tmp1 = px[1] * ((ulong)py[5]);
			r[0] = (uint)(tmp  = tmp1 +                   ((ulong)r[0])); tmp >>= 32;
			r[1] = (uint)(tmp +=                          ((ulong)r[1])); tmp >>= 32;
			r[2] = (uint)(tmp += px[3] * ((ulong)py[5]) + ((ulong)r[2])); tmp >>= 32;
			r[2] = (uint)(tmp1 +=                         ((ulong)r[2])); tmp1 >>= 32;
			r[3] = (uint)(tmp1 += tmp +                   ((ulong)r[3])); tmp1 >>= 32;
			
			r[2] = (uint)(tmp  = px[2] * ((ulong)py[4]) + ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[2] * ((ulong)py[5]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += tmp1 +                   ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += tmp2 +                   ((ulong)r[5])); tmp >>= 32;
			r6   = (uint)(tmp += z1 +                     ((ulong)r6  ));
			r7  += (uint)(tmp >> 32);

			while (r6 != 0 || r7 != 0) {
				r[0] = (uint)(tmp =  r6 + ((ulong)r[0])); tmp >>= 32;
				r[1] = (uint)(tmp += r7 + ((ulong)r[1])); tmp >>= 32;
				r[2] = (uint)(tmp += r6 + ((ulong)r[2])); tmp >>= 32;
				r[3] = (uint)(tmp += r7 + ((ulong)r[3])); tmp >>= 32;
				r[4] = (uint)(tmp +=      ((ulong)r[4])); tmp >>= 32;
				r[5] = (uint)(tmp +=      ((ulong)r[5]));
				r6 = (uint)(tmp >> 32);
				r7 = 0;
			}

			Number ret = new Number (new uint[] { r[0], r[1], r[2], r[3], r[4], r[5] });
			if (CompareTo (r[0], r[1], r[2], r[3], r[4], r[5]) < 0) {
				ret.SubtractInPlace (PRIME);
			}
			return ret;
		}

		public Number Multiply_Safe (Number x, Number y)
		{
			if (x.data.Length < 6 || y.data.Length < 6) throw new ArgumentException ();

			ulong tmp;
			uint[] r = new uint[12], px = x.data, py = y.data;

			r[0] = (uint)(tmp = px[0] * ((ulong)py[0])); tmp >>= 32;
			r[1] = (uint)(tmp += px[0] * ((ulong)py[1])); tmp >>= 32;
			r[2] = (uint)(tmp += px[0] * ((ulong)py[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[0] * ((ulong)py[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[0] * ((ulong)py[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[0] * ((ulong)py[5])); tmp >>= 32;
			r[6] = (uint)tmp;

			r[1] = (uint)(tmp = px[1] * ((ulong)py[0]) + ((ulong)r[1])); tmp >>= 32;
			r[2] = (uint)(tmp += px[1] * ((ulong)py[1]) + ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[1] * ((ulong)py[2]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[1] * ((ulong)py[3]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[1] * ((ulong)py[4]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[1] * ((ulong)py[5]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)tmp;

			r[2] = (uint)(tmp = px[2] * ((ulong)py[0]) + ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[2] * ((ulong)py[1]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[2] * ((ulong)py[2]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[2] * ((ulong)py[3]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[2] * ((ulong)py[4]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[2] * ((ulong)py[5]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)tmp;

			r[3] = (uint)(tmp = px[3] * ((ulong)py[0]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[3] * ((ulong)py[1]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[3] * ((ulong)py[2]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[3] * ((ulong)py[3]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[3] * ((ulong)py[4]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[3] * ((ulong)py[5]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)tmp;

			r[4] = (uint)(tmp = px[4] * ((ulong)py[0]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[4] * ((ulong)py[1]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[4] * ((ulong)py[2]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[4] * ((ulong)py[3]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[4] * ((ulong)py[4]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)(tmp += px[4] * ((ulong)py[5]) + ((ulong)r[9])); tmp >>= 32;
			r[10] = (uint)tmp;

			r[5] = (uint)(tmp = px[5] * ((ulong)py[0]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[5] * ((ulong)py[1]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[5] * ((ulong)py[2]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[5] * ((ulong)py[3]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)(tmp += px[5] * ((ulong)py[4]) + ((ulong)r[9])); tmp >>= 32;
			r[10] = (uint)(tmp += px[5] * ((ulong)py[5]) + ((ulong)r[10])); tmp >>= 32;
			r[11] = (uint)tmp;

			if (r[11] == 0 && r[10] == 0 && r[9] == 0 && r[8] == 0 && r[7] == 0 && r[6] == 0) {
				int cmp = CompareTo (r[0], r[1], r[2], r[3], r[4], r[5]);
				if (cmp == 0)
					return Number.Zero;
				else if (cmp > 0)
					return new Number (r);
			}
			Reduction (r);
			r[6] = r[7] = r[8] = r[9] = r[10] = r[11] = 0;
			return new Number (r);
		}

		int CompareTo (uint x1, uint x2, uint x3, uint x4, uint x5, uint x6)
		{
			int cmp;
			if ((cmp = P6.CompareTo (x6)) != 0) return cmp;
			if ((cmp = P5.CompareTo (x5)) != 0) return cmp;
			if ((cmp = P4.CompareTo (x4)) != 0) return cmp;
			if ((cmp = P3.CompareTo (x3)) != 0) return cmp;
			if ((cmp = P2.CompareTo (x2)) != 0) return cmp;
			if ((cmp = P1.CompareTo (x1)) != 0) return cmp;
			return 0;
		}

		public override Number Invert (Number x)
		{
			return ToElement (base.Invert (x));
		}

		void Reduction (uint[] x)
		{
			ulong
			carry = ((ulong)x[0]) + ((ulong)x[6]) + ((ulong)x[10]);
			uint x1 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[1]) + ((ulong)x[7]) + ((ulong)x[11]);
			uint x2 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[2]) + ((ulong)x[6]) + ((ulong)x[8]) + ((ulong)x[10]);
			uint x3 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[3]) + ((ulong)x[7]) + ((ulong)x[9]) + ((ulong)x[11]);
			uint x4 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[4]) + ((ulong)x[8]) + ((ulong)x[10]);
			uint x5 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[5]) + ((ulong)x[9]) + ((ulong)x[11]);
			uint x6 = (uint)carry;
			uint x7 = (uint)(carry >> 32);

			while (x7 != 0 || CompareTo (x1, x2, x3, x4, x5, x6) <= 0) {
				uint tmp, tmp2 = ((x1 -= P1) > ~P1 ? 1U : 0U);
				tmp = P2 + tmp2; tmp2 = (tmp < tmp2 | (x2 -= tmp) > ~tmp ? 1U : 0U);
				tmp = P3 + tmp2; tmp2 = (tmp < tmp2 | (x3 -= tmp) > ~tmp ? 1U : 0U);
				tmp = P4 + tmp2; tmp2 = (tmp < tmp2 | (x4 -= tmp) > ~tmp ? 1U : 0U);
				tmp = P5 + tmp2; tmp2 = (tmp < tmp2 | (x5 -= tmp) > ~tmp ? 1U : 0U);
				tmp = P6 + tmp2; tmp2 = (tmp < tmp2 | (x6 -= tmp) > ~tmp ? 1U : 0U);
				x7 -= tmp2;
			}

			x[0] = x1;
			x[1] = x2;
			x[2] = x3;
			x[3] = x4;
			x[4] = x5;
			x[5] = x6;
		}

		public override Number ToElement (Number x)
		{
			if (x.length == 6 || (x.length < 6 && x.data.Length >= 6))
				return x;
			if (x.length > 6)
				throw new ArgumentOutOfRangeException ();
			uint[] tmp = new uint[6];
			for (int i = 0; i < x.length; i++)
				tmp[i] = x.data[i];
			return new Number (tmp);
		}

		public override ECPoint GetInfinityPoint (ECGroup group)
		{
			return new ECPoint (group, PADDED_ZERO, PADDED_ZERO, PADDED_ZERO);
		}
	}
}
