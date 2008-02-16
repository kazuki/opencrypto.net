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
	public partial class EncryptionTime : UserControl
	{
		ImplementationType _implType = ImplementationType.RijndaelRuntime;
		int _threads = 1;
		int _dataSize = 1024 * 1024 * 32;
		int _keySize = 128, _blockSize = 128;

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
			set { _dataSize = value;}
		}

		public EncryptionTime ()
		{
			InitializeComponent ();
			label1.Text = Helper.ToName (_implType);
		}

		public void Start ()
		{
			ThreadPool.QueueUserWorkItem (EncryptionThread);
		}

		void EncryptionThread (object instance)
		{
			DateTime start = DateTime.Now;
			instance = Helper.CreateInstance (_implType);
			int totalBlocks = _dataSize / (_blockSize >> 3);
			int unitBlocks = 1024 * 1024 * 2 / (_blockSize >> 3);
			byte[] key = new byte[_keySize >> 3];
			byte[] iv = new byte[_blockSize >> 3];
			byte[] data1 = new byte[unitBlocks * (_blockSize >> 3)];
			byte[] data2 = new byte[unitBlocks * (_blockSize >> 3)];
			Invoke (new SetProgressbarDelegate (SetProgressbarMax), totalBlocks);

			SymmetricAlgorithm algo = instance as SymmetricAlgorithm;
			SymmetricAlgorithmPlus algo2 = instance as SymmetricAlgorithmPlus;
			IBlockCipher bc = instance as IBlockCipher;
			SetProgressbarDelegate setprog = new SetProgressbarDelegate (SetProgressbarValue);
			int step = unitBlocks;

			if (algo2 != null) {
				algo2.NumberOfThreads = _threads;
			}
			if (algo != null) {
				algo.Mode = CipherMode.ECB;
			}
			if (bc != null) {
				bc.Init (true, new KeyParameter (key));
			}

			if (algo != null) {
				using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
					for (int i = 0; i <= totalBlocks - step; i += step) {
						ct.TransformBlock (data1, 0, data1.Length, data2, 0);
						Invoke (setprog, i);
					}
				}
			} else if (bc != null) {
				for (int i = 0; i < totalBlocks; i += step) {
					for (int q = 0; q < unitBlocks; q ++)
						bc.ProcessBlock (data1, q * iv.Length, data2, q * iv.Length);
					Invoke (setprog, i);
				}
			}
			TimeSpan span = DateTime.Now.Subtract (start);
			Invoke (setprog, totalBlocks);
			Invoke (new SetTextDelegate (SetText), label1, Helper.ToName (_implType) + " - Time: " + span.ToString ());
		}
		delegate void SetTextDelegate (Control ctrl, string text);
		delegate void SetProgressbarDelegate (int value);
		void SetText (Control ctrl, string text)
		{
			ctrl.Text = text;
		}
		void SetProgressbarMax (int max)
		{
			progressBar1.Maximum = max;
		}
		void SetProgressbarValue (int value)
		{
			progressBar1.Value = value;
		}
	}
}
