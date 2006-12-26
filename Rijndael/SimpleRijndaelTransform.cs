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
	sealed class SimpleRijndaelTransform : SymmetricTransform
	{
		static readonly byte[] RCON = new byte[30];
		static readonly byte[] SBOX = new byte[256];
		static readonly byte[] LOG_TABLE = new byte[256];
		static readonly byte[] ALOG_TABLE = new byte[256];

		int _Nb, _Nk, _Nr;
		byte[] _expandedKey;
		byte[] _C;
#if DEBUG
		int _roundIndex;
#endif

		static SimpleRijndaelTransform ()
		{
			Compute_LogAndAntiLog ();
			Compute_Rcon ();
			Compute_SBox ();
		}
		public SimpleRijndaelTransform (RijndaelManaged algo, byte[] rgbKey, byte[] rgbIV, bool encryption)
			: base (algo, encryption, rgbIV)
		{
			_Nb = rgbIV.Length >> 2;
			_Nk = rgbKey.Length >> 2;
			switch (_Nb * _Nk) {
				case 16: _Nr = 10; break;
				case 24:
				case 36: _Nr = 12; break;
				case 32:
				case 48:
				case 64: _Nr = 14; break;
				default: throw new ArgumentException ();
			}
			SetupKey (rgbKey, out _expandedKey);

			switch (_Nb) {
				case 4:
				case 6: _C = new byte[] {1, 2, 3}; break;
				case 8: _C = new byte[] {1, 3, 4}; break;
			default: throw new ArgumentException ();
			}
		}

		#region 各種数値計算
		private static byte RotationShiftL (byte x)
		{
			return (byte)((x << 1) | (x >> 7));
		}
		private static void Compute_Rcon ()
		{
			byte tmp = 1;
			RCON[0] = 0;
			for (int i = 1; i < RCON.Length; i++) {
				RCON[i] = tmp;
				tmp = mul_over_gf (tmp, 2);
			}
#if DEBUG
			for (int i = 0; i < RCON.Length; i ++)
				Console.Write ("0x{0:x2}000000, ", RCON[i]);
#endif
		}
		private static void Compute_SBox ()
		{
			for (int i = 0; i < SBOX.Length; i++) {
				SBOX[i] = 0x63;
				byte x = inv_over_gf ((byte)i);
				for (int k = 0; k < 5; k++) {
					SBOX[i] ^= (byte)x;
					x = RotationShiftL (x);
				}
			}
		}
		private static void Compute_LogAndAntiLog ()
		{
			byte a = 1;
			byte d;

			for (byte c = 0; c < 255; c++) {
				ALOG_TABLE[c] = a;

				d = (byte)(a & 0x80);
				a <<= 1;
				if (d == 0x80) {
					a ^= 0x1b;
				}
				a ^= ALOG_TABLE[c];

				LOG_TABLE[ALOG_TABLE[c]] = c;
			}
			ALOG_TABLE[255] = ALOG_TABLE[0];
			LOG_TABLE[0] = 0;
		}
		#endregion

		#region ガロア体上の数値演算

		private static byte mul_over_gf (byte x, byte y)
		{
			byte z = 0;
			int flag;
			for (int i = 0; i < 8; i++) {
				if ((y & 1) == 1)
					z ^= x;
				flag = (x & 0x80);
				x <<= 1;
				if (flag != 0)
					x ^= 0x1B;
				y >>= 1;
			}
			return z;
		}

		private static byte inv_over_gf (byte x)
		{
			if (x == 0)
				return 0;
			return ALOG_TABLE [255 - LOG_TABLE [x]];
		}

		#endregion

		#region キーの展開
		private void SetupKey (byte[] rgbKey, out byte[] expandedKey)
		{
			byte[] temp = new byte[4];
			int Nk4 = _Nk << 2;
			expandedKey = new byte[(_Nb << 2) * (_Nr + 1)];

			for (int i = 0; i < Nk4; i++)
				expandedKey[i] = rgbKey[i];

			for (int i = Nk4; i < expandedKey.Length; i += 4) {
				for (int k = 0; k < 4; k ++)
					temp[k] = expandedKey[i - 4 + k];
				if (i % Nk4 == 0) {
					RotByte (temp);
					SubByte (temp);
					temp[0] ^= RCON[i / Nk4];
				} else if (_Nk > 6 && (i % Nk4) == (4 << 2))
					SubByte (temp);
				for (int k = 0; k < 4; k ++)
					expandedKey[i + k] = (byte)(expandedKey[i - Nk4 + k] ^ temp[k]);
			}

#if DEBUG
			Console.WriteLine ("*** Expanded Key ***");
			for (int i = 0; i < expandedKey.Length; i++) {
				Console.Write ("{0:x2} ", expandedKey[i]);
				if ((i + 1) % (_Nb << 2) == 0)
					Console.WriteLine ("");
			}
			Console.WriteLine ("");
#endif
		}
		#endregion

		#region SubByte
		private void SubByte (byte[] x)
		{
			for (int i = 0; i < x.Length; i++)
				x[i] = SBOX[x[i]];
#if DEBUG
			Console.Write ("round[{0:d2}].s_box ", _roundIndex);
			for (int i = 0; i < x.Length; i++) Console.Write ("{0:x2}", x[i]);
			Console.WriteLine ("");
#endif
		}
		#endregion

		#region RotByte
		private void RotByte (byte[] x)
		{
			byte tmp = x[0];
			for (int i = 0; i < 3; i++)
				x[i] = x[i + 1];
			x[3] = tmp;
		}
		#endregion

		#region ShiftRow
		private void ShiftRow (byte[] x)
		{
			byte[] temp = new byte[_Nb << 2];
			
			for (int i = 0, p = 0; i < _Nb; i ++) {
				temp[p] = x[p ++];

				for (int k = 0; k < 3; k++)
					temp[p ++] = x[(((i + _C[k]) % _Nb) << 2) + k + 1];
			}

			for (int i = 0; i < x.Length; i++)
				x[i] = temp[i];

#if DEBUG
			Console.Write ("round[{0:d2}].s_row ", _roundIndex);
			for (int i = 0; i < x.Length; i++) Console.Write ("{0:x2}", x[i]);
			Console.WriteLine ("");
#endif
		}
		#endregion

		#region MixColumn
		private void MixColumn (byte[] x)
		{
			for (int i = 0; i < x.Length; i += 4) {
				byte x0 = x[i + 0];
				byte x1 = x[i + 1];
				byte x2 = x[i + 2];
				byte x3 = x[i + 3];
				x[i + 0] = (byte)(mul_over_gf (x0, 2) ^ mul_over_gf (x1, 3) ^ x2 ^ x3);
				x[i + 1] = (byte)(x0 ^ mul_over_gf (x1, 2) ^ mul_over_gf (x2, 3) ^ x3);
				x[i + 2] = (byte)(x0 ^ x1 ^ mul_over_gf (x2, 2) ^ mul_over_gf (x3, 3));
				x[i + 3] = (byte)(mul_over_gf (x0, 3) ^ x1 ^ x2 ^ mul_over_gf (x3, 2));
			}

#if DEBUG
			Console.Write ("round[{0:d2}].m_col ", _roundIndex);
			for (int i = 0; i < x.Length; i++) Console.Write ("{0:x2}", x[i]);
			Console.WriteLine ("");
#endif
		}
		#endregion

		#region AddRoundKey
		private void AddRoundKey (byte[] x, int offset)
		{
			for (int i = 0, q = offset; i < x.Length; i++, q++)
				x[i] ^= _expandedKey[q];
#if DEBUG
			Console.Write ("round[{0:d2}].k_sch ", _roundIndex);
			for (int i = 0; i < x.Length; i++) Console.Write ("{0:x2}", _expandedKey[i + offset]);
			Console.WriteLine ("");
#endif
		}
		#endregion

		#region Round
#if DEBUG
		private void StartRound (byte[] data)
		{
			Console.Write ("round[{0:d2}].start ", _roundIndex);
			for (int i = 0; i < data.Length; i++) Console.Write ("{0:x2}", data[i]);
			Console.WriteLine ("");
		}
#endif
		private void Round (byte[] data, int keyOffset)
		{
#if DEBUG
			StartRound (data);
#endif
			SubByte (data);
			ShiftRow (data);
			MixColumn (data);
			AddRoundKey (data, keyOffset);

#if DEBUG
			for (int i = 0; i < data.Length;) {
				Console.Write ("{0:x2} ", data[i]);
				if (++i % 4 == 0) Console.WriteLine (string.Empty);
			}
#endif
		}

		private void FinalRound (byte[] data, int keyOffset)
		{
#if DEBUG
			StartRound (data);
#endif
			SubByte (data);
			ShiftRow (data);
			AddRoundKey (data, keyOffset);
		}
		#endregion

		#region SymmetricTransform members

		protected override void EncryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			byte[] temp = new byte[_Nb << 2];
			for (int i = 0; i < temp.Length; i++)
				temp[i] = inputBuffer[i + inputOffset];

#if DEBUG
			StartRound (temp);
#endif
			AddRoundKey (temp, 0);
			for (int i = 1; i < _Nr; i ++) {
#if DEBUG
				_roundIndex = i;
#endif
				Round (temp, i * (_Nb << 2));
			}
#if DEBUG
			_roundIndex = _Nr;
#endif
			FinalRound (temp, _expandedKey.Length - (_Nb << 2));

			for (int i = 0; i < temp.Length; i++)
				outputBuffer[i + outputOffset] = temp[i];
		}
		
		protected override void DecryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset)
		{
			EncryptECB (inputBuffer, inputOffset, outputBuffer, outputOffset);
		}

		#endregion
	}
}
