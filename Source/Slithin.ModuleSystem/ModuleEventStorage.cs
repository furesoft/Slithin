using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Slithin.ModuleSystem.StdLib;
using Slithin.ModuleSystem.WASInterface;

namespace Slithin.Core;

public static class ModuleEventStorage
{
    private static readonly Dictionary<string, List<(object, MethodInfo)>> _subscriptions = new();

    public static void Invoke<TArgument>(string name, TArgument argument)
    {
        var type = typeof(TArgument);
        //check if sub exist, if true copy argument to modules memory and invoke methodinfo with the address

        if (_subscriptions.ContainsKey(name))
        {
            if (argument.Equals(default(TArgument)))
            {
                foreach (var x in _subscriptions[name]) 
                    x.Item2.Invoke(x.Item1, Array.Empty<object>());
            }
            else
            {
                var ptr = Allocate(argument, type);
                InitializeMemory(ptr, argument);

                _subscriptions[name].ForEach(x => x.Item2.Invoke(x.Item1, new object[] {ptr}));
            }
        }
    }

    private static void InitializeMemory<TArgument>(int ptr, TArgument argument)
    {
        if (argument is string s)
        {
            var bytes = Util.ToUtf8(s);
            bytes.Append((byte) 0);

            Marshal.Copy(bytes.ToArray(), 0, (IntPtr) ptr, bytes.Length);
        }
        else
        {
            Marshal.StructureToPtr(argument, (IntPtr) ptr, false);
        }
    }

    private static int Allocate<TArgument>(TArgument argument, Type type)
    {
        var ptr = 0;
        if (type.IsValueType)
            ptr = Allocator.Allocate(Marshal.SizeOf<TArgument>());
        else if (type.Name == "String") ptr = Allocator.Allocate(((string) (object) argument).Length);

        return ptr;
    }

    public static void SubscribeModule(dynamic instance)
    {
        Type instanceType = instance.GetType();

        foreach (var method in instanceType.GetMethods())
            if (method.Name.StartsWith("On")) //All Functions Starting With "On" are Event Subscriptions
            {
                if (_subscriptions.ContainsKey(method.Name))
                {
                    _subscriptions[method.Name].Add((instance, method));
                }
                else
                {
                    var subscriptions = new List<(object, MethodInfo)>();
                    subscriptions.Add((instance, method));

                    _subscriptions.Add(method.Name, subscriptions);
                }
            }
    }
}