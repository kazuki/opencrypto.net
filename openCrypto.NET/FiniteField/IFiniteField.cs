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
		/// <summary>�����ɗ^����ꂽ��̌��̉��Z���s�����ʂ�Ԃ�</summary>
		Number Add (Number x, Number y);

		/// <summary>�����ɗ^����ꂽ��̌��̌��Z���s�����ʂ�Ԃ�</summary>
		Number Subtract (Number x, Number y);

		/// <summary>�����ɗ^����ꂽ��̌��̏�Z���s�����ʂ�Ԃ�</summary>
		Number Multiply (Number x, Number y);

		/// <summary>�����ɗ^����ꂽ���̋t����Ԃ�</summary>
		Number Invert (Number x);

		/// <summary>�����ɗ^����ꂽ���l���A���̗L���̏�ł̕\���ɕϊ����ĕԂ�</summary>
		Number ToElement (Number x);

		/// <summary>�����ɗ^����ꂽ���̗L���̏�̌����A���l�ɕϊ����ĕԂ�</summary>
		Number ToNormal (Number x);

		/// <summary>���̗L���̖̂@��Ԃ�</summary>
		Number Modulus { get; }

		/// <summary>�^����ꂽ�ˉe���W����A�t�B�����W�ɕϊ����A���̗L���̏�̌�����ʐ��l�ɕϊ����ĕԂ�</summary>
		ECPoint ExportECPoint (Number x, Number y, Number z, ECGroup group);

		/// <summary>�������_��Ԃ�</summary>
		ECPoint GetInfinityPoint (ECGroup group);
	}
}
