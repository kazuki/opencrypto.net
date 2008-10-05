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
			this.label4 = new System.Windows.Forms.Label ();
			this.txtGeneratedKeyPass = new System.Windows.Forms.TextBox ();
			this.tabPage4 = new System.Windows.Forms.TabPage ();
			this.label5 = new System.Windows.Forms.Label ();
			this.tabPage5 = new System.Windows.Forms.TabPage ();
			this.label6 = new System.Windows.Forms.Label ();
			this.tabPage6 = new System.Windows.Forms.TabPage ();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel ();
			this.txtEncryptionCipher = new System.Windows.Forms.TextBox ();
			this.txtEncryptionPass = new System.Windows.Forms.TextBox ();
			this.label8 = new System.Windows.Forms.Label ();
			this.label7 = new System.Windows.Forms.Label ();
			this.txtEncryptionKey = new System.Windows.Forms.TextBox ();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel ();
			this.btnEncryption = new System.Windows.Forms.Button ();
			this.btnDecryption = new System.Windows.Forms.Button ();
			this.btnFileEncryption = new System.Windows.Forms.Button ();
			this.button1 = new System.Windows.Forms.Button ();
			this.label9 = new System.Windows.Forms.Label ();
			this.label10 = new System.Windows.Forms.Label ();
			this.txtEncryptionPlain = new System.Windows.Forms.TextBox ();
			this.tabControl1.SuspendLayout ();
			this.tabPage1.SuspendLayout ();
			this.tabPage2.SuspendLayout ();
			this.tabControl2.SuspendLayout ();
			this.tabPage3.SuspendLayout ();
			this.tableLayoutPanel1.SuspendLayout ();
			this.tableLayoutPanel3.SuspendLayout ();
			this.tabPage4.SuspendLayout ();
			this.tabPage5.SuspendLayout ();
			this.tabPage6.SuspendLayout ();
			this.tableLayoutPanel2.SuspendLayout ();
			this.flowLayoutPanel1.SuspendLayout ();
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
			this.tabControl1.Size = new System.Drawing.Size (368, 318);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add (this.label11);
			this.tabPage1.Location = new System.Drawing.Point (4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding (3);
			this.tabPage1.Size = new System.Drawing.Size (360, 292);
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
			this.label11.Size = new System.Drawing.Size (354, 286);
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
			this.tabPage2.Size = new System.Drawing.Size (360, 292);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "公開鍵暗号";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tabControl2
			// 
			this.tabControl2.Controls.Add (this.tabPage3);
			this.tabControl2.Controls.Add (this.tabPage4);
			this.tabControl2.Controls.Add (this.tabPage5);
			this.tabControl2.Controls.Add (this.tabPage6);
			this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl2.Location = new System.Drawing.Point (3, 3);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size (354, 286);
			this.tabControl2.TabIndex = 0;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add (this.tableLayoutPanel1);
			this.tabPage3.Location = new System.Drawing.Point (4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size (346, 260);
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
			this.tableLayoutPanel1.Controls.Add (this.txtGeneratedPublicKey, 1, 3);
			this.tableLayoutPanel1.Controls.Add (this.txtGeneratedKey, 1, 2);
			this.tableLayoutPanel1.Controls.Add (this.label3, 0, 3);
			this.tableLayoutPanel1.Controls.Add (this.label1, 0, 2);
			this.tableLayoutPanel1.Controls.Add (this.label4, 0, 1);
			this.tableLayoutPanel1.Controls.Add (this.txtGeneratedKeyPass, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point (0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size (346, 260);
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
			this.tableLayoutPanel3.Size = new System.Drawing.Size (346, 35);
			this.tableLayoutPanel3.TabIndex = 4;
			// 
			// cbKeyType
			// 
			this.cbKeyType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbKeyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbKeyType.FormattingEnabled = true;
			this.cbKeyType.Items.AddRange (new object[] {
            "secp112r1",
            "secp112r2",
            "secp128r1",
            "secp128r2",
            "secp160r1 (RSA1024bit相当)",
            "secp160r2 (RSA1024bit相当)",
            "secp192r1",
            "secp224r1 (RSA2048bit相当)",
            "secp256r1 (RSA3072bit相当)",
            "secp384r1",
            "secp521r1"});
			this.cbKeyType.Location = new System.Drawing.Point (244, 7);
			this.cbKeyType.Name = "cbKeyType";
			this.cbKeyType.Size = new System.Drawing.Size (99, 21);
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
			this.txtGeneratedPublicKey.Location = new System.Drawing.Point (78, 163);
			this.txtGeneratedPublicKey.Multiline = true;
			this.txtGeneratedPublicKey.Name = "txtGeneratedPublicKey";
			this.txtGeneratedPublicKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtGeneratedPublicKey.Size = new System.Drawing.Size (265, 94);
			this.txtGeneratedPublicKey.TabIndex = 7;
			// 
			// txtGeneratedKey
			// 
			this.txtGeneratedKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtGeneratedKey.Location = new System.Drawing.Point (78, 64);
			this.txtGeneratedKey.Multiline = true;
			this.txtGeneratedKey.Name = "txtGeneratedKey";
			this.txtGeneratedKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtGeneratedKey.Size = new System.Drawing.Size (265, 93);
			this.txtGeneratedKey.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point (3, 165);
			this.label3.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size (49, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "公開鍵：";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point (3, 66);
			this.label1.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (49, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "秘密鍵：";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point (3, 41);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size (69, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "パスフレーズ：";
			// 
			// txtGeneratedKeyPass
			// 
			this.txtGeneratedKeyPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtGeneratedKeyPass.Location = new System.Drawing.Point (78, 38);
			this.txtGeneratedKeyPass.Name = "txtGeneratedKeyPass";
			this.txtGeneratedKeyPass.Size = new System.Drawing.Size (265, 20);
			this.txtGeneratedKeyPass.TabIndex = 8;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add (this.label5);
			this.tabPage4.Location = new System.Drawing.Point (4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size (346, 260);
			this.tabPage4.TabIndex = 1;
			this.tabPage4.Text = "ディジタル署名の作成・検証";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point (0, 0);
			this.label5.Margin = new System.Windows.Forms.Padding (0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size (346, 260);
			this.label5.TabIndex = 0;
			this.label5.Text = "未実装";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add (this.label6);
			this.tabPage5.Location = new System.Drawing.Point (4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Size = new System.Drawing.Size (346, 260);
			this.tabPage5.TabIndex = 2;
			this.tabPage5.Text = "鍵交換";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point (0, 0);
			this.label6.Margin = new System.Windows.Forms.Padding (0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size (346, 260);
			this.label6.TabIndex = 1;
			this.label6.Text = "未実装";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabPage6
			// 
			this.tabPage6.Controls.Add (this.tableLayoutPanel2);
			this.tabPage6.Location = new System.Drawing.Point (4, 22);
			this.tabPage6.Name = "tabPage6";
			this.tabPage6.Size = new System.Drawing.Size (346, 260);
			this.tabPage6.TabIndex = 3;
			this.tabPage6.Text = "暗号化・復号";
			this.tabPage6.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel2.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add (this.txtEncryptionCipher, 1, 3);
			this.tableLayoutPanel2.Controls.Add (this.txtEncryptionPass, 1, 1);
			this.tableLayoutPanel2.Controls.Add (this.label8, 0, 1);
			this.tableLayoutPanel2.Controls.Add (this.label7, 0, 0);
			this.tableLayoutPanel2.Controls.Add (this.txtEncryptionKey, 1, 0);
			this.tableLayoutPanel2.Controls.Add (this.flowLayoutPanel1, 0, 2);
			this.tableLayoutPanel2.Controls.Add (this.label9, 0, 3);
			this.tableLayoutPanel2.Controls.Add (this.label10, 0, 4);
			this.tableLayoutPanel2.Controls.Add (this.txtEncryptionPlain, 1, 4);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point (0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 5;
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size (346, 260);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// txtEncryptionCipher
			// 
			this.txtEncryptionCipher.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEncryptionCipher.Location = new System.Drawing.Point (78, 142);
			this.txtEncryptionCipher.Multiline = true;
			this.txtEncryptionCipher.Name = "txtEncryptionCipher";
			this.txtEncryptionCipher.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtEncryptionCipher.Size = new System.Drawing.Size (265, 54);
			this.txtEncryptionCipher.TabIndex = 11;
			// 
			// txtEncryptionPass
			// 
			this.txtEncryptionPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtEncryptionPass.Location = new System.Drawing.Point (78, 75);
			this.txtEncryptionPass.Name = "txtEncryptionPass";
			this.txtEncryptionPass.Size = new System.Drawing.Size (265, 20);
			this.txtEncryptionPass.TabIndex = 9;
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point (3, 78);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size (69, 13);
			this.label8.TabIndex = 6;
			this.label8.Text = "パスフレーズ：";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point (3, 5);
			this.label7.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size (25, 13);
			this.label7.TabIndex = 0;
			this.label7.Text = "鍵：";
			// 
			// txtEncryptionKey
			// 
			this.txtEncryptionKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEncryptionKey.Location = new System.Drawing.Point (78, 3);
			this.txtEncryptionKey.Multiline = true;
			this.txtEncryptionKey.Name = "txtEncryptionKey";
			this.txtEncryptionKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtEncryptionKey.Size = new System.Drawing.Size (265, 66);
			this.txtEncryptionKey.TabIndex = 1;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.flowLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel2.SetColumnSpan (this.flowLayoutPanel1, 2);
			this.flowLayoutPanel1.Controls.Add (this.btnEncryption);
			this.flowLayoutPanel1.Controls.Add (this.btnDecryption);
			this.flowLayoutPanel1.Controls.Add (this.btnFileEncryption);
			this.flowLayoutPanel1.Controls.Add (this.button1);
			this.flowLayoutPanel1.Location = new System.Drawing.Point (12, 101);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size (322, 35);
			this.flowLayoutPanel1.TabIndex = 10;
			// 
			// btnEncryption
			// 
			this.btnEncryption.AutoSize = true;
			this.btnEncryption.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnEncryption.Location = new System.Drawing.Point (3, 3);
			this.btnEncryption.Name = "btnEncryption";
			this.btnEncryption.Padding = new System.Windows.Forms.Padding (3);
			this.btnEncryption.Size = new System.Drawing.Size (59, 29);
			this.btnEncryption.TabIndex = 0;
			this.btnEncryption.Text = "暗号化";
			this.btnEncryption.UseVisualStyleBackColor = true;
			this.btnEncryption.Click += new System.EventHandler (this.btnEncryption_Click);
			// 
			// btnDecryption
			// 
			this.btnDecryption.AutoSize = true;
			this.btnDecryption.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnDecryption.Location = new System.Drawing.Point (68, 3);
			this.btnDecryption.Name = "btnDecryption";
			this.btnDecryption.Padding = new System.Windows.Forms.Padding (3);
			this.btnDecryption.Size = new System.Drawing.Size (47, 29);
			this.btnDecryption.TabIndex = 0;
			this.btnDecryption.Text = "復号";
			this.btnDecryption.UseVisualStyleBackColor = true;
			this.btnDecryption.Click += new System.EventHandler (this.btnDecryption_Click);
			// 
			// btnFileEncryption
			// 
			this.btnFileEncryption.AutoSize = true;
			this.btnFileEncryption.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnFileEncryption.Enabled = false;
			this.btnFileEncryption.Location = new System.Drawing.Point (121, 3);
			this.btnFileEncryption.Name = "btnFileEncryption";
			this.btnFileEncryption.Padding = new System.Windows.Forms.Padding (3);
			this.btnFileEncryption.Size = new System.Drawing.Size (102, 29);
			this.btnFileEncryption.TabIndex = 0;
			this.btnFileEncryption.Text = "ファイルを暗号化";
			this.btnFileEncryption.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.AutoSize = true;
			this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button1.Enabled = false;
			this.button1.Location = new System.Drawing.Point (229, 3);
			this.button1.Name = "button1";
			this.button1.Padding = new System.Windows.Forms.Padding (3);
			this.button1.Size = new System.Drawing.Size (90, 29);
			this.button1.TabIndex = 0;
			this.button1.Text = "ファイルを復号";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point (3, 144);
			this.label9.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size (49, 13);
			this.label9.TabIndex = 0;
			this.label9.Text = "暗号文：";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point (3, 204);
			this.label10.Margin = new System.Windows.Forms.Padding (3, 5, 3, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size (37, 13);
			this.label10.TabIndex = 0;
			this.label10.Text = "平文：";
			// 
			// txtEncryptionPlain
			// 
			this.txtEncryptionPlain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
							| System.Windows.Forms.AnchorStyles.Left)
							| System.Windows.Forms.AnchorStyles.Right)));
			this.txtEncryptionPlain.Location = new System.Drawing.Point (78, 202);
			this.txtEncryptionPlain.Multiline = true;
			this.txtEncryptionPlain.Name = "txtEncryptionPlain";
			this.txtEncryptionPlain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtEncryptionPlain.Size = new System.Drawing.Size (265, 55);
			this.txtEncryptionPlain.TabIndex = 11;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (368, 318);
			this.Controls.Add (this.tabControl1);
			this.Name = "MainWindow";
			this.Text = "CryptoFrontend 0.1 Alpha.1";
			this.tabControl1.ResumeLayout (false);
			this.tabPage1.ResumeLayout (false);
			this.tabPage2.ResumeLayout (false);
			this.tabControl2.ResumeLayout (false);
			this.tabPage3.ResumeLayout (false);
			this.tableLayoutPanel1.ResumeLayout (false);
			this.tableLayoutPanel1.PerformLayout ();
			this.tableLayoutPanel3.ResumeLayout (false);
			this.tableLayoutPanel3.PerformLayout ();
			this.tabPage4.ResumeLayout (false);
			this.tabPage5.ResumeLayout (false);
			this.tabPage6.ResumeLayout (false);
			this.tableLayoutPanel2.ResumeLayout (false);
			this.tableLayoutPanel2.PerformLayout ();
			this.flowLayoutPanel1.ResumeLayout (false);
			this.flowLayoutPanel1.PerformLayout ();
			this.ResumeLayout (false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.TabPage tabPage6;
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
		private System.Windows.Forms.TextBox txtGeneratedKeyPass;
		private System.Windows.Forms.Button btnPublicKeyGenerate;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtEncryptionKey;
		private System.Windows.Forms.TextBox txtEncryptionPass;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnEncryption;
		private System.Windows.Forms.Button btnDecryption;
		private System.Windows.Forms.Button btnFileEncryption;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox txtEncryptionCipher;
		private System.Windows.Forms.TextBox txtEncryptionPlain;
		private System.Windows.Forms.Label label11;
	}
}

