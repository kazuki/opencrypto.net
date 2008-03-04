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
using Stopwatch = System.Diagnostics.Stopwatch;
using System.Security.Cryptography;
using ECDSAManaged = openCrypto.EllipticCurve.Signature.ECDSAManaged;
using openCrypto.EllipticCurve;
using openCrypto.EllipticCurve.KeyAgreement;

namespace openCrypto.Executable
{
	static class SpeedTest
	{
		static RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider ();

		public static double[] Run (ECDSAManaged ecdsa, int loop)
		{
			Stopwatch sw = new Stopwatch ();
			byte[] hash = new byte[(ecdsa.KeySize >> 3)];
			double[] result = new double[2];
			_rng.GetBytes (hash);
			byte[] sign = ecdsa.SignHash (hash);
			sw.Reset (); sw.Start ();
			for (int i = 0; i < loop; i ++)
				sign = ecdsa.SignHash (hash);
			sw.Stop ();
			result[0] = sw.Elapsed.TotalMilliseconds / (double)loop;

			bool signCheck = ecdsa.VerifyHash (hash, sign);
			sw.Reset (); sw.Start ();
			for (int i = 0; i < loop; i++)
				signCheck = ecdsa.VerifyHash (hash, sign);
			sw.Stop ();
			result[1] = sw.Elapsed.TotalMilliseconds / (double)loop;
			if (!signCheck)
				result[1] = -result[1];
			return result;
		}

		public static double Run (ECDiffieHellman ecdh, int loop)
		{
			Stopwatch sw = new Stopwatch ();
			int keyDataLen = 20;
			byte[] otherPublicKey = ecdh.Parameters.PublicKey;

			// re-generate key
			ecdh.Parameters.PrivateKey = null;

			ecdh.PerformKeyAgreement (otherPublicKey, keyDataLen);
			sw.Reset (); sw.Start ();
			for (int i = 0; i < loop; i++)
				ecdh.PerformKeyAgreement (otherPublicKey, keyDataLen);
			sw.Stop ();
			return sw.Elapsed.TotalMilliseconds / (double)loop;
		}

		public static double Run (ECMQV ecmqv, int loop)
		{
			Stopwatch sw = new Stopwatch ();
			int keyDataLen = 20;
			byte[] otherPublicKey1 = ecmqv.Parameters.KeyPair1.PublicKey;
			byte[] otherPublicKey2 = ecmqv.Parameters.KeyPair2.PublicKey;

			// re-generate key
			ecmqv.Parameters.KeyPair1.PrivateKey = null;
			ecmqv.Parameters.KeyPair2.PrivateKey = null;

			ecmqv.PerformKeyAgreement (otherPublicKey1, otherPublicKey2, keyDataLen);
			sw.Reset (); sw.Start ();
			for (int i = 0; i < loop; i++)
				ecmqv.PerformKeyAgreement (otherPublicKey1, otherPublicKey2, keyDataLen);
			sw.Stop ();
			return sw.Elapsed.TotalMilliseconds / (double)loop;
		}

		public static double[] Run (SymmetricAlgorithmPlus algo, CipherModePlus mode, int keySize, int blockSize, int dataSize, int threads)
		{
			double[] result;
			int totalBlocks = dataSize / (blockSize >> 3);
			byte[] key = new byte [keySize >> 3];
			byte[] iv  = new byte [blockSize >> 3];
			algo.NumberOfThreads = threads;
			algo.ModePlus = mode;
			result = Run (algo, key, iv, totalBlocks);
			for (int i = 0; i < result.Length; i ++)
				result[i] = dataSize * 8.0 / result[i] / (1024.0 * 1024.0);
			return result;
		}

		public static double[] Run (SymmetricAlgorithm algo, CipherMode mode, int keySize, int blockSize, int dataSize)
		{
			double[] result;
			int totalBlocks = dataSize / (blockSize >> 3);
			byte[] key = new byte [keySize >> 3];
			byte[] iv  = new byte [blockSize >> 3];
			algo.Mode = mode;
			result = Run (algo, key, iv, totalBlocks);
			for (int i = 0; i < result.Length; i ++)
				result[i] = dataSize * 8.0 / result[i] / (1024.0 * 1024.0);
			return result;
		}

		static double[] Run (SymmetricAlgorithm algo, byte[] key, byte[] iv, int totalBlocks)
		{
			double[] result = new double[] {0.0, 0.0};
			byte[] input = new byte[iv.Length * totalBlocks];
			byte[] output = new byte[iv.Length * totalBlocks];
			Stopwatch sw = new Stopwatch ();
			algo.KeySize = key.Length << 3;
			algo.BlockSize = iv.Length << 3;
			algo.FeedbackSize = iv.Length << 3;
			using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
				if (ct.CanTransformMultipleBlocks) {
					sw.Reset (); sw.Start ();
					ct.TransformBlock (input, 0, input.Length, output, 0);
					sw.Stop ();
				} else {
					sw.Reset (); sw.Start ();
					for (int i = 0, q = 0; i < totalBlocks; i ++, q += iv.Length) {
						ct.TransformBlock (input, q, iv.Length, output, q);
					}
					sw.Stop ();
				}
				result[0] = sw.Elapsed.TotalSeconds;
			}
			using (ICryptoTransform ct = algo.CreateDecryptor (key, iv)) {
				if (ct.CanTransformMultipleBlocks) {
					sw.Reset (); sw.Start ();
					ct.TransformBlock (input, 0, input.Length, output, 0);
					sw.Stop ();
				} else {
					sw.Reset (); sw.Start ();
					for (int i = 0, q = 0; i < totalBlocks; i++, q += iv.Length) {
						ct.TransformBlock (input, q, iv.Length, output, q);
					}
					sw.Stop ();
				}
				result[1] = sw.Elapsed.TotalSeconds;
			}
			return result;
		}
	}
}
