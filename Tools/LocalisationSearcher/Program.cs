using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace LocalisationSearcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var directoryToSearch = args[0];
            var stringsPath = GetStringsPath(directoryToSearch);
            JObject resultObj = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(stringsPath));

            var csPattern = "(GetString|GetStringFormat|WithLocalizedMessage)\\(\"(?<key>[^\"]+)\"\\)";
            var axamlPattern = @"Localisation '(?<key>[^}]+)'\}";

            var regxp = new Regex($"{csPattern}|{axamlPattern}");
            var csFiles = Directory.GetFiles(directoryToSearch, "*.cs", SearchOption.AllDirectories);
            var axamlFiles = Directory.GetFiles(directoryToSearch, "*.axaml", SearchOption.AllDirectories);

            var files = csFiles.Concat(axamlFiles);

            foreach (var file in files)
            {
                var matches = regxp.Matches(File.ReadAllText(file));

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        var key = match.Groups["key"].Value;
                        resultObj.TryAdd(key, key);
                    }
                }
            }

            var json = JsonConvert.SerializeObject(resultObj, Formatting.Indented);

            File.WriteAllText(stringsPath, json);
        }

        private static string GetStringsPath(string directoryToSearch)
        {
            return Directory.GetFiles(directoryToSearch, "strings.json", SearchOption.AllDirectories).First();
        }
    }
}