namespace Slithin.Modules.BaseServices.Models;

public interface IEventService
{
    void Invoke<T>(string name, T argument);

    void Subscribe<T>(string name, Action<T> action);
}
