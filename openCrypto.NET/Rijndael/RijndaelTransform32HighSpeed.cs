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
	class RijndaelTransform32HighSpeed : RijndaelTransform32
	{
		public RijndaelTransform32HighSpeed (RijndaelManaged algo, byte[] rgbKey, byte[] rgbIV, bool encryption)
			: base (algo, rgbKey, rgbIV, encryption)
		{
		}

		#region SymmetricTransform members

		protected override unsafe void EncryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			fixed (uint *pKey = _expandedKey, p0 = T0, p1 = T1, p2 = T2, p3 = T3)
			fixed (byte* pi = inputBuffer, po = outputBuffer) {
				switch (_Nb) {
					case 4:
						Encrypt128 (pKey, p0, p1, p2, p3, pi + inputOffset, po + outputOffset);
						break;
					case 6:
						Encrypt192 (pKey, p0, p1, p2, p3, pi + inputOffset, po + outputOffset);
						break;
					case 8:
						Encrypt256 (pKey, p0, p1, p2, p3, pi + inputOffset, po + outputOffset);
						break;
					default:
						throw new NotSupportedException ();
				}
			}
		}

		protected override unsafe void DecryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			fixed (uint* pKey = _expandedKey, p0 = iT0, p1 = iT1, p2 = iT2, p3 = iT3)
			fixed (byte* pi = inputBuffer, po = outputBuffer) {
				switch (_Nb) {
					case 4:
						Decrypt128 (pKey, p0, p1, p2, p3, pi + inputOffset, po + outputOffset);
						break;
					case 6:
						Decrypt192 (pKey, p0, p1, p2, p3, pi + inputOffset, po + outputOffset);
						break;
					case 8:
						Decrypt256 (pKey, p0, p1, p2, p3, pi + inputOffset, po + outputOffset);
						break;
					default:
						throw new NotSupportedException ();
				}
			}
		}

		#endregion

		#region Encrypt
		private unsafe void Encrypt128 (uint* ekey, uint* T0, uint* T1, uint* T2, uint* T3, byte* indata, byte* outdata)
		{
			uint a0, a1, a2, a3, b0, b1, b2, b3;

			/* Round 0 */
			a0 = (((uint)indata[0] << 24) | ((uint)indata[1] << 16) | ((uint)indata[2] << 8) | (uint)indata[3]) ^ ekey[0];
			a1 = (((uint)indata[4] << 24) | ((uint)indata[5] << 16) | ((uint)indata[6] << 8) | (uint)indata[7]) ^ ekey[1];
			a2 = (((uint)indata[8] << 24) | ((uint)indata[9] << 16) | ((uint)indata[10] << 8) | (uint)indata[11]) ^ ekey[2];
			a3 = (((uint)indata[12] << 24) | ((uint)indata[13] << 16) | ((uint)indata[14] << 8) | (uint)indata[15]) ^ ekey[3];

			/* Round 1 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[4];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a0] ^ ekey[5];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[6];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[7];

			/* Round 2 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[8];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b0] ^ ekey[9];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[10];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[11];

			/* Round 3 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[12];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a0] ^ ekey[13];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[14];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[15];

			/* Round 4 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[16];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b0] ^ ekey[17];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[18];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[19];

			/* Round 5 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[20];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a0] ^ ekey[21];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[22];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[23];

			/* Round 6 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[24];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b0] ^ ekey[25];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[26];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[27];

			/* Round 7 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[28];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a0] ^ ekey[29];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[30];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[31];

			/* Round 8 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[32];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b0] ^ ekey[33];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[34];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[35];

			/* Round 9 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[36];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a0] ^ ekey[37];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[38];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[39];

			ekey += 40;

			if (_Nr > 10) {

				/* Round 10 */
				a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[0];
				a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b0] ^ ekey[1];
				a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[2];
				a3 = T0[b3 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[3];

				/* Round 11 */
				b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[4];
				b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a0] ^ ekey[5];
				b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[6];
				b3 = T0[a3 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[7];

				ekey += 8;

				if (_Nr > 12) {

					/* Round 12 */
					a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[0];
					a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b0] ^ ekey[1];
					a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[2];
					a3 = T0[b3 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[3];

					/* Round 13 */
					b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[4];
					b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a0] ^ ekey[5];
					b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[6];
					b3 = T0[a3 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[7];

					ekey += 8;
				}
			}

			/* Final Round */
			uint tmp = (uint)(SBox[b0 >> 24] << 24) ^ (uint)(SBox[(byte)(b1 >> 16)] << 16)
				^ (uint)(SBox[(byte)(b2 >> 8)] << 8) ^ (uint)SBox[(byte)b3] ^ ekey[0];
			outdata[0] = (byte)(tmp >> 24);
			outdata[1] = (byte)(tmp >> 16);
			outdata[2] = (byte)(tmp >> 8);
			outdata[3] = (byte)(tmp);

			tmp = (uint)(SBox[b1 >> 24] << 24) ^ (uint)(SBox[(byte)(b2 >> 16)] << 16)
				^ (uint)(SBox[(byte)(b3 >> 8)] << 8) ^ (uint)SBox[(byte)b0] ^ ekey[1];
			outdata[4] = (byte)(tmp >> 24);
			outdata[5] = (byte)(tmp >> 16);
			outdata[6] = (byte)(tmp >> 8);
			outdata[7] = (byte)(tmp);

			tmp = (uint)(SBox[b2 >> 24] << 24) ^ (uint)(SBox[(byte)(b3 >> 16)] << 16)
				^ (uint)(SBox[(byte)(b0 >> 8)] << 8) ^ (uint)SBox[(byte)b1] ^ ekey[2];
			outdata[8] = (byte)(tmp >> 24);
			outdata[9] = (byte)(tmp >> 16);
			outdata[10] = (byte)(tmp >> 8);
			outdata[11] = (byte)(tmp);

			tmp = (uint)(SBox[b3 >> 24] << 24) ^ (uint)(SBox[(byte)(b0 >> 16)] << 16)
				^ (uint)(SBox[(byte)(b1 >> 8)] << 8) ^ (uint)SBox[(byte)b2] ^ ekey[3];
			outdata[12] = (byte)(tmp >> 24);
			outdata[13] = (byte)(tmp >> 16);
			outdata[14] = (byte)(tmp >> 8);
			outdata[15] = (byte)(tmp);
		}
		private unsafe void Encrypt192 (uint* ekey, uint* T0, uint* T1, uint* T2, uint* T3, byte* indata, byte* outdata)
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
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[6];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[7];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[8];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a0] ^ ekey[9];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[10];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[11];

			/* Round 2 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[12];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[13];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[14];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b0] ^ ekey[15];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[16];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[17];

			/* Round 3 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[18];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[19];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[20];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a0] ^ ekey[21];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[22];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[23];

			/* Round 4 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[24];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[25];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[26];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b0] ^ ekey[27];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[28];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[29];

			/* Round 5 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[30];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[31];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[32];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a0] ^ ekey[33];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[34];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[35];

			/* Round 6 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[36];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[37];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[38];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b0] ^ ekey[39];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[40];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[41];

			/* Round 7 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[42];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[43];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[44];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a0] ^ ekey[45];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[46];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[47];

			/* Round 8 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[48];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[49];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[50];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b0] ^ ekey[51];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[52];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[53];

			/* Round 9 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[54];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[55];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[56];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a0] ^ ekey[57];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[58];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[59];

			/* Round 10 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[60];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[61];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[62];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b0] ^ ekey[63];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[64];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[65];

			/* Round 11 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[66];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[67];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[68];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a0] ^ ekey[69];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[70];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[71];

			if (_Nr > 12) {

				/* Round 12 */
				a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[72];
				a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[73];
				a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[74];
				a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b0] ^ ekey[75];
				a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[76];
				a5 = T0[b5 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[77];

				/* Round 13 */
				b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[78];
				b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[79];
				b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[80];
				b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a0] ^ ekey[81];
				b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[82];
				b5 = T0[a5 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[83];

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
			outdata[23] = (byte)(SBox[(byte)b2] ^ (byte)ekey[ei]);
		}
		private unsafe void Encrypt256 (uint* ekey, uint* T0, uint* T1, uint* T2, uint* T3, byte* indata, byte* outdata)
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
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[8];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[9];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a6] ^ ekey[10];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a6 >> 8)] ^ T3[(byte)a7] ^ ekey[11];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a7 >> 8)] ^ T3[(byte)a0] ^ ekey[12];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a6 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[13];
			b6 = T0[a6 >> 24] ^ T1[(byte)(a7 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[14];
			b7 = T0[a7 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[15];

			/* Round 2 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[16];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[17];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b6] ^ ekey[18];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b6 >> 8)] ^ T3[(byte)b7] ^ ekey[19];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b7 >> 8)] ^ T3[(byte)b0] ^ ekey[20];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b6 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[21];
			a6 = T0[b6 >> 24] ^ T1[(byte)(b7 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[22];
			a7 = T0[b7 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[23];

			/* Round 3 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[24];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[25];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a6] ^ ekey[26];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a6 >> 8)] ^ T3[(byte)a7] ^ ekey[27];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a7 >> 8)] ^ T3[(byte)a0] ^ ekey[28];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a6 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[29];
			b6 = T0[a6 >> 24] ^ T1[(byte)(a7 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[30];
			b7 = T0[a7 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[31];

			/* Round 4 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[32];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[33];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b6] ^ ekey[34];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b6 >> 8)] ^ T3[(byte)b7] ^ ekey[35];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b7 >> 8)] ^ T3[(byte)b0] ^ ekey[36];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b6 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[37];
			a6 = T0[b6 >> 24] ^ T1[(byte)(b7 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[38];
			a7 = T0[b7 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[39];

			/* Round 5 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[40];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[41];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a6] ^ ekey[42];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a6 >> 8)] ^ T3[(byte)a7] ^ ekey[43];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a7 >> 8)] ^ T3[(byte)a0] ^ ekey[44];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a6 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[45];
			b6 = T0[a6 >> 24] ^ T1[(byte)(a7 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[46];
			b7 = T0[a7 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[47];

			/* Round 6 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[48];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[49];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b6] ^ ekey[50];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b6 >> 8)] ^ T3[(byte)b7] ^ ekey[51];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b7 >> 8)] ^ T3[(byte)b0] ^ ekey[52];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b6 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[53];
			a6 = T0[b6 >> 24] ^ T1[(byte)(b7 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[54];
			a7 = T0[b7 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[55];

			/* Round 7 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[56];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[57];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a6] ^ ekey[58];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a6 >> 8)] ^ T3[(byte)a7] ^ ekey[59];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a7 >> 8)] ^ T3[(byte)a0] ^ ekey[60];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a6 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[61];
			b6 = T0[a6 >> 24] ^ T1[(byte)(a7 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[62];
			b7 = T0[a7 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[63];

			/* Round 8 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[64];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[65];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b6] ^ ekey[66];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b6 >> 8)] ^ T3[(byte)b7] ^ ekey[67];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b7 >> 8)] ^ T3[(byte)b0] ^ ekey[68];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b6 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[69];
			a6 = T0[b6 >> 24] ^ T1[(byte)(b7 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[70];
			a7 = T0[b7 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[71];

			/* Round 9 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[72];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[73];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a6] ^ ekey[74];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a6 >> 8)] ^ T3[(byte)a7] ^ ekey[75];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a7 >> 8)] ^ T3[(byte)a0] ^ ekey[76];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a6 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[77];
			b6 = T0[a6 >> 24] ^ T1[(byte)(a7 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[78];
			b7 = T0[a7 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[79];

			/* Round 10 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[80];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[81];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b6] ^ ekey[82];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b6 >> 8)] ^ T3[(byte)b7] ^ ekey[83];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b7 >> 8)] ^ T3[(byte)b0] ^ ekey[84];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b6 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[85];
			a6 = T0[b6 >> 24] ^ T1[(byte)(b7 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[86];
			a7 = T0[b7 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[87];

			/* Round 11 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[88];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[89];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a6] ^ ekey[90];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a6 >> 8)] ^ T3[(byte)a7] ^ ekey[91];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a7 >> 8)] ^ T3[(byte)a0] ^ ekey[92];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a6 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[93];
			b6 = T0[a6 >> 24] ^ T1[(byte)(a7 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[94];
			b7 = T0[a7 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[95];

			/* Round 12 */
			a0 = T0[b0 >> 24] ^ T1[(byte)(b1 >> 16)] ^ T2[(byte)(b3 >> 8)] ^ T3[(byte)b4] ^ ekey[96];
			a1 = T0[b1 >> 24] ^ T1[(byte)(b2 >> 16)] ^ T2[(byte)(b4 >> 8)] ^ T3[(byte)b5] ^ ekey[97];
			a2 = T0[b2 >> 24] ^ T1[(byte)(b3 >> 16)] ^ T2[(byte)(b5 >> 8)] ^ T3[(byte)b6] ^ ekey[98];
			a3 = T0[b3 >> 24] ^ T1[(byte)(b4 >> 16)] ^ T2[(byte)(b6 >> 8)] ^ T3[(byte)b7] ^ ekey[99];
			a4 = T0[b4 >> 24] ^ T1[(byte)(b5 >> 16)] ^ T2[(byte)(b7 >> 8)] ^ T3[(byte)b0] ^ ekey[100];
			a5 = T0[b5 >> 24] ^ T1[(byte)(b6 >> 16)] ^ T2[(byte)(b0 >> 8)] ^ T3[(byte)b1] ^ ekey[101];
			a6 = T0[b6 >> 24] ^ T1[(byte)(b7 >> 16)] ^ T2[(byte)(b1 >> 8)] ^ T3[(byte)b2] ^ ekey[102];
			a7 = T0[b7 >> 24] ^ T1[(byte)(b0 >> 16)] ^ T2[(byte)(b2 >> 8)] ^ T3[(byte)b3] ^ ekey[103];

			/* Round 13 */
			b0 = T0[a0 >> 24] ^ T1[(byte)(a1 >> 16)] ^ T2[(byte)(a3 >> 8)] ^ T3[(byte)a4] ^ ekey[104];
			b1 = T0[a1 >> 24] ^ T1[(byte)(a2 >> 16)] ^ T2[(byte)(a4 >> 8)] ^ T3[(byte)a5] ^ ekey[105];
			b2 = T0[a2 >> 24] ^ T1[(byte)(a3 >> 16)] ^ T2[(byte)(a5 >> 8)] ^ T3[(byte)a6] ^ ekey[106];
			b3 = T0[a3 >> 24] ^ T1[(byte)(a4 >> 16)] ^ T2[(byte)(a6 >> 8)] ^ T3[(byte)a7] ^ ekey[107];
			b4 = T0[a4 >> 24] ^ T1[(byte)(a5 >> 16)] ^ T2[(byte)(a7 >> 8)] ^ T3[(byte)a0] ^ ekey[108];
			b5 = T0[a5 >> 24] ^ T1[(byte)(a6 >> 16)] ^ T2[(byte)(a0 >> 8)] ^ T3[(byte)a1] ^ ekey[109];
			b6 = T0[a6 >> 24] ^ T1[(byte)(a7 >> 16)] ^ T2[(byte)(a1 >> 8)] ^ T3[(byte)a2] ^ ekey[110];
			b7 = T0[a7 >> 24] ^ T1[(byte)(a0 >> 16)] ^ T2[(byte)(a2 >> 8)] ^ T3[(byte)a3] ^ ekey[111];

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
		private unsafe void Decrypt128 (uint* ekey, uint *iT0, uint *iT1, uint *iT2, uint *iT3, byte* indata, byte* outdata)
		{
			uint a0, a1, a2, a3, b0, b1, b2, b3;

			/* Round 0 */
			a0 = (((uint)indata[0] << 24) | ((uint)indata[1] << 16) | ((uint)indata[2] << 8) | (uint)indata[3]) ^ ekey[0];
			a1 = (((uint)indata[4] << 24) | ((uint)indata[5] << 16) | ((uint)indata[6] << 8) | (uint)indata[7]) ^ ekey[1];
			a2 = (((uint)indata[8] << 24) | ((uint)indata[9] << 16) | ((uint)indata[10] << 8) | (uint)indata[11]) ^ ekey[2];
			a3 = (((uint)indata[12] << 24) | ((uint)indata[13] << 16) | ((uint)indata[14] << 8) | (uint)indata[15]) ^ ekey[3];

			/* Round 1 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[4];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[5];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a3] ^ ekey[6];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[7];

			/* Round 2 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[8];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[9];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b3] ^ ekey[10];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[11];

			/* Round 3 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[12];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[13];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a3] ^ ekey[14];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[15];

			/* Round 4 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[16];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[17];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b3] ^ ekey[18];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[19];

			/* Round 5 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[20];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[21];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a3] ^ ekey[22];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[23];

			/* Round 6 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[24];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[25];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b3] ^ ekey[26];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[27];

			/* Round 7 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[28];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[29];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a3] ^ ekey[30];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[31];

			/* Round 8 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[32];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[33];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b3] ^ ekey[34];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[35];

			/* Round 9 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[36];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[37];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a3] ^ ekey[38];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[39];

			ekey += 40;

			if (_Nr > 10) {

				/* Round 10 */
				a0 = iT0[b0 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[0];
				a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[1];
				a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b3] ^ ekey[2];
				a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[3];

				/* Round 11 */
				b0 = iT0[a0 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[4];
				b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[5];
				b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a3] ^ ekey[6];
				b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[7];

				ekey += 8;

				if (_Nr > 12) {

					/* Round 12 */
					a0 = iT0[b0 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[0];
					a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[1];
					a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b3] ^ ekey[2];
					a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[3];

					/* Round 13 */
					b0 = iT0[a0 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[4];
					b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[5];
					b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a3] ^ ekey[6];
					b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[7];
					
					ekey += 8;
				}
			}

			/* Final Round */
			uint tmp = (uint)(iSBox[b0 >> 24] << 24) ^ (uint)(iSBox[(byte)(b3 >> 16)] << 16)
				^ (uint)(iSBox[(byte)(b2 >> 8)] << 8) ^ (uint)iSBox[(byte)b1] ^ ekey[0];
			outdata[0] = (byte)(tmp >> 24);
			outdata[1] = (byte)(tmp >> 16);
			outdata[2] = (byte)(tmp >> 8);
			outdata[3] = (byte)(tmp);

			tmp = (uint)(iSBox[b1 >> 24] << 24) ^ (uint)(iSBox[(byte)(b0 >> 16)] << 16)
				^ (uint)(iSBox[(byte)(b3 >> 8)] << 8) ^ (uint)iSBox[(byte)b2] ^ ekey[1];
			outdata[4] = (byte)(tmp >> 24);
			outdata[5] = (byte)(tmp >> 16);
			outdata[6] = (byte)(tmp >> 8);
			outdata[7] = (byte)(tmp);

			tmp = (uint)(iSBox[b2 >> 24] << 24) ^ (uint)(iSBox[(byte)(b1 >> 16)] << 16)
				^ (uint)(iSBox[(byte)(b0 >> 8)] << 8) ^ (uint)iSBox[(byte)b3] ^ ekey[2];
			outdata[8] = (byte)(tmp >> 24);
			outdata[9] = (byte)(tmp >> 16);
			outdata[10] = (byte)(tmp >> 8);
			outdata[11] = (byte)(tmp);

			tmp = (uint)(iSBox[b3 >> 24] << 24) ^ (uint)(iSBox[(byte)(b2 >> 16)] << 16)
				^ (uint)(iSBox[(byte)(b1 >> 8)] << 8) ^ (uint)iSBox[(byte)b0] ^ ekey[3];
			outdata[12] = (byte)(tmp >> 24);
			outdata[13] = (byte)(tmp >> 16);
			outdata[14] = (byte)(tmp >> 8);
			outdata[15] = (byte)(tmp);
		}
		private unsafe void Decrypt192 (uint* ekey, uint* iT0, uint* iT1, uint* iT2, uint* iT3, byte* indata, byte* outdata)
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
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[6];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[7];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a5] ^ ekey[8];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[9];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[10];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[11];

			/* Round 2 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[12];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[13];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b5] ^ ekey[14];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[15];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[16];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[17];

			/* Round 3 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[18];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[19];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a5] ^ ekey[20];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[21];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[22];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[23];

			/* Round 4 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[24];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[25];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b5] ^ ekey[26];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[27];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[28];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[29];

			/* Round 5 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[30];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[31];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a5] ^ ekey[32];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[33];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[34];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[35];

			/* Round 6 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[36];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[37];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b5] ^ ekey[38];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[39];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[40];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[41];

			/* Round 7 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[42];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[43];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a5] ^ ekey[44];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[45];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[46];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[47];

			/* Round 8 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[48];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[49];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b5] ^ ekey[50];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[51];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[52];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[53];

			/* Round 9 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[54];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[55];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a5] ^ ekey[56];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[57];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[58];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[59];

			/* Round 10 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[60];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[61];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b5] ^ ekey[62];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[63];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[64];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[65];

			/* Round 11 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[66];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[67];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a5] ^ ekey[68];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[69];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[70];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[71];

			if (_Nr > 12) {

				/* Round 12 */
				a0 = iT0[b0 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[72];
				a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[73];
				a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b5] ^ ekey[74];
				a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[75];
				a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[76];
				a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[77];

				/* Round 13 */
				b0 = iT0[a0 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[78];
				b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[79];
				b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a5] ^ ekey[80];
				b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[81];
				b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[82];
				b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[83];

				ei = 84;
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
			outdata[23] = (byte)(iSBox[(byte)b2] ^ (byte)ekey[ei]);
		}
		private unsafe void Decrypt256 (uint* ekey, uint* iT0, uint* iT1, uint* iT2, uint* iT3, byte* indata, byte* outdata)
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
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a7 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[8];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a6 >> 8)] ^ iT3[(byte)a5] ^ ekey[9];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a7 >> 8)] ^ iT3[(byte)a6] ^ ekey[10];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a7] ^ ekey[11];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[12];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[13];
			b6 = iT0[a6 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[14];
			b7 = iT0[a7 >> 24] ^ iT1[(byte)(a6 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[15];

			/* Round 2 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b7 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[16];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b6 >> 8)] ^ iT3[(byte)b5] ^ ekey[17];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b7 >> 8)] ^ iT3[(byte)b6] ^ ekey[18];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b7] ^ ekey[19];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[20];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[21];
			a6 = iT0[b6 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[22];
			a7 = iT0[b7 >> 24] ^ iT1[(byte)(b6 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[23];

			/* Round 3 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a7 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[24];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a6 >> 8)] ^ iT3[(byte)a5] ^ ekey[25];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a7 >> 8)] ^ iT3[(byte)a6] ^ ekey[26];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a7] ^ ekey[27];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[28];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[29];
			b6 = iT0[a6 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[30];
			b7 = iT0[a7 >> 24] ^ iT1[(byte)(a6 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[31];

			/* Round 4 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b7 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[32];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b6 >> 8)] ^ iT3[(byte)b5] ^ ekey[33];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b7 >> 8)] ^ iT3[(byte)b6] ^ ekey[34];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b7] ^ ekey[35];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[36];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[37];
			a6 = iT0[b6 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[38];
			a7 = iT0[b7 >> 24] ^ iT1[(byte)(b6 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[39];

			/* Round 5 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a7 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[40];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a6 >> 8)] ^ iT3[(byte)a5] ^ ekey[41];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a7 >> 8)] ^ iT3[(byte)a6] ^ ekey[42];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a7] ^ ekey[43];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[44];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[45];
			b6 = iT0[a6 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[46];
			b7 = iT0[a7 >> 24] ^ iT1[(byte)(a6 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[47];

			/* Round 6 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b7 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[48];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b6 >> 8)] ^ iT3[(byte)b5] ^ ekey[49];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b7 >> 8)] ^ iT3[(byte)b6] ^ ekey[50];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b7] ^ ekey[51];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[52];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[53];
			a6 = iT0[b6 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[54];
			a7 = iT0[b7 >> 24] ^ iT1[(byte)(b6 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[55];

			/* Round 7 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a7 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[56];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a6 >> 8)] ^ iT3[(byte)a5] ^ ekey[57];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a7 >> 8)] ^ iT3[(byte)a6] ^ ekey[58];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a7] ^ ekey[59];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[60];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[61];
			b6 = iT0[a6 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[62];
			b7 = iT0[a7 >> 24] ^ iT1[(byte)(a6 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[63];

			/* Round 8 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b7 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[64];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b6 >> 8)] ^ iT3[(byte)b5] ^ ekey[65];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b7 >> 8)] ^ iT3[(byte)b6] ^ ekey[66];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b7] ^ ekey[67];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[68];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[69];
			a6 = iT0[b6 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[70];
			a7 = iT0[b7 >> 24] ^ iT1[(byte)(b6 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[71];

			/* Round 9 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a7 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[72];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a6 >> 8)] ^ iT3[(byte)a5] ^ ekey[73];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a7 >> 8)] ^ iT3[(byte)a6] ^ ekey[74];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a7] ^ ekey[75];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[76];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[77];
			b6 = iT0[a6 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[78];
			b7 = iT0[a7 >> 24] ^ iT1[(byte)(a6 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[79];

			/* Round 10 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b7 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[80];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b6 >> 8)] ^ iT3[(byte)b5] ^ ekey[81];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b7 >> 8)] ^ iT3[(byte)b6] ^ ekey[82];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b7] ^ ekey[83];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[84];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[85];
			a6 = iT0[b6 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[86];
			a7 = iT0[b7 >> 24] ^ iT1[(byte)(b6 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[87];

			/* Round 11 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a7 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[88];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a6 >> 8)] ^ iT3[(byte)a5] ^ ekey[89];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a7 >> 8)] ^ iT3[(byte)a6] ^ ekey[90];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a7] ^ ekey[91];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[92];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[93];
			b6 = iT0[a6 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[94];
			b7 = iT0[a7 >> 24] ^ iT1[(byte)(a6 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[95];

			/* Round 12 */
			a0 = iT0[b0 >> 24] ^ iT1[(byte)(b7 >> 16)] ^ iT2[(byte)(b5 >> 8)] ^ iT3[(byte)b4] ^ ekey[96];
			a1 = iT0[b1 >> 24] ^ iT1[(byte)(b0 >> 16)] ^ iT2[(byte)(b6 >> 8)] ^ iT3[(byte)b5] ^ ekey[97];
			a2 = iT0[b2 >> 24] ^ iT1[(byte)(b1 >> 16)] ^ iT2[(byte)(b7 >> 8)] ^ iT3[(byte)b6] ^ ekey[98];
			a3 = iT0[b3 >> 24] ^ iT1[(byte)(b2 >> 16)] ^ iT2[(byte)(b0 >> 8)] ^ iT3[(byte)b7] ^ ekey[99];
			a4 = iT0[b4 >> 24] ^ iT1[(byte)(b3 >> 16)] ^ iT2[(byte)(b1 >> 8)] ^ iT3[(byte)b0] ^ ekey[100];
			a5 = iT0[b5 >> 24] ^ iT1[(byte)(b4 >> 16)] ^ iT2[(byte)(b2 >> 8)] ^ iT3[(byte)b1] ^ ekey[101];
			a6 = iT0[b6 >> 24] ^ iT1[(byte)(b5 >> 16)] ^ iT2[(byte)(b3 >> 8)] ^ iT3[(byte)b2] ^ ekey[102];
			a7 = iT0[b7 >> 24] ^ iT1[(byte)(b6 >> 16)] ^ iT2[(byte)(b4 >> 8)] ^ iT3[(byte)b3] ^ ekey[103];

			/* Round 13 */
			b0 = iT0[a0 >> 24] ^ iT1[(byte)(a7 >> 16)] ^ iT2[(byte)(a5 >> 8)] ^ iT3[(byte)a4] ^ ekey[104];
			b1 = iT0[a1 >> 24] ^ iT1[(byte)(a0 >> 16)] ^ iT2[(byte)(a6 >> 8)] ^ iT3[(byte)a5] ^ ekey[105];
			b2 = iT0[a2 >> 24] ^ iT1[(byte)(a1 >> 16)] ^ iT2[(byte)(a7 >> 8)] ^ iT3[(byte)a6] ^ ekey[106];
			b3 = iT0[a3 >> 24] ^ iT1[(byte)(a2 >> 16)] ^ iT2[(byte)(a0 >> 8)] ^ iT3[(byte)a7] ^ ekey[107];
			b4 = iT0[a4 >> 24] ^ iT1[(byte)(a3 >> 16)] ^ iT2[(byte)(a1 >> 8)] ^ iT3[(byte)a0] ^ ekey[108];
			b5 = iT0[a5 >> 24] ^ iT1[(byte)(a4 >> 16)] ^ iT2[(byte)(a2 >> 8)] ^ iT3[(byte)a1] ^ ekey[109];
			b6 = iT0[a6 >> 24] ^ iT1[(byte)(a5 >> 16)] ^ iT2[(byte)(a3 >> 8)] ^ iT3[(byte)a2] ^ ekey[110];
			b7 = iT0[a7 >> 24] ^ iT1[(byte)(a6 >> 16)] ^ iT2[(byte)(a4 >> 8)] ^ iT3[(byte)a3] ^ ekey[111];

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

		static readonly uint[] T1 = {
			0xa5c66363, 0x84f87c7c, 0x99ee7777, 0x8df67b7b, 0x0dfff2f2, 0xbdd66b6b, 0xb1de6f6f, 0x5491c5c5,
			0x50603030, 0x03020101, 0xa9ce6767, 0x7d562b2b, 0x19e7fefe, 0x62b5d7d7, 0xe64dabab, 0x9aec7676,
			0x458fcaca, 0x9d1f8282, 0x4089c9c9, 0x87fa7d7d, 0x15effafa, 0xebb25959, 0xc98e4747, 0x0bfbf0f0,
			0xec41adad, 0x67b3d4d4, 0xfd5fa2a2, 0xea45afaf, 0xbf239c9c, 0xf753a4a4, 0x96e47272, 0x5b9bc0c0,
			0xc275b7b7, 0x1ce1fdfd, 0xae3d9393, 0x6a4c2626, 0x5a6c3636, 0x417e3f3f, 0x02f5f7f7, 0x4f83cccc,
			0x5c683434, 0xf451a5a5, 0x34d1e5e5, 0x08f9f1f1, 0x93e27171, 0x73abd8d8, 0x53623131, 0x3f2a1515,
			0x0c080404, 0x5295c7c7, 0x65462323, 0x5e9dc3c3, 0x28301818, 0xa1379696, 0x0f0a0505, 0xb52f9a9a,
			0x090e0707, 0x36241212, 0x9b1b8080, 0x3ddfe2e2, 0x26cdebeb, 0x694e2727, 0xcd7fb2b2, 0x9fea7575,
			0x1b120909, 0x9e1d8383, 0x74582c2c, 0x2e341a1a, 0x2d361b1b, 0xb2dc6e6e, 0xeeb45a5a, 0xfb5ba0a0,
			0xf6a45252, 0x4d763b3b, 0x61b7d6d6, 0xce7db3b3, 0x7b522929, 0x3edde3e3, 0x715e2f2f, 0x97138484,
			0xf5a65353, 0x68b9d1d1, 0x00000000, 0x2cc1eded, 0x60402020, 0x1fe3fcfc, 0xc879b1b1, 0xedb65b5b,
			0xbed46a6a, 0x468dcbcb, 0xd967bebe, 0x4b723939, 0xde944a4a, 0xd4984c4c, 0xe8b05858, 0x4a85cfcf,
			0x6bbbd0d0, 0x2ac5efef, 0xe54faaaa, 0x16edfbfb, 0xc5864343, 0xd79a4d4d, 0x55663333, 0x94118585,
			0xcf8a4545, 0x10e9f9f9, 0x06040202, 0x81fe7f7f, 0xf0a05050, 0x44783c3c, 0xba259f9f, 0xe34ba8a8,
			0xf3a25151, 0xfe5da3a3, 0xc0804040, 0x8a058f8f, 0xad3f9292, 0xbc219d9d, 0x48703838, 0x04f1f5f5,
			0xdf63bcbc, 0xc177b6b6, 0x75afdada, 0x63422121, 0x30201010, 0x1ae5ffff, 0x0efdf3f3, 0x6dbfd2d2,
			0x4c81cdcd, 0x14180c0c, 0x35261313, 0x2fc3ecec, 0xe1be5f5f, 0xa2359797, 0xcc884444, 0x392e1717,
			0x5793c4c4, 0xf255a7a7, 0x82fc7e7e, 0x477a3d3d, 0xacc86464, 0xe7ba5d5d, 0x2b321919, 0x95e67373,
			0xa0c06060, 0x98198181, 0xd19e4f4f, 0x7fa3dcdc, 0x66442222, 0x7e542a2a, 0xab3b9090, 0x830b8888,
			0xca8c4646, 0x29c7eeee, 0xd36bb8b8, 0x3c281414, 0x79a7dede, 0xe2bc5e5e, 0x1d160b0b, 0x76addbdb,
			0x3bdbe0e0, 0x56643232, 0x4e743a3a, 0x1e140a0a, 0xdb924949, 0x0a0c0606, 0x6c482424, 0xe4b85c5c,
			0x5d9fc2c2, 0x6ebdd3d3, 0xef43acac, 0xa6c46262, 0xa8399191, 0xa4319595, 0x37d3e4e4, 0x8bf27979,
			0x32d5e7e7, 0x438bc8c8, 0x596e3737, 0xb7da6d6d, 0x8c018d8d, 0x64b1d5d5, 0xd29c4e4e, 0xe049a9a9,
			0xb4d86c6c, 0xfaac5656, 0x07f3f4f4, 0x25cfeaea, 0xafca6565, 0x8ef47a7a, 0xe947aeae, 0x18100808,
			0xd56fbaba, 0x88f07878, 0x6f4a2525, 0x725c2e2e, 0x24381c1c, 0xf157a6a6, 0xc773b4b4, 0x5197c6c6,
			0x23cbe8e8, 0x7ca1dddd, 0x9ce87474, 0x213e1f1f, 0xdd964b4b, 0xdc61bdbd, 0x860d8b8b, 0x850f8a8a,
			0x90e07070, 0x427c3e3e, 0xc471b5b5, 0xaacc6666, 0xd8904848, 0x05060303, 0x01f7f6f6, 0x121c0e0e,
			0xa3c26161, 0x5f6a3535, 0xf9ae5757, 0xd069b9b9, 0x91178686, 0x5899c1c1, 0x273a1d1d, 0xb9279e9e,
			0x38d9e1e1, 0x13ebf8f8, 0xb32b9898, 0x33221111, 0xbbd26969, 0x70a9d9d9, 0x89078e8e, 0xa7339494,
			0xb62d9b9b, 0x223c1e1e, 0x92158787, 0x20c9e9e9, 0x4987cece, 0xffaa5555, 0x78502828, 0x7aa5dfdf,
			0x8f038c8c, 0xf859a1a1, 0x80098989, 0x171a0d0d, 0xda65bfbf, 0x31d7e6e6, 0xc6844242, 0xb8d06868,
			0xc3824141, 0xb0299999, 0x775a2d2d, 0x111e0f0f, 0xcb7bb0b0, 0xfca85454, 0xd66dbbbb, 0x3a2c1616,
		};
		static readonly uint[] T2 = {
			0x63a5c663, 0x7c84f87c, 0x7799ee77, 0x7b8df67b, 0xf20dfff2, 0x6bbdd66b, 0x6fb1de6f, 0xc55491c5,
			0x30506030, 0x01030201, 0x67a9ce67, 0x2b7d562b, 0xfe19e7fe, 0xd762b5d7, 0xabe64dab, 0x769aec76,
			0xca458fca, 0x829d1f82, 0xc94089c9, 0x7d87fa7d, 0xfa15effa, 0x59ebb259, 0x47c98e47, 0xf00bfbf0,
			0xadec41ad, 0xd467b3d4, 0xa2fd5fa2, 0xafea45af, 0x9cbf239c, 0xa4f753a4, 0x7296e472, 0xc05b9bc0,
			0xb7c275b7, 0xfd1ce1fd, 0x93ae3d93, 0x266a4c26, 0x365a6c36, 0x3f417e3f, 0xf702f5f7, 0xcc4f83cc,
			0x345c6834, 0xa5f451a5, 0xe534d1e5, 0xf108f9f1, 0x7193e271, 0xd873abd8, 0x31536231, 0x153f2a15,
			0x040c0804, 0xc75295c7, 0x23654623, 0xc35e9dc3, 0x18283018, 0x96a13796, 0x050f0a05, 0x9ab52f9a,
			0x07090e07, 0x12362412, 0x809b1b80, 0xe23ddfe2, 0xeb26cdeb, 0x27694e27, 0xb2cd7fb2, 0x759fea75,
			0x091b1209, 0x839e1d83, 0x2c74582c, 0x1a2e341a, 0x1b2d361b, 0x6eb2dc6e, 0x5aeeb45a, 0xa0fb5ba0,
			0x52f6a452, 0x3b4d763b, 0xd661b7d6, 0xb3ce7db3, 0x297b5229, 0xe33edde3, 0x2f715e2f, 0x84971384,
			0x53f5a653, 0xd168b9d1, 0x00000000, 0xed2cc1ed, 0x20604020, 0xfc1fe3fc, 0xb1c879b1, 0x5bedb65b,
			0x6abed46a, 0xcb468dcb, 0xbed967be, 0x394b7239, 0x4ade944a, 0x4cd4984c, 0x58e8b058, 0xcf4a85cf,
			0xd06bbbd0, 0xef2ac5ef, 0xaae54faa, 0xfb16edfb, 0x43c58643, 0x4dd79a4d, 0x33556633, 0x85941185,
			0x45cf8a45, 0xf910e9f9, 0x02060402, 0x7f81fe7f, 0x50f0a050, 0x3c44783c, 0x9fba259f, 0xa8e34ba8,
			0x51f3a251, 0xa3fe5da3, 0x40c08040, 0x8f8a058f, 0x92ad3f92, 0x9dbc219d, 0x38487038, 0xf504f1f5,
			0xbcdf63bc, 0xb6c177b6, 0xda75afda, 0x21634221, 0x10302010, 0xff1ae5ff, 0xf30efdf3, 0xd26dbfd2,
			0xcd4c81cd, 0x0c14180c, 0x13352613, 0xec2fc3ec, 0x5fe1be5f, 0x97a23597, 0x44cc8844, 0x17392e17,
			0xc45793c4, 0xa7f255a7, 0x7e82fc7e, 0x3d477a3d, 0x64acc864, 0x5de7ba5d, 0x192b3219, 0x7395e673,
			0x60a0c060, 0x81981981, 0x4fd19e4f, 0xdc7fa3dc, 0x22664422, 0x2a7e542a, 0x90ab3b90, 0x88830b88,
			0x46ca8c46, 0xee29c7ee, 0xb8d36bb8, 0x143c2814, 0xde79a7de, 0x5ee2bc5e, 0x0b1d160b, 0xdb76addb,
			0xe03bdbe0, 0x32566432, 0x3a4e743a, 0x0a1e140a, 0x49db9249, 0x060a0c06, 0x246c4824, 0x5ce4b85c,
			0xc25d9fc2, 0xd36ebdd3, 0xacef43ac, 0x62a6c462, 0x91a83991, 0x95a43195, 0xe437d3e4, 0x798bf279,
			0xe732d5e7, 0xc8438bc8, 0x37596e37, 0x6db7da6d, 0x8d8c018d, 0xd564b1d5, 0x4ed29c4e, 0xa9e049a9,
			0x6cb4d86c, 0x56faac56, 0xf407f3f4, 0xea25cfea, 0x65afca65, 0x7a8ef47a, 0xaee947ae, 0x08181008,
			0xbad56fba, 0x7888f078, 0x256f4a25, 0x2e725c2e, 0x1c24381c, 0xa6f157a6, 0xb4c773b4, 0xc65197c6,
			0xe823cbe8, 0xdd7ca1dd, 0x749ce874, 0x1f213e1f, 0x4bdd964b, 0xbddc61bd, 0x8b860d8b, 0x8a850f8a,
			0x7090e070, 0x3e427c3e, 0xb5c471b5, 0x66aacc66, 0x48d89048, 0x03050603, 0xf601f7f6, 0x0e121c0e,
			0x61a3c261, 0x355f6a35, 0x57f9ae57, 0xb9d069b9, 0x86911786, 0xc15899c1, 0x1d273a1d, 0x9eb9279e,
			0xe138d9e1, 0xf813ebf8, 0x98b32b98, 0x11332211, 0x69bbd269, 0xd970a9d9, 0x8e89078e, 0x94a73394,
			0x9bb62d9b, 0x1e223c1e, 0x87921587, 0xe920c9e9, 0xce4987ce, 0x55ffaa55, 0x28785028, 0xdf7aa5df,
			0x8c8f038c, 0xa1f859a1, 0x89800989, 0x0d171a0d, 0xbfda65bf, 0xe631d7e6, 0x42c68442, 0x68b8d068,
			0x41c38241, 0x99b02999, 0x2d775a2d, 0x0f111e0f, 0xb0cb7bb0, 0x54fca854, 0xbbd66dbb, 0x163a2c16,
		};
		static readonly uint[] T3 = {
			0x6363a5c6, 0x7c7c84f8, 0x777799ee, 0x7b7b8df6, 0xf2f20dff, 0x6b6bbdd6, 0x6f6fb1de, 0xc5c55491,
			0x30305060, 0x01010302, 0x6767a9ce, 0x2b2b7d56, 0xfefe19e7, 0xd7d762b5, 0xababe64d, 0x76769aec,
			0xcaca458f, 0x82829d1f, 0xc9c94089, 0x7d7d87fa, 0xfafa15ef, 0x5959ebb2, 0x4747c98e, 0xf0f00bfb,
			0xadadec41, 0xd4d467b3, 0xa2a2fd5f, 0xafafea45, 0x9c9cbf23, 0xa4a4f753, 0x727296e4, 0xc0c05b9b,
			0xb7b7c275, 0xfdfd1ce1, 0x9393ae3d, 0x26266a4c, 0x36365a6c, 0x3f3f417e, 0xf7f702f5, 0xcccc4f83,
			0x34345c68, 0xa5a5f451, 0xe5e534d1, 0xf1f108f9, 0x717193e2, 0xd8d873ab, 0x31315362, 0x15153f2a,
			0x04040c08, 0xc7c75295, 0x23236546, 0xc3c35e9d, 0x18182830, 0x9696a137, 0x05050f0a, 0x9a9ab52f,
			0x0707090e, 0x12123624, 0x80809b1b, 0xe2e23ddf, 0xebeb26cd, 0x2727694e, 0xb2b2cd7f, 0x75759fea,
			0x09091b12, 0x83839e1d, 0x2c2c7458, 0x1a1a2e34, 0x1b1b2d36, 0x6e6eb2dc, 0x5a5aeeb4, 0xa0a0fb5b,
			0x5252f6a4, 0x3b3b4d76, 0xd6d661b7, 0xb3b3ce7d, 0x29297b52, 0xe3e33edd, 0x2f2f715e, 0x84849713,
			0x5353f5a6, 0xd1d168b9, 0x00000000, 0xeded2cc1, 0x20206040, 0xfcfc1fe3, 0xb1b1c879, 0x5b5bedb6,
			0x6a6abed4, 0xcbcb468d, 0xbebed967, 0x39394b72, 0x4a4ade94, 0x4c4cd498, 0x5858e8b0, 0xcfcf4a85,
			0xd0d06bbb, 0xefef2ac5, 0xaaaae54f, 0xfbfb16ed, 0x4343c586, 0x4d4dd79a, 0x33335566, 0x85859411,
			0x4545cf8a, 0xf9f910e9, 0x02020604, 0x7f7f81fe, 0x5050f0a0, 0x3c3c4478, 0x9f9fba25, 0xa8a8e34b,
			0x5151f3a2, 0xa3a3fe5d, 0x4040c080, 0x8f8f8a05, 0x9292ad3f, 0x9d9dbc21, 0x38384870, 0xf5f504f1,
			0xbcbcdf63, 0xb6b6c177, 0xdada75af, 0x21216342, 0x10103020, 0xffff1ae5, 0xf3f30efd, 0xd2d26dbf,
			0xcdcd4c81, 0x0c0c1418, 0x13133526, 0xecec2fc3, 0x5f5fe1be, 0x9797a235, 0x4444cc88, 0x1717392e,
			0xc4c45793, 0xa7a7f255, 0x7e7e82fc, 0x3d3d477a, 0x6464acc8, 0x5d5de7ba, 0x19192b32, 0x737395e6,
			0x6060a0c0, 0x81819819, 0x4f4fd19e, 0xdcdc7fa3, 0x22226644, 0x2a2a7e54, 0x9090ab3b, 0x8888830b,
			0x4646ca8c, 0xeeee29c7, 0xb8b8d36b, 0x14143c28, 0xdede79a7, 0x5e5ee2bc, 0x0b0b1d16, 0xdbdb76ad,
			0xe0e03bdb, 0x32325664, 0x3a3a4e74, 0x0a0a1e14, 0x4949db92, 0x06060a0c, 0x24246c48, 0x5c5ce4b8,
			0xc2c25d9f, 0xd3d36ebd, 0xacacef43, 0x6262a6c4, 0x9191a839, 0x9595a431, 0xe4e437d3, 0x79798bf2,
			0xe7e732d5, 0xc8c8438b, 0x3737596e, 0x6d6db7da, 0x8d8d8c01, 0xd5d564b1, 0x4e4ed29c, 0xa9a9e049,
			0x6c6cb4d8, 0x5656faac, 0xf4f407f3, 0xeaea25cf, 0x6565afca, 0x7a7a8ef4, 0xaeaee947, 0x08081810,
			0xbabad56f, 0x787888f0, 0x25256f4a, 0x2e2e725c, 0x1c1c2438, 0xa6a6f157, 0xb4b4c773, 0xc6c65197,
			0xe8e823cb, 0xdddd7ca1, 0x74749ce8, 0x1f1f213e, 0x4b4bdd96, 0xbdbddc61, 0x8b8b860d, 0x8a8a850f,
			0x707090e0, 0x3e3e427c, 0xb5b5c471, 0x6666aacc, 0x4848d890, 0x03030506, 0xf6f601f7, 0x0e0e121c,
			0x6161a3c2, 0x35355f6a, 0x5757f9ae, 0xb9b9d069, 0x86869117, 0xc1c15899, 0x1d1d273a, 0x9e9eb927,
			0xe1e138d9, 0xf8f813eb, 0x9898b32b, 0x11113322, 0x6969bbd2, 0xd9d970a9, 0x8e8e8907, 0x9494a733,
			0x9b9bb62d, 0x1e1e223c, 0x87879215, 0xe9e920c9, 0xcece4987, 0x5555ffaa, 0x28287850, 0xdfdf7aa5,
			0x8c8c8f03, 0xa1a1f859, 0x89898009, 0x0d0d171a, 0xbfbfda65, 0xe6e631d7, 0x4242c684, 0x6868b8d0,
			0x4141c382, 0x9999b029, 0x2d2d775a, 0x0f0f111e, 0xb0b0cb7b, 0x5454fca8, 0xbbbbd66d, 0x16163a2c,
		};
		static readonly uint[] iT1 = {
			0x5051f4a7, 0x537e4165, 0xc31a17a4, 0x963a275e, 0xcb3bab6b, 0xf11f9d45, 0xabacfa58, 0x934be303,
			0x552030fa, 0xf6ad766d, 0x9188cc76, 0x25f5024c, 0xfc4fe5d7, 0xd7c52acb, 0x80263544, 0x8fb562a3,
			0x49deb15a, 0x6725ba1b, 0x9845ea0e, 0xe15dfec0, 0x02c32f75, 0x12814cf0, 0xa38d4697, 0xc66bd3f9,
			0xe7038f5f, 0x9515929c, 0xebbf6d7a, 0xda955259, 0x2dd4be83, 0xd3587421, 0x2949e069, 0x448ec9c8,
			0x6a75c289, 0x78f48e79, 0x6b99583e, 0xdd27b971, 0xb6bee14f, 0x17f088ad, 0x66c920ac, 0xb47dce3a,
			0x1863df4a, 0x82e51a31, 0x60975133, 0x4562537f, 0xe0b16477, 0x84bb6bae, 0x1cfe81a0, 0x94f9082b,
			0x58704868, 0x198f45fd, 0x8794de6c, 0xb7527bf8, 0x23ab73d3, 0xe2724b02, 0x57e31f8f, 0x2a6655ab,
			0x07b2eb28, 0x032fb5c2, 0x9a86c57b, 0xa5d33708, 0xf2302887, 0xb223bfa5, 0xba02036a, 0x5ced1682,
			0x2b8acf1c, 0x92a779b4, 0xf0f307f2, 0xa14e69e2, 0xcd65daf4, 0xd50605be, 0x1fd13462, 0x8ac4a6fe,
			0x9d342e53, 0xa0a2f355, 0x32058ae1, 0x75a4f6eb, 0x390b83ec, 0xaa4060ef, 0x065e719f, 0x51bd6e10,
			0xf93e218a, 0x3d96dd06, 0xaedd3e05, 0x464de6bd, 0xb591548d, 0x0571c45d, 0x6f0406d4, 0xff605015,
			0x241998fb, 0x97d6bde9, 0xcc894043, 0x7767d99e, 0xbdb0e842, 0x8807898b, 0x38e7195b, 0xdb79c8ee,
			0x47a17c0a, 0xe97c420f, 0xc9f8841e, 0x00000000, 0x83098086, 0x48322bed, 0xac1e1170, 0x4e6c5a72,
			0xfbfd0eff, 0x560f8538, 0x1e3daed5, 0x27362d39, 0x640a0fd9, 0x21685ca6, 0xd19b5b54, 0x3a24362e,
			0xb10c0a67, 0x0f9357e7, 0xd2b4ee96, 0x9e1b9b91, 0x4f80c0c5, 0xa261dc20, 0x695a774b, 0x161c121a,
			0x0ae293ba, 0xe5c0a02a, 0x433c22e0, 0x1d121b17, 0x0b0e090d, 0xadf28bc7, 0xb92db6a8, 0xc8141ea9,
			0x8557f119, 0x4caf7507, 0xbbee99dd, 0xfda37f60, 0x9ff70126, 0xbc5c72f5, 0xc544663b, 0x345bfb7e,
			0x768b4329, 0xdccb23c6, 0x68b6edfc, 0x63b8e4f1, 0xcad731dc, 0x10426385, 0x40139722, 0x2084c611,
			0x7d854a24, 0xf8d2bb3d, 0x11aef932, 0x6dc729a1, 0x4b1d9e2f, 0xf3dcb230, 0xec0d8652, 0xd077c1e3,
			0x6c2bb316, 0x99a970b9, 0xfa119448, 0x2247e964, 0xc4a8fc8c, 0x1aa0f03f, 0xd8567d2c, 0xef223390,
			0xc787494e, 0xc1d938d1, 0xfe8ccaa2, 0x3698d40b, 0xcfa6f581, 0x28a57ade, 0x26dab78e, 0xa43fadbf,
			0xe42c3a9d, 0x0d507892, 0x9b6a5fcc, 0x62547e46, 0xc2f68d13, 0xe890d8b8, 0x5e2e39f7, 0xf582c3af,
			0xbe9f5d80, 0x7c69d093, 0xa96fd52d, 0xb3cf2512, 0x3bc8ac99, 0xa710187d, 0x6ee89c63, 0x7bdb3bbb,
			0x09cd2678, 0xf46e5918, 0x01ec9ab7, 0xa8834f9a, 0x65e6956e, 0x7eaaffe6, 0x0821bccf, 0xe6ef15e8,
			0xd9bae79b, 0xce4a6f36, 0xd4ea9f09, 0xd629b07c, 0xaf31a4b2, 0x312a3f23, 0x30c6a594, 0xc035a266,
			0x37744ebc, 0xa6fc82ca, 0xb0e090d0, 0x1533a7d8, 0x4af10498, 0xf741ecda, 0x0e7fcd50, 0x2f1791f6,
			0x8d764dd6, 0x4d43efb0, 0x54ccaa4d, 0xdfe49604, 0xe39ed1b5, 0x1b4c6a88, 0xb8c12c1f, 0x7f466551,
			0x049d5eea, 0x5d018c35, 0x73fa8774, 0x2efb0b41, 0x5ab3671d, 0x5292dbd2, 0x33e91056, 0x136dd647,
			0x8c9ad761, 0x7a37a10c, 0x8e59f814, 0x89eb133c, 0xeecea927, 0x35b761c9, 0xede11ce5, 0x3c7a47b1,
			0x599cd2df, 0x3f55f273, 0x791814ce, 0xbf73c737, 0xea53f7cd, 0x5b5ffdaa, 0x14df3d6f, 0x867844db,
			0x81caaff3, 0x3eb968c4, 0x2c382434, 0x5fc2a340, 0x72161dc3, 0x0cbce225, 0x8b283c49, 0x41ff0d95,
			0x7139a801, 0xde080cb3, 0x9cd8b4e4, 0x906456c1, 0x617bcb84, 0x70d532b6, 0x74486c5c, 0x42d0b857,
		};
		static readonly uint[] iT2 = {
			0xa75051f4, 0x65537e41, 0xa4c31a17, 0x5e963a27, 0x6bcb3bab, 0x45f11f9d, 0x58abacfa, 0x03934be3,
			0xfa552030, 0x6df6ad76, 0x769188cc, 0x4c25f502, 0xd7fc4fe5, 0xcbd7c52a, 0x44802635, 0xa38fb562,
			0x5a49deb1, 0x1b6725ba, 0x0e9845ea, 0xc0e15dfe, 0x7502c32f, 0xf012814c, 0x97a38d46, 0xf9c66bd3,
			0x5fe7038f, 0x9c951592, 0x7aebbf6d, 0x59da9552, 0x832dd4be, 0x21d35874, 0x692949e0, 0xc8448ec9,
			0x896a75c2, 0x7978f48e, 0x3e6b9958, 0x71dd27b9, 0x4fb6bee1, 0xad17f088, 0xac66c920, 0x3ab47dce,
			0x4a1863df, 0x3182e51a, 0x33609751, 0x7f456253, 0x77e0b164, 0xae84bb6b, 0xa01cfe81, 0x2b94f908,
			0x68587048, 0xfd198f45, 0x6c8794de, 0xf8b7527b, 0xd323ab73, 0x02e2724b, 0x8f57e31f, 0xab2a6655,
			0x2807b2eb, 0xc2032fb5, 0x7b9a86c5, 0x08a5d337, 0x87f23028, 0xa5b223bf, 0x6aba0203, 0x825ced16,
			0x1c2b8acf, 0xb492a779, 0xf2f0f307, 0xe2a14e69, 0xf4cd65da, 0xbed50605, 0x621fd134, 0xfe8ac4a6,
			0x539d342e, 0x55a0a2f3, 0xe132058a, 0xeb75a4f6, 0xec390b83, 0xefaa4060, 0x9f065e71, 0x1051bd6e,
			0x8af93e21, 0x063d96dd, 0x05aedd3e, 0xbd464de6, 0x8db59154, 0x5d0571c4, 0xd46f0406, 0x15ff6050,
			0xfb241998, 0xe997d6bd, 0x43cc8940, 0x9e7767d9, 0x42bdb0e8, 0x8b880789, 0x5b38e719, 0xeedb79c8,
			0x0a47a17c, 0x0fe97c42, 0x1ec9f884, 0x00000000, 0x86830980, 0xed48322b, 0x70ac1e11, 0x724e6c5a,
			0xfffbfd0e, 0x38560f85, 0xd51e3dae, 0x3927362d, 0xd9640a0f, 0xa621685c, 0x54d19b5b, 0x2e3a2436,
			0x67b10c0a, 0xe70f9357, 0x96d2b4ee, 0x919e1b9b, 0xc54f80c0, 0x20a261dc, 0x4b695a77, 0x1a161c12,
			0xba0ae293, 0x2ae5c0a0, 0xe0433c22, 0x171d121b, 0x0d0b0e09, 0xc7adf28b, 0xa8b92db6, 0xa9c8141e,
			0x198557f1, 0x074caf75, 0xddbbee99, 0x60fda37f, 0x269ff701, 0xf5bc5c72, 0x3bc54466, 0x7e345bfb,
			0x29768b43, 0xc6dccb23, 0xfc68b6ed, 0xf163b8e4, 0xdccad731, 0x85104263, 0x22401397, 0x112084c6,
			0x247d854a, 0x3df8d2bb, 0x3211aef9, 0xa16dc729, 0x2f4b1d9e, 0x30f3dcb2, 0x52ec0d86, 0xe3d077c1,
			0x166c2bb3, 0xb999a970, 0x48fa1194, 0x642247e9, 0x8cc4a8fc, 0x3f1aa0f0, 0x2cd8567d, 0x90ef2233,
			0x4ec78749, 0xd1c1d938, 0xa2fe8cca, 0x0b3698d4, 0x81cfa6f5, 0xde28a57a, 0x8e26dab7, 0xbfa43fad,
			0x9de42c3a, 0x920d5078, 0xcc9b6a5f, 0x4662547e, 0x13c2f68d, 0xb8e890d8, 0xf75e2e39, 0xaff582c3,
			0x80be9f5d, 0x937c69d0, 0x2da96fd5, 0x12b3cf25, 0x993bc8ac, 0x7da71018, 0x636ee89c, 0xbb7bdb3b,
			0x7809cd26, 0x18f46e59, 0xb701ec9a, 0x9aa8834f, 0x6e65e695, 0xe67eaaff, 0xcf0821bc, 0xe8e6ef15,
			0x9bd9bae7, 0x36ce4a6f, 0x09d4ea9f, 0x7cd629b0, 0xb2af31a4, 0x23312a3f, 0x9430c6a5, 0x66c035a2,
			0xbc37744e, 0xcaa6fc82, 0xd0b0e090, 0xd81533a7, 0x984af104, 0xdaf741ec, 0x500e7fcd, 0xf62f1791,
			0xd68d764d, 0xb04d43ef, 0x4d54ccaa, 0x04dfe496, 0xb5e39ed1, 0x881b4c6a, 0x1fb8c12c, 0x517f4665,
			0xea049d5e, 0x355d018c, 0x7473fa87, 0x412efb0b, 0x1d5ab367, 0xd25292db, 0x5633e910, 0x47136dd6,
			0x618c9ad7, 0x0c7a37a1, 0x148e59f8, 0x3c89eb13, 0x27eecea9, 0xc935b761, 0xe5ede11c, 0xb13c7a47,
			0xdf599cd2, 0x733f55f2, 0xce791814, 0x37bf73c7, 0xcdea53f7, 0xaa5b5ffd, 0x6f14df3d, 0xdb867844,
			0xf381caaf, 0xc43eb968, 0x342c3824, 0x405fc2a3, 0xc372161d, 0x250cbce2, 0x498b283c, 0x9541ff0d,
			0x017139a8, 0xb3de080c, 0xe49cd8b4, 0xc1906456, 0x84617bcb, 0xb670d532, 0x5c74486c, 0x5742d0b8,
		};
		static readonly uint[] iT3 = {
			0xf4a75051, 0x4165537e, 0x17a4c31a, 0x275e963a, 0xab6bcb3b, 0x9d45f11f, 0xfa58abac, 0xe303934b,
			0x30fa5520, 0x766df6ad, 0xcc769188, 0x024c25f5, 0xe5d7fc4f, 0x2acbd7c5, 0x35448026, 0x62a38fb5,
			0xb15a49de, 0xba1b6725, 0xea0e9845, 0xfec0e15d, 0x2f7502c3, 0x4cf01281, 0x4697a38d, 0xd3f9c66b,
			0x8f5fe703, 0x929c9515, 0x6d7aebbf, 0x5259da95, 0xbe832dd4, 0x7421d358, 0xe0692949, 0xc9c8448e,
			0xc2896a75, 0x8e7978f4, 0x583e6b99, 0xb971dd27, 0xe14fb6be, 0x88ad17f0, 0x20ac66c9, 0xce3ab47d,
			0xdf4a1863, 0x1a3182e5, 0x51336097, 0x537f4562, 0x6477e0b1, 0x6bae84bb, 0x81a01cfe, 0x082b94f9,
			0x48685870, 0x45fd198f, 0xde6c8794, 0x7bf8b752, 0x73d323ab, 0x4b02e272, 0x1f8f57e3, 0x55ab2a66,
			0xeb2807b2, 0xb5c2032f, 0xc57b9a86, 0x3708a5d3, 0x2887f230, 0xbfa5b223, 0x036aba02, 0x16825ced,
			0xcf1c2b8a, 0x79b492a7, 0x07f2f0f3, 0x69e2a14e, 0xdaf4cd65, 0x05bed506, 0x34621fd1, 0xa6fe8ac4,
			0x2e539d34, 0xf355a0a2, 0x8ae13205, 0xf6eb75a4, 0x83ec390b, 0x60efaa40, 0x719f065e, 0x6e1051bd,
			0x218af93e, 0xdd063d96, 0x3e05aedd, 0xe6bd464d, 0x548db591, 0xc45d0571, 0x06d46f04, 0x5015ff60,
			0x98fb2419, 0xbde997d6, 0x4043cc89, 0xd99e7767, 0xe842bdb0, 0x898b8807, 0x195b38e7, 0xc8eedb79,
			0x7c0a47a1, 0x420fe97c, 0x841ec9f8, 0x00000000, 0x80868309, 0x2bed4832, 0x1170ac1e, 0x5a724e6c,
			0x0efffbfd, 0x8538560f, 0xaed51e3d, 0x2d392736, 0x0fd9640a, 0x5ca62168, 0x5b54d19b, 0x362e3a24,
			0x0a67b10c, 0x57e70f93, 0xee96d2b4, 0x9b919e1b, 0xc0c54f80, 0xdc20a261, 0x774b695a, 0x121a161c,
			0x93ba0ae2, 0xa02ae5c0, 0x22e0433c, 0x1b171d12, 0x090d0b0e, 0x8bc7adf2, 0xb6a8b92d, 0x1ea9c814,
			0xf1198557, 0x75074caf, 0x99ddbbee, 0x7f60fda3, 0x01269ff7, 0x72f5bc5c, 0x663bc544, 0xfb7e345b,
			0x4329768b, 0x23c6dccb, 0xedfc68b6, 0xe4f163b8, 0x31dccad7, 0x63851042, 0x97224013, 0xc6112084,
			0x4a247d85, 0xbb3df8d2, 0xf93211ae, 0x29a16dc7, 0x9e2f4b1d, 0xb230f3dc, 0x8652ec0d, 0xc1e3d077,
			0xb3166c2b, 0x70b999a9, 0x9448fa11, 0xe9642247, 0xfc8cc4a8, 0xf03f1aa0, 0x7d2cd856, 0x3390ef22,
			0x494ec787, 0x38d1c1d9, 0xcaa2fe8c, 0xd40b3698, 0xf581cfa6, 0x7ade28a5, 0xb78e26da, 0xadbfa43f,
			0x3a9de42c, 0x78920d50, 0x5fcc9b6a, 0x7e466254, 0x8d13c2f6, 0xd8b8e890, 0x39f75e2e, 0xc3aff582,
			0x5d80be9f, 0xd0937c69, 0xd52da96f, 0x2512b3cf, 0xac993bc8, 0x187da710, 0x9c636ee8, 0x3bbb7bdb,
			0x267809cd, 0x5918f46e, 0x9ab701ec, 0x4f9aa883, 0x956e65e6, 0xffe67eaa, 0xbccf0821, 0x15e8e6ef,
			0xe79bd9ba, 0x6f36ce4a, 0x9f09d4ea, 0xb07cd629, 0xa4b2af31, 0x3f23312a, 0xa59430c6, 0xa266c035,
			0x4ebc3774, 0x82caa6fc, 0x90d0b0e0, 0xa7d81533, 0x04984af1, 0xecdaf741, 0xcd500e7f, 0x91f62f17,
			0x4dd68d76, 0xefb04d43, 0xaa4d54cc, 0x9604dfe4, 0xd1b5e39e, 0x6a881b4c, 0x2c1fb8c1, 0x65517f46,
			0x5eea049d, 0x8c355d01, 0x877473fa, 0x0b412efb, 0x671d5ab3, 0xdbd25292, 0x105633e9, 0xd647136d,
			0xd7618c9a, 0xa10c7a37, 0xf8148e59, 0x133c89eb, 0xa927eece, 0x61c935b7, 0x1ce5ede1, 0x47b13c7a,
			0xd2df599c, 0xf2733f55, 0x14ce7918, 0xc737bf73, 0xf7cdea53, 0xfdaa5b5f, 0x3d6f14df, 0x44db8678,
			0xaff381ca, 0x68c43eb9, 0x24342c38, 0xa3405fc2, 0x1dc37216, 0xe2250cbc, 0x3c498b28, 0x0d9541ff,
			0xa8017139, 0x0cb3de08, 0xb4e49cd8, 0x56c19064, 0xcb84617b, 0x32b670d5, 0x6c5c7448, 0xb85742d0,
		};
	}
}
