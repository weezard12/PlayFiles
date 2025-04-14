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
        private ResourceDictionary _lightTheme;
        private ResourceDictionary _darkTheme;
        private bool _isDarkMode = false;

        public static MainWindow Instance { get; private set; }
        private bool isRunning = true;
        private DispatcherTimer _timer;

        public static bool HideCursorWhenPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            StartTimer();
            Thread checkingThread = new Thread(CheckDateTime) { IsBackground = true };
            checkingThread.Start();


            LoadThemes();
        }

        private void LoadThemes()
        {
            try
            {
                // Load the light theme
                _lightTheme = new ResourceDictionary();
                _lightTheme.Source = new Uri("pack://application:,,,/PlayFiles;component/Resources/LightTheme.xaml", UriKind.Absolute);

                // Load the dark theme
                _darkTheme = new ResourceDictionary();
                _darkTheme.Source = new Uri("pack://application:,,,/PlayFiles;component/Resources/DarkTheme.xaml", UriKind.Absolute);

                // Apply the default theme (light)
                ApplyTheme(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading themes: {ex.Message}", "Theme Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            _isDarkMode = ThemeToggle.IsChecked ?? false;
            ApplyTheme(_isDarkMode);
        }

        private void ApplyTheme(bool isDarkMode)
        {
            try
            {
                ResourceDictionary currentTheme = isDarkMode ? _darkTheme : _lightTheme;

                // Clear existing theme resources and merge the new theme
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(currentTheme);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying theme: {ex.Message}", "Theme Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
        public void UploadFromYT(object sender, RoutedEventArgs e)
        {
            FileInfo fileInfo = new FileInfo("File From YouTube") {type = FileType.YT };
            ConfirmFile newWindow = new ConfirmFile(fileInfo, this);
            newWindow.ShowDialog();
        }
        public void UploadFromWEB(object sender, RoutedEventArgs e)
        {
            FileInfo fileInfo = new FileInfo("Web File") { type = FileType.WEB };
            ConfirmFile newWindow = new ConfirmFile(fileInfo, this);
            newWindow.ShowDialog();
        }
        public void UpdateButtons()
        {
            FilesLoadedPanel.Children.Clear();
            foreach (var button in FileInfo.FilesLoaded)
            {
                FilesLoadedPanel.Children.Add(new FileButtonGrid( new FileButton(button) { Margin = new Thickness(5) } ));
            }
        }

        private void CheckDateTime()
        {
            while (isRunning)
            {
                DateTime currentTime = DateTime.Now;
                foreach (var dateTime in FileInfo.FilesLoaded)
                {
                    if (currentTime.ToString("HH:mm:ss") == dateTime.activeTime.ToString("HH:mm:ss"))
                    {
                        // Update UI on the main thread
                        Dispatcher.Invoke(() =>
                        {
                            //dateTime.PlayFile();
                            MediaWindow mediaWindow = new MediaWindow(dateTime);
                            try
                            {
                                mediaWindow.Show();
                            }
                            catch { }
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

        private void KeepScreenOn_Checked(object sender, RoutedEventArgs e)
        {
            ScreenLogic.EnableAlwaysOnMode();
        }

        private void KeepScreenOn_Unchecked(object sender, RoutedEventArgs e)
        {
            ScreenLogic.DisableAlwaysOnMode();
        }

        private void HideCursor_Click(object sender, RoutedEventArgs e)
        {
            HideCursorWhenPlaying = !HideCursorWhenPlaying;
        }
        private void Quit_Clicked(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
