namespace Slithin.Modules.Tools.Models;

/// <summary>
/// A service to invoke tools
/// </summary>
public interface IToolInvokerService
{
    Dictionary<string, ITool> Tools { get; }

    void Init();

    void Invoke(string id, ToolProperties props);
}
