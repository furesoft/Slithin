﻿using Slithin.ModuleSystem.WASInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using WebAssembly.Runtime;

namespace Slithin.ModuleSystem
{
    public static class ModuleImporter
    {
        public static List<Type> Types = new();

        public unsafe static void Export(Type type)
        {
            foreach (var field in type.GetFields())
            {
                if (field.IsStatic)
                {
                    var fiattr = field.GetCustomAttribute<WasmImportValueAttribute>();

                    if (fiattr != null)
                    {
                        var mem = Sg_wasm.__mem + fiattr.Offset;

                        object value = null;
                        if (field.FieldType.IsValueType)
                        {
                            value = Marshal.PtrToStructure(mem, field.FieldType);
                        }
                        else if (field.FieldType == typeof(string))
                        {
                            if (fiattr.Length != default)
                            {
                                value = Marshal.PtrToStringUTF8(mem, fiattr.Length);
                            }
                            else
                            {
                                value = Marshal.PtrToStringUTF8(mem);
                            }
                        }

                        field.SetValue(null, value);
                    }
                }
            }
        }

        public static void Import(Type type, ImportDictionary imports)
        {
            var attr = type.GetCustomAttribute<WasmExportAttribute>();

            Types.Add(type);

            if (attr != null)
            {
                IDictionary<string, RuntimeImport> dict;
                if (imports.ContainsKey(attr.Name))
                {
                    dict = imports[attr.Name];
                }
                else
                {
                    dict = new Dictionary<string, RuntimeImport>();
                    imports.Add(attr.Name, dict);
                }

                ImportFunctions(type, dict);
            }

            foreach (var field in type.GetFields())
            {
                if (field.IsStatic)
                {
                    var fattr = field.GetCustomAttribute<WasmExportValueAttribute>();

                    if (fattr != null)
                    {
                        var value = field.GetValue(null);

                        var mem = Sg_wasm.__mem + fattr.Offset;

                        if (field.FieldType.IsValueType)
                        {
                            Marshal.StructureToPtr(value, mem, false);
                        }
                        else if (value is string str)
                        {
                            var utf8 = Util.ToUtf8(str);

                            Marshal.Copy(utf8, 0, mem, str.Length);
                        }
                    }
                }
            }
        }

        private static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
        {
            Func<Type[], Type> getType;
            var isAction = methodInfo.ReturnType.Equals((typeof(void)));
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);

            if (isAction)
            {
                getType = Expression.GetActionType;
            }
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }

            if (methodInfo.IsStatic)
            {
                return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
            }

            return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }

        private static void ImportFunctions(Type type, IDictionary<string, RuntimeImport> dict)
        {
            foreach (var method in type.GetMethods())
            {
                var attr = method.GetCustomAttribute<WasmExportAttribute>();

                if (attr != null && method.IsStatic)
                {
                    var del = CreateDelegate(method, null);

                    dict.Add(attr.Name, new FunctionImport(del));
                }
            }
        }
    }
}