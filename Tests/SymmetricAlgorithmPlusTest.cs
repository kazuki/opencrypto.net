// 
// Copyright (c) 2006, Kazuki Oikawa
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// * Redistributions of source code must retain the above copyright notice,
//   this list of conditions and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// * Neither the name of the author nor the names of its contributors may be used
//   to endorse or promote products derived from this software
//   without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#if TEST

using System;
using System.Security.Cryptography;
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture]
	public class SymmetricAlgorithmPlusTest
	{
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
