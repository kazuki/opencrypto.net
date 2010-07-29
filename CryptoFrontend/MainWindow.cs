using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.IO;
using openCrypto;
using openCrypto.EllipticCurve;
using openCrypto.EllipticCurve.Encryption;
using openCrypto.EllipticCurve.KeyAgreement;
using openCrypto.EllipticCurve.Signature;

namespace CryptoFrontend
{
	public partial class MainWindow : Form
	{
		KeyStore _store;

		public MainWindow ()
		{
			InitializeComponent ();
			cbKeyType.SelectedIndex = 8;
			txtGeneratedKey.Font = new Font (FontFamily.GenericMonospace, (int)(Font.Size + 1.5), Font.Unit);
			txtGeneratedPublicKey.Font = txtGeneratedKey.Font;
			txtGeneratedKeyPass.Font = txtGeneratedKey.Font;
			txtDecryptKeyPass.Font = txtGeneratedKey.Font;
			tabControl1.SelectedTab = tabPage2;
			cbPassEncryptType.SelectedIndex = 0;
			cbEncryptCrypto.SelectedIndex = 2;

			_store = new KeyStore (Path.Combine (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.ApplicationData), "openCrypto.NET"), "keystore.xml"));
			_store.PrivateKeyUpdated += delegate (object sender, EventArgs args) {
				object selected = cbPrivateKeys.SelectedItem;
				object selected2 = cbPrivateKeys2.SelectedItem;
				cbPrivateKeys.Items.Clear ();
				cbPrivateKeys2.Items.Clear ();
				foreach (KeyEntry entry in _store.PrivateKeys) {
					cbPrivateKeys.Items.Add (entry);
					cbPrivateKeys2.Items.Add (entry);
				}
				cbPrivateKeys.SelectedItem = selected;
				if (cbPrivateKeys.SelectedIndex < 0 && cbPrivateKeys.Items.Count > 0)
					cbPrivateKeys.SelectedIndex = 0;
				cbPrivateKeys2.SelectedItem = selected2;
				if (cbPrivateKeys2.SelectedIndex < 0 && cbPrivateKeys2.Items.Count > 0)
					cbPrivateKeys2.SelectedIndex = 0;
			};
			_store.PublicKeyUpdated += delegate (object sender, EventArgs args) {
				object selected = cbPublicKeys.SelectedItem;
				object selected2 = cbPublicKeys2.SelectedItem;
				cbPublicKeys.Items.Clear ();
				cbPublicKeys2.Items.Clear ();
				foreach (KeyEntry entry in _store.PublicKeys) {
					cbPublicKeys.Items.Add (entry);
					cbPublicKeys2.Items.Add (entry);
				}
				cbPublicKeys.SelectedItem = selected;
				if (cbPublicKeys.SelectedIndex < 0 && cbPublicKeys.Items.Count > 0)
					cbPublicKeys.SelectedIndex = 0;
				cbPublicKeys2.SelectedItem = selected2;
				if (cbPublicKeys2.SelectedIndex < 0 && cbPublicKeys2.Items.Count > 0)
					cbPublicKeys2.SelectedIndex = 0;
			};
			_store.Load ();
		}

		#region PublicKey - KeyGeneration Tab
		private void btnKeyGenerate_Click (object sender, EventArgs e)
		{
			ECDomainNames domain = (ECDomainNames)cbKeyType.SelectedIndex;
			ECDSA dsa = new ECDSA (domain);
			string domainName = domain.ToString ().Substring(4);
			byte[] privateKeyBytes = dsa.Parameters.PrivateKey;
			txtGeneratedKey.Text = ToPrivateKeyString (privateKeyBytes, txtGeneratedKeyPass.Text, domain);
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

		private void btnDecryptPrivateKey_Click (object sender, EventArgs e)
		{
			try {
				ECDomainNames domain;
				byte[] privateKey = ParsePrivateKey (txtGeneratedKey.Text, txtGeneratedKeyPass.Text, out domain);
				txtGeneratedKey.Text = ToPrivateKeyString (privateKey, string.Empty, domain);
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
			}
		}

		private void btnEncryptPrivateKey_Click (object sender, EventArgs e)
		{
			try {
				ECDomainNames domain;
				byte[] privateKey = ParsePrivateKey (txtGeneratedKey.Text, txtGeneratedKeyPass.Text, out domain);
				txtGeneratedKey.Text = ToPrivateKeyString (privateKey, txtGeneratedKeyPass.Text, domain);
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
			}
		}

		string ToPrivateKeyString (byte[] privateKey, string passphrase, ECDomainNames domain)
		{
			string domainName = domain.ToString ().Substring (4);
			if (passphrase.Length > 0) {
				byte[] pass = ComputeHash (new SHA256Managed (), Encoding.UTF8.GetBytes (txtGeneratedKeyPass.Text), true);
				byte[] iv = ComputeHash (new SHA1Managed (), Encoding.UTF8.GetBytes (txtGeneratedKeyPass.Text), true);
				Array.Resize<byte> (ref iv, 128 >> 3);
				string encType = null;
				SymmetricAlgorithm algo = null;
				switch (cbPassEncryptType.SelectedIndex) {
					case 0:
						encType = "camellia256";
						algo = new CamelliaManaged ();
						break;
					case 1:
						encType = "rijndael256";
						algo = new openCrypto.RijndaelManaged ();
						break;
					default:
						throw new CryptographicException ("暗号化の種類を認識できません");
				}
				byte[] encrypted = Encrypt (algo, CipherMode.CBC, pass, iv, privateKey);
				string privateKeyText = Convert.ToBase64String (encrypted);
				return encType + "=" + domainName + "=" + privateKeyText;
			} else {
				string privateKeyText = Convert.ToBase64String (privateKey);
				return domainName + "=" + privateKeyText;
			}
		}
		#endregion

		#region PublicKey - KeyManagement Tab
		private void btnRegisterPrivateKey_Click (object sender, EventArgs e)
		{
			try {
				ECDomainNames domain;
				byte[] privateKey = ParsePrivateKey (txtGeneratedKey.Text, txtGeneratedKeyPass.Text, out domain);
				ECDSA ecdsa = new ECDSA (domain);
				ecdsa.Parameters.PrivateKey = privateKey;
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
				return;
			}
			using (TextInputDialog dlg = new TextInputDialog ("名前を入力", "秘密鍵の名前を入力してください")) {
				if (dlg.ShowDialog () == DialogResult.OK) {
					_store.AddPrivateKeyEntry (dlg.InputText, txtGeneratedKey.Text);
				}
			}
		}

		private void btnAddPrivateKey_Click (object sender, EventArgs e)
		{
			MessageBox.Show ("秘密鍵の追加は鍵生成タブから行ってください");
		}

		private void btnAddPublicKey_Click (object sender, EventArgs e)
		{
			string name;
			using (TextInputDialog dlg = new TextInputDialog ("名前の入力", "公開鍵の名前を入力してください")) {
				if (dlg.ShowDialog () != DialogResult.OK)
					return;
				name = dlg.InputText;
			}
			using (TextInputDialog dlg = new TextInputDialog (name + "の公開鍵", name + "の公開鍵を入力してください", 4)) {
				if (dlg.ShowDialog () != DialogResult.OK)
					return;
				try {
					ECDomainNames domain;
					byte[] publicKey = ParsePublicKey (dlg.InputText, out domain);
					ECDSA ecdsa = new ECDSA (domain);
					ecdsa.Parameters.PublicKey = publicKey;
				} catch (Exception ex) {
					MessageBox.Show (ex.Message);
					return;
				}
				_store.AddPublicKeyEntry (name, dlg.InputText);
			}
		}

		private void btnPrivateKeyRename_Click (object sender, EventArgs e)
		{
			if (cbPrivateKeys.SelectedIndex < 0)
				return;
			Rename (cbPrivateKeys.SelectedItem as KeyEntry);
			_store.Save ();
			_store.RaisePrivateKeyUpdatedEvent ();
		}

		private void btnPublicKeyRename_Click (object sender, EventArgs e)
		{
			if (cbPublicKeys.SelectedIndex < 0)
				return;
			Rename (cbPublicKeys.SelectedItem as KeyEntry);
			_store.Save ();
			_store.RaisePublicKeyUpdatedEvent ();
		}

		void Rename (KeyEntry entry)
		{
			using (TextInputDialog dlg = new TextInputDialog ("名前の変更", "名前を変更してOKを押してください")) {
				dlg.SetDefaultText (entry.Name, true);
				if (dlg.ShowDialog () == DialogResult.OK) {
					entry.Name = dlg.InputText;
				}
			}
		}

		private void btnPublicKeyDel_Click (object sender, EventArgs e)
		{
			Remove (cbPublicKeys.SelectedItem as KeyEntry);
		}

		private void btnPrivateKeyDel_Click (object sender, EventArgs e)
		{
			Remove (cbPrivateKeys.SelectedItem as KeyEntry);
		}

		void Remove (KeyEntry entry)
		{
			if (entry == null)
				return;
			if (MessageBox.Show (entry.Name + "を削除してもよろしいですか？", "確認", MessageBoxButtons.OKCancel) == DialogResult.OK)
				_store.RemoveEntry (entry);
		}

		private void btnPrivateKeyCopy_Click (object sender, EventArgs e)
		{
			KeyEntry entry = cbPrivateKeys.SelectedItem as KeyEntry;
			try {
				Clipboard.SetText (entry.Key);
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
			}
		}

		private void btnPublicKeyCopy_Click (object sender, EventArgs e)
		{
			KeyEntry entry = cbPublicKeys.SelectedItem as KeyEntry;
			try {
				Clipboard.SetText (entry.Key);
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
			}
		}
		#endregion

		#region PublicKey - Encrypt Tab
		private void btnEncryptText_Click (object sender, EventArgs e)
		{
			if (txtEncryptPlain.Text.Length == 0)
				return;
			try {
				KeyEntry publicKeyEntry = cbPublicKeys2.SelectedItem as KeyEntry;
				if (publicKeyEntry == null)
					throw new Exception ("暗号化に利用する公開鍵を選択してください");
				ECDomainNames domain;
				byte[] publicKey = ParsePublicKey (publicKeyEntry.Key, out domain);
				string encryptType = null;
				SymmetricAlgorithm algo = null;
				switch (cbEncryptCrypto.SelectedIndex) {
					case 0:
						encryptType = "ecies+xor";
						algo = null;
						break;
					case 1:
					case 2:
						encryptType = "ecies+camellia";
						algo = new CamelliaManaged ();
						algo.BlockSize = 128;
						if (cbEncryptCrypto.SelectedIndex == 1) {
							encryptType += "128";
							algo.KeySize = 128;
						} else {
							encryptType += "256";
							algo.KeySize = 256;
						}
						break;
					case 3:
					case 4:
						encryptType = "ecies+rijndael";
						algo = new openCrypto.RijndaelManaged ();
						algo.BlockSize = 128;
						if (cbEncryptCrypto.SelectedIndex == 3) {
							encryptType += "128";
							algo.KeySize = 128;
						} else {
							encryptType += "256";
							algo.KeySize = 256;
						}
						break;
					default:
						throw new CryptographicException ("Unknown");
				}
				if (algo != null) {
					algo.Mode = CipherMode.CBC;
					algo.Padding = PaddingMode.PKCS7;
				}
				ECIES ecies = new ECIES (domain, algo);
				ecies.Parameters.PublicKey = publicKey;
				string encrypted = Convert.ToBase64String (ecies.Encrypt (Encoding.UTF8.GetBytes (txtEncryptPlain.Text)));
				txtEncryptCipher.Text = encryptType + "=" + encrypted;
			} catch (Exception ex) {
				MessageBox.Show (ex.Message);
			}
		}
		#endregion

		#region Public Key - Decrypt Tab
		private void btnDecryptText_Click (object sender, EventArgs e)
		{
			try {
				KeyEntry privateKeyEntry = cbPrivateKeys2.SelectedItem as KeyEntry;
				if (privateKeyEntry == null)
					throw new Exception ("復号に利用する秘密鍵を指定してください");
				ECDomainNames domain;
				byte[] privateKey = ParsePrivateKey (privateKeyEntry.Key, txtDecryptKeyPass.Text, out domain);
				string text = txtDecryptCipher.Text;
				string encrypt_type;
				byte[] encrypted;
				try {
					encrypt_type = text.Substring (0, text.IndexOf ('='));
					text = text.Substring (text.IndexOf ('=') + 1);
					encrypted = Convert.FromBase64String (text);
				} catch {
					throw new CryptographicException ("暗号文のフォーマットを認識できません");
				}
				if (encrypt_type.StartsWith ("ecies+")) {
					encrypt_type = encrypt_type.Substring (6);
					SymmetricAlgorithm algo = null;
					switch (encrypt_type) {
						case "xor":
							break;
						case "camellia128":
						case "camellia256":
						case "rijndael128":
						case "rijndael256":
							algo = encrypt_type.StartsWith ("camellia") ? (SymmetricAlgorithm)new CamelliaManaged () : (SymmetricAlgorithm)new openCrypto.RijndaelManaged ();
							algo.BlockSize = 128;
							algo.KeySize = encrypt_type.EndsWith ("128") ? 128 : 256;
							algo.Mode = CipherMode.CBC;
							algo.Padding = PaddingMode.PKCS7;
							break;
						default:
							throw new CryptographicException ("対応していない暗号化形式です");
					}
					ECIES ecies = new ECIES (domain, algo);
					ecies.Parameters.PrivateKey = privateKey;
					txtDecryptPlain.Text = Encoding.UTF8.GetString (ecies.Decrypt (encrypted));
				} else {
					throw new CryptographicException ("対応していない暗号化形式です");
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
					string encType = str_key.Substring (0, str_key.IndexOf ('='));
					str_key = str_key.Substring (str_key.IndexOf ('=') + 1);
					str_domain = str_key.Substring (0, str_key.IndexOf ('='));
					str_key = str_key.Substring (str_key.IndexOf ('=') + 1);
					byte[] encrypted = Convert.FromBase64String (str_key);
					try {
						SymmetricAlgorithm algo = null;
						switch (encType) {
							case "camellia256":
								algo = new CamelliaManaged ();
								break;
							case "rijndael256":
								algo = new openCrypto.RijndaelManaged ();
								break;
							default:
								throw new CryptographicException ("秘密鍵の暗号化タイプを認識できません");
						}
						key = Decrypt (algo, CipherMode.CBC, pass, iv, encrypted);
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
