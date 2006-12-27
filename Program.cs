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

namespace openCrypto
{
	class Program
	{
		/// <summary>
		/// <ja>開発補助用のエントリポイント。ある程度実装できたらクラスライブラリに移行する</ja>
		/// </summary>
		static void Main (string[] args)
		{
			SymmetricAlgorithmPlus[] algos = new SymmetricAlgorithmPlus[] {
				new CamelliaManaged (),
				new RijndaelManaged ()
			};
			CipherImplementationType[] types = new CipherImplementationType[] {
				CipherImplementationType.LowMemory,
				CipherImplementationType.Balanced,
				CipherImplementationType.HighSpeed
			};
			String[] list = new String[] {"Camellia", "Rijndael"};
			String[] tlist = new String[]{"LowMemory", "Balanced", "HighSpeed"};

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
	}
}
