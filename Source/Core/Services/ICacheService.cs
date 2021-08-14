namespace Slithin.Core.Services
{
    public interface ICacheService
    {
        void Add<T>(string name, T obj);

        public T Get<T>(string name, T obj = default);
    }
}
