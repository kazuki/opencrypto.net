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
	public class FiniteFieldTest
	{
		[Test]
		public void PowTest ()
		{
			IFiniteField[] ffList = new IFiniteField[2];
			ffList[0] = new SECP192r1 ();
			ffList[1] = new Montgomery (ffList[0].Modulus);
			foreach (IFiniteField ff in ffList) {
				Number x = ff.ToElement (Number.CreateRandomElement (ff.Modulus));
				Number pow2 = ff.Multiply (x, x);
				Number pow3 = ff.Multiply (pow2, x);
				Number pow4 = ff.Multiply (pow3, x);
				Assert.IsTrue (ff.Pow (x, Number.Zero).CompareTo (ff.ToElement (Number.One)) == 0);
				Assert.IsTrue (ff.Pow (x, Number.One).CompareTo (x) == 0);
				Assert.IsTrue (ff.Pow (x, Number.Two).CompareTo (pow2) == 0);
				Assert.IsTrue (ff.Pow (x, Number.Three).CompareTo (pow3) == 0);
				Assert.IsTrue (ff.Pow (x, Number.Four).CompareTo (pow4) == 0);
			}
		}

		[Test]
		public void SqrtTest ()
		{
			IFiniteField[] ffList = new IFiniteField[2];
			ffList[0] = new SECP192r1 ();
			ffList[1] = new Montgomery (ffList[0].Modulus);
			int repeats = 10;
			foreach (IFiniteField ff in ffList) {
				for (int i = 0; i < repeats; i++) {
					Number x = ff.ToElement (Number.CreateRandomElement (ff.Modulus));
					Number xx = ff.Multiply (x, x);
					Number sqrt = ff.Sqrt (xx);
					Assert.IsTrue (ff.Multiply (sqrt, sqrt).CompareTo (xx) == 0);
				}
			}
		}

		[Test]
		public void Test_SECP192r1 ()
		{
			SECP192r1 secp = new SECP192r1 ();
			Number max1 = secp.ToElement (SECP192r1.PRIME - Number.One);
			Number max2 = secp.ToElement (SECP192r1.PRIME - new Number (new uint[] { 0, 0, 1, 0, 0, 0 }));
			Number hoge = secp.Multiply (max1, max2);
		}
	}
}
