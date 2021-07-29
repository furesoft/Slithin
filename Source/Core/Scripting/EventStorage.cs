using System.Collections.Generic;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Slithin.Core.Scripting
{
    public class EventStorage
    {
        private Dictionary<string, List<JSValue>> _callbacks = new();

        public void Invoke(string name, object[] args = null)
        {
            if (_callbacks.ContainsKey(name))
            {
                foreach (var callback in _callbacks[name])
                {
                    if (callback.Is<Function>())
                    {
                        var func = callback.As<Function>();
                        var a = new Arguments();

                        if (args is not null)
                        {
                            foreach (var item in args)
                            {
                                a.Add(item);
                            }
                        }

                        func.Call(a);
                    }
                }
            }
        }

        public void Subscribe(string name, JSValue callback)
        {
            if (_callbacks.ContainsKey(name))
            {
                _callbacks[name].Add(callback);
            }
            else
            {
                var tmp = new List<JSValue>();
                tmp.Add(callback);

                _callbacks.Add(name, tmp);
            }
        }
    }
}
