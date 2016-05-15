using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagLib;

namespace Kadan
{
    class MusicManager
    {
        //properties
        private int count;
        public int Count { get; set; }

        //Delegate
        public delegate void MusicManagerMessageDelegate(string message);
        MusicManagerMessageDelegate messageDelegate;

        public delegate void MusicManagerSuccessDelegate(List<Song> dictionary);
        MusicManagerSuccessDelegate successDelegate;

        public void RegisterMessageDelegate(MusicManagerMessageDelegate myDelegate) {
            this.messageDelegate = myDelegate;
        }
        public void RegisterSuccessErrorDelegate(MusicManagerSuccessDelegate myDelegate) {
            this.successDelegate = myDelegate;
        }

        //Methods
        public void getAllAudioFromFolderWithPath(string folderPath)
        {
            if (folderPath != null)
            {
                DirectoryInfo dir = new DirectoryInfo(folderPath);

                if (dir.Exists)
                {
                    List<Song> songs = new List<Song>();

                    foreach (FileInfo file in dir.GetFiles("*.mp3"))
                    {
                        var audioFile = TagLib.File.Create(file.FullName);
                        Song song = new Song(audioFile);
                        songs.Add(song);
                        count++;
                    }
                    if (count == 0)
                    {
                        messageDelegate("There are no files in this directory. Try another one please!");
                    }
                    else {
                        messageDelegate(folderPath);
                        successDelegate(songs);
                    }
                }
                else {
                    messageDelegate("There is no such directory. Try again please!");
                }
            }
        }
    }
}
