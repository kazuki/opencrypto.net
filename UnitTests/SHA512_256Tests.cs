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
using SHA512_256Managed = openCrypto.SHA.SHA512_256Managed;

namespace openCrypto.Tests
{
	[TestFixture, Category ("Secure Hash Function")]
	public class SHA512_256Tests
	{
		[Test]
		public void NIST_Examples ()
		{
			Test ("one block msg", Encoding.ASCII.GetBytes ("abc"), "53048E2681941EF99B2E29B76B4C7DABE4C2D0C634FC6D46E0E2F13107E7AF23");
			Test ("two block msg", Encoding.ASCII.GetBytes ("abcdefghbcdefghicdefghijdefghijkefghijklfghijklmghijklmnhijklmnoijklmnopjklmnopqklmnopqrlmnopqrsmnopqrstnopqrstu"),
				"3928E184FB8690F840DA3988121D31BE65CB9D3EF83EE6146FEAC861E19B563A");
		}

		[Test]
		public void PropertyTest ()
		{
			SHA512_256Managed sha = new SHA512_256Managed ();
			Assert.AreEqual (256, sha.HashSize);
		}

		void Test (string msg, byte[] data, string expected)
		{
			byte[] actual = new SHA512_256Managed ().ComputeHash (data);
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < actual.Length; i ++)
				sb.Append (actual[i].ToString ("x2"));
			Assert.AreEqual (expected.ToLower(), sb.ToString(), "ComputeHash #" + msg);
		}
	}
}
