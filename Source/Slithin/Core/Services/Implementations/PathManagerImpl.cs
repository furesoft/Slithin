using System;
using System.IO;

namespace Slithin.Core.Services.Implementations
{
    public class PathManagerImpl : IPathManager
    {
        public string BackupsDir => Path.Combine(ConfigBaseDir, "Backups");
        public string ConfigBaseDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Slithin");

        public string CustomScreensDir => Path.Combine(ConfigBaseDir, "Screens");

        public string NotebooksDir => Path.Combine(ConfigBaseDir, "Notebooks");

        public string ScriptsDir => Path.Combine(ConfigBaseDir, "Scripts");

        public string TemplatesDir => Path.Combine(ConfigBaseDir, "Templates");

        public void Init()
        {
            if (!Directory.Exists(ConfigBaseDir))
            {
                Directory.CreateDirectory(ConfigBaseDir);
                Directory.CreateDirectory(TemplatesDir);
                Directory.CreateDirectory(NotebooksDir);
                Directory.CreateDirectory(ScriptsDir);
                Directory.CreateDirectory(CustomScreensDir);
                Directory.CreateDirectory(BackupsDir);

                File.WriteAllText(Path.Combine(ConfigBaseDir, "templates.json"), "{\"templates\": []}");
            }

            InitDir(TemplatesDir);
            InitDir(NotebooksDir);
            InitDir(ScriptsDir);
            InitDir(BackupsDir);
            InitDir(CustomScreensDir);
        }

        private void InitDir(string dir)
        {
            var di = new DirectoryInfo(dir);

            if (!di.Exists)
            {
                di.Create();
            }
        }
    }
}
