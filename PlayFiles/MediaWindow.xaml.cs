﻿using Microsoft.Win32;
using PlayFiles.Logic;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using FileInfo = PlayFiles.Logic.FileInfo;

namespace PlayFiles
{
    /// <summary>
    /// Interaction logic for MediaWindow.xaml
    /// </summary>
    public partial class MediaWindow : Window
    {
        // In case where the media is a web page or a YouTube video, we need to use the WebBrowser control
        private WebBrowser webBrowser;

        // In case the Play With VLC option is selected, we need save the VLC process
        private Process vlcProcess;


        public MediaWindow(FileInfo file)
        {
            InitializeComponent();

            if (file.OpenAsFullscreen)
            {
                // Make the window full-screen
                this.WindowState = WindowState.Maximized;
                this.WindowStyle = WindowStyle.None;
                this.ResizeMode = ResizeMode.NoResize;
            }

            if(MainWindow.HideCursorWhenPlaying)
                Cursor = Cursors.None;

            if (file.type == FileType.LOCAL)
            {
                MediaElement Media = new MediaElement();
                MainGrid.Children.Add(Media);
                Media.Source = new Uri(file.Path);
                Media.LoadedBehavior = MediaState.Manual;
                Media.UnloadedBehavior = MediaState.Close;


                switch (file.CloseMediaAction.MediaType)
                {
                    case CloseMediaType.OnFinished:
                        Media.MediaEnded += (sender, e) =>
                            this.Close(); // Close the window when the media ends
                        break;

                    case CloseMediaType.AfterTime:
                        // Calculate the interval in milliseconds
                        TimeSpan interval = new TimeSpan(file.CloseMediaAction.Date.Hour, file.CloseMediaAction.Date.Minute, file.CloseMediaAction.Date.Second);

                        if (interval.TotalMilliseconds <= 0)
                            return;


                        // Initialize DispatcherTimer
                        DispatcherTimer _timer = new DispatcherTimer
                        {
                            Interval = interval
                        };

                        // Event fired when the timer interval elapses
                        _timer.Tick += (object sender, EventArgs e) =>
                        {
                            Close();
                            vlcProcess?.Kill(); // Kill the VLC process if it was started
                            _timer.Stop();
                        };
                        _timer.Start();
                        break;
                    case CloseMediaType.OnDate:
                        // Calculate the time until the specified date
                        TimeSpan timeUntilClose = file.CloseMediaAction.Date - DateTime.Now;

                        if (timeUntilClose.TotalMilliseconds > 0)
                        {
                            // Use DispatcherTimer to check the date periodically
                            var timer = new System.Windows.Threading.DispatcherTimer();
                            timer.Interval = TimeSpan.FromMilliseconds(500); // Check every 500ms
                            timer.Tick += (sender, e) =>
                            {
                                if (DateTime.Now >= file.CloseMediaAction.Date)
                                {
                                    timer.Stop(); // Stop the timer
                                    vlcProcess?.Kill(); // Kill the VLC process if it was started
                                    this.Close(); // Close the window
                                }
                            };
                            timer.Start();
                        }
                        break;

                }


                if (file.OpenWithVLCMediaPlayer)
                {
                    OpenMediaInVLC(file.Path);
                    Hide();
                    return;
                }

                // Sets the window focus
                if (file.FocusMediaWhenPlayed)
                {
                    Topmost = true;
                    Focus();
                }


                Media.Play();

                    // Close window on Escape key press
                    KeyDown += (object sender, KeyEventArgs e) =>
                    {
                        if (e.Key == Key.Escape)
                        {
                            this.Close();
                        }
                    };
            }
            else if (file.type == FileType.YT)
            {
                SetWebBrowserCompatibility();
                webBrowser = new WebBrowser();
                MainGrid.Children.Add(webBrowser);
                webBrowser.Navigate(GetEmbeddedLink(file.Path) + "?autoplay=1");
            }
            else if (file.type == FileType.WEB)
            {
                SetWebBrowserCompatibility();
                webBrowser = new WebBrowser();
                MainGrid.Children.Add(webBrowser);
                webBrowser.Navigate(file.Path);
            }

            // Clean up when the window is closed
            Closed += (object sender, EventArgs e) =>
            {
                webBrowser?.Navigate("about:blank");
                Trace.WriteLine("AAA");
            };
            
        }

        private void SetWebBrowserCompatibility()
        {
            try
            {
                string appName = System.IO.Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"))
                {
                    key.SetValue(appName + ".exe", 11001, RegistryValueKind.DWord);
                    key.SetValue(appName + ".vshost.exe", 11001, RegistryValueKind.DWord);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to set browser compatibility: " + ex.Message);
            }
        }

        static string GetEmbeddedLink(string youtubeLink)
        {
            // Extract the video ID from the YouTube link
            Uri uri = new Uri(youtubeLink);
            string videoId = System.Web.HttpUtility.ParseQueryString(uri.Query)["v"];

            // Construct the embedded link
            string embeddedLink = $"https://www.youtube.com/embed/{videoId}";
            return embeddedLink;
        }

        private void OpenMediaInVLC(string mediaPath)
        {
            string vlcPath = @"C:\Program Files\VideoLAN\VLC\vlc.exe";
            if (!File.Exists(vlcPath))
                vlcPath = @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe";


            try
            {
                // Command-line options for VLC
                string arguments = $"\"{mediaPath}\" --fullscreen --video-on-top --play-and-exit";

                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = vlcPath,
                    Arguments = arguments, // Add arguments for fullscreen and focus
                    UseShellExecute = false
                };

                vlcProcess = Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                
            }
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            webBrowser?.Navigate("about:blank");
            Trace.WriteLine("Closed Window");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Trace.WriteLine("Closing Window");
        }
    }
}
