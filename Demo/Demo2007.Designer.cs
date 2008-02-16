namespace Demo
{
	partial class Demo2007
	{
		/// <summary>
		/// 必要なデザイナ変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows フォーム デザイナで生成されたコード

		/// <summary>
		/// デザイナ サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディタで変更しないでください。
		/// </summary>
		private void InitializeComponent ()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl ();
			this.tabPage1 = new System.Windows.Forms.TabPage ();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel ();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel ();
			this.nudCamellia = new System.Windows.Forms.NumericUpDown ();
			this.btnStartCamelliaEncryptionTime = new System.Windows.Forms.Button ();
			this.tabPage2 = new System.Windows.Forms.TabPage ();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel ();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel ();
			this.btnStartRijndaelEncryptionTime = new System.Windows.Forms.Button ();
			this.nudRijndael = new System.Windows.Forms.NumericUpDown ();
			this.tabPage3 = new System.Windows.Forms.TabPage ();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel ();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel ();
			this.btnStartEncryptionSpeed = new System.Windows.Forms.Button ();
			this.nudEncryptionSpeed = new System.Windows.Forms.NumericUpDown ();
			this.tabPage4 = new System.Windows.Forms.TabPage ();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel ();
			this.label1 = new System.Windows.Forms.Label ();
			this.lblBCSign = new System.Windows.Forms.Label ();
			this.lblBCVerify = new System.Windows.Forms.Label ();
			this.label5 = new System.Windows.Forms.Label ();
			this.lblOCSign = new System.Windows.Forms.Label ();
			this.lblOCVerify = new System.Windows.Forms.Label ();
			this.pbEcdsaSignBC = new System.Windows.Forms.ProgressBar ();
			this.btnStartECDSA = new System.Windows.Forms.Button ();
			this.pbEcdsaVerifyBC = new System.Windows.Forms.ProgressBar ();
			this.pbEcdsaSignOC = new System.Windows.Forms.ProgressBar ();
			this.pbEcdsaVerifyOC = new System.Windows.Forms.ProgressBar ();
			this.label2 = new System.Windows.Forms.Label ();
			this.etCamelliaBC = new Demo.EncryptionTime ();
			this.etCamelliaOC = new Demo.EncryptionTime ();
			this.etRijndaelEB = new Demo.EncryptionTime ();
			this.etRijndaelBC = new Demo.EncryptionTime ();
			this.etRijndaelOC = new Demo.EncryptionTime ();
			this.encryptionSpeed1 = new Demo.EncryptionSpeed ();
			this.encryptionSpeed2 = new Demo.EncryptionSpeed ();
			this.encryptionSpeed3 = new Demo.EncryptionSpeed ();
			this.encryptionSpeed4 = new Demo.EncryptionSpeed ();
			this.encryptionSpeed5 = new Demo.EncryptionSpeed ();
			this.tabControl1.SuspendLayout ();
			this.tabPage1.SuspendLayout ();
			this.tableLayoutPanel1.SuspendLayout ();
			this.tableLayoutPanel5.SuspendLayout ();
			((System.ComponentModel.ISupportInitialize)(this.nudCamellia)).BeginInit ();
			this.tabPage2.SuspendLayout ();
			this.tableLayoutPanel2.SuspendLayout ();
			this.tableLayoutPanel6.SuspendLayout ();
			((System.ComponentModel.ISupportInitialize)(this.nudRijndael)).BeginInit ();
			this.tabPage3.SuspendLayout ();
			this.tableLayoutPanel3.SuspendLayout ();
			this.tableLayoutPanel7.SuspendLayout ();
			((System.ComponentModel.ISupportInitialize)(this.nudEncryptionSpeed)).BeginInit ();
			this.tabPage4.SuspendLayout ();
			this.tableLayoutPanel4.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add (this.tabPage1);
			this.tabControl1.Controls.Add (this.tabPage2);
			this.tabControl1.Controls.Add (this.tabPage3);
			this.tabControl1.Controls.Add (this.tabPage4);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point (0, 0);
			this.tabControl1.Margin = new System.Windows.Forms.Padding (5, 6, 5, 6);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size (948, 664);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add (this.tableLayoutPanel1);
			this.tabPage1.Location = new System.Drawing.Point (4, 33);
			this.tabPage1.Margin = new System.Windows.Forms.Padding (5, 6, 5, 6);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding (5, 6, 5, 6);
			this.tabPage1.Size = new System.Drawing.Size (940, 627);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Camellia (暗号化所要時間)";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add (this.etCamelliaBC, 0, 0);
			this.tableLayoutPanel1.Controls.Add (this.etCamelliaOC, 0, 2);
			this.tableLayoutPanel1.Controls.Add (this.tableLayoutPanel5, 0, 4);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point (5, 6);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding (0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel1.Size = new System.Drawing.Size (930, 615);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel5.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel5.Controls.Add (this.btnStartCamelliaEncryptionTime, 1, 0);
			this.tableLayoutPanel5.Controls.Add (this.nudCamellia, 0, 0);
			this.tableLayoutPanel5.Location = new System.Drawing.Point (753, 203);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 1;
			this.tableLayoutPanel5.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size (174, 65);
			this.tableLayoutPanel5.TabIndex = 4;
			// 
			// nudCamellia
			// 
			this.nudCamellia.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nudCamellia.Location = new System.Drawing.Point (3, 3);
			this.nudCamellia.Maximum = new decimal (new int[] {
            8,
            0,
            0,
            0});
			this.nudCamellia.Minimum = new decimal (new int[] {
            1,
            0,
            0,
            0});
			this.nudCamellia.Name = "nudCamellia";
			this.nudCamellia.Size = new System.Drawing.Size (38, 31);
			this.nudCamellia.TabIndex = 5;
			this.nudCamellia.Value = new decimal (new int[] {
            1,
            0,
            0,
            0});
			// 
			// btnStartCamelliaEncryptionTime
			// 
			this.btnStartCamelliaEncryptionTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStartCamelliaEncryptionTime.Location = new System.Drawing.Point (66, 0);
			this.btnStartCamelliaEncryptionTime.Margin = new System.Windows.Forms.Padding (0);
			this.btnStartCamelliaEncryptionTime.Name = "btnStartCamelliaEncryptionTime";
			this.btnStartCamelliaEncryptionTime.Size = new System.Drawing.Size (108, 40);
			this.btnStartCamelliaEncryptionTime.TabIndex = 2;
			this.btnStartCamelliaEncryptionTime.Text = "Start";
			this.btnStartCamelliaEncryptionTime.UseVisualStyleBackColor = true;
			this.btnStartCamelliaEncryptionTime.Click += new System.EventHandler (this.btnStartCamelliaEncryptionTime_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add (this.tableLayoutPanel2);
			this.tabPage2.Location = new System.Drawing.Point (4, 33);
			this.tabPage2.Margin = new System.Windows.Forms.Padding (5, 6, 5, 6);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding (5, 6, 5, 6);
			this.tabPage2.Size = new System.Drawing.Size (940, 627);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Rijndael (暗号化所要時間)";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add (this.tableLayoutPanel6, 0, 6);
			this.tableLayoutPanel2.Controls.Add (this.etRijndaelEB, 0, 0);
			this.tableLayoutPanel2.Controls.Add (this.etRijndaelBC, 0, 2);
			this.tableLayoutPanel2.Controls.Add (this.etRijndaelOC, 0, 4);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point (5, 6);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 7;
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel2.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel2.Size = new System.Drawing.Size (930, 626);
			this.tableLayoutPanel2.TabIndex = 8;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel6.ColumnCount = 2;
			this.tableLayoutPanel6.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel6.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel6.Controls.Add (this.btnStartRijndaelEncryptionTime, 1, 0);
			this.tableLayoutPanel6.Controls.Add (this.nudRijndael, 0, 0);
			this.tableLayoutPanel6.Location = new System.Drawing.Point (749, 303);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 1;
			this.tableLayoutPanel6.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size (178, 49);
			this.tableLayoutPanel6.TabIndex = 7;
			// 
			// btnStartRijndaelEncryptionTime
			// 
			this.btnStartRijndaelEncryptionTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStartRijndaelEncryptionTime.Location = new System.Drawing.Point (70, 0);
			this.btnStartRijndaelEncryptionTime.Margin = new System.Windows.Forms.Padding (0);
			this.btnStartRijndaelEncryptionTime.Name = "btnStartRijndaelEncryptionTime";
			this.btnStartRijndaelEncryptionTime.Size = new System.Drawing.Size (108, 40);
			this.btnStartRijndaelEncryptionTime.TabIndex = 2;
			this.btnStartRijndaelEncryptionTime.Text = "Start";
			this.btnStartRijndaelEncryptionTime.UseVisualStyleBackColor = true;
			this.btnStartRijndaelEncryptionTime.Click += new System.EventHandler (this.btnStartRijndaelEncryptionTime_Click);
			// 
			// nudRijndael
			// 
			this.nudRijndael.Location = new System.Drawing.Point (3, 3);
			this.nudRijndael.Maximum = new decimal (new int[] {
            8,
            0,
            0,
            0});
			this.nudRijndael.Minimum = new decimal (new int[] {
            1,
            0,
            0,
            0});
			this.nudRijndael.Name = "nudRijndael";
			this.nudRijndael.Size = new System.Drawing.Size (38, 31);
			this.nudRijndael.TabIndex = 3;
			this.nudRijndael.Value = new decimal (new int[] {
            1,
            0,
            0,
            0});
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add (this.tableLayoutPanel3);
			this.tabPage3.Location = new System.Drawing.Point (4, 33);
			this.tabPage3.Margin = new System.Windows.Forms.Padding (5, 6, 5, 6);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding (5, 6, 5, 6);
			this.tabPage3.Size = new System.Drawing.Size (940, 627);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Camellia/Rijndael (暗号化速度)";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Controls.Add (this.tableLayoutPanel7, 0, 10);
			this.tableLayoutPanel3.Controls.Add (this.encryptionSpeed1, 0, 0);
			this.tableLayoutPanel3.Controls.Add (this.encryptionSpeed2, 0, 2);
			this.tableLayoutPanel3.Controls.Add (this.encryptionSpeed3, 0, 4);
			this.tableLayoutPanel3.Controls.Add (this.encryptionSpeed4, 0, 6);
			this.tableLayoutPanel3.Controls.Add (this.encryptionSpeed5, 0, 8);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point (5, 6);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 11;
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel3.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel3.Size = new System.Drawing.Size (930, 626);
			this.tableLayoutPanel3.TabIndex = 3;
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel7.ColumnCount = 2;
			this.tableLayoutPanel7.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel7.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel7.Controls.Add (this.btnStartEncryptionSpeed, 1, 0);
			this.tableLayoutPanel7.Controls.Add (this.nudEncryptionSpeed, 0, 0);
			this.tableLayoutPanel7.Location = new System.Drawing.Point (749, 503);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 1;
			this.tableLayoutPanel7.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.Size = new System.Drawing.Size (178, 49);
			this.tableLayoutPanel7.TabIndex = 8;
			// 
			// btnStartEncryptionSpeed
			// 
			this.btnStartEncryptionSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStartEncryptionSpeed.Location = new System.Drawing.Point (70, 0);
			this.btnStartEncryptionSpeed.Margin = new System.Windows.Forms.Padding (0);
			this.btnStartEncryptionSpeed.Name = "btnStartEncryptionSpeed";
			this.btnStartEncryptionSpeed.Size = new System.Drawing.Size (108, 40);
			this.btnStartEncryptionSpeed.TabIndex = 2;
			this.btnStartEncryptionSpeed.Text = "Start";
			this.btnStartEncryptionSpeed.UseVisualStyleBackColor = true;
			this.btnStartEncryptionSpeed.Click += new System.EventHandler (this.btnStartEncryptionSpeed_Click);
			// 
			// nudEncryptionSpeed
			// 
			this.nudEncryptionSpeed.Location = new System.Drawing.Point (3, 3);
			this.nudEncryptionSpeed.Maximum = new decimal (new int[] {
            8,
            0,
            0,
            0});
			this.nudEncryptionSpeed.Minimum = new decimal (new int[] {
            1,
            0,
            0,
            0});
			this.nudEncryptionSpeed.Name = "nudEncryptionSpeed";
			this.nudEncryptionSpeed.Size = new System.Drawing.Size (38, 31);
			this.nudEncryptionSpeed.TabIndex = 3;
			this.nudEncryptionSpeed.Value = new decimal (new int[] {
            1,
            0,
            0,
            0});
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add (this.tableLayoutPanel4);
			this.tabPage4.Location = new System.Drawing.Point (4, 33);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size (940, 627);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "ECDSA";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Absolute, 180F));
			this.tableLayoutPanel4.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel4.Controls.Add (this.label1, 0, 1);
			this.tableLayoutPanel4.Controls.Add (this.lblBCSign, 0, 2);
			this.tableLayoutPanel4.Controls.Add (this.lblBCVerify, 0, 3);
			this.tableLayoutPanel4.Controls.Add (this.label5, 0, 5);
			this.tableLayoutPanel4.Controls.Add (this.lblOCSign, 0, 6);
			this.tableLayoutPanel4.Controls.Add (this.lblOCVerify, 0, 7);
			this.tableLayoutPanel4.Controls.Add (this.pbEcdsaSignBC, 1, 2);
			this.tableLayoutPanel4.Controls.Add (this.btnStartECDSA, 1, 8);
			this.tableLayoutPanel4.Controls.Add (this.pbEcdsaVerifyBC, 1, 3);
			this.tableLayoutPanel4.Controls.Add (this.pbEcdsaSignOC, 1, 6);
			this.tableLayoutPanel4.Controls.Add (this.pbEcdsaVerifyOC, 1, 7);
			this.tableLayoutPanel4.Controls.Add (this.label2, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point (0, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 9;
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel4.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size (940, 638);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point (3, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (120, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Bouncy Castle";
			// 
			// lblBCSign
			// 
			this.lblBCSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblBCSign.AutoSize = true;
			this.lblBCSign.Location = new System.Drawing.Point (133, 80);
			this.lblBCSign.Name = "lblBCSign";
			this.lblBCSign.Padding = new System.Windows.Forms.Padding (0, 10, 0, 0);
			this.lblBCSign.Size = new System.Drawing.Size (44, 34);
			this.lblBCSign.TabIndex = 0;
			this.lblBCSign.Text = "Sign";
			// 
			// lblBCVerify
			// 
			this.lblBCVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblBCVerify.AutoSize = true;
			this.lblBCVerify.Location = new System.Drawing.Point (123, 120);
			this.lblBCVerify.Name = "lblBCVerify";
			this.lblBCVerify.Padding = new System.Windows.Forms.Padding (0, 10, 0, 0);
			this.lblBCVerify.Size = new System.Drawing.Size (54, 34);
			this.lblBCVerify.TabIndex = 0;
			this.lblBCVerify.Text = "Verify";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point (3, 200);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size (102, 24);
			this.label5.TabIndex = 0;
			this.label5.Text = "openCrypto";
			// 
			// lblOCSign
			// 
			this.lblOCSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblOCSign.AutoSize = true;
			this.lblOCSign.Location = new System.Drawing.Point (133, 240);
			this.lblOCSign.Name = "lblOCSign";
			this.lblOCSign.Padding = new System.Windows.Forms.Padding (0, 10, 0, 0);
			this.lblOCSign.Size = new System.Drawing.Size (44, 34);
			this.lblOCSign.TabIndex = 0;
			this.lblOCSign.Text = "Sign";
			// 
			// lblOCVerify
			// 
			this.lblOCVerify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblOCVerify.AutoSize = true;
			this.lblOCVerify.Location = new System.Drawing.Point (123, 280);
			this.lblOCVerify.Name = "lblOCVerify";
			this.lblOCVerify.Padding = new System.Windows.Forms.Padding (0, 10, 0, 0);
			this.lblOCVerify.Size = new System.Drawing.Size (54, 34);
			this.lblOCVerify.TabIndex = 0;
			this.lblOCVerify.Text = "Verify";
			// 
			// pbEcdsaSignBC
			// 
			this.pbEcdsaSignBC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbEcdsaSignBC.Location = new System.Drawing.Point (183, 83);
			this.pbEcdsaSignBC.Name = "pbEcdsaSignBC";
			this.pbEcdsaSignBC.Size = new System.Drawing.Size (754, 34);
			this.pbEcdsaSignBC.TabIndex = 2;
			// 
			// btnStartECDSA
			// 
			this.btnStartECDSA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStartECDSA.Location = new System.Drawing.Point (846, 323);
			this.btnStartECDSA.Name = "btnStartECDSA";
			this.btnStartECDSA.Size = new System.Drawing.Size (91, 38);
			this.btnStartECDSA.TabIndex = 1;
			this.btnStartECDSA.Text = "Start";
			this.btnStartECDSA.UseVisualStyleBackColor = true;
			this.btnStartECDSA.Click += new System.EventHandler (this.btnStartECDSA_Click);
			// 
			// pbEcdsaVerifyBC
			// 
			this.pbEcdsaVerifyBC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbEcdsaVerifyBC.Location = new System.Drawing.Point (183, 123);
			this.pbEcdsaVerifyBC.Name = "pbEcdsaVerifyBC";
			this.pbEcdsaVerifyBC.Size = new System.Drawing.Size (754, 34);
			this.pbEcdsaVerifyBC.TabIndex = 3;
			// 
			// pbEcdsaSignOC
			// 
			this.pbEcdsaSignOC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbEcdsaSignOC.Location = new System.Drawing.Point (183, 243);
			this.pbEcdsaSignOC.Name = "pbEcdsaSignOC";
			this.pbEcdsaSignOC.Size = new System.Drawing.Size (754, 34);
			this.pbEcdsaSignOC.TabIndex = 4;
			// 
			// pbEcdsaVerifyOC
			// 
			this.pbEcdsaVerifyOC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbEcdsaVerifyOC.Location = new System.Drawing.Point (183, 283);
			this.pbEcdsaVerifyOC.Name = "pbEcdsaVerifyOC";
			this.pbEcdsaVerifyOC.Size = new System.Drawing.Size (754, 34);
			this.pbEcdsaVerifyOC.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.tableLayoutPanel4.SetColumnSpan (this.label2, 2);
			this.label2.Location = new System.Drawing.Point (3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size (934, 40);
			this.label2.TabIndex = 6;
			this.label2.Text = "署名の作成／検証にかかる時間 (バーが短いものほど高速)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// etCamelliaBC
			// 
			this.etCamelliaBC.DataSize = 8388608;
			this.etCamelliaBC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.etCamelliaBC.ImplementationType = Demo.ImplementationType.CamelliaBouncyCastle;
			this.etCamelliaBC.Location = new System.Drawing.Point (0, 0);
			this.etCamelliaBC.Margin = new System.Windows.Forms.Padding (0);
			this.etCamelliaBC.Name = "etCamelliaBC";
			this.etCamelliaBC.NumberOfThreads = 1;
			this.etCamelliaBC.Size = new System.Drawing.Size (930, 90);
			this.etCamelliaBC.TabIndex = 3;
			// 
			// etCamelliaOC
			// 
			this.etCamelliaOC.DataSize = 8388608;
			this.etCamelliaOC.Dock = System.Windows.Forms.DockStyle.Fill;
			this.etCamelliaOC.ImplementationType = Demo.ImplementationType.CamelliaOpenCrypto;
			this.etCamelliaOC.Location = new System.Drawing.Point (0, 100);
			this.etCamelliaOC.Margin = new System.Windows.Forms.Padding (0);
			this.etCamelliaOC.Name = "etCamelliaOC";
			this.etCamelliaOC.NumberOfThreads = 1;
			this.etCamelliaOC.Size = new System.Drawing.Size (930, 90);
			this.etCamelliaOC.TabIndex = 3;
			// 
			// etRijndaelEB
			// 
			this.etRijndaelEB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.etRijndaelEB.DataSize = 8388608;
			this.etRijndaelEB.ImplementationType = Demo.ImplementationType.RijndaelRuntime;
			this.etRijndaelEB.Location = new System.Drawing.Point (0, 0);
			this.etRijndaelEB.Margin = new System.Windows.Forms.Padding (0);
			this.etRijndaelEB.Name = "etRijndaelEB";
			this.etRijndaelEB.NumberOfThreads = 1;
			this.etRijndaelEB.Size = new System.Drawing.Size (930, 90);
			this.etRijndaelEB.TabIndex = 4;
			// 
			// etRijndaelBC
			// 
			this.etRijndaelBC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.etRijndaelBC.DataSize = 8388608;
			this.etRijndaelBC.ImplementationType = Demo.ImplementationType.RijndaelBouncyCastle;
			this.etRijndaelBC.Location = new System.Drawing.Point (0, 100);
			this.etRijndaelBC.Margin = new System.Windows.Forms.Padding (0);
			this.etRijndaelBC.Name = "etRijndaelBC";
			this.etRijndaelBC.NumberOfThreads = 1;
			this.etRijndaelBC.Size = new System.Drawing.Size (930, 90);
			this.etRijndaelBC.TabIndex = 5;
			// 
			// etRijndaelOC
			// 
			this.etRijndaelOC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.etRijndaelOC.DataSize = 8388608;
			this.etRijndaelOC.ImplementationType = Demo.ImplementationType.RijndaelOpenCrypto;
			this.etRijndaelOC.Location = new System.Drawing.Point (0, 200);
			this.etRijndaelOC.Margin = new System.Windows.Forms.Padding (0);
			this.etRijndaelOC.Name = "etRijndaelOC";
			this.etRijndaelOC.NumberOfThreads = 1;
			this.etRijndaelOC.Size = new System.Drawing.Size (930, 90);
			this.etRijndaelOC.TabIndex = 6;
			// 
			// encryptionSpeed1
			// 
			this.encryptionSpeed1.DataSize = 33554432;
			this.encryptionSpeed1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.encryptionSpeed1.ImplementationType = Demo.ImplementationType.RijndaelRuntime;
			this.encryptionSpeed1.Location = new System.Drawing.Point (0, 0);
			this.encryptionSpeed1.Margin = new System.Windows.Forms.Padding (0);
			this.encryptionSpeed1.Name = "encryptionSpeed1";
			this.encryptionSpeed1.NumberOfThreads = 1;
			this.encryptionSpeed1.Size = new System.Drawing.Size (930, 90);
			this.encryptionSpeed1.TabIndex = 0;
			// 
			// encryptionSpeed2
			// 
			this.encryptionSpeed2.DataSize = 33554432;
			this.encryptionSpeed2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.encryptionSpeed2.ImplementationType = Demo.ImplementationType.RijndaelBouncyCastle;
			this.encryptionSpeed2.Location = new System.Drawing.Point (0, 100);
			this.encryptionSpeed2.Margin = new System.Windows.Forms.Padding (0);
			this.encryptionSpeed2.Name = "encryptionSpeed2";
			this.encryptionSpeed2.NumberOfThreads = 1;
			this.encryptionSpeed2.Size = new System.Drawing.Size (930, 90);
			this.encryptionSpeed2.TabIndex = 3;
			// 
			// encryptionSpeed3
			// 
			this.encryptionSpeed3.DataSize = 33554432;
			this.encryptionSpeed3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.encryptionSpeed3.ImplementationType = Demo.ImplementationType.RijndaelOpenCrypto;
			this.encryptionSpeed3.Location = new System.Drawing.Point (0, 200);
			this.encryptionSpeed3.Margin = new System.Windows.Forms.Padding (0);
			this.encryptionSpeed3.Name = "encryptionSpeed3";
			this.encryptionSpeed3.NumberOfThreads = 1;
			this.encryptionSpeed3.Size = new System.Drawing.Size (930, 90);
			this.encryptionSpeed3.TabIndex = 4;
			// 
			// encryptionSpeed4
			// 
			this.encryptionSpeed4.DataSize = 33554432;
			this.encryptionSpeed4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.encryptionSpeed4.ImplementationType = Demo.ImplementationType.CamelliaBouncyCastle;
			this.encryptionSpeed4.Location = new System.Drawing.Point (0, 300);
			this.encryptionSpeed4.Margin = new System.Windows.Forms.Padding (0);
			this.encryptionSpeed4.Name = "encryptionSpeed4";
			this.encryptionSpeed4.NumberOfThreads = 1;
			this.encryptionSpeed4.Size = new System.Drawing.Size (930, 90);
			this.encryptionSpeed4.TabIndex = 5;
			// 
			// encryptionSpeed5
			// 
			this.encryptionSpeed5.DataSize = 33554432;
			this.encryptionSpeed5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.encryptionSpeed5.ImplementationType = Demo.ImplementationType.CamelliaOpenCrypto;
			this.encryptionSpeed5.Location = new System.Drawing.Point (0, 400);
			this.encryptionSpeed5.Margin = new System.Windows.Forms.Padding (0);
			this.encryptionSpeed5.Name = "encryptionSpeed5";
			this.encryptionSpeed5.NumberOfThreads = 1;
			this.encryptionSpeed5.Size = new System.Drawing.Size (930, 90);
			this.encryptionSpeed5.TabIndex = 6;
			// 
			// Demo2008
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (10F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (948, 664);
			this.Controls.Add (this.tabControl1);
			this.Font = new System.Drawing.Font ("メイリオ", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding (5, 6, 5, 6);
			this.Name = "Demo2008";
			this.Text = "Demo";
			this.tabControl1.ResumeLayout (false);
			this.tabPage1.ResumeLayout (false);
			this.tableLayoutPanel1.ResumeLayout (false);
			this.tableLayoutPanel5.ResumeLayout (false);
			((System.ComponentModel.ISupportInitialize)(this.nudCamellia)).EndInit ();
			this.tabPage2.ResumeLayout (false);
			this.tableLayoutPanel2.ResumeLayout (false);
			this.tableLayoutPanel6.ResumeLayout (false);
			((System.ComponentModel.ISupportInitialize)(this.nudRijndael)).EndInit ();
			this.tabPage3.ResumeLayout (false);
			this.tableLayoutPanel3.ResumeLayout (false);
			this.tableLayoutPanel7.ResumeLayout (false);
			((System.ComponentModel.ISupportInitialize)(this.nudEncryptionSpeed)).EndInit ();
			this.tabPage4.ResumeLayout (false);
			this.tableLayoutPanel4.ResumeLayout (false);
			this.tableLayoutPanel4.PerformLayout ();
			this.ResumeLayout (false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private EncryptionTime etCamelliaOC;
		private EncryptionTime etCamelliaBC;
		private EncryptionTime etRijndaelOC;
		private EncryptionTime etRijndaelBC;
		private EncryptionTime etRijndaelEB;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private EncryptionSpeed encryptionSpeed1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private EncryptionSpeed encryptionSpeed2;
		private EncryptionSpeed encryptionSpeed3;
		private EncryptionSpeed encryptionSpeed4;
		private EncryptionSpeed encryptionSpeed5;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblBCSign;
		private System.Windows.Forms.Label lblBCVerify;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblOCSign;
		private System.Windows.Forms.Label lblOCVerify;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Button btnStartRijndaelEncryptionTime;
		private System.Windows.Forms.NumericUpDown nudRijndael;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.Button btnStartEncryptionSpeed;
		private System.Windows.Forms.NumericUpDown nudEncryptionSpeed;
		private System.Windows.Forms.ProgressBar pbEcdsaSignBC;
		private System.Windows.Forms.ProgressBar pbEcdsaVerifyBC;
		private System.Windows.Forms.ProgressBar pbEcdsaSignOC;
		private System.Windows.Forms.ProgressBar pbEcdsaVerifyOC;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.NumericUpDown nudCamellia;
		private System.Windows.Forms.Button btnStartCamelliaEncryptionTime;
		private System.Windows.Forms.Button btnStartECDSA;
		private System.Windows.Forms.Label label2;
	}
}