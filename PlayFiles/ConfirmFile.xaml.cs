using Microsoft.Win32;
using PlayFiles.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace PlayFiles
{
    /// <summary>
    /// Interaction logic for ConfirmFile.xaml
    /// </summary>
    public partial class ConfirmFile : Window
    {
        Logic.FileInfo FileInfo;
        MainWindow window;
        public ConfirmFile(Logic.FileInfo fileInfo,MainWindow window)
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            this.FileInfo = fileInfo;
            this.window = window;

            if (FileInfo.type == FileType.YT || FileInfo.type == FileType.WEB)
            {
                FilePathText.Text = "Media URL:";
                ChangePath.Visibility = Visibility.Collapsed;
            }

            FilePath.Text = fileInfo.Path;
            FileName.Text = fileInfo.Name;
            HoursComboBox.SelectedIndex = fileInfo.activeTime.Hour;
            MinutesComboBox.SelectedIndex = fileInfo.activeTime.Minute;
            SecondsComboBox.SelectedIndex = fileInfo.activeTime.Second;

            OpenOnFullscreen.IsChecked = FileInfo.OpenAsFullscreen;
            FocusOnPlay.IsChecked = FileInfo.FocusMediaWhenPlayed;
        }
        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            FileInfo.Path = FilePath.Text;
            FileInfo.Name = FileName.Text;
            FileInfo.activeTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HoursComboBox.SelectedIndex, MinutesComboBox.SelectedIndex, SecondsComboBox.SelectedIndex);

            if(!Logic.FileInfo.FilesLoaded.Contains(FileInfo))
                Logic.FileInfo.FilesLoaded.Add(FileInfo);
            window.UpdateButtons();
            Close();
        }

        private void OpenOnFullscreen_Click(object sender, RoutedEventArgs e)
        {
            FileInfo.OpenAsFullscreen = !FileInfo.OpenAsFullscreen;
        }

        private void ChangePath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                FileInfo.Path = openFileDialog.FileName;
                FilePath.Text = openFileDialog.FileName;
            }
        }
    }
}
