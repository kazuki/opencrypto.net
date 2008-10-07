namespace CryptoFrontend
{
	class KeyEntry
	{
		public string Name;
		public string Key;

		public KeyEntry (string name, string key)
		{
			Name = name;
			Key = key;
		}

		public override string ToString ()
		{
			return Name + " (" + Key + ")";
		}
	}
}
