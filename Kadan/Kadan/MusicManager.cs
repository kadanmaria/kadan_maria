using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagLib;
using System.Data.SQLite;

namespace Kadan
{
    class MusicManager
    {
        //properties
        private int count;
        public int Count { get; set; }

        private SQLConnector connector;

        //Delegate
        public delegate void MusicManagerMessageDelegate(string message);
        MusicManagerMessageDelegate messageDelegate;

        public delegate void MusicManagerSuccessDelegate(List<Song> dictionary);
        MusicManagerSuccessDelegate successDelegate;
        
        public void RegisterMessageDelegate(MusicManagerMessageDelegate myDelegate) {
            this.messageDelegate = myDelegate;
        }
        public void RegisterSuccessDelegate(MusicManagerSuccessDelegate myDelegate) {
            this.successDelegate = myDelegate;
        }

        //Methods
        public void initializeWithMusicFromDB() {
            SQLConnector connector = new SQLConnector();
            this.connector = connector;
            
            successDelegate(this.connector.LoadData());
        }

        public void clearAllAudioFromDB() {
            connector.clearDB();
            successDelegate(this.connector.LoadData());
        }

        public void getAllAudioFromFolderWithPath(string folderPath)
        {
            if (folderPath != null && folderPath !=@"")
            {
                DirectoryInfo dir = new DirectoryInfo(folderPath);

                if (dir.Exists)
                {
                    List<Song> songs = new List<Song>();

                    foreach (FileInfo file in dir.GetFiles("*.mp3"))
                    {
                        var audioFile = TagLib.File.Create(file.FullName);
                        Song song = new Song(audioFile, folderPath, file.FullName);
                        songs.Add(song);
                        count++;
                    }
                    if (count == 0)
                    {
                        messageDelegate("There are no files in this directory. Try another one please!");
                    }
                    else {
                        messageDelegate(folderPath);
                        
                        this.connector.uploadToDB(songs);
                        successDelegate(this.connector.LoadData());
                    }
                }
                else {
                    messageDelegate("There is no such directory. Try again please!");
                }
            }
        }
        
        public void saveUpdatesToMetadata(Song song) {
            SQLConnector connector = new SQLConnector();
            this.connector = connector;

            if (System.IO.File.Exists(song.FullName))
            {
                using (var fileSong = TagLib.File.Create(song.FullName))
                {
                    string[] performers = new string[] { song.Performer };
                    fileSong.Tag.Title = song.Title;
                    fileSong.Tag.Performers = performers;
                    fileSong.Tag.Year = (uint)Convert.ToInt32(song.Year);
                    fileSong.Tag.Album = song.Album;

                    fileSong.Save();

                    messageDelegate("Saved!");
                    this.connector.updateSongInDB(song);
                }
            }
            else {
                messageDelegate("No such song!");
                this.connector.deleteSongFromDB(song);
            }      
        }

        public void searchInDBWithOptions(Dictionary<String, String>  args) {
            string query = "select id, title, performer, duration, album, year, fullName from songs where";
            foreach (var item in args.Keys) {
                query = string.Concat(query," " + item + " like '%" + args[item] + "%'");
            }
            successDelegate(connector.LoadData(query));
        }
    }

}
