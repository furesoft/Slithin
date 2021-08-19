using System;
using System.Reflection;
using System.Threading.Tasks;
using Octokit;
using Slithin.Core.Scripting;

namespace Slithin.Core
{
    public static class Updater
    {
        public static async Task StartUpdate()
        {
            var client = new GitHubClient(new ProductHeaderValue("SomeName"));
            var releases = await client.Repository.Release.GetAll("furesoft", "Slithin");

            var latestGitHubVersion = new Version(releases[0].TagName);
            var localVersion = Assembly.GetExecutingAssembly().GetName().Version;

            //Compare the Versions
            var versionComparison = localVersion.CompareTo(latestGitHubVersion);
            if (versionComparison < 0)
            {
                Utils.OpenUrl("https://github.com/furesoft/Slithin/releases");
            }
        }
    }
}
