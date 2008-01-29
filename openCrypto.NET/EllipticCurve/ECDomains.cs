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
using System.Collections.Generic;
using openCrypto.FiniteField;

namespace openCrypto.EllipticCurve
{
	public static class ECDomains
	{
		delegate ECDomainParameters CreateDomainParameterDelegate ();
		static Dictionary<int, ECDomainParameters> _cache;
		static Dictionary<int, CreateDomainParameterDelegate> _creator;

		static ECDomains ()
		{
			_cache = new Dictionary<int, ECDomainParameters> ();
			_creator = new Dictionary<int, CreateDomainParameterDelegate> ();

			_creator.Add ((int)ECDomainNames.secp160r1, Create_secp160r1);
			_creator.Add ((int)ECDomainNames.secp192r1, Create_secp192r1);
			_creator.Add ((int)ECDomainNames.secp256r1, Create_secp256r1);
		}

		public static ECDomainParameters GetDomainParameter (ECDomainNames domainName)
		{
			ECDomainParameters domain;
			if (_cache.TryGetValue ((int)domainName, out domain))
				return domain;
			domain = _creator[(int)domainName] ();
			_cache[(int)domainName] = domain;
			return domain;
		}

		static ECDomainParameters Create_secp160r1 ()
		{
			Number p = new Number (new uint[] {2147483647, 4294967295, 4294967295, 4294967295, 4294967295});
			Number a = new Number (new uint[] {2147483644, 4294967295, 4294967295, 4294967295, 4294967295});
			Number b = new Number (new uint[] {3311794757, 2178208941, 1705834655, 1421703819, 479706876});
			Number gX = new Number (new uint[] {332135554, 1757645753, 1180985737, 2398450472, 1251390824});
			Number gY = new Number (new uint[] {2059795250, 69423415, 1507641618, 828937341, 598091861});
			Number order = new Number (new uint[] {3396674135, 4180127443, 128200, 0, 0, 1});
			Montgomery mont = new Montgomery (p);
			return Create (a, b, gX, gY, order, 1, mont);
		}

		static ECDomainParameters Create_secp192r1 ()
		{
			NIST_P192 ff = new NIST_P192 ();
			Number a = new Number (new uint[] {4294967292, 4294967295, 4294967294, 4294967295, 4294967295, 4294967295});
			Number b = new Number (new uint[] {3242637745, 4273528556, 1914974281, 262662571, 3852239079, 1679885593});
			Number gX = new Number (new uint[] {2197753874, 4110355197, 1134659584, 2092900587, 2955972854, 411936782});			
			Number gY = new Number (new uint[] {511264785, 1945728929, 1797574101, 1661997549, 4291353208, 119090069});
			Number order = new Number (new uint[] {3033671729, 342608305, 2581526582, 4294967295, 4294967295, 4294967295});
			return Create (a, b, gX, gY, order, 1, ff);
		}

		static ECDomainParameters Create_secp256r1 ()
		{
			NIST_P256 ff = new NIST_P256 ();
			Number a = new Number (new uint[] {4294967292, 4294967295, 4294967295, 0, 0, 0, 1, 4294967295});
			Number b = new Number (new uint[] {668098635, 1003371582, 3428036854, 1696401072, 1989707452, 3018571093, 2855965671, 1522939352});
			Number gX = new Number (new uint[] {3633889942, 4104206661, 770388896, 1996717441, 1671708914, 4173129445, 3777774151, 1796723186});
			Number gY = new Number (new uint[] {935285237, 3417718888, 1798397646, 734933847, 2081398294, 2397563722, 4263149467, 1340293858});
			Number order = new Number (new uint[] {4234356049, 4089039554, 2803342980, 3169254061, 4294967295, 4294967295, 0, 4294967295});
			return Create (a, b, gX, gY, order, 1, ff);
		}

		static ECDomainParameters Create (Number a, Number b, Number gX, Number gY, Number order, uint h, IFiniteField ff)
		{
			ECGroup group = new ECGroup (ff.ToElement (a), ff.ToElement (b), ff.Modulus, ff);
			ECPoint basePoint = new ECPoint (group, ff.ToElement (gX), ff.ToElement (gY), ff.ToElement (Number.One));
			return new ECDomainParameters (group, basePoint, order, h, (uint)ff.Modulus.BitCount (), new Classical (order));
		}
	}
}
