using System;

namespace Slithin.Core;

[AttributeUsage(AttributeTargets.Class)]
public class PageIconAttribute : Attribute
{
    public PageIconAttribute(string key)
    {
        Key = key;
    }

    public string Key { get; set; }
}
