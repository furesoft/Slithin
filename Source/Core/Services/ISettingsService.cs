namespace Slithin.Core.Services
{
    public interface ISettingsService
    {
        Settings Get();

        void Save(Settings settings);
    }
}
