using System.Collections.Generic;
using NUnit.Framework;
using Slithin.ModuleSystem.StdLib;

namespace Slithin.Tests;

[TestFixture]
public class AllocatorTests
{
    [Test]
    public void Test()
    {
        Allocator.HeapBaseAddress = 1024;
        var list = new List<int>();

        for (var j = 0; j < 100; j++)
        {
            var i = Allocator.Allocate(sizeof(int) * j);
            list.Add(i);
        }

        Allocator.Free(0);
    }
}