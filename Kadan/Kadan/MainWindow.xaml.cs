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
        private Dictionary<String, String> args;

        //Main method
        public MainWindow() {
            InitializeComponent();

            MusicManager musicManager = new MusicManager();
            musicManager.RegisterMessageDelegate(new MusicManager.MusicManagerMessageDelegate(gotMessage));
            musicManager.RegisterSuccessDelegate(new MusicManager.MusicManagerSuccessDelegate(gotListFromMetadata));
            this.musicManager = musicManager;

            this.musicManager.updateData();
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
            this.SearchButton.IsEnabled = false;
            this.BackButton.IsEnabled = false;
        }

        //Delegate
        private void gotMessage(String message) {
        }

        private void gotListFromMetadata(List<Song> list) {
            dataGrid.ItemsSource = list;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            musicManager.searchInDBWithOptions(args);

            comboBox.Items.Clear();
            this.searchTextBox.Text = @"";

            this.AddButton.IsEnabled = false;
            this.BackButton.IsEnabled = true;
            this.SearchButton.IsEnabled = false;
            this.comboBox.IsEnabled = false;
            this.searchTextBox.IsEnabled = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.AddButton.IsEnabled = true;
            this.SearchButton.IsEnabled = false;
            this.comboBox.IsEnabled = true;
            this.searchTextBox.IsEnabled = true;
            this.textBlock.Text = @"";

            comboBox.Items.Clear();
            foreach (var item in dataGrid.Columns)
            {
                if (item.Header.ToString() != "Id" && item.Header.ToString() != "FullName")
                {
                    comboBox.Items.Add(item.Header);
                }
            }

            musicManager.updateData();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.args == null)
            {
                Dictionary<String, String> args = new Dictionary<string, string>();
                this.args = args;
            }
            if (comboBox.SelectedValue != null && searchTextBox.Text != null)
            {
                string s1 = comboBox.SelectedValue.ToString().ToLower().Replace("'", "&#39");
                string s2 = searchTextBox.Text.Replace("'", "&#39");
                args.Add(s1, s2);

                this.textBlock.Text = string.Concat(this.textBlock.Text, comboBox.SelectedValue.ToString() + ": "+ searchTextBox.Text +"\n");
                this.searchTextBox.Text = @"";
                if (comboBox.Items.Count > 0)
                {
                    this.comboBox.Items.Remove(comboBox.SelectedValue);
                } else
                {
                    this.AddButton.IsEnabled = false;

                }
            }
            this.SearchButton.IsEnabled = true;
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            comboBox.Items.Clear();

        }
    }
}
