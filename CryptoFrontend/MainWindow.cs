using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using openCrypto;
using openCrypto.EllipticCurve;
using openCrypto.EllipticCurve.Encryption;
using openCrypto.EllipticCurve.KeyAgreement;
using openCrypto.EllipticCurve.Signature;

namespace CryptoFrontend
{
	public partial class MainWindow : Form
	{
		public MainWindow ()
		{
			InitializeComponent ();
			cbKeyType.SelectedIndex = 8;
			this.MinimumSize = this.Size;
			txtGeneratedKey.Font = new Font (FontFamily.GenericMonospace, (int)(Font.Size + 1.5), Font.Unit);
			txtGeneratedPublicKey.Font = txtGeneratedKey.Font;
			txtGeneratedKeyPass.Font = txtGeneratedKey.Font;
			txtEncryptionKey.Font = txtGeneratedKey.Font;
			tabControl1.SelectedTab = tabPage2;
		}

		#region PublicKey - KeyGeneration Tab
		private void btnKeyGenerate_Click (object sender, EventArgs e)
		{
			ECDomainNames domain = (ECDomainNames)cbKeyType.SelectedIndex;
			ECDSA dsa = new ECDSA (domain);
			string domainName = domain.ToString ().Substring(4);
			byte[] privateKeyBytes = dsa.Parameters.PrivateKey;
			if (txtGeneratedKeyPass.Text.Length > 0) {
				byte[] pass = ComputeHash (new SHA256Managed (), Encoding.UTF8.GetBytes (txtGeneratedKeyPass.Text), true);
				byte[] iv = ComputeHash (new SHA1Managed (), Encoding.UTF8.GetBytes (txtGeneratedKeyPass.Text), true);
				Array.Resize<byte> (ref iv, 128 >> 3);
				byte[] encrypted = Encrypt (new CamelliaManaged (), CipherMode.CBC, pass, iv, privateKeyBytes);
				string privateKey = Convert.ToBase64String (encrypted);
				txtGeneratedKey.Text = "camellia256=" + domainName + "=" + privateKey;
			} else {
				string privateKey = Convert.ToBase64String (privateKeyBytes);
				txtGeneratedKey.Text = domainName + "=" + privateKey;
			}
			string publicKey = Convert.ToBase64String (dsa.Parameters.ExportPublicKey (domain != ECDomainNames.secp224r1 ? true : false));
			txtGeneratedPublicKey.Text = domainName + "=" + publicKey;
		}

		private void btnPublicKeyGenerate_Click (object sender, EventArgs e)
		{
			try {
				ECDomainNames domain;
				byte[] privateKey = ParsePrivateKey (txtGeneratedKey.Text, txtGeneratedKeyPass.Text, out domain);
				ECDSA ecdsa = new ECDSA (domain);
				ecdsa.Parameters.PrivateKey = privateKey;
				string publicKey = Convert.ToBase64String (ecdsa.Parameters.ExportPublicKey (domain != ECDomainNames.secp224r1 ? true : false));
				txtGeneratedPublicKey.Text = domain.ToString ().Substring(4) + "=" + publicKey;
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
			}
		}
		#endregion

		#region PublicKey - Encryption Tab
		private void btnEncryption_Click (object sender, EventArgs e)
		{
			try {
				ECDomainNames domain;
				byte[] publicKey = ParsePublicKey (txtEncryptionKey.Text, out domain);
				ECIES ecies = new ECIES (domain);
				ecies.Parameters.PublicKey = publicKey;
				string encrypted = Convert.ToBase64String (ecies.Encrypt (Encoding.UTF8.GetBytes (txtEncryptionPlain.Text)));
				txtEncryptionCipher.Text = "ecies+xor=" + encrypted;
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
			}
		}

		private void btnDecryption_Click (object sender, EventArgs e)
		{
			try {
				ECDomainNames domain;
				byte[] privateKey = ParsePrivateKey (txtEncryptionKey.Text, txtEncryptionPass.Text, out domain);
				string text = txtEncryptionCipher.Text;
				string encrypt_type;
				byte[] encrypted;
				try {
					encrypt_type = text.Substring (0, text.IndexOf ('='));
					text = text.Substring (text.IndexOf ('=') + 1);
					encrypted = Convert.FromBase64String (text);
				} catch {
					throw new CryptographicException ("暗号文のフォーマットを認識できません");
				}
				switch (encrypt_type) {
					case "ecies+xor": {
						ECIES ecies = new ECIES (domain);
						ecies.Parameters.PrivateKey = privateKey;
						txtEncryptionPlain.Text = Encoding.UTF8.GetString (ecies.Decrypt (encrypted));
						break;
					}
					default: {
						throw new CryptographicException ("対応していない暗号化形式です");
					}
				}
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
			}
		}
		#endregion

