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
using System.IO;
using System.Xml;
using openCrypto.FiniteField;
using openCrypto.EllipticCurve;
using CryptographicException = System.Security.Cryptography.CryptographicException;

namespace openCrypto.ECDSA
{
	public class ECDSAManaged : System.Security.Cryptography.AsymmetricAlgorithm
	{
		/// <summary>Private key</summary>
		Number _d;

		/// <summary>Public key</summary>
		ECPoint _Q;

		/// <summary>Elliptic Curve Domain Parameters</summary>
		ECDomainParameters _domain;
		int _orderBits;

		internal ECDSAManaged (ECDomainParameters domain)
		{
			_domain = domain;
			_orderBits = _domain.N.BitCount ();
		}

		public ECDSAManaged (ECDomainNames domain)
			: this (ECDomains.GetDomainParameter (domain))
		{
		}

		public ECDSAManaged (Uri domain_oid)
			: this (ECDomains.GetDomainParameter (domain_oid))
		{
		}

		#region Sign/Verify methods
		private Number[] Sign (Number e)
		{
			Number r, r2, s, k;
			IFiniteField field = _domain.FieldN;
			do {
				do {
					// Step.1
					k = Number.CreateRandomElement (_domain.N);

					// Step.2
					ECPoint tmp = _domain.G.Multiply (k).Export ();

					// Step.3
					r = tmp.X % _domain.N;
					if (!r.IsZero ())
						break;
				} while (true);

				// Step.4
				k = field.Invert (field.ToElement (k));

				// Step.6
				r2 = field.ToElement (r);
				e = field.ToElement (e);
				s = field.Multiply (k, field.Add (e, field.Multiply (r2, field.ToElement (_d))));
				if (!s.IsZero ()) {
					s = field.ToNormal (s);
					break;
				}
			} while (true);
			return new Number[] { r, s };
		}

		private bool Verify (Number[] sign, Number e)
		{
			Number r = sign[0], s = sign[1];
			IFiniteField field = _domain.FieldN;

			if (r >= _domain.N || s >= _domain.N)
				return false;

			// Step.1
			e = field.ToElement (e);
			s = field.ToElement (s);
			Number r2 = field.ToElement (r);

			// Step.2
			Number w = field.Invert (s);

			// Step.3
			Number u1 = field.ToNormal (field.Multiply (e, w));
			Number u2 = field.ToNormal (field.Multiply (r2, w));

			// Step.4
			//ECPoint X = _domain.G.Multiply (u1).Add (_Q.Multiply (u2));
			ECPoint X;
			if (u1.IsZero ())
				X = _domain.FieldN.GetInfinityPoint (_domain.Group) .Add (_Q.Multiply (u2));
			else
				X = ECPoint.MultiplyAndAdd (_domain.G, u1, _Q, u2);

			// Step.5
			if (X.IsInifinity ())
				return false;
			X = X.Export ();

			// Step.6
			Number v = X.X % _domain.N;
			return r.CompareTo (v) == 0;
		}

		public byte[] SignHash (byte[] hash)
		{
			if (hash == null)
				throw new ArgumentNullException ();
			if (hash.Length == 0)
				throw new ArgumentException ();
			if (_d == null && _Q == null)
				CreateNewPrivateKey ();
			if (_d == null)
				throw new CryptographicException ();
			Number[] sig = Sign (HashToNumber (hash));
			byte[] raw = new byte[(_domain.Bits >> 2) + ((_domain.Bits & 7) == 0 ? 0 : 2)];
			sig[0].CopyTo (raw, 0);
			sig[1].CopyTo (raw, raw.Length >> 1);
			return raw;
		}

		public bool VerifyHash (byte[] hash, byte[] sig)
		{
			if (sig.Length != (_domain.Bits >> 2) + ((_domain.Bits & 7) == 0 ? 0 : 2))
				throw new ArgumentException ();
			if (hash.Length == 0)
				throw new ArgumentException ();
			if (_Q == null && _d != null)
				CreatePublicKeyFromPrivateKey ();
			if (_Q == null)
				throw new CryptographicException ();
			int halfLen = sig.Length >> 1;
			Number a = new Number (sig, 0, halfLen);
			Number b = new Number (sig, halfLen, halfLen);
			return Verify (new Number[] {a, b}, HashToNumber (hash));
		}

