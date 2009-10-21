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
			ECDSA ecdsa = new ECDSA (ECDomainNames.secp192r1);
			byte[] hash = RNG.GetBytes (ecdsa.KeySize >> 3);
			byte[] sign = ecdsa.SignHash (hash);
			byte[] publicKey = ecdsa.Parameters.PublicKey;

			ecdsa = new ECDSA (ECDomainNames.secp192r1);
			ecdsa.Parameters.PublicKey = publicKey;
			Assert.IsTrue (ecdsa.VerifyHash (hash, sign), "Success Test");

			sign[0] ++;
			Assert.IsFalse (ecdsa.VerifyHash (hash, sign), "Failure Test");
		}

		[Test]
		public void PrivateKeyTest ()
		{
			ECDSA ecdsa = new ECDSA (ECDomainNames.secp192r1);
			byte[] hash = RNG.GetBytes (ecdsa.KeySize >> 3);
			byte[] sign = ecdsa.SignHash (hash);
			byte[] publicKey = ecdsa.Parameters.PublicKey;
			byte[] privateKey = ecdsa.Parameters.PrivateKey;

			ecdsa = new ECDSA (ECDomainNames.secp192r1);
			ecdsa.Parameters.PublicKey = publicKey;
			Assert.IsTrue (ecdsa.VerifyHash (hash, sign), "Success Test #1");

			sign[0]++;
			Assert.IsFalse (ecdsa.VerifyHash (hash, sign), "Failure Test #1");

			ecdsa = new ECDSA (ECDomainNames.secp192r1);
			ecdsa.Parameters.PrivateKey = privateKey;
			hash = RNG.GetBytes (ecdsa.KeySize >> 3);
			sign = ecdsa.SignHash (hash);

			ecdsa = new ECDSA (ECDomainNames.secp192r1);
			ecdsa.Parameters.PublicKey = publicKey;
			Assert.IsTrue (ecdsa.VerifyHash (hash, sign), "Success Test #2");

			sign[0]++;
			Assert.IsFalse (ecdsa.VerifyHash (hash, sign), "Failure Test #2");
		}

		[Test]
		public void Test_GEC2 ()
		{
			ECDSA ecdsa1 = new ECDSA (ECDomainNames.secp160r1);
			ECDSA ecdsa2 = new ECDSA (ECDomainNames.secp160r1);

			ecdsa1.Parameters.PrivateKey = new byte[] {0xAA, 0x37, 0x4F, 0xFC, 0x3C, 0xE1, 0x44, 0xE6, 0xB0, 0x73, 0x30, 0x79, 0x72, 0xCB, 0x6D, 0x57, 0xB2, 0xA4, 0xE9, 0x82};
			ecdsa2.Parameters.PublicKey = ecdsa1.Parameters.PublicKey;
			byte[] k = openCrypto.FiniteField.Number.Parse ("702232148019446860144825009548118511996283736794", 10).ToByteArray (20, false);
			byte[] H = new byte[] {0xA9, 0x99, 0x3E, 0x36, 0x47, 0x06, 0x81, 0x6A, 0xBA, 0x3E, 0x25, 0x71, 0x78, 0x50, 0xC2, 0x6C, 0x9C, 0xD0, 0xD8, 0x9D};
			byte[] expectedSign = new byte[] {
				0xCE, 0x28, 0x73, 0xE5, 0xBE, 0x44, 0x95, 0x63, 0x39, 0x1F, 0xEB, 0x47, 0xDD, 0xCB, 0xA2, 0xDC, 0x16, 0x37, 0x91, 0x91,
				0x34, 0x80, 0xEC, 0x13, 0x71, 0xA0, 0x91, 0xA4, 0x64, 0xB3, 0x1C, 0xE4, 0x7D, 0xF0, 0xCB, 0x8A, 0xA2, 0xD9, 0x8B, 0x54,
			};

			byte[] sign = ecdsa1.SignHash (H, k);
			Assert.AreEqual (expectedSign, sign);

			Assert.IsTrue (ecdsa2.VerifyHash (H, sign));
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
			int repeat = 5;
			for (int i = 0; i < repeat; i ++) {
				ECDSA ecdsa = new ECDSA (domainName);
				byte[] pubKey = ecdsa.Parameters.PublicKey;
				byte[] hash = RNG.GetBytes (ecdsa.KeySize >> 3);
				byte[] sign = ecdsa.SignHash (hash);
				ecdsa = new ECDSA (domainName);
				ecdsa.Parameters.PublicKey = pubKey;
				Assert.IsTrue (ecdsa.VerifyHash (hash, sign), "Success Test " + domainName.ToString ());
				sign[0]++;
				Assert.IsFalse (ecdsa.VerifyHash (hash, sign), "Failure Test " + domainName.ToString ());
			}
		}
	}
}
