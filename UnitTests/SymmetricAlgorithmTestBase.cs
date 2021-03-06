// 
// Copyright (c) 2006-2009, Kazuki Oikawa
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
using System.Security.Cryptography;
using NUnit.Framework;

namespace openCrypto.Tests
{
	/// <summary>
	/// <ja>対称鍵アルゴリズムのテストを行う抽象クラス</ja>
	/// </summary>
	public abstract class SymmetricAlgorithmTestBase
	{
		protected static CipherImplementationType[] _types = new CipherImplementationType[] {
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
			try {
				while (helper.ReadNext ()) {
					counter ++;
					if (ct == null || helper.IsUpdateKey) {
						byte[] key = helper.Key;
						keyText = ToString (key);
						if (ct != null) ct.Dispose ();
						ct = algo.CreateEncryptor (key, new byte[algo.BlockSize >> 3]);
					}
					byte[] output = new byte[ct.OutputBlockSize];
					ct.TransformBlock (helper.PlainText, 0, helper.PlainText.Length, output, 0);
					for (int i = 0; i < output.Length; i ++)
						if (output[i] != helper.CryptText[i])
							Assert.Fail (counter.ToString () + " " + ToString(helper.CryptText) + " != " + ToString (output) + " [Key=" + keyText + "]");
				}
			} finally {
				if (ct != null) ct.Dispose ();
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
						RNG.GetBytes (pt);
						RNG.GetBytes (key);
						RNG.GetBytes (iv);
						algo.ImplementationType = type;
						using (ICryptoTransform t = algo.CreateEncryptor (key, iv))
							for (int i = 0; i < pt.Length; i += blockSize >> 3)
								t.TransformBlock (pt, i, blockSize >> 3, ct, i);
						using (ICryptoTransform t = algo.CreateDecryptor (key, iv))
							for (int i = 0; i < ct.Length; i += blockSize >> 3)
								t.TransformBlock (ct, i, blockSize >> 3, tmp, i);
						for (int i = 0; i < pt.Length; i ++)
							Assert.AreEqual (pt[i], tmp[i], "Mode:" + type.ToString () + " Key:" + keySize.ToString () + "bits Block:" + blockSize.ToString () + "bits Pos:" + i.ToString ());
					}
					if (blockSizes.SkipSize == 0)
						break;
					blockSize += blockSizes.SkipSize;
				} while (blockSize <= blockSizes.MaxSize);
				if (keySizes.SkipSize == 0)
					break;
				keySize += keySizes.SkipSize;
			} while (keySize <= keySizes.MaxSize);
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
						RNG.GetBytes (pt);
						RNG.GetBytes (key);
						RNG.GetBytes (iv);
						algo.ImplementationType = type;
						using (ICryptoTransform t = algo.CreateEncryptor (key, iv))
							Assert.AreEqual (pt.Length, t.TransformBlock (pt, 0, pt.Length, ct, 0), "Mode:" + type.ToString () + " Encryption");
						using (ICryptoTransform t = algo.CreateDecryptor (key, iv))
							Assert.AreEqual (ct.Length, t.TransformBlock (ct, 0, ct.Length, tmp, 0), "Mode:" + type.ToString () + " Encryption");
						for (int i = 0; i < pt.Length; i ++)
							Assert.AreEqual (pt[i], tmp[i], "Mode:" + type.ToString () + " Key:" + keySize.ToString () + "bits Block:" + blockSize.ToString () + "bits Pos:" + i.ToString ());
					}
					if (blockSizes.SkipSize == 0)
						break;
					blockSize += blockSizes.SkipSize;
				} while (blockSize <= blockSizes.MaxSize);
				if (keySizes.SkipSize == 0)
					break;
				keySize += keySizes.SkipSize;
			} while (keySize <= keySizes.MaxSize);
		}

		protected void Test_SameBuffer (SymmetricAlgorithmPlus algo)
		{
			byte[] key = RNG.GetBytes (algo.KeySize >> 3);
			byte[] iv = RNG.GetBytes (algo.BlockSize >> 3);
			algo.Padding = PaddingMode.None;

			foreach (CipherModePlus mode in Enum.GetValues (typeof (CipherModePlus))) {
				algo.ModePlus = mode;
				if (mode == CipherModePlus.CTS)
					continue;

				byte[] buf1 = RNG.GetBytes ((algo.BlockSize >> 3) * 4);
				byte[] buf2 = RNG.GetBytes ((algo.BlockSize >> 3) * 4);

				using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
					ct.TransformBlock (buf1, 0, buf1.Length, buf2, 0);
				}
				using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
					ct.TransformBlock (buf1, 0, buf1.Length, buf1, 0);
				}
				Assert.AreEqual (buf2, buf1, string.Format ("Encrypt with {0} mode", mode));
				using (ICryptoTransform ct = algo.CreateDecryptor (key, iv)) {
					ct.TransformBlock (buf1, 0, buf1.Length, buf2, 0);
				}
				using (ICryptoTransform ct = algo.CreateDecryptor (key, iv)) {
					ct.TransformBlock (buf1, 0, buf1.Length, buf1, 0);
				}
				Assert.AreEqual (buf2, buf1, string.Format ("Decrypt with {0} mode", mode));
			}
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
