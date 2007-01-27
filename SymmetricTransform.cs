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
using System.Security.Cryptography;
using System.Threading;

namespace openCrypto
{
	/// <summary>
	/// <ja>暗号変換の基本操作を定義する抽象クラス</ja>
	/// </summary>
	/// <remarks>
	/// <ja>各種ブロック暗号モードを実装しているので、派生クラスではECBモードのみを実装</ja>
	/// </remarks>
	public abstract class SymmetricTransform : ICryptoTransform
	{
		private SymmetricAlgorithmPlus _algo;
		protected CipherModePlus _mode;
		private int _blockBytes, _mt_threshold, _threads;
		private bool _useThread = false;
		protected bool _encryptMode;
		private ConfluentWaitHandle _waitHandle;
		private byte[] _iv;
		private byte[] _temp;
		private byte[][] _mt_temp;

		public SymmetricTransform (SymmetricAlgorithmPlus algo, bool encryptMode, byte[] iv)
		{
			_algo = algo;
			_blockBytes = algo.BlockSize >> 3;
			_encryptMode = encryptMode;
			_mt_threshold = _blockBytes * 2;
			_iv = (byte[])iv.Clone ();
			_mode = algo.ModePlus;

			if (_algo.NumberOfThreads > 1 && (algo.ModePlus == CipherModePlus.ECB || algo.ModePlus == CipherModePlus.CTR)) {
				_useThread = true;
				_waitHandle = new ConfluentWaitHandle ();
				_threads = _algo.NumberOfThreads;
				if (_mode != CipherModePlus.ECB) {
					_mt_temp = new byte[_threads][];
					for (int i = 0; i < _mt_temp.Length; i ++)
						_mt_temp[i] = new byte[iv.Length];
				}
			} else if (_mode != CipherModePlus.ECB)
				_temp = new byte[iv.Length];
		}

		public bool CanReuseTransform {
			get { return false; }
		}

		public bool CanTransformMultipleBlocks {
			get { return true; }
		}

		public int InputBlockSize {
			get { return _blockBytes; }
		}

		public int OutputBlockSize {
			get { return _blockBytes; }
		}