		Number HashToNumber (byte[] hash)
		{
			int len = Math.Min (hash.Length, _orderBits >> 3);
			return new Number (hash, 0, len);
		}
		#endregion

		#region Serialize methods
		public override void FromXmlString (string xmlString)
		{
			throw new Exception ("The method or operation is not implemented.");
		}

		public override string ToXmlString (bool includePrivateParameters)
		{
			if (includePrivateParameters)
				throw new NotSupportedException ();
			if (_Q == null) {
				if (_d == null)
					CreateNewPrivateKey ();
				CreatePublicKeyFromPrivateKey ();
			}
			ECPoint publicKey = _Q.Export ();

			using (StringWriter sw = new StringWriter ())
			using (XmlTextWriter writer = new XmlTextWriter (sw)) {
				writer.Formatting = Formatting.Indented;
				writer.Indentation = 2;
				writer.IndentChar = ' ';
				writer.WriteStartElement ("ECDSAKeyValue", "http://www.w3.org/2001/04/xmldsig-more#");
				writer.WriteStartElement ("DomainParameters");
				if (_domain.URN != null) {
					writer.WriteStartElement ("NamedCurve");
					writer.WriteAttributeString ("URN", _domain.URN.ToString ());
					writer.WriteEndElement ();
				} else {
					ECPoint basePoint = _domain.G.Export ();
					writer.WriteStartElement ("ExplicitParams");
					writer.WriteElementString ("P", _domain.P.ToString (10));
					writer.WriteStartElement ("CurveParams");
					writer.WriteStartElement ("A");
					writer.WriteAttributeString ("Value", _domain.A.ToString (10));
					writer.WriteEndElement ();
					writer.WriteStartElement ("B");
					writer.WriteAttributeString ("Value", _domain.B.ToString (10));
					writer.WriteEndElement ();
					writer.WriteEndElement ();
					writer.WriteStartElement ("BasePointParams");
					writer.WriteStartElement ("BasePoint");
					writer.WriteStartElement ("X");
					writer.WriteAttributeString ("Value", basePoint.X.ToString (10));
					writer.WriteEndElement ();
					writer.WriteStartElement ("Y");
					writer.WriteAttributeString ("Value", basePoint.Y.ToString (10));
					writer.WriteEndElement ();
					writer.WriteEndElement ();
					writer.WriteElementString ("Order", _domain.N.ToString (10));
					writer.WriteEndElement ();
					writer.WriteEndElement ();
				}
				writer.WriteEndElement ();
				writer.WriteStartElement ("PublicKey");
				writer.WriteStartElement ("X");
				writer.WriteAttributeString ("Value", publicKey.X.ToString (10));
				writer.WriteEndElement ();
				writer.WriteStartElement ("Y");
				writer.WriteAttributeString ("Value", publicKey.Y.ToString (10));
				writer.WriteEndElement ();
				writer.WriteEndElement ();
				writer.WriteEndElement ();
				return sw.ToString ();
			}
		}
		#endregion

		#region Misc
		void CreateNewPrivateKey ()
		{
			_d = Number.CreateRandomElement (_domain.N);
		}

		void CreatePublicKeyFromPrivateKey ()
		{
			_Q = _domain.G.Multiply (_d);
		}

		protected override void Dispose (bool disposing)
		{
		}

		public override string KeyExchangeAlgorithm {
			get { return null; }
		}

		public override string SignatureAlgorithm {
			get { return "ECDSA"; }
		}

		public override int KeySize {
			get {
				return (int)_domain.Bits;
			}
			set {
				throw new NotSupportedException ();
			}
		}

		public override System.Security.Cryptography.KeySizes[] LegalKeySizes {
			get {
				throw new NotSupportedException ();
			}
		}
		#endregion
	}
}
