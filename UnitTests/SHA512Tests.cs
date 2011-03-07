// 
// Copyright (c) 2011, Kazuki Oikawa
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
using System.Text;
using NUnit.Framework;
using MS_SHA512 = System.Security.Cryptography.SHA512Managed;
using SHA512Managed = openCrypto.SHA.SHA512Managed;

namespace openCrypto.Tests
{
	[TestFixture, Category ("Secure Hash Function")]
	public class SHA512Tests
	{
		[Test]
		public void NIST_Examples ()
		{
			Test (new byte[0]);
			Test (Encoding.ASCII.GetBytes ("abc"));
			Test (Encoding.ASCII.GetBytes ("abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmnoijklmnopjklmnopqklmnopqrlmnopqrsmnopqrstnopqrstu"));
			Test (new byte[111]);
			Test (new byte[112]);
			Test (new byte[113]);
			Test (new byte[122]);
			Test (new byte[1000]);
			Test (Encoding.ASCII.GetBytes (new string ('A', 1000)));
			Test (Encoding.ASCII.GetBytes (new string ('U', 1005)));
		}

		[Test]
		public void RandomTest ()
		{
			Random rnd = new Random ();
			int min = 1, max = 1024 * 16;
			for (int i = 0; i < 1024; i  ++)
					Test (rnd.Next (min, max));
		}

		void Test (int size)
		{
			Test (openCrypto.RNG.GetBytes (size));
		}

		void Test (byte[] data)
		{
			byte[] expected = new MS_SHA512 ().ComputeHash (data);
			byte[] actual = new SHA512Managed ().ComputeHash (data);
			Assert.AreEqual (expected, actual, "ComputeHash " + data.Length.ToString () + " bytes");
		}
	}
}
