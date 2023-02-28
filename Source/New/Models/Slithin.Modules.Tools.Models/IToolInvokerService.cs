namespace Slithin.Modules.Tools.Models;

public interface IToolInvokerService
{
    Dictionary<string, ITool> Tools { get; }

    void Init();

    void Invoke(string id);
}
