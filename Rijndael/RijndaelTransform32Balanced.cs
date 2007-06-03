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
	class RijndaelTransform32Balanced : RijndaelTransform32
	{
		public RijndaelTransform32Balanced (RijndaelManaged algo, byte[] rgbKey, byte[] rgbIV, bool encryption)
			: base (algo, rgbKey, rgbIV, encryption)
		{
		}

		#region SymmetricTransform members

		protected override unsafe void EncryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			fixed (byte* pi = inputBuffer, po = outputBuffer) {
				switch (_Nb) {
					case 4:
						Encrypt128 (pi + inputOffset, po + outputOffset, _expandedKey);
						break;
					case 6:
						Encrypt192 (pi + inputOffset, po + outputOffset, _expandedKey);
						break;
					case 8:
						Encrypt256 (pi + inputOffset, po + outputOffset, _expandedKey);
						break;
					default:
						throw new NotSupportedException ();
				}
			}
		}

		protected override unsafe void DecryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			fixed (byte* pi = inputBuffer, po = outputBuffer) {
				switch (_Nb) {
					case 4:
						Decrypt128 (pi + inputOffset, po + outputOffset, _expandedKey);
						break;
					case 6:
						Decrypt192 (pi + inputOffset, po + outputOffset, _expandedKey);
						break;
					case 8:
						Decrypt256 (pi + inputOffset, po + outputOffset, _expandedKey);
						break;
					default:
						throw new NotSupportedException ();
				}
			}
		}

		#endregion

		#region Encrypt
		private unsafe void Encrypt128 (byte* indata, byte* outdata, uint[] ekey)
		{
			uint a0, a1, a2, a3, b0, b1, b2, b3;
			int ei = 8;
			int end = (_Nr == 10 ? 40 : (_Nr == 12 ? 48 : 56));

			/* Round 0 */
			a0 = (((uint)indata[0] << 24) | ((uint)indata[1] << 16) | ((uint)indata[2] << 8) | (uint)indata[3]) ^ ekey[0];
			a1 = (((uint)indata[4] << 24) | ((uint)indata[5] << 16) | ((uint)indata[6] << 8) | (uint)indata[7]) ^ ekey[1];
			a2 = (((uint)indata[8] << 24) | ((uint)indata[9] << 16) | ((uint)indata[10] << 8) | (uint)indata[11]) ^ ekey[2];
			a3 = (((uint)indata[12] << 24) | ((uint)indata[13] << 16) | ((uint)indata[14] << 8) | (uint)indata[15]) ^ ekey[3];

			/* Round 1 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[4];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[5];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[6];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[7];

			while (ei < end) {
				a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[ei++];
				a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[ei++];
				a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[ei++];
				a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[ei++];
				b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[ei++];
				b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[ei++];
				b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[ei++];
				b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[ei++];
			}

			/* Final Round */
			outdata[0] = (byte)(SBox[b0 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[1] = (byte)(SBox[(byte)(b1 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[2] = (byte)(SBox[(byte)(b2 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[3] = (byte)(SBox[(byte)b3] ^ (byte)ekey[ei++]);

			outdata[4] = (byte)(SBox[b1 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[5] = (byte)(SBox[(byte)(b2 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[6] = (byte)(SBox[(byte)(b3 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[7] = (byte)(SBox[(byte)b0] ^ (byte)ekey[ei++]);

			outdata[8] = (byte)(SBox[b2 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[9] = (byte)(SBox[(byte)(b3 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[10] = (byte)(SBox[(byte)(b0 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[11] = (byte)(SBox[(byte)b1] ^ (byte)ekey[ei++]);

			outdata[12] = (byte)(SBox[b3 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[13] = (byte)(SBox[(byte)(b0 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[14] = (byte)(SBox[(byte)(b1 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[15] = (byte)(SBox[(byte)b2] ^ (byte)ekey[ei++]);
		}

		private unsafe void Encrypt192 (byte* indata, byte* outdata, uint[] ekey)
		{
			uint a0, a1, a2, a3, a4, a5, b0, b1, b2, b3, b4, b5;
			int ei = 12;
			int end = (_Nr == 12 ? 72 : 84);

			/* Round 0 */
			a0 = (((uint)indata[0] << 24) | ((uint)indata[1] << 16) | ((uint)indata[2] << 8) | (uint)indata[3]) ^ ekey[0];
			a1 = (((uint)indata[4] << 24) | ((uint)indata[5] << 16) | ((uint)indata[6] << 8) | (uint)indata[7]) ^ ekey[1];
			a2 = (((uint)indata[8] << 24) | ((uint)indata[9] << 16) | ((uint)indata[10] << 8) | (uint)indata[11]) ^ ekey[2];
			a3 = (((uint)indata[12] << 24) | ((uint)indata[13] << 16) | ((uint)indata[14] << 8) | (uint)indata[15]) ^ ekey[3];
			a4 = (((uint)indata[16] << 24) | ((uint)indata[17] << 16) | ((uint)indata[18] << 8) | (uint)indata[19]) ^ ekey[4];
			a5 = (((uint)indata[20] << 24) | ((uint)indata[21] << 16) | ((uint)indata[22] << 8) | (uint)indata[23]) ^ ekey[5];

			/* Round 1 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[6];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[7];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[8];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[9];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[10];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[11];

			while (ei < end) {
				a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[ei++];
				a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[ei++];
				a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[ei++];
				a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[ei++];
				a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[ei++];
				a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[ei++];
				b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[ei++];
				b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[ei++];
				b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[ei++];
				b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[ei++];
				b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[ei++];
				b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[ei++];
			}

			/* Final Round */
			outdata[0] = (byte)(SBox[b0 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[1] = (byte)(SBox[(byte)(b1 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[2] = (byte)(SBox[(byte)(b2 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[3] = (byte)(SBox[(byte)b3] ^ (byte)ekey[ei++]);

			outdata[4] = (byte)(SBox[b1 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[5] = (byte)(SBox[(byte)(b2 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[6] = (byte)(SBox[(byte)(b3 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[7] = (byte)(SBox[(byte)b4] ^ (byte)ekey[ei++]);

			outdata[8] = (byte)(SBox[b2 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[9] = (byte)(SBox[(byte)(b3 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[10] = (byte)(SBox[(byte)(b4 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[11] = (byte)(SBox[(byte)b5] ^ (byte)ekey[ei++]);

			outdata[12] = (byte)(SBox[b3 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[13] = (byte)(SBox[(byte)(b4 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[14] = (byte)(SBox[(byte)(b5 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[15] = (byte)(SBox[(byte)b0] ^ (byte)ekey[ei++]);

			outdata[16] = (byte)(SBox[b4 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[17] = (byte)(SBox[(byte)(b5 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[18] = (byte)(SBox[(byte)(b0 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[19] = (byte)(SBox[(byte)b1] ^ (byte)ekey[ei++]);

			outdata[20] = (byte)(SBox[b5 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[21] = (byte)(SBox[(byte)(b0 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[22] = (byte)(SBox[(byte)(b1 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[23] = (byte)(SBox[(byte)b2] ^ (byte)ekey[ei++]);
		}

		private unsafe void Encrypt256 (byte* indata, byte* outdata, uint[] ekey)
		{
			uint a0, a1, a2, a3, a4, a5, a6, a7, b0, b1, b2, b3, b4, b5, b6, b7;

			/* Round 0 */
			a0 = (((uint)indata[0] << 24) | ((uint)indata[1] << 16) | ((uint)indata[2] << 8) | (uint)indata[3]) ^ ekey[0];
			a1 = (((uint)indata[4] << 24) | ((uint)indata[5] << 16) | ((uint)indata[6] << 8) | (uint)indata[7]) ^ ekey[1];
			a2 = (((uint)indata[8] << 24) | ((uint)indata[9] << 16) | ((uint)indata[10] << 8) | (uint)indata[11]) ^ ekey[2];
			a3 = (((uint)indata[12] << 24) | ((uint)indata[13] << 16) | ((uint)indata[14] << 8) | (uint)indata[15]) ^ ekey[3];
			a4 = (((uint)indata[16] << 24) | ((uint)indata[17] << 16) | ((uint)indata[18] << 8) | (uint)indata[19]) ^ ekey[4];
			a5 = (((uint)indata[20] << 24) | ((uint)indata[21] << 16) | ((uint)indata[22] << 8) | (uint)indata[23]) ^ ekey[5];
			a6 = (((uint)indata[24] << 24) | ((uint)indata[25] << 16) | ((uint)indata[26] << 8) | (uint)indata[27]) ^ ekey[6];
			a7 = (((uint)indata[28] << 24) | ((uint)indata[29] << 16) | ((uint)indata[30] << 8) | (uint)indata[31]) ^ ekey[7];

			/* Round 1 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[8];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[9];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a6]))) ^ ekey[10];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a6 >> 8)] ^ RotByte (T0[(byte)a7]))) ^ ekey[11];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a7 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[12];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a6 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[13];
			b6 = T0[a6 >> 24] ^ RotByte (T0[(byte)(a7 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[14];
			b7 = T0[a7 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[15];

			for (int ei = 16; ei < 112; ) {
				a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[ei++];
				a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[ei++];
				a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b6]))) ^ ekey[ei++];
				a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b6 >> 8)] ^ RotByte (T0[(byte)b7]))) ^ ekey[ei++];
				a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b7 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[ei++];
				a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b6 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[ei++];
				a6 = T0[b6 >> 24] ^ RotByte (T0[(byte)(b7 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[ei++];
				a7 = T0[b7 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[ei++];
				b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[ei++];
				b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[ei++];
				b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a6]))) ^ ekey[ei++];
				b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a6 >> 8)] ^ RotByte (T0[(byte)a7]))) ^ ekey[ei++];
				b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a7 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[ei++];
				b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a6 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[ei++];
				b6 = T0[a6 >> 24] ^ RotByte (T0[(byte)(a7 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[ei++];
				b7 = T0[a7 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[ei++];
			}

			/* Final Round */
			outdata[0] = (byte)(SBox[b0 >> 24] ^ (byte)(ekey[112] >> 24));
			outdata[1] = (byte)(SBox[(byte)(b1 >> 16)] ^ (byte)(ekey[112] >> 16));
			outdata[2] = (byte)(SBox[(byte)(b3 >> 8)] ^ (byte)(ekey[112] >> 8));
			outdata[3] = (byte)(SBox[(byte)b4] ^ (byte)ekey[112]);

			outdata[4] = (byte)(SBox[b1 >> 24] ^ (byte)(ekey[113] >> 24));
			outdata[5] = (byte)(SBox[(byte)(b2 >> 16)] ^ (byte)(ekey[113] >> 16));
			outdata[6] = (byte)(SBox[(byte)(b4 >> 8)] ^ (byte)(ekey[113] >> 8));
			outdata[7] = (byte)(SBox[(byte)b5] ^ (byte)ekey[113]);

			outdata[8] = (byte)(SBox[b2 >> 24] ^ (byte)(ekey[114] >> 24));
			outdata[9] = (byte)(SBox[(byte)(b3 >> 16)] ^ (byte)(ekey[114] >> 16));
			outdata[10] = (byte)(SBox[(byte)(b5 >> 8)] ^ (byte)(ekey[114] >> 8));
			outdata[11] = (byte)(SBox[(byte)b6] ^ (byte)ekey[114]);

			outdata[12] = (byte)(SBox[b3 >> 24] ^ (byte)(ekey[115] >> 24));
			outdata[13] = (byte)(SBox[(byte)(b4 >> 16)] ^ (byte)(ekey[115] >> 16));
			outdata[14] = (byte)(SBox[(byte)(b6 >> 8)] ^ (byte)(ekey[115] >> 8));
			outdata[15] = (byte)(SBox[(byte)b7] ^ (byte)ekey[115]);

			outdata[16] = (byte)(SBox[b4 >> 24] ^ (byte)(ekey[116] >> 24));
			outdata[17] = (byte)(SBox[(byte)(b5 >> 16)] ^ (byte)(ekey[116] >> 16));
			outdata[18] = (byte)(SBox[(byte)(b7 >> 8)] ^ (byte)(ekey[116] >> 8));
			outdata[19] = (byte)(SBox[(byte)b0] ^ (byte)ekey[116]);

			outdata[20] = (byte)(SBox[b5 >> 24] ^ (byte)(ekey[117] >> 24));
			outdata[21] = (byte)(SBox[(byte)(b6 >> 16)] ^ (byte)(ekey[117] >> 16));
			outdata[22] = (byte)(SBox[(byte)(b0 >> 8)] ^ (byte)(ekey[117] >> 8));
			outdata[23] = (byte)(SBox[(byte)b1] ^ (byte)ekey[117]);

			outdata[24] = (byte)(SBox[b6 >> 24] ^ (byte)(ekey[118] >> 24));
			outdata[25] = (byte)(SBox[(byte)(b7 >> 16)] ^ (byte)(ekey[118] >> 16));
			outdata[26] = (byte)(SBox[(byte)(b1 >> 8)] ^ (byte)(ekey[118] >> 8));
			outdata[27] = (byte)(SBox[(byte)b2] ^ (byte)ekey[118]);

			outdata[28] = (byte)(SBox[b7 >> 24] ^ (byte)(ekey[119] >> 24));
			outdata[29] = (byte)(SBox[(byte)(b0 >> 16)] ^ (byte)(ekey[119] >> 16));
			outdata[30] = (byte)(SBox[(byte)(b2 >> 8)] ^ (byte)(ekey[119] >> 8));
			outdata[31] = (byte)(SBox[(byte)b3] ^ (byte)ekey[119]);
		}
		#endregion

		#region Decrypt
		private unsafe void Decrypt128 (byte* indata, byte* outdata, uint[] ekey)
		{
			uint a0, a1, a2, a3, b0, b1, b2, b3;
			int ei = 8;
			int end = (_Nr == 10 ? 40 : (_Nr == 12 ? 48 : 56));

			/* Round 0 */
			a0 = (((uint)indata[0] << 24) | ((uint)indata[1] << 16) | ((uint)indata[2] << 8) | (uint)indata[3]) ^ ekey[0];
			a1 = (((uint)indata[4] << 24) | ((uint)indata[5] << 16) | ((uint)indata[6] << 8) | (uint)indata[7]) ^ ekey[1];
			a2 = (((uint)indata[8] << 24) | ((uint)indata[9] << 16) | ((uint)indata[10] << 8) | (uint)indata[11]) ^ ekey[2];
			a3 = (((uint)indata[12] << 24) | ((uint)indata[13] << 16) | ((uint)indata[14] << 8) | (uint)indata[15]) ^ ekey[3];

			/* Round 1 */
			b0 = iT0[a0 >> 24] ^ RotByte (iT0[(byte)(a3 >> 16)] ^ RotByte (iT0[(byte)(a2 >> 8)] ^ RotByte (iT0[(byte)a1]))) ^ ekey[4];
			b1 = iT0[a1 >> 24] ^ RotByte (iT0[(byte)(a0 >> 16)] ^ RotByte (iT0[(byte)(a3 >> 8)] ^ RotByte (iT0[(byte)a2]))) ^ ekey[5];
			b2 = iT0[a2 >> 24] ^ RotByte (iT0[(byte)(a1 >> 16)] ^ RotByte (iT0[(byte)(a0 >> 8)] ^ RotByte (iT0[(byte)a3]))) ^ ekey[6];
			b3 = iT0[a3 >> 24] ^ RotByte (iT0[(byte)(a2 >> 16)] ^ RotByte (iT0[(byte)(a1 >> 8)] ^ RotByte (iT0[(byte)a0]))) ^ ekey[7];

			while (ei < end) {
				a0 = iT0[b0 >> 24] ^ RotByte (iT0[(byte)(b3 >> 16)] ^ RotByte (iT0[(byte)(b2 >> 8)] ^ RotByte (iT0[(byte)b1]))) ^ ekey[ei++];
				a1 = iT0[b1 >> 24] ^ RotByte (iT0[(byte)(b0 >> 16)] ^ RotByte (iT0[(byte)(b3 >> 8)] ^ RotByte (iT0[(byte)b2]))) ^ ekey[ei++];
				a2 = iT0[b2 >> 24] ^ RotByte (iT0[(byte)(b1 >> 16)] ^ RotByte (iT0[(byte)(b0 >> 8)] ^ RotByte (iT0[(byte)b3]))) ^ ekey[ei++];
				a3 = iT0[b3 >> 24] ^ RotByte (iT0[(byte)(b2 >> 16)] ^ RotByte (iT0[(byte)(b1 >> 8)] ^ RotByte (iT0[(byte)b0]))) ^ ekey[ei++];
				b0 = iT0[a0 >> 24] ^ RotByte (iT0[(byte)(a3 >> 16)] ^ RotByte (iT0[(byte)(a2 >> 8)] ^ RotByte (iT0[(byte)a1]))) ^ ekey[ei++];
				b1 = iT0[a1 >> 24] ^ RotByte (iT0[(byte)(a0 >> 16)] ^ RotByte (iT0[(byte)(a3 >> 8)] ^ RotByte (iT0[(byte)a2]))) ^ ekey[ei++];
				b2 = iT0[a2 >> 24] ^ RotByte (iT0[(byte)(a1 >> 16)] ^ RotByte (iT0[(byte)(a0 >> 8)] ^ RotByte (iT0[(byte)a3]))) ^ ekey[ei++];
				b3 = iT0[a3 >> 24] ^ RotByte (iT0[(byte)(a2 >> 16)] ^ RotByte (iT0[(byte)(a1 >> 8)] ^ RotByte (iT0[(byte)a0]))) ^ ekey[ei++];
			}

			/* Final Round */
			outdata[0] = (byte)(iSBox[b0 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[1] = (byte)(iSBox[(byte)(b3 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[2] = (byte)(iSBox[(byte)(b2 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[3] = (byte)(iSBox[(byte)b1] ^ (byte)ekey[ei++]);

			outdata[4] = (byte)(iSBox[b1 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[5] = (byte)(iSBox[(byte)(b0 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[6] = (byte)(iSBox[(byte)(b3 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[7] = (byte)(iSBox[(byte)b2] ^ (byte)ekey[ei++]);

			outdata[8] = (byte)(iSBox[b2 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[9] = (byte)(iSBox[(byte)(b1 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[10] = (byte)(iSBox[(byte)(b0 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[11] = (byte)(iSBox[(byte)b3] ^ (byte)ekey[ei++]);

			outdata[12] = (byte)(iSBox[b3 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[13] = (byte)(iSBox[(byte)(b2 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[14] = (byte)(iSBox[(byte)(b1 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[15] = (byte)(iSBox[(byte)b0] ^ (byte)ekey[ei++]);
		}

		private unsafe void Decrypt192 (byte* indata, byte* outdata, uint[] ekey)
		{
			uint a0, a1, a2, a3, a4, a5, b0, b1, b2, b3, b4, b5;
			int ei = 12;
			int end = (_Nr == 12 ? 72 : 84);

			/* Round 0 */
			a0 = (((uint)indata[0] << 24) | ((uint)indata[1] << 16) | ((uint)indata[2] << 8) | (uint)indata[3]) ^ ekey[0];
			a1 = (((uint)indata[4] << 24) | ((uint)indata[5] << 16) | ((uint)indata[6] << 8) | (uint)indata[7]) ^ ekey[1];
			a2 = (((uint)indata[8] << 24) | ((uint)indata[9] << 16) | ((uint)indata[10] << 8) | (uint)indata[11]) ^ ekey[2];
			a3 = (((uint)indata[12] << 24) | ((uint)indata[13] << 16) | ((uint)indata[14] << 8) | (uint)indata[15]) ^ ekey[3];
			a4 = (((uint)indata[16] << 24) | ((uint)indata[17] << 16) | ((uint)indata[18] << 8) | (uint)indata[19]) ^ ekey[4];
			a5 = (((uint)indata[20] << 24) | ((uint)indata[21] << 16) | ((uint)indata[22] << 8) | (uint)indata[23]) ^ ekey[5];

			/* Round 1 */
			b0 = iT0[a0 >> 24] ^ RotByte (iT0[(byte)(a5 >> 16)] ^ RotByte (iT0[(byte)(a4 >> 8)] ^ RotByte (iT0[(byte)a3]))) ^ ekey[6];
			b1 = iT0[a1 >> 24] ^ RotByte (iT0[(byte)(a0 >> 16)] ^ RotByte (iT0[(byte)(a5 >> 8)] ^ RotByte (iT0[(byte)a4]))) ^ ekey[7];
			b2 = iT0[a2 >> 24] ^ RotByte (iT0[(byte)(a1 >> 16)] ^ RotByte (iT0[(byte)(a0 >> 8)] ^ RotByte (iT0[(byte)a5]))) ^ ekey[8];
			b3 = iT0[a3 >> 24] ^ RotByte (iT0[(byte)(a2 >> 16)] ^ RotByte (iT0[(byte)(a1 >> 8)] ^ RotByte (iT0[(byte)a0]))) ^ ekey[9];
			b4 = iT0[a4 >> 24] ^ RotByte (iT0[(byte)(a3 >> 16)] ^ RotByte (iT0[(byte)(a2 >> 8)] ^ RotByte (iT0[(byte)a1]))) ^ ekey[10];
			b5 = iT0[a5 >> 24] ^ RotByte (iT0[(byte)(a4 >> 16)] ^ RotByte (iT0[(byte)(a3 >> 8)] ^ RotByte (iT0[(byte)a2]))) ^ ekey[11];

			while (ei < end) {
				a0 = iT0[b0 >> 24] ^ RotByte (iT0[(byte)(b5 >> 16)] ^ RotByte (iT0[(byte)(b4 >> 8)] ^ RotByte (iT0[(byte)b3]))) ^ ekey[ei++];
				a1 = iT0[b1 >> 24] ^ RotByte (iT0[(byte)(b0 >> 16)] ^ RotByte (iT0[(byte)(b5 >> 8)] ^ RotByte (iT0[(byte)b4]))) ^ ekey[ei++];
				a2 = iT0[b2 >> 24] ^ RotByte (iT0[(byte)(b1 >> 16)] ^ RotByte (iT0[(byte)(b0 >> 8)] ^ RotByte (iT0[(byte)b5]))) ^ ekey[ei++];
				a3 = iT0[b3 >> 24] ^ RotByte (iT0[(byte)(b2 >> 16)] ^ RotByte (iT0[(byte)(b1 >> 8)] ^ RotByte (iT0[(byte)b0]))) ^ ekey[ei++];
				a4 = iT0[b4 >> 24] ^ RotByte (iT0[(byte)(b3 >> 16)] ^ RotByte (iT0[(byte)(b2 >> 8)] ^ RotByte (iT0[(byte)b1]))) ^ ekey[ei++];
				a5 = iT0[b5 >> 24] ^ RotByte (iT0[(byte)(b4 >> 16)] ^ RotByte (iT0[(byte)(b3 >> 8)] ^ RotByte (iT0[(byte)b2]))) ^ ekey[ei++];
				b0 = iT0[a0 >> 24] ^ RotByte (iT0[(byte)(a5 >> 16)] ^ RotByte (iT0[(byte)(a4 >> 8)] ^ RotByte (iT0[(byte)a3]))) ^ ekey[ei++];
				b1 = iT0[a1 >> 24] ^ RotByte (iT0[(byte)(a0 >> 16)] ^ RotByte (iT0[(byte)(a5 >> 8)] ^ RotByte (iT0[(byte)a4]))) ^ ekey[ei++];
				b2 = iT0[a2 >> 24] ^ RotByte (iT0[(byte)(a1 >> 16)] ^ RotByte (iT0[(byte)(a0 >> 8)] ^ RotByte (iT0[(byte)a5]))) ^ ekey[ei++];
				b3 = iT0[a3 >> 24] ^ RotByte (iT0[(byte)(a2 >> 16)] ^ RotByte (iT0[(byte)(a1 >> 8)] ^ RotByte (iT0[(byte)a0]))) ^ ekey[ei++];
				b4 = iT0[a4 >> 24] ^ RotByte (iT0[(byte)(a3 >> 16)] ^ RotByte (iT0[(byte)(a2 >> 8)] ^ RotByte (iT0[(byte)a1]))) ^ ekey[ei++];
				b5 = iT0[a5 >> 24] ^ RotByte (iT0[(byte)(a4 >> 16)] ^ RotByte (iT0[(byte)(a3 >> 8)] ^ RotByte (iT0[(byte)a2]))) ^ ekey[ei++];
			}

			/* Final Round */
			outdata[0] = (byte)(iSBox[b0 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[1] = (byte)(iSBox[(byte)(b5 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[2] = (byte)(iSBox[(byte)(b4 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[3] = (byte)(iSBox[(byte)b3] ^ (byte)ekey[ei++]);

			outdata[4] = (byte)(iSBox[b1 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[5] = (byte)(iSBox[(byte)(b0 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[6] = (byte)(iSBox[(byte)(b5 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[7] = (byte)(iSBox[(byte)b4] ^ (byte)ekey[ei++]);

			outdata[8] = (byte)(iSBox[b2 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[9] = (byte)(iSBox[(byte)(b1 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[10] = (byte)(iSBox[(byte)(b0 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[11] = (byte)(iSBox[(byte)b5] ^ (byte)ekey[ei++]);

			outdata[12] = (byte)(iSBox[b3 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[13] = (byte)(iSBox[(byte)(b2 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[14] = (byte)(iSBox[(byte)(b1 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[15] = (byte)(iSBox[(byte)b0] ^ (byte)ekey[ei++]);

			outdata[16] = (byte)(iSBox[b4 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[17] = (byte)(iSBox[(byte)(b3 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[18] = (byte)(iSBox[(byte)(b2 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[19] = (byte)(iSBox[(byte)b1] ^ (byte)ekey[ei++]);

			outdata[20] = (byte)(iSBox[b5 >> 24] ^ (byte)(ekey[ei] >> 24));
			outdata[21] = (byte)(iSBox[(byte)(b4 >> 16)] ^ (byte)(ekey[ei] >> 16));
			outdata[22] = (byte)(iSBox[(byte)(b3 >> 8)] ^ (byte)(ekey[ei] >> 8));
			outdata[23] = (byte)(iSBox[(byte)b2] ^ (byte)ekey[ei++]);
		}

		private unsafe void Decrypt256 (byte* indata, byte* outdata, uint[] ekey)
		{
			uint a0, a1, a2, a3, a4, a5, a6, a7, b0, b1, b2, b3, b4, b5, b6, b7;

			/* Round 0 */
			a0 = (((uint)indata[0] << 24) | ((uint)indata[1] << 16) | ((uint)indata[2] << 8) | (uint)indata[3]) ^ ekey[0];
			a1 = (((uint)indata[4] << 24) | ((uint)indata[5] << 16) | ((uint)indata[6] << 8) | (uint)indata[7]) ^ ekey[1];
			a2 = (((uint)indata[8] << 24) | ((uint)indata[9] << 16) | ((uint)indata[10] << 8) | (uint)indata[11]) ^ ekey[2];
			a3 = (((uint)indata[12] << 24) | ((uint)indata[13] << 16) | ((uint)indata[14] << 8) | (uint)indata[15]) ^ ekey[3];
			a4 = (((uint)indata[16] << 24) | ((uint)indata[17] << 16) | ((uint)indata[18] << 8) | (uint)indata[19]) ^ ekey[4];
			a5 = (((uint)indata[20] << 24) | ((uint)indata[21] << 16) | ((uint)indata[22] << 8) | (uint)indata[23]) ^ ekey[5];
			a6 = (((uint)indata[24] << 24) | ((uint)indata[25] << 16) | ((uint)indata[26] << 8) | (uint)indata[27]) ^ ekey[6];
			a7 = (((uint)indata[28] << 24) | ((uint)indata[29] << 16) | ((uint)indata[30] << 8) | (uint)indata[31]) ^ ekey[7];

			/* Round 1 */
			b0 = iT0[a0 >> 24] ^ RotByte (iT0[(byte)(a7 >> 16)] ^ RotByte (iT0[(byte)(a5 >> 8)] ^ RotByte (iT0[(byte)a4]))) ^ ekey[8];
			b1 = iT0[a1 >> 24] ^ RotByte (iT0[(byte)(a0 >> 16)] ^ RotByte (iT0[(byte)(a6 >> 8)] ^ RotByte (iT0[(byte)a5]))) ^ ekey[9];
			b2 = iT0[a2 >> 24] ^ RotByte (iT0[(byte)(a1 >> 16)] ^ RotByte (iT0[(byte)(a7 >> 8)] ^ RotByte (iT0[(byte)a6]))) ^ ekey[10];
			b3 = iT0[a3 >> 24] ^ RotByte (iT0[(byte)(a2 >> 16)] ^ RotByte (iT0[(byte)(a0 >> 8)] ^ RotByte (iT0[(byte)a7]))) ^ ekey[11];
			b4 = iT0[a4 >> 24] ^ RotByte (iT0[(byte)(a3 >> 16)] ^ RotByte (iT0[(byte)(a1 >> 8)] ^ RotByte (iT0[(byte)a0]))) ^ ekey[12];
			b5 = iT0[a5 >> 24] ^ RotByte (iT0[(byte)(a4 >> 16)] ^ RotByte (iT0[(byte)(a2 >> 8)] ^ RotByte (iT0[(byte)a1]))) ^ ekey[13];
			b6 = iT0[a6 >> 24] ^ RotByte (iT0[(byte)(a5 >> 16)] ^ RotByte (iT0[(byte)(a3 >> 8)] ^ RotByte (iT0[(byte)a2]))) ^ ekey[14];
			b7 = iT0[a7 >> 24] ^ RotByte (iT0[(byte)(a6 >> 16)] ^ RotByte (iT0[(byte)(a4 >> 8)] ^ RotByte (iT0[(byte)a3]))) ^ ekey[15];

			for (int ei = 16; ei < 112; ) {
				a0 = iT0[b0 >> 24] ^ RotByte (iT0[(byte)(b7 >> 16)] ^ RotByte (iT0[(byte)(b5 >> 8)] ^ RotByte (iT0[(byte)b4]))) ^ ekey[ei++];
				a1 = iT0[b1 >> 24] ^ RotByte (iT0[(byte)(b0 >> 16)] ^ RotByte (iT0[(byte)(b6 >> 8)] ^ RotByte (iT0[(byte)b5]))) ^ ekey[ei++];
				a2 = iT0[b2 >> 24] ^ RotByte (iT0[(byte)(b1 >> 16)] ^ RotByte (iT0[(byte)(b7 >> 8)] ^ RotByte (iT0[(byte)b6]))) ^ ekey[ei++];
				a3 = iT0[b3 >> 24] ^ RotByte (iT0[(byte)(b2 >> 16)] ^ RotByte (iT0[(byte)(b0 >> 8)] ^ RotByte (iT0[(byte)b7]))) ^ ekey[ei++];
				a4 = iT0[b4 >> 24] ^ RotByte (iT0[(byte)(b3 >> 16)] ^ RotByte (iT0[(byte)(b1 >> 8)] ^ RotByte (iT0[(byte)b0]))) ^ ekey[ei++];
				a5 = iT0[b5 >> 24] ^ RotByte (iT0[(byte)(b4 >> 16)] ^ RotByte (iT0[(byte)(b2 >> 8)] ^ RotByte (iT0[(byte)b1]))) ^ ekey[ei++];
				a6 = iT0[b6 >> 24] ^ RotByte (iT0[(byte)(b5 >> 16)] ^ RotByte (iT0[(byte)(b3 >> 8)] ^ RotByte (iT0[(byte)b2]))) ^ ekey[ei++];
				a7 = iT0[b7 >> 24] ^ RotByte (iT0[(byte)(b6 >> 16)] ^ RotByte (iT0[(byte)(b4 >> 8)] ^ RotByte (iT0[(byte)b3]))) ^ ekey[ei++];
				b0 = iT0[a0 >> 24] ^ RotByte (iT0[(byte)(a7 >> 16)] ^ RotByte (iT0[(byte)(a5 >> 8)] ^ RotByte (iT0[(byte)a4]))) ^ ekey[ei++];
				b1 = iT0[a1 >> 24] ^ RotByte (iT0[(byte)(a0 >> 16)] ^ RotByte (iT0[(byte)(a6 >> 8)] ^ RotByte (iT0[(byte)a5]))) ^ ekey[ei++];
				b2 = iT0[a2 >> 24] ^ RotByte (iT0[(byte)(a1 >> 16)] ^ RotByte (iT0[(byte)(a7 >> 8)] ^ RotByte (iT0[(byte)a6]))) ^ ekey[ei++];
				b3 = iT0[a3 >> 24] ^ RotByte (iT0[(byte)(a2 >> 16)] ^ RotByte (iT0[(byte)(a0 >> 8)] ^ RotByte (iT0[(byte)a7]))) ^ ekey[ei++];
				b4 = iT0[a4 >> 24] ^ RotByte (iT0[(byte)(a3 >> 16)] ^ RotByte (iT0[(byte)(a1 >> 8)] ^ RotByte (iT0[(byte)a0]))) ^ ekey[ei++];
				b5 = iT0[a5 >> 24] ^ RotByte (iT0[(byte)(a4 >> 16)] ^ RotByte (iT0[(byte)(a2 >> 8)] ^ RotByte (iT0[(byte)a1]))) ^ ekey[ei++];
				b6 = iT0[a6 >> 24] ^ RotByte (iT0[(byte)(a5 >> 16)] ^ RotByte (iT0[(byte)(a3 >> 8)] ^ RotByte (iT0[(byte)a2]))) ^ ekey[ei++];
				b7 = iT0[a7 >> 24] ^ RotByte (iT0[(byte)(a6 >> 16)] ^ RotByte (iT0[(byte)(a4 >> 8)] ^ RotByte (iT0[(byte)a3]))) ^ ekey[ei++];
			}

			/* Final Round */
			outdata[0] = (byte)(iSBox[b0 >> 24] ^ (byte)(ekey[112] >> 24));
			outdata[1] = (byte)(iSBox[(byte)(b7 >> 16)] ^ (byte)(ekey[112] >> 16));
			outdata[2] = (byte)(iSBox[(byte)(b5 >> 8)] ^ (byte)(ekey[112] >> 8));
			outdata[3] = (byte)(iSBox[(byte)b4] ^ (byte)ekey[112]);

			outdata[4] = (byte)(iSBox[b1 >> 24] ^ (byte)(ekey[113] >> 24));
			outdata[5] = (byte)(iSBox[(byte)(b0 >> 16)] ^ (byte)(ekey[113] >> 16));
			outdata[6] = (byte)(iSBox[(byte)(b6 >> 8)] ^ (byte)(ekey[113] >> 8));
			outdata[7] = (byte)(iSBox[(byte)b5] ^ (byte)ekey[113]);

			outdata[8] = (byte)(iSBox[b2 >> 24] ^ (byte)(ekey[114] >> 24));
			outdata[9] = (byte)(iSBox[(byte)(b1 >> 16)] ^ (byte)(ekey[114] >> 16));
			outdata[10] = (byte)(iSBox[(byte)(b7 >> 8)] ^ (byte)(ekey[114] >> 8));
			outdata[11] = (byte)(iSBox[(byte)b6] ^ (byte)ekey[114]);

			outdata[12] = (byte)(iSBox[b3 >> 24] ^ (byte)(ekey[115] >> 24));
			outdata[13] = (byte)(iSBox[(byte)(b2 >> 16)] ^ (byte)(ekey[115] >> 16));
			outdata[14] = (byte)(iSBox[(byte)(b0 >> 8)] ^ (byte)(ekey[115] >> 8));
			outdata[15] = (byte)(iSBox[(byte)b7] ^ (byte)ekey[115]);

			outdata[16] = (byte)(iSBox[b4 >> 24] ^ (byte)(ekey[116] >> 24));
			outdata[17] = (byte)(iSBox[(byte)(b3 >> 16)] ^ (byte)(ekey[116] >> 16));
			outdata[18] = (byte)(iSBox[(byte)(b1 >> 8)] ^ (byte)(ekey[116] >> 8));
			outdata[19] = (byte)(iSBox[(byte)b0] ^ (byte)ekey[116]);

			outdata[20] = (byte)(iSBox[b5 >> 24] ^ (byte)(ekey[117] >> 24));
			outdata[21] = (byte)(iSBox[(byte)(b4 >> 16)] ^ (byte)(ekey[117] >> 16));
			outdata[22] = (byte)(iSBox[(byte)(b2 >> 8)] ^ (byte)(ekey[117] >> 8));
			outdata[23] = (byte)(iSBox[(byte)b1] ^ (byte)ekey[117]);

			outdata[24] = (byte)(iSBox[b6 >> 24] ^ (byte)(ekey[118] >> 24));
			outdata[25] = (byte)(iSBox[(byte)(b5 >> 16)] ^ (byte)(ekey[118] >> 16));
			outdata[26] = (byte)(iSBox[(byte)(b3 >> 8)] ^ (byte)(ekey[118] >> 8));
			outdata[27] = (byte)(iSBox[(byte)b2] ^ (byte)ekey[118]);

			outdata[28] = (byte)(iSBox[b7 >> 24] ^ (byte)(ekey[119] >> 24));
			outdata[29] = (byte)(iSBox[(byte)(b6 >> 16)] ^ (byte)(ekey[119] >> 16));
			outdata[30] = (byte)(iSBox[(byte)(b4 >> 8)] ^ (byte)(ekey[119] >> 8));
			outdata[31] = (byte)(iSBox[(byte)b3] ^ (byte)ekey[119]);
		}
		#endregion
	}
}
