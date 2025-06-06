﻿using PlayFiles.Logic;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace PlayFiles
{
    /// <summary>
    /// Interaction logic for CloseMedieControl.xaml
    /// </summary>
    public partial class CloseMedieControl : UserControl
    {
        public CloseMediaAction CloseMediaAction { get; private set; }
        public CloseMedieControl()
        {
            InitializeComponent();
        }

        public void SetFileOfThatAction(FileInfo fileInfo)
        {
            CloseMediaAction = fileInfo.CloseMediaAction;

            if (CloseMediaAction == null)
            {
                ResetMediaSelection();
                CloseMediaAction = new CloseMediaAction(CloseMediaType.OnFinished);
            }

            SetupComboBox(SecondsComboBox, 60);
            SetupComboBox(MinutesComboBox, 60);
            SetupComboBox(HoursComboBox, 24);

            HoursComboBox.SelectedIndex = CloseMediaAction.Date.Hour;
            MinutesComboBox.SelectedIndex = CloseMediaAction.Date.Minute;
            SecondsComboBox.SelectedIndex = CloseMediaAction.Date.Second;
            CloseWhenComboBox.SelectedIndex = (int)CloseMediaAction.MediaType;
            if(CloseMediaAction.MediaType == CloseMediaType.Never || CloseMediaAction.MediaType == CloseMediaType.OnFinished)
                ResetMediaSelection();
        }

        public void UpdateCloseMediaAction()
        {
            CloseMediaAction.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, HoursComboBox.SelectedIndex, MinutesComboBox.SelectedIndex, SecondsComboBox.SelectedIndex);
        }

        private void SetupComboBox(ComboBox comboBox, int maxValue)
        {
            comboBox.Items.Clear();

            // Make sure the style is applied before adding items
            comboBox.Style = (Style)FindResource("ModernComboBox");

            for (int i = 0; i < maxValue; i++)
            {
                string content = i > 9 ? i.ToString() : "0" + i.ToString();

                // Create a simple ComboBoxItem
                ComboBoxItem item = new ComboBoxItem
                {
                    Content = content,
                    Style = (Style)FindResource("ModernComboBoxItem")
                };

                comboBox.Items.Add(item);
            }
        }

        private void ResetMediaSelection()
        {
            TimeSelectionGrid.Visibility = Visibility.Collapsed;
            //HoursComboBox.SelectedIndex = 0;
            //MinutesComboBox.SelectedIndex = 0;
            //SecondsComboBox.SelectedIndex = 0;
        }
        private void CloseWhenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CloseWhenComboBox == null || TimeSelectionGrid == null)
                return;
            if(CloseWhenComboBox.SelectedIndex == 3 || CloseWhenComboBox.SelectedIndex == 0)
                ResetMediaSelection();
            else
            {
                TimeSelectionGrid.Visibility = Visibility.Visible;
            }
            CloseMediaAction = new CloseMediaAction((CloseMediaType)CloseWhenComboBox.SelectedIndex);
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
        public DateTime Date { get; set; } // For CloseMediaOnDate

        public CloseMediaAction(CloseMediaType mediaType)
        {
            MediaType = mediaType;
            if (mediaType == CloseMediaType.OnFinished || mediaType == CloseMediaType.Never)
                return;
            this.Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
        }
    }
}
