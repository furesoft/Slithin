using System.Collections.Generic;
using NodeEditor.Model;
using Slithin.Core;
using Slithin.VPL;

namespace Slithin.Core.VPLNodeBuilding;

public class NodeCategory : NotifyObject
{
    private IList<INodeTemplate>? _templates;
    public string Name { get; set; }

    public IList<INodeTemplate>? Templates
    {
        get => _templates;
        set => SetValue(ref _templates, value);
    }

    public override string ToString()
    {
        return Name;
    }
}
