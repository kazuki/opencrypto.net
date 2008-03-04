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
using System.Security.Cryptography;
using openCrypto.KeyDerivationFunctions;
using openCrypto.FiniteField;

namespace openCrypto.EllipticCurve.Encryption
{
	/// <summary>
	/// ECIES (Ellipic Curve Integrated Encryption Scheme)
	/// 
	/// Reference:
	///    SEC1, 5.1 (p40 - p44)
	/// </summary>
	public class ECIES : AsymmetricAlgorithm
	{
		KeyDerivationFunction _kdf;
		ECDomainParameters _domain;
		ECIESParameters _params;
		KeyedHashAlgorithm _mac;
		byte[] _sharedInfo = null;

		public ECIES (ECDomainNames name)
		{
			_domain = ECDomains.GetDomainParameter (name);
			_kdf = new ANSI_X963_KDF (new SHA1Managed ());
			_params = new ECIESParameters (_domain);
			_mac = new HMACSHA1 ();
		}

		public byte[] Encrypt (byte[] value)
#if TEST
		{
			ECKeyPair pair = new ECKeyPair (null, null, _domain);
			return Encrypt (value, pair.PrivateKey);
		}

		public byte[] Encrypt (byte[] value, byte[] randomK)
#endif
		{
			int domainLen = (int)((_domain.Bits >> 3) + ((_domain.Bits & 7) == 0 ? 0 : 1));
			int encKeyLen = value.Length; // TODO: 3-DESなどの場合はencKeyLenを共通鍵のサイズとなるように変更する
			int macKeyLen = _mac.HashSize >> 3;
			byte[] result;
			int ridx = 0;

			if (_params.D == null)
				_params.CreateNewPrivateKey ();
			if (_params.Q == null)
				_params.CreatePublicKeyFromPrivateKey ();

			// Step.1
#if !TEST
			ECKeyPair pair = new ECKeyPair (null, null, _domain);
#else
			ECKeyPair pair = new ECKeyPair (new Number (randomK, false), null, _domain);
#endif
			
			// Step.2
			// TODO: 点圧縮を利用しないオプションを追加する
			byte[] R = pair.ExportPublicKey (true);
			result = new byte[R.Length + value.Length + macKeyLen];
			for (int i = 0; i < R.Length; i ++) result[ridx ++] = R[i];

			// Step.3 & 4
			// TODO: Cofactor Diffie-Hellmanプリミティブを利用するオプションを追加する
			byte[] z = _params.Q.Multiply (pair.D).Export ().X.ToByteArray (domainLen, false);

			// Step.5
			byte[] K = _kdf.Calculate (z, encKeyLen + macKeyLen);

			// Step.6
			// TODO: 3-DESなどの対称鍵暗号を利用する場合はコメントアウトされている以下のコードを利用して鍵を取得する
			// byte[] EK = new byte[encKeyLen];
			// for (int i = 0; i < EK.Length; i ++)
			// 	EK[i] = K[i];
			byte[] MK = new byte[macKeyLen];
			for (int i = 0; i < MK.Length; i ++)
				MK[i] = K[K.Length - MK.Length + i];

			// Step.7
			// TODO: XOR以外の暗号アルゴリズムを利用可能にする
			for (int i = 0; i < value.Length; i ++)
				result[ridx++] = (byte)(value[i] ^ K[i]);

			// Step.8
			// TODO: HMAC-SHA1-80への対応
			_mac.Key = MK;
			_mac.Initialize ();
			_mac.TransformBlock (result, R.Length, value.Length, null, 0);
			if (_sharedInfo == null)
				_mac.TransformFinalBlock (result, 0, 0);
			else
				_mac.TransformFinalBlock (_sharedInfo, 0, _sharedInfo.Length);
			_mac.Hash.CopyTo (result, ridx);

			return result;
		}

		public byte[] Decrypt (byte[] value)
		{
			if (_params.Q == null) {
				if (_params.D == null)
					throw new CryptographicException ();
				_params.CreatePublicKeyFromPrivateKey ();
			}

			int domainLen = (int)((_domain.Bits >> 3) + ((_domain.Bits & 7) == 0 ? 0 : 1));
			int macKeyLen = _mac.HashSize >> 3;
			
			// Step.1
			if (value[0] != 2 && value[0] != 3 && value[0] != 4)
				throw new CryptographicException ();
			byte[] RBytes = new byte[domainLen + 1];
			byte[] EM = new byte[value.Length - RBytes.Length - macKeyLen];
			byte[] D = new byte[macKeyLen];
			if (value.Length != RBytes.Length + EM.Length + D.Length)
				throw new CryptographicException ();
			Array.Copy (value, 0, RBytes, 0, RBytes.Length);
			Array.Copy (value, RBytes.Length, EM, 0, EM.Length);
			Array.Copy (value, RBytes.Length + EM.Length, D, 0, D.Length);
			int encKeyLen = EM.Length; // TODO: 3-DESなどの場合はencKeyLenを共通鍵のサイズとなるように変更する

			// Step.2
			ECPoint R = new ECPoint (_domain.Group, RBytes);

			// Step.3
			// TODO: Step.3

			// Step.4 & 5
			// TODO: Cofactor Diffie-Hellmanプリミティブを利用するオプションを追加する
			byte[] Z = R.Multiply (_params.D).Export ().X.ToByteArray (domainLen, false);

			// Step.6
			byte[] K = _kdf.Calculate (Z, encKeyLen + macKeyLen);

			// Step.7
			// TODO: 3-DESなどの対称鍵暗号を利用する場合はコメントアウトされている以下のコードを利用して鍵を取得する
			// byte[] EK = new byte[encKeyLen];
			// for (int i = 0; i < EK.Length; i ++)
			// 	EK[i] = K[i];
			byte[] MK = new byte[macKeyLen];
			for (int i = 0; i < MK.Length; i++)
				MK[i] = K[K.Length - MK.Length + i];

			// Step.8
			// TODO: HMAC-SHA1-80への対応
			_mac.Key = MK;
			_mac.Initialize ();
			_mac.TransformBlock (EM, 0, EM.Length, null, 0);
			if (_sharedInfo == null)
				_mac.TransformFinalBlock (EM, 0, 0);
			else
				_mac.TransformFinalBlock (_sharedInfo, 0, _sharedInfo.Length);
			byte[] hash = _mac.Hash;
			for (int i = 0; i < hash.Length; i ++)
				if (hash[i] != D[i])
					throw new CryptographicException ();

			// Step.9
			// TODO: XOR以外の暗号アルゴリズムを利用可能にする
			byte[] result = new byte [EM.Length];
			for (int i = 0; i < result.Length; i ++)
				result[i] = (byte)(EM[i] ^ K[i]);

			return result;
		}

		#region Key import/export
		public override void FromXmlString (string xmlString)
		{
			throw new Exception ("The method or operation is not implemented.");
		}

		public override string ToXmlString (bool includePrivateParameters)
		{
			throw new Exception ("The method or operation is not implemented.");
		}
		#endregion

		#region Misc
		public ECIESParameters Parameters {
			get { return _params; }
		}

		public byte[] SharedInfo1 {
			get { return _kdf.SharedInfo; }
			set { _kdf.SharedInfo = value;}
		}

		public byte[] SharedInfo2 {
			get { return _sharedInfo; }
			set { _sharedInfo = value; }
		}

		protected override void Dispose (bool disposing)
		{
		}

		public override string KeyExchangeAlgorithm {
			get { return null; }
		}

		public override string SignatureAlgorithm {
			get { return null; }
		}
		#endregion
	}
}
