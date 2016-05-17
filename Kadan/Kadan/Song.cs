using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using System.Data.SQLite;

namespace Kadan
{
    class Song
    {
        public string Title { get; set; }
        public string Performer { get; set; }
        public string Duration { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string Path { get; set; }

        public Song() {
        }
        
        public Song(TagLib.File audioFile, string path) {
            this.Title = audioFile.Tag.Title == null ? @"Title " : audioFile.Tag.Title;
            this.Performer = audioFile.Tag.FirstPerformer == null ? @"Performer " : audioFile.Tag.FirstPerformer;
            this.Album = audioFile.Tag.Album == null ? @"Album " : audioFile.Tag.Album;
            this.Duration = audioFile.Properties.Duration.ToString("mm\\:ss");
            this.Year = audioFile.Tag.Year.ToString();
            this.Path = path;
            updateQuotes();
        }
        
        public Song(SQLiteDataReader reader) {
            this.Title = reader["title"].ToString();
            this.Performer = reader["performer"].ToString();
            this.Album = reader["album"].ToString();
            this.Duration = reader["duration"].ToString();
            this.Year = reader["year"].ToString();
            this.Path = reader["path"].ToString();
        }

        private void updateQuotes()
        {
            if (Title.Contains("'") && Title != null)  
                Title = Title.Replace("'", " ");
            if (Album.Contains("'") && Album != null)
                Album = Album.Replace("'", " ");
            if (Performer.Contains("'") && Performer != null)
                Performer = Performer.Replace("'", " ");
        }
    }
}
