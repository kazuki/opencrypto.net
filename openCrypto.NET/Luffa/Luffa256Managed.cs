﻿// 
// Copyright (c) 2009-2010, Kazuki Oikawa
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

		protected override sealed unsafe void HashCore (uint* v, byte* m, uint* c)
		{
			uint t0 = v[0] ^ v[8] ^ v[16];
			uint t1 = v[1] ^ v[9] ^ v[17];
			uint t2 = v[2] ^ v[10] ^ v[18];
			uint t3 = v[3] ^ v[11] ^ v[19];
			uint t4 = v[4] ^ v[12] ^ v[20];
			uint t5 = v[5] ^ v[13] ^ v[21];
			uint t6 = v[6] ^ v[14] ^ v[22];
			uint t7 = v[7] ^ v[15] ^ v[23];
			uint m0 = ((uint)m[0] << 24) | ((uint)m[1] << 16) | ((uint)m[2] << 8) | m[3];
			uint m1 = ((uint)m[4] << 24) | ((uint)m[5] << 16) | ((uint)m[6] << 8) | m[7];
			uint m2 = ((uint)m[8] << 24) | ((uint)m[9] << 16) | ((uint)m[10] << 8) | m[11];
			uint m3 = ((uint)m[12] << 24) | ((uint)m[13] << 16) | ((uint)m[14] << 8) | m[15];
			uint m4 = ((uint)m[16] << 24) | ((uint)m[17] << 16) | ((uint)m[18] << 8) | m[19];
			uint m5 = ((uint)m[20] << 24) | ((uint)m[21] << 16) | ((uint)m[22] << 8) | m[23];
			uint m6 = ((uint)m[24] << 24) | ((uint)m[25] << 16) | ((uint)m[26] << 8) | m[27];
			uint m7 = ((uint)m[28] << 24) | ((uint)m[29] << 16) | ((uint)m[30] << 8) | m[31];

			uint tmp = t7; t7 = t6; t6 = t5; t5 = t4;
			t4 = t3 ^ tmp; t3 = t2 ^ tmp; t2 = t1;
			t1 = t0 ^ tmp; t0 = tmp;

			v[0] ^= t0 ^ m0; v[1] ^= t1 ^ m1;
			v[2] ^= t2 ^ m2; v[3] ^= t3 ^ m3;
			v[4] ^= t4 ^ m4; v[5] ^= t5 ^ m5;
			v[6] ^= t6 ^ m6; v[7] ^= t7 ^ m7;

			tmp = m7; m7 = m6; m6 = m5; m5 = m4;
			m4 = m3 ^ tmp; m3 = m2 ^ tmp; m2 = m1;
			m1 = m0 ^ tmp; m0 = tmp;

			v[8] ^= t0 ^ m0;  v[9] ^= t1 ^ m1;
			v[10] ^= t2 ^ m2; v[11] ^= t3 ^ m3;
			v[12] ^= t4 ^ m4; v[13] ^= t5 ^ m5;
			v[14] ^= t6 ^ m6; v[15] ^= t7 ^ m7;

			tmp = m7; m7 = m6; m6 = m5; m5 = m4;
			m4 = m3 ^ tmp; m3 = m2 ^ tmp; m2 = m1;
			m1 = m0 ^ tmp; m0 = tmp;

			v[16] ^= t0 ^ m0; v[17] ^= t1 ^ m1;
			v[18] ^= t2 ^ m2; v[19] ^= t3 ^ m3;
			v[20] ^= t4 ^ m4; v[21] ^= t5 ^ m5;
			v[22] ^= t6 ^ m6; v[23] ^= t7 ^ m7;

			Permute (v, 0, c);
			Permute (v + 8, 1, c + 16);
			Permute (v + 16, 2, c + 32);
		}
	}
}
