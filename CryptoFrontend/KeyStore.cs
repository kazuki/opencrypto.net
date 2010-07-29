using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace CryptoFrontend
{
	class KeyStore
	{
		string _path;
		List<KeyEntry> _privateKeys = new List<KeyEntry> ();
		List<KeyEntry> _publicKeys = new List<KeyEntry> ();

		public event EventHandler PrivateKeyUpdated;
		public event EventHandler PublicKeyUpdated;

		public KeyStore (string path)
		{
			_path = path;
		}

		public void Load ()
		{
			if (!File.Exists (_path))
				return;
			_privateKeys.Clear ();
			_publicKeys.Clear ();
			using (XmlTextReader reader = new XmlTextReader (_path)) {
				while (reader.Read ()) {
					if (!reader.IsStartElement ())
						continue;
					if (reader.Name != "private" && reader.Name != "public")
						continue;
					bool isPrivate = (reader.Name == "private");
					string name = null, key = null;
					XmlReader sub_reader = reader.ReadSubtree ();
					while (sub_reader.Read ()) {
						if (!sub_reader.IsStartElement ()) continue;
						switch (sub_reader.Name) {
							case "name":
								name = sub_reader.ReadElementString ();
								break;
							case "key":
								key = sub_reader.ReadElementString ();
								break;
						}
					}
					if (name != null && key != null) {
						if (isPrivate)
							_privateKeys.Add (new KeyEntry (name, key));
						else
							_publicKeys.Add (new KeyEntry (name, key));
					}
				}
			}

			if (_privateKeys.Count > 0)
				RaisePrivateKeyUpdatedEvent ();
			if (_publicKeys.Count > 0)
				RaisePublicKeyUpdatedEvent ();
		}

		public void Save ()
		{
			Directory.CreateDirectory (Path.GetDirectoryName (_path));
			using (XmlTextWriter writer = new XmlTextWriter (_path, new System.Text.UTF8Encoding (false))) {
				writer.Formatting = Formatting.Indented;
				writer.Indentation = 1;
				writer.IndentChar = '\t';
				writer.WriteStartDocument ();
				writer.WriteStartElement ("keys");
				foreach (KeyEntry entry in _privateKeys) {
					writer.WriteStartElement ("private");
					writer.WriteElementString ("name", entry.Name);
					writer.WriteElementString ("key", entry.Key);
					writer.WriteEndElement ();
				}
				foreach (KeyEntry entry in _publicKeys) {
					writer.WriteStartElement ("public");
					writer.WriteElementString ("name", entry.Name);
					writer.WriteElementString ("key", entry.Key);
					writer.WriteEndElement ();
				}
				writer.WriteEndElement ();
				writer.WriteEndDocument ();
			}
		}

		public void AddPrivateKeyEntry (string name, string key)
		{
			_privateKeys.Add (new KeyEntry (name, key));
			Save ();
			RaisePrivateKeyUpdatedEvent ();
		}

		public void AddPublicKeyEntry (string name, string key)
		{
			_publicKeys.Add (new KeyEntry (name, key));
			Save ();
			RaisePublicKeyUpdatedEvent ();
		}

		public void RaisePrivateKeyUpdatedEvent ()
		{
			if (PrivateKeyUpdated != null)
				PrivateKeyUpdated (this, EventArgs.Empty);
		}

		public void RemoveEntry (KeyEntry entry)
		{
			if (_privateKeys.Contains (entry)) {
				_privateKeys.Remove (entry);
				Save ();
				RaisePrivateKeyUpdatedEvent ();
				return;
			}
			if (_publicKeys.Contains (entry)) {
				_publicKeys.Remove (entry);
				Save ();
				RaisePublicKeyUpdatedEvent ();
				return;
			}
		}

		public void RaisePublicKeyUpdatedEvent ()
		{
			if (PublicKeyUpdated != null)
				PublicKeyUpdated (this, EventArgs.Empty);
		}

		public IList<KeyEntry> PublicKeys {
			get { return _publicKeys.AsReadOnly (); }
		}

		public IList<KeyEntry> PrivateKeys {
			get { return _privateKeys.AsReadOnly (); }
		}
	}
}
