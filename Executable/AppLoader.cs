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
using ECDSAManaged = openCrypto.EllipticCurve.Signature.ECDSAManaged;
using openCrypto.EllipticCurve;
using openCrypto.EllipticCurve.KeyAgreement;

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

		static double[] Run (SymmetricAlgorithmPlus algo, CipherModePlus mode, int keySize, int blockSize, int dataSize, int threads)
		{
			double[] result = SpeedTest.Run (algo, mode, keySize, blockSize, dataSize, threads);
			for (int i = 0; i < 10; i++) {
				double[] temp = SpeedTest.Run (algo, mode, keySize, blockSize, dataSize, threads);
				result[0] = Math.Max (result[0], temp[0]);
				result[1] = Math.Max (result[1], temp[1]);
			}
			return result;
		}

		static double[] Run (ECDSAManaged ecdsa)
		{
			int loopA = 5, loopB = 5;
			double[] result = SpeedTest.Run (ecdsa, loopA);
			for (int i = 0; i < loopB; i++) {
				double[] temp = SpeedTest.Run (ecdsa, loopA);
				result[0] = Math.Min (result[0], temp[0]);
				result[1] = Math.Min (result[1], temp[1]);
			}
			return result;
		}

		static double Run (ECDiffieHellman ecdh)
		{
			int loopA = 5, loopB = 5;
			double result = SpeedTest.Run (ecdh, loopA);
			for (int i = 0; i < loopB; i++) {
				double temp = SpeedTest.Run (ecdh, loopA);
				result = Math.Min (result, temp);
			}
			return result;
		}

		static double Run (ECMQV ecdh)
		{
			int loopA = 5, loopB = 5;
			double result = SpeedTest.Run (ecdh, loopA);
			for (int i = 0; i < loopB; i++) {
				double temp = SpeedTest.Run (ecdh, loopA);
				result = Math.Min (result, temp);
			}
			return result;
		}

		static void Main ()
		{
			CipherMode mode = CipherMode.ECB;
			int dataSize = 1024 * 1024;
			double[] result;

			Console.WriteLine ("Symmetric-Key Algorithm:");
			result = Run (new CamelliaManaged (), mode, 128, 128, dataSize);
			Console.WriteLine ("Camellia 128bit Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1]);
			result = Run (new CamelliaManaged (), mode, 192, 128, dataSize);
			Console.WriteLine ("Camellia 192bit Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1]);
			result = Run (new CamelliaManaged (), mode, 256, 128, dataSize);
			Console.WriteLine ("Camellia 256bit Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1]);

			result = Run (new RijndaelManaged (), mode, 128, 128, dataSize);
			Console.WriteLine ("Rijndael 128bit Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1]);
			result = Run (new RijndaelManaged (), mode, 192, 128, dataSize);
			Console.WriteLine ("Rijndael 192bit Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1]);
			result = Run (new RijndaelManaged (), mode, 256, 128, dataSize);
			Console.WriteLine ("Rijndael 256bit Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1]);

			if (Environment.ProcessorCount > 1) {
				result = Run (new CamelliaManaged (), CipherModePlus.ECB, 128, 128, dataSize, 2);
				Console.WriteLine ("Camellia 128bit (2-threads) Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1]);

				result = Run (new RijndaelManaged (), CipherModePlus.ECB, 128, 128, dataSize, 2);
				Console.WriteLine ("Rijndael 128bit (2-threads) Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1]);

				if (Environment.ProcessorCount != 2) {
					result = Run (new CamelliaManaged (), CipherModePlus.ECB, 128, 128, dataSize, Environment.ProcessorCount);
					Console.WriteLine ("Camellia 128bit ({2}-threads) Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1], Environment.ProcessorCount);

					result = Run (new RijndaelManaged (), CipherModePlus.ECB, 128, 128, dataSize, Environment.ProcessorCount);
					Console.WriteLine ("Rijndael 128bit ({2}-threads) Encrypt: {0:f2}Mbps, Decrypt: {1:f2}Mbps", result[0], result[1], Environment.ProcessorCount);
				}
			}
			Console.WriteLine ();

			Console.WriteLine ("ECDSA:");
			for (int i = (int)ECDomainNames.secp112r1; i <= (int)ECDomainNames.secp521r1; i++) {
				ECDomainNames domain = (ECDomainNames)i;
				result = Run (new ECDSAManaged (domain));
				Console.WriteLine ("{0}: Sign: {1}ms, Verify: {2}ms", domain, result[0], result[1]);
			}
			Console.WriteLine ();

			Console.WriteLine ("ECDH:");
			for (int i = (int)ECDomainNames.secp112r1; i <= (int)ECDomainNames.secp521r1; i++) {
				ECDomainNames domain = (ECDomainNames)i;
				double ret = Run (new ECDiffieHellman (domain));
				Console.WriteLine ("{0}: {1}ms", domain, ret);
			}
			Console.WriteLine ();

			Console.WriteLine ("ECMQV:");
			for (int i = (int)ECDomainNames.secp112r1; i <= (int)ECDomainNames.secp521r1; i++) {
				ECDomainNames domain = (ECDomainNames)i;
				double ret = Run (new ECMQV (domain));
				Console.WriteLine ("{0}: {1}ms", domain, ret);
			}
		}
	}
}
