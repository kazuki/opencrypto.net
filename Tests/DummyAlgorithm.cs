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

namespace openCrypto.Tests
{
	/// <summary>
	/// <ja>
	/// 各種テスト用のダミー対称アルゴリズム。
	/// キーサイズとブロックサイズは64bit固定で、
	/// 暗号化／復号プロセスはただのコピー
	/// </ja>
	/// </summary>
	class DummyAlgorithm : SymmetricAlgorithmPlus
	{
		public DummyAlgorithm ()
		{
			base.KeySizeValue = 64;
			base.BlockSizeValue = 64;
			base.FeedbackSizeValue = 64;
			base.LegalBlockSizesValue = new KeySizes[] { new KeySizes (64, 64, 0) };
			base.LegalKeySizesValue = new KeySizes[] { new KeySizes (64, 64, 0) };
		}

		public override bool SupportsBlockModeParallelization {
			get { return true; }
		}

		public override ICryptoTransform CreateDecryptor (byte[] rgbKey, byte[] rgbIV)
		{
			return new DummyTransform (this, false, rgbIV);
		}

		public override ICryptoTransform CreateEncryptor (byte[] rgbKey, byte[] rgbIV)
		{
			return new DummyTransform (this, true, rgbIV);
		}

		public override void GenerateIV ()
		{
			IVValue = new byte[8];
		}

		public override void GenerateKey ()
		{
			KeyValue = new byte[8];
		}

		public override bool HasImplementation (CipherImplementationType type)
		{
			return true;
		}

		class DummyTransform : SymmetricTransform
		{
			public DummyTransform (SymmetricAlgorithmPlus algo, bool encryptMode, byte[] iv)
				: base (algo, encryptMode, iv)
			{
			}

			protected override void EncryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
			{
				Buffer.BlockCopy (inputBuffer, inputOffset, outputBuffer, outputOffset, InputBlockSize);
			}

			protected override void DecryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
			{
				Buffer.BlockCopy (inputBuffer, inputOffset, outputBuffer, outputOffset, InputBlockSize);
			}
		}
	}
}
#endif
