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

namespace PlayFiles
{
    /// <summary>
    /// Interaction logic for CloseMedieControl.xaml
    /// </summary>
    public partial class CloseMedieControl : UserControl
    {
        public CloseMedieControl()
        {
            InitializeComponent();
        }


        private void CloseWhenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    public enum CloseMediaType
    {
        OnFinished = 0,
        OnDate = 1,
        AfterTime = 2,
        Never = 3
    }

    public class CloseMediaAction
    {
        public CloseMediaType MediaType { get; private set; }
        public DateTime? Date { get; set; } // For CloseMediaOnDate

        public CloseMediaAction(CloseMediaType mediaType)
        {
            MediaType = mediaType;
        }
    }
}
