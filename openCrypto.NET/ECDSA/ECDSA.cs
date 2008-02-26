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
	public class ECDSA : System.Security.Cryptography.AsymmetricAlgorithm
	{
		ECDSAParameters _param;

		internal ECDSA (ECDSAParameters param)
		{
			_param = param;
		}

		internal ECDSA (ECDomainParameters domain)
		{
			_param = new ECDSAParameters (null, null, domain);
		}

		public ECDSA (ECDomainNames domain)
			: this (ECDomains.GetDomainParameter (domain))
		{
		}

		public ECDSA (Uri domain_oid)
			: this (ECDomains.GetDomainParameter (domain_oid))
		{
		}

		internal ECDSAParameters Parameters {
			get { return _param; }
		}

		#region Sign/Verify methods
		private Number[] Sign (Number e)
		{
			Number r, r2, s, k;
			IFiniteField field = _param.Domain.FieldN;
			do {
				do {
					// Step.1
					k = Number.CreateRandomElement (_param.Domain.N);

					// Step.2
					ECPoint tmp = _param.Domain.G.Multiply (k).Export ();

					// Step.3
					r = tmp.X;
					if (!r.IsZero ())
						break;
				} while (true);

				// Step.4
				k = field.Invert (field.ToElement (k));

				// Step.6
				r2 = field.ToElement (r);
				e = field.ToElement (e);
				s = field.Multiply (k, field.Add (e, field.Multiply (r2, _param.D)));
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
			IFiniteField field = _param.Domain.FieldN;

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
			//ECPoint X = _param.Domain.G.Multiply (u1).Add (_param.Q.Multiply (u2));
			ECPoint X;
			if (u1.IsZero ())
				X = _param.Domain.FieldN.GetInfinityPoint (_param.Domain.Group) .Add (_param.Q.Multiply (u2));
			else
				X = ECPoint.MultiplyAndAdd (_param.Domain.G, u1, _param.Q, u2);

			// Step.5
			if (X.IsInifinity ())
				return false;
			X = X.Export ();

			// Step.6
			return r.CompareTo (X.X) == 0;
		}

		public byte[] SignHash (byte[] hash)
		{
			if (hash == null)
				throw new ArgumentNullException ();
			if (hash.Length == 0)
				throw new ArgumentException ();
			if (_param.D == null && _param.Q == null)
				_param.CreateNewPrivateKey ();
			if (_param.D == null)
				throw new CryptographicException ();
			Number[] sig = Sign (new Number (hash));
			byte[] raw = new byte[(_param.Domain.Bits >> 2) + ((_param.Domain.Bits & 7) == 0 ? 0 : 2)];
			sig[0].CopyTo (raw, 0);
			sig[1].CopyTo (raw, raw.Length >> 1);
			return raw;
		}

		public bool VerifyHash (byte[] hash, byte[] sig)
		{
			if (sig.Length != (_param.Domain.Bits >> 2) + ((_param.Domain.Bits & 7) == 0 ? 0 : 2))
				throw new ArgumentException ();
			if (hash.Length == 0)
				throw new ArgumentException ();
			if (_param.Q == null && _param.D != null)
				_param.CreatePublicKeyFromPrivateKey ();
			if (_param.Q == null)
				throw new CryptographicException ();
			int halfLen = sig.Length >> 1;
			Number a = new Number (sig, 0, halfLen);
			Number b = new Number (sig, halfLen, halfLen);
			return Verify (new Number[] {a, b}, new Number (hash));
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
			ECPoint publicKey = _param.Q.Export ();

			using (StringWriter sw = new StringWriter ())
			using (XmlTextWriter writer = new XmlTextWriter (sw)) {
				writer.Formatting = Formatting.Indented;
				writer.Indentation = 2;
				writer.IndentChar = ' ';
				writer.WriteStartElement ("ECDSAKeyValue", "http://www.w3.org/2001/04/xmldsig-more#");
				writer.WriteStartElement ("DomainParameters");
				if (_param.Domain.URN != null) {
					writer.WriteStartElement ("NamedCurve");
					writer.WriteAttributeString ("URN", _param.Domain.URN.ToString ());
					writer.WriteEndElement ();
				} else {
					ECPoint basePoint = _param.Domain.G.Export ();
					writer.WriteStartElement ("ExplicitParams");
					writer.WriteElementString ("P", _param.Domain.P.ToString (10));
					writer.WriteStartElement ("CurveParams");
					writer.WriteStartElement ("A");
					writer.WriteAttributeString ("Value", _param.Domain.A.ToString (10));
					writer.WriteEndElement ();
					writer.WriteStartElement ("B");
					writer.WriteAttributeString ("Value", _param.Domain.B.ToString (10));
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
					writer.WriteElementString ("Order", _param.Domain.N.ToString (10));
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
				return (int)_param.Domain.Bits;
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
