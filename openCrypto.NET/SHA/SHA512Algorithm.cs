// 
// Copyright (c) 2011, Kazuki Oikawa
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

namespace openCrypto.SHA
{
	public abstract class SHA512Algorithm : HashAlgorithm
	{
		int _filled = 0;
		const int BLOCK_SIZE = 1024 / 8;
		const int BLOCK_WORDS = BLOCK_SIZE / 8;
		ulong[] _msg = new ulong[80];
		ulong[] _state = new ulong[8];
		ulong _data_len = 0;

		public SHA512Algorithm (int hashSize)
		{
			base.HashSizeValue = hashSize;
			Initialize ();
		}

		public override void Initialize ()
		{
			_filled = 0;
			_data_len = 0;
			InitializeInitialHashValue (_state);
		}

		protected abstract void InitializeInitialHashValue (ulong[] H);

		protected override unsafe void HashCore (byte[] array, int ibStart, int cbSize)
		{
			_data_len += (uint)cbSize;
			int filled = _filled;
			int idx = filled >> 3;
			int pos = filled & 0x7;

			fixed (ulong* w = _msg, H = _state)
			fixed (byte* array_ptr = array) {
				byte *p = array_ptr, end = array_ptr + cbSize;
				if (pos != 0) {
					for (int i = pos; i < 8 && p < end; i ++, p ++, filled ++)
						w[idx] |= (ulong)(*p) << ((8 - i - 1) * 8);
					idx = filled >> 3; pos = filled & 0x7;
				}
				while (true) {
					if (idx == BLOCK_WORDS) {
						HashCore (w, H);
						idx = filled = 0;
					}
					if (p + 8 > end)
						break;
					w[idx++] =
						(((ulong)*(p + 0)) << 56) |
						(((ulong)*(p + 1)) << 48) |
						(((ulong)*(p + 2)) << 40) |
						(((ulong)*(p + 3)) << 32) |
						(((ulong)*(p + 4)) << 24) |
						(((ulong)*(p + 5)) << 16) |
						(((ulong)*(p + 6)) <<  8) |
						(((ulong)*(p + 7)) <<  0);
					p += 8;
					filled += 8;
				}
				if (p < end) {
					w[idx] = 0;
					for (int i = 7; p < end; i --, p ++, filled ++)
						w[idx] |= (ulong)(*p) << (i * 8);
				}
				_filled = filled;
			}
		}

		protected unsafe override byte[] HashFinal ()
		{
			fixed (ulong* w = _msg, H = _state) {
				int idx = _filled >> 3;
				int pos = _filled & 0x7;

				if (pos == 0)
					w[idx] = 0x8000000000000000UL;
				else
					w[idx] |= 0x80UL << ((8 - pos - 1) * 8);
				for (int i = idx + 1; i < 16; i ++)
						w[i] = 0;

				if (idx >= 14) {
					HashCore (w, H);

					for (int i = 0; i < 14; i ++)
						w[i] = 0;
				}

				w[14] = 0;
				w[15] = _data_len << 3;
				HashCore (w, H);
				
				byte[] hash = new byte[HashSize / 8];
				int word_size = hash.Length / 8;
				int hash_pos = 0;
				for (int i = 0; i < word_size; i ++, hash_pos += 8) {
					ulong h = H[i];
					hash[hash_pos + 0] = (byte)(h >> 56);
					hash[hash_pos + 1] = (byte)(h >> 48);
					hash[hash_pos + 2] = (byte)(h >> 40);
					hash[hash_pos + 3] = (byte)(h >> 32);
					hash[hash_pos + 4] = (byte)(h >> 24);
					hash[hash_pos + 5] = (byte)(h >> 16);
					hash[hash_pos + 6] = (byte)(h >>  8);
					hash[hash_pos + 7] = (byte)(h >>  0);
				}
				for (int shift = 56; hash_pos < hash.Length; hash_pos ++, shift -= 8)
					hash[hash_pos] = (byte)(H[word_size] >> shift);
				return hash;
			}
		}

