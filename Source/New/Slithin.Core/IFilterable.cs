namespace Slithin.Core;

public interface IFilterable<out T>
    where T : new()
{
    T Filter { get; }
}
