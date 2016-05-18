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

        public SQLConnector() {
            if (!System.IO.File.Exists("Music.sqlite"))
            {
                CreateTable();
            }            
        }

        private void SetConnection()
        {
            sqlConnection = new SQLiteConnection("Data Source=Music.sqlite;Version=3;New=False;Compress=True;");
        }
        
        private void CreateTable()
        {
            SQLiteConnection.CreateFile("Music.sqlite");
            ExecuteQuery("create table songs (title varchar(30), performer varchar(30), duration varchar(30), album varchar(30), year int, path varchar(30))");
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

        public void uploadToDB(List<Song> songs) {
            foreach (Song song in songs) {
                ExecuteQuery("insert or replace into songs (title, performer, duration, album, year, path) values ('" + song.Title + "', '" + song.Performer + "', '" + song.Duration + "', '" + song.Album + "', " + song.Year + ", '" + song.Path + "' )");
            }
        }
        public void clearDB()
        {
            SetConnection();
            ExecuteQuery("delete from songs");  
        }

        public List<Song> LoadData()
        {
            SetConnection();
            sqlConnection.Open();
            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "select title, performer, duration, album, year, path from songs";
            SQLiteDataReader reader = sqlCommand.ExecuteReader();

            List<Song> songs = new List<Song>();
            while (reader.Read())
            {
                Song song = new Song(reader);
                songs.Add(song);
            }
            sqlConnection.Close();
            return songs;
        }
    }
}
