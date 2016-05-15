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
            musicManager.RegisterSuccessErrorDelegate(new MusicManager.MusicManagerSuccessDelegate(gotListFromMetadata));  
            this.musicManager = musicManager;
        }

        //Actions
        private void button_Click(object sender, RoutedEventArgs e) {


            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            
            string path = dialog.SelectedPath;
            dataGrid.ItemsSource = null;
            musicManager.getAllAudioFromFolderWithPath(path);
                 
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
