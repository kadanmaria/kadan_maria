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
                        Song song = new Song(audioFile, folderPath);
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
    }
}
