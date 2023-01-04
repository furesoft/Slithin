namespace Slithin.Modules.Events.Models;

public interface IEventService
{
    void Invoke<T>(string name, T argument);

    void Subscribe<T>(string name, Action<T> action);
}
