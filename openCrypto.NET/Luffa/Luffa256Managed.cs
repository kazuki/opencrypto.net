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

		protected override unsafe void HashCore (uint* v, uint* m)
		{
			uint* t = stackalloc uint[8];
			for (int i = 0; i < 8; i++)
				t[i] = v[i] ^ v[8 + i] ^ v[16 + i];
			Double (t);
			for (int i = 0; i < 8; i++)
				v[i] ^= t[i] ^ m[i];
			Double (m);
			for (int i = 0; i < 8; i++)
				v[i + 8] ^= t[i] ^ m[i];
			Double (m);
			for (int i = 0; i < 8; i++)
				v[i + 16] ^= t[i] ^ m[i];
			fixed (uint* c = InitialValues) {
				Permute (v, 0, c);
				Permute (v + 8, 1, c + 16);
				Permute (v + 16, 2, c + 32);
			}
		}
	}
}
