// 
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
	public class Luffa384Managed : LuffaManaged
	{
		public Luffa384Managed () : base (384)
		{
		}

		protected override sealed unsafe void HashCore (uint* v, byte* b, uint* c)
		{
			uint* m = stackalloc uint[8];
			uint t0 = v[0] ^ v[8] ^ v[16] ^ v[24];
			uint t1 = v[1] ^ v[9] ^ v[17] ^ v[25];
			uint t2 = v[2] ^ v[10] ^ v[18] ^ v[26];
			uint t3 = v[3] ^ v[11] ^ v[19] ^ v[27];
			uint t4 = v[4] ^ v[12] ^ v[20] ^ v[28];
			uint t5 = v[5] ^ v[13] ^ v[21] ^ v[29];
			uint t6 = v[6] ^ v[14] ^ v[22] ^ v[30];
			uint t7 = v[7] ^ v[15] ^ v[23] ^ v[31];
			uint tmp = t7; t7 = t6; t6 = t5; t5 = t4;
			t4 = t3 ^ tmp; t3 = t2 ^ tmp; t2 = t1;
			t1 = t0 ^ tmp; t0 = tmp;

			v[0] ^= t0; v[8] ^= t0; v[16] ^= t0; v[24] ^= t0;
			v[1] ^= t1; v[9] ^= t1; v[17] ^= t1; v[25] ^= t1;
			v[2] ^= t2; v[10] ^= t2; v[18] ^= t2; v[26] ^= t2;
			v[3] ^= t3; v[11] ^= t3; v[19] ^= t3; v[27] ^= t3;
			v[4] ^= t4; v[12] ^= t4; v[20] ^= t4; v[28] ^= t4;
			v[5] ^= t5; v[13] ^= t5; v[21] ^= t5; v[29] ^= t5;
			v[6] ^= t6; v[14] ^= t6; v[22] ^= t6; v[30] ^= t6;
			v[7] ^= t7; v[15] ^= t7; v[23] ^= t7; v[31] ^= t7;

			t0 = v[24]; t1 = v[25]; t2 = v[26]; t3 = v[27];
			t4 = v[28]; t5 = v[29]; t6 = v[30]; t7 = v[31];
			Double (v + 24);
			v[24] ^= v[16]; v[25] ^= v[17]; v[26] ^= v[18]; v[27] ^= v[19];
			v[28] ^= v[20]; v[29] ^= v[21]; v[30] ^= v[22]; v[31] ^= v[23];
			Double (v + 16);
			v[16] ^= v[8]; v[17] ^= v[9]; v[18] ^= v[10]; v[19] ^= v[11];
			v[20] ^= v[12]; v[21] ^= v[13]; v[22] ^= v[14]; v[23] ^= v[15];
			Double (v + 8);
			v[8] ^= v[0]; v[9] ^= v[1]; v[10] ^= v[2]; v[11] ^= v[3];
			v[12] ^= v[4]; v[13] ^= v[5]; v[14] ^= v[6]; v[15] ^= v[7];
			Double (v);
			Copy (m, b);
			v[0] ^= t0 ^ m[0]; v[1] ^= t1 ^ m[1]; v[2] ^= t2 ^ m[2]; v[3] ^= t3 ^ m[3];
			v[4] ^= t4 ^ m[4]; v[5] ^= t5 ^ m[5]; v[6] ^= t6 ^ m[6]; v[7] ^= t7 ^ m[7];
			Double (m);
			v[8] ^= m[0]; v[9] ^= m[1]; v[10] ^= m[2]; v[11] ^= m[3];
			v[12] ^= m[4]; v[13] ^= m[5]; v[14] ^= m[6]; v[15] ^= m[7];
			Double (m);
			v[16] ^= m[0]; v[17] ^= m[1]; v[18] ^= m[2]; v[19] ^= m[3];
			v[20] ^= m[4]; v[21] ^= m[5]; v[22] ^= m[6]; v[23] ^= m[7];
			Double (m);
			v[24] ^= m[0]; v[25] ^= m[1]; v[26] ^= m[2]; v[27] ^= m[3];
			v[28] ^= m[4]; v[29] ^= m[5]; v[30] ^= m[6]; v[31] ^= m[7];

			Permute (v, 0, c);
			Permute (v + 8, 1, c + 16);
			Permute (v + 16, 2, c + 32);
			Permute (v + 24, 3, c + 48);
		}
	}
}
