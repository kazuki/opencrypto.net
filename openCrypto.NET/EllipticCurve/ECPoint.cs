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
using openCrypto.FiniteField;

namespace openCrypto.EllipticCurve
{
	class ECPoint
	{
		ECGroup _group;
		IFiniteField _field;
		Number _x, _y, _z;
		Number _x2 = null, _z2 = null, _z3 = null, _y2 = null, _y4 = null;
		ECPoint[] _multiplyHelperPoints = null;
		ECPoint _inv = null;

		const int MultiplyWindowSize = 5;

		public ECPoint (ECGroup group, Number x, Number y, Number z)
		{
			_group = group;
			_field = group.FiniteField;
			_x = x;
			_y = y;
			_z = z;
		}

		public ECGroup Group {
			get { return _group; }
		}

		public Number X {
			get { return _x; }
		}

		public Number Y {
			get { return _y; }
		}

		public Number Z {
			get { return _z; }
		}

		public bool IsInifinity ()
		{
			return _z.IsZero ();
		}

		public ECPoint Double ()
		{
			if (this.IsInifinity ())
				return this;
			if (_x2 == null) _x2 = _field.Multiply (_x, _x);
			if (_z2 == null) _z2 = _field.Multiply (_z, _z);
			if (_y2 == null) _y2 = _field.Multiply (_y, _y);
			if (_y4 == null) _y4 = _field.Multiply (_y2, _y2);

			Number l1 = _field.Multiply (_field.Subtract (_x, _z2), _field.Add (_x, _z2));
			l1 = _field.Add (_field.Add (l1, l1), l1);

			Number Z = _field.Multiply (_y, _z);
			Z = _field.Add (Z, Z);

			Number l2 = _field.Multiply (_x, _y2);
			l2 = _field.Add (l2, l2);
			l2 = _field.Add (l2, l2);

			Number X = _field.Subtract (_field.Multiply (l1, l1), _field.Add (l2, l2));

			Number l3 = _field.Add (_y4, _y4);
			l3 = _field.Add (l3, l3);
			l3 = _field.Add (l3, l3);

			Number Y = _field.Subtract (_field.Multiply (l1, _field.Subtract (l2, X)), l3);

			return new ECPoint (_group, X, Y, Z);
		}

		public ECPoint Add (ECPoint other)
		{
			if (this.IsInifinity ())
				return other;
			if (other.IsInifinity ())
				return this;
			Number z1p2 = this._z2, z2p2 = other._z2, z1p3 = this._z3, z2p3 = other._z3;
			if (z1p2 == null) this._z2 = z1p2 = _field.Multiply (_z, _z);
			if (z2p2 == null) other._z2 = z2p2 = _field.Multiply (other._z, other._z);
			if (z1p3 == null) this._z3 = z1p3 = _field.Multiply (z1p2, this._z);
			if (z2p3 == null) other._z3 = z2p3 = _field.Multiply (z2p2, other._z);

			Number u1 = _field.Multiply (_x, z2p2);
			Number u2 = _field.Multiply (other._x, z1p2);
			Number H = _field.Subtract (u2, u1);

			Number s1 = _field.Multiply (_y, z2p3);
			Number s2 = _field.Multiply (other._y, z1p3);
			Number r = _field.Subtract (s2, s1);
			if (H.IsZero ()) {
				if (r.IsZero ())
					return Double ();
				return _field.GetInfinityPoint (_group);
			}

			Number H2 = _field.Multiply (H, H);
			Number H3 = _field.Multiply (H2, H);
			Number X = _field.Subtract (_field.Subtract (_field.Multiply (r, r), H3), _field.Multiply (_field.Add (u1, u1), H2));
			Number Y = _field.Subtract (_field.Multiply (r, _field.Subtract (_field.Multiply (u1, H2), X)), _field.Multiply (s1, H3));
			Number Z = _field.Multiply (_field.Multiply (_z, other._z), H);

			return new ECPoint (_group, X, Y, Z);
		}

