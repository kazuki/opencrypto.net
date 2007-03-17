namespace Demo
{
	partial class SpeedCheckForm
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label3;
			System.Windows.Forms.Label label4;
			System.Windows.Forms.Label label5;
			System.Windows.Forms.Label label6;
			System.Windows.Forms.ColumnHeader columnHeader1;
			System.Windows.Forms.ColumnHeader columnHeader2;
			System.Windows.Forms.ColumnHeader columnHeader3;
			System.Windows.Forms.ColumnHeader columnHeader4;
			System.Windows.Forms.ColumnHeader columnHeader5;
			System.Windows.Forms.ColumnHeader columnHeader6;
			this.cbImpl = new System.Windows.Forms.ComboBox ();
			this.cbBlockMode = new System.Windows.Forms.ComboBox ();
			this.cbKeySize = new System.Windows.Forms.ComboBox ();
			this.cbBlockSize = new System.Windows.Forms.ComboBox ();
			this.numThreads = new System.Windows.Forms.NumericUpDown ();
			this.cbDataSize = new System.Windows.Forms.ComboBox ();
			this.btnRunAll1 = new System.Windows.Forms.Button ();
			this.btnRunSpeedTest = new System.Windows.Forms.Button ();
			this.btnKeySchedulingSpeed = new System.Windows.Forms.Button ();
			this.btnRunAllImpl1 = new System.Windows.Forms.Button ();
			this.btnRunAllImpl2 = new System.Windows.Forms.Button ();
			this.btnRunAll2 = new System.Windows.Forms.Button ();
			this.btnClear = new System.Windows.Forms.Button ();
			this.listResult = new System.Windows.Forms.ListView ();
			this.checkBox1 = new System.Windows.Forms.CheckBox ();
			label1 = new System.Windows.Forms.Label ();
			label2 = new System.Windows.Forms.Label ();
			label3 = new System.Windows.Forms.Label ();
			label4 = new System.Windows.Forms.Label ();
			label5 = new System.Windows.Forms.Label ();
			label6 = new System.Windows.Forms.Label ();
			columnHeader1 = new System.Windows.Forms.ColumnHeader ();
			columnHeader2 = new System.Windows.Forms.ColumnHeader ();
			columnHeader3 = new System.Windows.Forms.ColumnHeader ();
			columnHeader4 = new System.Windows.Forms.ColumnHeader ();
			columnHeader5 = new System.Windows.Forms.ColumnHeader ();
			columnHeader6 = new System.Windows.Forms.ColumnHeader ();
			((System.ComponentModel.ISupportInitialize)(this.numThreads)).BeginInit ();
			this.SuspendLayout ();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point (13, 17);
			label1.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size (187, 27);
			label1.TabIndex = 0;
			label1.Text = "&Implementation:";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point (13, 60);
			label2.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size (219, 27);
			label2.TabIndex = 2;
			label2.Text = "Block Cipher &Mode:";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point (13, 103);
			label3.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size (107, 27);
			label3.TabIndex = 4;
			label3.Text = "&Key Size:";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point (13, 146);
			label4.Margin = new System.Windows.Forms.Padding (4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size (126, 27);
			label4.TabIndex = 6;
			label4.Text = "&Block Size:";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point (13, 229);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size (105, 27);
			label5.TabIndex = 10;
			label5.Text = "&Threads:";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point (13, 189);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size (117, 27);
			label6.TabIndex = 8;
			label6.Text = "&Data Size:";
			// 
			// columnHeader1
			// 
			columnHeader1.Text = "Implementation";
			columnHeader1.Width = 240;
			// 
			// columnHeader2
			// 
			columnHeader2.Text = "Mode";
			columnHeader2.Width = 55;
			// 
			// columnHeader3
			// 
			columnHeader3.Text = "Key";
			columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			columnHeader3.Width = 90;
			// 
			// columnHeader4
			// 
			columnHeader4.Text = "Block";
			columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			columnHeader4.Width = 90;
			// 
			// columnHeader5
			// 
			columnHeader5.Text = "Encryption";
			columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			columnHeader5.Width = 140;
			// 
			// columnHeader6
			// 
			columnHeader6.Text = "Decryption";
			columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			columnHeader6.Width = 140;
			// 
			// cbImpl
			// 
			this.cbImpl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbImpl.Location = new System.Drawing.Point (240, 14);
			this.cbImpl.Margin = new System.Windows.Forms.Padding (4, 5, 4, 5);
			this.cbImpl.Name = "cbImpl";
			this.cbImpl.Size = new System.Drawing.Size (260, 33);
			this.cbImpl.TabIndex = 1;
			this.cbImpl.SelectedIndexChanged += new System.EventHandler (this.cbImpl_SelectedIndexChanged);
			// 
			// cbBlockMode
			// 
			this.cbBlockMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBlockMode.FormattingEnabled = true;
			this.cbBlockMode.Location = new System.Drawing.Point (240, 57);
			this.cbBlockMode.Margin = new System.Windows.Forms.Padding (4, 5, 4, 5);
			this.cbBlockMode.Name = "cbBlockMode";
			this.cbBlockMode.Size = new System.Drawing.Size (160, 33);
			this.cbBlockMode.TabIndex = 3;
			// 
			// cbKeySize
			// 
			this.cbKeySize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbKeySize.FormattingEnabled = true;
			this.cbKeySize.Location = new System.Drawing.Point (240, 100);
			this.cbKeySize.Margin = new System.Windows.Forms.Padding (4, 5, 4, 5);
			this.cbKeySize.Name = "cbKeySize";
			this.cbKeySize.Size = new System.Drawing.Size (160, 33);
			this.cbKeySize.TabIndex = 5;
			// 
			// cbBlockSize
			// 
			this.cbBlockSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBlockSize.FormattingEnabled = true;
			this.cbBlockSize.Location = new System.Drawing.Point (240, 143);
			this.cbBlockSize.Margin = new System.Windows.Forms.Padding (4, 5, 4, 5);
			this.cbBlockSize.Name = "cbBlockSize";
			this.cbBlockSize.Size = new System.Drawing.Size (160, 33);
			this.cbBlockSize.TabIndex = 7;
			// 
			// numThreads
			// 
			this.numThreads.Location = new System.Drawing.Point (240, 227);
			this.numThreads.Minimum = new decimal (new int[] {
            1,
            0,
            0,
            0});
			this.numThreads.Name = "numThreads";
			this.numThreads.Size = new System.Drawing.Size (160, 40);
			this.numThreads.TabIndex = 11;
			this.numThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.numThreads.Value = new decimal (new int[] {
            1,
            0,
            0,
            0});
			// 
			// cbDataSize
			// 
			this.cbDataSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataSize.FormattingEnabled = true;
			this.cbDataSize.Items.AddRange (new object[] {
            "1 KB",
            "8 KB",
            "64 KB",
            "512 KB",
            "1 MB",
            "4 MB",
            "8 MB",
            "16 MB",
            "32 MB",
            "64 MB",
            "128 MB",
            "256 MB"});
			this.cbDataSize.Location = new System.Drawing.Point (240, 186);
			this.cbDataSize.Margin = new System.Windows.Forms.Padding (4, 5, 4, 5);
			this.cbDataSize.Name = "cbDataSize";
			this.cbDataSize.Size = new System.Drawing.Size (160, 33);
			this.cbDataSize.TabIndex = 9;
			// 
			// btnRunAll1
			// 
			this.btnRunAll1.Location = new System.Drawing.Point (12, 404);
			this.btnRunAll1.Name = "btnRunAll1";
			this.btnRunAll1.Size = new System.Drawing.Size (188, 40);
			this.btnRunAll1.TabIndex = 14;
			this.btnRunAll1.Text = "All Patterns";
			this.btnRunAll1.UseVisualStyleBackColor = true;
			this.btnRunAll1.Click += new System.EventHandler (this.btnRunAll_Click);
			// 
			// btnRunSpeedTest
			// 
			this.btnRunSpeedTest.Location = new System.Drawing.Point (12, 276);
			this.btnRunSpeedTest.Name = "btnRunSpeedTest";
			this.btnRunSpeedTest.Size = new System.Drawing.Size (188, 76);
			this.btnRunSpeedTest.TabIndex = 12;
			this.btnRunSpeedTest.Text = "En/Decryption Speed";
			this.btnRunSpeedTest.UseVisualStyleBackColor = true;
			this.btnRunSpeedTest.Click += new System.EventHandler (this.btnRunSpeedTest_Click);
			// 
			// btnKeySchedulingSpeed
			// 
			this.btnKeySchedulingSpeed.Location = new System.Drawing.Point (212, 276);
			this.btnKeySchedulingSpeed.Name = "btnKeySchedulingSpeed";
			this.btnKeySchedulingSpeed.Size = new System.Drawing.Size (188, 75);
			this.btnKeySchedulingSpeed.TabIndex = 15;
			this.btnKeySchedulingSpeed.Text = "Key-Scheduling Speed";
			this.btnKeySchedulingSpeed.UseVisualStyleBackColor = true;
			this.btnKeySchedulingSpeed.Click += new System.EventHandler (this.btnKeySchedulingSpeed_Click);
			// 
			// btnRunAllImpl1
			// 
			this.btnRunAllImpl1.Location = new System.Drawing.Point (12, 358);
			this.btnRunAllImpl1.Name = "btnRunAllImpl1";
			this.btnRunAllImpl1.Size = new System.Drawing.Size (188, 40);
			this.btnRunAllImpl1.TabIndex = 13;
			this.btnRunAllImpl1.Text = "All Impl";
			this.btnRunAllImpl1.UseVisualStyleBackColor = true;
			this.btnRunAllImpl1.Click += new System.EventHandler (this.btnRunAllImpl_Click);
			// 
			// btnRunAllImpl2
			// 
			this.btnRunAllImpl2.Location = new System.Drawing.Point (212, 357);
			this.btnRunAllImpl2.Name = "btnRunAllImpl2";
			this.btnRunAllImpl2.Size = new System.Drawing.Size (188, 40);
			this.btnRunAllImpl2.TabIndex = 16;
			this.btnRunAllImpl2.Text = "All Impl";
			this.btnRunAllImpl2.UseVisualStyleBackColor = true;
			this.btnRunAllImpl2.Click += new System.EventHandler (this.btnRunAllImpl2_Click);
			// 
			// btnRunAll2
			// 
			this.btnRunAll2.Location = new System.Drawing.Point (212, 403);
			this.btnRunAll2.Name = "btnRunAll2";
			this.btnRunAll2.Size = new System.Drawing.Size (188, 40);
			this.btnRunAll2.TabIndex = 17;
			this.btnRunAll2.Text = "All Patterns";
			this.btnRunAll2.UseVisualStyleBackColor = true;
			this.btnRunAll2.Click += new System.EventHandler (this.btnRunAll2_Click);
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClear.Location = new System.Drawing.Point (749, 12);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size (100, 35);
			this.btnClear.TabIndex = 19;
			this.btnClear.Text = "&Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler (this.btnClear_Click);
			// 
			// listResult
			// 
			this.listResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.listResult.Columns.AddRange (new System.Windows.Forms.ColumnHeader[] {
            columnHeader1,
            columnHeader2,
            columnHeader3,
            columnHeader4,
            columnHeader5,
            columnHeader6});
			this.listResult.FullRowSelect = true;
			this.listResult.GridLines = true;
			this.listResult.Location = new System.Drawing.Point (407, 55);
			this.listResult.MultiSelect = false;
			this.listResult.Name = "listResult";
			this.listResult.ShowGroups = false;
			this.listResult.Size = new System.Drawing.Size (442, 389);
			this.listResult.TabIndex = 20;
			this.listResult.UseCompatibleStateImageBehavior = false;
			this.listResult.View = System.Windows.Forms.View.Details;
			this.listResult.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler (this.listResult_ColumnClick);
			// 
			// checkBox1
			// 
			this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point (669, 15);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size (74, 31);
			this.checkBox1.TabIndex = 21;
			this.checkBox1.Text = "Sort";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler (this.checkBox1_CheckedChanged);
			// 
			// SpeedCheckForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (861, 456);
			this.Controls.Add (this.checkBox1);
			this.Controls.Add (this.listResult);
			this.Controls.Add (this.btnClear);
			this.Controls.Add (this.btnRunAllImpl2);
			this.Controls.Add (this.btnRunAll2);
			this.Controls.Add (this.btnRunAllImpl1);
			this.Controls.Add (this.btnKeySchedulingSpeed);
			this.Controls.Add (this.btnRunSpeedTest);
			this.Controls.Add (this.btnRunAll1);
			this.Controls.Add (this.cbDataSize);
			this.Controls.Add (label6);
			this.Controls.Add (this.numThreads);
			this.Controls.Add (label5);
			this.Controls.Add (this.cbBlockSize);
			this.Controls.Add (label4);
			this.Controls.Add (this.cbKeySize);
			this.Controls.Add (label3);
			this.Controls.Add (this.cbBlockMode);
			this.Controls.Add (label2);
			this.Controls.Add (this.cbImpl);
			this.Controls.Add (label1);
			this.Font = new System.Drawing.Font ("Lucida Sans Unicode", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding (14, 17, 14, 17);
			this.Name = "SpeedCheckForm";
			this.Text = "Performance Demo Program";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			((System.ComponentModel.ISupportInitialize)(this.numThreads)).EndInit ();
			this.ResumeLayout (false);
			this.PerformLayout ();

		}
		private System.Windows.Forms.ComboBox cbImpl;
		private System.Windows.Forms.ComboBox cbBlockMode;
		private System.Windows.Forms.ComboBox cbKeySize;
		private System.Windows.Forms.ComboBox cbBlockSize;
		private System.Windows.Forms.NumericUpDown numThreads;
		private System.Windows.Forms.ComboBox cbDataSize;
		private System.Windows.Forms.Button btnRunAll1;
		private System.Windows.Forms.Button btnRunSpeedTest;
		private System.Windows.Forms.Button btnKeySchedulingSpeed;
		private System.Windows.Forms.Button btnRunAllImpl1;
		private System.Windows.Forms.Button btnRunAllImpl2;
		private System.Windows.Forms.Button btnRunAll2;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.ListView listResult;
		private System.Windows.Forms.CheckBox checkBox1;
	}
}