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
          //  Song song = new Song(selected);
          //  selected.

            EditWindow editWindow = new EditWindow();
            editWindow.Show();
            this.Hide();
        }

        //Delegate
        private void gotMessage(String message) {
            label.Content = message;
        }

        private void gotListFromMetadata(List<Song> list) {
            dataGrid.ItemsSource = list;
        }
        
    }
}
