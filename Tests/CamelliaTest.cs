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

#if TEST

using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using NUnit.Framework;

namespace openCrypto.Tests
{
	/// <summary>
	/// <ja>Camellia対称鍵アルゴリズムのテストを行うクラス</ja>
	/// </summary>
	[TestFixture]
	public class CamelliaTest : SymmetricAlgorithmTestBase
	{
		public CamelliaTest ()
		{
		}

		[Test]
		public void TestECB ()
		{
			ECBTestReader helper = new ECBTestReader ();
			TestECB (new CamelliaManaged (), helper);
		}

		[Test]
		public void TestECB_MultiBlock_1 ()
		{
			TestECB_MultiBlock_1 (new CamelliaManaged ());
		}

		class ECBTestReader : ECBTestHelper
		{
			StreamReader _reader;

			public ECBTestReader ()
			{
				Assembly asm = Assembly.GetExecutingAssembly ();
				_reader = new StreamReader (asm.GetManifestResourceStream ("t_camellia.txt"));
			}

			public override bool ReadNext ()
			{
				String line;
				while ((line = _reader.ReadLine ()) != null) {
					line = line.TrimEnd ();
					if (line.Length < 10 || line[1] != ' ') continue;

					switch (line[0]) {
					case 'K':
						UpdateKey (ParseBytes (line));
						break;
					case 'P':
						UpdatePlainText (ParseBytes (line));
						break;
					case 'C':
						UpdateCryptText (ParseBytes (line));
						return true;
					}
				}
				return false;
			}

			static byte[] ParseBytes (string line)
			{
				int pos = line.IndexOf (':') + 2;
				byte[] bytes = new byte [(line.Length - pos + 1) / 3];
				for (int i = pos; i < line.Length; i += 3)
					bytes[(i - pos) / 3] = byte.Parse (line.Substring (i, 2), System.Globalization.NumberStyles.HexNumber);
				return bytes;
			}

			public override void Close ()
			{
				_reader.Close ();
			}
		}
	}
}

#endif
