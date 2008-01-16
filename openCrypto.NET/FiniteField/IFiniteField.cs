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
using openCrypto.EllipticCurve;

namespace openCrypto.FiniteField
{
	interface IFiniteField
	{
		/// <summary>引数に与えられた二つの元の加算を行い結果を返す</summary>
		Number Add (Number x, Number y);

		/// <summary>引数に与えられた二つの元の減算を行い結果を返す</summary>
		Number Subtract (Number x, Number y);

		/// <summary>引数に与えられた二つの元の乗算を行い結果を返す</summary>
		Number Multiply (Number x, Number y);

		/// <summary>引数に与えられた元の逆元を返す</summary>
		Number Invert (Number x);

		/// <summary>引数に与えられた数値を、この有限体上での表現に変換して返す</summary>
		Number ToElement (Number x);

		/// <summary>引数に与えられたこの有限体上の元を、数値に変換して返す</summary>
		Number ToNormal (Number x);

		/// <summary>この有限体の法を返す</summary>
		Number Modulus { get; }

		/// <summary>与えられた射影座標からアフィン座標に変換し、この有限体上の元を一般数値に変換して返す</summary>
		ECPoint ExportECPoint (Number x, Number y, Number z, ECGroup group);

		/// <summary>無限遠点を返す</summary>
		ECPoint GetInfinityPoint (ECGroup group);
	}
}
