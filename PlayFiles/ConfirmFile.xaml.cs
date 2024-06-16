using PlayFiles.Logic;
using System;
using System.Collections.Generic;
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
        public ConfirmFile(Logic.FileInfo fileInfo)
        {
            InitializeComponent();
        }
        public void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        public void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            Logic.FileInfo.filesLoaded.Add(FileInfo);
            Close();
        }
    }
}
