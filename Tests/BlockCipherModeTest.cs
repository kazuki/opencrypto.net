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
	[TestFixture]
	public class BlockCipherModeTest
	{
		[Test]
		public void ECBTest ()
		{
			DummyAlgorithm algo = new DummyAlgorithm ();
			Test (algo, "ECB #1", Helpers.GetRNGBytes (8), Helpers.GetRNGBytes (8),
					new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 255, 254, 253, 252, 251, 250, 249, 248},
					new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 255, 254, 253, 252, 251, 250, 249, 248});
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
