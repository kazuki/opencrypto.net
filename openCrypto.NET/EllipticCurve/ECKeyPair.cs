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
		internal ECDomainNames _domainName = ECDomainNames.none;

		/// <param name="d">Private Key</param>
		/// <param name="Q">Public Key</param>
		internal ECKeyPair (Number d, ECPoint Q, ECDomainParameters domain)
			: this (d, Q, domain, ECDomainNames.none)
		{
			_domainName = ECDomains.GetDomainName (domain);
		}

		internal ECKeyPair (Number d, ECPoint Q, ECDomainParameters domain, ECDomainNames domainName)
		{
			_d = d;
			_Q = Q;
			_domain = domain;
			_domainName = domainName;
		}

		public static ECKeyPair Create (ECDomainNames domain)
		{
			return new ECKeyPair (null, null, ECDomains.GetDomainParameter (domain), domain);
		}

		public static ECKeyPair Create (ECDomainNames domain, byte[] privateKey, byte[] publicKey)
		{
			ECKeyPair pair = Create (domain);
			pair.PrivateKey = privateKey;
			pair._Q = new ECPoint (pair.Domain.Group, publicKey);
			return pair;
		}

		public static ECKeyPair CreatePrivate (ECDomainNames domain, byte[] privateKey)
		{
			ECKeyPair pair = Create (domain);
			pair.PrivateKey = privateKey;
			return pair;
		}

		public static ECKeyPair CreatePublic (ECDomainNames domain, byte[] publicKey)
		{
			ECKeyPair pair = Create (domain);
			pair.PublicKey = publicKey;
			return pair;
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

		public ECDomainNames DomainName {
			get { return _domainName; }
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

		public bool ValidatePublicKey ()
		{
			if (_d == null || _Q == null)
				return true;
			ECPoint point1 = _Q.Export ();
			ECPoint point2 = _domain.G.Multiply (_d).Export ();
			return point1.X.CompareTo (point2.X) == 0 &&
				point1.Y.CompareTo (point2.Y) == 0 &&
				point1.Z.CompareTo (point2.Z) == 0;
		}
	}
}
