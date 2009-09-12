// 
// Copyright (c) 2008, Kazuki Oikawa
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
using openCrypto.FiniteField;
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture, Category ("ECC")]
	public class NumberTest
	{
		[Test]
		public void ConstructorTest1 ()
		{
			byte[] ary1 = new byte[] {0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78, 0x89, 0x9a, 0xab, 0xbc, 0xcd, 0xde, 0xef, 0xf1, 0x12};
			byte[] ary2 = new byte[16]; ary2[ary2.Length - 1] = ary1[ary1.Length - 1];
			byte[] ary3 = new byte[15]; Array.Copy (ary1, 0, ary3, 0, ary3.Length);
			byte[] ary4 = new byte[15]; ary4[ary4.Length - 1] = ary3[ary3.Length - 1];
			byte[] ary5 = new byte[14]; Array.Copy (ary1, 0, ary5, 0, ary5.Length);
			byte[] ary6 = new byte[14]; ary6[ary6.Length - 1] = ary5[ary5.Length - 1];
			byte[] ary7 = new byte[13]; Array.Copy (ary1, 0, ary7, 0, ary7.Length);
			byte[] ary8 = new byte[13]; ary8[ary8.Length - 1] = ary7[ary7.Length - 1];
			byte[] ary9 = new byte[32];
			byte[] tmp = new byte[16];

			RNG.Instance.GetBytes (ary9);
			ary1.CopyTo (ary9, 5);

			Number v;
			Number v1 = new Number (ary1, true);
			Number v2 = new Number (ary2, true);
			Number v3 = new Number (ary3, true);
			Number v4 = new Number (ary4, true);
			Number v5 = new Number (ary5, true);
			Number v6 = new Number (ary6, true);
			Number v7 = new Number (ary7, true);
			Number v8 = new Number (ary8, true);
			Number v9 = new Number (ary9, 5, ary1.Length, true);

			v1.CopyTo (tmp, 0);
			Assert.AreEqual (ary1, tmp, "#1");
			
			tmp = new byte[15];
			v = v1 - v2;
			Assert.IsTrue (v.CompareTo (v3) == 0, "#2");
			v.CopyTo (tmp, 0);
			Assert.AreEqual (ary3, tmp, "#3");

			tmp = new byte[14];
			v = v3 - v4;
			Assert.IsTrue (v.CompareTo (v5) == 0, "#4");
			v.CopyTo (tmp, 0);
			Assert.AreEqual (ary5, tmp, "#5");

			tmp = new byte[13];
			v = v5 - v6;
			Assert.IsTrue (v.CompareTo (v7) == 0, "#6");
			v.CopyTo (tmp, 0);
			Assert.AreEqual (ary7, tmp, "#7");

			Assert.IsTrue (v1.CompareTo (v9) == 0, "#8");
		}

		[Test]
		public void ConstructorTest2 ()
		{
			byte[] ary1 = new byte[] {0x12, 0x23, 0x34, 0x45, 0x56, 0x67, 0x78, 0x89, 0x9a, 0xab, 0xbc, 0xcd, 0xde, 0xef, 0xf1, 0x12};
			byte[] ary2 = new byte[16]; ary2[0] = ary1[0];
			byte[] ary3 = new byte[15]; Array.Copy (ary1, 1, ary3, 0, ary3.Length);
			byte[] ary4 = new byte[15]; ary4[0] = ary3[0];
			byte[] ary5 = new byte[14]; Array.Copy (ary1, 2, ary5, 0, ary5.Length);
			byte[] ary6 = new byte[14]; ary6[0] = ary5[0];
			byte[] ary7 = new byte[13]; Array.Copy (ary1, 3, ary7, 0, ary7.Length);
			byte[] ary8 = new byte[13]; ary8[0] = ary7[0];
			byte[] ary9 = new byte[32];
			byte[] tmp = new byte[16];

			RNG.Instance.GetBytes (ary9);
			ary1.CopyTo (ary9, 5);

			Number v;
			Number v1 = new Number (ary1, false);
			Number v2 = new Number (ary2, false);
			Number v3 = new Number (ary3, false);
			Number v4 = new Number (ary4, false);
			Number v5 = new Number (ary5, false);
			Number v6 = new Number (ary6, false);
			Number v7 = new Number (ary7, false);
			Number v8 = new Number (ary8, false);
			Number v9 = new Number (ary9, 5, ary1.Length, false);

			v1.CopyToBigEndian (tmp, 0, ary1.Length);
			Assert.AreEqual (ary1, tmp, "#1");
			
			tmp = new byte[15];
			v = v1 - v2;
			Assert.IsTrue (v.CompareTo (v3) == 0, "#2");
			v.CopyToBigEndian (tmp, 0, ary3.Length);
			Assert.AreEqual (ary3, tmp, "#3");

			tmp = new byte[14];
			v = v3 - v4;
			Assert.IsTrue (v.CompareTo (v5) == 0, "#4");
			v.CopyToBigEndian (tmp, 0, ary5.Length);
			Assert.AreEqual (ary5, tmp, "#5");

			tmp = new byte[13];
			v = v5 - v6;
			Assert.IsTrue (v.CompareTo (v7) == 0, "#6");
			v.CopyToBigEndian (tmp, 0, ary7.Length);
			Assert.AreEqual (ary7, tmp, "#7");

			Assert.IsTrue (v1.CompareTo (v9) == 0, "#8");
		}

		[Test]
		public void ConstructorTest3 ()
		{
			byte[] data1 = RNG.GetRNGBytes (7);
			byte[] data2 = new byte[10];
			data1[0] = 0;
			data1[data1.Length - 1] = 0;
			Number n1 = new Number (data1, false);
			n1.CopyToBigEndian (data2, 1, 7);
			Number n2 = new Number (data2, 1, 7, false);
			Assert.IsTrue (n1.CompareTo (n2) == 0);
		}

		[Test]
		public void ShiftTest ()
		{
			Number x = new Number (RNG.GetRNGBytes (8), false);
			string s = x.ToString (2);
			for (int i = 0; i < s.Length - 1; i ++)
				Assert.AreEqual (s.Substring (0, s.Length - i), (x >> i).ToString (2), ">> " + i.ToString ());

			x = Number.Parse ("1", 2);
			s = "1";
			for (int i = 0; i < s.Length - 1; i++)
				Assert.AreEqual (s + new string ('0', i), (x << i).ToString (2), "<< " + i.ToString ());

			x = new Number (RNG.GetRNGBytes (8), false);
			s = x.ToString (2);
			for (int i = 0; i < s.Length - 1; i++)
				Assert.AreEqual (s + new string ('0', i), (x << i).ToString (2), "<< " + i.ToString ());
		}
	}
}
