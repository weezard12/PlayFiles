using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PlayFiles.Logic
{
    internal class FileButtonGrid : Grid
    {
        private readonly FileButton _fileButton;
        private readonly Button _closeButton;

        public FileButtonGrid(FileButton fileButton)
        {
            this._fileButton = fileButton;

            // Create a red circle with X icon
            Grid closeIconGrid = CreateCloseIcon();

            // Create a custom template for the button to remove default hover effects
            ControlTemplate buttonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            buttonTemplate.VisualTree = contentPresenter;

            // Initialize Close Button with custom icon
            _closeButton = new Button
            {
                Content = closeIconGrid,
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Background = null,
                BorderBrush = null,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(2),
                Template = buttonTemplate,
                RenderTransformOrigin = new Point(0.5, 0.5)
            };

            // Add ScaleTransform to the button
            _closeButton.RenderTransform = new ScaleTransform(1.0, 1.0);

            // Add mouse enter event to scale up
            _closeButton.MouseEnter += (sender, e) =>
            {
                ScaleButton(_closeButton, 1.0, 1.2, 100);
            };

            // Add mouse leave event to scale back
            _closeButton.MouseLeave += (sender, e) =>
            {
                ScaleButton(_closeButton, 1.2, 1.0, 100);
            };

            // Add click handler
            _closeButton.Click += (object sender, RoutedEventArgs e) =>
            {
                FileInfo.FilesLoaded.Remove(_fileButton.fileInfo);
                MainWindow.Instance.UpdateButtons();
            };

            ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            Children.Add(_closeButton);
            Children.Add(_fileButton);

            Grid.SetColumn(_closeButton, 0);
            Grid.SetColumn(_fileButton, 1);
        }

        // Create a red circle with X icon
        private Grid CreateCloseIcon()
        {
            Grid iconGrid = new Grid
            {
                Width = 22,
                Height = 22
            };

            // Create red circle
            Ellipse circle = new Ellipse
            {
                Fill = new SolidColorBrush(Colors.Red),
                Width = 22,
                Height = 22
            };

            // Create X lines
            Line line1 = new Line
            {
                X1 = 6,
                Y1 = 6,
                X2 = 16,
                Y2 = 16,
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(Colors.White)
            };

            Line line2 = new Line
            {
                X1 = 16,
                Y1 = 6,
                X2 = 6,
                Y2 = 16,
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(Colors.White)
            };

            // Add elements to grid
            iconGrid.Children.Add(circle);
            iconGrid.Children.Add(line1);
            iconGrid.Children.Add(line2);

            return iconGrid;
        }

        // Helper method to create and start a scale animation
        private void ScaleButton(Button button, double fromScale, double toScale, int durationMs)
        {
            ScaleTransform scaleTransform = (ScaleTransform)button.RenderTransform;

            DoubleAnimation scaleXAnimation = new DoubleAnimation
            {
                From = fromScale,
                To = toScale,
                Duration = TimeSpan.FromMilliseconds(durationMs)
            };

            DoubleAnimation scaleYAnimation = new DoubleAnimation
            {
                From = fromScale,
                To = toScale,
                Duration = TimeSpan.FromMilliseconds(durationMs)
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
        }
    }
}