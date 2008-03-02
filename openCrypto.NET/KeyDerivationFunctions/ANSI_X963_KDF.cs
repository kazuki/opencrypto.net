// 
// Copyright (c) 2008, Kazuki Oikawa
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

namespace openCrypto.KeyDerivationFunctions
{
	public class ANSI_X963_KDF : KeyDerivationFunction
	{
		HashAlgorithm _hashAlgo;

		public ANSI_X963_KDF (HashAlgorithm hashAlgo)
		{
			_hashAlgo = hashAlgo;
		}

		public override byte[] Calculate (byte[] sharedValue, int keyDataLength)
		{
			// SEC1 3.6.1 (p.29)

			// Step.1 & Step.2: Skip

			// Step.3:
			byte[] counter = new byte[] {0, 0, 0, 1};

			// Step.4 & Step.5:
			int hashBytes = _hashAlgo.HashSize >> 3;
			int blocks = (keyDataLength / hashBytes) + (keyDataLength % hashBytes == 0 ? 0 : 1);
			byte[] K = new byte [blocks * hashBytes];
			byte[] buffer = new byte[sharedValue.Length + counter.Length + (_sharedInfo == null ? 0 : _sharedInfo.Length)];
			for (int i = 0; i < sharedValue.Length; i ++)
				buffer[i] = sharedValue[i];
			for (int i = 0, q = 0; i < blocks; i ++, q += hashBytes) {
				// Copy counter
				buffer[sharedValue.Length] = counter[0];
				buffer[sharedValue.Length + 1] = counter[1];
				buffer[sharedValue.Length + 2] = counter[2];
				buffer[sharedValue.Length + 3] = counter[3];

				// Copy shared info
				if (_sharedInfo != null) {
					for (int k = 0; k < _sharedInfo.Length; k ++)
						buffer[sharedValue.Length + 4 + k] = _sharedInfo[k];
				}

				// Compute hash
				byte[] hash = _hashAlgo.ComputeHash (buffer);

				// Copy hash to result
				for (int k = 0; k < hash.Length; k ++)
					K[q + k] = hash[k];

				// Increment counter;
				if (++counter[0] == 0)
					if (++counter[1] == 0)
						if (++counter[2] == 0)
							counter[3] ++;
			}

			// Step.6
			return K;
		}
	}
}
