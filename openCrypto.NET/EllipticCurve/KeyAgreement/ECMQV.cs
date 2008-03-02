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
using System.Security.Cryptography;
using openCrypto.FiniteField;
using openCrypto.KeyDerivationFunctions;

namespace openCrypto.EllipticCurve.KeyAgreement
{
	public class ECMQV : ECKeyAgreement
	{
		ECMQVParameters _params;
		KeyDerivationFunction _kdf;

		#region Constructors
		internal ECMQV (ECDomainParameters domain)
		{
			_params = new ECMQVParameters (domain);
			_kdf = new ANSI_X963_KDF (new SHA1Managed ());
		}

		public ECMQV (ECDomainNames name)
			: this (ECDomains.GetDomainParameter (name))
		{
		}

		public ECMQV (Uri oid)
			: this (ECDomains.GetDomainParameter (oid))
		{
		}
		#endregion

		#region Properties
		public ECMQVParameters Parameters {
			get { return _params; }
		}
		#endregion

		#region MQV KeyAgreement
		public byte[] PerformKeyAgreement (byte[] otherPublicKey1, byte[] otherPublicKey2, int keyDataLength)
		{
			ECPoint otherQ1 = new ECPoint (_params.Domain.Group, otherPublicKey1);
			ECPoint otherQ2 = new ECPoint (_params.Domain.Group, otherPublicKey2);
			IFiniteField ff = _params.Domain.FieldN;

			// MQV Primitives
			if (_params.KeyPair1.D == null)
				_params.KeyPair1.CreateNewPrivateKey ();
			if (_params.KeyPair2.D == null)
				_params.KeyPair2.CreateNewPrivateKey ();
			if (_params.KeyPair2.Q == null)
				_params.KeyPair2.CreatePublicKeyFromPrivateKey ();
			int logBits = _params.Domain.N.BitCount ();
			logBits = (logBits >> 1) + ((logBits & 1) == 0 ? 0 : 1);
			Number mod = Number.One << logBits;
			Number mask = mod - Number.One;
			Number q2u = (_params.KeyPair2.Q.Export ().X & mask) + mod;
			Number s = ff.Add (_params.KeyPair2.D, ff.Multiply (q2u, _params.KeyPair1.D));
			Number q2v = (otherQ2.Export ().X & mask) + mod;
			ECPoint P = otherQ2.Add (otherQ1.Multiply (q2v)).Multiply (s * new Number (new uint[] {_params.Domain.H}));
			if (P.IsInifinity ())
				throw new CryptographicException ();
			int keyBytes = (int)((_params.Domain.Bits >> 3) + ((_params.Domain.Bits & 7) == 0 ? 0 : 1));
			byte[] sharedSecretValue = P.Export ().X.ToByteArray (keyBytes, false);

			// KDF
			_kdf.SharedInfo = _sharedInfo;
			return _kdf.Calculate (sharedSecretValue, keyDataLength);
		}
		#endregion

		#region Parameter import/export
		public override void FromXmlString (string xmlString)
		{
			throw new NotImplementedException ();
		}

		public override string ToXmlString (bool includePrivateParameters)
		{
			throw new NotImplementedException ();
		}
		#endregion

		#region Misc
		protected override void Dispose (bool disposing)
		{
		}

		public override string KeyExchangeAlgorithm {
			get { return "EVMQV"; }
		}

		public override string SignatureAlgorithm {
			get { return null; }
		}
		#endregion
	}
}
