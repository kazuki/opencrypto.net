using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Demo
{
	static class MemoryUsageCheck
	{
		static bool _checked = false;
		static byte[] _key, _iv, _input, _output;

		static void CheckFirstCall ()
		{
			if (_checked) throw new Exception ();
			_checked = true;
			_key = new byte[128 >> 3];
			_iv = new byte[128 >> 3];
			_input = new byte[_iv.Length];
			_output = new byte[_iv.Length];
		}

		public static long CheckRuntimeRijndael ()
		{
			CheckFirstCall();
			long after, before = GC.GetTotalMemory (true);

			using (SymmetricAlgorithm algo = new RijndaelManaged ()) {
				using (ICryptoTransform ct = algo.CreateEncryptor (_key, _iv)) {
					ct.TransformBlock (_input, 0, _input.Length, _output, 0);
					after = GC.GetTotalMemory (true);
					ct.TransformBlock (_input, 0, _input.Length, _output, 0);
				}
			}

			return after - before;
		}

		public static long CheckOpenCryptoRijndael ()
		{
			CheckFirstCall();
			long after, before = GC.GetTotalMemory (true);

			using (SymmetricAlgorithm algo = new openCrypto.RijndaelManaged ()) {
				using (ICryptoTransform ct = algo.CreateEncryptor (_key, _iv)) {
					ct.TransformBlock (_input, 0, _input.Length, _output, 0);
					after = GC.GetTotalMemory (true);
					ct.TransformBlock (_input, 0, _input.Length, _output, 0);
				}
			}

			return after - before;
		}

		public static long CheckBCRijndael ()
		{
			CheckFirstCall ();
			long after, before = GC.GetTotalMemory (true);

			IBlockCipher cipher = new CbcBlockCipher(new AesFastEngine ());
			cipher.Init(true, new ParametersWithIV(new KeyParameter(_key), _iv));
			cipher.ProcessBlock (_input, 0, _output, 0);
			after = GC.GetTotalMemory (true);
			cipher.ProcessBlock (_input, 0, _output, 0);

			return after - before;
		}

		public static long CheckOpenCryptoCamellia ()
		{
			CheckFirstCall();
			long after, before = GC.GetTotalMemory (true);

			using (SymmetricAlgorithm algo = new openCrypto.CamelliaManaged ()) {
				using (ICryptoTransform ct = algo.CreateEncryptor (_key, _iv)) {
					ct.TransformBlock (_input, 0, _input.Length, _output, 0);
					after = GC.GetTotalMemory (true);
					ct.TransformBlock (_input, 0, _input.Length, _output, 0);
				}
			}

			return after - before;
		}
	}
}
