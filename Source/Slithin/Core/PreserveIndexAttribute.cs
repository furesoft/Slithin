using System;

namespace Slithin.Core;

[AttributeUsage(AttributeTargets.Class)]
public class PreserveIndexAttribute : Attribute
{
    public PreserveIndexAttribute(int index)
    {
        Index = index;
    }

    public int Index { get; set; }
}
