using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Demo
{
	static class Helper
	{
		static Dictionary<ImplementationType, string> _implTable;

		static Helper()
		{
			_implTable = new Dictionary<ImplementationType, string> ();
			_implTable.Add (ImplementationType.RijndaelRuntime, "Rijndael (Runtime)");
			_implTable.Add (ImplementationType.RijndaelBouncyCastle, "AES (Bouncy Castle)");
			_implTable.Add (ImplementationType.RijndaelOpenCrypto, "Rijndael (openCrypto)");
			_implTable.Add (ImplementationType.CamelliaOpenCrypto, "Camellia (openCrypto)");
		}

		public static string ToName (ImplementationType type)
		{
			string ret;
			if (_implTable.TryGetValue (type, out ret))
				return ret;
			return null;
		}

		public static ImplementationType ToImplementationType (string text)
		{
			foreach (KeyValuePair<ImplementationType, string> pair in _implTable) {
				if (text.Equals (pair.Value))
					return pair.Key;
			}
			throw new Exception ();
		}

		public static bool IsSymmetricAlgorithmPlus (object algo)
		{
			return (algo as openCrypto.SymmetricAlgorithmPlus != null);
		}

		public static KeySizes[] GetKeySizes (object algo)
		{
			SymmetricAlgorithm sa = algo as SymmetricAlgorithm;
			if (sa != null)
				return sa.LegalKeySizes;
			Org.BouncyCastle.Crypto.IBlockCipher cipher = algo as Org.BouncyCastle.Crypto.IBlockCipher;
			if (cipher == null)
				throw new NotSupportedException ();
			return new KeySizes[] { new KeySizes (128, 256, 64) };
		}

		public static KeySizes[] GetBlockSizes (object algo)
		{
			SymmetricAlgorithm sa = algo as SymmetricAlgorithm;
			if (sa != null)
				return sa.LegalBlockSizes;
			Org.BouncyCastle.Crypto.IBlockCipher cipher = algo as Org.BouncyCastle.Crypto.IBlockCipher;
			if (cipher == null)
				throw new NotSupportedException ();
			return new KeySizes[] {new KeySizes (128, 128, 0)};
		}

		public static object CreateInstance (ImplementationType type)
		{
			switch (type) {
			case ImplementationType.RijndaelRuntime:
				return new RijndaelManaged ();
			case ImplementationType.RijndaelBouncyCastle:
				return new Org.BouncyCastle.Crypto.Engines.AesFastEngine ();
			case ImplementationType.RijndaelOpenCrypto:
				return new openCrypto.RijndaelManaged ();
			case ImplementationType.CamelliaOpenCrypto:
				return new openCrypto.CamelliaManaged ();
			default:
				return null;
			}
		}
	}
}