		public int TransformBlock (byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			inputCount -= inputCount % InputBlockSize;

			if (!_useThread || inputCount < _mt_threshold) {
				switch (_mode) {
				case CipherModePlus.ECB:
					if (_encryptMode) {
						for (int i = inputOffset, o = outputOffset; i < inputOffset + inputCount; i += InputBlockSize, o += OutputBlockSize)
							EncryptECB (inputBuffer, i, outputBuffer, o);
					} else {
						for (int i = inputOffset, o = outputOffset; i < inputOffset + inputCount; i += InputBlockSize, o += OutputBlockSize)
							DecryptECB (inputBuffer, i, outputBuffer, o);
					}
					break;
				case CipherModePlus.CBC:
					if (_encryptMode) {
						for (int i = inputOffset, o = outputOffset; i < inputOffset + inputCount; i += InputBlockSize, o += OutputBlockSize) {
							for (int j = 0; j < InputBlockSize; j ++)
								_temp[j] = (byte)(inputBuffer [i + j] ^ _iv[j]);
							EncryptECB (_temp, 0, outputBuffer, o);
							Buffer.BlockCopy (outputBuffer, o, _iv, 0, InputBlockSize);
						}
					} else {
						for (int i = inputOffset, o = outputOffset; i < inputOffset + inputCount; i += InputBlockSize, o += OutputBlockSize) {
							DecryptECB (inputBuffer, i, outputBuffer, o);
							for (int j = 0; j < InputBlockSize; j ++) {
								outputBuffer[o + j] ^= _iv[j];
								_iv[j] = inputBuffer[i + j];
							}
						}
					}
					break;
				case CipherModePlus.CFB:
					if (_encryptMode) {
						for (int i = inputOffset, o = outputOffset; i < inputOffset + inputCount; i += InputBlockSize, o += OutputBlockSize) {
							EncryptECB (_iv, 0, _temp, 0);
							for (int j = 0; j < InputBlockSize; j ++)
								_iv[j] = outputBuffer[o + j] = (byte)(inputBuffer[i + j] ^ _temp [j]);
						}
					} else {
						for (int i = inputOffset, o = outputOffset; i < inputOffset + inputCount; i += InputBlockSize, o += OutputBlockSize) {
							EncryptECB (_iv, 0, _temp, 0);
							for (int j = 0; j < InputBlockSize; j ++) {
								_iv[j] = inputBuffer[i + j];
								outputBuffer[o + j] = (byte)(inputBuffer[i + j] ^ _temp [j]);
							}
						}
					}
					break;
				case CipherModePlus.OFB:
					for (int i = inputOffset, o = outputOffset; i < inputOffset + inputCount; i += InputBlockSize, o += OutputBlockSize) {
						EncryptECB (_iv, 0, _temp, 0);
						for (int j = 0; j < InputBlockSize; j ++) {
							_iv[j] = _temp[j];
							outputBuffer[o + j] = (byte)(inputBuffer[i + j] ^ _temp [j]);
						}
					}
					break;
				case CipherModePlus.CTR:
					for (int i = inputOffset, o = outputOffset; i < inputOffset + inputCount; i += InputBlockSize, o += OutputBlockSize) {
						EncryptECB (_iv, 0, _temp, 0);
						for (int j = 0; j < InputBlockSize; j ++)
							outputBuffer[o + j] = (byte)(inputBuffer[i + j] ^ _temp [j]);
						for (int j = InputBlockSize - 1; j >= 0; j --) {
							_iv[j] ++;
							if (_iv[j] != 0)
								break;
						}
					}
					break;
				default:
					throw new CryptographicException ();
				}
			} else {
				int blocks = inputCount / _blockBytes;
				int div = blocks / _threads;
				int rem = blocks % _threads;
				_waitHandle.StartThreads (_threads);
				for (int i = 0, idx = 0, q = 0; i < _threads; i ++) {
					int size = _blockBytes * (div + (rem-- > 0 ? 1 : 0));
					bool isLast = (q + size) == inputCount;
					ThreadPool.QueueUserWorkItem (ThreadProcess, new ProcessThreadInfo (this, inputBuffer, inputOffset + q, size, outputBuffer, outputOffset + q, idx, i, isLast));
					idx += size / _blockBytes;
					q += size;
				}
				_waitHandle.WaitOne ();
			}

			return inputCount;
		}

		public byte[] TransformFinalBlock (byte[] inputBuffer, int inputOffset, int inputCount)
		{
			return (_encryptMode ? EncryptFinalBlock (inputBuffer, inputOffset, inputCount)
			                     : DecryptFinalBlock (inputBuffer, inputOffset, inputCount));
		}

		private byte[] EncryptFinalBlock (byte[] inputBuffer, int inputOffset, int inputCount)
		{
			int mod = inputCount % InputBlockSize;
			if (mod != 0 && _algo.Padding == PaddingMode.None)
				throw new CryptographicException ();

			if (inputCount == 0 && (_algo.Padding == PaddingMode.None || _algo.Padding == PaddingMode.Zeros))
				return new byte[0];

			int retBlocks = inputCount / InputBlockSize;
			bool appendLastBlock = false;
			if (mod > 0 || (_algo.Padding != PaddingMode.None && _algo.Padding != PaddingMode.Zeros)) {
				retBlocks ++;
				appendLastBlock = true;
			}
			byte padSize = (byte)(retBlocks * InputBlockSize - inputCount);
			int norSize = inputCount - mod;
			byte[] buf = new byte [retBlocks * OutputBlockSize];
			TransformBlock (inputBuffer, inputOffset, norSize, buf, 0);

			if (appendLastBlock) {
				byte[] lastBlock = new byte [InputBlockSize];

				switch (_algo.Padding) {
					case PaddingMode.ANSIX923:
						Buffer.BlockCopy (inputBuffer, inputOffset + norSize, lastBlock, 0, mod);
						lastBlock [lastBlock.Length - 1] = padSize;
						break;
					case PaddingMode.PKCS7:
						Buffer.BlockCopy (inputBuffer, inputOffset + norSize, lastBlock, 0, mod);
						for (int i = mod; i < lastBlock.Length; i ++)
							lastBlock[i] = padSize;
						break;
					case PaddingMode.ISO10126:
						RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider ();
						rng.GetBytes (lastBlock);
						Buffer.BlockCopy (inputBuffer, inputOffset + norSize, lastBlock, 0, mod);
						lastBlock[lastBlock.Length - 1] = padSize;
						break;
					default:
						Buffer.BlockCopy (inputBuffer, inputOffset + norSize, lastBlock, 0, mod);
						break;
				}
			
				TransformBlock (lastBlock, 0, InputBlockSize, buf, (retBlocks - 1) * OutputBlockSize);
				Array.Clear (lastBlock, 0, lastBlock.Length);
			}

			return buf;
		}

