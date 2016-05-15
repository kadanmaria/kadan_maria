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
        public delegate void MusicManagerDelegate(string message);
        MusicManagerDelegate myDelegate;
        private int count { get; set; }

        public void RegisterDelegate(MusicManagerDelegate myDelegate)
        {
            this.myDelegate = myDelegate;
        }

        public MusicManager() {
        }

        public void getAllAudioFromFolderWithPath(string folderPath)
        { 
            DirectoryInfo dir = new DirectoryInfo(folderPath);

            if (dir.Exists)
            {
                foreach (FileInfo file in dir.GetFiles("*.mp3"))
                {
                    var audioFile = TagLib.File.Create(file.FullName);
                    Console.WriteLine("Album: {0}\n\n", audioFile.Tag.Album);
                    count++;
                }
                if (count == 0 && myDelegate != null)
                {
                    myDelegate("There are no files in this directory. Try another one please!");
                }
                else if (count != 0 && myDelegate != null) {
                    myDelegate(folderPath);
                }
            }
            else {
                if (myDelegate != null)
                    myDelegate("There is no such directory. Try again please!");
            }

        }
        
        public void wrileInfoFromFile(String path) {
            var audioFile = TagLib.File.Create(path);

            Console.WriteLine("Album: {0}\nSinger: {1}\nTitle: {2}\nYear: {3}\nDuration: {4}"
                , audioFile.Tag.Album
                , String.Join(", ", audioFile.Tag.Performers)
                , audioFile.Tag.Title
                , audioFile.Tag.Year
                , audioFile.Properties.Duration.ToString("mm\\:ss"));
        }

        
    }
}
