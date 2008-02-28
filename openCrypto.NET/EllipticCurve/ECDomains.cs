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
	static class ECDomains
	{
		delegate ECDomainParameters CreateDomainParameterDelegate ();
		static Dictionary<int, ECDomainParameters> _cache;
		static Dictionary<int, Uri> _oidReverseMap;
		static Dictionary<Uri, int> _oidMap;
		static Dictionary<int, CreateDomainParameterDelegate> _creator;
		const string URN_OID_PREFIX = "urn:oid:";
		const string OID_CERTICOM_EC = URN_OID_PREFIX + "1.3.132.0.";
		const string OID_ANSI_X9_62 = URN_OID_PREFIX + "1.2.840.10045.";
		const string OID_ANSI_X9_64_PRIME_CURVE = OID_ANSI_X9_62 + "3.1.";

		static ECDomains ()
		{
			_cache = new Dictionary<int, ECDomainParameters> ();
			_creator = new Dictionary<int, CreateDomainParameterDelegate> ();
			_oidMap = new Dictionary<Uri, int> ();
			_oidReverseMap = new Dictionary<int, Uri> ();

			ECDomainNames[] names = new ECDomainNames[] {
				ECDomainNames.secp112r1,
				ECDomainNames.secp112r2,
				ECDomainNames.secp128r1,
				ECDomainNames.secp128r2,
				ECDomainNames.secp160r1,
				ECDomainNames.secp160r2,
				ECDomainNames.secp192r1,
				ECDomainNames.secp224r1,
				ECDomainNames.secp256r1,
				ECDomainNames.secp384r1,
				ECDomainNames.secp521r1
			};
			CreateDomainParameterDelegate[] procs = new CreateDomainParameterDelegate[] {
				Create_secp112r1,
				Create_secp112r2,
				Create_secp128r1,
				Create_secp128r2,
				Create_secp160r1,
				Create_secp160r2,
				Create_secp192r1,
				Create_secp224r1,
				Create_secp256r1,
				Create_secp384r1,
				Create_secp521r1
			};
			Uri[] oids = new Uri[] {
				new Uri (OID_CERTICOM_EC + "6"),
				new Uri (OID_CERTICOM_EC + "7"),
				new Uri (OID_CERTICOM_EC + "28"),
				new Uri (OID_CERTICOM_EC + "29"),
				new Uri (OID_CERTICOM_EC + "8"),
				new Uri (OID_CERTICOM_EC + "30"),
				new Uri (OID_ANSI_X9_64_PRIME_CURVE + "1"),
				new Uri (OID_CERTICOM_EC + "33"),
				new Uri (OID_ANSI_X9_64_PRIME_CURVE + "7"),
				new Uri (OID_CERTICOM_EC + "34"),
				new Uri (OID_CERTICOM_EC + "35")
			};

			for (int i = 0; i < names.Length; i ++) {
				_creator.Add ((int)names[i], procs[i]);
				_oidMap.Add (oids[i], (int)names[i]);
				_oidReverseMap.Add ((int)names[i], oids[i]);
			}
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

		public static ECDomainParameters GetDomainParameter (Uri oid)
		{
			return GetDomainParameter (GetDomainName (oid));
		}

		public static ECDomainNames GetDomainName (Uri oid)
		{
			return (ECDomainNames)_oidMap[oid];
		}

		static ECDomainParameters Create_secp112r1 ()
		{
			Number p = new Number (new uint[] {3199017099, 1583775862, 717185763, 56188});
			Number a = new Number (new uint[] {3199017096, 1583775862, 717185763, 56188});
			Number b = new Number (new uint[] {292563746, 384753289, 4172940345, 26014});
			Number gX = new Number (new uint[] {4190302360, 1592224597, 1916377434, 2376});
			Number gY = new Number (new uint[] {267875584, 3231858190, 3853485860, 43164});
			Number order = new Number (new uint[] {2892325317, 1584802015, 717185763, 56188});
			Montgomery mont = new Montgomery (p);
			return Create (a, b, gX, gY, order, 1, mont, _oidReverseMap[(int)ECDomainNames.secp112r1]);
		}

		static ECDomainParameters Create_secp112r2 ()
		{
			Number p = new Number (new uint[] {3199017099, 1583775862, 717185763, 56188});
			Number a = new Number (new uint[] {1544482860, 2315954934, 3259762163, 24871});
			Number b = new Number (new uint[] {1283839753, 3983867075, 4051787189, 20958});
			Number gX = new Number (new uint[] {3499263555, 3034670237, 179693714, 19363});
			Number gY = new Number (new uint[] {1855286935, 927457011, 1190496302, 44493});
			Number order = new Number (new uint[] {86036555, 3612966049, 179296440, 14047});
			Montgomery mont = new Montgomery (p);
			return Create (a, b, gX, gY, order, 4, mont, _oidReverseMap[(int)ECDomainNames.secp112r2]);
		}

		static ECDomainParameters Create_secp128r1 ()
		{
			Number p = new Number (new uint[] {4294967295, 4294967295, 4294967295, 4294967293});
			Number a = new Number (new uint[] {4294967292, 4294967295, 4294967295, 4294967293});
			Number b = new Number (new uint[] {753819347, 3626277180, 276427837, 3900012993});
			Number gX = new Number (new uint[] {2771147654, 203972732, 2341051181, 371193682});
			Number gY = new Number (new uint[] {3723328131, 3224216210, 1538255635, 3478833209});
			Number order = new Number (new uint[] {2419630357, 1973619995, 0, 4294967294});
			Montgomery mont = new Montgomery (p);
			return Create (a, b, gX, gY, order, 1, mont, _oidReverseMap[(int)ECDomainNames.secp128r1]);
		}

		static ECDomainParameters Create_secp128r2 ()
		{
			Number p = new Number (new uint[] {4294967295, 4294967295, 4294967295, 4294967293});
			Number a = new Number (new uint[] {3220811489, 3210333339, 3518217214, 3590527384});
			Number b = new Number (new uint[] {3144518237, 3693897048, 2161125657, 1592720547});
			Number gX = new Number (new uint[] {3454779712, 3875222183, 1582770563, 2070586840});
			Number gY = new Number (new uint[] {1606634308, 1896283776, 2303539950, 666276202});
			Number order = new Number (new uint[] {101954979, 3187680370, 2147483647, 1073741823});
			Montgomery mont = new Montgomery (p);
			return Create (a, b, gX, gY, order, 4, mont, _oidReverseMap[(int)ECDomainNames.secp128r2]);
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
			return Create (a, b, gX, gY, order, 1, mont, _oidReverseMap[(int)ECDomainNames.secp160r1]);
		}

		static ECDomainParameters Create_secp160r2 ()
		{
			Number p = new Number (new uint[] {4294945907, 4294967294, 4294967295, 4294967295, 4294967295});
			Number a = new Number (new uint[] {4294945904, 4294967294, 4294967295, 4294967295, 4294967295});
			Number b = new Number (new uint[] {4110649530, 73813338, 2874615625, 4216974219, 3034658003});
			Number gX = new Number (new uint[] {826592877, 821500317, 525332763, 691671422, 1390194740});
			Number gY = new Number (new uint[] {2815704878, 4187499774, 3765565965, 3811701398, 4272946930});
			Number order = new Number (new uint[] {4087456107, 3884361752, 13598, 0, 0, 1});
			Montgomery mont = new Montgomery (p);
			return Create (a, b, gX, gY, order, 1, mont, _oidReverseMap[(int)ECDomainNames.secp160r2]);
		}

		static ECDomainParameters Create_secp192r1 ()
		{
			SECP192r1 ff = new SECP192r1 ();
			Number a = new Number (new uint[] {4294967292, 4294967295, 4294967294, 4294967295, 4294967295, 4294967295});
			Number b = new Number (new uint[] {3242637745, 4273528556, 1914974281, 262662571, 3852239079, 1679885593});
			Number gX = new Number (new uint[] {2197753874, 4110355197, 1134659584, 2092900587, 2955972854, 411936782});			
			Number gY = new Number (new uint[] {511264785, 1945728929, 1797574101, 1661997549, 4291353208, 119090069});
			Number order = new Number (new uint[] {3033671729, 342608305, 2581526582, 4294967295, 4294967295, 4294967295});
			return Create (a, b, gX, gY, order, 1, ff, _oidReverseMap[(int)ECDomainNames.secp192r1]);
		}

		static ECDomainParameters Create_secp224r1 ()
		{
			SECP224r1 ff = new SECP224r1 ();
			Number a = new Number (new uint[] {4294967294, 4294967295, 4294967295, 4294967294, 4294967295, 4294967295, 4294967295});
			Number b = new Number (new uint[] {592838580, 655046979, 3619674298, 1346678967, 4114690646, 201634731, 3020229253});
			Number gX = new Number (new uint[] {291249441, 875725014, 1455558946, 1241760211, 840143033, 1807007615, 3071151293});
			Number gY = new Number (new uint[] {2231402036, 1154843033, 1510426468, 3443750304, 1277353958, 3052872699, 3174523784});
			Number order = new Number (new uint[] {1549543997, 333261125, 3770216510, 4294907554, 4294967295, 4294967295, 4294967295});
			return Create (a, b, gX, gY, order, 1, ff, _oidReverseMap[(int)ECDomainNames.secp224r1]);
		}

		static ECDomainParameters Create_secp256r1 ()
		{
			SECP256r1 ff = new SECP256r1 ();
			Number a = new Number (new uint[] {4294967292, 4294967295, 4294967295, 0, 0, 0, 1, 4294967295});
			Number b = new Number (new uint[] {668098635, 1003371582, 3428036854, 1696401072, 1989707452, 3018571093, 2855965671, 1522939352});
			Number gX = new Number (new uint[] {3633889942, 4104206661, 770388896, 1996717441, 1671708914, 4173129445, 3777774151, 1796723186});
			Number gY = new Number (new uint[] {935285237, 3417718888, 1798397646, 734933847, 2081398294, 2397563722, 4263149467, 1340293858});
			Number order = new Number (new uint[] {4234356049, 4089039554, 2803342980, 3169254061, 4294967295, 4294967295, 0, 4294967295});
			return Create (a, b, gX, gY, order, 1, ff, _oidReverseMap[(int)ECDomainNames.secp256r1]);
		}

		static ECDomainParameters Create_secp384r1 ()
		{
			SECP384r1 ff = new SECP384r1 ();
			Number a = new Number (new uint[] {4294967292, 0, 0, 4294967295, 4294967294, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295});
			Number b = new Number (new uint[] {3555470063, 713410797, 2318324125, 3327539597, 1343457114, 51644559, 4269883666, 404593774, 3824692505, 2559444331, 3795773412, 3006345127});
			Number gX = new Number (new uint[] {1920338615, 978607672, 3210029420, 1426256477, 2186553912, 1509376480, 2343017368, 1847409506, 4079005044, 2394015518, 3196781879, 2861025826});
			Number gY = new Number (new uint[] {2431258207, 2051218812, 494829981, 174109134, 3052452032, 3923390739, 681186428, 4176747965, 2459098153, 1570674879, 2519084143, 907533898});
			Number order = new Number (new uint[] {3435473267, 3974895978, 1219536762, 1478102450, 4097256927, 3345173889, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295});
			return Create (a, b, gX, gY, order, 1, ff, _oidReverseMap[(int)ECDomainNames.secp384r1]);
		}

		static ECDomainParameters Create_secp521r1 ()
		{
			SECP521r1 ff = new SECP521r1 ();
			Number a = new Number (new uint[] {4294967292, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 511});
			Number b = new Number (new uint[] {1800421120, 4014284756, 1026307313, 896786312, 1001504519, 374522045, 3967718267, 1444493649, 2398161377, 3098839441, 2578650611, 2732225115, 3062186222, 2459574688, 2384239135, 2503915873, 81});
			Number gX = new Number (new uint[] {3269836134, 4185816625, 2238333595, 860402625, 2734663902, 4263362855, 4024916264, 2706071159, 1800224186, 4163415904, 88061217, 2623832377, 597013570, 2654915430, 67430861, 2240677559, 198});
			Number gY = new Number (new uint[] {2681300560, 2294191222, 2725429824, 893153414, 1068304225, 3310401793, 1593058880, 2548986521, 658400812, 397393175, 1469793384, 2566210633, 746396633, 1552572340, 2587607044, 959015544, 280});
			Number order = new Number (new uint[] {2436391945, 3144660766, 2308720558, 1001769400, 4144604624, 2144076104, 3207566955, 1367771011, 4294967290, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 4294967295, 511});
			return Create (a, b, gX, gY, order, 1, ff, _oidReverseMap[(int)ECDomainNames.secp521r1]);
		}

		static ECDomainParameters Create (Number a, Number b, Number gX, Number gY, Number order, uint h, IFiniteField ff, Uri uri)
		{
			ECGroup group = new ECGroup (ff.ToElement (a), ff.ToElement (b), ff.Modulus, ff);
			ECPoint basePoint = new ECPoint (group, ff.ToElement (gX), ff.ToElement (gY), ff.ToElement (Number.One));
			return new ECDomainParameters (group, basePoint, order, h, (uint)ff.Modulus.BitCount (), new Classical (order), uri);
		}
	}
}
