namespace Slithin.Core.Services
{
    public interface IImportProviderFactory
    {
        IImportProvider GetImportProvider(string baseExtension, string filename);

        void Init();
    }
}
