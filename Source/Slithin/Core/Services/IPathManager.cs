namespace Slithin.Core.Services
{
    public interface IPathManager
    {
        public string BackupsDir { get; }
        public string ConfigBaseDir { get; }
        public string CustomScreensDir { get; }
        public string NotebooksDir { get; }
        public string ScriptsDir { get; }
        public string TemplatesDir { get; }

        void Init();
    }
}
