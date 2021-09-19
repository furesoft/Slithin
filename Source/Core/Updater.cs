using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using Ionic.Zip;
using Octokit;

namespace Slithin.Core
{
    public static class Updater
    {
        public static string GetReleaseFilename()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return $"Windows_x{(Environment.Is64BitOperatingSystem ? 64 : 86)}.zip";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return $"Linux_x64.zip";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return $"OSX_x64.zip";
            }

            return "";
        }

        public static async void StartUpdate()
        {
            var client = new GitHubClient(new ProductHeaderValue("SomeName"));
            var releases = await client.Repository.Release.GetAll("furesoft", "Slithin");

            var latestGitHubVersion = new Version(releases[0].TagName);
            var localVersion = Assembly.GetExecutingAssembly().GetName().Version;

            //Compare the Versions
            var versionComparison = localVersion.CompareTo(latestGitHubVersion);

            if (versionComparison >= 0)
                return;

            var asset = GetAsset(releases[0]);
            using var wc = new WebClient();
            
            var tmp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var fileName = Path.Combine(tmp, Path.GetFileName(asset.BrowserDownloadUrl));

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            if (File.Exists(fileName))
                return;

            wc.DownloadProgressChanged += (s, e) =>
            {
                NotificationService.Show($"Downloading {asset.Name} ({e.ProgressPercentage} %)");
            };

            wc.DownloadFileAsync(new Uri(asset.BrowserDownloadUrl), fileName);
            wc.DownloadFileCompleted += (s, e) =>
            {
                NotificationService.Show("Extracting Update");
                using (var zip = new ZipFile(fileName))
                {
                    zip.ExtractProgress += (zs, ze) =>
                    {
                        if (ze.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
                        {
                            NotificationService.Show($"Extracting {Path.GetFileName(ze.ArchiveName)} ({Math.Round((float)ze.EntriesExtracted / (float)ze.EntriesTotal * 100)} %)");
                        }
                    };

                    zip.ExtractAll(Path.Combine(tmp, "SlithinUpdate"), ExtractExistingFileAction.OverwriteSilently);
                }

                File.Delete(fileName);

                NotificationService.Hide();

                UpdateScriptGenerator.ApplyUpdate(Path.Combine(tmp, "SlithinUpdate"), Environment.CurrentDirectory);

                Environment.Exit(0);
            };
        }

        private static ReleaseAsset GetAsset(Release release)
        {
            foreach (var asset in release.Assets)
            {
                if (asset.Name == GetReleaseFilename())
                {
                    return asset;
                }
            }

            return null;
        }
    }
}
