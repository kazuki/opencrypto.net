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
using CryptographicException = System.Security.Cryptography.CryptographicException;

namespace openCrypto.EllipticCurve.Signature
{
	public class ECDSA : System.Security.Cryptography.AsymmetricAlgorithm
	{
		ECDSAParameters _params;
		int _orderBits;

		internal ECDSA (ECDomainParameters domain)
		{
			_params = new ECDSAParameters (null, null, domain);
			_orderBits = _params.Domain.N.BitCount ();
		}

		public ECDSA (ECDomainNames domain)
			: this (ECDomains.GetDomainParameter (domain))
		{
		}

		public ECDSA (Uri domain_oid)
			: this (ECDomains.GetDomainParameter (domain_oid))
		{
		}

		#region Sign/Verify methods
		//private Number[] Sign (Number e)
		public byte[] SignHash (byte[] hash)
		{
			if (hash == null)
				throw new ArgumentNullException ();
			if (hash.Length == 0)
				throw new ArgumentException ();
			if (_params.D == null && _params.Q == null)
				_params.CreateNewPrivateKey ();
			if (_params.D == null)
				throw new CryptographicException ();

			Number r, s, k;
			IFiniteField field = _params.Domain.FieldN;
			int keyBytes = (int)((_params.Domain.Bits >> 3) + ((_params.Domain.Bits & 7) == 0 ? 0U : 1U));
			byte[] raw = new byte[keyBytes << 1];
			Number e = HashToNumber (hash);

			do {
				do {
					// Step.1
					k = Number.CreateRandomElement (_params.Domain.N);

					// Step.2
					ECPoint tmp = _params.Domain.G.Multiply (k).Export ();

					// Step.3
					r = tmp.X % _params.Domain.N;
					if (!r.IsZero ()) {
						r.CopyToBigEndian (raw, 0, keyBytes);
						break;
					}
				} while (true);

				// Step.4
				k = field.Invert (field.ToElement (k));

				// Step.6
				r = field.ToElement (r);
				e = field.ToElement (e);
				s = field.Multiply (k, field.Add (e, field.Multiply (r, field.ToElement (_params.D))));
				if (!s.IsZero ()) {
					s = field.ToNormal (s);
					s.CopyToBigEndian (raw, raw.Length >> 1, keyBytes);
					break;
				}
			} while (true);

			return raw;
		}

		public bool VerifyHash (byte[] hash, byte[] sig)
		{
			if (sig.Length != (_params.Domain.Bits >> 2) + ((_params.Domain.Bits & 7) == 0 ? 0 : 2))
				throw new ArgumentException ();
			if (hash.Length == 0)
				throw new ArgumentException ();
			if (_params.Q == null && _params.D != null)
				_params.CreatePublicKeyFromPrivateKey ();
			if (_params.Q == null)
				throw new CryptographicException ();

			int halfLen = sig.Length >> 1;
			Number r = new Number (sig, 0, halfLen, false);
			Number s = new Number (sig, halfLen, halfLen, false);
			Number e = HashToNumber (hash);
			IFiniteField field = _params.Domain.FieldN;

			if (r >= _params.Domain.N || s >= _params.Domain.N)
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
			//ECPoint X = _params.Domain.G.Multiply (u1).Add (_params.Q.Multiply (u2));
			ECPoint X;
			if (u1.IsZero ())
				X = _params.Domain.FieldN.GetInfinityPoint (_params.Domain.Group) .Add (_params.Q.Multiply (u2));
			else
				X = ECPoint.MultiplyAndAdd (_params.Domain.G, u1, _params.Q, u2);

			// Step.5
			if (X.IsInifinity ())
				return false;
			X = X.Export ();

			// Step.6
			Number v = X.X % _params.Domain.N;
			return r.CompareTo (v) == 0;
		}

		Number HashToNumber (byte[] hash)
		{
			int len = Math.Min (hash.Length, _orderBits >> 3);
			return new Number (hash, 0, len, false);
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
			if (_params.Q == null) {
				if (_params.D == null)
					_params.CreateNewPrivateKey ();
				_params.CreatePublicKeyFromPrivateKey ();
			}
			ECPoint publicKey = _params.Q.Export ();

			using (StringWriter sw = new StringWriter ())
			using (XmlTextWriter writer = new XmlTextWriter (sw)) {
				writer.Formatting = Formatting.Indented;
				writer.Indentation = 2;
				writer.IndentChar = ' ';
				writer.WriteStartElement ("ECDSAKeyValue", "http://www.w3.org/2001/04/xmldsig-more#");
				writer.WriteStartElement ("DomainParameters");
				if (_params.Domain.URN != null) {
					writer.WriteStartElement ("NamedCurve");
					writer.WriteAttributeString ("URN", _params.Domain.URN.ToString ());
					writer.WriteEndElement ();
				} else {
					ECPoint basePoint = _params.Domain.G.Export ();
					writer.WriteStartElement ("ExplicitParams");
					writer.WriteElementString ("P", _params.Domain.P.ToString (10));
					writer.WriteStartElement ("CurveParams");
					writer.WriteStartElement ("A");
					writer.WriteAttributeString ("Value", _params.Domain.A.ToString (10));
					writer.WriteEndElement ();
					writer.WriteStartElement ("B");
					writer.WriteAttributeString ("Value", _params.Domain.B.ToString (10));
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
					writer.WriteElementString ("Order", _params.Domain.N.ToString (10));
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

		#region Parameter
		public ECDSAParameters Parameters {
			get { return _params; }
		}
		#endregion

		#region Misc
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
				return (int)_params.Domain.Bits;
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
