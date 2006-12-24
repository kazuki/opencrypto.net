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
		private CipherModePlus _mode;
		private int _blockBytes, _mt_threshold;
		protected bool _encryptMode;
		private Thread[] _threads;
		private byte[] _iv;
		private byte[] _temp;

		public SymmetricTransform (SymmetricAlgorithmPlus algo, bool encryptMode, byte[] iv)
		{
			_algo = algo;
			_blockBytes = algo.BlockSize >> 3;
			_encryptMode = encryptMode;
			_mt_threshold = _blockBytes * 2;
			_iv = (byte[])iv.Clone ();
			_mode = algo.ModePlus;
			if (_mode != CipherModePlus.ECB)
				_temp = new byte[iv.Length];

			if (_algo.NumberOfThreads > 1 && (algo.ModePlus == CipherModePlus.ECB || algo.ModePlus == CipherModePlus.CTR)) {
				_threads = new Thread [algo.NumberOfThreads];
			} else {
				_threads = null;
			}
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

			if (_threads == null || inputCount < _mt_threshold) {
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
				throw new NotImplementedException ();
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
		}
	}
}
