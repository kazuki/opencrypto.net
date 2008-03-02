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
	public class ECDiffieHellman : ECKeyAgreement
	{
		ECDiffieHellmanParameters _params;
		KeyDerivationFunction _kdf;

		#region Constructors
		internal ECDiffieHellman (ECDiffieHellmanParameters param)
		{
			_params = param;
			_kdf = new ANSI_X963_KDF (new SHA1Managed ());
		}

		public ECDiffieHellman (ECDomainNames name)
			: this (new ECDiffieHellmanParameters (null, null, ECDomains.GetDomainParameter (name)))
		{
		}

		public ECDiffieHellman (Uri oid)
			: this (new ECDiffieHellmanParameters (null, null, ECDomains.GetDomainParameter (oid)))
		{
		}
		#endregion

		#region DiffieHellman KeyAgreement
		public byte[] PerformKeyAgreement (byte[] otherPublicKey, int keyDataLength)
		{
			ECPoint other = new ECPoint (_params.Domain.Group, otherPublicKey);

			// Diffie-Hellman Primitives
			if (_params.D == null)
				_params.CreateNewPrivateKey ();
			Number sharedSecretField = other.Multiply (_params.D).Export ().X;
			byte[] sharedSecretValue = new byte[(_params.Domain.Bits >> 3) + ((_params.Domain.Bits & 7) == 0 ? 0 : 1)];
			sharedSecretField.CopyToBigEndian (sharedSecretValue, 0, sharedSecretValue.Length);

			// KDF
			_kdf.SharedInfo = _sharedInfo;
			return _kdf.Calculate (sharedSecretValue, keyDataLength);
		}
		#endregion

		#region Key Import/Export
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
		public ECDiffieHellmanParameters Parameters {
			get { return _params; }
		}

		protected override void Dispose (bool disposing)
		{
		}

		public override string KeyExchangeAlgorithm {
			get { return "ECDH"; }
		}

		public override string SignatureAlgorithm {
			get { return null; }
		}		
		#endregion
	}
}
