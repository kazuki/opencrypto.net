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
using System;
using openCrypto.EllipticCurve;
using openCrypto.EllipticCurve.Encryption;
using openCrypto.FiniteField;

namespace openCrypto.Tests
{
	[TestFixture, Category ("ECC")]
	public class ECIESTest
	{
		[Test]
		public void Test_GEC2 ()
		{
			ECDomainNames domainName = ECDomainNames.secp160r1;
			ECDomainParameters domain = ECDomains.GetDomainParameter (domainName);
			ECIES ecies = new ECIES (domainName);
			Number V_Private = Number.Parse ("45FB58A92A17AD4B15101C66E74F277E2B460866", 16);
			ECKeyPair pair = new ECKeyPair (V_Private, null, domain);
			pair.CreatePublicKeyFromPrivateKey ();
			ecies.Parameters._Q = pair._Q;
			byte[] M = System.Text.Encoding.ASCII.GetBytes ("abcdefghijklmnopqrst");
			byte[] k = Number.Parse ("702232148019446860144825009548118511996283736794", 10).ToByteArray (20, false);
			byte[] C = ecies.Encrypt (M, k);
			byte[] expectedC = new byte[] {0x02, 0xCE, 0x28, 0x73, 0xE5, 0xBE, 0x44, 0x95, 0x63, 0x39, 0x1F, 0xEB, 0x47, 0xDD, 0xCB, 0xA2, 0xDC, 0x16, 0x37, 0x91, 0x91, 0x71, 0x23, 0xC8, 0x70, 0xA3, 0x1A, 0x81, 0xEA, 0x75, 0x83, 0x29, 0x0D, 0x1B, 0xA1, 0x7B, 0xC8, 0x75, 0x94, 0x35, 0xED, 0x1C, 0xCD, 0xA9, 0xEB, 0x4E, 0xD2, 0x73, 0x60, 0xBE, 0x89, 0x67, 0x29, 0xAD, 0x18, 0x54, 0x93, 0x62, 0x25, 0x91, 0xE5};
			Assert.AreEqual (expectedC, C, "Encryption");

			ecies = new ECIES (domainName);
			ecies.Parameters._d = V_Private;
			byte[] M2 = ecies.Decrypt (C);
			Assert.AreEqual (M, M2, "Decryption");
		}

		[Test]
		public void Test_Random ()
		{
			ECDomainNames domainName = ECDomainNames.secp160r1;
			for (int i = 0; i < 10; i ++) {
				ECIES ecies1 = new ECIES (domainName);
				ECIES ecies2 = new ECIES (domainName);
				byte[] plainText = RNG.GetRNGBytes (RNG.GetRNGBytes (1)[0] + RNG.GetRNGBytes (1)[0]);

				// ecies2 exports public key.
				byte[] publicKey = ecies2.Parameters.ExportPublicKey (true);

				// ecies1 imports public key.
				ecies1.Parameters.PublicKey = publicKey;

				// ecies1 encrypt plainText.
				byte[] cipherText = ecies1.Encrypt (plainText);

				// ecies2 decrypt cipherText.
				byte[] decrypted = ecies2.Decrypt (cipherText);

				// Check !
				Assert.AreEqual (plainText, decrypted);
			}
		}

		[Test]
		public void Test_Random_with_SharedInfo1 ()
		{
			ECDomainNames domainName = ECDomainNames.secp256r1;
			for (int i = 0; i < 5; i++) {
				ECIES ecies1 = new ECIES (domainName);
				ECIES ecies2 = new ECIES (domainName);
				byte[] sharedInfo = RNG.GetRNGBytes (RNG.GetRNGBytes (1)[0] + 1);
				byte[] plainText = RNG.GetRNGBytes (RNG.GetRNGBytes (1)[0] + RNG.GetRNGBytes (1)[0] + 1);

				// setup shared info 1
				ecies1.SharedInfo1 = sharedInfo;
				ecies2.SharedInfo1 = sharedInfo;

				// ecies2 exports public key.
				byte[] publicKey = ecies2.Parameters.ExportPublicKey (true);

				// ecies1 imports public key.
				ecies1.Parameters.PublicKey = publicKey;

				// ecies1 encrypt plainText.
				byte[] cipherText = ecies1.Encrypt (plainText);

				// ecies2 decrypt cipherText.
				byte[] decrypted = ecies2.Decrypt (cipherText);

				// Check !
				Assert.AreEqual (plainText, decrypted);
			}
		}

		[Test]
		public void Test_Random_with_SharedInfo2 ()
		{
			ECDomainNames domainName = ECDomainNames.secp192r1;
			for (int i = 0; i < 5; i++) {
				ECIES ecies1 = new ECIES (domainName);
				ECIES ecies2 = new ECIES (domainName);
				byte[] sharedInfo = RNG.GetRNGBytes (RNG.GetRNGBytes (1)[0] + 1);
				byte[] plainText = RNG.GetRNGBytes (RNG.GetRNGBytes (1)[0] + RNG.GetRNGBytes (1)[0] + 1);

				// setup shared info 1
				ecies1.SharedInfo2 = sharedInfo;
				ecies2.SharedInfo2 = sharedInfo;

				// ecies2 exports public key.
				byte[] publicKey = ecies2.Parameters.ExportPublicKey (true);

				// ecies1 imports public key.
				ecies1.Parameters.PublicKey = publicKey;

				// ecies1 encrypt plainText.
				byte[] cipherText = ecies1.Encrypt (plainText);

				// ecies2 decrypt cipherText.
				byte[] decrypted = ecies2.Decrypt (cipherText);

				// Check !
				Assert.AreEqual (plainText, decrypted);
			}
		}

