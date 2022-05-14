using System;
using Slithin.Core;

namespace Slithin.Core.Menu;

[AttributeUsage(AttributeTargets.Class)]
public class PreserveIndexAttribute : Attribute
{
    public PreserveIndexAttribute(int index)
    {
        Index = index;
    }

    public int Index { get; set; }
}
