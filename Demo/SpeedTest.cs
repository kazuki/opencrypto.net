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
	static class SpeedTest
	{
		public static double[] Run (ImplementationType implType, string mode, int keySize, int blockSize, int dataSize, int threads)
		{
			double[] result;
			int totalBlocks = dataSize / (blockSize >> 3);
			byte[] key = new byte [keySize >> 3];
			byte[] iv  = new byte [blockSize >> 3];
			object algo = Helper.CreateInstance (implType);
			do {
				SymmetricAlgorithmPlus sap = algo as SymmetricAlgorithmPlus;
				if (sap != null) {
					sap.NumberOfThreads = threads;
					sap.ModePlus = (CipherModePlus)Enum.Parse (typeof (CipherModePlus), mode);
					result = Run (sap, key, iv, totalBlocks);
					break;
				}
				SymmetricAlgorithm sa = algo as SymmetricAlgorithm;
				if (sa != null) {
					sa.Mode = (CipherMode)Enum.Parse (typeof (CipherMode), mode);
					result = Run (sa, key, iv, totalBlocks);
					break;
				}
				result = Run (algo as IBlockCipher, mode, key, iv, totalBlocks);
			} while (false);

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

		static double[] Run (IBlockCipher algo, string mode, byte[] key, byte[] iv, int totalBlocks)
		{
			double[] result = new double[] {0.0, 0.0};
			byte[][] data = new byte[][] { new byte[iv.Length], new byte[iv.Length] };
			Stopwatch sw = new Stopwatch ();
			IBlockCipher cipher = Setup (algo, mode, true, key, iv);
			sw.Reset (); sw.Start ();
			for (int i = 0; i < totalBlocks; i ++) {
				cipher.ProcessBlock (data[i & 1], 0, data[(i ^ 1) & 1], 0);
			}
			sw.Stop ();
			result[0] = sw.Elapsed.TotalSeconds;

			cipher = Setup (algo, mode, false, key, iv);
			sw.Reset (); sw.Start ();
			for (int i = 0; i < totalBlocks; i++) {
				cipher.ProcessBlock (data[i & 1], 0, data[(i ^ 1) & 1], 0);
			}
			sw.Stop ();
			result[1] = sw.Elapsed.TotalSeconds;
			
			return result;
		}

		static IBlockCipher Setup (IBlockCipher algo, string mode, bool encryption, byte[] key, byte[] iv)
		{
			if (mode == "ECB") {
				algo.Init (encryption, new KeyParameter (key));
				return algo;
			}
			IBlockCipher cipher = null;
			if (mode == "CBC") cipher = new Org.BouncyCastle.Crypto.Modes.CbcBlockCipher (algo);
			if (mode == "CFB") cipher = new Org.BouncyCastle.Crypto.Modes.CfbBlockCipher (algo, iv.Length << 3);
			if (mode == "OFB") cipher = new Org.BouncyCastle.Crypto.Modes.OfbBlockCipher (algo, iv.Length);
			if (cipher != null) {
				cipher.Init (encryption, new ParametersWithIV (new KeyParameter (key), iv));
				return cipher;
			}
			throw new ArgumentException ();
		}
	}
}
