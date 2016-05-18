using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using System.Data.SQLite;

namespace Kadan
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Performer { get; set; }
        public string Duration { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public string FullName { get; set; }

        private string quote = "'";
        private string quoteHTML = "&#39";

        public Song() {
        }

        public Song(TagLib.File audioFile, string path, string fullName) {
            this.Title = audioFile.Tag.Title == null ? @"Title " : audioFile.Tag.Title;
            this.Performer = audioFile.Tag.FirstPerformer == null ? @"Performer " : audioFile.Tag.FirstPerformer;
            this.Album = audioFile.Tag.Album == null ? @"Album " : audioFile.Tag.Album;
            this.Duration = audioFile.Properties.Duration.ToString("mm\\:ss") == null ? @"Duration " : audioFile.Properties.Duration.ToString("mm\\:ss");
            this.Year = audioFile.Tag.Year.ToString() == null ? @"Year " : audioFile.Tag.Year.ToString();
            this.FullName = fullName == null ? @"FullName " : fullName;
            updateQuotes(quote, quoteHTML);
        }
        
        public Song(SQLiteDataReader reader) {
            this.Id = Convert.ToInt32(reader["id"]);
            this.Title = reader["title"].ToString();
            this.Performer = reader["performer"].ToString();
            this.Album = reader["album"].ToString();
            this.Duration = reader["duration"].ToString();
            this.Year = reader["year"].ToString();
            this.FullName = reader["fullName"].ToString();
            updateQuotes(quoteHTML, quote);
        }

        public Song(System.Collections.IList selectedItem)
        {
            foreach (var item in selectedItem)
            {
                var song = item as Song;

                this.Id = song.Id;
                this.Title = song.Title;
                this.Performer = song.Performer;
                this.Album = song.Album;
                this.Duration = song.Duration;
                this.Year = song.Year;
                this.FullName = song.FullName;
            }
        }

        private void updateQuotes(string from, string to)
        {
            if (Title.Contains(from) && Title != null)  
                Title = Title.Replace(from, to);
            if (Album.Contains(from) && Album != null)
                Album = Album.Replace(from, to);
            if (Performer.Contains(from) && Performer != null)
                Performer = Performer.Replace(from, to);
            if (Year.Contains(from) && Year != null)
                Year = Year.Replace(from, to);
            if (FullName.Contains(from) && FullName != null)
                FullName = FullName.Replace(from, to);
            if (Duration.Contains(from) && Duration != null)
                Duration = Duration.Replace(from, to);
        }

        public Song(Song song) {
            this.Id = song.Id;
            this.Title = song.Title;
            this.Performer = song.Performer;
            this.Album = song.Album;
            this.Duration = song.Duration;
            this.Year = song.Year;
            this.FullName = song.FullName;
        }
    }
}
