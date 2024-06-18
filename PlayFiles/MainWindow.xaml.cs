﻿using Microsoft.Win32;
using PlayFiles.Logic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
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
    }
}
