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

		#region Encrypt
		protected override unsafe void Encrypt128 (byte* indata, byte* outdata, uint[] ekey)
		{
			uint a0, a1, a2, a3, b0, b1, b2, b3;
			int ei = 40;

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

			/* Round 2 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[8];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[9];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[10];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[11];

			/* Round 3 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[12];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[13];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[14];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[15];

			/* Round 4 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[16];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[17];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[18];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[19];

			/* Round 5 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[20];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[21];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[22];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[23];

			/* Round 6 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[24];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[25];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[26];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[27];

			/* Round 7 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[28];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[29];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[30];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[31];

			/* Round 8 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[32];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[33];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[34];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[35];

			/* Round 9 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[36];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[37];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[38];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[39];

			if (_Nr > 10) {

				/* Round 10 */
				a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[40];
				a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[41];
				a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[42];
				a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[43];

				/* Round 11 */
				b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[44];
				b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[45];
				b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[46];
				b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[47];

				ei = 48;

				if (_Nr > 12) {

					/* Round 12 */
					a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[48];
					a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[49];
					a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[50];
					a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[51];

					/* Round 13 */
					b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[52];
					b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[53];
					b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[54];
					b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[55];

					ei = 56;
				}
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

		protected override unsafe void Encrypt192 (byte* indata, byte* outdata, uint[] ekey)
		{
			uint a0, a1, a2, a3, a4, a5, b0, b1, b2, b3, b4, b5;
			int ei = 72;

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

			/* Round 2 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[12];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[13];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[14];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[15];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[16];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[17];

			/* Round 3 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[18];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[19];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[20];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[21];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[22];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[23];

			/* Round 4 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[24];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[25];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[26];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[27];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[28];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[29];

			/* Round 5 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[30];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[31];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[32];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[33];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[34];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[35];

			/* Round 6 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[36];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[37];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[38];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[39];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[40];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[41];

			/* Round 7 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[42];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[43];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[44];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[45];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[46];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[47];

			/* Round 8 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[48];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[49];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[50];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[51];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[52];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[53];

			/* Round 9 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[54];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[55];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[56];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[57];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[58];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[59];

			/* Round 10 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[60];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[61];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[62];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[63];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[64];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[65];

			/* Round 11 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[66];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[67];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[68];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[69];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[70];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[71];

			if (_Nr > 12) {

				/* Round 12 */
				a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[72];
				a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[73];
				a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[74];
				a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[75];
				a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[76];
				a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[77];

				/* Round 13 */
				b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[78];
				b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[79];
				b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[80];
				b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[81];
				b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[82];
				b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[83];

				ei = 84;
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

		protected override unsafe void Encrypt256 (byte* indata, byte* outdata, uint[] ekey)
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

			/* Round 2 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[16];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[17];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b6]))) ^ ekey[18];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b6 >> 8)] ^ RotByte (T0[(byte)b7]))) ^ ekey[19];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b7 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[20];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b6 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[21];
			a6 = T0[b6 >> 24] ^ RotByte (T0[(byte)(b7 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[22];
			a7 = T0[b7 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[23];

			/* Round 3 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[24];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[25];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a6]))) ^ ekey[26];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a6 >> 8)] ^ RotByte (T0[(byte)a7]))) ^ ekey[27];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a7 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[28];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a6 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[29];
			b6 = T0[a6 >> 24] ^ RotByte (T0[(byte)(a7 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[30];
			b7 = T0[a7 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[31];

			/* Round 4 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[32];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[33];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b6]))) ^ ekey[34];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b6 >> 8)] ^ RotByte (T0[(byte)b7]))) ^ ekey[35];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b7 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[36];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b6 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[37];
			a6 = T0[b6 >> 24] ^ RotByte (T0[(byte)(b7 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[38];
			a7 = T0[b7 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[39];

			/* Round 5 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[40];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[41];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a6]))) ^ ekey[42];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a6 >> 8)] ^ RotByte (T0[(byte)a7]))) ^ ekey[43];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a7 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[44];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a6 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[45];
			b6 = T0[a6 >> 24] ^ RotByte (T0[(byte)(a7 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[46];
			b7 = T0[a7 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[47];

			/* Round 6 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[48];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[49];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b6]))) ^ ekey[50];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b6 >> 8)] ^ RotByte (T0[(byte)b7]))) ^ ekey[51];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b7 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[52];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b6 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[53];
			a6 = T0[b6 >> 24] ^ RotByte (T0[(byte)(b7 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[54];
			a7 = T0[b7 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[55];

			/* Round 7 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[56];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[57];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a6]))) ^ ekey[58];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a6 >> 8)] ^ RotByte (T0[(byte)a7]))) ^ ekey[59];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a7 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[60];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a6 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[61];
			b6 = T0[a6 >> 24] ^ RotByte (T0[(byte)(a7 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[62];
			b7 = T0[a7 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[63];

			/* Round 8 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[64];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[65];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b6]))) ^ ekey[66];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b6 >> 8)] ^ RotByte (T0[(byte)b7]))) ^ ekey[67];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b7 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[68];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b6 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[69];
			a6 = T0[b6 >> 24] ^ RotByte (T0[(byte)(b7 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[70];
			a7 = T0[b7 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[71];

			/* Round 9 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[72];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[73];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a6]))) ^ ekey[74];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a6 >> 8)] ^ RotByte (T0[(byte)a7]))) ^ ekey[75];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a7 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[76];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a6 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[77];
			b6 = T0[a6 >> 24] ^ RotByte (T0[(byte)(a7 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[78];
			b7 = T0[a7 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[79];

			/* Round 10 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[80];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[81];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b6]))) ^ ekey[82];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b6 >> 8)] ^ RotByte (T0[(byte)b7]))) ^ ekey[83];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b7 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[84];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b6 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[85];
			a6 = T0[b6 >> 24] ^ RotByte (T0[(byte)(b7 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[86];
			a7 = T0[b7 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[87];

			/* Round 11 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[88];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[89];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a6]))) ^ ekey[90];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a6 >> 8)] ^ RotByte (T0[(byte)a7]))) ^ ekey[91];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a7 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[92];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a6 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[93];
			b6 = T0[a6 >> 24] ^ RotByte (T0[(byte)(a7 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[94];
			b7 = T0[a7 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[95];

			/* Round 12 */
			a0 = T0[b0 >> 24] ^ RotByte (T0[(byte)(b1 >> 16)] ^ RotByte (T0[(byte)(b3 >> 8)] ^ RotByte (T0[(byte)b4]))) ^ ekey[96];
			a1 = T0[b1 >> 24] ^ RotByte (T0[(byte)(b2 >> 16)] ^ RotByte (T0[(byte)(b4 >> 8)] ^ RotByte (T0[(byte)b5]))) ^ ekey[97];
			a2 = T0[b2 >> 24] ^ RotByte (T0[(byte)(b3 >> 16)] ^ RotByte (T0[(byte)(b5 >> 8)] ^ RotByte (T0[(byte)b6]))) ^ ekey[98];
			a3 = T0[b3 >> 24] ^ RotByte (T0[(byte)(b4 >> 16)] ^ RotByte (T0[(byte)(b6 >> 8)] ^ RotByte (T0[(byte)b7]))) ^ ekey[99];
			a4 = T0[b4 >> 24] ^ RotByte (T0[(byte)(b5 >> 16)] ^ RotByte (T0[(byte)(b7 >> 8)] ^ RotByte (T0[(byte)b0]))) ^ ekey[100];
			a5 = T0[b5 >> 24] ^ RotByte (T0[(byte)(b6 >> 16)] ^ RotByte (T0[(byte)(b0 >> 8)] ^ RotByte (T0[(byte)b1]))) ^ ekey[101];
			a6 = T0[b6 >> 24] ^ RotByte (T0[(byte)(b7 >> 16)] ^ RotByte (T0[(byte)(b1 >> 8)] ^ RotByte (T0[(byte)b2]))) ^ ekey[102];
			a7 = T0[b7 >> 24] ^ RotByte (T0[(byte)(b0 >> 16)] ^ RotByte (T0[(byte)(b2 >> 8)] ^ RotByte (T0[(byte)b3]))) ^ ekey[103];

			/* Round 13 */
			b0 = T0[a0 >> 24] ^ RotByte (T0[(byte)(a1 >> 16)] ^ RotByte (T0[(byte)(a3 >> 8)] ^ RotByte (T0[(byte)a4]))) ^ ekey[104];
			b1 = T0[a1 >> 24] ^ RotByte (T0[(byte)(a2 >> 16)] ^ RotByte (T0[(byte)(a4 >> 8)] ^ RotByte (T0[(byte)a5]))) ^ ekey[105];
			b2 = T0[a2 >> 24] ^ RotByte (T0[(byte)(a3 >> 16)] ^ RotByte (T0[(byte)(a5 >> 8)] ^ RotByte (T0[(byte)a6]))) ^ ekey[106];
			b3 = T0[a3 >> 24] ^ RotByte (T0[(byte)(a4 >> 16)] ^ RotByte (T0[(byte)(a6 >> 8)] ^ RotByte (T0[(byte)a7]))) ^ ekey[107];
			b4 = T0[a4 >> 24] ^ RotByte (T0[(byte)(a5 >> 16)] ^ RotByte (T0[(byte)(a7 >> 8)] ^ RotByte (T0[(byte)a0]))) ^ ekey[108];
			b5 = T0[a5 >> 24] ^ RotByte (T0[(byte)(a6 >> 16)] ^ RotByte (T0[(byte)(a0 >> 8)] ^ RotByte (T0[(byte)a1]))) ^ ekey[109];
			b6 = T0[a6 >> 24] ^ RotByte (T0[(byte)(a7 >> 16)] ^ RotByte (T0[(byte)(a1 >> 8)] ^ RotByte (T0[(byte)a2]))) ^ ekey[110];
			b7 = T0[a7 >> 24] ^ RotByte (T0[(byte)(a0 >> 16)] ^ RotByte (T0[(byte)(a2 >> 8)] ^ RotByte (T0[(byte)a3]))) ^ ekey[111];

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
		protected override unsafe void Decrypt128 (byte* indata, byte* outdata, uint[] ekey)
		{
		}

		protected override unsafe void Decrypt192 (byte* indata, byte* outdata, uint[] ekey)
		{
		}

		protected override unsafe void Decrypt256 (byte* indata, byte* outdata, uint[] ekey)
		{
		}
		#endregion
	}
}
