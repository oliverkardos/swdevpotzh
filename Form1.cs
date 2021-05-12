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
using System.Data.SQLite;
using System.Data.SqlClient;

namespace UrbanApp
{
    public partial class Form1 : Form
    {
        public string dbfilepath = null;
        bool dbfileselected = false;
        //  dbconnector sqliteconn = new dbconnector("default.db");
        SQLiteCommand dbcommand;
        bool connectionOpen = false;
        bool writemodeflag = true;
        string textfilepath = null;
        DataTable dt2 = new DataTable(); // txt processing
        List<int> ids = new List<int>();
        List<string> cordsx = new List<string>();
        List<string> cordsy = new List<string>();
        List<string> names = new List<string>();
        List<string> data1 = new List<string>();
        List<string> data2 = new List<string>();
        SQLiteCommand cmddbwrite;

        //private Button selectButton;
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
                writemodeflag = false;

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
            if (writemodeflag)
            {
                // cmddbwrite

                dbconnector sqliteconn = new dbconnector(dbfilepath);
                sqliteconn.openConnection();

                foreach (int i in ids)
                {
                    cmddbwrite = new SQLiteCommand("insert into urbanization(id,cordx,cordy,name,data1,data2) values('" + ids[i] + "','" + cordsx[i] + "','" + cordsy[i] + "','" + names[i] + "','" + data1[i] + "','" + data2[i] + "');", sqliteconn.GetConnection());
                    cmddbwrite.ExecuteNonQuery(); // insert
                }
                MessageBox.Show("Database write success");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dbfilepath != null)
            {
                dbconnector sqliteconn = new dbconnector(dbfilepath);
                sqliteconn.openConnection();
                connectionOpen = true;
                button2.BackColor = Color.Green;
                dbcommand = new SQLiteCommand("SELECT * FROM urbanization", sqliteconn.GetConnection());
                SQLiteDataReader dbreader = dbcommand.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(dbreader);
                /* v1 for testing - manual fill
                                dt.Columns.Add("ID", typeof(int));
                                dt.Columns.Add("CoordX", typeof(string));
                                dt.Columns.Add("CoordY", typeof(string));
                                dt.Columns.Add("Name", typeof(string));
                                dt.Columns.Add("data1", typeof(int));
                                dt.Columns.Add("data2", typeof(int));
                */
                // v2 autofill
                dataGridView1.DataSource = dt;
                sqliteconn.Close();
            }
            else MessageBox.Show("Error: no database file is selected, nothing to connect to");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ids.Clear();
            cordsx.Clear();
            cordsy.Clear();
            names.Clear();
            data1.Clear();
            data2.Clear();
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Title = "Select your TEXT CSV database file";
            openFileDialog1.DefaultExt = "db";
            openFileDialog1.Filter = "CSV TEXT files (*.csv)|*.csv|CSV files with txt extension (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.ShowDialog();
            csvPathTextBox.Text = openFileDialog1.FileName;
            textfilepath = openFileDialog1.FileName;
            button3.Text = "[Browse for next file]";
            StreamReader sr = new StreamReader(textfilepath, true); // auto encoding detection
            dt2.Clear(); // just to be safe, in case this isnt the first file
                         // manual fill
                         // first the headers
            dt2.Columns.Add("ID", typeof(int));
            dt2.Columns.Add("CoordX", typeof(string));
            dt2.Columns.Add("CoordY", typeof(string));
            dt2.Columns.Add("Name", typeof(string));
            dt2.Columns.Add("data1", typeof(int));
            dt2.Columns.Add("data2", typeof(int));

            // then rows 
            // v1
            /*
            do
            {
                string linebylineread = sr.ReadLine();
                textBox2.Text += linebylineread + "  " + "\n";
                MessageBox.Show(linebylineread);
                //string[] splitatcomma = linebylineread.Split(';', 6);
                 string[] splitatcomma = linebylineread.Split(';',6);

                ids.Add(Convert.ToInt16(splitatcomma[0]));
                cordsx.Add(splitatcomma[1]);
                cordsy.Add(splitatcomma[2]);
                names.Add(splitatcomma[3]);
                data1.Add(splitatcomma[4]);
                data2.Add(splitatcomma[5]);
                dt2.Rows.Add(splitatcomma);
            }
            while (sr.ReadLine() != null);
            */
            string linebylineread = "";
            // v2
            while ((linebylineread = sr.ReadLine()) != null)
            {
               // string linebylineread = sr.ReadLine();
                textBox2.Text += linebylineread + "  " + "\n";
                MessageBox.Show(linebylineread);
                //string[] splitatcomma = linebylineread.Split(';', 6);
                string[] splitatcomma = linebylineread.Split(';', 6);

                ids.Add(Convert.ToInt16(splitatcomma[0]));
                cordsx.Add(splitatcomma[1]);
                cordsy.Add(splitatcomma[2]);
                names.Add(splitatcomma[3]);
                data1.Add(splitatcomma[4]);
                data2.Add(splitatcomma[5]);
                dt2.Rows.Add(splitatcomma);
            }
      //      while (sr.ReadLine() != null);
            sr.Close();
            sr.Dispose();

            dataGridView2.DataSource = dt2;


        }

private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
{

}

public void button4_Click(object sender, EventArgs e)
{

//


}

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
