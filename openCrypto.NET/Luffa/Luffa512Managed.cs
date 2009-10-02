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
	public class Luffa512Managed : LuffaManaged
	{
		public Luffa512Managed () : base (512)
		{
		}

		protected override unsafe void HashCore (uint* v, uint* m)
		{
			uint* t = stackalloc uint[8];
			for (int i = 0; i < 8; i++)
				t[i] = v[i] ^ v[8 + i] ^ v[16 + i] ^ v[24 + i] ^ v[32 + i];
			Double (t);
			for (int i = 0; i < 8; i++) {
				v[i] ^= t[i];
				v[8 + i] ^= t[i];
				v[16 + i] ^= t[i];
				v[24 + i] ^= t[i];
				v[32 + i] ^= t[i];
			}

			for (int i = 0; i < 8; i++)
				t[i] = v[i];

			Double (v);
			for (int i = 0; i < 8; i++)
				v[i] ^= v[8 + i];

			Double (v + 8);
			for (int i = 0; i < 8; i++)
				v[8 + i] ^= v[16 + i];

			Double (v + 16);
			for (int i = 0; i < 8; i++)
				v[16 + i] ^= v[24 + i];

			Double (v + 24);
			for (int i = 0; i < 8; i++)
				v[24 + i] ^= v[32 + i];

			Double (v + 32);
			for (int i = 0; i < 8; i++) {
				v[32 + i] ^= t[i];
				t[i] = v[32 + i];
			}

			Double (v + 32);
			for (int i = 0; i < 8; i++)
				v[32 + i] ^= v[24 + i];

			Double (v + 24);
			for (int i = 0; i < 8; i++)
				v[24 + i] ^= v[16 + i];

			Double (v + 16);
			for (int i = 0; i < 8; i++)
				v[16 + i] ^= v[8 + i];

			Double (v + 8);
			for (int i = 0; i < 8; i++)
				v[8 + i] ^= v[i];

			Double (v);
			for (int i = 0; i < 8; i++)
				v[i] ^= t[i] ^ m[i];

			Double (m);
			for (int i = 0; i < 8; i++)
				v[i + 8] ^= m[i];

			Double (m);
			for (int i = 0; i < 8; i++)
				v[i + 16] ^= m[i];

			Double (m);
			for (int i = 0; i < 8; i++)
				v[i + 24] ^= m[i];

			Double (m);
			for (int i = 0; i < 8; i++)
				v[i + 32] ^= m[i];

			fixed (uint* c = InitialValues) {
				Permute (v, 0, c);
				Permute (v + 8, 1, c + 16);
				Permute (v + 16, 2, c + 32);
				Permute (v + 24, 3, c + 48);
				Permute (v + 32, 4, c + 64);
			}
		}
	}
}
