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
	/// <ja>Camellia 32bit実装の抽象クラス</ja>
	/// </summary>
	abstract class CamelliaTransform : SymmetricTransform
	{
		protected const int BlockbyteSize = 128 / 8;
		protected const int BlockWordSize = BlockbyteSize >> 2;
		protected const int KeyTableByteLength = 272;
		protected const int KeyTableLength = KeyTableByteLength / 4;

		protected int _flayerLimit;
		protected uint[] _keyTable = new uint[KeyTableByteLength];
		protected uint[] _sbox1, _sbox2, _sbox3, _sbox4;

		protected CamelliaTransform (SymmetricAlgorithmPlus algo, bool encryptMode, int flayerLimit, byte[] iv, uint[] sbox1, uint[] sbox2, uint[] sbox3, uint[] sbox4)
			: base (algo, encryptMode, iv)
		{
			_flayerLimit = flayerLimit;

			_sbox1 = sbox1;
			_sbox2 = sbox2;
			_sbox3 = sbox3;
			_sbox4 = sbox4;
		}

		protected abstract unsafe void EncryptBlock (uint* k, uint* sbox1, uint* sbox2, uint* sbox3, uint* sbox4, uint* plaintext, uint* ciphertext);
		protected abstract unsafe void DecryptBlock (uint* k, uint* sbox1, uint* sbox2, uint* sbox3, uint* sbox4, uint* ciphertext, uint* plaintext);

		protected override unsafe void EncryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			fixed (byte* input = &inputBuffer[inputOffset], output = &outputBuffer[outputOffset])
			fixed (uint *k = _keyTable, p1 = _sbox1, p2 = _sbox2, p3 = _sbox3, p4 = _sbox4)
				EncryptBlock (k, p1, p2, p3, p4, (uint*)input, (uint*)output);
		}

		protected override unsafe void DecryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			fixed (byte* input = &inputBuffer[inputOffset], output = &outputBuffer[outputOffset])
			fixed (uint* k = _keyTable, p1 = _sbox1, p2 = _sbox2, p3 = _sbox3, p4 = _sbox4)
				DecryptBlock (k, p1, p2, p3, p4, (uint*)input, (uint*)output);
		}
	}
}
