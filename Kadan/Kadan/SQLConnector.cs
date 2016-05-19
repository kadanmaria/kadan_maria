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

        public SQLConnector()
        {
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
            ExecuteQuery("create table songs (id INTEGER PRIMARY KEY, title varchar(30), performer varchar(30), duration varchar(30), album varchar(30), year int, fullName varchar(30))");
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

        public void updateSongInDB(Song song)
        {
            ExecuteQuery("UPDATE songs SET title = '" + song.Title + "', performer = '" + song.Performer + "', album = '" + song.Album + "', year = " + song.Year + " WHERE id = " + song.Id);
        }

        public void deleteSongFromDB(Song song)
        {
            ExecuteQuery("delete from songs where id = " + song.Id);
        }

        public void uploadToDB(List<Song> songs)
        {

            foreach (Song song in songs)
            {
                string get = "select id from songs where fullName = '" + song.FullName + "'";
                List<string> idList = selectStateWithQuery(get);
                if (idList.Count > 0)
                {
                    foreach (string id in idList)
                    {
                        ExecuteQuery("update songs set title ='" + song.Title + "', performer ='" + song.Performer + "', duration ='" + song.Duration + "', album ='" + song.Album + "', year =" + song.Year + ", fullName ='" + song.FullName + "' where id=" + id);
                    }
                }
                else
                {
                    ExecuteQuery("insert into songs (title, performer, duration, album, year, fullName) values ('" + song.Title + "', '" + song.Performer + "', '" + song.Duration + "', '" + song.Album + "', " + song.Year + ", '" + song.FullName + "')");
                }
            }
        }

        public void clearDB()
        {
            SetConnection();
            ExecuteQuery("delete from songs");
        }

        public List<Song> LoadData()
        {
            SQLiteDataReader reader = executeSelect("select id, title, performer, duration, album, year, fullName from songs");

            List<Song> songs = new List<Song>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    Song song = new Song(reader);
                    songs.Add(song);
                }
                
            }
            sqlConnection.Close();
            return songs;
        }

        public List<Song> LoadData(string str)
        {
            SQLiteDataReader reader = executeSelect(str);

            List<Song> songs = new List<Song>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    Song song = new Song(reader);
                    songs.Add(song);
                }
            }
            sqlConnection.Close();
            return songs;
        }

        private SQLiteDataReader executeSelect(string str)
        {
            SetConnection();
            sqlConnection.Open();
            sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = str;
            SQLiteDataReader reader = null;
            try {
                reader = sqlCommand.ExecuteReader();
            } catch
            {

            }
            return reader;
        }

        public List<String> selectStateWithQuery(string str)
        {
            SQLiteDataReader reader = executeSelect(str);
            List<string> ids = new List<string>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    ids.Add(reader["id"].ToString());
                }
            }
            sqlConnection.Close();
            return ids;
        }
    }
}
