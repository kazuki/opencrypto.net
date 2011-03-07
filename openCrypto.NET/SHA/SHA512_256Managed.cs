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
	public class SHA512_256Managed : SHA512Algorithm
	{
		public SHA512_256Managed () : base (256)
		{
		}

		protected override void InitializeInitialHashValue (ulong[] H)
		{
			H[0] = 0x22312194fc2bf72c;
			H[1] = 0x9f555fa3c84c64c2;
			H[2] = 0x2393b86b6f53b151;
			H[3] = 0x963877195940eabd;
			H[4] = 0x96283ee2a88effe3;
			H[5] = 0xbe5e1e2553863992;
			H[6] = 0x2b0199fc2c85b8aa;
			H[7] = 0x0eb72ddc81c52ca2;
		}
	}
}
