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
        public string Path { get; set; }

        public Song() {
        }

        public Song(int id, string title, string performer, string album, string duration, string year, string path) {
            this.Id = id;
            this.Title = title;
            this.Performer = performer;
            this.Album = album;
            this.Duration = duration;
            this.Year = year;
            this.Path = path;
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
            this.Id = Convert.ToInt32(reader["id"]);
            this.Title = reader["title"].ToString();
            this.Performer = reader["performer"].ToString();
            this.Album = reader["album"].ToString();
            this.Duration = reader["duration"].ToString();
            this.Year = reader["year"].ToString();
            this.Path = reader["path"].ToString();
        }

        public Song(System.Collections.IList selectedItem)
        {
            foreach (var item in selectedItem)
            {
                var song = item as Song;
                songWithSong(song);
            }
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

        private void songWithSong(Song song) {
            this.Id = song.Id;
            this.Title = song.Title;
            this.Performer = song.Performer;
            this.Album = song.Album;
            this.Duration = song.Duration;
            this.Year = song.Year;
            this.Path = song.Path;
        }
    }
}
