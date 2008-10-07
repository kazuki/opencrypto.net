namespace CryptoFrontend
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl ();
			this.tabPage1 = new System.Windows.Forms.TabPage ();
			this.label11 = new System.Windows.Forms.Label ();
			this.tabPage2 = new System.Windows.Forms.TabPage ();
			this.tabControl2 = new System.Windows.Forms.TabControl ();
			this.tabPage3 = new System.Windows.Forms.TabPage ();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel ();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel ();
			this.cbKeyType = new System.Windows.Forms.ComboBox ();
			this.label2 = new System.Windows.Forms.Label ();
			this.btnKeyGenerate = new System.Windows.Forms.Button ();
			this.btnPublicKeyGenerate = new System.Windows.Forms.Button ();
			this.txtGeneratedPublicKey = new System.Windows.Forms.TextBox ();
			this.txtGeneratedKey = new System.Windows.Forms.TextBox ();
			this.label3 = new System.Windows.Forms.Label ();
			this.label1 = new System.Windows.Forms.Label ();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel ();
			this.txtGeneratedKeyPass = new System.Windows.Forms.TextBox ();
			this.cbPassEncryptType = new System.Windows.Forms.ComboBox ();
			this.label4 = new System.Windows.Forms.Label ();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel ();
			this.btnRegisterPrivateKey = new System.Windows.Forms.Button ();
			this.btnDecryptPrivateKey = new System.Windows.Forms.Button ();
			this.btnEncryptPrivateKey = new System.Windows.Forms.Button ();
			this.tabPage4 = new System.Windows.Forms.TabPage ();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel ();
			this.groupBox2 = new System.Windows.Forms.GroupBox ();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel ();
			this.cbPublicKeys = new System.Windows.Forms.ComboBox ();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel ();
			this.btnAddPublicKey = new System.Windows.Forms.Button ();
			this.btnPublicKeyRename = new System.Windows.Forms.Button ();
			this.btnPublicKeyDel = new System.Windows.Forms.Button ();
			this.btnPublicKeyCopy = new System.Windows.Forms.Button ();
			this.groupBox1 = new System.Windows.Forms.GroupBox ();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel ();
			this.cbPrivateKeys = new System.Windows.Forms.ComboBox ();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel ();
			this.btnPrivateKeyRename = new System.Windows.Forms.Button ();
			this.btnPrivateKeyDel = new System.Windows.Forms.Button ();
			this.btnPrivateKeyCopy = new System.Windows.Forms.Button ();
			this.tabPage5 = new System.Windows.Forms.TabPage ();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel ();
			this.txtEncryptPlain = new System.Windows.Forms.TextBox ();
			this.label6 = new System.Windows.Forms.Label ();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel ();
			this.btnEncryptText = new System.Windows.Forms.Button ();
			this.btnEncryptFile = new System.Windows.Forms.Button ();
			this.txtEncryptCipher = new System.Windows.Forms.TextBox ();
			this.label15 = new System.Windows.Forms.Label ();
			this.cbEncryptCrypto = new System.Windows.Forms.ComboBox ();
			this.label14 = new System.Windows.Forms.Label ();
			this.label13 = new System.Windows.Forms.Label ();
			this.cbPublicKeys2 = new System.Windows.Forms.ComboBox ();
			this.tabPage7 = new System.Windows.Forms.TabPage ();
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel ();
			this.txtDecryptCipher = new System.Windows.Forms.TextBox ();
			this.txtDecryptKeyPass = new System.Windows.Forms.TextBox ();
			this.label16 = new System.Windows.Forms.Label ();
			this.label17 = new System.Windows.Forms.Label ();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel ();
			this.btnDecryptText = new System.Windows.Forms.Button ();
			this.btnDecryptFile = new System.Windows.Forms.Button ();
			this.txtDecryptPlain = new System.Windows.Forms.TextBox ();
			this.label19 = new System.Windows.Forms.Label ();
			this.label20 = new System.Windows.Forms.Label ();
			this.cbPrivateKeys2 = new System.Windows.Forms.ComboBox ();
			this.btnAddPrivateKey = new System.Windows.Forms.Button ();
			this.tabControl1.SuspendLayout ();
			this.tabPage1.SuspendLayout ();
			this.tabPage2.SuspendLayout ();
			this.tabControl2.SuspendLayout ();
			this.tabPage3.SuspendLayout ();
			this.tableLayoutPanel1.SuspendLayout ();
			this.tableLayoutPanel3.SuspendLayout ();
			this.tableLayoutPanel4.SuspendLayout ();
			this.flowLayoutPanel2.SuspendLayout ();
			this.tabPage4.SuspendLayout ();
			this.tableLayoutPanel5.SuspendLayout ();
			this.groupBox2.SuspendLayout ();
			this.tableLayoutPanel7.SuspendLayout ();
			this.flowLayoutPanel4.SuspendLayout ();
			this.groupBox1.SuspendLayout ();
			this.tableLayoutPanel6.SuspendLayout ();
			this.flowLayoutPanel3.SuspendLayout ();
			this.tabPage5.SuspendLayout ();
			this.tableLayoutPanel8.SuspendLayout ();
			this.flowLayoutPanel5.SuspendLayout ();
			this.tabPage7.SuspendLayout ();
			this.tableLayoutPanel9.SuspendLayout ();
			this.flowLayoutPanel6.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add (this.tabPage1);
			this.tabControl1.Controls.Add (this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point (0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size (444, 416);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add (this.label11);
			this.tabPage1.Location = new System.Drawing.Point (4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding (3);
			this.tabPage1.Size = new System.Drawing.Size (436, 390);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "共通鍵暗号";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label11.Location = new System.Drawing.Point (3, 3);
			this.label11.Margin = new System.Windows.Forms.Padding (0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size (430, 384);
			this.label11.TabIndex = 1;
			this.label11.Text = "未実装";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add (this.tabControl2);
			this.tabPage2.Location = new System.Drawing.Point (4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding (3);
			this.tabPage2.Size = new System.Drawing.Size (436, 390);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "公開鍵暗号";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabControl2
			// 
			this.tabControl2.Controls.Add (this.tabPage3);
			this.tabControl2.Controls.Add (this.tabPage4);
			this.tabControl2.Controls.Add (this.tabPage5);
			this.tabControl2.Controls.Add (this.tabPage7);
			this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl2.Location = new System.Drawing.Point (3, 3);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size (430, 384);
			this.tabControl2.TabIndex = 0;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add (this.tableLayoutPanel1);
			this.tabPage3.Location = new System.Drawing.Point (4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size (422, 358);
			this.tabPage3.TabIndex = 0;
			this.tabPage3.Text = "鍵生成";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel1.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add (this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel1.Controls.Add (this.txtGeneratedPublicKey, 1, 4);
			this.tableLayoutPanel1.Controls.Add (this.txtGeneratedKey, 1, 3);
			this.tableLayoutPanel1.Controls.Add (this.label3, 0, 4);
			this.tableLayoutPanel1.Controls.Add (this.label1, 0, 3);
			this.tableLayoutPanel1.Controls.Add (this.tableLayoutPanel4, 1, 2);
			this.tableLayoutPanel1.Controls.Add (this.label4, 0, 2);
			this.tableLayoutPanel1.Controls.Add (this.flowLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point (0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size (422, 358);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 4;
			this.tableLayoutPanel1.SetColumnSpan (this.tableLayoutPanel3, 2);
			this.tableLayoutPanel3.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel3.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel3.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel3.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add (this.cbKeyType, 3, 0);
			this.tableLayoutPanel3.Controls.Add (this.label2, 2, 0);
			this.tableLayoutPanel3.Controls.Add (this.btnKeyGenerate, 0, 0);
			this.tableLayoutPanel3.Controls.Add (this.btnPublicKeyGenerate, 1, 0);
			this.tableLayoutPanel3.Location = new System.Drawing.Point (0, 0);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel3.Size = new System.Drawing.Size (422, 35);
			this.tableLayoutPanel3.TabIndex = 4;
			// 
			// cbKeyType
			// 
			this.cbKeyType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbKeyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbKeyType.FormattingEnabled = true;
			this.cbKeyType.Items.AddRange (new object[] {
            "secp112r1 (RSA512bit相当)",
            "secp112r2 (RSA512bit相当)",
            "secp128r1",
            "secp128r2",
            "secp160r1 (RSA1024bit相当)",
            "secp160r2 (RSA1024bit相当)",
            "secp192r1",
            "secp224r1 (RSA2048bit相当)",
            "secp256r1 (RSA3072bit相当)",
            "secp384r1 (RSA7680bit相当)",
            "secp521r1 (RSA15360bit相当)"});
			this.cbKeyType.Location = new System.Drawing.Point (244, 7);
			this.cbKeyType.Name = "cbKeyType";
			this.cbKeyType.Size = new System.Drawing.Size (175, 21);
			this.cbKeyType.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point (204, 11);
			this.label2.Margin = new System.Windows.Forms.Padding (6, 0, 0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (37, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "強度：";
			// 
			// btnKeyGenerate
			// 
			this.btnKeyGenerate.AutoSize = true;
			this.btnKeyGenerate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnKeyGenerate.Location = new System.Drawing.Point (3, 3);
			this.btnKeyGenerate.Name = "btnKeyGenerate";
			this.btnKeyGenerate.Padding = new System.Windows.Forms.Padding (3);
			this.btnKeyGenerate.Size = new System.Drawing.Size (93, 29);
			this.btnKeyGenerate.TabIndex = 0;
			this.btnKeyGenerate.Text = "秘密鍵の作成";
			this.btnKeyGenerate.UseVisualStyleBackColor = true;
			this.btnKeyGenerate.Click += new System.EventHandler (this.btnKeyGenerate_Click);
			// 
			// btnPublicKeyGenerate
			// 
			this.btnPublicKeyGenerate.AutoSize = true;
			this.btnPublicKeyGenerate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnPublicKeyGenerate.Location = new System.Drawing.Point (102, 3);
			this.btnPublicKeyGenerate.Name = "btnPublicKeyGenerate";
			this.btnPublicKeyGenerate.Padding = new System.Windows.Forms.Padding (3);
			this.btnPublicKeyGenerate.Size = new System.Drawing.Size (93, 29);
			this.btnPublicKeyGenerate.TabIndex = 0;
			this.btnPublicKeyGenerate.Text = "公開鍵の作成";
			this.btnPublicKeyGenerate.UseVisualStyleBackColor = true;
			this.btnPublicKeyGenerate.Click += new System.EventHandler (this.btnPublicKeyGenerate_Click);
			// 
			// txtGeneratedPublicKey
			// 
			this.txtGeneratedPublicKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtGeneratedPublicKey.Location = new System.Drawing.Point (78, 231);
			this.txtGeneratedPublicKey.Multiline = true;
			this.txtGeneratedPublicKey.Name = "txtGeneratedPublicKey";
			this.txtGeneratedPublicKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtGeneratedPublicKey.Size = new System.Drawing.Size (341, 124);
			this.txtGeneratedPublicKey.TabIndex = 7;
			// 
			// txtGeneratedKey
			// 
			this.txtGeneratedKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtGeneratedKey.Location = new System.Drawing.Point (78, 102);
			this.txtGeneratedKey.Multiline = true;
			this.txtGeneratedKey.Name = "txtGeneratedKey";
			this.txtGeneratedKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtGeneratedKey.Size = new System.Drawing.Size (341, 123);
			this.txtGeneratedKey.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point (3, 233);
			this.label3.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size (49, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "公開鍵：";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point (3, 104);
			this.label1.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (49, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "秘密鍵：";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel4.Controls.Add (this.txtGeneratedKeyPass, 0, 0);
			this.tableLayoutPanel4.Controls.Add (this.cbPassEncryptType, 1, 0);
			this.tableLayoutPanel4.Location = new System.Drawing.Point (75, 70);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel4.Size = new System.Drawing.Size (347, 29);
			this.tableLayoutPanel4.TabIndex = 8;
			// 
			// txtGeneratedKeyPass
			// 
			this.txtGeneratedKeyPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGeneratedKeyPass.Location = new System.Drawing.Point (3, 4);
			this.txtGeneratedKeyPass.Name = "txtGeneratedKeyPass";
			this.txtGeneratedKeyPass.PasswordChar = '*';
			this.txtGeneratedKeyPass.Size = new System.Drawing.Size (228, 20);
			this.txtGeneratedKeyPass.TabIndex = 9;
			// 
			// cbPassEncryptType
			// 
			this.cbPassEncryptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPassEncryptType.FormattingEnabled = true;
			this.cbPassEncryptType.Items.AddRange (new object[] {
            "Camellia 256bit",
            "Rijndael 256bit"});
			this.cbPassEncryptType.Location = new System.Drawing.Point (237, 3);
			this.cbPassEncryptType.Name = "cbPassEncryptType";
			this.cbPassEncryptType.Size = new System.Drawing.Size (107, 21);
			this.cbPassEncryptType.TabIndex = 10;
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point (3, 78);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size (69, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "パスフレーズ：";
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.SetColumnSpan (this.flowLayoutPanel2, 2);
			this.flowLayoutPanel2.Controls.Add (this.btnRegisterPrivateKey);
			this.flowLayoutPanel2.Controls.Add (this.btnDecryptPrivateKey);
			this.flowLayoutPanel2.Controls.Add (this.btnEncryptPrivateKey);
			this.flowLayoutPanel2.Location = new System.Drawing.Point (0, 35);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding (0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size (422, 35);
			this.flowLayoutPanel2.TabIndex = 10;
			// 
			// btnRegisterPrivateKey
			// 
			this.btnRegisterPrivateKey.AutoSize = true;
			this.btnRegisterPrivateKey.Location = new System.Drawing.Point (3, 3);
			this.btnRegisterPrivateKey.Name = "btnRegisterPrivateKey";
			this.btnRegisterPrivateKey.Padding = new System.Windows.Forms.Padding (3);
			this.btnRegisterPrivateKey.Size = new System.Drawing.Size (111, 29);
			this.btnRegisterPrivateKey.TabIndex = 9;
			this.btnRegisterPrivateKey.Text = "秘密鍵を登録する";
			this.btnRegisterPrivateKey.UseVisualStyleBackColor = true;
			this.btnRegisterPrivateKey.Click += new System.EventHandler (this.btnRegisterPrivateKey_Click);
			// 
			// btnDecryptPrivateKey
			// 
			this.btnDecryptPrivateKey.AutoSize = true;
			this.btnDecryptPrivateKey.Location = new System.Drawing.Point (120, 3);
			this.btnDecryptPrivateKey.Name = "btnDecryptPrivateKey";
			this.btnDecryptPrivateKey.Padding = new System.Windows.Forms.Padding (3);
			this.btnDecryptPrivateKey.Size = new System.Drawing.Size (145, 29);
			this.btnDecryptPrivateKey.TabIndex = 9;
			this.btnDecryptPrivateKey.Text = "秘密鍵の暗号を解除する";
			this.btnDecryptPrivateKey.UseVisualStyleBackColor = true;
			this.btnDecryptPrivateKey.Click += new System.EventHandler (this.btnDecryptPrivateKey_Click);
			// 
			// btnEncryptPrivateKey
			// 
			this.btnEncryptPrivateKey.AutoSize = true;
			this.btnEncryptPrivateKey.Location = new System.Drawing.Point (271, 3);
			this.btnEncryptPrivateKey.Name = "btnEncryptPrivateKey";
			this.btnEncryptPrivateKey.Padding = new System.Windows.Forms.Padding (3);
			this.btnEncryptPrivateKey.Size = new System.Drawing.Size (123, 29);
			this.btnEncryptPrivateKey.TabIndex = 9;
			this.btnEncryptPrivateKey.Text = "秘密鍵を暗号化する";
			this.btnEncryptPrivateKey.UseVisualStyleBackColor = true;
			this.btnEncryptPrivateKey.Click += new System.EventHandler (this.btnEncryptPrivateKey_Click);
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add (this.tableLayoutPanel5);
			this.tabPage4.Location = new System.Drawing.Point (4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size (422, 358);
			this.tabPage4.TabIndex = 1;
			this.tabPage4.Text = "鍵の管理";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 1;
			this.tableLayoutPanel5.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Controls.Add (this.groupBox2, 0, 1);
			this.tableLayoutPanel5.Controls.Add (this.groupBox1, 0, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point (0, 0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 2;
			this.tableLayoutPanel5.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel5.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size (422, 358);
			this.tableLayoutPanel5.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.AutoSize = true;
			this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox2.Controls.Add (this.tableLayoutPanel7);
			this.groupBox2.Location = new System.Drawing.Point (3, 110);
			this.groupBox2.Margin = new System.Windows.Forms.Padding (3, 6, 3, 3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding (10);
			this.groupBox2.Size = new System.Drawing.Size (416, 95);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "公開鍵の管理";
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.AutoSize = true;
			this.tableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel7.ColumnCount = 1;
			this.tableLayoutPanel7.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.Controls.Add (this.cbPublicKeys, 0, 0);
			this.tableLayoutPanel7.Controls.Add (this.flowLayoutPanel4, 0, 1);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point (10, 23);
			this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 2;
			this.tableLayoutPanel7.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel7.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel7.Size = new System.Drawing.Size (396, 62);
			this.tableLayoutPanel7.TabIndex = 0;
			// 
			// cbPublicKeys
			// 
			this.cbPublicKeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.cbPublicKeys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPublicKeys.FormattingEnabled = true;
			this.cbPublicKeys.Location = new System.Drawing.Point (3, 3);
			this.cbPublicKeys.Name = "cbPublicKeys";
			this.cbPublicKeys.Size = new System.Drawing.Size (390, 21);
			this.cbPublicKeys.TabIndex = 0;
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel4.AutoSize = true;
			this.flowLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel4.Controls.Add (this.btnAddPublicKey);
			this.flowLayoutPanel4.Controls.Add (this.btnPublicKeyRename);
			this.flowLayoutPanel4.Controls.Add (this.btnPublicKeyDel);
			this.flowLayoutPanel4.Controls.Add (this.btnPublicKeyCopy);
			this.flowLayoutPanel4.Location = new System.Drawing.Point (0, 27);
			this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding (0);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size (396, 35);
			this.flowLayoutPanel4.TabIndex = 1;
			// 
			// btnAddPublicKey
			// 
			this.btnAddPublicKey.AutoSize = true;
			this.btnAddPublicKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnAddPublicKey.Location = new System.Drawing.Point (3, 3);
			this.btnAddPublicKey.Name = "btnAddPublicKey";
			this.btnAddPublicKey.Padding = new System.Windows.Forms.Padding (3);
			this.btnAddPublicKey.Size = new System.Drawing.Size (47, 29);
			this.btnAddPublicKey.TabIndex = 0;
			this.btnAddPublicKey.Text = "追加";
			this.btnAddPublicKey.UseVisualStyleBackColor = true;
			this.btnAddPublicKey.Click += new System.EventHandler (this.btnAddPublicKey_Click);
			// 
			// btnPublicKeyRename
			// 
			this.btnPublicKeyRename.AutoSize = true;
			this.btnPublicKeyRename.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnPublicKeyRename.Location = new System.Drawing.Point (56, 3);
			this.btnPublicKeyRename.Name = "btnPublicKeyRename";
			this.btnPublicKeyRename.Padding = new System.Windows.Forms.Padding (3);
			this.btnPublicKeyRename.Size = new System.Drawing.Size (81, 29);
			this.btnPublicKeyRename.TabIndex = 0;
			this.btnPublicKeyRename.Text = "名前の変更";
			this.btnPublicKeyRename.UseVisualStyleBackColor = true;
			this.btnPublicKeyRename.Click += new System.EventHandler (this.btnPublicKeyRename_Click);
			// 
			// btnPublicKeyDel
			// 
			this.btnPublicKeyDel.AutoSize = true;
			this.btnPublicKeyDel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnPublicKeyDel.Location = new System.Drawing.Point (143, 3);
			this.btnPublicKeyDel.Name = "btnPublicKeyDel";
			this.btnPublicKeyDel.Padding = new System.Windows.Forms.Padding (3);
			this.btnPublicKeyDel.Size = new System.Drawing.Size (47, 29);
			this.btnPublicKeyDel.TabIndex = 0;
			this.btnPublicKeyDel.Text = "削除";
			this.btnPublicKeyDel.UseVisualStyleBackColor = true;
			this.btnPublicKeyDel.Click += new System.EventHandler (this.btnPublicKeyDel_Click);
			// 
			// btnPublicKeyCopy
			// 
			this.btnPublicKeyCopy.AutoSize = true;
			this.btnPublicKeyCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnPublicKeyCopy.Location = new System.Drawing.Point (196, 3);
			this.btnPublicKeyCopy.Name = "btnPublicKeyCopy";
			this.btnPublicKeyCopy.Padding = new System.Windows.Forms.Padding (3);
			this.btnPublicKeyCopy.Size = new System.Drawing.Size (119, 29);
			this.btnPublicKeyCopy.TabIndex = 0;
			this.btnPublicKeyCopy.Text = "クリップボードにコピー";
			this.btnPublicKeyCopy.UseVisualStyleBackColor = true;
			this.btnPublicKeyCopy.Click += new System.EventHandler (this.btnPublicKeyCopy_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add (this.tableLayoutPanel6);
			this.groupBox1.Location = new System.Drawing.Point (3, 6);
			this.groupBox1.Margin = new System.Windows.Forms.Padding (3, 6, 3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding (10);
			this.groupBox1.Size = new System.Drawing.Size (416, 95);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "秘密鍵の管理";
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 1;
			this.tableLayoutPanel6.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Controls.Add (this.cbPrivateKeys, 0, 0);
			this.tableLayoutPanel6.Controls.Add (this.flowLayoutPanel3, 0, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point (10, 23);
			this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel6.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel6.Size = new System.Drawing.Size (396, 62);
			this.tableLayoutPanel6.TabIndex = 0;
			// 
			// cbPrivateKeys
			// 
			this.cbPrivateKeys.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.cbPrivateKeys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPrivateKeys.FormattingEnabled = true;
			this.cbPrivateKeys.Location = new System.Drawing.Point (3, 3);
			this.cbPrivateKeys.Name = "cbPrivateKeys";
			this.cbPrivateKeys.Size = new System.Drawing.Size (390, 21);
			this.cbPrivateKeys.TabIndex = 0;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel3.AutoSize = true;
			this.flowLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel3.Controls.Add (this.btnAddPrivateKey);
			this.flowLayoutPanel3.Controls.Add (this.btnPrivateKeyRename);
			this.flowLayoutPanel3.Controls.Add (this.btnPrivateKeyDel);
			this.flowLayoutPanel3.Controls.Add (this.btnPrivateKeyCopy);
			this.flowLayoutPanel3.Location = new System.Drawing.Point (0, 27);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding (0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size (396, 35);
			this.flowLayoutPanel3.TabIndex = 1;
			// 
			// btnPrivateKeyRename
			// 
			this.btnPrivateKeyRename.AutoSize = true;
			this.btnPrivateKeyRename.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnPrivateKeyRename.Location = new System.Drawing.Point (56, 3);
			this.btnPrivateKeyRename.Name = "btnPrivateKeyRename";
			this.btnPrivateKeyRename.Padding = new System.Windows.Forms.Padding (3);
			this.btnPrivateKeyRename.Size = new System.Drawing.Size (81, 29);
			this.btnPrivateKeyRename.TabIndex = 0;
			this.btnPrivateKeyRename.Text = "名前の変更";
			this.btnPrivateKeyRename.UseVisualStyleBackColor = true;
			this.btnPrivateKeyRename.Click += new System.EventHandler (this.btnPrivateKeyRename_Click);
			// 
			// btnPrivateKeyDel
			// 
			this.btnPrivateKeyDel.AutoSize = true;
			this.btnPrivateKeyDel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnPrivateKeyDel.Location = new System.Drawing.Point (143, 3);
			this.btnPrivateKeyDel.Name = "btnPrivateKeyDel";
			this.btnPrivateKeyDel.Padding = new System.Windows.Forms.Padding (3);
			this.btnPrivateKeyDel.Size = new System.Drawing.Size (47, 29);
			this.btnPrivateKeyDel.TabIndex = 0;
			this.btnPrivateKeyDel.Text = "削除";
			this.btnPrivateKeyDel.UseVisualStyleBackColor = true;
			this.btnPrivateKeyDel.Click += new System.EventHandler (this.btnPrivateKeyDel_Click);
			// 
			// btnPrivateKeyCopy
			// 
			this.btnPrivateKeyCopy.AutoSize = true;
			this.btnPrivateKeyCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnPrivateKeyCopy.Location = new System.Drawing.Point (196, 3);
			this.btnPrivateKeyCopy.Name = "btnPrivateKeyCopy";
			this.btnPrivateKeyCopy.Padding = new System.Windows.Forms.Padding (3);
			this.btnPrivateKeyCopy.Size = new System.Drawing.Size (119, 29);
			this.btnPrivateKeyCopy.TabIndex = 0;
			this.btnPrivateKeyCopy.Text = "クリップボードにコピー";
			this.btnPrivateKeyCopy.UseVisualStyleBackColor = true;
			this.btnPrivateKeyCopy.Click += new System.EventHandler (this.btnPrivateKeyCopy_Click);
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add (this.tableLayoutPanel8);
			this.tabPage5.Location = new System.Drawing.Point (4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding (3);
			this.tabPage5.Size = new System.Drawing.Size (422, 358);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "暗号化";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.ColumnCount = 2;
			this.tableLayoutPanel8.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel8.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Controls.Add (this.txtEncryptPlain, 1, 3);
			this.tableLayoutPanel8.Controls.Add (this.label6, 0, 0);
			this.tableLayoutPanel8.Controls.Add (this.flowLayoutPanel5, 0, 2);
			this.tableLayoutPanel8.Controls.Add (this.txtEncryptCipher, 1, 4);
			this.tableLayoutPanel8.Controls.Add (this.label15, 0, 1);
			this.tableLayoutPanel8.Controls.Add (this.cbEncryptCrypto, 1, 1);
			this.tableLayoutPanel8.Controls.Add (this.label14, 0, 3);
			this.tableLayoutPanel8.Controls.Add (this.label13, 0, 4);
			this.tableLayoutPanel8.Controls.Add (this.cbPublicKeys2, 1, 0);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point (3, 3);
			this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 5;
			this.tableLayoutPanel8.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel8.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel8.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel8.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel8.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel8.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size (416, 352);
			this.tableLayoutPanel8.TabIndex = 1;
			// 
			// txtEncryptPlain
			// 
			this.txtEncryptPlain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEncryptPlain.Location = new System.Drawing.Point (77, 98);
			this.txtEncryptPlain.Multiline = true;
			this.txtEncryptPlain.Name = "txtEncryptPlain";
			this.txtEncryptPlain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtEncryptPlain.Size = new System.Drawing.Size (336, 122);
			this.txtEncryptPlain.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point (3, 9);
			this.label6.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size (49, 13);
			this.label6.TabIndex = 0;
			this.label6.Text = "公開鍵：";
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.flowLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel8.SetColumnSpan (this.flowLayoutPanel5, 2);
			this.flowLayoutPanel5.Controls.Add (this.btnEncryptText);
			this.flowLayoutPanel5.Controls.Add (this.btnEncryptFile);
			this.flowLayoutPanel5.Location = new System.Drawing.Point (121, 57);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size (173, 35);
			this.flowLayoutPanel5.TabIndex = 10;
			// 
			// btnEncryptText
			// 
			this.btnEncryptText.AutoSize = true;
			this.btnEncryptText.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnEncryptText.Location = new System.Drawing.Point (3, 3);
			this.btnEncryptText.Name = "btnEncryptText";
			this.btnEncryptText.Padding = new System.Windows.Forms.Padding (3);
			this.btnEncryptText.Size = new System.Drawing.Size (59, 29);
			this.btnEncryptText.TabIndex = 0;
			this.btnEncryptText.Text = "暗号化";
			this.btnEncryptText.UseVisualStyleBackColor = true;
			this.btnEncryptText.Click += new System.EventHandler (this.btnEncryptText_Click);
			// 
			// btnEncryptFile
			// 
			this.btnEncryptFile.AutoSize = true;
			this.btnEncryptFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnEncryptFile.Enabled = false;
			this.btnEncryptFile.Location = new System.Drawing.Point (68, 3);
			this.btnEncryptFile.Name = "btnEncryptFile";
			this.btnEncryptFile.Padding = new System.Windows.Forms.Padding (3);
			this.btnEncryptFile.Size = new System.Drawing.Size (102, 29);
			this.btnEncryptFile.TabIndex = 0;
			this.btnEncryptFile.Text = "ファイルを暗号化";
			this.btnEncryptFile.UseVisualStyleBackColor = true;
			// 
			// txtEncryptCipher
			// 
			this.txtEncryptCipher.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEncryptCipher.Location = new System.Drawing.Point (77, 226);
			this.txtEncryptCipher.Multiline = true;
			this.txtEncryptCipher.Name = "txtEncryptCipher";
			this.txtEncryptCipher.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtEncryptCipher.Size = new System.Drawing.Size (336, 123);
			this.txtEncryptCipher.TabIndex = 11;
			// 
			// label15
			// 
			this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point (3, 27);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size (68, 26);
			this.label15.TabIndex = 6;
			this.label15.Text = "暗号化\r\nアルゴリズム：";
			// 
			// cbEncryptCrypto
			// 
			this.cbEncryptCrypto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbEncryptCrypto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEncryptCrypto.FormattingEnabled = true;
			this.cbEncryptCrypto.Items.AddRange (new object[] {
            "XOR",
            "Camellia 128bit",
            "Camellia 256bit",
            "Rijndael 128bit",
            "Rijndael 256bit"});
			this.cbEncryptCrypto.Location = new System.Drawing.Point (77, 30);
			this.cbEncryptCrypto.Name = "cbEncryptCrypto";
			this.cbEncryptCrypto.Size = new System.Drawing.Size (336, 21);
			this.cbEncryptCrypto.TabIndex = 12;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point (3, 100);
			this.label14.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size (37, 13);
			this.label14.TabIndex = 0;
			this.label14.Text = "平文：";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point (3, 228);
			this.label13.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size (49, 13);
			this.label13.TabIndex = 0;
			this.label13.Text = "暗号文：";
			// 
			// cbPublicKeys2
			// 
			this.cbPublicKeys2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbPublicKeys2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPublicKeys2.FormattingEnabled = true;
			this.cbPublicKeys2.Location = new System.Drawing.Point (77, 3);
			this.cbPublicKeys2.Name = "cbPublicKeys2";
			this.cbPublicKeys2.Size = new System.Drawing.Size (336, 21);
			this.cbPublicKeys2.TabIndex = 13;
			// 
			// tabPage7
			// 
			this.tabPage7.Controls.Add (this.tableLayoutPanel9);
			this.tabPage7.Location = new System.Drawing.Point (4, 22);
			this.tabPage7.Name = "tabPage7";
			this.tabPage7.Padding = new System.Windows.Forms.Padding (3);
			this.tabPage7.Size = new System.Drawing.Size (422, 358);
			this.tabPage7.TabIndex = 5;
			this.tabPage7.Text = "復号";
			this.tabPage7.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.ColumnCount = 2;
			this.tableLayoutPanel9.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel9.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Controls.Add (this.txtDecryptCipher, 1, 3);
			this.tableLayoutPanel9.Controls.Add (this.txtDecryptKeyPass, 1, 1);
			this.tableLayoutPanel9.Controls.Add (this.label16, 0, 1);
			this.tableLayoutPanel9.Controls.Add (this.label17, 0, 0);
			this.tableLayoutPanel9.Controls.Add (this.flowLayoutPanel6, 0, 2);
			this.tableLayoutPanel9.Controls.Add (this.txtDecryptPlain, 1, 4);
			this.tableLayoutPanel9.Controls.Add (this.label19, 0, 3);
			this.tableLayoutPanel9.Controls.Add (this.label20, 0, 4);
			this.tableLayoutPanel9.Controls.Add (this.cbPrivateKeys2, 1, 0);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point (3, 3);
			this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 5;
			this.tableLayoutPanel9.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel9.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel9.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel9.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel9.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel9.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel9.Size = new System.Drawing.Size (416, 352);
			this.tableLayoutPanel9.TabIndex = 2;
			// 
			// txtDecryptCipher
			// 
			this.txtDecryptCipher.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDecryptCipher.Location = new System.Drawing.Point (78, 97);
			this.txtDecryptCipher.Multiline = true;
			this.txtDecryptCipher.Name = "txtDecryptCipher";
			this.txtDecryptCipher.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDecryptCipher.Size = new System.Drawing.Size (335, 123);
			this.txtDecryptCipher.TabIndex = 11;
			// 
			// txtDecryptKeyPass
			// 
			this.txtDecryptKeyPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDecryptKeyPass.Location = new System.Drawing.Point (78, 30);
			this.txtDecryptKeyPass.Name = "txtDecryptKeyPass";
			this.txtDecryptKeyPass.PasswordChar = '*';
			this.txtDecryptKeyPass.Size = new System.Drawing.Size (335, 20);
			this.txtDecryptKeyPass.TabIndex = 9;
			// 
			// label16
			// 
			this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point (3, 33);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size (69, 13);
			this.label16.TabIndex = 6;
			this.label16.Text = "パスフレーズ：";
			// 
			// label17
			// 
			this.label17.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point (3, 9);
			this.label17.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size (49, 13);
			this.label17.TabIndex = 0;
			this.label17.Text = "秘密鍵：";
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.flowLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel9.SetColumnSpan (this.flowLayoutPanel6, 2);
			this.flowLayoutPanel6.Controls.Add (this.btnDecryptText);
			this.flowLayoutPanel6.Controls.Add (this.btnDecryptFile);
			this.flowLayoutPanel6.Location = new System.Drawing.Point (133, 56);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size (149, 35);
			this.flowLayoutPanel6.TabIndex = 10;
			// 
			// btnDecryptText
			// 
			this.btnDecryptText.AutoSize = true;
			this.btnDecryptText.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnDecryptText.Location = new System.Drawing.Point (3, 3);
			this.btnDecryptText.Name = "btnDecryptText";
			this.btnDecryptText.Padding = new System.Windows.Forms.Padding (3);
			this.btnDecryptText.Size = new System.Drawing.Size (47, 29);
			this.btnDecryptText.TabIndex = 0;
			this.btnDecryptText.Text = "復号";
			this.btnDecryptText.UseVisualStyleBackColor = true;
			this.btnDecryptText.Click += new System.EventHandler (this.btnDecryptText_Click);
			// 
			// btnDecryptFile
			// 
			this.btnDecryptFile.AutoSize = true;
			this.btnDecryptFile.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnDecryptFile.Enabled = false;
			this.btnDecryptFile.Location = new System.Drawing.Point (56, 3);
			this.btnDecryptFile.Name = "btnDecryptFile";
			this.btnDecryptFile.Padding = new System.Windows.Forms.Padding (3);
			this.btnDecryptFile.Size = new System.Drawing.Size (90, 29);
			this.btnDecryptFile.TabIndex = 0;
			this.btnDecryptFile.Text = "ファイルを復号";
			this.btnDecryptFile.UseVisualStyleBackColor = true;
			// 
			// txtDecryptPlain
			// 
			this.txtDecryptPlain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtDecryptPlain.Location = new System.Drawing.Point (78, 226);
			this.txtDecryptPlain.Multiline = true;
			this.txtDecryptPlain.Name = "txtDecryptPlain";
			this.txtDecryptPlain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDecryptPlain.Size = new System.Drawing.Size (335, 123);
			this.txtDecryptPlain.TabIndex = 11;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point (3, 99);
			this.label19.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size (49, 13);
			this.label19.TabIndex = 0;
			this.label19.Text = "暗号文：";
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point (3, 228);
			this.label20.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size (37, 13);
			this.label20.TabIndex = 0;
			this.label20.Text = "平文：";
			// 
			// cbPrivateKeys2
			// 
			this.cbPrivateKeys2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbPrivateKeys2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPrivateKeys2.FormattingEnabled = true;
			this.cbPrivateKeys2.Location = new System.Drawing.Point (78, 3);
			this.cbPrivateKeys2.Name = "cbPrivateKeys2";
			this.cbPrivateKeys2.Size = new System.Drawing.Size (335, 21);
			this.cbPrivateKeys2.TabIndex = 13;
			// 
			// btnAddPrivateKey
			// 
			this.btnAddPrivateKey.AutoSize = true;
			this.btnAddPrivateKey.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnAddPrivateKey.Location = new System.Drawing.Point (3, 3);
			this.btnAddPrivateKey.Name = "btnAddPrivateKey";
			this.btnAddPrivateKey.Padding = new System.Windows.Forms.Padding (3);
			this.btnAddPrivateKey.Size = new System.Drawing.Size (47, 29);
			this.btnAddPrivateKey.TabIndex = 0;
			this.btnAddPrivateKey.Text = "追加";
			this.btnAddPrivateKey.UseVisualStyleBackColor = true;
			this.btnAddPrivateKey.Click += new System.EventHandler (this.btnAddPrivateKey_Click);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (444, 416);
			this.Controls.Add (this.tabControl1);
			this.Name = "MainWindow";
			this.Text = "CryptoFrontend 0.1 Alpha.3";
			this.tabControl1.ResumeLayout (false);
			this.tabPage1.ResumeLayout (false);
			this.tabPage2.ResumeLayout (false);
			this.tabControl2.ResumeLayout (false);
			this.tabPage3.ResumeLayout (false);
			this.tableLayoutPanel1.ResumeLayout (false);
			this.tableLayoutPanel1.PerformLayout ();
			this.tableLayoutPanel3.ResumeLayout (false);
			this.tableLayoutPanel3.PerformLayout ();
			this.tableLayoutPanel4.ResumeLayout (false);
			this.tableLayoutPanel4.PerformLayout ();
			this.flowLayoutPanel2.ResumeLayout (false);
			this.flowLayoutPanel2.PerformLayout ();
			this.tabPage4.ResumeLayout (false);
			this.tableLayoutPanel5.ResumeLayout (false);
			this.tableLayoutPanel5.PerformLayout ();
			this.groupBox2.ResumeLayout (false);
			this.groupBox2.PerformLayout ();
			this.tableLayoutPanel7.ResumeLayout (false);
			this.tableLayoutPanel7.PerformLayout ();
			this.flowLayoutPanel4.ResumeLayout (false);
			this.flowLayoutPanel4.PerformLayout ();
			this.groupBox1.ResumeLayout (false);
			this.groupBox1.PerformLayout ();
			this.tableLayoutPanel6.ResumeLayout (false);
			this.tableLayoutPanel6.PerformLayout ();
			this.flowLayoutPanel3.ResumeLayout (false);
			this.flowLayoutPanel3.PerformLayout ();
			this.tabPage5.ResumeLayout (false);
			this.tableLayoutPanel8.ResumeLayout (false);
			this.tableLayoutPanel8.PerformLayout ();
			this.flowLayoutPanel5.ResumeLayout (false);
			this.flowLayoutPanel5.PerformLayout ();
			this.tabPage7.ResumeLayout (false);
			this.tableLayoutPanel9.ResumeLayout (false);
			this.tableLayoutPanel9.PerformLayout ();
			this.flowLayoutPanel6.ResumeLayout (false);
			this.flowLayoutPanel6.PerformLayout ();
			this.ResumeLayout (false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.ComboBox cbKeyType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnKeyGenerate;
		private System.Windows.Forms.TextBox txtGeneratedKey;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtGeneratedPublicKey;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnPublicKeyGenerate;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TextBox txtGeneratedKeyPass;
		private System.Windows.Forms.ComboBox cbPassEncryptType;
		private System.Windows.Forms.Button btnRegisterPrivateKey;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Button btnDecryptPrivateKey;
		private System.Windows.Forms.Button btnEncryptPrivateKey;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.ComboBox cbPrivateKeys;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Button btnPrivateKeyRename;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.ComboBox cbPublicKeys;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		private System.Windows.Forms.Button btnAddPublicKey;
		private System.Windows.Forms.Button btnPublicKeyRename;
		private System.Windows.Forms.Button btnPublicKeyDel;
		private System.Windows.Forms.Button btnPrivateKeyDel;
		private System.Windows.Forms.Button btnPublicKeyCopy;
		private System.Windows.Forms.Button btnPrivateKeyCopy;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private System.Windows.Forms.TextBox txtEncryptPlain;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.Button btnEncryptText;
		private System.Windows.Forms.Button btnEncryptFile;
		private System.Windows.Forms.TextBox txtEncryptCipher;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.ComboBox cbEncryptCrypto;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.ComboBox cbPublicKeys2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.TextBox txtDecryptCipher;
		private System.Windows.Forms.TextBox txtDecryptKeyPass;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		private System.Windows.Forms.Button btnDecryptText;
		private System.Windows.Forms.Button btnDecryptFile;
		private System.Windows.Forms.TextBox txtDecryptPlain;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.ComboBox cbPrivateKeys2;
		private System.Windows.Forms.Button btnAddPrivateKey;
	}
}

