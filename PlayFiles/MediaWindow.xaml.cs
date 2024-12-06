using Microsoft.Win32;
using PlayFiles.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        WebBrowser webBrowser;

        public MediaWindow(FileInfo file)
        {
            InitializeComponent();
            if(file.type == FileType.LOCAL)
            {
                MediaElement Media = new MediaElement();
                MainGrid.Children.Add(Media);
                Media.Source = new Uri(file.Path);
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
            else if(file.type == FileType.YT)
            {
                SetWebBrowserCompatibility();
                webBrowser = new WebBrowser();
                MainGrid.Children.Add(webBrowser);
                webBrowser.Navigate(GetEmbeddedLink(file.Path)+"?autoplay=1");
            }
            else if (file.type == FileType.WEB)
            {
                SetWebBrowserCompatibility();
                webBrowser = new WebBrowser();
                MainGrid.Children.Add(webBrowser);
                webBrowser.Navigate(file.Path);
            }

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
        private void Window_Closed(object sender, EventArgs e)
        {
            webBrowser?.Navigate("about:blank");
            Trace.WriteLine("AAA");
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Trace.WriteLine("AAA");

        }
    }
}

