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

using System.Text;
using NUnit.Framework;
using SHA512_224Managed = openCrypto.SHA.SHA512_224Managed;

namespace openCrypto.Tests
{
	[TestFixture, Category ("Secure Hash Function")]
	public class SHA512_224Tests
	{
		[Test]
		public void NIST_Examples ()
		{
			Test ("one block msg", Encoding.ASCII.GetBytes ("abc"), "4634270F707B6A54DAAE7530460842E20E37ED265CEEE9A43E8924AA");
			Test ("two block msg", Encoding.ASCII.GetBytes ("abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmnoijklmnopjklmnopqklmnopqrlmnopqrsmnopqrstnopqrstu"),
				"23FEC5BB94D60B23308192640B0C453335D664734FE40E7268674AF9");
		}

		[Test]
		public void PropertyTest ()
		{
			SHA512_224Managed sha = new SHA512_224Managed ();
			Assert.AreEqual (224, sha.HashSize);
		}

		void Test (string msg, byte[] data, string expected)
		{
			byte[] actual = new SHA512_224Managed ().ComputeHash (data);
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < actual.Length; i ++)
				sb.Append (actual[i].ToString ("x2"));
			Assert.AreEqual (expected.ToLower(), sb.ToString(), "ComputeHash #" + msg);
		}
	}
}
