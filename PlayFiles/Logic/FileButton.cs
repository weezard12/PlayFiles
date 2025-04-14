using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PlayFiles.Logic
{
    internal class FileButton : Button
    {
        public FileInfo fileInfo { get; private set; }
        public FileButton(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
            Content = String.Format("{0} -> {1}", fileInfo.activeTime.ToString("HH:mm:ss"), fileInfo.Name);
        }
        protected override void OnClick()
        {
            base.OnClick();
            ConfirmFile newWindow = new ConfirmFile(fileInfo, MainWindow.Instance);
            newWindow.ShowDialog();
        }
    }
}
