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
	/// <ja>各種テスト用のダミー対称アルゴリズム</ja>
	/// </summary>
	class DummyAlgorithm : SymmetricAlgorithmPlus
	{
		public DummyAlgorithm ()
		{
			base.KeySizeValue = 128;
			base.BlockSizeValue = 128;
			base.FeedbackSizeValue = 128;
			base.LegalBlockSizesValue = new KeySizes[] { new KeySizes (128, 128, 0) };
			base.LegalKeySizesValue = new KeySizes[] { new KeySizes (128, 128, 0) };
		}

		public override bool SupportsMultiThread {
			get { return true; }
		}

		public override ICryptoTransform CreateDecryptor (byte[] rgbKey, byte[] rgbIV)
		{
			return new DummyTransform (this, false);
		}

		public override ICryptoTransform CreateEncryptor (byte[] rgbKey, byte[] rgbIV)
		{
			return new DummyTransform (this, true);
		}

		public override void GenerateIV ()
		{
			IVValue = new byte[16];
		}

		public override void GenerateKey ()
		{
			KeyValue = new byte[16];
		}

		class DummyTransform : SymmetricTransform
		{
			public DummyTransform (SymmetricAlgorithmPlus algo, bool encryptMode)
				: base (algo, encryptMode)
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
