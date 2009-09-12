// 
// Copyright (c) 2008, Kazuki Oikawa
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
using System.Collections.Generic;
using System.Text;

namespace openCrypto.FiniteField
{
	class Number : IComparable<Number>, IComparable
	{
		static string CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyz";
		public uint[] data;
		public int length;
		static Number ZERO = null, ONE = null, TWO = null, THREE = null, FOUR = null, TWENTY_SEVEN = null;

		#region Constructors
		public Number (int capacity)
		{
			data = new uint[capacity];
			length = capacity;
		}

		public Number (uint[] data)
			: this (data, data.Length)
		{
		}

		public Number (uint[] data, int length)
		{
			this.data = data;
			this.length = (data.Length >= length ? length : data.Length);
			Normalize ();
		}

		public unsafe Number (uint* data, int length)
		{
			this.data = new uint [length];
			this.length = length;
			for (int i = 0; i < length; i ++)
				this.data[i] = data[i];
			Normalize ();
		}

		public Number (byte[] data, bool isLittleEndian)
			: this (ToUInt32Array (data, 0, data.Length, isLittleEndian))
		{
		}

		public Number (byte[] data, int offset, int length, bool isLittleEndian)
			: this (ToUInt32Array (data, offset, length, isLittleEndian))
		{
		}

		static uint[] ToUInt32Array (byte[] data, int offset, int length, bool isLittleEndian)
		{
			uint[] tmp = new uint [(length >> 2) + ((length & 3) == 0 ? 0 : 1)];
			if (isLittleEndian) {
				int i = offset, q = 0, last = offset + length;
				for (; i < last - 3; i += 4)
					tmp[q ++] = ((uint)data[i]) | (((uint)data[i + 1]) << 8) | (((uint)data[i + 2]) << 16) | (((uint)data[i + 3]) << 24);
				if (last == i)
					return tmp;
				int diff = last - i;
				if (diff == 1)
					tmp[q] = data[i];
				else if (diff == 2)
					tmp[q] = ((uint)data[i]) | (((uint)data[i + 1]) << 8);
				else
					tmp[q] = ((uint)data[i]) | (((uint)data[i + 1]) << 8) | (((uint)data[i + 2]) << 16);
			} else {
				int i = offset + length - 1, q = 0;
				for (; i >= 3 + offset; i -= 4)
					tmp[q++] = ((uint)data[i]) | (((uint)data[i - 1]) << 8) | (((uint)data[i - 2]) << 16) | (((uint)data[i - 3]) << 24);
				int diff = i - offset;
				if (diff < 0)
					return tmp;
				if (diff == 0)
					tmp[q] = data[i];
				else if (diff == 1)
					tmp[q] = ((uint)data[i]) | (((uint)data[i - 1]) << 8);
				else
					tmp[q] = ((uint)data[i]) | (((uint)data[i - 1]) << 8) | (((uint)data[i - 2]) << 16);
			}
			return tmp;
		}

		public Number (Number x)
		{
			length = x.length;
			data = new uint[length];
			for (int i = 0; i < data.Length; i++)
				data[i] = x.data[i];
		}

		public Number (Number x, uint padding)
		{
			length = x.length;
			data = new uint[length + padding];
			for (int i = 0; i < length; i++)
				data[i] = x.data[i];
		}
		#endregion

		#region Properties
		public static Number Zero {
			get {
				if (ZERO == null)
					ZERO = new Number (new uint[] {0}, 1);
				return ZERO;
			}
		}
		public static Number One {
			get {
				if (ONE == null)
					ONE = new Number (new uint[] {1}, 1);
				return ONE;
			}
		}
		public static Number Two {
			get {
				if (TWO == null)
					TWO = new Number (new uint[] {2}, 1);
				return TWO;
			}
		}
		public static Number Three {
			get {
				if (THREE == null)
					THREE = new Number (new uint[] {3}, 1);
				return THREE;
			}
		}
		public static Number Four {
			get {
				if (FOUR == null)
					FOUR = new Number (new uint[] {4}, 1);
				return FOUR;
			}
		}
		public static Number TwentySeven {
			get {
				if (TWENTY_SEVEN == null)
					TWENTY_SEVEN = new Number (new uint[] {27}, 1);
				return TWENTY_SEVEN;
			}
		}
		#endregion

