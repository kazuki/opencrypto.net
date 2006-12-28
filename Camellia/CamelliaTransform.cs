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

		protected abstract uint LeftRotate1 (uint value);

		unsafe void EncryptBlock (uint* plaintext, uint* ciphertext, uint *k, uint[] sbox1, uint[] sbox2, uint[] sbox3, uint[] sbox4)
		{
			uint x0 = plaintext[0] ^ k[0];
			uint x1 = plaintext[1] ^ k[1];
			uint x2 = plaintext[2] ^ k[2];
			uint x3 = plaintext[3] ^ k[3];
			
			for (int i = 0;; i++) {
				uint s1 = x0 ^ k[4];
				uint U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				uint s2 = x1 ^ k[5];
				uint D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x2 ^= D ^ U;
				x3 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x2 ^ k[6];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x3 ^ k[7];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x0 ^= D ^ U;
				x1 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x0 ^ k[8];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x1 ^ k[9];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x2 ^= D ^ U;
				x3 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x2 ^ k[10];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x3 ^ k[11];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x0 ^= D ^ U;
				x1 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x0 ^ k[12];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x1 ^ k[13];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x2 ^= D ^ U;
				x3 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x2 ^ k[14];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x3 ^ k[15];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x0 ^= D ^ U;
				x1 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				if (i == _flayerLimit) break;
				
				k += 16;
				x1 ^= LeftRotate1 (x0 & k[0]);
				x0 ^= x1 | k[1];
				x2 ^= x3 | k[3];
				x3 ^= LeftRotate1 (x2 & k[2]);
			}
			
			ciphertext[0] = k[16] ^ x2;
			ciphertext[1] = k[17] ^ x3;
			ciphertext[2] = k[18] ^ x0;
			ciphertext[3] = k[19] ^ x1;
		}

		unsafe void DecryptBlock (uint* ciphertext, uint* plaintext, uint* k, uint[] sbox1, uint[] sbox2, uint[] sbox3, uint[] sbox4)
		{
			k += (_flayerLimit == 2 ? 46 : 62);
			uint x0 = ciphertext[0] ^ k[2];
			uint x1 = ciphertext[1] ^ k[3];
			uint x2 = ciphertext[2] ^ k[4];
			uint x3 = ciphertext[3] ^ k[5];
			
			for (int i = 0;; i++) {
				uint s1 = x0 ^ k[0];
				uint U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				uint s2 = x1 ^ k[1];
				uint D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x2 ^= D ^ U;
				x3 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x2 ^ k[-2];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x3 ^ k[-1];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x0 ^= D ^ U;
				x1 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x0 ^ k[-4];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x1 ^ k[-3];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x2 ^= D ^ U;
				x3 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x2 ^ k[-6];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x3 ^ k[-5];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x0 ^= D ^ U;
				x1 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x0 ^ k[-8];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x1 ^ k[-7];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x2 ^= D ^ U;
				x3 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				s1 = x2 ^ k[-10];
				U = sbox1[(byte)s1] ^ sbox2[(byte)(s1 >> 8)] ^ sbox3[(byte)(s1 >> 16)] ^ sbox4[(byte)(s1 >> 24)];
				s2 = x3 ^ k[-9];
				D = sbox2[(byte)s2] ^ sbox3[(byte)(s2 >> 8)] ^ sbox4[(byte)(s2 >> 16)] ^ sbox1[(byte)(s2 >> 24)];
				x0 ^= D ^ U;
				x1 ^= D ^ U ^ ((U << 8) | (U >> 24));
				
				if (i == _flayerLimit) break;
				k -= 16;
				x1 ^= LeftRotate1 (x0 & k[4]);
				x0 ^= x1 | k[5];
				x2 ^= x3 | k[3];
				x3 ^= LeftRotate1 (x2 & k[2]);
			}
			
			plaintext[0] = k[-14] ^ x2;
			plaintext[1] = k[-13] ^ x3;
			plaintext[2] = k[-12] ^ x0;
			plaintext[3] = k[-11] ^ x1;
		}

		protected override unsafe void EncryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			fixed (byte* input = &inputBuffer[inputOffset], output = &outputBuffer[outputOffset])
			fixed (uint *k = _keyTable)
				EncryptBlock ((uint*)input, (uint*)output, k, _sbox1, _sbox2, _sbox3, _sbox4);
		}

		protected override unsafe void DecryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			fixed (byte* input = &inputBuffer[inputOffset], output = &outputBuffer[outputOffset])
			fixed (uint *k = _keyTable)
				DecryptBlock ((uint*)input, (uint*)output, k, _sbox1, _sbox2, _sbox3, _sbox4);
		}
	}
}
