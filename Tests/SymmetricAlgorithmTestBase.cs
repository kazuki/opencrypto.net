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
using System.Security.Cryptography;
using NUnit.Framework;

namespace openCrypto.Tests
{
	/// <summary>
	/// <ja>対称鍵アルゴリズムのテストを行う抽象クラス</ja>
	/// </summary>
	public abstract class SymmetricAlgorithmTestBase
	{
		static CipherImplementationType[] _types = new CipherImplementationType[] {
			CipherImplementationType.Study,
			CipherImplementationType.LowMemory,
			CipherImplementationType.Balanced,
			CipherImplementationType.HighSpeed
		};

		protected SymmetricAlgorithmTestBase ()
		{
		}
		
		protected void TestECB (SymmetricAlgorithmPlus algo, ECBTestHelper helper)
		{
			ICryptoTransform ct = null;
			int counter = 0;
			algo.Mode = CipherMode.ECB;
			String keyText = "";
			while (helper.ReadNext ()) {
				counter ++;
				if (ct == null || helper.IsUpdateKey) {
					byte[] key = helper.Key;
					keyText = ToString (key);
					ct = algo.CreateEncryptor (key, new byte[algo.BlockSize >> 3]);
				}
				byte[] output = new byte[ct.OutputBlockSize];
				ct.TransformBlock (helper.PlainText, 0, helper.PlainText.Length, output, 0);
				for (int i = 0; i < output.Length; i ++)
					if (output[i] != helper.CryptText[i])
						Assert.Fail (counter.ToString () + " " + ToString(helper.CryptText) + " != " + ToString (output) + " [Key=" + keyText + "]");
			}
		}

		static String ToString (byte[] raw)
		{
			String text = "";
			for (int i = 0; i < raw.Length; i ++)
				text += raw[i].ToString ("x2");
			return text;
		}

		protected void Test_MultiBlock_1 (SymmetricAlgorithmPlus algo)
		{
			KeySizes keySizes = algo.LegalKeySizes[0];
			KeySizes blockSizes = algo.LegalBlockSizes[0];

			int keySize = keySizes.MinSize;
			do {
				int blockSize = blockSizes.MinSize;
				do {
					byte[] pt = new byte[(blockSize >> 3) * 3];
					byte[] ct = new byte[pt.Length];
					byte[] tmp = new byte[pt.Length];
					byte[] key = new byte[keySize >> 3];
					byte[] iv = new byte[blockSize >> 3];
					algo.BlockSize = blockSize;
					algo.KeySize = keySize;
					foreach (CipherImplementationType type in _types) {
						if (!algo.HasImplementation (type))
							continue;
						Helpers.RNG.GetBytes (pt);
						Helpers.RNG.GetBytes (key);
						Helpers.RNG.GetBytes (iv);
						algo.ImplementationType = type;
						using (ICryptoTransform t = algo.CreateEncryptor (key, iv))
							for (int i = 0; i < pt.Length; i += blockSize >> 3)
								t.TransformBlock (pt, i, blockSize >> 3, ct, i);
						using (ICryptoTransform t = algo.CreateDecryptor (key, iv))
							for (int i = 0; i < ct.Length; i += blockSize >> 3)
								t.TransformBlock (ct, i, blockSize >> 3, tmp, i);
						for (int i = 0; i < pt.Length; i ++)
							Assert.AreEqual (pt[i], tmp[i], "Mode:" + type.ToString () + " Pos:" + i.ToString ());
					}
					blockSize += blockSizes.SkipSize;
				} while (blockSize < blockSizes.MaxSize);
				keySize += keySizes.SkipSize;
			} while (keySize < keySizes.MaxSize);
		}

		protected void Test_MultiBlock_2 (SymmetricAlgorithmPlus algo)
		{
			KeySizes keySizes = algo.LegalKeySizes[0];
			KeySizes blockSizes = algo.LegalBlockSizes[0];

			int keySize = keySizes.MinSize;
			do {
				int blockSize = blockSizes.MinSize;
				do {
					byte[] pt = new byte[(blockSize >> 3) * 3];
					byte[] ct = new byte[pt.Length];
					byte[] tmp = new byte[pt.Length];
					byte[] key = new byte[keySize >> 3];
					byte[] iv = new byte[blockSize >> 3];
					algo.BlockSize = blockSize;
					algo.KeySize = keySize;
					foreach (CipherImplementationType type in _types) {
						if (!algo.HasImplementation (type))
							continue;
						Helpers.RNG.GetBytes (pt);
						Helpers.RNG.GetBytes (key);
						Helpers.RNG.GetBytes (iv);
						algo.ImplementationType = type;
						using (ICryptoTransform t = algo.CreateEncryptor (key, iv))
							Assert.AreEqual (pt.Length, t.TransformBlock (pt, 0, pt.Length, ct, 0), "Mode:" + type.ToString () + " Encryption");
						using (ICryptoTransform t = algo.CreateDecryptor (key, iv))
							Assert.AreEqual (ct.Length, t.TransformBlock (ct, 0, ct.Length, tmp, 0), "Mode:" + type.ToString () + " Encryption");
						for (int i = 0; i < pt.Length; i ++)
							Assert.AreEqual (pt[i], tmp[i], "Mode:" + type.ToString () + " Pos:" + i.ToString ());
					}
					blockSize += blockSizes.SkipSize;
				} while (blockSize < blockSizes.MaxSize);
				keySize += keySizes.SkipSize;
			} while (keySize < keySizes.MaxSize);
		}

		protected abstract class ECBTestHelper
		{
			protected byte[] _key = null, _plain = null, _crypt = null;
			protected bool   _encryptMode, _updateFlag = false;

			protected void UpdateKey (byte[] key)
			{
				_key = (byte[])key.Clone ();
				_updateFlag = true;
			}

			protected void UpdatePlainText (byte[] plain)
			{
				_plain = (byte[])plain.Clone ();
			}

			protected void UpdateCryptText (byte[] crypt)
			{
				_crypt = (byte[])crypt.Clone ();
			}

			public abstract bool ReadNext ();
			public abstract void Close ();

			public bool IsUpdateKey {
				get { return _updateFlag; }
			}

			public byte[] Key {
				get {
					_updateFlag = false;
					return _key;
				}
			}

			public byte[] PlainText {
				get { return _plain; }
			}

			public byte[] CryptText {
				get { return _crypt; }
			}

			public bool IsEncryptMode {
				get { return _encryptMode; }
			}
		}
	}
}

#endif
