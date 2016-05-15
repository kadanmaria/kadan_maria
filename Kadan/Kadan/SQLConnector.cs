using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Kadan
{
    class SQLConnector
    {
        private SQLiteConnection sqlConnection;
        private SQLiteCommand sqlCommand;
        private SQLiteDataAdapter DB;
       // private DataSet DS = new DataSet();
       // private DataTable DT = new DataTable();

        private void SetConnection()
        {
            sqlConnection = new SQLiteConnection("Data Source=Music.db;Version=3;New=False;Compress=True;");
        }

        private void ExecuteQuery(string txtQuery)
        {
            SetConnection();
            sqlConnection.Open();
            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = txtQuery;
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        private void LoadData()
        {
            SetConnection();
            sqlConnection.Open();
            sqlCommand = sqlConnection.CreateCommand();
            string CommandText = "select id, desc from mains";
            DB = new SQLiteDataAdapter(CommandText, sqlConnection);
        //    DS.Reset();
        //    DB.Fill(DS);
        //    DT = DS.Tables[0];
        //    Grid.DataSource = DT;
            sqlConnection.Close();
        }

    }
}
