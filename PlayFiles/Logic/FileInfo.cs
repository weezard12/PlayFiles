using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFiles.Logic
{
    public enum FileType
    {
        LOCAL,
        MP4,
        MP3,
        YT,
        WEB
    }
    public class FileInfo
    {
        public static List<FileInfo> FilesLoaded { get; set; } = new List<FileInfo>();
        public FileType type = FileType.LOCAL;

        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime activeTime;

        public bool OpenAsFullscreen { get; set; } = true;
        public bool FocusMediaWhenPlayed { get; set; } = true;
        public bool OpenWithVLCMediaPlayer { get; set; } = false;

        public CloseMediaAction CloseMediaAction { get; set; }

        public FileInfo(string path, bool withDot = true)
        {
            this.Path = path;
            Name = GetFileNameFromPath(path, withDot);
            activeTime = DateTime.Now;
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
            while(i != -1 && path[i] != '\\')
            {
                r = path[i] + r;
                i--;
            }
            return r;
        }

        public void PlayFile()
        {
            Process.Start(new ProcessStartInfo(Path));
        }
    }
}
