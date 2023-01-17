using System.Collections.Concurrent;

namespace Slithin.Modules.Menu.Models.ContextualMenu;

public class ContextualRegistrar
{
    private readonly ConcurrentDictionary<string, ConcurrentBag<ContextualElement>> _elements = new();

    public void RegisterFor(string id, ContextualElement element)
    {
        if (!_elements.ContainsKey(id))
        {
            _elements.TryAdd(id, new());
        }
        
        _elements[id].Add(element);
    }

    public IEnumerable<ContextualElement> GetAllElements(string id)
    {
        if (!_elements.ContainsKey(id))
        {
            return Array.Empty<ContextualElement>();
        }
        
        return _elements[id];
    }
}
