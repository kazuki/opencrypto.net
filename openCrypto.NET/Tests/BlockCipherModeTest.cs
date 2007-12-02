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

#if TEST

using System;
using System.Security.Cryptography;
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture, Category ("SymmetricCryptography")]
	public class BlockCipherModeTest
	{
		[Test]
		public void ECBTest ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.Mode = CipherMode.ECB;
			Test (algo, "ECB #1", Helpers.GetRNGBytes (8), Helpers.GetRNGBytes (8),
					new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 255, 254, 253, 252, 251, 250, 249, 248},
					new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 255, 254, 253, 252, 251, 250, 249, 248});
		}

		[Test]
		public void CBCTest ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.Mode = CipherMode.CBC;
			Test (algo, "CBC #1", Helpers.GetRNGBytes (8),
					new byte[] {2, 3, 4, 5, 6, 7, 8, 9},
					new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 255, 254, 253, 252, 251, 250, 249, 248},
					new byte[] {3, 1, 7, 1, 3, 1, 15, 1, 255 ^ 3, 254 ^ 1, 253 ^ 7, 252 ^ 1, 251 ^ 3, 250 ^ 1, 249 ^ 15, 248 ^ 1});
		}

		[Test]
		public void CFBTest ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.Mode = CipherMode.CFB;
			Test (algo, "CFB #1", Helpers.GetRNGBytes (8),
					new byte[] {2, 3, 4, 5, 6, 7, 8, 9},
					new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 255, 254, 253, 252, 251, 250, 249, 248},
					new byte[] {2 ^ 1, 3 ^ 2, 4 ^ 3, 5 ^ 4, 6 ^ 5, 7 ^ 6, 8 ^ 7, 9 ^ 8,
					            255 ^ 2 ^ 1, 254 ^ 3 ^ 2, 253 ^ 4 ^ 3, 252 ^ 5 ^ 4, 251 ^ 6 ^ 5, 250 ^ 7 ^ 6, 249 ^ 8 ^ 7, 248 ^ 9 ^ 8});
		}

		[Test]
		public void OFBTest ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.Mode = CipherMode.OFB;
			Test (algo, "OFB #1", Helpers.GetRNGBytes (8),
					new byte[] {2, 3, 4, 5, 6, 7, 8, 9},
					new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 255, 254, 253, 252, 251, 250, 249, 248},
					new byte[] {2 ^ 1, 3 ^ 2, 4 ^ 3, 5 ^ 4, 6 ^ 5, 7 ^ 6, 8 ^ 7, 9 ^ 8,
					            255 ^ 2, 254 ^ 3, 253 ^ 4, 252 ^ 5, 251 ^ 6, 250 ^ 7, 249 ^ 8, 248 ^ 9});
		}

		[Test]
		public void CTRTest ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			algo.ModePlus = CipherModePlus.CTR;
			Test (algo, "CTR #1", Helpers.GetRNGBytes (8),
					new byte[] {0, 0xFF, 1, 0xFF, 2, 0xFF, 3, 0xFF},
					new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 255, 254, 253, 252, 251, 250, 249, 248},
					new byte[] {1 ^ 0, 2 ^ 0xFF, 3 ^ 1, 4 ^ 0xFF, 5 ^ 2, 6 ^ 0xFF, 7 ^ 3, 8 ^ 0xFF,
									255 ^ 0, 254 ^ 0xFF, 253 ^ 1, 252 ^ 0xFF, 251 ^ 2, 250 ^ 0xFF, 249 ^ 4, 248 ^ 0});
		}
		
		static void Test (SymmetricAlgorithmPlus algo, String msg, byte[] key, byte[] iv, byte[] src, byte[] enc)
		{
			using (ICryptoTransform ect = algo.CreateEncryptor (key, iv), dct = algo.CreateDecryptor (key, iv)) {
				byte[] temp = new byte [iv.Length], temp2 = new byte [iv.Length];
				for (int i = 0; i < src.Length; i += iv.Length) {
					ect.TransformBlock (src, i, iv.Length, temp, 0);
					for (int j = 0; j < temp.Length; j ++)
						Assert.AreEqual (enc[i + j], temp[j], msg + string.Format (" Encryption Error Block:{0}, Pos:{1}", i / iv.Length, j));
					dct.TransformBlock (temp, 0, temp.Length, temp2, 0);
					for (int j = 0; j < temp2.Length; j ++) {
						Assert.AreEqual (src[i + j], temp2[j], msg + string.Format (" Decryption Error Block:{0}, Pos:{1}", i / iv.Length, j));
						temp[j] = temp2[j] = 0;
					}
				}
			}
		}
	}
}

#endif
