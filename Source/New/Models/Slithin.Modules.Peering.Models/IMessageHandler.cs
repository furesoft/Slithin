namespace Slithin.Modules.Peering.Models;

public interface IMessageHandler<T>
{
    void Handle(T message);
}
