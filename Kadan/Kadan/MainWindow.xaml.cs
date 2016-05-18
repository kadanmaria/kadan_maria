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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace Kadan
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicManager musicManager;

        //Main method
        public MainWindow() {
            InitializeComponent();

            MusicManager musicManager = new MusicManager();
            musicManager.RegisterMessageDelegate(new MusicManager.MusicManagerMessageDelegate(gotMessage));
            musicManager.RegisterSuccessDelegate(new MusicManager.MusicManagerSuccessDelegate(gotListFromMetadata));  
            this.musicManager = musicManager;

            this.musicManager.initializeWithMusicFromDB();
        }

        //Actions
        private void button_GetClick(object sender, RoutedEventArgs e) {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dataGrid.ItemsSource = null;
                string path = dialog.SelectedPath;
                musicManager.getAllAudioFromFolderWithPath(path);
            }
        }

        private void button_ClearClick(object sender, RoutedEventArgs e) {
            musicManager.clearAllAudioFromDB();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var grid = sender as System.Windows.Controls.DataGrid;
            var selected = grid.SelectedItems;

            Song song = new Song(selected);
            EditWindow editWindow = new EditWindow();
            editWindow.SelectedSong = song;

            editWindow.Show();
            this.Hide();
        }

        private void Window_ContentRendered(object sender, EventArgs e) {
            foreach (var item in dataGrid.Columns)
            {
                if (item.Header.ToString() != "Id" && item.Header.ToString() != "FullName")
                {
                    comboBox.Items.Add(item.Header);
                }
            }

            foreach (var item in dataGrid.Columns)
            {
                if (item.Header.ToString() == "Id" || item.Header.ToString() == "FullName")
                {
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        //Delegate
        private void gotMessage(String message) {
            //.Content = message;
        }

        private void gotListFromMetadata(List<Song> list) {
            dataGrid.ItemsSource = list;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> args = new Dictionary<string, string>();
            args.Add(comboBox.SelectedValue.ToString().ToLower(), textBox.Text);

            musicManager.searchInDBWithOptions(args);
        }
    }
}
