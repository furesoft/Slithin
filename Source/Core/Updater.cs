using System;
using System.Linq;
using System.Threading.Tasks;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.Events;
using NetSparkleUpdater.SignatureVerifiers;
using Slithin.Controls;

namespace Slithin.Core
{
    public static class Updater
    {
        public static SparkleUpdater _netSparkle = new("http://furesoft.ml/app_updates/Slithin/appcast.xml", new DSAChecker(SecurityMode.UseIfPossible));

        private static UpdateInfo _updateInfo;

        public static async Task StartUpdate()
        {
            _updateInfo = await _netSparkle.CheckForUpdatesQuietly();

            if (_updateInfo.Status == UpdateStatus.UpdateAvailable)
            {
                _netSparkle.DownloadFinished -= Sparkle_FinishedDownloading;
                _netSparkle.DownloadFinished += Sparkle_FinishedDownloading;

                _netSparkle.DownloadHadError -= Sparkle_DownloadError;
                _netSparkle.DownloadHadError += Sparkle_DownloadError;

                _netSparkle.DownloadMadeProgress += Sparkle_DownloadMadeProgress;

                await _netSparkle.InitAndBeginDownload(_updateInfo.Updates.First());
            }
            else
            {
                NotificationService.Hide();
            }
        }

        private static void Sparkle_CloseApplication()
        {
            Environment.Exit(0);
        }

        private static void Sparkle_DownloadError(AppCastItem item, string path, Exception exception)
        {
            NotificationService.Show("We had an error during the download process -- " + exception.Message);
        }

        private static void Sparkle_DownloadMadeProgress(object sender, AppCastItem item, ItemDownloadProgressEventArgs e)
        {
            NotificationService.Show($"Downloading Update {e.ProgressPercentage} %");
        }

        private static async void Sparkle_FinishedDownloading(AppCastItem item, string path)
        {
            var ok = await DialogService.ShowDialog("The Update downloaded. Would you install it?");

            if (ok)
            {
                _netSparkle.CloseApplication += Sparkle_CloseApplication;
                _netSparkle.InstallUpdate(_updateInfo.Updates.First());
            }
        }
    }
}
