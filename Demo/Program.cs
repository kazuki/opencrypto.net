using System;
using System.Windows.Forms;

namespace Demo
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			if (args.Length == 1) {
				int type; long size;
				if (!int.TryParse (args[0], out type))
					return;
				switch ((ImplementationType)type) {
					case ImplementationType.RijndaelRuntime:
						size = MemoryUsageCheck.CheckRuntimeRijndael ();
						break;
					case ImplementationType.RijndaelBouncyCastle:
						size = MemoryUsageCheck.CheckBCRijndael ();
						break;
					case ImplementationType.RijndaelOpenCrypto:
						size = MemoryUsageCheck.CheckOpenCryptoRijndael ();
						break;
					case ImplementationType.CamelliaOpenCrypto:
						size = MemoryUsageCheck.CheckOpenCryptoCamellia ();
						break;
					default:
						return;
				}
				Console.Write ("メモリ使用量 ({0}): ", Helper.ToName ((ImplementationType)type));
				Console.WriteLine ("{0} バイト", size);
				return;
			}

			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);
			Application.Run (new SpeedCheckForm ());
		}
	}
}
