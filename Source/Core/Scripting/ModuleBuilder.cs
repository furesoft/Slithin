using System;
using System.Collections.Generic;
using System.Linq;
using NiL.JS;
using NiL.JS.Core;

namespace Slithin.Core.Scripting
{
    public class ModuleBuilder
    {
        private readonly List<Type> _ctors = new();
        private readonly Dictionary<string, JSValue> _values = new();
        private JSValue _defaultExport;

        public void Add(string name, JSValue value)
        {
            if (!_values.ContainsKey(name))
            {
                _values.Add(name, value);
            }
        }

        public void Add(JSValue export)
        {
            _defaultExport = export;
        }

        public void AddConstructor(Type type)
        {
            _ctors.Add(type);
        }

        public void AddFunction(string name, Delegate value)
        {
            Add(name, JSValue.Marshal(value));
        }

        public Module Build(string cmdArgument)
        {
            Module result = null;

            if (_defaultExport != null)
            {
                result = new Module(cmdArgument, "export default ns;");
                result.Context.DefineVariable("ns").Assign(_defaultExport);
                return result;
            }

            var items = _values.Keys;
            var typeNames = _ctors.Select(_ => _.Name);

            var exports = $"export {{ {string.Join(" , ", items.Concat(typeNames))} }};";

            result = new Module(cmdArgument, exports);

            foreach (var item in _values)
            {
                result.Context.DefineVariable(item.Key).Assign(item.Value);
            }
            foreach (var ctor in _ctors)
            {
                result.Context.DefineConstructor(ctor);
            }

            return result;
        }
    }
}
