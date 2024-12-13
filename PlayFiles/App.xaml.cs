using Microsoft.Win32;
using PlayFiles.Logic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PlayFiles
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Code to run before application closes
            ScreenLogic.DisableAlwaysOnMode();
        }
    }
}
