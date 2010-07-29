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

using NUnit.Framework;
using openCrypto.FiniteField;
using openCrypto.EllipticCurve;
using openCrypto.EllipticCurve.KeyAgreement;

namespace openCrypto.Tests
{
	[TestFixture, Category ("ECC")]
	public class ECDHTest
	{
		[Test]
		public void Test_Random ()
		{
			for (int i = 0; i < 10; i ++) {
				ECDiffieHellman ecdh1 = new ECDiffieHellman (ECDomainNames.secp256r1);
				ECDiffieHellman ecdh2 = new ECDiffieHellman (ECDomainNames.secp256r1);

				byte[] key1 = ecdh1.PerformKeyAgreement (ecdh2.Parameters.PublicKey, 20);
				byte[] key2 = ecdh2.PerformKeyAgreement (ecdh1.Parameters.PublicKey, 20);
				Assert.AreEqual (key1, key2, "#1");

				key1 = ecdh1.PerformKeyAgreement (ecdh2.Parameters.PublicKey, 128);
				key2 = ecdh2.PerformKeyAgreement (ecdh1.Parameters.PublicKey, 128);
				Assert.AreEqual (key1, key2, "#2");

				byte[] shareInfo = RNG.GetBytes (16);
				ecdh1.SharedInfo = shareInfo;
				ecdh2.SharedInfo = shareInfo;
				key1 = ecdh1.PerformKeyAgreement (ecdh2.Parameters.PublicKey, 256);
				key2 = ecdh2.PerformKeyAgreement (ecdh1.Parameters.PublicKey, 256);
				Assert.AreEqual (key1, key2, "#3");

				key1 = ecdh1.PerformKeyAgreement (ecdh2.Parameters, 128);
				key2 = ecdh2.PerformKeyAgreement (ecdh1.Parameters, 128);
				Assert.AreEqual (key1, key2, "#4");
			}
		}

		[Test]
		public void Test_Random_with_SharedInfo ()
		{
			for (int i = 0; i < 10; i++) {
				ECDiffieHellman ecdh1 = new ECDiffieHellman (ECDomainNames.secp256r1);
				ECDiffieHellman ecdh2 = new ECDiffieHellman (ECDomainNames.secp256r1);
				byte[] sharedInfo = RNG.GetBytes (RNG.GetBytes (1)[0] + 1);

				ecdh1.SharedInfo = sharedInfo;
				ecdh2.SharedInfo = sharedInfo;

				byte[] key1 = ecdh1.PerformKeyAgreement (ecdh2.Parameters.PublicKey, 20);
				byte[] key2 = ecdh2.PerformKeyAgreement (ecdh1.Parameters.PublicKey, 20);
				Assert.AreEqual (key1, key2, "#1");

				key1 = ecdh1.PerformKeyAgreement (ecdh2.Parameters.PublicKey, 128);
				key2 = ecdh2.PerformKeyAgreement (ecdh1.Parameters.PublicKey, 128);
				Assert.AreEqual (key1, key2, "#2");

				byte[] shareInfo = RNG.GetBytes (16);
				ecdh1.SharedInfo = shareInfo;
				ecdh2.SharedInfo = shareInfo;
				key1 = ecdh1.PerformKeyAgreement (ecdh2.Parameters.PublicKey, 256);
				key2 = ecdh2.PerformKeyAgreement (ecdh1.Parameters.PublicKey, 256);
				Assert.AreEqual (key1, key2, "#3");
			}
		}

		[Test]
		public void Test_GEC2 ()
		{
			ECDiffieHellman U = new ECDiffieHellman (ECDomainNames.secp160r1);
			ECDiffieHellman V = new ECDiffieHellman (ECDomainNames.secp160r1);
			int byteLen = (int)((U.Parameters.Domain.Bits >> 3) + ((U.Parameters.Domain.Bits & 7) == 0 ? 0 : 1));
			U.Parameters.PrivateKey = Number.Parse ("971761939728640320549601132085879836204587084162", 10).ToByteArray (byteLen, false);
			V.Parameters.PrivateKey = Number.Parse ("399525573676508631577122671218044116107572676710", 10).ToByteArray (byteLen, false);

			int keyDataLen = 20;
			byte[] kU = U.PerformKeyAgreement (V.Parameters.PublicKey, keyDataLen);
			byte[] kV = V.PerformKeyAgreement (U.Parameters.PublicKey, keyDataLen);
			Assert.AreEqual (kU, kV, "#1");

			byte[] expected = Number.Parse ("744AB703F5BC082E59185F6D049D2D367DB245C2", 16).ToByteArray (20, false);
			Assert.AreEqual (kU, expected, "#2");
		}
	}
}