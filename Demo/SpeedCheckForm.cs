using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Demo
{
	public partial class SpeedCheckForm : Form
	{
		ListViewColumnSorter _sorter = new ListViewColumnSorter ();

		object _state1, _state2, _state3, _state4;

		public SpeedCheckForm()
		{
			InitializeComponent();

			Array impls = Enum.GetValues (typeof (ImplementationType));
			foreach (ImplementationType impl in impls)
				cbImpl.Items.Add (Helper.ToName(impl));
			cbImpl.SelectedIndex = 0;
			cbDataSize.SelectedIndex = 10;
			listResult.ListViewItemSorter = _sorter;
			listResult.Sorting = SortOrder.None;

			numThreads.Maximum = Environment.ProcessorCount * 4;
		}

		private void cbImpl_SelectedIndexChanged (object sender, EventArgs e)
		{
			cbBlockMode.Items.Clear ();
			cbBlockSize.Items.Clear ();
			cbKeySize.Items.Clear ();

			object algo = Helper.CreateInstance (Helper.ToImplementationType ((string)cbImpl.SelectedItem));
			Type modeType = Helper.IsSymmetricAlgorithmPlus (algo)
			                ? typeof (openCrypto.CipherModePlus) : typeof (CipherMode);
			bool quickHack = (algo as RijndaelManaged != null ? true : false);
			string[] modes = Enum.GetNames (modeType);
			for (int i = 0; i < modes.Length; i ++) {
				if (!"CTS".Equals (modes [i]) && !(quickHack && "OFB".Equals (modes[i])))
					cbBlockMode.Items.Add (modes [i]);
			}
			cbBlockMode.SelectedIndex = 0;

			KeySizes[] keySizes = Helper.GetKeySizes (algo);
			foreach (KeySizes ks in keySizes) {
				for (int i = ks.MinSize; i <= ks.MaxSize; i += ks.SkipSize) {
					cbKeySize.Items.Add (i);
					if (ks.SkipSize == 0) break;
				}
			}
			KeySizes[] blockSizes = Helper.GetBlockSizes (algo);
			foreach (KeySizes bs in blockSizes) {
				for (int i = bs.MinSize; i <= bs.MaxSize; i += bs.SkipSize) {
					cbBlockSize.Items.Add (i);
					if (bs.SkipSize == 0) break;
				}
			}
			cbKeySize.SelectedIndex = 0;
			cbBlockSize.SelectedIndex = 0;
		}


		private void btnRunAllImpl_Click (object sender, EventArgs e)
		{
			RunAllImplementation (btnRunSpeedTest_Click, true, true, true);
		}

		private void btnRunAll_Click (object sender, EventArgs e)
		{
			RunAllPattern (btnRunSpeedTest_Click, true, true, true);
		}

		private void btnRunAllImpl2_Click (object sender, EventArgs e)
		{
			RunAllImplementation (btnKeySchedulingSpeed_Click, false, true, true);
		}

		private void btnRunAll2_Click (object sender, EventArgs e)
		{
			RunAllPattern (btnKeySchedulingSpeed_Click, false, true, true);
		}

		void BackupState ()
		{
			_state1 = cbImpl.SelectedItem;
			_state2 = cbBlockMode.SelectedItem;
			_state3 = cbKeySize.SelectedItem;
			_state4 = cbBlockSize.SelectedItem;
		}

		void RestoreState ()
		{
			cbImpl.SelectedItem = _state1;
			cbBlockMode.SelectedItem = _state2;
			cbKeySize.SelectedItem = _state3;
			cbBlockSize.SelectedItem = _state4;
		}

		void RunAllImplementation (EventHandler handler, bool checkMode, bool checkKeySize, bool checkBlockSize)
		{
			object mode = cbBlockMode.SelectedItem;
			object keySize = cbKeySize.SelectedItem;
			object blockSize = cbBlockSize.SelectedItem;
			BackupState ();
			for (int i1 = 0; i1 < cbImpl.Items.Count; i1++) {
				cbImpl.SelectedIndex = i1;
				if (checkMode && !cbBlockMode.Items.Contains (mode)) continue;
				if (checkKeySize && !cbKeySize.Items.Contains (keySize)) continue;
				if (checkBlockSize && !cbBlockSize.Items.Contains (blockSize)) continue;
				if (checkMode) cbBlockMode.SelectedItem = mode;
				if (checkKeySize) cbKeySize.SelectedItem = keySize;
				if (checkBlockSize) cbBlockSize.SelectedItem = blockSize;
				handler (null, EventArgs.Empty);
			}
			RestoreState ();
		}

		void RunAllPattern (EventHandler handler, bool checkMode, bool checkKeySize, bool checkBlockSize)
		{
			BackupState ();
			for (int i1 = 0; i1 < cbImpl.Items.Count; i1++) {
				cbImpl.SelectedIndex = i1;
				for (int i2 = 0; i2 < cbBlockMode.Items.Count; i2++) {
					cbBlockMode.SelectedIndex = i2;
					for (int i3 = 0; i3 < cbKeySize.Items.Count; i3++) {
						cbKeySize.SelectedIndex = i3;
						for (int i4 = 0; i4 < cbBlockSize.Items.Count; i4++) {
							cbBlockSize.SelectedIndex = i4;
							handler (null, EventArgs.Empty);
							if (!checkBlockSize) break;
						}
						if (!checkKeySize) break;
					}
					if (!checkMode) break;
				}
			}
			RestoreState ();
		}

		private void btnKeySchedulingSpeed_Click (object sender, EventArgs e)
		{
			ImplementationType type = Helper.ToImplementationType ((string)cbImpl.SelectedItem);
			int keySize = (int)cbKeySize.SelectedItem;
			int blockSize = (int)cbBlockSize.SelectedItem;
			string[] items = new string[6];
			items[0] = Helper.ToName (type);
			items[1] = "-";
			items[2] = keySize.ToString () + "bits";
			items[3] = blockSize.ToString () + "bits";
			try {
				uint[] result = KeySchedulingTest.Run (type, keySize, blockSize);
				items[4] = result[0].ToString () + "/s";
				items[5] = result[1].ToString () + "/s";
			} catch {
				items[4] = "N/A";
				items[5] = "N/A";
			}
			listResult.Items.Add (new ListViewItem (items));
			while (listResult.Items.Count > 100) listResult.Items.RemoveAt (0);
			listResult.EnsureVisible (listResult.Items.Count - 1);
			Application.DoEvents ();
		}

		private void btnRunSpeedTest_Click (object sender, EventArgs e)
		{
			ImplementationType type = Helper.ToImplementationType ((string)cbImpl.SelectedItem);
			string mode = (string)cbBlockMode.SelectedItem;
			int keySize = (int)cbKeySize.SelectedItem;
			int blockSize = (int)cbBlockSize.SelectedItem;
			string[] items = new string [6];
			items[0] = Helper.ToName (type);
			items[1] = mode;
			items[2] = keySize.ToString () + "bits";
			items[3] = blockSize.ToString () + "bits";
			try {
				double[] result = SpeedTest.Run (type, mode, keySize, blockSize, ToSize ((string)cbDataSize.SelectedItem), (int)numThreads.Value);
				items[4] = result[0].ToString ("f2") + "Mbps";
				items[5] = result[1].ToString ("f2") + "Mbps";
			} catch {
				items[4] = "N/A";
				items[5] = "N/A";
			}
			listResult.Items.Add (new ListViewItem (items));
			while (listResult.Items.Count > 100) listResult.Items.RemoveAt (0);
			listResult.EnsureVisible (listResult.Items.Count - 1);
			Application.DoEvents ();
		}

		static int ToSize (string text)
		{
			int val = 0;
			foreach (char c in text) {
				if (char.IsDigit (c))
					val = val * 10 + (c - '0');
				else
					break;
			}
			if (text.Contains ("MB"))
				return val * 1024 * 1024;
			if (text.Contains ("KB"))
				return val * 1024;
			return val;
		}

		private void btnClear_Click (object sender, EventArgs e)
		{
			listResult.Items.Clear ();
		}

		private void listResult_ColumnClick (object sender, ColumnClickEventArgs e)
		{
			if (e.Column == _sorter.SortColumn) {
				if (_sorter.Order == SortOrder.Ascending) {
					_sorter.Order = SortOrder.Descending;
				} else {
					_sorter.Order = SortOrder.Ascending;
				}
			} else {
				_sorter.SortColumn = e.Column;
				_sorter.Order = SortOrder.Ascending;
			}

			if (!checkBox1.Checked)
				checkBox1.Checked = true;
			else
				listResult.Sort ();
		}

		private void checkBox1_CheckedChanged (object sender, EventArgs e)
		{
			if (checkBox1.Checked)
				listResult.Sort ();
			else {
				listResult.Sorting = SortOrder.None;
				_sorter.Order = SortOrder.None;
			}
		}

		public class ListViewColumnSorter : IComparer
		{
			private int ColumnToSort;
			private SortOrder OrderOfSort;
			private IComparer ObjectCompare;

			public ListViewColumnSorter ()
			{
				ColumnToSort = 0;
				OrderOfSort = SortOrder.None;
				ObjectCompare = new CaseInsensitiveComparer ();
			}

			public int Compare (object x, object y)
			{
				ListViewItem xx = (ListViewItem)x;
				ListViewItem yy = (ListViewItem)y;
				int result = xx.SubItems[ColumnToSort].Text.Length.CompareTo (yy.SubItems[ColumnToSort].Text.Length);
				if (result == 0)
					result = ObjectCompare.Compare (xx.SubItems[ColumnToSort].Text, yy.SubItems[ColumnToSort].Text);

				if (OrderOfSort == SortOrder.Ascending) {
					return result;
				} else if (OrderOfSort == SortOrder.Descending) {
					return -result;
				} else {
					return 0;
				}
			}

			public int SortColumn {
				set { ColumnToSort = value; }
				get { return ColumnToSort;  }
			}

			public SortOrder Order {
				set { OrderOfSort = value; }
				get { return OrderOfSort;  }
			}
		}

		private void chkKillTime_CheckedChanged (object sender, EventArgs e)
		{
			Thread thrd = new Thread (KillTimeThread);
			thrd.Start ();
		}

		delegate void SetComboBoxSelectedIndexHandler (ComboBox cb, int index);
		void SetComboBoxSelectedIndex (ComboBox cb, int index)
		{
			cb.SelectedIndex = index;
		}

		void KillTimeThread ()
		{
			object[] args = new object[] {null, EventArgs.Empty};
			EventHandler handler = new EventHandler (btnRunSpeedTest_Click);
			SetComboBoxSelectedIndexHandler set = new SetComboBoxSelectedIndexHandler (SetComboBoxSelectedIndex);

			while (true) {
				if (!chkKillTime.Checked) break;
				for (int i1 = 0; i1 < cbImpl.Items.Count; i1++) {
					if (!chkKillTime.Checked) break;
					Invoke (set, cbImpl, i1);
					for (int i2 = 0; i2 < cbBlockMode.Items.Count; i2++) {
						if (!chkKillTime.Checked) break;
						Invoke (set, cbBlockMode, i2);
						for (int i3 = 0; i3 < cbKeySize.Items.Count; i3++) {
							if (!chkKillTime.Checked) break;
							Invoke (set, cbKeySize, i3);
							for (int i4 = 0; i4 < cbBlockSize.Items.Count; i4++) {
								if (!chkKillTime.Checked) break;
								Invoke (set, cbBlockSize, i4);
								Invoke (handler, args);
							}
						}
					}
				}
			}
			Invoke (new EventHandler (btnClear_Click), args);
		}
	}
}