		#region Basic Operators
		public static unsafe int Add (uint* x, int xlen, uint* y, int ylen, uint* z)
		{
			ulong sum = 0;
			uint* b, s;
			int blen, slen;
			int i = 0;

			if (xlen >= ylen) {
				b = x; blen = xlen;
				s = y; slen = ylen;
			} else {
				b = y; blen = ylen;
				s = x; slen = xlen;
			}

			for (; i < slen; i++) {
				z[i] = (uint)(sum += ((ulong)b[i]) + ((ulong)s[i]));
				sum >>= 32;
			}
			for (; sum > 0 && i < blen; i++) {
				z[i] = (uint)(sum += ((ulong)b[i]));
				sum >>= 32;
			}
			if (sum > 0) {
				z[i++] = (uint)sum;
			} else {
				for (; i < blen; i++)
					z[i] = b[i];
			}
			return i;
		}

		public static unsafe int AddInPlace (uint* x, int xlen, uint* y, int ylen)
		{
			ulong sum = 0;
			int i = 0;

			for (; i < ylen; i++) {
				x[i] = (uint)(sum += ((ulong)x[i]) + ((ulong)y[i]));
				sum >>= 32;
			}
			for (; sum > 0 && i < xlen; i++) {
				x[i] = (uint)(sum += ((ulong)x[i]));
				sum >>= 32;
			}
			if (sum == 0)
				return xlen;

			x[i++] = (uint)sum;
			return i;
		}

		public static unsafe int Add (uint* x, int xlen, uint y, uint* z)
		{
			ulong sum = 0;
			int i = 1;

			z[0] = (uint)(sum = ((ulong)x[0]) + ((ulong)y));
			sum >>= 32;
			for (; sum > 0 && i < xlen; i++) {
				z[i] = (uint)(sum += ((ulong)x[i]));
				sum >>= 32;
			}
			if (sum > 0) {
				z[i] = (uint)sum;
			} else {
				for (; i < xlen; i++)
					z[i] = x[i];
			}
			return i;
		}

		public static unsafe int Subtract (uint* big, int blen, uint* small, int slen, uint* result)
		{
			int i = 0;
			uint carry = 0;
			for (; i < slen; i++) {
				uint tmp = small[i] + carry;
				carry = (tmp < carry | (result[i] = big[i] - tmp) > ~tmp ? 1U : 0U);
			}
			if (carry == 1U) {
				do {
					result[i] = big[i] - 1;
				} while (result[i++] == 0xFFFFFFFF && i < blen);
			}
			for (; i < blen; i++)
				result[i] = big[i];
			if (result[i - 1] == 0)
				return i - 1;
			return i;
		}

		public static unsafe int SubtractInPlace (uint* big, int blen, uint* small, int slen)
		{
			int i = 0;
			uint carry = 0;
			for (; i < slen; i++) {
				uint tmp = small[i] + carry;
				carry = (tmp < carry | (big[i] -= tmp) > ~tmp ? 1U : 0U);
			}
			if (carry == 1U) {
				do {
					big[i] --;
				} while (big[i++] == 0xFFFFFFFF && i < blen);
			}
			while (blen > 1 && big[blen - 1] == 0)
				blen --;
			return blen;
		}

		public unsafe void SubtractInPlace (Number small)
		{
			fixed (uint* px = data, py = small.data) {
				length = SubtractInPlace (px, length, py, small.length);
			}
		}

		public static unsafe uint DivideInPlace (uint* x, int xlen, uint y)
		{
			int i = xlen - 1;
			ulong r = x[i];
			x[i] = (uint)(r / y);
			r %= y;
			while (i-- > 0) {
				r <<= 32;
				r |= x[i];
				x[i] = (uint)(r / y);
				r %= y;
			}
			return (uint)r;
		}

		public static unsafe Number Divide (uint* x, int len, uint y, out uint rem)
		{
			uint[] ret = new uint[len];
			int i = len - 1;
			ulong r = x[i];
			ret[i] = (uint)(r / y);
			r %= y;
			while (i-- > 0) {
				r <<= 32;
				r |= x[i];
				ret[i] = (uint)(r / y);
				r %= y;
			}
			rem = (uint)r;
			return new Number (ret);
		}

