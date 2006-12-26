// 
// Copyright (c) 2006, Kazuki Oikawa
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
	/// <ja>Camelliaアルゴリズムの実装クラスです</ja>
	/// </summary>
	public class CamelliaManaged : Camellia
	{
		public CamelliaManaged ()
		{
			_implType = CipherImplementationType.HighSpeed;
		}
		
		public override ICryptoTransform CreateEncryptor (byte[] rgbKey, byte[] rgbIV)
		{
			return Create (rgbKey, rgbIV, true);
		}
		
		public override ICryptoTransform CreateDecryptor (byte[] rgbKey, byte[] rgbIV)
		{
			return Create (rgbKey, rgbIV, false);
		}
		
		private ICryptoTransform Create (byte[] rgbKey, byte[] rgbIV, bool encrypt)
		{
			if (ImplementationType != CipherImplementationType.HighSpeed)
				throw new NotImplementedException ();

			if (BitConverter.IsLittleEndian) {
				if (IntPtr.Size != 8)
					return new CamelliaTransformLE (this, rgbKey, rgbIV, encrypt);
				return new CamelliaTransform64LE (rgbKey, rgbIV, this, encrypt);
			} else {
				return new CamelliaTransformBE (this, rgbKey, rgbIV, encrypt);
			}
		}

		public override bool HasImplementation (CipherImplementationType type)
		{
			return (type == CipherImplementationType.HighSpeed);
		}
	}
}
