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
using System.Security.Cryptography;

namespace openCrypto
{
	/// <summary>
	/// <ja>Camellia対称暗号アルゴリズムのすべての実装の継承元となる基本クラス</ja>
	/// </summary>
	public abstract class Camellia : SymmetricAlgorithmPlus
	{
		static KeySizes[] _legalKeySizes = new KeySizes[] { new KeySizes (128, 256, 64) };
		static KeySizes[] _legalBlockSizes = new KeySizes[] { new KeySizes (128, 128, 0) };

		protected Camellia ()
		{
			base.KeySizeValue = _legalKeySizes[0].MinSize;
			base.BlockSizeValue = _legalBlockSizes[0].MinSize;
			base.FeedbackSizeValue = _legalBlockSizes[0].MinSize;
			base.LegalBlockSizesValue = _legalBlockSizes;
			base.LegalKeySizesValue = _legalKeySizes;
		}
		
		public override bool SupportsBlockModeParallelization {
			get {
				return true;
			}
		}

		public override void GenerateIV ()
		{
			base.IVValue = new byte [base.BlockSizeValue >> 3];
			RNG.Instance.GetBytes (base.IVValue);
		}

		public override void GenerateKey ()
		{
			base.KeyValue = new byte [base.KeySizeValue >> 3];
			RNG.Instance.GetBytes (base.KeyValue);
		}
	}
}
