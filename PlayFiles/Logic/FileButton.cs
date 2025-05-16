using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlayFiles.Logic
{
    internal class FileButton : Button
    {
        public FileInfo fileInfo { get; private set; }

        public FileButton(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
            Content = String.Format("{0} -> {1}  |  {2}", fileInfo.activeTime.ToString("HH:mm:ss"), fileInfo.GetCloseTime().ToString("HH:mm:ss"), fileInfo.Name);

            // Set base properties
            Padding = new Thickness(12, 0, 8, 0);
            Margin = new Thickness(0, 0, 5, 0);
            HorizontalContentAlignment = HorizontalAlignment.Left;
            FontSize = 13;
            BorderThickness = new Thickness(0);

            // Apply the themed template
            ApplyThemedTemplate();

            // Subscribe to theme changed event if available
            if (MainWindow.Instance != null && MainWindow.Instance.ThemeChanged != null)
            {
                MainWindow.Instance.ThemeChanged += ApplyThemedTemplate;
            }
        }

        private void ApplyThemedTemplate()
        {
            // Create a template with rounded corners that supports theming
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));

            // Set the border properties
            borderFactory.Name = "border";
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(6));
            borderFactory.SetValue(Border.BorderThicknessProperty, new Thickness(1));

            // Theme-specific bindings
            borderFactory.SetValue(Border.BackgroundProperty, new DynamicResourceExtension("PanelBackgroundBrush"));
            borderFactory.SetValue(Border.BorderBrushProperty, new DynamicResourceExtension("BorderBrush"));

            // Add the content presenter
            FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenter.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            contentPresenter.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            contentPresenter.SetValue(ContentPresenter.MarginProperty, new Thickness(5));
            borderFactory.AppendChild(contentPresenter);

            // Set the visual tree for the template
            template.VisualTree = borderFactory;

            // Add template triggers for mouse interactions
            Trigger mouseOverTrigger = new Trigger { Property = IsMouseOverProperty, Value = true };
            mouseOverTrigger.Setters.Add(new Setter(Border.BackgroundProperty, new DynamicResourceExtension("BorderBrush"), "border"));
            mouseOverTrigger.Setters.Add(new Setter(ForegroundProperty, new DynamicResourceExtension("BackgroundBrush")));
            mouseOverTrigger.Setters.Add(new Setter(CursorProperty, Cursors.Hand));
            template.Triggers.Add(mouseOverTrigger);

            // Set the template to the button
            Template = template;

            // Set foreground to follow theme
            SetResourceReference(ForegroundProperty, "ForegroundBrush");
        }

        protected override void OnClick()
        {
            base.OnClick();
            ConfirmFile newWindow = new ConfirmFile(fileInfo, MainWindow.Instance);
            newWindow.ShowDialog();
        }
        
    }
}