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
	public class ShortKeyedHashAlgorithm : KeyedHashAlgorithm
	{
		KeyedHashAlgorithm _base;
		int _hashBits;

		public ShortKeyedHashAlgorithm (KeyedHashAlgorithm baseAlgo, int outputHashBits)
		{
			_base = baseAlgo;
			_hashBits = outputHashBits;
			if (outputHashBits <= 0 || _base.HashSize < outputHashBits)
				throw new ArgumentOutOfRangeException ();
		}

		protected override void HashCore (byte[] array, int ibStart, int cbSize)
		{
			_base.TransformBlock (array, ibStart, cbSize, null, 0);
		}

		protected override byte[] HashFinal ()
		{
			_base.TransformFinalBlock (new byte[0], 0, 0);
			byte[] output = new byte [_hashBits >> 3];
			Buffer.BlockCopy (_base.Hash, 0, output, 0, output.Length);
			return output;
		}

		public override void Initialize ()
		{
			_base.Initialize ();
		}

		public override int HashSize {
			get { return _hashBits; }
		}

		public override bool CanReuseTransform {
			get { return _base.CanReuseTransform; }
		}

		public override bool CanTransformMultipleBlocks {
			get { return _base.CanTransformMultipleBlocks; }
		}

		public override int InputBlockSize {
			get { return _base.InputBlockSize; }
		}

		public override int OutputBlockSize {
			get { return _base.OutputBlockSize; }
		}

		public override byte[] Key {
			get { return _base.Key; }
			set { _base.Key = value;}
		}

		protected override void Dispose (bool disposing)
		{
			(_base as IDisposable).Dispose ();
		}
	}
}
