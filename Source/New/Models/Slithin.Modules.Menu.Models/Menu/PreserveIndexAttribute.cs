using System;

namespace Slithin.Modules.Menu.Models.Menu;

[AttributeUsage(AttributeTargets.Class)]
public class PreserveIndexAttribute : Attribute
{
    public PreserveIndexAttribute(int index)
    {
        Index = index;
    }

    public int Index { get; set; }
}
