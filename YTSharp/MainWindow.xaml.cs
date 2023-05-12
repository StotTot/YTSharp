using System.Threading;
using System;
using System.Windows;
using System.Windows.Controls;
using YoutubeDLSharp;
using System.Net;
using System.IO.Compression;
using System.IO;
using System.Windows.Shapes;
using System.Threading.Tasks;
using YoutubeDLSharp.Metadata;
using System.Security.Policy;
using System.Diagnostics;

namespace YTSharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public YoutubeDL ytdl { get; }

        private IProgress<DownloadProgress> prog;

        public MainWindow()
        {
            ytdl = new YoutubeDL();
            ytdl.YoutubeDLPath = "C:\\Users\\Stot_\\Downloads\\yt-dlp.exe";
            ytdl.FFmpegPath = "C:\\Users\\Stot_\\Downloads\\ffmpeg.exe";
            ytdl.OutputFolder = "YT_Downloads\\";
            prog = new Progress<DownloadProgress>((p) => showProg(p));
            InitializeComponent();
        }

        private void showProg(DownloadProgress p)
        {
            ProgBar.Value = p.Progress * 100;
            TextProg.Text = $"speed: {p.DownloadSpeed} | ETA: {p.ETA}";
        }
        async private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var cts = new CancellationTokenSource();
            //RunResult<VideoData> mtdta = await ytdl.RunVideoDataFetch(UrlBox.Text);
            //if (mtdta.Success)
            //{
            //    Console.WriteLine(mtdta.Data);
            //}
            var res = await ytdl.RunVideoDownload(UrlBox.Text, progress: prog, ct: cts.Token);
            if (res.Success)
            {
                MessageBox.Show("Successfully downloaded " + UrlBox.Text, "YTSharp");
            }
        }
        private void DependButton_Click(object sender, RoutedEventArgs e)
        {
            using (WebClient wc = new WebClient())
            {
                Console.WriteLine("in the body");
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

            MessageBox.Show("Successfully downloaded dependencies", "YTSharp");
        }
        // TODO: Still need a quality selector.
        //private async void FindButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var mtdta = await ytdl.RunVideoDataFetch(UrlBox.Text);
        //    VideoData video = mtdta.Data;
        //    FormatData[] formats = video.Formats;
        //    for (int i  = 0; i < formats.Length; i++)
        //    {
        //        Console.WriteLine(formats[i].Quality.ToString);
        //    }
        //}

        async private void DownloadAudioButton_Click(object sender, RoutedEventArgs e)
        {
            var cts = new CancellationTokenSource();
            //RunResult<VideoData> mtdta = await ytdl.RunVideoDataFetch(UrlBox.Text);
            //if (mtdta.Success)
            //{
            //    Console.WriteLine(mtdta.Data);
            //}
            var res = await ytdl.RunAudioDownload(UrlBox.Text, progress: prog, ct: cts.Token);
            if (res.Success)
            {
                MessageBox.Show("Successfully downloaded " + UrlBox.Text, "YTSharp");
            }
        }
    }
}
