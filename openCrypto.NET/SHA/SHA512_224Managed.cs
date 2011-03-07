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
	public class SHA512_224Managed : SHA512Algorithm
	{
		public SHA512_224Managed () : base (224)
		{
		}

		protected override void InitializeInitialHashValue (ulong[] H)
		{
			H[0] = 0x8c3d37c819544da2;
			H[1] = 0x73e1996689dcd4d6;
			H[2] = 0x1dfab7ae32ff9c82;
			H[3] = 0x679dd514582f9fcf;
			H[4] = 0x0f6d2b697bd44da8;
			H[5] = 0x77e36f7304c48942;
			H[6] = 0x3f9d85a86a1d36c8;
			H[7] = 0x1112e6ad91d692a1;
		}
	}
}