		public unsafe uint DivideInPlace (uint y)
		{
			fixed (uint* p = data) {
				return DivideInPlace (p, length, y);
			}
		}

		public unsafe Number Divide (uint y, out uint rem)
		{
			fixed (uint* p = data) {
				return Divide (p, length, y, out rem);
			}
		}

		public static unsafe Number[] Divide (Number x, Number y)
		{
			fixed (uint* px = x.data, py = y.data) {
				return Divide (px, x.length, py, y.length);
			}
		}

		public static unsafe Number[] Divide (uint* x, int xlen, uint* y, int ylen)
		{
			if (ylen == 1) {
				uint hoge;
				return new Number[] { Divide (x, xlen, y[0], out hoge), new Number (new uint[] { hoge }, 1) };
			} else if (xlen == 1 && *x == 0) {
				return new Number[] { Number.Zero, Number.Zero };
			} else if (xlen < ylen) {
				uint[] tmp = new uint[xlen];
				for (int i = 0; i < tmp.Length; i++) tmp[i] = x[i];
				return new Number[] { Number.Zero, new Number (tmp) };
			}

			int remainderLen = xlen + 1, divisorLen = ylen + 1;
			uint mask = 0x80000000;
			uint val = y[ylen - 1];
			int shift = 0;
			int resultPos = xlen - ylen;

			while (mask != 0 && (val & mask) == 0) {
				shift++; mask >>= 1;
			}
			uint[] quot = new uint[xlen - ylen + 1];
			uint[] rem = LeftShift (x, xlen, shift, 1);
			uint[] y2 = LeftShift (y, ylen, shift, 1);

			uint firstDivisorByte = y2[ylen - 1];
			ulong secondDivisorByte = y2[ylen - 2];

			int j = remainderLen - ylen, pos = remainderLen - 1;

			while (j > 0) {
				ulong dividend = ((ulong)rem[pos] << 32) + (ulong)rem[pos - 1];

				ulong q_hat = dividend / (ulong)firstDivisorByte;
				ulong r_hat = dividend % (ulong)firstDivisorByte;

				do {

					if (q_hat == 0x100000000 ||
						(q_hat * secondDivisorByte) > ((r_hat << 32) + rem[pos - 2])) {
						q_hat--;
						r_hat += (ulong)firstDivisorByte;

						if (r_hat < 0x100000000)
							continue;
					}
					break;
				} while (true);

				uint t;
				uint dPos = 0;
				int nPos = pos - divisorLen + 1;
				ulong mc = 0;
				uint uint_q_hat = (uint)q_hat;
				do {
					mc += (ulong)y2[dPos] * (ulong)uint_q_hat;
					t = rem[nPos];
					rem[nPos] -= (uint)mc;
					mc >>= 32;
					if (rem[nPos] > t) mc++;
					dPos++; nPos++;
				} while (dPos < divisorLen);

				nPos = pos - divisorLen + 1;
				dPos = 0;

				// Overestimate
				if (mc != 0) {
					uint_q_hat--;
					ulong sum = 0;

					do {
						sum = ((ulong)rem[nPos]) + ((ulong)y2[dPos]) + sum;
						rem[nPos] = (uint)sum;
						sum >>= 32;
						dPos++; nPos++;
					} while (dPos < divisorLen);

				}

				quot[resultPos--] = (uint)uint_q_hat;

				pos--;
				j--;
			}

			if (shift == 0)
				return new Number[] { new Number (quot), new Number (rem, ylen) };
			return new Number[] { new Number (quot), new Number (RightShift (rem, rem.Length, shift), ylen) };
		}

		public static Number Multiply (Number x, uint y)
		{
			uint[] result = new uint[x.length + 1];
			ulong tmp = 0;
			for (int i = 0; i < x.length; i++) {
				tmp += ((ulong)x.data[i]) * ((ulong)y);
				result[i] = (uint)tmp;
				tmp >>= 32;
			}
			result[x.length] = (uint)tmp;
			return new Number (result);
		}

		public static Number MultiplyAndAdd (Number x, uint plus, uint multiply)
		{
			uint[] result = new uint[x.length + 1];
			ulong tmp = plus;
			for (int i = 0; i < x.length; i++) {
				tmp += ((ulong)x.data[i]) * ((ulong)multiply);
				result[i] = (uint)tmp;
				tmp >>= 32;
			}
			result[x.length] = (uint)tmp;
			return new Number (result);
		}

