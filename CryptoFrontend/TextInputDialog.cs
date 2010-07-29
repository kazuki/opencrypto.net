using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CryptoFrontend
{
	public partial class TextInputDialog : Form
	{
		string _inputtext = "";

		public TextInputDialog ()
		{
			InitializeComponent ();
		}

		public TextInputDialog (string title, string msg) : this ()
		{
			this.Text = title;
			label1.Text = msg;
		}

		public TextInputDialog (string title, string msg, int lines)
			: this (title, msg)
		{
			if (lines > 1) {
				textBox1.Multiline = true;
				textBox1.ScrollBars = ScrollBars.Vertical;
				textBox1.Height = textBox1.Font.Height * (lines + 1);
			}
		}

		private void btnOK_Click (object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			_inputtext = textBox1.Text;
			this.Close ();
		}

		private void btnCancel_Click (object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close ();
		}

		public void SetDefaultText (string text, bool selected)
		{
			textBox1.Text = text;
			if (selected)
				textBox1.SelectAll ();
		}

		public string InputText {
			get { return _inputtext; }
		}
	}
}
