using System.Threading;
using System;
using System.Windows;
using System.Windows.Controls;
using YoutubeDLSharp;

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
            ytdl.OutputFolder = "C:\\Users\\Stot_\\Downloads\\YT_Downloads\\";
            InitializeComponent();
        }

        /* TODO: Ability to dowload external binaries.
         * 
        async private void Init_Dlp_FfMpeg()
        {
            try
            {
                this.ytdl.YoutubeDLPath = await YoutubeDLSharp.Utils.DownloadYtDlp();
                this.ytdl.FFmpegPath = await YoutubeDLSharp.Utils.DownloadFFmpeg();
            }
        }
        */

        async private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            var progress = new Progress<DownloadProgress>(p => ProgBar.Value = p.Progress);
            var cts = new CancellationTokenSource();
            await ytdl.RunVideoDownload(UrlBox.Text, progress: progress, ct: cts.Token);
        }
    }
}
