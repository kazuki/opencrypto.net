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
	class ECDomainParameters
	{
		ECGroup _group;
		ECPoint _G;
		Number _order;
		IFiniteField _field;
		uint _h;
		uint _bits;

		public ECDomainParameters (ECGroup group, ECPoint G, Number order, uint h, uint bits, IFiniteField fieldN)
		{
			_group = group;
			_G = G;
			_order = order;
			_h = h;
			_bits = bits;
			_field = fieldN;
		}

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
		public Number Field {
			get { return _group.Q; }
		}

		public IFiniteField FieldN {
			get { return _field; }
		}

		/// <summary>The order of base point.</summary>
		public Number N {
			get { return _order; }
		}

		/// <summary>The co-factor</summary>
		public uint H {
			get { return _h; }
		}
	}
}
