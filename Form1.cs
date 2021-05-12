using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security;

namespace UrbanApp
{
    public partial class Form1 : Form
    {
        public string dbfilepath = null;

        private Button selectButton;
        public Form1()
        {
            InitializeComponent();
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog2.InitialDirectory = @"C:\";
            openFileDialog2.Title = "Select your SQLITE database file";
            openFileDialog2.DefaultExt = "db";
            openFileDialog2.Filter = "SQLITE DB files (*.db)|*.db|SQLITE files with sqlite extension (*.sqlite)|*.sqlite|All files (*.*)|*.*";
            openFileDialog2.FilterIndex = 1;
            openFileDialog2.ShowDialog();
            textBox1.Text = openFileDialog2.FileName;
            dbfilepath = openFileDialog2.FileName;
            button1.Enabled = false;
            button1.Text = "[File selected]";
            textBox1.Enabled = false;
            button2.Enabled = true;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                openFileDialog2.ReadOnlyChecked = true;
                openFileDialog2.ShowReadOnly = true;
                MessageBox.Show("Write-protected mode is now enabled. You can now select 'read-only' option when browsing for your database file. Whether you select that or not, the app will not write new data to the db. Some functionalities of the app have been restricted in order to be able to work in write-protected mode. If you want to revert from read-only mode to standard read-write mode, please restart the app.");
                csvPathTextBox.Text = "Write prevention is enabled";
                btnWriteToDB.Enabled = false;
                btnWriteToDB.Text = "write disabled";

            }
            if (!checkBox1.Checked)
            {
                checkBox1.Checked = true;
                //openFileDialog2.ReadOnlyChecked = false;
                //openFileDialog2.ShowReadOnly = false;
                MessageBox.Show("Please restart the app to revert to read-write mode as this feature is designed not to alter filestreams that are already opened.");
            }
        }

        private void btnWriteToDB_Click(object sender, EventArgs e)
        {

        }
    }
}