		private byte[] DecryptFinalBlock (byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (inputCount % InputBlockSize != 0)
				throw new CryptographicException ();
			byte[] tmp = new byte[inputCount / InputBlockSize * OutputBlockSize];
			TransformBlock (inputBuffer, inputOffset, inputCount, tmp, 0);

			if (_algo.Padding == PaddingMode.None || _algo.Padding == PaddingMode.Zeros)
				return tmp;
			byte[] tmp2 = new byte[tmp.Length - tmp[tmp.Length - 1]];
			Buffer.BlockCopy (tmp, 0, tmp2, 0, tmp2.Length);
			return tmp2;
		}

		protected abstract void EncryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset);
		protected abstract void DecryptECB (byte[] inputBuffer, int inputOffset, byte[] outputBuffer, int outputOffset);

		public virtual void Dispose ()
		{
			if (_waitHandle != null)
				_waitHandle.Close ();
		}

		void ThreadProcess (object o)
		{
			ProcessThreadInfo info = (ProcessThreadInfo)o;
			switch (_mode) {
			case CipherModePlus.ECB:
				if (_encryptMode) {
					for (int q = 0; q < info.InputCount; q += InputBlockSize)
						EncryptECB (info.InputBuffer, info.InputOffset + q, info.OutputBuffer, info.OutputOffset + q);
				} else {
					for (int q = 0; q < info.InputCount; q += InputBlockSize)
						DecryptECB (info.InputBuffer, info.InputOffset + q, info.OutputBuffer, info.OutputOffset + q);
				}
				break;
			case CipherModePlus.CTR:
				byte[] temp = _mt_temp[info.ThreadIndex];
				if (info.BlockIndex == 0) {
					for (int i = 0; i < _iv.Length; i ++)
						temp[i] = _iv[i];
				} else {
					int indexbuf = info.BlockIndex;
					for (int i = _iv.Length - 1; i >= 0; i --) {
						int tmp = _iv[i] + (indexbuf & 0xFF);
						indexbuf >>= 8;
						if (tmp > 0xFF)
							indexbuf += tmp >> 8;
						temp[i] = (byte)tmp;
					}
				}

				for (int q = 0; q < info.InputCount; q += InputBlockSize) {
					EncryptECB (temp, 0, info.OutputBuffer, info.OutputOffset + q);
					Xor (info.OutputBuffer, info.OutputOffset + q, info.InputBuffer, info.InputOffset + q, temp.Length);
					for (int j = temp.Length - 1; j >= 0; j --) {
						temp[j] ++;
						if (temp[j] != 0)
							break;
					}
				}

				if (info.NeedsUpdateIV) {
					for (int i = 0; i < temp.Length; i ++)
						_iv[i] = temp[i];
				}
				break;
			}
			_waitHandle.EndThread ();
		}

		private static void Xor (byte[] x, int x_offset, byte[] y, int y_offset, int count)
		{
			for (int i = 0; i < count; i ++)
				x[x_offset + i] ^= y[y_offset + i];
		}
		
		internal struct ProcessThreadInfo
		{
			public byte[] InputBuffer, OutputBuffer;
			public int InputOffset, OutputOffset, InputCount, BlockIndex, ThreadIndex;
			public bool NeedsUpdateIV;

			public ProcessThreadInfo (SymmetricTransform st, byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset, int blockIndex, int threadIndex, bool updateIV)
			{
				InputBuffer = inputBuffer; OutputBuffer = outputBuffer;
				InputOffset = inputOffset; OutputOffset = outputOffset;
				InputCount = inputCount;
				ThreadIndex = threadIndex;
				BlockIndex = blockIndex;
				NeedsUpdateIV = updateIV;
			}
		}
	}
}