		private unsafe void HashCore (ulong* w, ulong* H)
		{
			for (int i = 16; i <= 79; i++) {
				ulong t0 = w[i - 2], t1 = w[i - 15];
				t0 = (((t0 << 45) | (t0 >> 19)) ^ ((t0 <<  3) | (t0 >> 61)) ^ (t0 >> 6));
				t1 = (((t1 << 63) | (t1 >>  1)) ^ ((t1 << 56) | (t1 >>  8)) ^ (t1 >> 7));
				w[i] = t0 + w[i - 7] + t1 + w[i - 16];
			}

			ulong[] K = _K;
			ulong a = H[0], b = H[1], c = H[2], d = H[3], e = H[4], f = H[5], g = H[6], h = H[7];
			int t = 0;
			for (int i = 0; i < 10; i++) {
				// 8 * i + 0
				h += (((e << 50) | (e >> 14)) ^ ((e << 46) | (e >> 18)) ^ ((e << 23) | (e >> 41))) + ((e & f) ^ (~e & g)) + K[t] + w[t++];
				d += h;
				h += (((a << 36) | (a >> 28)) ^ ((a << 30) | (a >> 34)) ^ ((a << 25) | (a >> 39))) + ((a & b) ^ (a & c) ^ (b & c));

				// 8 * i + 1
				g += (((d << 50) | (d >> 14)) ^ ((d << 46) | (d >> 18)) ^ ((d << 23) | (d >> 41))) + ((d & e) ^ (~d & f)) + K[t] + w[t++];
				c += g;
				g += (((h << 36) | (h >> 28)) ^ ((h << 30) | (h >> 34)) ^ ((h << 25) | (h >> 39))) + ((h & a) ^ (h & b) ^ (a & b));

				// 8 * i + 2
				f += (((c << 50) | (c >> 14)) ^ ((c << 46) | (c >> 18)) ^ ((c << 23) | (c >> 41))) + ((c & d) ^ (~c & e)) + K[t] + w[t++];
				b += f;
				f += (((g << 36) | (g >> 28)) ^ ((g << 30) | (g >> 34)) ^ ((g << 25) | (g >> 39))) + ((g & h) ^ (g & a) ^ (h & a));

				// 8 * i + 3
				e += (((b << 50) | (b >> 14)) ^ ((b << 46) | (b >> 18)) ^ ((b << 23) | (b >> 41))) + ((b & c) ^ (~b & d)) + K[t] + w[t++];
				a += e;
				e += (((f << 36) | (f >> 28)) ^ ((f << 30) | (f >> 34)) ^ ((f << 25) | (f >> 39))) + ((f & g) ^ (f & h) ^ (g & h));

				// 8 * i + 4
				d += (((a << 50) | (a >> 14)) ^ ((a << 46) | (a >> 18)) ^ ((a << 23) | (a >> 41))) + ((a & b) ^ (~a & c)) + K[t] + w[t++];
				h += d;
				d += (((e << 36) | (e >> 28)) ^ ((e << 30) | (e >> 34)) ^ ((e << 25) | (e >> 39))) + ((e & f) ^ (e & g) ^ (f & g));

				// 8 * i + 5
				c += (((h << 50) | (h >> 14)) ^ ((h << 46) | (h >> 18)) ^ ((h << 23) | (h >> 41))) + ((h & a) ^ (~h & b)) + K[t] + w[t++];
				g += c;
				c += (((d << 36) | (d >> 28)) ^ ((d << 30) | (d >> 34)) ^ ((d << 25) | (d >> 39))) + ((d & e) ^ (d & f) ^ (e & f));

				// 8 * i + 6
				b += (((g << 50) | (g >> 14)) ^ ((g << 46) | (g >> 18)) ^ ((g << 23) | (g >> 41))) + ((g & h) ^ (~g & a)) + K[t] + w[t++];
				f += b;
				b += (((c << 36) | (c >> 28)) ^ ((c << 30) | (c >> 34)) ^ ((c << 25) | (c >> 39))) + ((c & d) ^ (c & e) ^ (d & e));

				// 8 * i + 7
				a += (((f << 50) | (f >> 14)) ^ ((f << 46) | (f >> 18)) ^ ((f << 23) | (f >> 41))) + ((f & g) ^ (~f & h)) + K[t] + w[t++];
				e += a;
				a += (((b << 36) | (b >> 28)) ^ ((b << 30) | (b >> 34)) ^ ((b << 25) | (b >> 39))) + ((b & c) ^ (b & d) ^ (c & d));
			}

			H[0] += a; H[1] += b; H[2] += c; H[3] += d;
			H[4] += e; H[5] += f; H[6] += g; H[7] += h;
		}

