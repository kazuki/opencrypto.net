using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Stopwatch = System.Diagnostics.Stopwatch;
using SymmetricAlgorithmPlus = openCrypto.SymmetricAlgorithmPlus;
using CipherModePlus = openCrypto.CipherModePlus;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace Demo
{
	public partial class EncryptionSpeed : UserControl
	{
		ImplementationType _implType = ImplementationType.RijndaelRuntime;
		int _threads = 1;
		int _dataSize = 1024 * 1024 * 32;
		int _keySize = 128, _blockSize = 128;
		double _speed = 0.0;

		public EncryptionSpeed ()
		{
			InitializeComponent ();
		}

		public ImplementationType ImplementationType {
			get { return _implType; }
			set {
				_implType = value;
				label1.Text = Helper.ToName (value);
			}
		}

		public int NumberOfThreads {
			get { return _threads; }
			set {
				if (value <= 0)
					throw new ArgumentOutOfRangeException ();
				_threads = value;
			}
		}

		public int DataSize {
			get { return _dataSize; }
			set { _dataSize = value; }
		}

		public double Speed {
			get { return _speed; }
		}

		public void Start ()
		{
			DateTime start = DateTime.Now;
			object instance = Helper.CreateInstance (_implType);
			int totalBlocks = _dataSize / (_blockSize >> 3);
			byte[] key = new byte[_keySize >> 3];
			byte[] iv = new byte[_blockSize >> 3];
			byte[] data1 = new byte[_dataSize];
			byte[] data2 = new byte[_dataSize];
			SymmetricAlgorithm algo = instance as SymmetricAlgorithm;
			SymmetricAlgorithmPlus algo2 = instance as SymmetricAlgorithmPlus;
			IBlockCipher bc = instance as IBlockCipher;
			int step = iv.Length;

			if (algo2 != null) {
				algo2.NumberOfThreads = _threads;
			}
			if (algo != null) {
				algo.Mode = CipherMode.ECB;
			}
			if (bc != null) {
				bc.Init (true, new KeyParameter (key));
			}

			Stopwatch sw = new Stopwatch ();
			sw.Reset ();
			sw.Start ();
			if (algo != null) {
				using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
					ct.TransformBlock (data1, 0, data1.Length, data2, 0);
				}
			} else if (bc != null) {
				for (int i = 0, q = 0; i < totalBlocks; i++, q += step) {
					bc.ProcessBlock (data1, q, data2, q);
				}
			}
			sw.Stop ();

			_speed = _dataSize * 8.0 / sw.Elapsed.TotalSeconds / (1024.0 * 1024.0);
			label1.Text = Helper.ToName (_implType) + " - ‘¬“x: " + _speed.ToString ("f2") + " Mbps";
		}

		public void SetProgressbar (int max)
		{
			progressBar1.Maximum = max;
			progressBar1.Value = (int)_speed;
		}
	}
}
