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
	public class ECMQVTest
	{
		[Test]
		public void Test_Random ()
		{
			ECDomainNames name = ECDomainNames.secp256r1;
			for (int i = 0; i < 10; i ++) {
				ECMQV ecmqv1 = new ECMQV (name);
				ECMQV ecmqv2 = new ECMQV (name);
				int keyDataLen = 20;

				byte[] key1 = ecmqv1.PerformKeyAgreement (ecmqv2.Parameters.KeyPair1.PublicKey, ecmqv2.Parameters.KeyPair2.PublicKey, keyDataLen);
				byte[] key2 = ecmqv2.PerformKeyAgreement (ecmqv1.Parameters.KeyPair1.PublicKey, ecmqv1.Parameters.KeyPair2.PublicKey, keyDataLen);
				Assert.AreEqual (key1, key2);
			}
		}

		[Test]
		public void Test_Random_with_SharedInfo ()
		{
			ECDomainNames name = ECDomainNames.secp256r1;
			for (int i = 0; i < 10; i++) {
				ECMQV ecmqv1 = new ECMQV (name);
				ECMQV ecmqv2 = new ECMQV (name);
				int keyDataLen = 20;

				byte[] sharedInfo = RNG.GetBytes (RNG.GetBytes (1)[0] + 1);
				ecmqv1.SharedInfo = sharedInfo;
				ecmqv2.SharedInfo = sharedInfo;

				byte[] key1 = ecmqv1.PerformKeyAgreement (ecmqv2.Parameters.KeyPair1.PublicKey, ecmqv2.Parameters.KeyPair2.PublicKey, keyDataLen);
				byte[] key2 = ecmqv2.PerformKeyAgreement (ecmqv1.Parameters.KeyPair1.PublicKey, ecmqv1.Parameters.KeyPair2.PublicKey, keyDataLen);
				Assert.AreEqual (key1, key2);
			}
		}

		[Test]
		public void Test_MQV ()
		{
			ECDomainNames name = ECDomainNames.secp160r1;
			ECMQV ecmqv1 = new ECMQV (name);
			ECMQV ecmqv2 = new ECMQV (name);
			int keyDataLen = 20;
			int keyBytes = 20;

			ecmqv1.Parameters.KeyPair1.PrivateKey = Number.Parse ("971761939728640320549601132085879836204587084162", 10).ToByteArray (keyBytes, false);
			ecmqv1.Parameters.KeyPair2.PrivateKey = Number.Parse ("117720748206090884214100397070943062470184499100", 10).ToByteArray (keyBytes, false);
			ecmqv2.Parameters.KeyPair1.PrivateKey = Number.Parse ("399525573676508631577122671218044116107572676710", 10).ToByteArray (keyBytes, false);
			ecmqv2.Parameters.KeyPair2.PrivateKey = Number.Parse ("141325380784931851783969312377642205317371311134", 10).ToByteArray (keyBytes, false);

			byte[] key1 = ecmqv1.PerformKeyAgreement (ecmqv2.Parameters.KeyPair1.PublicKey, ecmqv2.Parameters.KeyPair2.PublicKey, keyDataLen);
			byte[] key2 = ecmqv2.PerformKeyAgreement (ecmqv1.Parameters.KeyPair1.PublicKey, ecmqv1.Parameters.KeyPair2.PublicKey, keyDataLen);

			Assert.AreEqual (key1, key2, "#1");
			Assert.AreEqual (key1, Number.Parse ("C06763F8C3D2452C1CC5D29BD61918FB485063F6", 16).ToByteArray (keyDataLen, false), "#2");
		}
	}
}