		public static unsafe Number Multiply (Number x, Number y)
		{
			if (x.length == 1)
				return Multiply (y, x.data[0]);
			if (y.length == 1)
				return Multiply (x, y.data[0]);
			uint[] z = new uint[x.length + y.length];
			fixed (uint* px = x.data, py = y.data, pz = z) {
				Multiply (px, x.length, py, y.length, pz);
			}
			return new Number (z);
		}

		public static unsafe void Multiply (uint* x, int xlen, uint* y, int ylen, uint* z)
		{
			uint* xend = x + xlen, yend = y + ylen;
			for (uint* xx = x, yy = y, zz = z; xx < xend; xx++, zz++) {
				if (*xx == 0)
					continue;
				ulong carry = 0;

				uint* zz2 = zz;
				for (uint* yy2 = yy; yy2 < yend; yy2++, zz2++) {
					carry += ((ulong)*xx * (ulong)*yy2) + (ulong)*zz2;
					*zz2 = (uint)carry;
					carry >>= 32;
				}

				if (carry != 0)
					*zz2 = (uint)carry;
			}
		}

		#endregion

		#region Operator overrides
		public static unsafe Number operator + (Number x, Number y)
		{
			uint[] z = new uint[(x.length >= y.length ? x.length : y.length) + 1];
			fixed (uint* px = x.data, py = y.data, pz = z) {
				return new Number (z, Number.Add (px, x.length, py, y.length, pz));
			}
		}

		public static unsafe Number operator + (Number x, uint y)
		{
			uint[] z = new uint[x.length + 1];
			fixed (uint* px = x.data, pz = z) {
				return new Number (z, Number.Add (px, x.length, y, pz));
			}
		}

		public static unsafe Number operator - (Number x, Number y)
		{
			uint[] z = new uint[x.length >= y.length ? x.length : y.length];
			fixed (uint* px = x.data, py = y.data, pz = z) {
				return new Number (z, Number.Subtract (px, x.length, py, y.length, pz));
			}
		}

		public static unsafe Number operator * (Number x, uint y)
		{
			return Number.Multiply (x, y);
		}

		public static unsafe Number operator * (Number x, Number y)
		{
			return Number.Multiply (x, y);
		}

		public static unsafe Number operator / (Number x, uint y)
		{
			uint rem;
			fixed (uint* px = x.data) {
				return Number.Divide (px, x.length, y, out rem);
			}
		}

		public static unsafe Number operator / (Number x, Number y)
		{
			fixed (uint* px = x.data, py = y.data) {
				return Number.Divide (px, x.length, py, y.length)[0];
			}
		}

		public static unsafe uint operator % (Number x, uint y)
		{
			uint rem;
			fixed (uint* px = x.data) {
				Number.Divide (px, x.length, y, out rem);
				return rem;
			}
		}

		public static unsafe Number operator % (Number x, Number y)
		{
			fixed (uint* px = x.data, py = y.data) {
				return Number.Divide (px, x.length, py, y.length)[1];
			}
		}

		public static unsafe Number operator << (Number x, int shift)
		{
			fixed (uint* px = x.data) {
				return new Number (LeftShift (px, x.length, shift, 0));
			}
		}

		public static unsafe Number operator >> (Number x, int shift)
		{
			return new Number (RightShift (x.data, x.length, shift));
		}

		public static Number operator & (Number x, Number y)
		{
			return x.And (y);
		}

		public static bool operator < (Number x, Number y)
		{
			return x.CompareTo (y) < 0;
		}

		public static bool operator > (Number x, Number y)
		{
			return x.CompareTo (y) > 0;
		}

		public static bool operator <= (Number x, Number y)
		{
			return x.CompareTo (y) <= 0;
		}

		public static bool operator >= (Number x, Number y)
		{
			return x.CompareTo (y) >= 0;
		}
		#endregion

		#region Bit operators
		public unsafe Number LeftShift (int n)
		{
			fixed (uint* p = data) {
				uint[] tmp = LeftShift (p, length, n, 0);
				return new Number (tmp);
			}
		}

