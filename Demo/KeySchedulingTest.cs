using System;
using Stopwatch = System.Diagnostics.Stopwatch;
using SymmetricAlgorithmPlus = openCrypto.SymmetricAlgorithmPlus;
using CipherModePlus = openCrypto.CipherModePlus;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace Demo
{
	static class KeySchedulingTest
	{
		const int LOOP = 1 << 14;
		public static uint[] Run (ImplementationType implType, int keySize, int blockSize)
		{
			byte[] key = new byte [keySize >> 3];
			byte[] iv  = new byte [blockSize >> 3];
			object algo = Helper.CreateInstance (implType);
			SymmetricAlgorithmPlus sap = algo as SymmetricAlgorithmPlus;
			if (sap != null) {
				return Run (sap, key, iv);
			}
			SymmetricAlgorithm sa = algo as SymmetricAlgorithm;
			if (sa != null) {
				return Run (sa, key, iv);
			}
			return Run (algo as IBlockCipher, key, iv);
		}

		static uint[] Run (SymmetricAlgorithm algo, byte[] key, byte[] iv)
		{
			algo.KeySize = key.Length << 3;
			algo.BlockSize = iv.Length << 3;
			algo.FeedbackSize = iv.Length << 3;

			Stopwatch sw = new Stopwatch ();
			uint[] result = new uint [2];
			sw.Reset (); sw.Start ();
			for (int i = 0; i < LOOP; i ++) {
				algo.CreateEncryptor (key, iv).Dispose ();
			}
			sw.Stop ();
			result[0] = (uint)(LOOP / sw.Elapsed.TotalSeconds);

			sw.Reset (); sw.Start ();
			for (int i = 0; i < LOOP; i++) {
				algo.CreateDecryptor (key, iv).Dispose ();
			}
			sw.Stop ();
			result[1] = (uint)(LOOP / sw.Elapsed.TotalSeconds);

			return result;
		}

		static uint[] Run (IBlockCipher algo, byte[] key, byte[] iv)
		{
			IBlockCipher cipher = new Org.BouncyCastle.Crypto.Modes.CbcBlockCipher (algo);
			ParametersWithIV param = new ParametersWithIV (new KeyParameter (key), iv);
			Stopwatch sw = new Stopwatch ();
			uint[] result = new uint[2];
			sw.Reset (); sw.Start ();
			for (int i = 0; i < LOOP; i++) {
				cipher.Init (true, param);
				cipher.Reset ();
			}
			sw.Stop ();
			result[0] = (uint)(LOOP / sw.Elapsed.TotalSeconds);

			sw.Reset (); sw.Start ();
			for (int i = 0; i < LOOP; i++) {
				cipher.Init (false, param);
				cipher.Reset ();
			}
			sw.Stop ();
			result[1] = (uint)(LOOP / sw.Elapsed.TotalSeconds);

			return result;
		}
	}
}
