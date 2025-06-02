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
            await Task.Run(() => Core.Initialize());

            using var libVLC = new LibVLC();
            using var media = new Media(libVLC, new Uri(mediaPath));

            var tcs = new TaskCompletionSource<TimeSpan>();

            media.ParsedChanged += (sender, e) =>
            {
                if (e.ParsedStatus == MediaParsedStatus.Done)
                {
                    var duration = TimeSpan.FromMilliseconds(media.Duration);
                    tcs.TrySetResult(duration);
                }
                else if (e.ParsedStatus == MediaParsedStatus.Failed)
                {
                    tcs.TrySetResult(TimeSpan.Zero);
                }
            };

            // Start parsing
            media.Parse(MediaParseOptions.ParseNetwork);

            // Add timeout to prevent hanging
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            cts.Token.Register(() => tcs.TrySetResult(TimeSpan.Zero));

            return await tcs.Task;
        }

    }
}
