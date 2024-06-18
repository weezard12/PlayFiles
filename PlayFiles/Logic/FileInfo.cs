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

        public string Path { get; set; }
        public string Name { get; set; }
        DateTime activeTime;
        public FileInfo(string path)
        {
            this.Path = path;
            Name = GetFileNameFromPath(path, true);
            activeTime = DateTime.Now.AddHours(1);
        }
        public static string GetFileNameFromPath(string path, bool withDot)
        {
            string r = String.Empty;
            int i = path.Length-1;
            if (!withDot)
            {
                while (path[i] != '.')
                    i--;
                i--;
            }
            while(i != 0 && path[i] != '\\')
            {
                r = path[i] + r;
                i--;
            }
            return r;
        }
    }
}