		public Number RightShift (int n)
		{
			return new Number (RightShift (data, length, n));
		}

		public static unsafe uint[] LeftShift (uint* data, int len, int n, int padding)
		{
			uint[] ret;
			if (n == 0) {
				ret = new uint[len + padding];
				for (int i = 0; i < len; i++) ret[i] = data[i];
			} else {
				int w = n >> 5;
				n &= ((1 << 5) - 1);
				ret = new uint[len + w + (n == 0 ? 0 : 1) + padding];

				int i = 0;
				if (n != 0) {
					uint carry = 0;
					while (i < len) {
						ret[i + w] = (data[i] << n) | carry;
						carry = data[i] >> (32 - n);
						i++;
					}
					ret[i + w] = carry;
				} else {
					while (i < len) {
						ret[i + w] = data[i];
						i++;
					}
				}
			}
			return ret;
		}

		public static uint[] RightShift (uint[] data, int length, int n)
		{
			if (n == 0) {
				uint[] tmp = new uint[length];
				for (int i = 0; i < length; i++)
					tmp[i] = data[i];
				return tmp;
			} else {
				int w = n >> 5;
				n &= ((1 << 5) - 1);
				uint[] tmp = new uint[length - w];
				int l = tmp.Length;

				if (n != 0) {
					uint x, carry = 0;
					while (l-- > 0) {
						x = data[l + w];
						tmp[l] = (x >> n) | carry;
						carry = x << (32 - n);
					}
				} else {
					while (l-- > 0)
						tmp[l] = data[l + w];
				}
				return tmp;
			}
		}

		public int BitCount ()
		{
			this.Normalize ();

			uint value = data[length - 1];
			uint mask = 0x80000000;
			int bits = 32;

			while (bits > 0 && (value & mask) == 0) {
				bits--;
				mask >>= 1;
			}
			bits += ((length - 1) << 5);

			return bits;
		}

		public uint GetBit (int n)
		{
			uint bytePos = (uint)n >> 5;
			byte bitPos = (byte)(n & 0x1F);
			if (bytePos >= data.Length)
				return 0;
			return (data[bytePos] >> bitPos) & 1;
		}

		public uint GetContinuousBitCount (int start)
		{
			uint counter = 0;
			while (start >= 0 && GetBit (start--) == 1) counter++;
			return counter;
		}

		public void PlusBit (int pos)
		{
			uint bytePos = (uint)pos >> 5;
			uint value = 1U << (pos & 0x1F);
			if (bytePos < data.Length) {
				length = data.Length;
				ulong tmp = ((ulong)data[bytePos]) + (ulong)value;
				data[bytePos] = (uint)tmp;
				tmp >>= 32;
				if (tmp == 0) {
					Normalize ();
					return;
				}
				uint i = bytePos + 1;
				for (; i < length; i ++)
					if (++data[i] != 0) {
						Normalize ();
						return;
					}
				if (i < data.Length) {
					data[i] = 1;
					Normalize ();
					return;
				}
				value = 1;
				bytePos = (uint)data.Length;
			}
			uint[] data2 = new uint[bytePos + 1];
			for (int i = 0; i < data.Length; i ++)
				data2[i] = data[i];
			data2[bytePos] = value;
			data = data2;
			length = data.Length;
			Normalize ();
			return;
		}

		public Number And (Number mask)
		{
			uint[] tmp = new uint[Math.Min (length, mask.length)];
			for (int i = 0; i < tmp.Length; i ++)
				tmp[i] = data[i] & mask.data[i];
			return new Number (tmp);
		}

		#endregion

		#region Misc
		public void Normalize ()
		{
			while (length > 0 && data[length - 1] == 0) length--;
			if (length == 0)
				length = 1;
		}
		public static unsafe bool IsZero (uint* x, int len)
		{
			for (int i = len - 1; i >= 0; i--)
				if (x[i] != 0)
					return false;
			return true;
		}
		public bool IsZero ()
		{
			return length == 1 && data[0] == 0;
		}

		public bool IsOne ()
		{
			return length == 1 && data[0] == 1;
		}

		public static Number CreateRandomElement (Number max)
		{
			int bits = max.BitCount ();
			byte[] raw = new byte [bits >> 3];
			while (true) {
				RNG.Instance.GetBytes (raw);
				Number ret = new Number (raw, true);
				if (max.CompareTo (ret) > 0)
					return ret;
			}
		}

