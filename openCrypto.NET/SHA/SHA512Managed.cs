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

namespace openCrypto.SHA
{
	public class SHA512Managed : SHA512Algorithm
	{
		public SHA512Managed () : base (512)
		{
		}

		protected override void InitializeInitialHashValue (ulong[] H)
		{
			H[0] = 0x6a09e667f3bcc908;
			H[1] = 0xbb67ae8584caa73b;
			H[2] = 0x3c6ef372fe94f82b;
			H[3] = 0xa54ff53a5f1d36f1;
			H[4] = 0x510e527fade682d1;
			H[5] = 0x9b05688c2b3e6c1f;
			H[6] = 0x1f83d9abfb41bd6b;
			H[7] = 0x5be0cd19137e2179;
		}
	}
}
