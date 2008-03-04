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

#if TEST

using NUnit.Framework;
using openCrypto.EllipticCurve;
using openCrypto.EllipticCurve.Signature;

namespace openCrypto.Tests
{
	[TestFixture, Category ("ECC")]
	public class ECDSATest
	{
		[Test]
		public void PublicKeyTest ()
		{
			ECDSAManaged ecdsa = new ECDSAManaged (ECDomainNames.secp192r1);
			byte[] hash = RNG.GetRNGBytes (ecdsa.KeySize >> 3);
			byte[] sign = ecdsa.SignHash (hash);
			byte[] publicKey = ecdsa.Parameters.PublicKey;

			ecdsa = new ECDSAManaged (ECDomainNames.secp192r1);
			ecdsa.Parameters.PublicKey = publicKey;
			Assert.IsTrue (ecdsa.VerifyHash (hash, sign), "Success Test");

			sign[0] ++;
			Assert.IsFalse (ecdsa.VerifyHash (hash, sign), "Failure Test");
		}

		[Test]
		public void PrivateKeyTest ()
		{
			ECDSAManaged ecdsa = new ECDSAManaged (ECDomainNames.secp192r1);
			byte[] hash = RNG.GetRNGBytes (ecdsa.KeySize >> 3);
			byte[] sign = ecdsa.SignHash (hash);
			byte[] publicKey = ecdsa.Parameters.PublicKey;
			byte[] privateKey = ecdsa.Parameters.PrivateKey;

			ecdsa = new ECDSAManaged (ECDomainNames.secp192r1);
			ecdsa.Parameters.PublicKey = publicKey;
			Assert.IsTrue (ecdsa.VerifyHash (hash, sign), "Success Test #1");

			sign[0]++;
			Assert.IsFalse (ecdsa.VerifyHash (hash, sign), "Failure Test #1");

			ecdsa = new ECDSAManaged (ECDomainNames.secp192r1);
			ecdsa.Parameters.PrivateKey = privateKey;
			hash = RNG.GetRNGBytes (ecdsa.KeySize >> 3);
			sign = ecdsa.SignHash (hash);

			ecdsa = new ECDSAManaged (ECDomainNames.secp192r1);
			ecdsa.Parameters.PublicKey = publicKey;
			Assert.IsTrue (ecdsa.VerifyHash (hash, sign), "Success Test #2");

			sign[0]++;
			Assert.IsFalse (ecdsa.VerifyHash (hash, sign), "Failure Test #2");
		}

		[Test]
		public void Test_secp112r1 ()
		{
			SignVerifyTest (ECDomainNames.secp112r1);
		}

		[Test]
		public void Test_secp112r2 ()
		{
			SignVerifyTest (ECDomainNames.secp112r2);
		}

		[Test]
		public void Test_secp128r1 ()
		{
			SignVerifyTest (ECDomainNames.secp128r1);
		}

		[Test]
		public void Test_secp128r2 ()
		{
			SignVerifyTest (ECDomainNames.secp128r2);
		}

		[Test]
		public void Test_secp160r1 ()
		{
			SignVerifyTest (ECDomainNames.secp160r1);
		}

		[Test]
		public void Test_secp160r2 ()
		{
			SignVerifyTest (ECDomainNames.secp160r2);
		}

		[Test]
		public void Test_secp192r1 ()
		{
			SignVerifyTest (ECDomainNames.secp192r1);
		}

		[Test]
		public void Test_secp224r1 ()
		{
			SignVerifyTest (ECDomainNames.secp224r1);
		}

		[Test]
		public void Test_secp256r1 ()
		{
			SignVerifyTest (ECDomainNames.secp256r1);
		}

		[Test]
		public void Test_secp384r1 ()
		{
			SignVerifyTest (ECDomainNames.secp384r1);
		}

		[Test]
		public void Test_secp521r1 ()
		{
			SignVerifyTest (ECDomainNames.secp521r1);
		}

		static void SignVerifyTest (ECDomainNames domainName)
		{
			int repeat = 10;
			for (int i = 0; i < repeat; i ++) {
				ECDSAManaged ecdsa = new ECDSAManaged (domainName);
				byte[] pubKey = ecdsa.Parameters.PublicKey;
				byte[] hash = RNG.GetRNGBytes (ecdsa.KeySize >> 3);
				byte[] sign = ecdsa.SignHash (hash);
				ecdsa = new ECDSAManaged (domainName);
				ecdsa.Parameters.PublicKey = pubKey;
				Assert.IsTrue (ecdsa.VerifyHash (hash, sign), "Success Test " + domainName.ToString ());
				sign[0]++;
				Assert.IsFalse (ecdsa.VerifyHash (hash, sign), "Failure Test " + domainName.ToString ());
			}
		}
	}
}

#endif
