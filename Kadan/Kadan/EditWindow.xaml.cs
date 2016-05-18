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

        public EditWindow()
        {
            InitializeComponent();
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
            Song song = new Song(SelectedSong.Id, this.TitleTextBox.Text, this.PerformerTextBox.Text, this.AlbumTextBox.Text, this.DurationTextBox.Text, this.YearTextBox.Text, this.PathTextBox.Text);
            SQLConnector connector = new SQLConnector();
            connector.updateSongInDB(song);

            Window_Closing(sender, null);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.TitleTextBox.Text = this.SelectedSong.Title;
            this.AlbumTextBox.Text = this.SelectedSong.Album;
            this.PerformerTextBox.Text = this.SelectedSong.Performer;
            this.YearTextBox.Text = this.SelectedSong.Year;
            this.DurationTextBox.Text = this.SelectedSong.Duration;
            this.PathTextBox.Text = this.SelectedSong.Path;
        }
    }
}
