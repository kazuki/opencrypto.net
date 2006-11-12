// 
// Copyright (c) 2006, Kazuki Oikawa
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// * Redistributions of source code must retain the above copyright notice,
//   this list of conditions and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution.
// * Neither the name of the author nor the names of its contributors may be used
//   to endorse or promote products derived from this software
//   without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Security.Cryptography;
using System.Threading;

namespace openCrypto
{
	public abstract class SymmetricTransform : ICryptoTransform
	{
		private SymmetricAlgorithmPlus _algo;
		private int _blockBytes, _mt_threshold;
		protected bool _encryptMode;
		private Thread[] _threads;

		public SymmetricTransform (SymmetricAlgorithmPlus algo, bool encryptMode)
		{
			_algo = algo;
			_blockBytes = algo.BlockSize >> 3;
			_encryptMode = encryptMode;
			_mt_threshold = _blockBytes * 2;

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
				if (_encryptMode) {
					for (int i = inputOffset, o = outputOffset; i < inputCount; i += InputBlockSize, o += OutputBlockSize)
						EncryptECB (inputBuffer, i, outputBuffer, o);
				} else {
					for (int i = inputOffset, o = outputOffset; i < inputCount; i += InputBlockSize, o += OutputBlockSize)
						DecryptECB (inputBuffer, i, outputBuffer, o);
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
