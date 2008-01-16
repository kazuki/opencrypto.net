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
using openCrypto.EllipticCurve;

namespace openCrypto.ECDSA
{
	public class ECDSAParameters
	{
		Number _d;
		ECPoint _Q;
		ECDomainParameters _domain;

		/// <param name="d">Private Key</param>
		/// <param name="Q">Public Key</param>
		public ECDSAParameters (Number d, ECPoint Q, ECDomainParameters domain)
		{
			_d = d;
			_Q = Q;
			_domain = domain;
		}

		public static ECDSAParameters CreateNew (ECDomainParameters domain)
		{
			return CreateNew (domain, Number.CreateRandomElement (domain.N));
		}

		public static ECDSAParameters CreateNew (ECDomainParameters domain, Number d)
		{
			return new ECDSAParameters (d, domain.G.Multiply (d), domain);
		}

		public Number D {
			get { return _d; }
		}

		public ECPoint Q {
			get { return _Q; }
		}

		public ECDomainParameters Domain {
			get { return _domain; }
		}
	}
}
