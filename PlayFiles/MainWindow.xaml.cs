using Microsoft.Win32;
using PlayFiles.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PlayFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        private bool isRunning = true;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            StartTimer();
            Thread checkingThread = new Thread(CheckDateTime) { IsBackground = true };
            checkingThread.Start();
            
        }
        public void Upload(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? response = openFileDialog.ShowDialog();

            if (response == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                ConfirmFile newWindow = new ConfirmFile(fileInfo,this);
                newWindow.ShowDialog();
            }

        }
        public void UpdateButtons()
        {
            FilesLoadedPanel.Children.Clear();
            foreach (var button in FileInfo.filesLoaded)
            {
                FilesLoadedPanel.Children.Add(new FileButton(button) { Margin = new Thickness(5) });
            }
        }

        private void CheckDateTime()
        {
            while (isRunning)
            {
                DateTime currentTime = DateTime.Now;
                foreach (var dateTime in FileInfo.filesLoaded)
                {
                    if (currentTime.ToString("yyyy-MM-dd HH:mm:ss") == dateTime.activeTime.ToString("yyyy-MM-dd HH:mm:ss"))
                    {
                        // Update UI on the main thread
                        Dispatcher.Invoke(() =>
                        {
                            //dateTime.PlayFile();
                            new MediaWindow(dateTime.Path).Show();
                        });
                    }
                }
                Thread.Sleep(1000); // Check every second
            }
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime.Text = "Current Time: "+DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
