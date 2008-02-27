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

namespace openCrypto.FiniteField
{
	class Barrett : Classical
	{
		Number constant;

		#region Constructor
		public Barrett (Number mod) : base (mod)
		{
			int len = mod.length << 1;
			constant = new Number (len + 1);
			constant.data[len] = 1;
			constant = constant / mod;
		}
		#endregion

		#region IFiniteField Members
		public override Number Multiply (Number x, Number y)
		{
			Number z = x * y;
			if (z.length > mod.length << 1)
				z %= mod;
			else
				Reduce (z);
			return z;
		}
		#endregion

		#region Barrett Reduction
		public unsafe void Reduce (Number x)
		{
			int k = mod.length, kp1 = k + 1, km1 = k - 1;

			if (x.length < k)
				return;

			Number q = new Number (x.length - km1 + constant.length);
			Number r = new Number (kp1);
			fixed (uint* pq = q.data, pr = r.data, pm = mod.data, px = x.data, pc = constant.data) {
				Number.Multiply (px + km1, x.length - km1, pc, constant.length, pq);

				x.length = (x.length > kp1 ? kp1 : x.length);
				x.Normalize ();

				uint* xs = pq + kp1, xe = xs + q.length - kp1;
				uint* ys = pm, ye = ys + mod.length;
				uint* zs = pr, ze = zs + kp1;

				for (; xs < xe; xs++, zs++) {
					if (*xs == 0) continue;
					ulong carry = 0;
					uint* zp = zs;
					for (uint* yp = ys; yp < ye && zp < ze; yp++, zp++) {
						carry += ((ulong)*xs) * ((ulong)*yp) + ((ulong)*zp);
						*zp = (uint)carry;
						carry >>= 32;
					}
					if (carry != 0 && zp < ze)
						*zp = (uint)carry;
				}
				r.Normalize ();

				if (r.CompareTo (x) <= 0) {
					x.length = Number.Subtract (px, x.length, pr, r.length, px);
				} else {
					Number val = new Number (kp1 + 1);
					val.data[kp1] = 1;
					fixed (uint* pv = val.data) {
						val.length = Number.Subtract (pv, val.length, pr, r.length, pv);
						x.length = Number.Add (px, x.length, pv, val.length, px);
					}
				}
				x.Normalize ();

				while (x.CompareTo (mod) >= 0) {
					x.length = Number.Subtract (px, x.length, pm, mod.length, px);
				}
			}
		}
		#endregion
	}
}
