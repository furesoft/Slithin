using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using AuroraModularis.Core;
using Ionic.Zip;
using Octokit;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.UI.Models;
using Slithin.Modules.Updater.Models;
using Container = AuroraModularis.Core.Container;

namespace Slithin.Modules.Updater;

internal class UpdaterImplementation : IUpdaterService
{
    public async Task<bool> CheckForUpdate()
    {
        var client = new GitHubClient(new ProductHeaderValue("SomeName"));
        var releases = await client.Repository.Release.GetAll("furesoft", "Slithin");

        var latestGitHubVersion = new Version(releases.First(_ => !_.Prerelease).TagName);
        var localVersion = Assembly.GetExecutingAssembly().GetName().Version;

        //Compare the Versions
        var versionComparison = localVersion.CompareTo(latestGitHubVersion);

        return versionComparison <= 0;
    }

    public async Task StartUpdate()
    {
        var localisationService = Container.Current.Resolve<ILocalisationService>();
        var notificationService = Container.Current.Resolve<INotificationService>();

        var client = new GitHubClient(new ProductHeaderValue("SomeName"));
        var releases = await client.Repository.Release.GetAll("furesoft", "Slithin");

        var release = releases[0];
        var latestGitHubVersion = new Version(release.TagName);
        var localVersion = Assembly.GetExecutingAssembly().GetName().Version;

        //Compare the Versions
        var versionComparison = localVersion.CompareTo(latestGitHubVersion);

        if (versionComparison >= 0)
        {
            return;
        }

        var status = notificationService.ShowStatus("", true);

        var asset = GetAsset(release);

        var wc = new WebClient();

        var tmp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        var fileName = Path.Combine(tmp, Path.GetFileName(asset?.BrowserDownloadUrl));

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        if (File.Exists(fileName))
        {
            return;
        }

        wc.DownloadProgressChanged += (s, e) =>
        {
            status.Step(localisationService.GetStringFormat(
                "Downloading {0} ({1} %)",
                "Update",
                e.ProgressPercentage));
        };

        wc.DownloadFileAsync(new Uri(asset.BrowserDownloadUrl), fileName);

        void OnWcOnDownloadFileCompleted(object s, AsyncCompletedEventArgs e)
        {
            status.Step(localisationService.GetString("Extracting Update"));
            using (var zip = new ZipFile(fileName))
            {
                void OnZipOnExtractProgress(object zs, ExtractProgressEventArgs ze)
                {
                    if (ze.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
                    {
                        status.Step(localisationService.GetStringFormat("Extracting {0} ({1} %)", Path.GetFileName(ze.ArchiveName), Math.Round(ze.EntriesExtracted / (float)ze.EntriesTotal * 100)));
                    }
                }

                zip.ExtractProgress += OnZipOnExtractProgress;

                zip.ExtractAll(Path.Combine(tmp, "SlithinUpdate"), ExtractExistingFileAction.OverwriteSilently);
            }

            File.Delete(fileName);

            UpdateScriptGenerator.ApplyUpdate(Path.Combine(tmp, "SlithinUpdate"), Environment.CurrentDirectory);

            Environment.Exit(0);
        }

        wc.DownloadFileCompleted += OnWcOnDownloadFileCompleted;
    }

    private static string GetReleaseFilename()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return $"win-x{(Environment.Is64BitOperatingSystem ? 64 : 86)}.zip";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "linux_x64.zip";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "osx_x64.zip";
        }

        return "";
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

    private static void SaveChangelog(Release releases)
    {
        var content = releases.Body;

        var pathManager = Container.Current.Resolve<IPathManager>();

        File.WriteAllText(Path.Combine(pathManager.ConfigBaseDir, "changelog.txt"), content);
    }
}
