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
using System.Diagnostics;
using System.Security.Cryptography;
#if TEST
using openCrypto.Tests;
#endif

namespace openCrypto
{
	class Program
	{
		/// <summary>
		/// <ja>開発補助用のエントリポイント。ある程度実装できたらクラスライブラリに移行する</ja>
		/// </summary>
		static void Main (string[] args)
		{
#if TEST
			if (args.Length == 1 && args[0] == "test") {
				RunTests ();
				return;
			}
#endif

			SymmetricAlgorithmPlus[] algos = new SymmetricAlgorithmPlus[] {
				new CamelliaManaged (),
				new RijndaelManaged ()
			};
			CipherImplementationType[] types = new CipherImplementationType[] {
				CipherImplementationType.LowMemory,
				CipherImplementationType.Balanced,
				CipherImplementationType.HighSpeed
			};
			CipherModePlus[] modes = new CipherModePlus[] {
				CipherModePlus.ECB,
				CipherModePlus.CBC,
				CipherModePlus.OFB,
				CipherModePlus.CFB,
				CipherModePlus.CTS,
				CipherModePlus.CTR,
			};
			String[] list = new String[] {"Camellia", "Rijndael"};
			String[] tlist = new String[]{"LowMemory", "Balanced", "HighSpeed"};
			String[] mlist = new String[]{"ECB", "CBC", "OFB", "CFB", "CTS", "CTR"};

			Console.WriteLine ("||アルゴリズム||実装タイプ||キーサイズ||ブロックサイズ||暗号化速度||復号速度||鍵生成時間(暗号)||鍵生成時間(復号)||");
			for (int i = 0; i < algos.Length; i ++) {
				for (int j = 0; j < types.Length; j ++) {
					if (!algos[i].HasImplementation (types[j]))
						continue;
					algos[i].ImplementationType = types[j];
					KeySizes keySizes = algos[i].LegalKeySizes[0];
					KeySizes blockSizes = algos[i].LegalBlockSizes[0];
					int keySize = keySizes.MinSize;
					do {
						int blockSize = blockSizes.MinSize;
						do {
							algos[i].BlockSize = blockSize;
							algos[i].KeySize = keySize;
							cryptoSpeedTest (algos[i]);
							double[] speeds = new double[]{0.0, 0.0, 0.0, 0.0};
							for (int k = 0; k < 5; k ++) {
								double[] temp = keygenSpeedTest (algos[i]);
								speeds[2] += temp[0];
								speeds[3] += temp[0];
								temp = cryptoSpeedTest (algos[i]);
								speeds[0] += temp[0];
								speeds[1] += temp[1];
							}
							for (int k = 0; k < 4; k ++)
								speeds[k] /= 5.0;
							
							Console.WriteLine ("||{0}||{5}||{1} bits||{2} bits||{3:f2} Mbps||{4:f2} Mbps||{6:f2} ticks||{7:f2} ticks||",
													 list[i], algos[i].KeySize, algos[i].BlockSize,
													 speeds[0], speeds[1], tlist[j], speeds[2], speeds[3]);
							if (blockSizes.SkipSize == 0) break;
							blockSize += blockSizes.SkipSize;
						} while (blockSize <= blockSizes.MaxSize);
						if (keySizes.SkipSize == 0) break;
						keySize += keySizes.SkipSize;
					} while (keySize <= keySizes.MaxSize);
				}
			}

			Console.WriteLine (String.Empty);
			Console.WriteLine ("||ブロックモード||加速率(暗号化:%)||加速率(復号:%)||");
			for (int i = 0; i < modes.Length; i ++) {
				try {
					double[] avg = new double[] {0.0, 0.0};
					for (int k = 0; k < 5; k ++) {
						double[] temp = BlockCipherModeCost (modes[i]);
						avg[0] += temp[0];
						avg[1] += temp[1];
					}
					Console.WriteLine ("||{0}||{1:f2}||{2:f2}||", mlist[i], avg[0] * 100.0 / 5.0, avg[1] * 100.0 / 5.0);
				} catch {}
			}
		}

		static double[] keygenSpeedTest (SymmetricAlgorithmPlus algo)
		{
			const int LOOPS = 1 << 10;
			byte[] key  = new byte[algo.KeySize >> 3];
			byte[] iv   = new byte[algo.BlockSize >> 3];
			Stopwatch sw = new Stopwatch ();
			double time;

			Helpers.RNG.GetBytes (key);
			sw.Stop (); sw.Reset (); sw.Start ();
			for (int i = 0; i < LOOPS; i ++) {
				using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
				}
			}
			sw.Stop ();
			time = sw.ElapsedTicks / (double)LOOPS;

