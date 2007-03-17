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
	/// <ja>System.Security.Cryptography.SymmetricAlgorithmの拡張クラス。
	/// 対称アルゴリズムのすべての実装が継承する必要がある、抽象基本クラスを表します。</ja>
	/// </summary>
	public abstract class SymmetricAlgorithmPlus : SymmetricAlgorithm
	{
		protected int _threads = 1;
		protected CipherModePlus _mode;
		protected CipherImplementationType _implType = CipherImplementationType.HighSpeed;

		/// <summary>
		/// <ja>ブロックモードを利用した並列化に対応しているかを返す</ja>
		/// </summary>
		public abstract bool SupportsBlockModeParallelization {
			get;
		}

		/// <summary>
		/// <ja>スレッド数を取得または設定します。1を設定すると並列処理を行いません</ja>
		/// </summary>
		public int NumberOfThreads {
			get { return _threads; }
			set {
				if (!SupportsBlockModeParallelization)
					return;
				if (value < 1)
					throw new ArgumentOutOfRangeException ();
				_threads = value;
			}
		}

		/// <summary>
		/// <ja>対称アルゴリズムの操作モードを取得または設定します</ja>
		/// </summary>
		public override CipherMode Mode {
			get { return (CipherMode)_mode; }
			set { _mode = (CipherModePlus)value; }
		}

		/// <summary>
		/// <ja>対称アルゴリズムの操作モードを取得または設定します</ja>
		/// </summary>
		public CipherModePlus ModePlus {
			get { return _mode; }
			set { _mode = value;}
		}

		/// <summary>
		/// <ja>利用する実装タイプを取得または設定します</ja>
		/// </summary>
		public CipherImplementationType ImplementationType {
			get { return _implType; }
			set { _implType = value;}
		}

		/// <summary>
		/// <ja>指定された実装タイプを保持しているかを返すメソッド</ja>
		/// </summary>
		public abstract bool HasImplementation (CipherImplementationType type);
	}
}
