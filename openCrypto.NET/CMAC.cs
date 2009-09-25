// 
// Copyright (c) 2009, Kazuki Oikawa
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

namespace openCrypto
{
	/// <summary>CMAC Implementation (NIST SP 800-38B)</summary>
	public class CMAC : KeyedHashAlgorithm
	{
		SymmetricAlgorithm _algo;
		ICryptoTransform _ct = null;
		byte[] _k1, _k2, _state, _buf;
		int _bufFilled, _hashBits;

		static readonly byte[] R_128, R_64;

		static CMAC ()
		{
			R_64 = new byte[8];
			R_64[R_64.Length - 1] = 0x1B;

			R_128 = new byte[16];
			R_128[R_128.Length - 1] = 0x87;
		}

		public CMAC (SymmetricAlgorithm algo)
			: this (algo, algo.BlockSize)
		{
		}

		public CMAC (SymmetricAlgorithm algo, int hashBits)
		{
			_algo = algo;
			_algo.Padding = PaddingMode.None;
			_algo.Mode = CipherMode.ECB;
			_hashBits = hashBits;

			if (_algo.BlockSize != 64 && _algo.BlockSize != 128)
				throw new NotSupportedException ();
			if (_hashBits <= 0 || _hashBits > _algo.BlockSize)
				throw new ArgumentOutOfRangeException ();
			if ((_hashBits & 7) != 0)
				throw new NotSupportedException ();
		}
		
		public override void Initialize ()
		{
			_ct = _algo.CreateEncryptor ();
			_k1 = new byte[_algo.BlockSize >> 3];
			_k2 = new byte[_k1.Length];
			_state = new byte[_k1.Length];
			_buf = new byte[_k1.Length];
			_bufFilled = 0;
			byte[] R = (_algo.BlockSize == 64 ? R_64 : R_128);

			_ct.TransformBlock (new byte[_k1.Length], 0, _k1.Length, _k1, 0);
			int msb = MSB_1 (_k1);
			LeftShift_1 (_k1);
			if (msb != 0) {
				for (int i = 0; i < _k1.Length; i++)
					_k1[i] ^= R[i];
			}

			Buffer.BlockCopy (_k1, 0, _k2, 0, _k2.Length);
			LeftShift_1 (_k2);
			if (MSB_1 (_k1) != 0) {
				for (int i = 0; i < _k2.Length; i++)
					_k2[i] ^= R[i];
			}
		}

		protected override void HashCore (byte[] array, int ibStart, int cbSize)
		{
			if (_ct == null)
				Initialize ();

			if (_bufFilled != 0) {
				int copySize = Math.Min (_buf.Length - _bufFilled, cbSize);
				Buffer.BlockCopy (array, ibStart, _buf, _bufFilled, copySize);
				_bufFilled += copySize;
				ibStart += copySize;
				cbSize -= copySize;
				if (_bufFilled == _buf.Length && cbSize > 0) {
					for (int i = 0; i < _buf.Length; i++)
						_buf[i] ^= _state[i];
					_ct.TransformBlock (_buf, 0, _buf.Length, _state, 0);
					_bufFilled = 0;
				}
			}

			while (cbSize > _buf.Length) {
				for (int i = 0; i < _buf.Length; i++)
					_buf[i] = (byte)(_state[i] ^ array[ibStart + i]);
				_ct.TransformBlock (_buf, 0, _buf.Length, _state, 0);
				ibStart += _buf.Length;
				cbSize -= _buf.Length;
			}

			if (cbSize > 0) {
				_bufFilled = cbSize;
				Buffer.BlockCopy (array, ibStart, _buf, 0, cbSize);
			}
		}

		protected override byte[] HashFinal ()
		{
			if (_bufFilled == _buf.Length) {
				for (int i = 0; i < _buf.Length; i++)
					_buf[i] ^= (byte)(_k1[i] ^ _state[i]);
			} else {
				for (int i = 0; i < _bufFilled; i++)
					_buf[i] ^= (byte)(_k2[i] ^ _state[i]);
				_buf[_bufFilled] = (byte)(0x80 ^ _k2[_bufFilled] ^ _state[_bufFilled]);
				for (int i = _bufFilled + 1; i < _buf.Length; i++)
					_buf[i] = (byte)(_k2[i] ^ _state[i]);
			}
			_ct.TransformBlock (_buf, 0, _buf.Length, _state, 0);
			_ct = null;

			byte[] output = new byte[_hashBits >> 3];
			Buffer.BlockCopy (_state, 0, output, 0, output.Length);
			return output;
		}

		public override byte[] Key {
			get { return _algo.Key; }
			set {
				_algo.Key = (byte[])value.Clone ();
				base.Key = _algo.Key;
			}
		}

		public override bool CanReuseTransform {
			get { return true; }
		}

		public override bool CanTransformMultipleBlocks {
			get { return true; }
		}

		public override int HashSize {
			get { return _hashBits; }
		}

		static int MSB_1 (byte[] array)
		{
			return array[0] >> 7;
		}

		static void LeftShift_1 (byte[] array)
		{
			for (int i = 0; i < array.Length - 1; i ++)
				array[i] = (byte)((array[i] << 1) | (array[i + 1] >> 7));
			array[array.Length - 1] <<= 1;
		}
	}
}