		ECPoint[] SetupMultiplyHelperPoints ()
		{
			if (_multiplyHelperPoints != null)
				return _multiplyHelperPoints;
			ECPoint[] P = _multiplyHelperPoints = new ECPoint [1 << (MultiplyWindowSize - 1)];
			P[1] = this;
			P[2] = this.Double ();
			for (int i = 3; i < P.Length; i += 2)
				P[i] = P[i - 2].Add (P[2]);
			return P;
		}

		static unsafe int ComputeSignedWindowDecomposition (Number scaler, int *b, int *e)
		{
			int l = scaler.BitCount () - 1;
			int pow2w = 1 << MultiplyWindowSize;
			int pow2whalf = 1 << (MultiplyWindowSize - 1);
			int d = 0, j = 0;
			while (j <= l) {
				if (scaler.GetBit (j) == 0) {
					j++;
				} else {
					int t = j + MultiplyWindowSize - 1;
					if (t > l) t = l;
					uint h = 0;
					for (int q = t; q >= j; q--)
						h = (h << 1) + scaler.GetBit (q);
					if (h > pow2whalf) {
						b[d] = (int)h - pow2w;
						scaler.PlusBit (t + 1);
						l = scaler.BitCount () - 1;
					} else {
						b[d] = (int)h;
					}
					e[d] = j;
					d++;
					j = t + 1;
				}
			}
			return d;
		}

		public unsafe ECPoint Multiply (Number scaler)
		{
			ECPoint[] P = SetupMultiplyHelperPoints ();
#if true
			scaler = new Number (scaler, 1);
			int l = scaler.BitCount () - 1;
			int *b = stackalloc int [l >> 2];
			int* e = stackalloc int[l >> 2];
			int d = ComputeSignedWindowDecomposition (scaler, b, e);
			ECPoint Q = P[b[d - 1]];
			for (int i = d - 2; i >= 0; i --) {
				for (int k = 0; k < e[i + 1] - e[i]; k ++)
					Q = Q.Double ();
				if (b[i] > 0)
					Q = Q.Add (P[b[i]]);
				else
					Q = Q.Add (P[-b[i]].Invert ());
			}
			for (int k = 0; k < e[0]; k++)
				Q = Q.Double ();
#else
#if true
			if (P == null) {
				P = _multiplyHelperPoints = new ECPoint[16];
				P[1] = this;
				P[2] = this.Double ();
				P[3] = P[1].Add (P[2]);
				P[5] = P[3].Add (P[2]);
				P[7] = P[5].Add (P[2]);
				P[9] = P[7].Add (P[2]);
				P[11] = P[9].Add (P[2]);
				P[13] = P[11].Add (P[2]);
				P[15] = P[13].Add (P[2]);
			}
			int j = scaler.BitCount () - 1;
			ECPoint Q = _field.GetInfinityPoint (_group);
			while (j >= 0) {
				if (scaler.GetBit (j) == 0) {
					Q = Q.Double ();
					j--;
				} else {
					int n = j - 1;
					uint h = (1 << 3) | (scaler.GetBit (n--) << 2) | (scaler.GetBit (n--) << 1) | scaler.GetBit (n);
					int t = j - 3;
					while ((h & 1) == 0) {
						h >>= 1;
						t++;
					}
					for (int i = 1; i <= j - t + 1; i++)
						Q = Q.Double ();
					Q = Q.Add (P[(int)h]);
					j = t - 1;
				}
			}
#else
			ECPoint inv = Invert ();
			if (P == null) {
				P = _multiplyHelperPoints = new ECPoint[16];
				P[1] = this;
				P[2] = this.Double ();
				P[3] = P[1].Add (P[2]);
				P[5] = P[3].Add (P[2]);
				P[7] = P[5].Add (P[2]);
				P[9] = P[7].Add (P[2]);
				P[11] = P[9].Add (P[2]);
				P[13] = P[11].Add (P[2]);
				P[15] = P[13].Add (P[2]);
			}
			int j = scaler.BitCount () - 1;
			ECPoint Q = _field.GetInfinityPoint (_group);
			while (j >= 0) {
				uint continuous = scaler.GetContinuousBitCount (j);
				if (continuous == 0) {
					Q = Q.Double ();
					j--;
				} else if (continuous <= 4) {
					int n = j - 1;
					uint h = (1 << 3) | (scaler.GetBit (n--) << 2) | (scaler.GetBit (n--) << 1) | scaler.GetBit (n);
					int t = j - 3;
					while ((h & 1) == 0) {
						h >>= 1;
						t++;
					}
					for (int i = 1; i <= j - t + 1; i++)
						Q = Q.Double ();
					Q = Q.Add (P[(int)h]);
					j = t - 1;
				} else {
					Q = Q.Add (this);
					for (uint i = 0; i < continuous; i ++)
						Q = Q.Double ();
					Q = Q.Add (inv);
					j -= (int)continuous;
				}
			}
#endif
#endif
			return Q;
		}

