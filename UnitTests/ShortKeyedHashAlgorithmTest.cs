// 
// Copyright (c) 2009, Kazuki Oikawa
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

using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture, Category ("MAC")]
	public class ShortKeyedHashAlgorithmTest
	{
		/// <summary>Test vector from FIPS-198a</summary>
		[Test]
		public void SHA1_With_49ByteKey_Truncated_to_12Byte ()
		{
			byte[] actual, expected;
			using (ShortKeyedHashAlgorithm shmac = new ShortKeyedHashAlgorithm (new HMACSHA1 (), 12 * 8))
			using (HMACSHA1 hmac = new HMACSHA1 ()) {
				shmac.Key = new byte[] {0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79,
					0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f, 0x80, 0x81, 0x82, 0x83,
					0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d,
					0x8e, 0x8f, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97,
					0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f, 0xa0};
				hmac.Key = shmac.Key;

				actual = hmac.ComputeHash (Encoding.ASCII.GetBytes ("Sample #4"));
				expected = new byte[] {0x9e, 0xa8, 0x86, 0xef, 0xe2, 0x68, 0xdb, 0xec, 0xce, 0x42, 0x0c, 0x75, 0x24, 0xdf, 0x32, 0xe0, 0x75, 0x1a, 0x2a, 0x26};
				Assert.AreEqual (expected, actual, "HMAC");

				actual = shmac.ComputeHash (Encoding.ASCII.GetBytes ("Sample #4"));
				expected = new byte[] {0x9e, 0xa8, 0x86, 0xef, 0xe2, 0x68, 0xdb, 0xec, 0xce, 0x42, 0x0c, 0x75};
				Assert.AreEqual (expected, actual, "Short");
			}
		}
	}
}
