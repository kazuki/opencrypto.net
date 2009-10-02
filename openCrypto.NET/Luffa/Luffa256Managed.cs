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

namespace openCrypto
{
	public class Luffa256Managed : LuffaManaged
	{
		public Luffa256Managed () : base (256)
		{
		}

		protected Luffa256Managed (int hashLen) : base (hashLen)
		{
			if (hashLen != 224)
				throw new System.ArgumentOutOfRangeException ();
		}

		protected override unsafe void HashCore (uint* v, uint* m, uint* c)
		{
			uint* t = stackalloc uint[8];
			uint tmp;
			t[0] = v[0] ^ v[8] ^ v[16];
			t[1] = v[1] ^ v[9] ^ v[17];
			t[2] = v[2] ^ v[10] ^ v[18];
			t[3] = v[3] ^ v[11] ^ v[19];
			t[4] = v[4] ^ v[12] ^ v[20];
			t[5] = v[5] ^ v[13] ^ v[21];
			t[6] = v[6] ^ v[14] ^ v[22];
			t[7] = v[7] ^ v[15] ^ v[23];

			tmp = t[7]; t[7] = t[6]; t[6] = t[5]; t[5] = t[4];
			t[4] = t[3] ^ tmp; t[3] = t[2] ^ tmp; t[2] = t[1];
			t[1] = t[0] ^ tmp; t[0] = tmp;

			v[0] ^= t[0] ^ m[0]; v[1] ^= t[1] ^ m[1];
			v[2] ^= t[2] ^ m[2]; v[3] ^= t[3] ^ m[3];
			v[4] ^= t[4] ^ m[4]; v[5] ^= t[5] ^ m[5];
			v[6] ^= t[6] ^ m[6]; v[7] ^= t[7] ^ m[7];

			tmp = m[7]; m[7] = m[6]; m[6] = m[5]; m[5] = m[4];
			m[4] = m[3] ^ tmp; m[3] = m[2] ^ tmp; m[2] = m[1];
			m[1] = m[0] ^ tmp; m[0] = tmp;

			v[8] ^= t[0] ^ m[0];  v[9] ^= t[1] ^ m[1];
			v[10] ^= t[2] ^ m[2]; v[11] ^= t[3] ^ m[3];
			v[12] ^= t[4] ^ m[4]; v[13] ^= t[5] ^ m[5];
			v[14] ^= t[6] ^ m[6]; v[15] ^= t[7] ^ m[7];

			tmp = m[7]; m[7] = m[6]; m[6] = m[5]; m[5] = m[4];
			m[4] = m[3] ^ tmp; m[3] = m[2] ^ tmp; m[2] = m[1];
			m[1] = m[0] ^ tmp; m[0] = tmp;

			v[16] ^= t[0] ^ m[0]; v[17] ^= t[1] ^ m[1];
			v[18] ^= t[2] ^ m[2]; v[19] ^= t[3] ^ m[3];
			v[20] ^= t[4] ^ m[4]; v[21] ^= t[5] ^ m[5];
			v[22] ^= t[6] ^ m[6]; v[23] ^= t[7] ^ m[7];

			Permute (v, c);
			Permute (v + 8, 1, c + 16);
			Permute (v + 16, 2, c + 32);
		}
	}
}
