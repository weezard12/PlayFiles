using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFiles.Logic
{
    public class FileInfo
    {
        public static List<FileInfo> filesLoaded = new List<FileInfo>();

        string path;
        string name;
        DateTime activeTime;
        public FileInfo(string path)
        {
            this.path = path;
            activeTime = DateTime.Now.AddHours(1);
        }
    }
}
