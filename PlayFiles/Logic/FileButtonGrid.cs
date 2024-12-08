using System;
using System.Windows;
using System.Windows.Controls;

namespace PlayFiles.Logic
{
    internal class FileButtonGrid : Grid
    {
        private readonly FileButton _fileButton;
        private readonly Button _closeButton;

        public FileButtonGrid(FileButton fileButton)
        {
            this._fileButton = fileButton;

            // Initialize Close Button
            _closeButton = new Button
            {
                Content = "X", // Close button label
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Attach close button click event
            _closeButton.Click += (object sender, RoutedEventArgs e) =>
            {
                // Remove the associated FileInfo from the collection and update UI
                FileInfo.FilesLoaded.Remove(_fileButton.fileInfo);
                MainWindow.Instance.UpdateButtons();
            };

            // Define the grid structure
            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto }); // Close button column
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) }); // File button column

            // Add buttons to the grid
            Children.Add(_closeButton);
            Children.Add(_fileButton);

            // Set grid positions
            Grid.SetColumn(_closeButton, 0);
            Grid.SetColumn(_fileButton, 1);
        }
    }
}
