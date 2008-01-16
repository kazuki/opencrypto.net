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
	class Montgomery : Classical
	{
		uint invMod;
		Number montOne = null;

		#region Constructor
		public Montgomery (Number mod) : base (mod)
		{
			this.invMod = Inverse (mod.data[0]);
		}

		static uint Inverse (uint n)
		{
			uint y = n, z;

			while ((z = n * y) != 1)
				y *= 2 - z;

			return (uint)-y;
		}
		#endregion

		#region IFiniteField Members
		public override unsafe Number Multiply (Number x, Number y)
		{
			if (x.IsZero () || y.IsZero ())
				return Number.Zero;
			if (x.length < y.length) {
				Number swap = x;
				x = y;
				y = swap;
			}
			uint[] z = new uint[mod.length + 1];
			fixed (uint* px = x.data, py = y.data, pz = z, pp = mod.data) {
				uint y0 = py[0];
				int i = 0;
				for (; i < x.length; i++) {
					uint u = (uint)((pz[0] + px[i] * y0) * invMod);
					uint xi = px[i];
					ulong carry1 = ((ulong)xi) * ((ulong)py[0]);
					ulong carry2 = ((ulong)u) * ((ulong)pp[0]);
					ulong carry = (((ulong)(uint)carry1) + ((ulong)(uint)carry2) + ((ulong)pz[0])) >> 32;
					carry1 >>= 32; carry2 >>= 32;
					int k = 1;
					for (; k < y.length; k++) {
						carry1 += ((ulong)xi) * ((ulong)py[k]);
						carry2 += ((ulong)u) * ((ulong)pp[k]);
						carry += ((ulong)(uint)carry1) + ((ulong)(uint)carry2) + ((ulong)pz[k]);
						pz[k - 1] = (uint)carry;
						carry >>= 32; carry1 >>= 32; carry2 >>= 32;
					}
					for (; carry1 != 0 && k < mod.length; k++) {
						carry2 += ((ulong)u) * ((ulong)pp[k]);
						carry += ((ulong)(uint)carry1) + ((ulong)(uint)carry2) + ((ulong)pz[k]);
						pz[k - 1] = (uint)carry;
						carry >>= 32; carry1 >>= 32; carry2 >>= 32;
					}
					for (; k < mod.length; k++) {
						carry2 += ((ulong)u) * ((ulong)pp[k]);
						carry += ((ulong)(uint)carry2) + ((ulong)pz[k]);
						pz[k - 1] = (uint)carry;
						carry >>= 32; carry2 >>= 32;
					}
					carry += carry1 + carry2 + ((ulong)pz[k]);
					pz[k - 1] = (uint)carry;
					pz[k] = (uint)(carry >> 32);
				}
				for (; i < mod.length; i++) {
					uint u = (uint)(pz[0] * invMod);
					ulong carry = (((ulong)u) * ((ulong)pp[0]) + ((ulong)pz[0])) >> 32;
					int k = 1;
					for (; k < mod.length; k++) {
						carry += ((ulong)u) * ((ulong)pp[k]) + ((ulong)pz[k]);
						pz[k - 1] = (uint)carry;
						carry >>= 32;
					}
					carry += ((ulong)pz[k]);
					pz[k - 1] = (uint)carry;
					pz[k] = (uint)(carry >> 32);
				}
				if (pz[mod.length] != 0 || Number.Compare (z, mod.length, mod.data, mod.length) >= 0)
					return new Number (z, Number.Subtract (pz, z.Length, pp, mod.length, pz));
				return new Number (z);
			}
		}

		public override Number ToElement (Number x)
		{
			Number tmp = x << (mod.length << 5);
			return tmp % mod;
		}

		public override Number ToNormal (Number x)
		{
			return Multiply (x, Number.One);
		}

		public override Number Invert (Number x)
		{
			return ToElement (base.Invert (Multiply (x, Number.One)));
		}

		public override ECPoint ExportECPoint (Number x, Number y, Number z, ECGroup group)
		{
			if (montOne == null) montOne = ToElement (Number.One);
			if (z.CompareTo (montOne) == 0)
				return new ECPoint (group, Multiply (x, Number.One), Multiply (y, Number.One), Number.One);

			Number izm = Invert (z);
			Number iz = Multiply (izm, Number.One);
			Number z2 = Multiply (iz, izm);
			return new ECPoint (group, Multiply (x, z2), Multiply (y, (Multiply (z2, izm))), Number.One);
		}
		#endregion
	}
}
