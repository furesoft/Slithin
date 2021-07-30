using System;
using System.Collections.Generic;
using System.Linq;
using NiL.JS;
using NiL.JS.Core;

namespace Slithin
{
    public class ModuleBuilder
    {
        private Dictionary<string, JSValue> _values = new();
        private JSValue defaultExport;

        public void Add(string name, JSValue value)
        {
            if (!_values.ContainsKey(name))
            {
                _values.Add(name, value);
            }
        }

        public void Add(JSValue export)
        {
            defaultExport = export;
        }

        public Module Build(string cmdArgument)
        {
            Module result = null;

            if (defaultExport == null)
            {
                string exports = $"export {{ {string.Join(" , ", _values.Keys.ToArray())} }};";

                result = new Module(cmdArgument, exports);

                foreach (var item in _values)
                {
                    result.Context.DefineVariable(item.Key).Assign(item.Value);
                }
            }
            else
            {
                result = new Module(cmdArgument, "export default ns;");
                result.Context.DefineVariable("ns").Assign(defaultExport);
            }

            return result;
        }
    }
}