		[Test]
		public void Test_Random_with_Both_SharedInfo ()
		{
			ECDomainNames domainName = ECDomainNames.secp128r1;
			for (int i = 0; i < 5; i++) {
				ECIES ecies1 = new ECIES (domainName);
				ECIES ecies2 = new ECIES (domainName);
				byte[] sharedInfo1 = RNG.GetRNGBytes (RNG.GetRNGBytes (1)[0] + 1);
				byte[] sharedInfo2 = RNG.GetRNGBytes (RNG.GetRNGBytes (1)[0] + 1);
				byte[] plainText = RNG.GetRNGBytes (RNG.GetRNGBytes (1)[0] + RNG.GetRNGBytes (1)[0] + 1);

				// setup shared info 1
				ecies1.SharedInfo1 = sharedInfo1;
				ecies2.SharedInfo1 = sharedInfo1;
				ecies1.SharedInfo2 = sharedInfo2;
				ecies2.SharedInfo2 = sharedInfo2;

				// ecies2 exports public key.
				byte[] publicKey = ecies2.Parameters.ExportPublicKey (true);

				// ecies1 imports public key.
				ecies1.Parameters.PublicKey = publicKey;

				// ecies1 encrypt plainText.
				byte[] cipherText = ecies1.Encrypt (plainText);

				// ecies2 decrypt cipherText.
				byte[] decrypted = ecies2.Decrypt (cipherText);

				// Check !
				Assert.AreEqual (plainText, decrypted);
			}
		}

		[Test]
		public void Test_Camellia ()
		{
			using (SymmetricAlgorithmPlus algo = new CamelliaManaged ()) {
				// Generate test data
				byte[] plain = RNG.GetRNGBytes (16 * 8);
				byte[] cipher, decrypted;
				ECIES ecies;

				// Test.1 128bit ECB Encryption with No-padding
				algo.KeySize = 128;
				algo.BlockSize = 128;
				algo.Mode = System.Security.Cryptography.CipherMode.ECB;
				algo.Padding = System.Security.Cryptography.PaddingMode.None;
				ecies = new ECIES (ECDomainNames.secp192r1, algo);
				cipher = ecies.Encrypt (plain);
				decrypted = ecies.Decrypt (cipher);
				Assert.AreEqual (plain, decrypted, "#1");

				// Test.2 128bit CBC Encryption with No-padding
				algo.Mode = System.Security.Cryptography.CipherMode.CBC;
				ecies = new ECIES (ECDomainNames.secp192r1, algo);
				cipher = ecies.Encrypt (plain);
				decrypted = ecies.Decrypt (cipher);
				Assert.AreEqual (plain, decrypted, "#2");

				// Test.3 128bit CBC Encryption with PKCS7 Padding
				algo.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
				ecies = new ECIES (ECDomainNames.secp192r1, algo);
				cipher = ecies.Encrypt (plain);
				decrypted = ecies.Decrypt (cipher);
				Assert.AreEqual (plain, decrypted, "#3");

				// Test.4 128bit CBC Encryption with PKCS7 Padding
				plain = RNG.GetRNGBytes (16 * 8 + 3);
				algo.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
				ecies = new ECIES (ECDomainNames.secp192r1, algo);
				cipher = ecies.Encrypt (plain);
				decrypted = ecies.Decrypt (cipher);
				Assert.AreEqual (plain, decrypted, "#4");

				// Test.5 128bit CBC Encryption with ANSIX923 Padding
				plain = RNG.GetRNGBytes (16 * 8 + 7);
				algo.Padding = System.Security.Cryptography.PaddingMode.ANSIX923;
				ecies = new ECIES (ECDomainNames.secp192r1, algo);
				cipher = ecies.Encrypt (plain);
				decrypted = ecies.Decrypt (cipher);
				Assert.AreEqual (plain, decrypted, "#5");

				// Test.6 128bit CBC Encryption with ISO10126 Padding
				plain = RNG.GetRNGBytes (16 * 8 + 9);
				algo.Padding = System.Security.Cryptography.PaddingMode.ISO10126;
				ecies = new ECIES (ECDomainNames.secp192r1, algo);
				cipher = ecies.Encrypt (plain);
				decrypted = ecies.Decrypt (cipher);
				Assert.AreEqual (plain, decrypted, "#6");

				// Test.7 128bit CBC Encryption with Zeros Padding
				plain = RNG.GetRNGBytes (16 * 8 + 11);
				algo.Padding = System.Security.Cryptography.PaddingMode.Zeros;
				ecies = new ECIES (ECDomainNames.secp192r1, algo);
				cipher = ecies.Encrypt (plain);
				decrypted = ecies.Decrypt (cipher);
				for (int i = 0; i < plain.Length; i ++)
					Assert.AreEqual (plain[i], decrypted[i], "#7.1");
				for (int i = plain.Length; i < decrypted.Length; i ++)
					Assert.AreEqual (0, decrypted[i], "#7.2");
			}
		}
	}
}