		#region Cryptographic Helpers
		static byte[] ComputeHash (HashAlgorithm hashAlgo, byte[] data, bool dispose)
		{
			return ComputeHash (hashAlgo, data, 0, data.Length, dispose);
		}

		static byte[] ComputeHash (HashAlgorithm hashAlgo, byte[] data, int offset, int bytes, bool dispose)
		{
			byte[] hash = hashAlgo.ComputeHash (data, offset, bytes);
			if (dispose)
				(hashAlgo as IDisposable).Dispose ();
			return hash;
		}

		static byte[] ParsePrivateKey (string str_key, string str_passwd, out ECDomainNames domain)
		{
			try {
				string str_domain = null;
				byte[] key = null;
				if (!char.IsDigit (str_key[0])) {
					if (str_passwd.Length == 0)
						throw new CryptographicException ("秘密鍵は暗号化されています。パスフレーズを入力してください。");
					byte[] pass = ComputeHash (new SHA256Managed (), Encoding.UTF8.GetBytes (str_passwd), true);
					byte[] iv = ComputeHash (new SHA1Managed (), Encoding.UTF8.GetBytes (str_passwd), true);
					Array.Resize<byte> (ref iv, 128 >> 3);
					str_key = str_key.Substring (str_key.IndexOf ('=') + 1);
					str_domain = str_key.Substring (0, str_key.IndexOf ('='));
					str_key = str_key.Substring (str_key.IndexOf ('=') + 1);
					byte[] encrypted = Convert.FromBase64String (str_key);
					try {
						key = Decrypt (new CamelliaManaged (), CipherMode.CBC, pass, iv, encrypted);
					} catch {
						throw new CryptographicException ("パスフレーズが違います");
					}
				} else {
					str_domain = str_key.Substring (0, str_key.IndexOf ('='));
					str_key = str_key.Substring (str_key.IndexOf ('=') + 1);
					key = Convert.FromBase64String (str_key);
				}
				str_domain = "secp" + str_domain;
				domain = (ECDomainNames)Enum.Parse (typeof (ECDomainNames), str_domain);
				return key;
			} catch (CryptographicException) {
				throw;
			} catch {
				throw new CryptographicException ("秘密鍵として認識することができません");
			}
		}

		static byte[] ParsePublicKey (string str_key, out ECDomainNames domain)
		{
			try {
				string str_domain = str_key.Substring (0, str_key.IndexOf ('='));
				str_key = str_key.Substring (str_key.IndexOf ('=') + 1);
				byte[] key = Convert.FromBase64String (str_key);
				str_domain = "secp" + str_domain;
				domain = (ECDomainNames)Enum.Parse (typeof (ECDomainNames), str_domain);
				return key;
			} catch {
				throw new CryptographicException ("公開鍵として認識することができません");
			}
		}

		static byte[] Encrypt (SymmetricAlgorithm algo, CipherMode mode, byte[] key, byte[] iv, byte[] data)
		{
			algo.BlockSize = iv.Length << 3;
			algo.KeySize = key.Length << 3;
			algo.Padding = PaddingMode.ISO10126;
			algo.Mode = mode;
			int blockSize = iv.Length;
			int output_size = data.Length + blockSize;
			if (output_size % blockSize != 0)
				output_size += blockSize;
			byte[] temp = new byte[output_size];
			using (ICryptoTransform ct = algo.CreateEncryptor (key, iv)) {
				int size = data.Length;
				int offset = 0;
				while (true) {
					int min = Math.Min (size - offset, blockSize);
					if (min == blockSize) {
						ct.TransformBlock (data, offset, min, temp, offset);
						offset += min;
					} else {
						byte[] hoge = ct.TransformFinalBlock (data, offset, min);
						Buffer.BlockCopy (hoge, 0, temp, offset, hoge.Length);
						int newsize = offset + hoge.Length;
						if (temp.Length != newsize)
							Array.Resize<byte> (ref temp, newsize);
						break;
					}
				}
			}
			return temp;
		}

		static byte[] Decrypt (SymmetricAlgorithm algo, CipherMode mode, byte[] key, byte[] iv, byte[] data)
		{
			algo.BlockSize = iv.Length << 3;
			algo.KeySize = key.Length << 3;
			algo.Padding = PaddingMode.ISO10126;
			algo.Mode = mode;
			int blockSize = iv.Length;
			byte[] temp = new byte[data.Length];
			using (ICryptoTransform ct = algo.CreateDecryptor (key, iv)) {
				int size = data.Length;
				int offset = 0;
				for (; offset < size - blockSize; offset += blockSize)
					ct.TransformBlock (data, offset, blockSize, temp, offset);
				byte[] hoge = ct.TransformFinalBlock (data, offset, blockSize);
				Buffer.BlockCopy (hoge, 0, temp, offset, hoge.Length);
				int newsize = offset + hoge.Length;
				if (temp.Length != newsize)
					Array.Resize<byte> (ref temp, newsize);
			}
			return temp;
		}
		#endregion
	}
}
