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

#if TEST
using System;
using openCrypto.EllipticCurve;
using openCrypto.FiniteField;
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture]
	public class RecommendedCurveTest
	{
		[Test]
		public void Test_secp192r1 ()
		{
			ECDomainParameters domain = ECDomains.GetDomainParameter (ECDomainNames.secp192r1);
			Test ("FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFE FFFFFFFF FFFFFFFF",
				"FFFFFFFF FFFFFFFF FFFFFFFF FFFFFFFE FFFFFFFF FFFFFFFC",
				"64210519 E59C80E7 0FA7E9AB 72243049 FEB8DEEC C146B9B1",
				"188DA80E B03090F6 7CBF20EB 43A18800 F4FF0AFD 82FF1012",
				" 7192B95 FFC8DA78 631011ED 6B24CDD5 73F977A1 1E794811",
				"FFFFFFFF FFFFFFFF FFFFFFFF 99DEF836 146BC9B1 B4D22831",
				1, domain);
		}

		[Test]
		public void Test_secp256r1 ()
		{
			ECDomainParameters domain = ECDomains.GetDomainParameter (ECDomainNames.secp256r1);
			Test ("FFFFFFFF 00000001 00000000 00000000 00000000 FFFFFFFF FFFFFFFF FFFFFFFF",
				"FFFFFFFF 00000001 00000000 00000000 00000000 FFFFFFFF FFFFFFFF FFFFFFFC",
				"5AC635D8 AA3A93E7 B3EBBD55 769886BC 651D06B0 CC53B0F6 3BCE3C3E 27D2604B",
				"6B17D1F2 E12C4247 F8BCE6E5 63A440F2 77037D81 2DEB33A0 F4A13945 D898C296",
				"4FE342E2 FE1A7F9B 8EE7EB4A 7C0F9E16 2BCE3357 6B315ECE CBB64068 37BF51F5",
				"FFFFFFFF 00000000 FFFFFFFF FFFFFFFF BCE6FAAD A7179E84 F3B9CAC2 FC632551",
				1, domain);
		}

		static void Test (string p, string a, string b, string Gx, string Gy, string n, int h, ECDomainParameters domain)
		{
			AreEqual (p, domain.Field, "p");
			AreEqual (a, domain.A, "a");
			AreEqual (b, domain.B, "b");
			AreEqual (Gx, domain.G.X, "Gx");
			AreEqual (Gy, domain.G.Y, "Gy");
			AreEqual (n, domain.N, "n");
			Assert.AreEqual (h, domain.H, "h");
		}

		static void AreEqual (string expected, Number actual, string msg)
		{
			expected = expected.Replace (" ", "").ToLower ();
			Assert.AreEqual (expected, actual.ToString (16), msg);
		}
	}
}
#endif
