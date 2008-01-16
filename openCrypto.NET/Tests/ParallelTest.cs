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
	public class ParallelTest
	{
		[Test]
		public void TestCBC ()
		{
			RunTest (new CamelliaManaged (), CipherModePlus.CBC);
		}

		[Test]
		public void TestCTR ()
		{
			RunTest (new CamelliaManaged (), CipherModePlus.CTR);
		}

		private void RunTest (SymmetricAlgorithmPlus algo, CipherModePlus mode)
		{
			const int TestDataSize = 1 << 20;
			byte[] data1 = new byte[TestDataSize];
			byte[] data2 = new byte[TestDataSize];
			byte[] data3 = new byte[TestDataSize];
			byte[] iv = new byte[algo.IV.Length];
			byte[] key = new byte[algo.Key.Length];

			RNG.Instance.GetBytes (data1);
			RNG.Instance.GetBytes (key);
			RNG.Instance.GetBytes (iv);
			algo.ModePlus = mode;

			algo.NumberOfThreads = 4;
			using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
				ct.TransformBlock (data1, 0, TestDataSize, data2, 0);
			}
			algo.NumberOfThreads = 1;
			using (ICryptoTransform ct = algo.CreateDecryptor (key, iv)) {
				ct.TransformBlock (data2, 0, TestDataSize, data3, 0);
			}
			for (int i = 0; i < TestDataSize; i ++)
				Assert.AreEqual (data1[i], data3[i], "Encryption Error");

			algo.NumberOfThreads = 1;
			using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
				ct.TransformBlock (data1, 0, TestDataSize, data2, 0);
			}
			algo.NumberOfThreads = 4;
			using (ICryptoTransform ct = algo.CreateDecryptor (key, iv)) {
				ct.TransformBlock (data2, 0, TestDataSize, data3, 0);
			}
			for (int i = 0; i < TestDataSize; i ++)
				Assert.AreEqual (data1[i], data3[i], "Decryption Error");
		}
	}
}
#endif
