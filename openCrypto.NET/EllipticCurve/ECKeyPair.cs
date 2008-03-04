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
	public class ECKeyPair
	{
		internal Number _d;
		internal ECPoint _Q;
		internal ECDomainParameters _domain;

		/// <param name="d">Private Key</param>
		/// <param name="Q">Public Key</param>
		internal ECKeyPair (Number d, ECPoint Q, ECDomainParameters domain)
		{
			_d = d;
			_Q = Q;
			_domain = domain;
		}

		internal void CreateNewPrivateKey ()
		{
			_d = Number.CreateRandomElement (_domain.N);
		}

		internal void CreatePublicKeyFromPrivateKey ()
		{
			_Q = _domain.G.Multiply (_d);
		}

		internal Number D {
			get { return _d; }
		}

		internal ECPoint Q {
			get { return _Q; }
		}

		internal ECDomainParameters Domain {
			get { return _domain; }
		}

		public byte[] PrivateKey {
			get {
				if (_d == null)
					CreateNewPrivateKey ();
				byte[] tmp = new byte[(_domain.Bits >> 3) + ((_domain.Bits & 7) == 0 ? 0 : 1)];
				_d.CopyToBigEndian (tmp, 0, tmp.Length);
				return tmp;
			}
			set {
				if (value == null) {
					_d = null;
					_Q = null;
				} else {
					Number tmp = new Number (value, false);
					if (tmp >= _domain.N)
						throw new ArgumentException ();
					_Q = null;
					_d = tmp;
				}
			}
		}

		public byte[] PublicKey {
			get {
				return ExportPublicKey (false);
			}
			set {
				_Q = new ECPoint (_domain.Group, value);
				_d = null;
			}
		}

		public byte[] ExportPublicKey (bool usePointCompression)
		{
			if (_Q == null) {
				if (_d == null)
					CreateNewPrivateKey ();
				CreatePublicKeyFromPrivateKey ();
			}
			return _Q.ToByteArray (usePointCompression);
		}
	}
}
