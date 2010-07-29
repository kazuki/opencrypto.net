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
using openCrypto.EllipticCurve;
using openCrypto.FiniteField;
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture, Category ("ECC")]
	public class CurveDomainValidation
	{
		[Test]
		public void secp112r1 ()
		{
			Validate (ECDomainNames.secp112r1);
		}

		[Test]
		public void secp112r2 ()
		{
			Validate (ECDomainNames.secp112r2);
		}

		[Test]
		public void secp128r1 ()
		{
			Validate (ECDomainNames.secp128r1);
		}

		[Test]
		public void secp128r2 ()
		{
			Validate (ECDomainNames.secp128r2);
		}

		[Test]
		public void secp160r1 ()
		{
			Validate (ECDomainNames.secp160r1);
		}

		[Test]
		public void secp160r2 ()
		{
			Validate (ECDomainNames.secp160r2);
		}

		[Test]
		public void secp192r1 ()
		{
			Validate (ECDomainNames.secp192r1);
		}

		[Test]
		public void secp224r1 ()
		{
			Validate (ECDomainNames.secp224r1);
		}

		[Test]
		public void secp256r1 ()
		{
			Validate (ECDomainNames.secp256r1);
		}

		[Test]
		public void secp384r1 ()
		{
			Validate (ECDomainNames.secp384r1);
		}

		[Test]
		public void secp521r1 ()
		{
			Validate (ECDomainNames.secp521r1);
		}

		static void Validate (ECDomainNames name)
		{
			Assert.IsTrue (ECDomains.GetDomainParameter (name).Validate (), name.ToString ());
		}
	}
}