			sw.Reset (); sw.Start ();
			for (int i = 0; i < LOOPS; i ++) {
				using (ICryptoTransform ct = algo.CreateDecryptor (key, iv)) {
				}
			}
			sw.Stop ();
			return new double[] {time, sw.Elapsed.Ticks / (double)LOOPS};
		}

		static double[] cryptoSpeedTest (SymmetricAlgorithmPlus algo)
		{
			const int SIZE = 1 << 20;
			byte[] bufA = new byte[SIZE];
			byte[] bufB = new byte[bufA.Length];
			byte[] key  = new byte[algo.KeySize >> 3];
			byte[] iv   = new byte[algo.BlockSize >> 3];
			Stopwatch sw = new Stopwatch ();
			double time, mbsize = SIZE * 8.0 / 1024.0 / 1024.0;

			Helpers.RNG.GetBytes (key);
			algo.Mode = CipherMode.ECB;
			sw.Stop (); sw.Reset (); sw.Start ();
			using (ICryptoTransform ct = algo.CreateEncryptor (key, iv))
				ct.TransformBlock (bufA, 0, bufA.Length, bufB, 0);
			sw.Stop ();
			time = sw.Elapsed.TotalSeconds;

			sw.Reset (); sw.Start ();
			using (ICryptoTransform ct = algo.CreateDecryptor (key, iv))
				ct.TransformBlock (bufB, 0, bufB.Length, bufA, 0);
			sw.Stop ();

			return new double[] {mbsize / time, mbsize / sw.Elapsed.TotalSeconds};
		}

		static double[] BlockCipherModeCost (CipherModePlus mode)
		{
			const int SIZE = 1 << 20;
			RijndaelManaged algo = new RijndaelManaged ();
			byte[] bufA = new byte[SIZE];
			byte[] bufB = new byte[bufA.Length];
			byte[] key  = new byte[algo.KeySize >> 3];
			byte[] iv   = new byte[algo.BlockSize >> 3];
			Stopwatch sw = new Stopwatch ();
			double time1, time2, time3, time4;

			Helpers.RNG.GetBytes (bufA);
			Helpers.RNG.GetBytes (key);
			Helpers.RNG.GetBytes (iv);

			algo.ModePlus = CipherModePlus.ECB;
			sw.Stop (); sw.Reset (); sw.Start ();
			using (ICryptoTransform ct = algo.CreateEncryptor (key, iv))
				ct.TransformBlock (bufA, 0, bufA.Length, bufB, 0);
			sw.Stop ();
			time1 = sw.Elapsed.TotalSeconds;

			sw.Reset (); sw.Start ();
			using (ICryptoTransform ct = algo.CreateDecryptor (key, iv))
				ct.TransformBlock (bufB, 0, bufB.Length, bufA, 0);
			sw.Stop ();
			time2 = sw.Elapsed.TotalSeconds;

			algo.ModePlus = mode;
			sw.Reset (); sw.Start ();
			using (ICryptoTransform ct = algo.CreateEncryptor (key, iv))
				ct.TransformBlock (bufA, 0, bufA.Length, bufB, 0);
			sw.Stop ();
			time3 = sw.Elapsed.TotalSeconds;

			sw.Reset (); sw.Start ();
			using (ICryptoTransform ct = algo.CreateDecryptor (key, iv))
				ct.TransformBlock (bufB, 0, bufB.Length, bufA, 0);
			sw.Stop ();
			time4 = sw.Elapsed.TotalSeconds;

			return new double[] {time1 / time3, time2 / time4};
		}

#if TEST
		static void RunTests ()
		{
			RijndaelTest rt = new RijndaelTest ();
			rt.TestECB ();
			rt.TestECB_MultiBlock_1 ();
			rt.TestECB_MultiBlock_2 ();
			rt.TestCBC_MultiBlock_1 ();
			rt.TestCBC_MultiBlock_2 ();
			rt.TestOFB_MultiBlock_1 ();
			rt.TestOFB_MultiBlock_2 ();
			rt.TestCFB_MultiBlock_1 ();
			rt.TestCFB_MultiBlock_2 ();
			rt.TestCTR_MultiBlock_1 ();
			rt.TestCTR_MultiBlock_2 ();

			CamelliaTest ct = new CamelliaTest ();
			ct.TestECB ();
			ct.TestECB_MultiBlock_1 ();
			ct.TestECB_MultiBlock_2 ();
			ct.TestCBC_MultiBlock_1 ();
			ct.TestCBC_MultiBlock_2 ();
			ct.TestOFB_MultiBlock_1 ();
			ct.TestOFB_MultiBlock_2 ();
			ct.TestCFB_MultiBlock_1 ();
			ct.TestCFB_MultiBlock_2 ();
			ct.TestCTR_MultiBlock_1 ();
			ct.TestCTR_MultiBlock_2 ();
		}
#endif
	}
}
