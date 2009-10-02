// 
// Copyright (c) 2009, Kazuki Oikawa
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
	/// Luffa C# Implementation (Specification Ver. 2.0, 15 Sep 2009)
	/// http://www.sdl.hitachi.co.jp/crypto/luffa/
	/// </summary>
	public abstract class LuffaManaged : HashAlgorithm
	{
		protected uint[] _v = null;
		protected byte[] _buf = new byte[32];
		protected int _filled = 0, _hashLen;

		protected LuffaManaged (int hashLength)
		{
			if (hashLength != 224 && hashLength != 256 && hashLength != 384 && hashLength != 512)
				throw new ArgumentOutOfRangeException ();
			_hashLen = hashLength;
		}

		public override void Initialize ()
		{
			int w;
			switch (_hashLen) {
				case 224:
				case 256:
					w = 3;
					break;
				case 384:
					w = 4;
					break;
				case 512:
					w = 5;
					break;
				default:
					throw new ArgumentOutOfRangeException ();
			}
			_v = new uint[40];
			for (int i = 0; i < w * 8; i++)
				_v[i] = StartingValues[i];
			_filled = 0;
		}

		protected unsafe override void HashCore (byte[] array, int ibStart, int cbSize)
		{
			if (_v == null)
				Initialize ();

			uint* m = stackalloc uint[8];
			fixed (uint* v = _v, c = InitialValues)
			fixed (byte* pary = array, pbuf = _buf) {
				byte* p = pary + ibStart;
				if (_filled > 0) {
					int size = Math.Min (_buf.Length - _filled, cbSize);
					for (int i = 0; i < size; i++)
						pbuf[_filled + i] = p[i];
					_filled += size;
					p += size;
					cbSize -= size;
					if (_filled == _buf.Length) {
						_filled = 0;
						Copy (m, pbuf);
						HashCore (v, m, c);
					}
				}
				while (cbSize >= _buf.Length) {
					Copy (m, p);
					HashCore (v, m, c);
					p += _buf.Length;
					cbSize -= _buf.Length;
				}
				if (cbSize > 0) {
					for (int i = 0; i < cbSize; i++)
						pbuf[i] = p[i];
					_filled = cbSize;
				}
			}
		}

		protected unsafe override byte[] HashFinal ()
		{
			uint* m = stackalloc uint[8];
			uint* t = stackalloc uint[8];
			fixed (uint* v = _v, c = InitialValues)
			fixed (byte* p = _buf) {
				// Padding
				p[_filled] = 0x80;
				for (int i = _filled + 1; i < _buf.Length; i++)
					p[i] = 0;
				Copy (m, p);
				HashCore (v, m, c);

				// blank block
				m[0] = m[1] = m[2] = m[3] = m[4] = m[5] = m[6] = m[7] = 0;

				// finalization
				byte[] r = new byte[_hashLen / 8];
				HashCore (v, m, c);
				t[0] = v[0] ^ v[8] ^ v[16] ^ v[24] ^ v[32];
				t[1] = v[1] ^ v[9] ^ v[17] ^ v[25] ^ v[33];
				t[2] = v[2] ^ v[10] ^ v[18] ^ v[26] ^ v[34];
				t[3] = v[3] ^ v[11] ^ v[19] ^ v[27] ^ v[35];
				t[4] = v[4] ^ v[12] ^ v[20] ^ v[28] ^ v[36];
				t[5] = v[5] ^ v[13] ^ v[21] ^ v[29] ^ v[37];
				t[6] = v[6] ^ v[14] ^ v[22] ^ v[30] ^ v[38];
				t[7] = v[7] ^ v[15] ^ v[23] ^ v[31] ^ v[39];

				r[0] = (byte)(t[0] >> 24); r[1] = (byte)(t[0] >> 16); r[2] = (byte)(t[0] >> 8); r[3] = (byte)(t[0]);
				r[4] = (byte)(t[1] >> 24); r[5] = (byte)(t[1] >> 16); r[6] = (byte)(t[1] >> 8); r[7] = (byte)(t[1]);
				r[8] = (byte)(t[2] >> 24); r[9] = (byte)(t[2] >> 16); r[10] = (byte)(t[2] >> 8); r[11] = (byte)(t[2]);
				r[12] = (byte)(t[3] >> 24); r[13] = (byte)(t[3] >> 16); r[14] = (byte)(t[3] >> 8); r[15] = (byte)(t[3]);
				r[16] = (byte)(t[4] >> 24); r[17] = (byte)(t[4] >> 16); r[18] = (byte)(t[4] >> 8); r[19] = (byte)(t[4]);
				r[20] = (byte)(t[5] >> 24); r[21] = (byte)(t[5] >> 16); r[22] = (byte)(t[5] >> 8); r[23] = (byte)(t[5]);
				r[24] = (byte)(t[6] >> 24); r[25] = (byte)(t[6] >> 16); r[26] = (byte)(t[6] >> 8); r[27] = (byte)(t[6]);
				if (_hashLen == 224)
					return r;
				r[28] = (byte)(t[7] >> 24); r[29] = (byte)(t[7] >> 16); r[30] = (byte)(t[7] >> 8); r[31] = (byte)(t[7]);
				if (_hashLen == 256)
					return r;

				HashCore (v, m, c);
				t[0] = v[0] ^ v[8] ^ v[16] ^ v[24] ^ v[32];
				t[1] = v[1] ^ v[9] ^ v[17] ^ v[25] ^ v[33];
				t[2] = v[2] ^ v[10] ^ v[18] ^ v[26] ^ v[34];
				t[3] = v[3] ^ v[11] ^ v[19] ^ v[27] ^ v[35];
				t[4] = v[4] ^ v[12] ^ v[20] ^ v[28] ^ v[36];
				t[5] = v[5] ^ v[13] ^ v[21] ^ v[29] ^ v[37];
				t[6] = v[6] ^ v[14] ^ v[22] ^ v[30] ^ v[38];
				t[7] = v[7] ^ v[15] ^ v[23] ^ v[31] ^ v[39];

				r[32] = (byte)(t[0] >> 24); r[33] = (byte)(t[0] >> 16); r[34] = (byte)(t[0] >> 8); r[35] = (byte)(t[0]);
				r[36] = (byte)(t[1] >> 24); r[37] = (byte)(t[1] >> 16); r[38] = (byte)(t[1] >> 8); r[39] = (byte)(t[1]);
				r[40] = (byte)(t[2] >> 24); r[41] = (byte)(t[2] >> 16); r[42] = (byte)(t[2] >> 8); r[43] = (byte)(t[2]);
				r[44] = (byte)(t[3] >> 24); r[45] = (byte)(t[3] >> 16); r[46] = (byte)(t[3] >> 8); r[47] = (byte)(t[3]);
				if (_hashLen == 384)
					return r;
				r[48] = (byte)(t[4] >> 24); r[49] = (byte)(t[4] >> 16); r[50] = (byte)(t[4] >> 8); r[51] = (byte)(t[4]);
				r[52] = (byte)(t[5] >> 24); r[53] = (byte)(t[5] >> 16); r[54] = (byte)(t[5] >> 8); r[55] = (byte)(t[5]);
				r[56] = (byte)(t[6] >> 24); r[57] = (byte)(t[6] >> 16); r[58] = (byte)(t[6] >> 8); r[59] = (byte)(t[6]);
				r[60] = (byte)(t[7] >> 24); r[61] = (byte)(t[7] >> 16); r[62] = (byte)(t[7] >> 8); r[63] = (byte)(t[7]);
				return r;
			}
		}

		protected abstract unsafe void HashCore (uint* v, uint* m, uint* c);

		protected static unsafe void Double (uint* x)
		{
			uint tmp = x[7];
			x[7] = x[6];
			x[6] = x[5];
			x[5] = x[4];
			x[4] = x[3] ^ tmp;
			x[3] = x[2] ^ tmp;
			x[2] = x[1];
			x[1] = x[0] ^ tmp;
			x[0] = tmp;
		}

		protected static unsafe void Permute (uint* v, uint* c)
		{
			Step (v, c[0], c[1]);
			Step (v, c[2], c[3]);
			Step (v, c[4], c[5]);
			Step (v, c[6], c[7]);
			Step (v, c[8], c[9]);
			Step (v, c[10], c[11]);
			Step (v, c[12], c[13]);
			Step (v, c[14], c[15]);
		}

		protected static unsafe void Permute (uint* v, int j, uint* c)
		{
			v[4] = (v[4] << j) | (v[4] >> (32 - j));
			v[5] = (v[5] << j) | (v[5] >> (32 - j));
			v[6] = (v[6] << j) | (v[6] >> (32 - j));
			v[7] = (v[7] << j) | (v[7] >> (32 - j));
			Step (v, c[0], c[1]);
			Step (v, c[2], c[3]);
			Step (v, c[4], c[5]);
			Step (v, c[6], c[7]);
			Step (v, c[8], c[9]);
			Step (v, c[10], c[11]);
			Step (v, c[12], c[13]);
			Step (v, c[14], c[15]);
		}

		protected static unsafe void Step (uint* v, uint c0, uint c1)
		{
			uint tmp;

			// SubCrumb (from p.23, Implementations of SubCrumb for Intel Core2)
			tmp   = v[0]; v[0] |= v[1]; v[2] ^= v[3];
			v[1] = ~v[1]; v[0] ^= v[3]; v[3] &= tmp;
			v[1] ^= v[3]; v[3] ^= v[2]; v[2] &= v[0];
			v[0] = ~v[0]; v[2] ^= v[1]; v[1] |= v[3];
			tmp  ^= v[1]; v[3] ^= v[2]; v[2] &= v[1];
			v[1] ^= v[0]; v[0]  = tmp;
			tmp   = v[5]; v[5] |= v[6]; v[7] ^= v[4];
			v[6] = ~v[6]; v[5] ^= v[4]; v[4] &= tmp;
			v[6] ^= v[4]; v[4] ^= v[7]; v[7] &= v[5];
			v[5] = ~v[5]; v[7] ^= v[6]; v[6] |= v[4];
			tmp  ^= v[6]; v[4] ^= v[7]; v[7] &= v[6];
			v[6] ^= v[5]; v[5]  = tmp;

			// MixWord & Add Constant
			v[4] ^= v[0]; v[5] ^= v[1]; v[6] ^= v[2]; v[7] ^= v[3];
			v[0] = ((v[0] << 2) | (v[0] >> 30)) ^ v[4];
			v[1] = ((v[1] << 2) | (v[1] >> 30)) ^ v[5];
			v[2] = ((v[2] << 2) | (v[2] >> 30)) ^ v[6];
			v[3] = ((v[3] << 2) | (v[3] >> 30)) ^ v[7];
			v[4] = ((v[4] << 14) | (v[4] >> 18)) ^ v[0];
			v[5] = ((v[5] << 14) | (v[5] >> 18)) ^ v[1];
			v[6] = ((v[6] << 14) | (v[6] >> 18)) ^ v[2];
			v[7] = ((v[7] << 14) | (v[7] >> 18)) ^ v[3];
			v[0] = ((v[0] << 10) | (v[0] >> 22)) ^ v[4] ^ c0; // Add Constant (c0)
			v[1] = ((v[1] << 10) | (v[1] >> 22)) ^ v[5];
			v[2] = ((v[2] << 10) | (v[2] >> 22)) ^ v[6];
			v[3] = ((v[3] << 10) | (v[3] >> 22)) ^ v[7];
			v[4] = ((v[4] << 1) | (v[4] >> 31)) ^ c1;         // Add Constant (c1)
			v[5] = (v[5] << 1) | (v[5] >> 31);
			v[6] = (v[6] << 1) | (v[6] >> 31);
			v[7] = (v[7] << 1) | (v[7] >> 31);
		}

		#region Misc
		unsafe void Copy (uint* m, byte* buf)
		{
			m[0] = ((uint)buf[0] << 24) | ((uint)buf[1] << 16) | ((uint)buf[2] << 8) | buf[3];
			m[1] = ((uint)buf[4] << 24) | ((uint)buf[5] << 16) | ((uint)buf[6] << 8) | buf[7];
			m[2] = ((uint)buf[8] << 24) | ((uint)buf[9] << 16) | ((uint)buf[10] << 8) | buf[11];
			m[3] = ((uint)buf[12] << 24) | ((uint)buf[13] << 16) | ((uint)buf[14] << 8) | buf[15];
			m[4] = ((uint)buf[16] << 24) | ((uint)buf[17] << 16) | ((uint)buf[18] << 8) | buf[19];
			m[5] = ((uint)buf[20] << 24) | ((uint)buf[21] << 16) | ((uint)buf[22] << 8) | buf[23];
			m[6] = ((uint)buf[24] << 24) | ((uint)buf[25] << 16) | ((uint)buf[26] << 8) | buf[27];
			m[7] = ((uint)buf[28] << 24) | ((uint)buf[29] << 16) | ((uint)buf[30] << 8) | buf[31];
		}
		#endregion

		#region Constants
		protected static readonly uint[] StartingValues = new uint[] {
			0x6d251e69, 0x44b051e0, 0x4eaa6fb4, 0xdbf78465,
			0x6e292011, 0x90152df4, 0xee058139, 0xdef610bb,
			0xc3b44b95, 0xd9d2f256, 0x70eee9a0, 0xde099fa3,
			0x5d9b0557, 0x8fc944b3, 0xcf1ccf0e, 0x746cd581,
			0xf7efc89d, 0x5dba5781, 0x04016ce5, 0xad659c05,
			0x0306194f, 0x666d1836, 0x24aa230a, 0x8b264ae7,
			0x858075d5, 0x36d79cce, 0xe571f7d7, 0x204b1f67,
			0x35870c6a, 0x57e9e923, 0x14bcb808, 0x7cde72ce,
			0x6c68e9be, 0x5ec41e22, 0xc825b7c7, 0xaffb4363,
			0xf5df3999, 0x0fc688f1, 0xb07224cc, 0x03e86cea
		};

		protected static readonly uint[] InitialValues = new uint[] {
			0x303994a6, 0xe0337818, 0xc0e65299, 0x441ba90d,
			0x6cc33a12, 0x7f34d442, 0xdc56983e, 0x9389217f,
			0x1e00108f, 0xe5a8bce6, 0x7800423d, 0x5274baf4,
			0x8f5b7882, 0x26889ba7, 0x96e1db12, 0x9a226e9d,
			0xb6de10ed, 0x01685f3d, 0x70f47aae, 0x05a17cf4,
			0x0707a3d4, 0xbd09caca, 0x1c1e8f51, 0xf4272b28,
			0x707a3d45, 0x144ae5cc, 0xaeb28562, 0xfaa7ae2b,
			0xbaca1589, 0x2e48f1c1, 0x40a46f3e, 0xb923c704,
			0xfc20d9d2, 0xe25e72c1, 0x34552e25, 0xe623bb72,
			0x7ad8818f, 0x5c58a4a4, 0x8438764a, 0x1e38e2e7,
			0xbb6de032, 0x78e38b9d, 0xedb780c8, 0x27586719,
			0xd9847356, 0x36eda57f, 0xa2c78434, 0x703aace7,
			0xb213afa5, 0xe028c9bf, 0xc84ebe95, 0x44756f91,
			0x4e608a22, 0x7e8fce32, 0x56d858fe, 0x956548be,
			0x343b138f, 0xfe191be2, 0xd0ec4e3d, 0x3cb226e5,
			0x2ceb4882, 0x5944a28e, 0xb3ad2208, 0xa1c4c355,
			0xf0d2e9e3, 0x5090d577, 0xac11d7fa, 0x2d1925ab,
			0x1bcb66f2, 0xb46496ac, 0x6f2d9bc9, 0xd1925ab0,
			0x78602649, 0x29131ab6, 0x8edae952, 0x0fc053c3,
			0x3b6ba548, 0x3f014f0c, 0xedae9520, 0xfc053c31
		};
		#endregion
	}
}
