using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlayFiles.Logic
{
    static class VlcUtils
    {
        public static string GetVlcExePath()
        {
            return Path.Combine(GetVlcFolderPath(),"vlc.exe");
        }
        public static string GetVlcFolderPath()
        {
            // Check if VLC is installed in the default location
            string vlcPath = @"C:\Program Files\VideoLAN\VLC";
            if (Directory.Exists(vlcPath))
            {
                return vlcPath;
            }
            // If not found, check the 32-bit Program Files folder
            vlcPath = @"C:\Program Files (x86)\VideoLAN\VLC";
            if (Directory.Exists(vlcPath))
            {
                return vlcPath;
            }
            // VLC not found
            return null;
        }

        public static TimeSpan GetMediaDuration(string mediaPath)
        {
            // Initialize LibVLC with the specified VLC installation path
            Core.Initialize();

            using var libVLC = new LibVLC();
            using var media = new Media(libVLC, new Uri(mediaPath));

            // Parse the media to retrieve metadata
            media.Parse();

            // Wait until parsing is complete
            while (media.ParsedStatus != MediaParsedStatus.Done)
            {
                Thread.Sleep(10);
            }

            // Retrieve the duration in milliseconds and convert to TimeSpan
            var durationInMilliseconds = media.Duration;
            return TimeSpan.FromMilliseconds(durationInMilliseconds);
        }

        public static async Task<TimeSpan> GetMediaDurationAsync(string mediaPath)
        {
            // Initialize LibVLC
            Core.Initialize();

            using var libVLC = new LibVLC();
            using var media = new Media(libVLC, new Uri(mediaPath));

            // Offload the parsing to a background thread
            await Task.Run(() =>
            {
                media.Parse();
                while (media.ParsedStatus != MediaParsedStatus.Done)
                {
                    Thread.Sleep(10);
                }
            });

            // Retrieve and return the duration
            var durationInMilliseconds = media.Duration;
            return TimeSpan.FromMilliseconds(durationInMilliseconds);
        }

    }
}
