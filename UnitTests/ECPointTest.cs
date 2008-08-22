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

using NUnit.Framework;
using openCrypto.FiniteField;
using openCrypto.EllipticCurve;

namespace openCrypto.Tests
{
	[TestFixture, Category ("ECC")]
	public class ECPointTest
	{
		[Test]
		public void ToByteArrayTest ()
		{
			ECDomainParameters domain = ECDomains.GetDomainParameter (ECDomainNames.secp192r1);
			ECGroup group = domain.Group;
			ECPoint p = domain.Group.FiniteField.GetInfinityPoint (group);
			ECPoint g = domain.G.Export ();
			byte[] tmp = p.ToByteArray (true);
			Assert.IsTrue (tmp.Length == 1, "#1");
			Assert.IsTrue (tmp[0] == 0, "#2");
			p = new ECPoint (group, tmp);
			Assert.IsTrue (p.IsInifinity (), "#3");

			tmp = domain.G.ToByteArray (false);
			Assert.IsTrue (tmp.Length == ((domain.Bits >> 3) + ((domain.Bits & 7) == 0 ? 0 : 1)) * 2 + 1, "#4");
			p = new ECPoint (group, tmp).Export ();
			Assert.IsTrue (p.X.CompareTo (g.X) == 0, "#5");
			Assert.IsTrue (p.Y.CompareTo (g.Y) == 0, "#6");

			tmp = domain.G.ToByteArray (true);
			Assert.IsTrue (tmp.Length == ((domain.Bits >> 3) + ((domain.Bits & 7) == 0 ? 0 : 1)) + 1, "#7");
			p = new ECPoint (group, tmp).Export ();
			Assert.IsTrue (p.X.CompareTo (g.X) == 0, "#8");
			Assert.IsTrue (p.Y.CompareTo (g.Y) == 0, "#9");
		}

		[Test]
		public void PointCompressTestA ()
		{
			// 法が4n-1の形の時のテスト
			int repeats = 10;
			PointCompressTest (ECDomainNames.secp112r1, repeats);
			PointCompressTest (ECDomainNames.secp112r2, repeats);
			PointCompressTest (ECDomainNames.secp128r1, repeats);
			PointCompressTest (ECDomainNames.secp128r2, repeats);
			PointCompressTest (ECDomainNames.secp160r1, repeats);
			PointCompressTest (ECDomainNames.secp160r2, repeats);
			PointCompressTest (ECDomainNames.secp192r1, repeats);
			PointCompressTest (ECDomainNames.secp256r1, repeats);
			PointCompressTest (ECDomainNames.secp384r1, repeats);
			PointCompressTest (ECDomainNames.secp521r1, repeats);
		}

		[Test]
		public void PointCompressTestB ()
		{
			//法が4n-1では無い
			int repeats = 10;
			PointCompressTest (ECDomainNames.secp224r1, repeats);
		}

		void PointCompressTest (ECDomainNames name, int repeats)
		{
			ECDomainParameters domain = ECDomains.GetDomainParameter (name);
			PointCompressTest ((int)domain.Bits, domain.Group, domain.G, repeats, name.ToString ());
		}

		void PointCompressTest (int bits, ECGroup group, ECPoint p, int repeats, string name)
		{
			int bytes = (bits >> 3) + ((bits & 7) == 0 ? 0 : 1) + 1;
			for (int i = 0; i < repeats; i ++) {
				byte[] tmp = p.ToByteArray (true);
				Assert.IsTrue (tmp.Length == bytes, name + " #1");
				ECPoint x = new ECPoint (group, tmp).Export ();
				ECPoint p2 = p.Export ();
				Assert.IsTrue (x.X.CompareTo (p2.X) == 0, name + " #2");
				Assert.IsTrue (x.Y.CompareTo (p2.Y) == 0, name + " #3");
				p = p.Multiply (Number.CreateRandomElement (group.P));
			}
		}
	}
}
