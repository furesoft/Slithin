using System;
using Slithin.Core;

namespace Slithin.Core.Menu;

[AttributeUsage(AttributeTargets.Class)]
public class PageIconAttribute : Attribute
{
    public PageIconAttribute(string key)
    {
        Key = key;
    }

    public string Key { get; set; }
}