		internal static readonly ulong[] _K = {
			0x428a2f98d728ae22, 0x7137449123ef65cd, 0xb5c0fbcfec4d3b2f, 0xe9b5dba58189dbbc,
			0x3956c25bf348b538, 0x59f111f1b605d019, 0x923f82a4af194f9b, 0xab1c5ed5da6d8118,
			0xd807aa98a3030242, 0x12835b0145706fbe, 0x243185be4ee4b28c, 0x550c7dc3d5ffb4e2,
			0x72be5d74f27b896f, 0x80deb1fe3b1696b1, 0x9bdc06a725c71235, 0xc19bf174cf692694,
			0xe49b69c19ef14ad2, 0xefbe4786384f25e3, 0x0fc19dc68b8cd5b5, 0x240ca1cc77ac9c65,
			0x2de92c6f592b0275, 0x4a7484aa6ea6e483, 0x5cb0a9dcbd41fbd4, 0x76f988da831153b5,
			0x983e5152ee66dfab, 0xa831c66d2db43210, 0xb00327c898fb213f, 0xbf597fc7beef0ee4,
			0xc6e00bf33da88fc2, 0xd5a79147930aa725, 0x06ca6351e003826f, 0x142929670a0e6e70,
			0x27b70a8546d22ffc, 0x2e1b21385c26c926, 0x4d2c6dfc5ac42aed, 0x53380d139d95b3df,
			0x650a73548baf63de, 0x766a0abb3c77b2a8, 0x81c2c92e47edaee6, 0x92722c851482353b,
			0xa2bfe8a14cf10364, 0xa81a664bbc423001, 0xc24b8b70d0f89791, 0xc76c51a30654be30,
			0xd192e819d6ef5218, 0xd69906245565a910, 0xf40e35855771202a, 0x106aa07032bbd1b8,
			0x19a4c116b8d2d0c8, 0x1e376c085141ab53, 0x2748774cdf8eeb99, 0x34b0bcb5e19b48a8,
			0x391c0cb3c5c95a63, 0x4ed8aa4ae3418acb, 0x5b9cca4f7763e373, 0x682e6ff3d6b2b8a3,
			0x748f82ee5defb2fc, 0x78a5636f43172f60, 0x84c87814a1f0ab72, 0x8cc702081a6439ec,
			0x90befffa23631e28, 0xa4506cebde82bde9, 0xbef9a3f7b2c67915, 0xc67178f2e372532b,
			0xca273eceea26619c, 0xd186b8c721c0c207, 0xeada7dd6cde0eb1e, 0xf57d4f7fee6ed178,
			0x06f067aa72176fba, 0x0a637dc5a2c898a6, 0x113f9804bef90dae, 0x1b710b35131c471b,
			0x28db77f523047d84, 0x32caab7b40c72493, 0x3c9ebe0a15c9bebc, 0x431d67c49c100d4c,
			0x4cc5d4becb3e42b6, 0x597f299cfc657e2a, 0x5fcb6fab3ad6faec, 0x6c44198c4a475817
		};
	}
}
