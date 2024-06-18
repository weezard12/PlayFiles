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

namespace PlayFiles
{
    /// <summary>
    /// Interaction logic for MediaWindow.xaml
    /// </summary>
    public partial class MediaWindow : Window
    {
        public MediaWindow(string path)
        {
            InitializeComponent();
            Media.Source = new Uri(path);
            Media.LoadedBehavior = MediaState.Manual;
            Media.Play();
            KeyDown += (object sender, KeyEventArgs e) =>
            {
                if (e.Key == Key.Escape)
                {
                    this.Close();
                }
            };
        }
    }
}
