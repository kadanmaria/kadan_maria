using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kadan
{
    public partial class EditWindow : Window
    {
        public Song SelectedSong { get; set; }
        private MusicManager musicManager;

        public EditWindow()
        {
            InitializeComponent();

            MusicManager musicManager = new MusicManager();
            musicManager.RegisterMessageDelegate(new MusicManager.MusicManagerMessageDelegate(gotMessage));
            this.musicManager = musicManager;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Window_Closing(sender, null);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Song updatedSong = new Song(SelectedSong);
            updatedSong.Title = this.TitleTextBox.Text;
            updatedSong.Album = this.AlbumTextBox.Text;
            updatedSong.Performer = this.PerformerTextBox.Text;
            updatedSong.Year = this.YearTextBox.Text;
            
            this.musicManager.saveUpdatesToMetadata(updatedSong);

            Window_Closing(sender, null);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.TitleTextBox.Text = this.SelectedSong.Title;
            this.AlbumTextBox.Text = this.SelectedSong.Album;
            this.PerformerTextBox.Text = this.SelectedSong.Performer;
            this.YearTextBox.Text = this.SelectedSong.Year;
            this.DurationTextBox.Text = this.SelectedSong.Duration;
        }

        //Delegate
        private void gotMessage(String message)
        {
            MessageBox.Show(message);
        }
    }
}
