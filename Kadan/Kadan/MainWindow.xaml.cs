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

namespace Kadan
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MusicManager musicManager;
        
        public MainWindow()
        {
            InitializeComponent();

            MusicManager musicManager = new MusicManager();
            musicManager.RegisterDelegate(new MusicManager.MusicManagerDelegate(showMessage));  
            this.musicManager = musicManager;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != @"")
            {
                musicManager.getAllAudioFromFolderWithPath(textBox.Text);
                textBox.Text = @"";
            }
        }

        private void showMessage(String message)
        {
            label.Content = message;
            Console.WriteLine(message);
        }

    }
}
