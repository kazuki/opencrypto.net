using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using openCrypto.ECDSA;
using OC_ECDomainParameters = openCrypto.EllipticCurve.ECDomainParameters;
using ECDomains = openCrypto.EllipticCurve.ECDomains;
using ECDomainNames = openCrypto.EllipticCurve.ECDomainNames;
using openCrypto.FiniteField;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.Utilities.Encoders;

namespace Demo
{
	public partial class Demo2007 : Form
	{
		public Demo2007 ()
		{
			InitializeComponent ();
		}

		private void btnStartCamelliaEncryptionTime_Click (object sender, EventArgs e)
		{
			etCamelliaBC.NumberOfThreads = etCamelliaOC.NumberOfThreads = (int)nudCamellia.Value;
			etCamelliaBC.DataSize = etCamelliaOC.DataSize = 1024 * 1024 * 640;

			etCamelliaOC.Start ();
			etCamelliaBC.Start ();
		}

		private void btnStartRijndaelEncryptionTime_Click (object sender, EventArgs e)
		{
			etRijndaelBC.NumberOfThreads = etRijndaelEB.NumberOfThreads = etRijndaelOC.NumberOfThreads = (int)nudRijndael.Value;
			etRijndaelBC.DataSize = etRijndaelEB.DataSize = etRijndaelOC.DataSize = 1024 * 1024 * 640;
			etRijndaelOC.Start ();
			etRijndaelEB.Start ();
			etRijndaelBC.Start ();
		}

		private void btnStartEncryptionSpeed_Click (object sender, EventArgs e)
		{
			encryptionSpeed1.NumberOfThreads =
				encryptionSpeed2.NumberOfThreads =
				encryptionSpeed3.NumberOfThreads =
				encryptionSpeed4.NumberOfThreads =
				encryptionSpeed5.NumberOfThreads = (int)nudEncryptionSpeed.Value;

			encryptionSpeed1.Start ();
			encryptionSpeed2.Start ();
			encryptionSpeed3.Start ();
			encryptionSpeed4.Start ();
			encryptionSpeed5.Start ();
			double max =
				Math.Max (encryptionSpeed1.Speed, 
				Math.Max (encryptionSpeed2.Speed,
				Math.Max (encryptionSpeed3.Speed,
				Math.Max (encryptionSpeed4.Speed, encryptionSpeed5.Speed))));
			max *= 1.1;
			encryptionSpeed1.SetProgressbar ((int)max);
			encryptionSpeed2.SetProgressbar ((int)max);
			encryptionSpeed3.SetProgressbar ((int)max);
			encryptionSpeed4.SetProgressbar ((int)max);
			encryptionSpeed5.SetProgressbar ((int)max);
		}

		private void btnStartECDSA_Click (object sender, EventArgs e)
		{
			byte[] hash = new byte[160 >> 3];
			Stopwatch sw = new Stopwatch ();
			double ocSignTime, ocVerifyTime, bcSignTime, bcVerifyTime;
			{
				ECDSA ecdsa = new ECDSA (ECDomainNames.secp192r1);
				ecdsa.ToXmlString (false);
				sw.Reset ();
				sw.Start ();
				byte[] ecdsaSign = ecdsa.SignHash (hash);
				sw.Stop ();
				ocSignTime = sw.Elapsed.TotalSeconds;
				sw.Reset ();
				sw.Start ();
				ecdsa.VerifyHash (hash, ecdsaSign);
				sw.Stop ();
				ocVerifyTime = sw.Elapsed.TotalSeconds;
			}

			{
				ECDsaSigner ecdsa = new ECDsaSigner ();
				X9ECParameters SEC_P192r1 = SecNamedCurves.GetByName ("secp192r1");
				BigInteger key = new BigInteger (SEC_P192r1.N.BitCount, new Random ());
				ECDomainParameters domain = new ECDomainParameters (SEC_P192r1.Curve, SEC_P192r1.G, SEC_P192r1.N);
				ECPrivateKeyParameters privateKey = new ECPrivateKeyParameters (key, domain);
				ECPoint publicKeyPoint = SEC_P192r1.G.Multiply (key);
				ECPublicKeyParameters publicKey = new ECPublicKeyParameters (publicKeyPoint, domain);
				ecdsa.Init (true, privateKey);

				sw.Reset ();
				sw.Start ();
				BigInteger[] sign = ecdsa.GenerateSignature (hash);
				sw.Stop ();
				bcSignTime = sw.Elapsed.TotalSeconds;

				ecdsa.Init (false, publicKey);
				sw.Reset ();
				sw.Start ();
				ecdsa.VerifySignature (hash, sign[0], sign[1]);
				sw.Stop ();
				bcVerifyTime = sw.Elapsed.TotalSeconds;
			}

			double scale = 1000;
			bcSignTime *= scale;
			bcVerifyTime *= scale;
			ocSignTime *= scale;
			ocVerifyTime *= scale;

			lblBCSign.Text = "Sign (" + bcSignTime.ToString ("f2") + "ms)";
			lblBCVerify.Text = "Verify (" + bcVerifyTime.ToString ("f2") + "ms)";
			lblOCSign.Text = "Sign (" + ocSignTime.ToString ("f2") + "ms)";
			lblOCVerify.Text = "Verify (" + ocVerifyTime.ToString ("f2") + "ms)";

			double max = Math.Max (ocSignTime, Math.Max (ocVerifyTime, Math.Max (bcSignTime, bcVerifyTime)));			
			max *= 1.1;

			pbEcdsaSignBC.Maximum = pbEcdsaVerifyBC.Maximum = pbEcdsaSignOC.Maximum = pbEcdsaVerifyOC.Maximum = (int)max;
			pbEcdsaSignBC.Value = (int)bcSignTime;
			pbEcdsaVerifyBC.Value = (int)bcVerifyTime;
			pbEcdsaSignOC.Value = (int)ocSignTime;
			pbEcdsaVerifyOC.Value = (int)ocVerifyTime;
		}
	}
}