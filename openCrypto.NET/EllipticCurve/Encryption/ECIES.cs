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
		SymmetricAlgorithm _symmetricAlgo = null;

		public ECIES (ECDomainNames name)
		{
			_domain = ECDomains.GetDomainParameter (name);
			_kdf = new ANSI_X963_KDF (new SHA1Managed ());
			_params = new ECIESParameters (_domain);
			_mac = new HMACSHA1 ();
		}

		public ECIES (ECDomainNames name, SymmetricAlgorithm symmetricAlgo) : this (name)
		{
			_symmetricAlgo = symmetricAlgo;
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
			int encBlockBytes = (_symmetricAlgo == null ? 0 : _symmetricAlgo.BlockSize >> 3);
			int encKeyLen = (_symmetricAlgo == null ? value.Length : _symmetricAlgo.KeySize >> 3);
			int encPaddingLen = 0;
			int encTotalBytes = value.Length;
			int macKeyLen = _mac.HashSize >> 3;
			byte[] result;
			int ridx = 0;

			if (_params.D == null)
				_params.CreateNewPrivateKey ();
			if (_params.Q == null)
				_params.CreatePublicKeyFromPrivateKey ();
			if (_symmetricAlgo != null) {
				int mod = value.Length % encBlockBytes;
				int rmod = encBlockBytes - mod;
				if (mod == 0) {
					if (!(_symmetricAlgo.Padding == PaddingMode.None || _symmetricAlgo.Padding == PaddingMode.Zeros))
						encPaddingLen = _symmetricAlgo.BlockSize >> 3;
				} else {
					encPaddingLen = rmod;
				}
				encTotalBytes += encPaddingLen;
			}

			// Step.1
#if !TEST
			ECKeyPair pair = new ECKeyPair (null, null, _domain);
#else
			ECKeyPair pair = new ECKeyPair (new Number (randomK, false), null, _domain);
#endif
			
			// Step.2
			// TODO: 点圧縮を利用しないオプションを追加する
			byte[] R = pair.ExportPublicKey (true);
			result = new byte[R.Length + encTotalBytes + macKeyLen];
			for (int i = 0; i < R.Length; i ++) result[ridx ++] = R[i];

			// Step.3 & 4
			// TODO: Cofactor Diffie-Hellmanプリミティブを利用するオプションを追加する
			byte[] z = _params.Q.Multiply (pair.D).Export ().X.ToByteArray (domainLen, false);

			// Step.5
			byte[] K = _kdf.Calculate (z, encKeyLen + macKeyLen);

			// Step.6
			byte[] MK = new byte[macKeyLen];
			for (int i = 0; i < MK.Length; i++)
				MK[i] = K[K.Length - MK.Length + i];

			// Step.7
			if (_symmetricAlgo == null) {
				for (int i = 0; i < value.Length; i++)
					result[ridx++] = (byte)(value[i] ^ K[i]);
			} else {
				byte[] EK = new byte[encKeyLen];
				for (int i = 0; i < EK.Length; i ++)
					EK[i] = K[i];
				using (ICryptoTransform transform = _symmetricAlgo.CreateEncryptor (EK, new byte[encBlockBytes])) {
					int i = 0;
					for (; i < value.Length - encBlockBytes; i += encBlockBytes)
						transform.TransformBlock (value, i, encBlockBytes, result, ridx + i);
					byte[] padding = transform.TransformFinalBlock (value, i, value.Length - i);
					Buffer.BlockCopy (padding, 0, result, ridx + i, padding.Length);
					ridx += i + padding.Length;
				}
			}

			// Step.8
			// TODO: HMAC-SHA1-80への対応
			_mac.Key = MK;
			_mac.Initialize ();
			_mac.TransformBlock (result, R.Length, encTotalBytes, null, 0);
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
			int encKeyLen = (_symmetricAlgo == null ? EM.Length : _symmetricAlgo.KeySize >> 3);;

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
			byte[] EK = null;
			if (_symmetricAlgo != null) {
				EK = new byte[encKeyLen];
				for (int i = 0; i < EK.Length; i ++)
					EK[i] = K[i];
			}
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
			byte[] result = new byte[EM.Length];
			if (_symmetricAlgo == null) {
				for (int i = 0; i < result.Length; i ++)
					result[i] = (byte)(EM[i] ^ K[i]);
			} else {
				int blockBytes = _symmetricAlgo.BlockSize >> 3;
				using (ICryptoTransform transform = _symmetricAlgo.CreateDecryptor (EK, new byte[blockBytes])) {
					int i = 0;
					for (; i < result.Length - blockBytes; i += blockBytes)
						transform.TransformBlock (EM, i, blockBytes, result, i);
					byte[] temp = transform.TransformFinalBlock (EM, i, EM.Length - i);
					Buffer.BlockCopy (temp, 0, result, i, temp.Length);
					if (temp.Length != blockBytes)
						Array.Resize<byte> (ref result, i + temp.Length);
				}
			}
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
