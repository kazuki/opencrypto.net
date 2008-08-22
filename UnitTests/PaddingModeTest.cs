// 
// Copyright (c) 2006, Kazuki Oikawa
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
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture, Category ("SymmetricCryptography")]
	public class PaddingModeTest
	{
		/// <summary>
		/// <ja>ANSIX923パディングモードの動作テスト</ja>
		/// </summary>
		[Test]
		public void TestANSIX923 ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.Mode = CipherMode.ECB;
			algo.Padding = PaddingMode.ANSIX923;
			PaddingTest (algo, "ANSIX923 Tests #1",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 0, 0, 0, 0, 0, 0, 0, 8});
			PaddingTest (algo, "ANSIX923 Tests #2",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 0, 0, 0, 0, 0, 7});
			PaddingTest (algo, "ANSIX923 Tests #3",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1});
			PaddingTest (algo, "ANSIX923 Tests #4",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0, 0, 0, 0, 0, 8});
			PaddingTest (algo, "ANSIX923 Tests #5",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 0, 0, 0, 0, 0, 0, 7});
		}

		/// <summary>
		/// <ja>Noneパディングモードの動作テスト</ja>
		/// </summary>
		[Test]
		public void TestNone ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.Mode = CipherMode.ECB;
			algo.Padding = PaddingMode.None;
			PaddingTest (algo, "None Tests #1",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8});
			try {
				PaddingTest (algo, String.Empty,
								 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9}, new byte[0]);
				Assert.Fail ("None Tests #2");
			} catch (CryptographicException) {}

			try {
				PaddingTest (algo, String.Empty,
								 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9}, new byte[0]);
				Assert.Fail ("None Tests #2");
			} catch (CryptographicException) {}

			try {
				PaddingTest (algo, String.Empty,
								 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}, new byte[0]);
				Assert.Fail ("None Tests #3");
			} catch (CryptographicException) {}

			PaddingTest (algo, "None Tests #4",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16});

			try {
				PaddingTest (algo, String.Empty,
								 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17}, new byte[0]);
				Assert.Fail ("None Tests #4");
			} catch (CryptographicException) {}
		}

		/// <summary>
		/// <ja>PKCS7パディングモードの動作テスト</ja>
		/// </summary>
		[Test]
		public void TestPKCS7 ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.Mode = CipherMode.ECB;
			algo.Padding = PaddingMode.PKCS7;
			PaddingTest (algo, "PKCS7 Tests #1",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 8, 8, 8, 8, 8, 8, 8, 8});
			PaddingTest (algo, "PKCS7 Tests #2",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 7, 7, 7, 7, 7, 7, 7});
			PaddingTest (algo, "PKCS7 Tests #3",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1});
			PaddingTest (algo, "PKCS7 Tests #4",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 8, 8, 8, 8, 8, 8, 8, 8});
			PaddingTest (algo, "PKCS7 Tests #5",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 7, 7, 7, 7, 7, 7, 7});
		}

		/// <summary>
		/// <ja>ISO10126パディングモードの動作テスト</ja>
		/// </summary>
		[Test]
		public void TestISO10126 ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.Mode = CipherMode.ECB;
			algo.Padding = PaddingMode.ISO10126;
			PaddingTest (algo, "ISO10126 Tests #1",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 0, 0, 0, 0, 0, 0, 0, 8});
			PaddingTest (algo, "ISO10126 Tests #2",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 0, 0, 0, 0, 0, 7});
			PaddingTest (algo, "ISO10126 Tests #3",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 1});
			PaddingTest (algo, "ISO10126 Tests #4",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 0, 0, 0, 0, 0, 0, 0, 8});
			PaddingTest (algo, "ISO10126 Tests #5",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 0, 0, 0, 0, 0, 0, 7});
		}

		/// <summary>
		/// <ja>Zerosパディングモードの動作テスト</ja>
		/// </summary>
		[Test]
		public void TestZeros ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.Mode = CipherMode.ECB;
			algo.Padding = PaddingMode.Zeros;
			PaddingTest (algo, "Zeros Tests #1",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8});
			PaddingTest (algo, "Zeros Tests #2",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 0, 0, 0, 0, 0, 0});
			PaddingTest (algo, "Zeros Tests #3",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0});
			PaddingTest (algo, "Zeros Tests #4",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16});
			PaddingTest (algo, "Zeros Tests #5",
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17},
							 new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 0, 0, 0, 0, 0, 0, 0});
		}

		/// <summary>
		/// <ja>パディングモードのテストを行うメソッド</ja>
		/// </summary>
		private static void PaddingTest (SymmetricAlgorithm algo, String msg, byte[] src, byte[] expected) {
			byte[] output;
			byte[] key = new byte[algo.KeySize];
			byte[] iv = new byte[algo.BlockSize];
			using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
				output = ct.TransformFinalBlock (src, 0, src.Length);
				Assert.AreEqual (expected.Length, output.Length, "Output length error in " + msg);
				if (algo.Padding != PaddingMode.ISO10126) {
					for (int i = 0; i < expected.Length; i ++)
						Assert.AreEqual (expected[i], output[i], "Encrypt error in " + msg);
				} else {
					Assert.AreEqual (expected[expected.Length - 1], output[expected.Length - 1], "Encrypt error in " + msg);
				}
			}
			Assert.AreEqual (expected[output.Length - 1], output[output.Length - 1], "Encrypt error in " + msg);
			using (ICryptoTransform ct = algo.CreateDecryptor (key, iv)) {
				byte[] decoded = ct.TransformFinalBlock (output, 0, output.Length);
				if (algo.Padding != PaddingMode.Zeros)
					Assert.AreEqual (src.Length, decoded.Length, "Decoded length error in " + msg);
				for (int i = 0; i < src.Length; i ++)
					Assert.AreEqual (src[i], decoded[i], "Decrypt error in " + msg);
			}
		}
	}
}
