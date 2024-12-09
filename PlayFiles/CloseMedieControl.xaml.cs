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
    }

    
    public abstract class CloseMediaType
    {
        public abstract int SelectionIdx { get; protected set; }
    }
    public class CloseMediaOnFinished : CloseMediaType
    {
        public override int SelectionIdx { get; protected set; } = 1;
    }
    public class CloseMediaOnDate : CloseMediaType
    {
        public override int SelectionIdx { get; protected set; } = 1;
        DateTime CloseAt { get; set; }
    }
    public class CloseMediaAfterTime : CloseMediaType
    {
        public override int SelectionIdx { get; protected set; } = 1;
        DateTime CloseAfter { get; set; }
    }
}
