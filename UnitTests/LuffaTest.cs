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

using System;
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture]
	public class LuffaTest
	{
		const int Luffa224 = 224;
		const int Luffa256 = 256;
		const int Luffa384 = 384;
		const int Luffa512 = 512;

		[Test]
		public void ABC ()
		{
			byte[] msg = System.Text.Encoding.ASCII.GetBytes ("abc");
			Test (Luffa224, msg, new byte[] {0xf2, 0x93, 0x11, 0xb8, 0x7e, 0x9e,
				0x40, 0xde, 0x76, 0x99, 0xbe, 0x23, 0xfb, 0xeb, 0x5a, 0x47, 0xcb,
				0x16, 0xea, 0x4f, 0x55, 0x56, 0xd4, 0x7c, 0xa4, 0x0c, 0x12, 0xad}, "Luffa224");
			Test (Luffa256, msg, new byte[] {0xf2, 0x93, 0x11, 0xb8, 0x7e, 0x9e, 0x40,
				0xde, 0x76, 0x99, 0xbe, 0x23, 0xfb, 0xeb, 0x5a, 0x47, 0xcb, 0x16, 0xea,
				0x4f, 0x55, 0x56, 0xd4, 0x7c, 0xa4, 0x0c, 0x12, 0xad, 0x76, 0x4a, 0x73, 0xbd}, "Luffa256");
			Test (Luffa384, msg, new byte[] {0x9a, 0x7a, 0xbb, 0x79, 0x7a, 0x84, 0x0e, 0x2d,
				0x42, 0x3c, 0x34, 0xc9, 0x1f, 0x55, 0x9f, 0x68, 0x09, 0xbd, 0xb2, 0x91, 0x6f,
				0xb2, 0xe9, 0xef, 0xfe, 0xc2, 0xfa, 0x0a, 0x7a, 0x69, 0x88, 0x1b, 0xe9, 0x87,
				0x24, 0x80, 0xc6, 0x35, 0xd2, 0x0d, 0x2f, 0xd6, 0xe9, 0x5d, 0x04, 0x66, 0x01, 0xa7}, "Luffa384");
			Test (Luffa512, msg, new byte[] {0xf4, 0x02, 0x45, 0x97, 0x3e, 0x80, 0xd7, 0x9d, 0x0f,
				0x4b, 0x9b, 0x20, 0x2d, 0xdd, 0x45, 0x05, 0xb8, 0x1b, 0x88, 0x30, 0x50, 0x1b, 0xea,
				0x31, 0x61, 0x2b, 0x58, 0x17, 0xaa, 0xe3, 0x87, 0x92, 0x1d, 0xce, 0xfd, 0x80, 0x8c,
				0xa2, 0xc7, 0x80, 0x20, 0xaf, 0xf5, 0x93, 0x45, 0xd6, 0xf9, 0x1f, 0x0e, 0xe6, 0xb2,
				0xee, 0xe1, 0x13, 0xf0, 0xcb, 0xcf, 0x22, 0xb6, 0x43, 0x81, 0x38, 0x7e, 0x8a}, "Luffa512");
		}

		[Test]
		public void BlockTest ()
		{
			Luffa256Managed luffa = new Luffa256Managed ();
			byte[] msg = RNG.GetRNGBytes (128);
			byte[] expected = luffa.ComputeHash (msg);

			luffa.Initialize ();
			luffa.TransformBlock (msg, 0, 32, null, 0);
			luffa.TransformBlock (msg, 32, 32, null, 0);
			luffa.TransformBlock (msg, 64, 32, null, 0);
			luffa.TransformFinalBlock (msg, 96, 32);
			Assert.AreEqual (expected, luffa.Hash, "#1");

			luffa.Initialize ();
			luffa.TransformBlock (msg, 0, 20, null, 0);
			luffa.TransformBlock (msg, 20, 20, null, 0);
			luffa.TransformBlock (msg, 40, 20, null, 0);
			luffa.TransformBlock (msg, 60, 20, null, 0);
			luffa.TransformBlock (msg, 80, 20, null, 0);
			luffa.TransformBlock (msg, 100, 20, null, 0);
			luffa.TransformFinalBlock (msg, 120, 8);
			Assert.AreEqual (expected, luffa.Hash, "#2");

			luffa.Initialize ();
			luffa.TransformBlock (msg, 0, 70, null, 0);
			luffa.TransformFinalBlock (msg, 70, 58);
			Assert.AreEqual (expected, luffa.Hash, "#3");
		}

		void Test (int bits, byte[] msg, byte[] expected, string text)
		{
			LuffaManaged luffa;
			switch (bits) {
				case Luffa224: luffa = new Luffa224Managed (); break;
				case Luffa256: luffa = new Luffa256Managed (); break;
				case Luffa384: luffa = new Luffa384Managed (); break;
				case Luffa512: luffa = new Luffa512Managed (); break;
				default: throw new ArgumentOutOfRangeException ();
			}

			byte[] actual = luffa.ComputeHash (msg);
			Assert.AreEqual (expected, actual, text);
		}
	}
}