		public void CopyTo (byte[] array, int offset)
		{
			int bitCount = BitCount ();
			int byteCount = (bitCount >> 3) + ((bitCount & 7) == 0 ? 0 : 1);
			int len = byteCount >> 2;
			int diff = byteCount & 3;
			int i = 0;
			for (; i < len; i++, offset += 4) {
				array[offset] = (byte)(data[i]);
				array[offset + 1] = (byte)(data[i] >> 8);
				array[offset + 2] = (byte)(data[i] >> 16);
				array[offset + 3] = (byte)(data[i] >> 24);
			}
			if (diff == 0)
				return;
			if (diff == 3)
				array[offset + 2] = (byte)(data[i] >> 16);
			if (diff >= 2)
				array[offset + 1] = (byte)(data[i] >> 8);
			array[offset] = (byte)(data[i]);
		}

		public void CopyToBigEndian (byte[] array, int offset, int maxLenBytes)
		{
			int bitCount = BitCount ();
			int byteCount = (bitCount >> 3) + ((bitCount & 7) == 0 ? 0 : 1);
			int len = byteCount >> 2;
			int diff = byteCount & 3;
			int i = 0;
			offset += byteCount - 1 + (maxLenBytes - byteCount);
			for (; i < len; i++, offset -= 4) {
				array[offset] = (byte)(data[i]);
				array[offset - 1] = (byte)(data[i] >> 8);
				array[offset - 2] = (byte)(data[i] >> 16);
				array[offset - 3] = (byte)(data[i] >> 24);
			}
			if (diff == 0)
				return;
			if (diff == 3)
				array[offset - 2] = (byte)(data[i] >> 16);
			if (diff >= 2)
				array[offset - 1] = (byte)(data[i] >> 8);
			array[offset] = (byte)(data[i]);
		}

		public byte[] ToByteArray (int byteLen, bool isLittleEndian)
		{
			byte[] bytes = new byte [byteLen];
			if (isLittleEndian)
				CopyTo (bytes, 0);
			else
				CopyToBigEndian (bytes, 0, byteLen);
			return bytes;
		}
		#endregion

		#region Parse
		public static Number Parse (string text, uint radix)
		{
			Number result = Number.Zero;
			text = text.ToLower ();
			for (int i = 0; i < text.Length; i++) {
				int idx = CHARACTERS.IndexOf (text[i]);
				if (idx < 0) throw new FormatException ();
				result = Number.MultiplyAndAdd (result, (uint)idx, radix);
			}
			return result;
		}
		#endregion

		#region Overrides
		public override int GetHashCode ()
		{
			int hash = (int)data[0];
			for (int i = 1; i < length; i++)
				hash ^= (int)data[i];
			return hash;
		}

		public string ToString (uint radix)
		{
			return ToString (radix, CHARACTERS);
		}

		public unsafe string ToString (uint radix, string characters)
		{
			int bufLen = length << 5;
			int pos = bufLen;
			char* buffer = stackalloc char[bufLen];
			uint* tmp = stackalloc uint[(int)length];
			for (int i = 0; i < length; i++)
				tmp[i] = data[i];

			while (!IsZero (tmp, length))
				buffer[--pos] = characters[(int)DivideInPlace (tmp, length, radix)];
			if (pos == bufLen)
				buffer[--pos] = '0';
			return new string (buffer, pos, bufLen - pos);
		}

		public override string ToString ()
		{
			return ToString (10);
		}
		#endregion

		#region IComparable<Number> Members

		public int CompareTo (object obj)
		{
			return CompareTo ((Number)obj);
		}

		public int CompareTo (Number other)
		{
			return Compare (data, length, other.data, other.length);
		}

		public static int Compare (uint[] x, int xlen, uint[] y, int ylen)
		{
			int ret = xlen.CompareTo (ylen);
			if (ret != 0)
				return ret;
			int idx = xlen - 1;
			while (idx != 0 && x[idx] == y[idx]) idx--;

			if (x[idx] > y[idx])
				return 1;
			else if (x[idx] < y[idx])
				return -1;
			else
				return 0;
		}

		#endregion
	}
}
