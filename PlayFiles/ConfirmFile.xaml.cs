using Microsoft.Win32;
using PlayFiles.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

            SetupComboBox(SecondsComboBox, 60);
            SetupComboBox(MinutesComboBox, 60);
            SetupComboBox(HoursComboBox, 24);

            HoursComboBox.SelectedIndex = fileInfo.activeTime.Hour;
            MinutesComboBox.SelectedIndex = fileInfo.activeTime.Minute;
            SecondsComboBox.SelectedIndex = fileInfo.activeTime.Second;

            OpenOnFullscreen.IsChecked = FileInfo.OpenAsFullscreen;
            FocusOnPlay.IsChecked = FileInfo.FocusMediaWhenPlayed;
            PlayWithVLC.IsChecked = FileInfo.OpenWithVLCMediaPlayer;

            VolumeSlider.Value = FileInfo.Volume;

            AudioLayerGrid.Visibility = FileInfo.OpenWithVLCMediaPlayer? Visibility.Visible : Visibility.Collapsed;
            AudioLayerTextBox.Text = fileInfo.AudioLayer.ToString();

            CloseMediaControl.SetFileOfThatAction(FileInfo);
        }

        private void SetupComboBox(ComboBox comboBox, int maxValue)
        {
            comboBox.Items.Clear();

            // Make sure the style is applied before adding items
            comboBox.Style = (Style)FindResource("ModernComboBox");

            for (int i = 0; i < maxValue; i++)
            {
                string content = i > 9 ? i.ToString() : "0" + i.ToString();

                // Create a simple ComboBoxItem
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = content,
                    Style = (Style)FindResource("ModernComboBoxItem")
                };

                comboBox.Items.Add(item);
            }

            // Ensure the first item is selected by default if nothing is selected
            if (comboBox.SelectedIndex == -1 && comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = 0;
            }
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

            CloseMediaControl.UpdateCloseMediaAction();
            FileInfo.CloseMediaAction = CloseMediaControl.CloseMediaAction;

            if (!Logic.FileInfo.FilesLoaded.Contains(FileInfo))
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

        private void FocusOnPlay_Click(object sender, RoutedEventArgs e)
        {
            FileInfo.FocusMediaWhenPlayed = !FileInfo.FocusMediaWhenPlayed;
        }

        private void PlayWithVLC_Click(object sender, RoutedEventArgs e)
        {
            FileInfo.OpenWithVLCMediaPlayer = !FileInfo.OpenWithVLCMediaPlayer;
            AudioLayerGrid.Visibility = FileInfo.OpenWithVLCMediaPlayer ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AudioLayersButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Implement audio layers functionality
            // For example, you could open a dialog to select audio tracks
            MessageBox.Show("Audio Layers selection will be implemented here.", "Audio Layers", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if(VolumeAmountText != null)
                VolumeAmountText.Text = VolumeSlider.Value + "%";

            if(FileInfo != null)
                FileInfo.Volume = (int)VolumeSlider.Value;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+"); // Matches non-numeric characters
            e.Handled = regex.IsMatch(e.Text);
        }

        private void AudioLayerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FileInfo == null)
                return;
            FileInfo.AudioLayer = int.Parse(AudioLayerTextBox.Text);
        }
    }
}
