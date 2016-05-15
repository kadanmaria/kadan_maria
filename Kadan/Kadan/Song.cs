using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace Kadan
{
    class Song
    {
        public string Title { get; set; }
        public string Performer { get; set; }
        public string Duration { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }

        public Song(TagLib.File audioFile) {

            this.Title = audioFile.Tag.Title;
            this.Performer = audioFile.Tag.FirstPerformer;
            this.Album = audioFile.Tag.Album;
            this.Duration = audioFile.Properties.Duration.ToString("mm\\:ss");
            this.Year = audioFile.Tag.Year.ToString();
        }
    }
}
