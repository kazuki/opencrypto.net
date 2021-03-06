// 
// Copyright (c) 2006, Kazuki Oikawa
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
using System.Reflection;
using System.IO;
using System.Security.Cryptography;
using NUnit.Framework;

namespace openCrypto.Tests
{
	[TestFixture, Category ("SymmetricCryptography")]
	public class RijndaelTest : SymmetricAlgorithmTestBase
	{
		public RijndaelTest ()
		{
		}

		[Test]
		public void TestECB ()
		{
			string[] files = new string[] {
#if MONO
				"ecb_tbl.txt", "ecb_vk.txt", "ecb_vt.txt"
#else
				"UnitTests.ecb_tbl.txt", "UnitTests.ecb_vk.txt", "UnitTests.ecb_vt.txt"
#endif
			};
			RijndaelManaged algo = new RijndaelManaged ();

			foreach (string file in files) {
				ECBTestReader helper = new ECBTestReader (file);
				foreach (CipherImplementationType type in _types) {
					if (algo.HasImplementation (type)) {
						algo.ImplementationType = type;
						TestECB (algo, helper);
					}
				}
			}
		}

		[Test]
		public void TestECB_MultiBlock_1 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.ECB;
			Test_MultiBlock_1 (algo);
		}

		[Test]
		public void TestECB_MultiBlock_2 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.ECB;
			Test_MultiBlock_2 (algo);
		}

		[Test]
		public void TestCBC_MultiBlock_1 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.CBC;
			Test_MultiBlock_1 (algo);
		}

		[Test]
		public void TestCBC_MultiBlock_2 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.CBC;
			Test_MultiBlock_2 (algo);
		}

		[Test]
		public void TestOFB_MultiBlock_1 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.OFB;
			Test_MultiBlock_1 (algo);
		}

		[Test]
		public void TestOFB_MultiBlock_2 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.OFB;
			Test_MultiBlock_2 (algo);
		}

		[Test]
		public void TestCFB_MultiBlock_1 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.CFB;
			Test_MultiBlock_1 (algo);
		}

		[Test]
		public void TestCFB_MultiBlock_2 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.CFB;
			Test_MultiBlock_2 (algo);
		}

		[Test]
		public void TestCTR_MultiBlock_1 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.CTR;
			Test_MultiBlock_1 (algo);
		}

		[Test]
		public void TestCTR_MultiBlock_2 ()
		{
			SymmetricAlgorithmPlus algo = new RijndaelManaged ();
			algo.ModePlus = CipherModePlus.CTR;
			Test_MultiBlock_2 (algo);
		}

		[Test]
		public void Test_SameBuffer ()
		{
			base.Test_SameBuffer (new RijndaelManaged ());
		}

		class ECBTestReader : ECBTestHelper
		{
			StreamReader _reader;

			public ECBTestReader (string name)
			{
				Assembly asm = Assembly.GetExecutingAssembly ();
				_reader = new StreamReader (asm.GetManifestResourceStream (name));
			}

			public override bool ReadNext ()
			{
				string line;
				byte[] key = null, plain = null, ct;
				
				while ((line = _reader.ReadLine ()) != null) {
					if (line.StartsWith ("KEY=")) {
						key = str2array (line);
						continue;
					}
					if (line.StartsWith ("PT=")) {
						plain = str2array (line);
						continue;
					}
					if (!line.StartsWith ("CT="))
						continue;
					
					ct = str2array (line);
					if (key != null) UpdateKey (key);
					if (plain != null) UpdatePlainText (plain);
					UpdateCryptText (ct);
					key = plain = ct = null;
					return true;
				}
				return false;
			}

			static byte[] str2array (string line)
			{
				line = line.Substring (line.IndexOf ('=') + 1);
				byte[] temp = new byte[line.Length >> 1];
				for (int i = 0; i < line.Length; i += 2)
					temp[i >> 1] = byte.Parse (line.Substring (i, 2), System.Globalization.NumberStyles.HexNumber);
				return temp;
			}

			public override void Close ()
			{
				_reader.Close ();
			}
		}
	}
}
