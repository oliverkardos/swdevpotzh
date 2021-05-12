using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite; // from nuget pkg

namespace UrbanApp
{
    public class dbconnector
    {
        /*
         * This class is used to connect to the local sqlite db
         * The file name is fetched from the user
         */
        private SQLiteConnection con = new SQLiteConnection("data source=hoteldb.db");

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
