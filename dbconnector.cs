using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite; // from nuget pkg

namespace UrbanApp
{
    public class dbconnector : Form1
    {
        /*
         * This class is used to connect to the local sqlite db
         * The file name is fetched from the user
         */
        public string db_filepath;
        private SQLiteConnection con;
        public dbconnector(string file_path)
        {
            db_filepath = file_path;
            con = new SQLiteConnection("data source=" + db_filepath);
        }



        // ---- v0
        // public string databasepath = null;
        //        public SQLiteConnection con = new SQLiteConnection("data source="+databasepath);
        //public string connectionString = string.Format("DataSource={0}", dbfilepath);
        ///public SQLiteConnection con = new SQLiteConnection("data source=default.db");

        public SQLiteConnection GetConnection()
        {
            return con;
        }
        public void openConnection()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
        }

        public void closeConnection()
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}
