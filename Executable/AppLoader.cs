// 
// Copyright (c) 2006-2008 Kazuki Oikawa
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
using openCrypto.FiniteField;
using ECDSAManaged = openCrypto.ECDSA.ECDSA;
using ECDSAParameters = openCrypto.ECDSA.ECDSAParameters;
using openCrypto.EllipticCurve;

namespace openCrypto.Executable
{
	class AppLoader
	{
		static double[] Run (SymmetricAlgorithm algo, CipherMode mode, int keySize, int blockSize, int dataSize)
		{
			double[] result = SpeedTest.Run (algo, mode, keySize, blockSize, dataSize);
			for (int i = 0; i < 10; i++) {
				double[] temp = SpeedTest.Run (algo, mode, keySize, blockSize, dataSize);
				result[0] = Math.Max (result[0], temp[0]);
				result[1] = Math.Max (result[1], temp[1]);
			}
			return result;
		}

		static double[] Run (ECDomainNames domain)
		{
			ECDSAParameters ecdsaParams = ECDSAParameters.CreateNew (ECDomains.GetDomainParameter (domain));
			ECDSAManaged ecdsa = new ECDSAManaged (ecdsaParams);
			int loopA = 5, loopB = 5;
			double[] result = SpeedTest.Run (ecdsa, ecdsaParams, loopA);
			for (int i = 0; i < loopB; i++) {
				double[] temp = SpeedTest.Run (ecdsa, ecdsaParams, loopA);
				result[0] = Math.Min (result[0], temp[0]);
				result[1] = Math.Min (result[1], temp[1]);
			}
			return result;
		}

		static void Main ()
		{
			CipherMode mode = CipherMode.ECB;
			int keySize = 128, blockSize = 128, dataSize = 1024 * 1024 * 8;
			double[] result;

			result = Run (new CamelliaManaged (), mode, keySize, blockSize, dataSize);
			Console.WriteLine ("Camellia Encrypt: {0}Mbps, Decrypt: {1}Mbps", result[0], result[1]);

			result = Run (new RijndaelManaged (), mode, keySize, blockSize, dataSize);
			Console.WriteLine ("Rijndael Encrypt: {0}Mbps, Decrypt: {1}Mbps", result[0], result[1]);

			for (int i = (int)ECDomainNames.secp160r1; i <= (int)ECDomainNames.secp521r1; i ++) {
				ECDomainNames domain = (ECDomainNames)i;
				result = Run (domain);
				Console.WriteLine ("{0}: Sign: {1}ms, Verify: {2}ms", domain, result[0], result[1]);
			}
		}
	}
}
