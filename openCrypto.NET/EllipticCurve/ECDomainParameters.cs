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
using openCrypto.FiniteField;

namespace openCrypto.EllipticCurve
{
	public class ECDomainParameters
	{
		ECGroup _group;
		ECPoint _G;
		Number _order;
		IFiniteField _fieldN;
		uint _h;
		uint _bits;
		Uri _uri;

		public ECDomainParameters (ECGroup group, ECPoint G, Number order, uint h, uint bits, IFiniteField fieldN)
			: this (group, G, order, h, bits, fieldN, null)
		{
		}

		public ECDomainParameters (ECGroup group, ECPoint G, Number order, uint h, uint bits, IFiniteField fieldN, Uri uri)
		{
			_group = group;
			_G = G;
			_order = order;
			_h = h;
			_bits = bits;
			_fieldN = fieldN;
			_uri = uri;
		}

		#region Properties
		public uint Bits {
			get { return _bits; }
		}

		public ECGroup Group {
			get { return _group; }
		}

		/// <summary>Base point</summary>
		public ECPoint G {
			get { return _G; }
		}

		/// <summary>Coefficient A (-3 was efficiency)</summary>
		public Number A {
			get { return _group.A; }
		}

		/// <summary>Coefficient B</summary>
		public Number B {
			get { return _group.B; }
		}

		/// <summary>The order of the field.</summary>
		public Number P {
			get { return _group.P; }
		}

		public IFiniteField FieldN {
			get { return _fieldN; }
		}

		/// <summary>The order of base point.</summary>
		public Number N {
			get { return _order; }
		}

		/// <summary>The co-factor</summary>
		public uint H {
			get { return _h; }
		}

		public Uri URN {
			get { return _uri; }
		}
		#endregion

		#region Methods
		/// <summary>
		/// TODO: 未実装のValidationステップを実装する
		/// </summary>
		public bool Validate ()
		{
			IFiniteField ff = _group.FiniteField;

			// Step1: Check that p is an odd prime
			// Step2: Check that a,b,Gx and Gy are integers in the interval [0, p - 1]
			ECPoint ExportedG = _G.Export ();
			Number Gx = ff.ToElement (ExportedG.X);
			Number Gy = ff.ToElement (ExportedG.Y);
			if (A > P || B > P || Gx > P || Gy > P)
				return false;

			// Step3: Check that 4*a^3 + 27*b^2 != 0 (mod p)
			Number Apow3 = ff.Multiply (A, ff.Multiply (A, A));
			Number Bpow2 = ff.Multiply (B, B);
			Number ret = ff.Add (ff.Multiply (ff.ToElement (Number.Four), ff.ToElement (Apow3)), ff.Multiply (ff.ToElement (Number.TwentySeven), Bpow2));
			if (ret.IsZero ())
				return false;

			// Step4: Gy^2 = Gx^3 + a*Gx + b
			Number aGx = ff.Multiply (A, Gx);
			Number Xpow3 = ff.Multiply (Gx, ff.Multiply (Gx, Gx));
			Number Ypow2 = ff.Multiply (Gy, Gy);
			ret = ff.Add (Xpow3, ff.Add (aGx, B));
			if (ret.CompareTo (Ypow2) != 0)
				return false;

			// Step5: Check that n is prime.
			// Step6: Check that h <= 4, and that h = (sqrt(p)+1)^2 / n

			// Step7: Check that nG = O
			ECPoint nG = _G.Multiply (N).Export ();
			if (!nG.IsInifinity ())
				return false;

			// Step8: Check that q^B != 1 (mod n) for any 1 <= B <= 20, and that nh != p
			Number p = Number.One;
			Classical c = new Classical (N);
			for (int i = 0; i <= 20; i ++) {
				p = c.Multiply (p, P);
				if (p.IsOne ())
					return false;
			}
			if (c.Multiply (N, new Number (new uint[] {H}, 1)).CompareTo (P) == 0)
				return false;

			return true;
		}
		#endregion
	}
}
