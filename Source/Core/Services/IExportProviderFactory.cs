namespace Slithin.Core.Services
{
    public interface IExportProviderFactory
    {
        string[] GetAvailableExtensions();

        IExportProvider GetExportProvider(string extension);

        void Init();
    }
}
