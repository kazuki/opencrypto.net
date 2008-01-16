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
	class Classical : IFiniteField
	{
		protected Number mod;

		#region Constructor
		public Classical (Number mod)
		{
			this.mod = mod;
		}
		#endregion

		#region IFiniteField Members

		public virtual unsafe Number Add (Number x, Number y)
		{
			uint[] z = new uint[(x.length >= y.length ? x.length : y.length) + 1];
			int zlen;
			fixed (uint* px = x.data, py = y.data, pz = z) {
				zlen = Number.Add (px, x.length, py, y.length, pz);
				if (Number.Compare (z, zlen, mod.data, mod.length) >= 0) {
					fixed (uint* pm = mod.data) {
						zlen = Number.Subtract (pz, zlen, pm, mod.length, pz);
					}
				}
			}
			return new Number (z, zlen);
		}

		public virtual unsafe Number Subtract (Number x, Number y)
		{
			uint[] z = new uint[mod.length + 1];
			int ret = Number.Compare (x.data, x.length, y.data, y.length);
			int zlen;
			if (ret == 0)
				return new Number (new uint[1], 1);
			if (ret < 0) {
				fixed (uint* px = x.data, py = y.data, pz = z, pm = mod.data) {
					zlen = Number.Add (px, x.length, pm, mod.length, pz);
					zlen = Number.Subtract (pz, zlen, py, y.length, pz);
					return new Number (z, zlen);
				}
			} else {
				fixed (uint* px = x.data, py = y.data, pz = z) {
					zlen = Number.Subtract (px, x.length, py, y.length, pz);
					return new Number (z, zlen);
				}
			}
		}

		public virtual Number Multiply (Number x, Number y)
		{
			return (x * y) % mod;
		}

		public virtual Number Invert (Number x)
		{
			Number a = mod;
			Number b = x;
			Number p0 = Number.Zero;
			Number p1 = Number.One;
			Number q0 = Number.Zero;
			Number q1 = Number.Zero;
			Number r0 = Number.Zero;
			Number r1 = Number.Zero;
			int step = 0;

			while (!b.IsZero ()) {
				if (step > 1) {
					Number temp = Subtract (p0, (p1 * q0) % mod);
					p0 = p1; p1 = temp;
				}

				Number[] ret = Number.Divide (a, b);
				q0 = q1;
				q1 = ret[0];
				r0 = r1;
				r1 = ret[1];
				a = b;
				b = ret[1];
				step++;
			}

			if (r0.data[0] != 1)
				throw new ArithmeticException ();

			return Subtract (p0, (p1 * q0) % mod);
		}

		public virtual Number ToElement (Number x)
		{
			return x;
		}

		public virtual Number ToNormal (Number x)
		{
			return x;
		}

		public virtual Number Modulus {
			get { return mod; }
		}

		public virtual ECPoint ExportECPoint (Number x, Number y, Number z, ECGroup group)
		{
			if (z.IsOne ())
				return new ECPoint (group, x, y, z);
			Number iz = Invert (z);
			Number z2 = Multiply (iz, iz);
			return new ECPoint (group, Multiply (x, z2), Multiply (y, Multiply (z2, iz)), Number.One);
		}

		public virtual ECPoint GetInfinityPoint (ECGroup group)
		{
			return new ECPoint (group, Number.Zero, Number.Zero, Number.Zero);
		}

		#endregion
	}
}
