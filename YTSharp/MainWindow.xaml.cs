using System.Threading;
using System;
using System.Windows;
using System.Windows.Controls;
using YoutubeDLSharp;
using System.Net;
using System.IO.Compression;
using System.IO;
using System.Windows.Shapes;

namespace YTSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public YoutubeDL ytdl { get; }

        public MainWindow()
        {
            ytdl = new YoutubeDL();
            ytdl.YoutubeDLPath = "C:\\Users\\Stot_\\Downloads\\yt-dlp.exe";
            ytdl.FFmpegPath = "C:\\Users\\Stot_\\Downloads\\ffmpeg.exe";
            ytdl.OutputFolder = "Videos\\YT_Downloads\\";
            InitializeComponent();
        }

        async private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<DownloadProgress>(p => ProgBar.Value = p.Progress);
            var cts = new CancellationTokenSource();
            await ytdl.RunVideoDownload(UrlBox.Text, progress: progress, ct: cts.Token);
        }

        private void DependButton_Click(object sender, RoutedEventArgs e)
        {
            using (WebClient wc = new WebClient())
            {
                Console.WriteLine("in the body");
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFile(
                    // Param1 = Link of file
                    new System.Uri("https://github.com/yt-dlp/yt-dlp/releases/download/2023.03.04/yt-dlp.exe"),
                    // Param2 = Path to save
                    "bin\\yt-dlp.exe"
                );
                wc.DownloadFile(
                    // Param1 = Link of file
                    new System.Uri("https://github.com/BtbN/FFmpeg-Builds/releases/download/latest/ffmpeg-master-latest-win64-gpl.zip"),
                    // Param2 = Path to save
                    "ffmpeg-master-latest-win64-gpl.zip"
                );
            }

            string zipPath = "ffmpeg-master-latest-win64-gpl.zip";
            string extractPath = "ffmpeg";

            ZipFile.ExtractToDirectory(zipPath, extractPath);

            File.Move("ffmpeg\\ffmpeg-master-latest-win64-gpl\\bin\\ffmpeg.exe", "bin\\ffmpeg.exe");

            Directory.Delete("ffmpeg", true);

            File.Delete(zipPath);

            //var ffmpegZip = ZipFile.OpenRead("ffmpeg-master-latest-win64-gpl.zip");
            //var entry = ffmpegZip.GetEntry("ffmpeg-master-latest-win64-gpl\\bin\\ffmpeg.exe");
            //entry.ExtractToFile("ffmpeg.exe", true);

            ytdl.YoutubeDLPath = "yt-dlp.exe";
            ytdl.FFmpegPath = "ffmpeg.exe";
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgBar.Value = e.ProgressPercentage;
        }
    }
}
