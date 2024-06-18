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
        FileInfo fileInfo;
        public FileButton(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
            Content = fileInfo.Name;
        }
        protected override void OnClick()
        {
            base.OnClick();
            ConfirmFile newWindow = new ConfirmFile(fileInfo, MainWindow.Instance);
            newWindow.ShowDialog();

        }
    }
}
