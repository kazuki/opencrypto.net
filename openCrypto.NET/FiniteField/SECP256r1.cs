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
	class SECP256r1 : GeneralizedMersennePrimeField
	{
		const uint P1 = 4294967295;
		const uint P2 = 4294967295;
		const uint P3 = 4294967295;
		const uint P4 = 0;
		const uint P5 = 0;
		const uint P6 = 0;
		const uint P7 = 1;
		const uint P8 = 4294967295;
		public static Number PRIME = new Number (new uint[] { P1, P2, P3, P4, P5, P6, P7, P8 });
		static Number PADDED_ZERO = new Number (new uint[8], 1);

		public SECP256r1 () : base (PRIME)
		{
		}

		public override Number Add (Number x, Number y)
		{
			ulong sum;
			uint[] pz = new uint[9], px = x.data, py = y.data;
			uint tmp, carry;

			if (x.data.Length < 8 || y.data.Length < 8)
				return ToElement (base.Add (x, y));

			pz[0] = (uint)(sum = ((ulong)px[0]) + ((ulong)py[0])); sum >>= 32;
			pz[1] = (uint)(sum += ((ulong)px[1]) + ((ulong)py[1])); sum >>= 32;
			pz[2] = (uint)(sum += ((ulong)px[2]) + ((ulong)py[2])); sum >>= 32;
			pz[3] = (uint)(sum += ((ulong)px[3]) + ((ulong)py[3])); sum >>= 32;
			pz[4] = (uint)(sum += ((ulong)px[4]) + ((ulong)py[4])); sum >>= 32;
			pz[5] = (uint)(sum += ((ulong)px[5]) + ((ulong)py[5])); sum >>= 32;
			pz[6] = (uint)(sum += ((ulong)px[6]) + ((ulong)py[6])); sum >>= 32;
			pz[7] = (uint)(sum += ((ulong)px[7]) + ((ulong)py[7])); sum >>= 32;
			pz[8] = (uint)sum;

			if (pz[8] != 0 || CompareTo (pz[0], pz[1], pz[2], pz[3], pz[4], pz[5], pz[6], pz[7]) <= 0) {
				carry = ((pz[0] -= P1) > ~P1 ? 1U : 0U);
				tmp = P2 + carry; carry = (tmp < carry | (pz[1] -= tmp) > ~tmp ? 1U : 0U);
				tmp = P3 + carry; carry = (tmp < carry | (pz[2] -= tmp) > ~tmp ? 1U : 0U);
				tmp = P4 + carry; carry = (tmp < carry | (pz[3] -= tmp) > ~tmp ? 1U : 0U);
				tmp = P5 + carry; carry = (tmp < carry | (pz[4] -= tmp) > ~tmp ? 1U : 0U);
				tmp = P6 + carry; carry = (tmp < carry | (pz[5] -= tmp) > ~tmp ? 1U : 0U);
				tmp = P7 + carry; carry = (tmp < carry | (pz[6] -= tmp) > ~tmp ? 1U : 0U);
				pz[7] -= P8 + carry;
			}
			pz[8] = 0;
			return new Number (pz);
		}

		public override Number Subtract (Number x, Number y)
		{
			if (x.data.Length < 8 || y.data.Length < 8)
				return ToElement (base.Subtract (x, y));
			int cmp = x.CompareTo (y);
			if (cmp == 0)
				return PADDED_ZERO;
			uint[] px = x.data, py = y.data, pz = new uint[8];
			if (cmp > 0) {
				uint tmp, carry;
				carry = ((pz[0] = px[0] - py[0]) > ~py[0] ? 1U : 0U);
				tmp = py[1] + carry; carry = (tmp < carry | (pz[1] = px[1] - tmp) > ~tmp ? 1U : 0U);
				tmp = py[2] + carry; carry = (tmp < carry | (pz[2] = px[2] - tmp) > ~tmp ? 1U : 0U);
				tmp = py[3] + carry; carry = (tmp < carry | (pz[3] = px[3] - tmp) > ~tmp ? 1U : 0U);
				tmp = py[4] + carry; carry = (tmp < carry | (pz[4] = px[4] - tmp) > ~tmp ? 1U : 0U);
				tmp = py[5] + carry; carry = (tmp < carry | (pz[5] = px[5] - tmp) > ~tmp ? 1U : 0U);
				tmp = py[6] + carry; carry = (tmp < carry | (pz[6] = px[6] - tmp) > ~tmp ? 1U : 0U);
				pz[7] = px[7] - py[7] - carry;
			} else {
				long tmp;
				int carry = 0;
				pz[0] = (uint)(tmp = ((long)px[0]) + ((long)P1) - py[0] - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[1] = (uint)(tmp = ((long)px[1]) + ((long)P2) - py[1] - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[2] = (uint)(tmp = ((long)px[2]) + ((long)P3) - py[2] - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[3] = (uint)(tmp = ((long)px[3]) + ((long)P4) - py[3] - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[4] = (uint)(tmp = ((long)px[4]) + ((long)P5) - py[4] - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[5] = (uint)(tmp = ((long)px[5]) + ((long)P6) - py[5] - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[6] = (uint)(tmp = ((long)px[6]) + ((long)P7) - py[6] - carry); carry = (tmp < 0 ? 1 : tmp > 0xFFFFFFFF ? -1 : 0);
				pz[7] = (uint)(tmp = ((long)px[7]) + ((long)P8) - py[7] - carry);
			}
			return new Number (pz);
		}

#if true
		public override Number Multiply (Number x, Number y)
		{
			if (x.data.Length < 8 || y.data.Length < 8) throw new ArgumentException ();
			uint[] px = x.data, py = y.data;
			ulong r0, r1, r2, r3, r4, r5, r6, r7;
			uint tmp32;
			ulong tmp, tmp1, tmp2, tmp3, tmp4, tmp5, tmp6, tmp7;
			ulong d1, d2, d3, d4, d5;
			ulong triple1, triple2;
			const ulong mask = 0xFFFFFFFF;
			const ulong carry = 0x100000000UL;
			const ulong negative = ulong.MaxValue - (((ulong)uint.MaxValue) * 16) * 8;

			tmp = ((ulong)px[0]) * ((ulong)py[0]); r0 = tmp & mask; r1 = tmp >> 32;
			tmp = ((ulong)px[1]) * ((ulong)py[0]); r1 += tmp & mask; r2 = tmp >> 32;
			tmp = ((ulong)px[2]) * ((ulong)py[0]); r2 += tmp & mask; r3 = tmp >> 32;
			tmp = ((ulong)px[3]) * ((ulong)py[0]); r3 += tmp & mask; r4 = tmp >> 32;
			tmp = ((ulong)px[4]) * ((ulong)py[0]); r4 += tmp & mask; r5 = tmp >> 32;
			tmp = ((ulong)px[5]) * ((ulong)py[0]); r5 += tmp & mask; r6 = tmp >> 32;
			tmp = ((ulong)px[6]) * ((ulong)py[0]); r6 += tmp & mask; r7 = tmp >> 32;
			tmp = ((ulong)px[7]) * ((ulong)py[0]); r7 += tmp & mask; tmp32 = (uint)(tmp >> 32);
			r7 += tmp32;
			r6 -= tmp32;
			r3 -= tmp32;
			r0 += tmp32;

			tmp = ((ulong)px[0]) * ((ulong)py[1]); r1 += tmp & mask; r2 += tmp >> 32;
			tmp = ((ulong)px[1]) * ((ulong)py[1]); r2 += tmp & mask; r3 += tmp >> 32;
			tmp = ((ulong)px[2]) * ((ulong)py[1]); r3 += tmp & mask; r4 += tmp >> 32;
			tmp = ((ulong)px[3]) * ((ulong)py[1]); r4 += tmp & mask; r5 += tmp >> 32;
			tmp = ((ulong)px[4]) * ((ulong)py[1]); r5 += tmp & mask; r6 += tmp >> 32;
			tmp = ((ulong)px[5]) * ((ulong)py[1]); r6 += tmp & mask; r7 += tmp >> 32;
			tmp = ((ulong)px[6]) * ((ulong)py[1]); r7 += tmp & mask; tmp1 = (uint)(tmp >> 32);
			tmp = ((ulong)px[7]) * ((ulong)py[1]); tmp1 += (uint)tmp; tmp32 = (uint)(tmp >> 32);
			r7 += tmp1;
			r6 -= tmp1 + tmp32;
			r4 -= tmp32;
			r3 -= tmp1 + tmp32;
			r1 += tmp32;
			r0 += tmp1 + tmp32;

			tmp = ((ulong)px[0]) * ((ulong)py[2]); r2 += tmp & mask; r3 += tmp >> 32;
			tmp = ((ulong)px[1]) * ((ulong)py[2]); r3 += tmp & mask; r4 += tmp >> 32;
			tmp = ((ulong)px[2]) * ((ulong)py[2]); r4 += tmp & mask; r5 += tmp >> 32;
			tmp = ((ulong)px[3]) * ((ulong)py[2]); r5 += tmp & mask; r6 += tmp >> 32;
			tmp = ((ulong)px[4]) * ((ulong)py[2]); r6 += tmp & mask; r7 += tmp >> 32;
			tmp = ((ulong)px[5]) * ((ulong)py[2]); r7 += tmp & mask; tmp1 = (uint)(tmp >> 32);
			tmp = ((ulong)px[6]) * ((ulong)py[2]); tmp1 += (uint)tmp; tmp2 = (uint)(tmp >> 32);
			tmp = ((ulong)px[7]) * ((ulong)py[2]); tmp2 += (uint)tmp; tmp32 = (uint)(tmp >> 32);
			r7 += tmp1 - tmp32;
			r6 -= tmp1 + tmp2;
			r5 -= tmp32;
			r4 -= tmp2 + tmp32;
			r3 -= tmp1 + tmp2;
			r2 += tmp32;
			r1 += tmp2 + tmp32;
			r0 += tmp1 + tmp2;

			tmp = ((ulong)px[0]) * ((ulong)py[3]); r3 += tmp & mask; r4 += tmp >> 32;
			tmp = ((ulong)px[1]) * ((ulong)py[3]); r4 += tmp & mask; r5 += tmp >> 32;
			tmp = ((ulong)px[2]) * ((ulong)py[3]); r5 += tmp & mask; r6 += tmp >> 32;
			tmp = ((ulong)px[3]) * ((ulong)py[3]); r6 += tmp & mask; r7 += tmp >> 32;
			tmp = ((ulong)px[4]) * ((ulong)py[3]); r7 += tmp & mask; tmp1 = (uint)(tmp >> 32);
			tmp = ((ulong)px[5]) * ((ulong)py[3]); tmp1 += (uint)tmp; tmp2 = (uint)(tmp >> 32);
			tmp = ((ulong)px[6]) * ((ulong)py[3]); tmp2 += (uint)tmp; tmp3 = (uint)(tmp >> 32);
			tmp = ((ulong)px[7]) * ((ulong)py[3]); tmp3 += (uint)tmp; tmp32 = (uint)(tmp >> 32);
			d1 = ((ulong)tmp32) << 1;
			r7 += tmp1 - tmp3 - tmp32;
			r6 -= tmp1 + tmp2;
			r5 -= tmp3 + tmp32;
			r4 -= tmp2 + tmp3;
			r3 -= tmp1 + tmp2 - d1;
			r2 += tmp3 + tmp32;
			r1 += tmp2 + tmp3;
			r0 += tmp1 + tmp2 - tmp32;

			tmp = ((ulong)px[0]) * ((ulong)py[4]); r4 += tmp & mask; r5 += tmp >> 32;
			tmp = ((ulong)px[1]) * ((ulong)py[4]); r5 += tmp & mask; r6 += tmp >> 32;
			tmp = ((ulong)px[2]) * ((ulong)py[4]); r6 += tmp & mask; r7 += tmp >> 32;
			tmp = ((ulong)px[3]) * ((ulong)py[4]); r7 += tmp & mask; tmp1 = (uint)(tmp >> 32);
			tmp = ((ulong)px[4]) * ((ulong)py[4]); tmp1 += (uint)tmp; tmp2 = (uint)(tmp >> 32);
			tmp = ((ulong)px[5]) * ((ulong)py[4]); tmp2 += (uint)tmp; tmp3 = (uint)(tmp >> 32);
			tmp = ((ulong)px[6]) * ((ulong)py[4]); tmp3 += (uint)tmp; tmp4 = (uint)(tmp >> 32);
			tmp = ((ulong)px[7]) * ((ulong)py[4]); tmp4 += (uint)tmp; tmp32 = (uint)(tmp >> 32);
			d1 = tmp4 << 1;
			d2 = ((ulong)tmp32) << 1;
			r7 += tmp1 - tmp3 - tmp4 - tmp32;
			r6 -= tmp1 + tmp2;
			r5 -= tmp3 + tmp4;
			r4 -= tmp2 + tmp3 - d2;
			r3 -= tmp1 + tmp2 - d1 - d2;
			r2 += tmp3 + tmp4;
			r1 += tmp2 + tmp3 - tmp32;
			r0 += tmp1 + tmp2 - tmp4 - tmp32;

			tmp = ((ulong)px[0]) * ((ulong)py[5]); r5 += tmp & mask; r6 += tmp >> 32;
			tmp = ((ulong)px[1]) * ((ulong)py[5]); r6 += tmp & mask; r7 += tmp >> 32;
			tmp = ((ulong)px[2]) * ((ulong)py[5]); r7 += tmp & mask; tmp1 = (uint)(tmp >> 32);
			tmp = ((ulong)px[3]) * ((ulong)py[5]); tmp1 += (uint)tmp; tmp2 = (uint)(tmp >> 32);
			tmp = ((ulong)px[4]) * ((ulong)py[5]); tmp2 += (uint)tmp; tmp3 = (uint)(tmp >> 32);
			tmp = ((ulong)px[5]) * ((ulong)py[5]); tmp3 += (uint)tmp; tmp4 = (uint)(tmp >> 32);
			tmp = ((ulong)px[6]) * ((ulong)py[5]); tmp4 += (uint)tmp; tmp5 = (uint)(tmp >> 32);
			tmp = ((ulong)px[7]) * ((ulong)py[5]); tmp5 += (uint)tmp; tmp32 = (uint)(tmp >> 32);
			d1 = tmp4 << 1;
			d2 = tmp5 << 1;
			d3 = ((ulong)tmp32) << 1;
			r7 += tmp1 - tmp3 - tmp4 - tmp5 - tmp32;
			r6 -= tmp1 + tmp2 - tmp32;
			r5 -= tmp3 + tmp4 - d3;
			r4 -= tmp2 + tmp3 - d2 - d3;
			r3 -= tmp1 + tmp2 - d1 - d2 - tmp32;
			r2 += tmp3 + tmp4 - tmp32;
			r1 += tmp2 + tmp3 - tmp5 - tmp32;
			r0 += tmp1 + tmp2 - tmp4 - tmp5 - tmp32;

			tmp = ((ulong)px[0]) * ((ulong)py[6]); r6 += tmp & mask; r7 += tmp >> 32;
			tmp = ((ulong)px[1]) * ((ulong)py[6]); r7 += tmp & mask; tmp1 = (uint)(tmp >> 32);
			tmp = ((ulong)px[2]) * ((ulong)py[6]); tmp1 += (uint)tmp; tmp2 = (uint)(tmp >> 32);
			tmp = ((ulong)px[3]) * ((ulong)py[6]); tmp2 += (uint)tmp; tmp3 = (uint)(tmp >> 32);
			tmp = ((ulong)px[4]) * ((ulong)py[6]); tmp3 += (uint)tmp; tmp4 = (uint)(tmp >> 32);
			tmp = ((ulong)px[5]) * ((ulong)py[6]); tmp4 += (uint)tmp; tmp5 = (uint)(tmp >> 32);
			tmp = ((ulong)px[6]) * ((ulong)py[6]); tmp5 += (uint)tmp; tmp6 = (uint)(tmp >> 32);
			tmp = ((ulong)px[7]) * ((ulong)py[6]); tmp6 += (uint)tmp; tmp32 = (uint)(tmp >> 32);
			d1 = tmp4 << 1;
			d2 = tmp5 << 1;
			d3 = tmp6 << 1;
			d4 = ((ulong)tmp32) << 1;
			triple1 = d4 + tmp32;
			r7 += tmp1 - tmp3 - tmp4 - tmp5 - tmp6;
			r6 -= tmp1 + tmp2 - tmp6 - triple1;
			r5 -= tmp3 + tmp4 - d3 - d4;
			r4 -= tmp2 + tmp3 - d2 - d3 - tmp32;
			r3 -= tmp1 + tmp2 - d1 - d2 - tmp6;
			r2 += tmp3 + tmp4 - tmp6 - tmp32;
			r1 += tmp2 + tmp3 - tmp5 - tmp6 - tmp32;
			r0 += tmp1 + tmp2 - tmp4 - tmp5 - tmp6 - tmp32;

			tmp = ((ulong)px[0]) * ((ulong)py[7]); r7 += tmp & mask; tmp1 = (uint)(tmp >> 32);
			tmp = ((ulong)px[1]) * ((ulong)py[7]); tmp1 += (uint)tmp; tmp2 = (uint)(tmp >> 32);
			tmp = ((ulong)px[2]) * ((ulong)py[7]); tmp2 += (uint)tmp; tmp3 = (uint)(tmp >> 32);
			tmp = ((ulong)px[3]) * ((ulong)py[7]); tmp3 += (uint)tmp; tmp4 = (uint)(tmp >> 32);
			tmp = ((ulong)px[4]) * ((ulong)py[7]); tmp4 += (uint)tmp; tmp5 = (uint)(tmp >> 32);
			tmp = ((ulong)px[5]) * ((ulong)py[7]); tmp5 += (uint)tmp; tmp6 = (uint)(tmp >> 32);
			tmp = ((ulong)px[6]) * ((ulong)py[7]); tmp6 += (uint)tmp; tmp7 = (uint)(tmp >> 32);
			tmp = ((ulong)px[7]) * ((ulong)py[7]); tmp7 += (uint)tmp; tmp32 = (uint)(tmp >> 32);
			d1 = tmp4 << 1;
			d2 = tmp5 << 1;
			d3 = tmp6 << 1;
			d4 = tmp7 << 1;
			d5 = ((ulong)tmp32) << 1;
			triple1 = d4 + tmp7;
			triple2 = d5 + tmp32;
			r7 += tmp1 - tmp3 - tmp4 - tmp5 - tmp6 + triple2;
			r6 -= tmp1 + tmp2 - tmp6 - triple1 - d5;
			r5 -= tmp3 + tmp4 - d3 - d4 - tmp32;
			r4 -= tmp2 + tmp3 - d2 - d3 - tmp7;
			r3 -= tmp1 + tmp2 - d1 - d2 - tmp6 + tmp32;
			r2 += tmp3 + tmp4 - tmp6 - tmp7 - tmp32;
			r1 += tmp2 + tmp3 - tmp5 - tmp6 - tmp7 - tmp32;
			r0 += tmp1 + tmp2 - tmp4 - tmp5 - tmp6 - tmp7;

			// check negative-value
			while (r0 >= negative) { r1--; r0 += carry; }
			while (r1 >= negative) { r2--; r1 += carry; }
			while (r2 >= negative) { r3--; r2 += carry; }
			while (r3 >= negative) { r4--; r3 += carry; }
			while (r4 >= negative) { r5--; r4 += carry; }
			while (r5 >= negative) { r6--; r5 += carry; }
			while (r6 >= negative) { r7--; r6 += carry; }
			while (r7 >= negative) {
				r0 += P1;
				r1 += P2;
				r2 += P3;
				r3 += P4;
				r4 += P5;
				r5 += P6;
				r6 += P7;
				r7 += P8;
			}

			// check carry
			while (r0 > mask || r1 > mask || r2 > mask || r3 > mask || r4 > mask || r5 > mask || r6 > mask || r7 > mask) {
				if (r7 > mask) {
					tmp32 = (uint)(r7 >> 32);
					r0 += tmp32;
					r3 -= tmp32;
					r6 -= tmp32;
					r7 = tmp32 + (ulong)((uint)r7);

					// check negative-value
					while (r3 >= negative) { r4--; r3 += carry; }
					while (r4 >= negative) { r5--; r4 += carry; }
					while (r5 >= negative) { r6--; r5 += carry; }
					while (r6 >= negative) { r7--; r6 += carry; }
				}
				tmp32 = (uint)(r0 >> 32); r0 = (uint)r0; r1 += tmp32;
				tmp32 = (uint)(r1 >> 32); r1 = (uint)r1; r2 += tmp32;
				tmp32 = (uint)(r2 >> 32); r2 = (uint)r2; r3 += tmp32;
				tmp32 = (uint)(r3 >> 32); r3 = (uint)r3; r4 += tmp32;
				tmp32 = (uint)(r4 >> 32); r4 = (uint)r4; r5 += tmp32;
				tmp32 = (uint)(r5 >> 32); r5 = (uint)r5; r6 += tmp32;
				tmp32 = (uint)(r6 >> 32); r6 = (uint)r6; r7 += tmp32;
			}

			Number ret = new Number (new uint[] {
				(uint)r0, (uint)r1, (uint)r2,
				(uint)r3, (uint)r4, (uint)r5,
				(uint)r6, (uint)r7
			});
			while (ret.CompareTo (PRIME) >= 0)
				ret.SubtractInPlace (PRIME);
			return ret;
		}
#else
		public override Number Multiply (Number x, Number y)
		{
			if (x.data.Length < 8 || y.data.Length < 8) throw new ArgumentException ();

			ulong tmp;
			uint[] r = new uint[16], px = x.data, py = y.data;

			r[0] = (uint)(tmp = px[0] * ((ulong)py[0])); tmp >>= 32;
			r[1] = (uint)(tmp += px[0] * ((ulong)py[1])); tmp >>= 32;
			r[2] = (uint)(tmp += px[0] * ((ulong)py[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[0] * ((ulong)py[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[0] * ((ulong)py[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[0] * ((ulong)py[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[0] * ((ulong)py[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[0] * ((ulong)py[7])); tmp >>= 32;
			r[8] = (uint)tmp;

			r[1] = (uint)(tmp = px[1] * ((ulong)py[0]) + ((ulong)r[1])); tmp >>= 32;
			r[2] = (uint)(tmp += px[1] * ((ulong)py[1]) + ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[1] * ((ulong)py[2]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[1] * ((ulong)py[3]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[1] * ((ulong)py[4]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[1] * ((ulong)py[5]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[1] * ((ulong)py[6]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[1] * ((ulong)py[7]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)tmp;

			r[2] = (uint)(tmp = px[2] * ((ulong)py[0]) + ((ulong)r[2])); tmp >>= 32;
			r[3] = (uint)(tmp += px[2] * ((ulong)py[1]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[2] * ((ulong)py[2]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[2] * ((ulong)py[3]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[2] * ((ulong)py[4]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[2] * ((ulong)py[5]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[2] * ((ulong)py[6]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)(tmp += px[2] * ((ulong)py[7]) + ((ulong)r[9])); tmp >>= 32;
			r[10] = (uint)tmp;

			r[3] = (uint)(tmp = px[3] * ((ulong)py[0]) + ((ulong)r[3])); tmp >>= 32;
			r[4] = (uint)(tmp += px[3] * ((ulong)py[1]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[3] * ((ulong)py[2]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[3] * ((ulong)py[3]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[3] * ((ulong)py[4]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[3] * ((ulong)py[5]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)(tmp += px[3] * ((ulong)py[6]) + ((ulong)r[9])); tmp >>= 32;
			r[10] = (uint)(tmp += px[3] * ((ulong)py[7]) + ((ulong)r[10])); tmp >>= 32;
			r[11] = (uint)tmp;

			r[4] = (uint)(tmp = px[4] * ((ulong)py[0]) + ((ulong)r[4])); tmp >>= 32;
			r[5] = (uint)(tmp += px[4] * ((ulong)py[1]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[4] * ((ulong)py[2]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[4] * ((ulong)py[3]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[4] * ((ulong)py[4]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)(tmp += px[4] * ((ulong)py[5]) + ((ulong)r[9])); tmp >>= 32;
			r[10] = (uint)(tmp += px[4] * ((ulong)py[6]) + ((ulong)r[10])); tmp >>= 32;
			r[11] = (uint)(tmp += px[4] * ((ulong)py[7]) + ((ulong)r[11])); tmp >>= 32;
			r[12] = (uint)tmp;

			r[5] = (uint)(tmp = px[5] * ((ulong)py[0]) + ((ulong)r[5])); tmp >>= 32;
			r[6] = (uint)(tmp += px[5] * ((ulong)py[1]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[5] * ((ulong)py[2]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[5] * ((ulong)py[3]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)(tmp += px[5] * ((ulong)py[4]) + ((ulong)r[9])); tmp >>= 32;
			r[10] = (uint)(tmp += px[5] * ((ulong)py[5]) + ((ulong)r[10])); tmp >>= 32;
			r[11] = (uint)(tmp += px[5] * ((ulong)py[6]) + ((ulong)r[11])); tmp >>= 32;
			r[12] = (uint)(tmp += px[5] * ((ulong)py[7]) + ((ulong)r[12])); tmp >>= 32;
			r[13] = (uint)tmp;

			r[6] = (uint)(tmp = px[6] * ((ulong)py[0]) + ((ulong)r[6])); tmp >>= 32;
			r[7] = (uint)(tmp += px[6] * ((ulong)py[1]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[6] * ((ulong)py[2]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)(tmp += px[6] * ((ulong)py[3]) + ((ulong)r[9])); tmp >>= 32;
			r[10] = (uint)(tmp += px[6] * ((ulong)py[4]) + ((ulong)r[10])); tmp >>= 32;
			r[11] = (uint)(tmp += px[6] * ((ulong)py[5]) + ((ulong)r[11])); tmp >>= 32;
			r[12] = (uint)(tmp += px[6] * ((ulong)py[6]) + ((ulong)r[12])); tmp >>= 32;
			r[13] = (uint)(tmp += px[6] * ((ulong)py[7]) + ((ulong)r[13])); tmp >>= 32;
			r[14] = (uint)tmp;

			r[7] = (uint)(tmp = px[7] * ((ulong)py[0]) + ((ulong)r[7])); tmp >>= 32;
			r[8] = (uint)(tmp += px[7] * ((ulong)py[1]) + ((ulong)r[8])); tmp >>= 32;
			r[9] = (uint)(tmp += px[7] * ((ulong)py[2]) + ((ulong)r[9])); tmp >>= 32;
			r[10] = (uint)(tmp += px[7] * ((ulong)py[3]) + ((ulong)r[10])); tmp >>= 32;
			r[11] = (uint)(tmp += px[7] * ((ulong)py[4]) + ((ulong)r[11])); tmp >>= 32;
			r[12] = (uint)(tmp += px[7] * ((ulong)py[5]) + ((ulong)r[12])); tmp >>= 32;
			r[13] = (uint)(tmp += px[7] * ((ulong)py[6]) + ((ulong)r[13])); tmp >>= 32;
			r[14] = (uint)(tmp += px[7] * ((ulong)py[7]) + ((ulong)r[14])); tmp >>= 32;
			r[15] = (uint)tmp;

			if (r[15] == 0 && r[14] == 0 && r[13] == 0 && r[12] == 0 && r[11] == 0 && r[10] == 0 && r[9] == 0 && r[8] == 0) {
				int cmp = CompareTo (r[0], r[1], r[2], r[3], r[4], r[5], r[6], r[7]);
				if (cmp == 0)
					return Number.Zero;
				else if (cmp > 0)
					return new Number (r);
			}
			Reduction (r);
			r[8] = r[9] = r[10] = r[11] = r[12] = r[13] = r[14] = r[15] = 0;
			return new Number (r);
		}

		void Reduction (uint[] x)
		{
			ulong
			carry = ((ulong)x[0]) + ((ulong)x[8]) + ((ulong)x[9]);
			uint x11 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[1]) + ((ulong)x[9]) + ((ulong)x[10]);
			uint x12 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[2]) + ((ulong)x[10]) + ((ulong)x[11]);
			uint x13 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[3]) + ((ulong)(uint)(x[11] << 1)) + ((ulong)(uint)(x[12] << 1)) + ((ulong)x[13]);
			uint x14 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[4]) + ((ulong)(uint)((x[12] << 1) | (x[11] >> 31))) + ((ulong)(uint)((x[13] << 1) | (x[12] >> 31))) + ((ulong)x[14]);
			uint x15 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[5]) + ((ulong)(uint)((x[13] << 1) | (x[12] >> 31))) + ((ulong)(uint)((x[14] << 1) | (x[13] >> 31))) + ((ulong)x[15]);
			uint x16 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[6]) + ((ulong)(uint)((x[14] << 1) | (x[13] >> 31))) + ((ulong)(uint)((x[15] << 1) | (x[14] >> 31))) + ((ulong)x[14]) + ((ulong)x[13]);
			uint x17 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[7]) + ((ulong)(uint)((x[15] << 1) | (x[14] >> 31))) + ((ulong)(uint)(x[15] >> 31)) + ((ulong)x[15]) + ((ulong)x[8]);
			uint x18 = (uint)carry;
			uint x19 = (uint)((carry >> 32) + (x[15] >> 31));

			carry = ((ulong)x[11]) + ((ulong)x[12]) + ((ulong)x[13]) + ((ulong)x[14]);
			uint x21 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[12]) + ((ulong)x[13]) + ((ulong)x[14]) + ((ulong)x[15]);
			uint x22 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[13]) + ((ulong)x[14]) + ((ulong)x[15]);
			uint x23 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[15]) + ((ulong)x[8]) + ((ulong)x[9]);
			uint x24 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[9]) + ((ulong)x[10]);
			uint x25 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[10]) + ((ulong)x[11]);
			uint x26 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[8]) + ((ulong)x[9]);
			uint x27 = (uint)carry; carry >>= 32;
			carry += ((ulong)x[10]) + ((ulong)x[11]) + ((ulong)x[12]) + ((ulong)x[13]);
			uint x28 = (uint)carry;
			uint x29 = (uint)(carry >> 32);

			int cmp;
			if ((cmp = x19.CompareTo (x29)) != 0) goto EndCompare;
			if ((cmp = x18.CompareTo (x28)) != 0) goto EndCompare;
			if ((cmp = x17.CompareTo (x27)) != 0) goto EndCompare;
			if ((cmp = x16.CompareTo (x26)) != 0) goto EndCompare;
			if ((cmp = x15.CompareTo (x25)) != 0) goto EndCompare;
			if ((cmp = x14.CompareTo (x24)) != 0) goto EndCompare;
			if ((cmp = x13.CompareTo (x23)) != 0) goto EndCompare;
			if ((cmp = x12.CompareTo (x22)) != 0) goto EndCompare;
			if ((cmp = x11.CompareTo (x21)) != 0) goto EndCompare;
		EndCompare:

			if (cmp == 0) {
				x[0] = x[1] = x[2] = x[3] = x[4] = x[5] = x[6] = x[7] = 0;
				return;
			} else if (cmp > 0) {
				uint tmp, tmp2;
				tmp2 = ((x[0] = x11 - x21) > ~x21 ? 1U : 0U);
				tmp = x22 + tmp2; tmp2 = (tmp < tmp2 | (x[1] = x12 - tmp) > ~tmp ? 1U : 0U);
				tmp = x23 + tmp2; tmp2 = (tmp < tmp2 | (x[2] = x13 - tmp) > ~tmp ? 1U : 0U);
				tmp = x24 + tmp2; tmp2 = (tmp < tmp2 | (x[3] = x14 - tmp) > ~tmp ? 1U : 0U);
				tmp = x25 + tmp2; tmp2 = (tmp < tmp2 | (x[4] = x15 - tmp) > ~tmp ? 1U : 0U);
				tmp = x26 + tmp2; tmp2 = (tmp < tmp2 | (x[5] = x16 - tmp) > ~tmp ? 1U : 0U);
				tmp = x27 + tmp2; tmp2 = (tmp < tmp2 | (x[6] = x17 - tmp) > ~tmp ? 1U : 0U);
				tmp = x28 + tmp2; tmp2 = (tmp < tmp2 | (x[7] = x18 - tmp) > ~tmp ? 1U : 0U);
				x[8] = x19 - x29 - tmp2;
				while (x[8] != 0 || CompareTo (x[0], x[1], x[2], x[3], x[4], x[5], x[6], x[7]) <= 0) {
					tmp2 = ((x[0] -= P1) > ~P1 ? 1U : 0U);
					tmp = P2 + tmp2; tmp2 = (tmp < tmp2 | (x[1] -= tmp) > ~tmp ? 1U : 0U);
					tmp = P3 + tmp2; tmp2 = (tmp < tmp2 | (x[2] -= tmp) > ~tmp ? 1U : 0U);
					tmp = P4 + tmp2; tmp2 = (tmp < tmp2 | (x[3] -= tmp) > ~tmp ? 1U : 0U);
					tmp = P5 + tmp2; tmp2 = (tmp < tmp2 | (x[4] -= tmp) > ~tmp ? 1U : 0U);
					tmp = P6 + tmp2; tmp2 = (tmp < tmp2 | (x[5] -= tmp) > ~tmp ? 1U : 0U);
					tmp = P7 + tmp2; tmp2 = (tmp < tmp2 | (x[6] -= tmp) > ~tmp ? 1U : 0U);
					tmp = P8 + tmp2; tmp2 = (tmp < tmp2 | (x[7] -= tmp) > ~tmp ? 1U : 0U);
					x[8] -= tmp2;
				}
			} else {
				while (true) {
					carry = ((ulong)x11) + ((ulong)P1); x11 = (uint)carry; carry >>= 32;
					carry += ((ulong)x12) + ((ulong)P2); x12 = (uint)carry; carry >>= 32;
					carry += ((ulong)x13) + ((ulong)P3); x13 = (uint)carry; carry >>= 32;
					carry += ((ulong)x14) + ((ulong)P4); x14 = (uint)carry; carry >>= 32;
					carry += ((ulong)x15) + ((ulong)P5); x15 = (uint)carry; carry >>= 32;
					carry += ((ulong)x16) + ((ulong)P6); x16 = (uint)carry; carry >>= 32;
					carry += ((ulong)x17) + ((ulong)P7); x17 = (uint)carry; carry >>= 32;
					carry += ((ulong)x18) + ((ulong)P8); x18 = (uint)carry; carry >>= 32;
					x19 += (uint)carry;

					if ((cmp = x19.CompareTo (x29)) != 0) goto EndCompare2;
					if ((cmp = x18.CompareTo (x28)) != 0) goto EndCompare2;
					if ((cmp = x17.CompareTo (x27)) != 0) goto EndCompare2;
					if ((cmp = x16.CompareTo (x26)) != 0) goto EndCompare2;
					if ((cmp = x15.CompareTo (x25)) != 0) goto EndCompare2;
					if ((cmp = x14.CompareTo (x24)) != 0) goto EndCompare2;
					if ((cmp = x13.CompareTo (x23)) != 0) goto EndCompare2;
					if ((cmp = x12.CompareTo (x22)) != 0) goto EndCompare2;
					if ((cmp = x11.CompareTo (x21)) != 0) goto EndCompare2;

				EndCompare2:
					if (cmp >= 0)
						break;
				}
				goto EndCompare;
			}
		}
#endif

		int CompareTo (uint x1, uint x2, uint x3, uint x4, uint x5, uint x6, uint x7, uint x8)
		{
			int cmp;
			if ((cmp = P8.CompareTo (x8)) != 0) return cmp;
			if ((cmp = P7.CompareTo (x7)) != 0) return cmp;
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

		public override Number ToElement (Number x)
		{
			if (x.length == 8 || (x.length < 8 && x.data.Length >= 8))
				return x;
			if (x.length > 8)
				throw new ArgumentOutOfRangeException ();
			uint[] tmp = new uint[8];
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
