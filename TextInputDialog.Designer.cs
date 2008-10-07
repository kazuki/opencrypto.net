namespace CryptoFrontend
{
	partial class TextInputDialog
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel ();
			this.label1 = new System.Windows.Forms.Label ();
			this.textBox1 = new System.Windows.Forms.TextBox ();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel ();
			this.btnOK = new System.Windows.Forms.Button ();
			this.btnCancel = new System.Windows.Forms.Button ();
			this.tableLayoutPanel1.SuspendLayout ();
			this.flowLayoutPanel1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle ());
			this.tableLayoutPanel1.Controls.Add (this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add (this.textBox1, 0, 1);
			this.tableLayoutPanel1.Controls.Add (this.flowLayoutPanel1, 0, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point (12, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle ());
			this.tableLayoutPanel1.Size = new System.Drawing.Size (456, 104);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point (3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size (31, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "hoge";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point (3, 28);
			this.textBox1.Margin = new System.Windows.Forms.Padding (3, 15, 3, 3);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size (450, 20);
			this.textBox1.TabIndex = 1;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add (this.btnCancel);
			this.flowLayoutPanel1.Controls.Add (this.btnOK);
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point (3, 66);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding (3, 15, 3, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size (450, 35);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// btnOK
			// 
			this.btnOK.AutoSize = true;
			this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnOK.Location = new System.Drawing.Point (320, 3);
			this.btnOK.Name = "btnOK";
			this.btnOK.Padding = new System.Windows.Forms.Padding (3);
			this.btnOK.Size = new System.Drawing.Size (38, 29);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "&OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler (this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.AutoSize = true;
			this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point (364, 3);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Padding = new System.Windows.Forms.Padding (3);
			this.btnCancel.Size = new System.Drawing.Size (83, 29);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "キャンセル(&C)";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler (this.btnCancel_Click);
			// 
			// TextInputDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size (476, 124);
			this.Controls.Add (this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TextInputDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.tableLayoutPanel1.ResumeLayout (false);
			this.tableLayoutPanel1.PerformLayout ();
			this.flowLayoutPanel1.ResumeLayout (false);
			this.flowLayoutPanel1.PerformLayout ();
			this.ResumeLayout (false);
			this.PerformLayout ();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
	}
}