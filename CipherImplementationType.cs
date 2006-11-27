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

namespace openCrypto
{
	/// <summary>
	/// <ja>実装の種類を表す列挙体</ja>
	/// </summary>
	public enum CipherImplementationType
	{
		/// <summary>
		/// <ja>速度よりも消費メモリ量の少なさを重視した実装</ja>
		/// </summary>
		LowMemory,

		/// <summary>
		/// <ja>速度とメモリ使用量の両立を目指した実装</ja>
		/// </summary>
		Balanced,

		/// <summary>
		/// <ja>速度を重視した実装</ja>
		/// </summary>
		HighSpeed
	}
}
