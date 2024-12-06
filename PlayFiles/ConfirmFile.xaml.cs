using PlayFiles.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            this.FileInfo = fileInfo;
            this.window = window;

            FilePath.Text = fileInfo.Path;
            FileName.Text = fileInfo.Name;
            HoursComboBox.SelectedIndex = fileInfo.activeTime.Hour;
            MinutesComboBox.SelectedIndex = fileInfo.activeTime.Minute;
            SecondsComboBox.SelectedIndex = fileInfo.activeTime.Second;
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

            if(!Logic.FileInfo.filesLoaded.Contains(FileInfo))
                Logic.FileInfo.filesLoaded.Add(FileInfo);
            window.UpdateButtons();
            Close();
        }
    }
}