		public static unsafe ECPoint MultiplyAndAdd (ECPoint p1, Number scaler1, ECPoint p2, Number scaler2)
		{
#if true
			int l = scaler1.BitCount ();
			if (l < scaler2.BitCount ())
				l = scaler2.BitCount ();
			int* b1 = stackalloc int[l >> 2];
			int* b2 = stackalloc int[l >> 2];
			int* e1 = stackalloc int[l >> 2];
			int* e2 = stackalloc int[l >> 2];
			int d1 = ComputeSignedWindowDecomposition (scaler1, b1, e1) - 1;
			int d2 = ComputeSignedWindowDecomposition (scaler2, b2, e2) - 1;
			ECPoint[] p1ary = p1.SetupMultiplyHelperPoints ();
			ECPoint[] p2ary = p2.SetupMultiplyHelperPoints ();
			int lastE;
			ECPoint Q;
			if (e1[d1] == e2[d2]) {
				Q = p1ary[b1[d1]].Add (p2ary[b2[d2--]]);
				lastE = e1[d1--];
			} else if (e1[d1] > e2[d2]) {
				Q = p1ary[b1[d1]];
				lastE = e1[d1--];
			} else {
				Q = p2ary[b2[d2]];
				lastE = e2[d2--];
			}
			while (d1 >= 0 || d2 >= 0) {
				int nextE;
				int nextType = 0;
				if (d1 >= 0 && d2 >= 0 && e1[d1] == e2[d2]) {
					nextE = e1[d1];
					nextType = 0;
				} else if ((d1 >= 0 && d2 < 0) || (d1 >= 0 && e1[d1] > e2[d2])) {
					nextE = e1[d1];
					nextType = 1;
				} else {
					nextE = e2[d2];
					nextType = 2;
				}
				for (int k = 0; k < lastE - nextE; k ++)
					Q = Q.Double ();
				if (nextType == 0 || nextType == 1) {
					if (b1[d1] > 0)
						Q = Q.Add (p1ary[b1[d1--]]);
					else
						Q = Q.Add (p1ary[-b1[d1--]].Invert ());
				}
				if (nextType == 0 || nextType == 2) {
					if (b2[d2] > 0)
						Q = Q.Add (p2ary[b2[d2--]]);
					else
						Q = Q.Add (p2ary[-b2[d2--]].Invert ());
				}
				lastE = nextE;
			}
			for (int k = 0; k < lastE; k++)
				Q = Q.Double ();
			return Q;
#else
			int l = scaler1.BitCount ();
			if (l < scaler2.BitCount ())
				l = scaler2.BitCount ();
			ECPoint Z = p1.Add (p2);
			ECPoint R = p1._field.GetInfinityPoint (p1._group);
			for (int i = l - 1; i >= 0; i --) {
				R = R.Double ();
				uint ki = scaler1.GetBit (i);
				uint li = scaler2.GetBit (i);
				if (ki == 0) {
					if (li == 1)
						R = R.Add (p2);
				} else {
					if (li == 0)
						R = R.Add (p1);
					else
						R = R.Add (Z);
				}
			}
			return R;
#endif
		}

		public ECPoint Export ()
		{
			if (IsInifinity ())
				return _field.GetInfinityPoint (_group);
			return _field.ExportECPoint (_x, _y, _z, _group);
		}

		public ECPoint Invert ()
		{
			if (_inv != null)
				return _inv;
			return _inv = new ECPoint (_group, _x, _field.Subtract (_field.Modulus, _y), _z);;
		}
	}
}
