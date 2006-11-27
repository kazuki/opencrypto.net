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
	public class SymmetricAlgorithmPlusTest
	{
		/// <summary>
		/// <ja>CipherModeとCipherModePlusの互換チェックテストケース</ja>
		/// </summary>
		[Test]
		public void TestCipherModeSetting ()
		{
			DummyAlgorithm dummy = new DummyAlgorithm ();

			dummy.Mode = CipherMode.ECB;
			Assert.IsTrue (dummy.ModePlus == CipherModePlus.ECB, "#1");

			dummy.Mode = CipherMode.CBC;
			Assert.IsTrue (dummy.ModePlus == CipherModePlus.CBC, "#2");

			dummy.Mode = CipherMode.CFB;
			Assert.IsTrue (dummy.ModePlus == CipherModePlus.CFB, "#3");

			dummy.Mode = CipherMode.OFB;
			Assert.IsTrue (dummy.ModePlus == CipherModePlus.OFB, "#4");

			dummy.Mode = CipherMode.CTS;
			Assert.IsTrue (dummy.ModePlus == CipherModePlus.CTS, "#5");

			dummy.ModePlus = CipherModePlus.ECB;
			Assert.IsTrue (dummy.Mode == CipherMode.ECB, "#6");

			dummy.ModePlus = CipherModePlus.CBC;
			Assert.IsTrue (dummy.Mode == CipherMode.CBC, "#7");

			dummy.ModePlus = CipherModePlus.CFB;
			Assert.IsTrue (dummy.Mode == CipherMode.CFB, "#8");

			dummy.ModePlus = CipherModePlus.OFB;
			Assert.IsTrue (dummy.Mode == CipherMode.OFB, "#9");

			dummy.ModePlus = CipherModePlus.CTS;
			Assert.IsTrue (dummy.Mode == CipherMode.CTS, "#10");
		}
	}
}
#endif
