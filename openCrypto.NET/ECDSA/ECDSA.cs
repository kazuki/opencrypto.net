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
using openCrypto.EllipticCurve;

namespace openCrypto.ECDSA
{
	public class ECDSA
	{
		ECDSAParameters _param;

		public ECDSA (ECDSAParameters param)
		{
			_param = param;
		}

		public ECDSAParameters Parameters {
			get { return _param; }
		}

		public Number[] Sign (Number e)
		{
			Number r, r2, s, k;
			IFiniteField field = _param.Domain.FieldN;
			do {
				do {
					// Step.1
					k = Number.CreateRandomElement (_param.Domain.N);

					// Step.2
					ECPoint tmp = _param.Domain.G.Multiply (k).Export ();

					// Step.3
					r = tmp.X;
					if (!r.IsZero ())
						break;
				} while (true);

				// Step.4
				k = field.Invert (field.ToElement (k));

				// Step.6
				r2 = field.ToElement (r);
				e = field.ToElement (e);
				s = field.Multiply (k, field.Add (e, field.Multiply (r2, _param.D)));
				if (!s.IsZero ()) {
					s = field.ToNormal (s);
					break;
				}
			} while (true);
			return new Number[] { r, s };
		}

		public bool Verify (Number[] sign, Number e)
		{
			Number r = sign[0], s = sign[1];
			IFiniteField field = _param.Domain.FieldN;

			// Step.1
			e = field.ToElement (e);
			s = field.ToElement (s);
			Number r2 = field.ToElement (r);

			// Step.2
			Number w = field.Invert (s);

			// Step.3
			Number u1 = field.ToNormal (field.Multiply (e, w));
			Number u2 = field.ToNormal (field.Multiply (r2, w));

			// Step.4
			//ECPoint X = _param.Domain.G.Multiply (u1).Add (_param.Q.Multiply (u2));
			ECPoint X;
			if (u1.IsZero ())
				X = _param.Domain.FieldN.GetInfinityPoint (_param.Domain.Group) .Add (_param.Q.Multiply (u2));
			else
				X = ECPoint.MultiplyAndAdd (_param.Domain.G, u1, _param.Q, u2);

			// Step.5
			if (X.IsInifinity ())
				return false;
			X = X.Export ();

			// Step.6
			return r.CompareTo (X.X) == 0;
		}
	}
}
