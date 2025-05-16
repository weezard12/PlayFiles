using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static PlayFiles.Logic.VlcUtils;

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

        public int Volume { get; set; } = 100;
        public int AudioLayer { get; set; } = 1;

        public TimeSpan MediaDuration { get; set; } // in seconds

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

        public DateTime GetCloseTime()
        {
            if(CloseMediaAction.MediaType == CloseMediaType.OnFinished)
                return activeTime + MediaDuration;
            if(CloseMediaAction.MediaType == CloseMediaType.AfterTime)
                return activeTime + new TimeSpan(CloseMediaAction.Date.Hour, CloseMediaAction.Date.Minute, CloseMediaAction.Date.Second);
            if(CloseMediaAction.MediaType == CloseMediaType.OnDate)
                return CloseMediaAction.Date;

            if (CloseMediaAction.MediaType == CloseMediaType.Never)
                return DateTime.MaxValue;

            return activeTime;
        }
    }
}
