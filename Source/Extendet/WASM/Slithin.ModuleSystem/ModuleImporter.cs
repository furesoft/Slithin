using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Slithin.ModuleSystem.WASInterface;
using WebAssembly.Runtime;

namespace Slithin.ModuleSystem;

public static class ModuleImporter
{
    public static readonly List<Type> Types = new();

    public static void Export(Type type, dynamic instance)
    {
        foreach (var field in type.GetFields())
        {
            if (!field.IsStatic) continue;

            ExportValues(field);

            ExportGlobals(instance, field);
        }
    }

    private static void ExportValues(FieldInfo field)
    {
        var fiattr = field.GetCustomAttribute<WasmImportValueAttribute>();

        if (fiattr == null) return;

        var mem = WasmMemory.Mem + fiattr.Offset;

        object value = null;

        if (field.FieldType.IsValueType)
            value = Marshal.PtrToStructure(mem, field.FieldType);
        else if (field.FieldType == typeof(string))
            value = fiattr.Length != default
                ? Marshal.PtrToStringUTF8(mem, fiattr.Length)
                : Marshal.PtrToStringUTF8(mem);

        field.SetValue(null, value);
    }

    private static void ExportGlobals(dynamic instance, FieldInfo field)
    {
        var giattr = field.GetCustomAttribute<WasmImportGlobalAttribute>();

        if (giattr == null) return;

        Type instanceType = instance.GetType();
        foreach (var prop in instanceType.GetProperties())
            if (giattr.Name == prop.Name)
                field.SetValue(null, prop.GetValue(instance));
    }

    public static void Import(Type type, ImportDictionary imports)
    {
        var attr = type.GetCustomAttribute<WasmExportAttribute>();

        Types.Add(type);

        IDictionary<string, RuntimeImport> dict = null;

        if (attr != null)
        {
            dict = InitImportdictionary(imports, attr);

            ImportFunctions(type, dict);
        }

        foreach (var field in type.GetFields()) ImportField(field, dict);
    }

    private static void ImportField(FieldInfo field, IDictionary<string, RuntimeImport> dict)
    {
        if (!field.IsStatic) return;
        var fattr = field.GetCustomAttribute<WasmExportValueAttribute>();

        if (fattr != null) ImportValues(field, fattr);

        ImportGlobals(field, dict);
    }

    private static IDictionary<string, RuntimeImport> InitImportdictionary(ImportDictionary imports,
        WasmExportAttribute attr)
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

        return dict;
    }

    private static void ImportValues(FieldInfo field, WasmExportValueAttribute fattr)
    {
        var value = field.GetValue(null);

        var mem = WasmMemory.Mem + fattr.Offset;

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

    private static void ImportGlobals(FieldInfo field, IDictionary<string, RuntimeImport> dict)
    {
        var gattr = field.GetCustomAttribute<WasmExportGlobalAttribute>();
        if (gattr == null) return;

        var value = field.GetValue(null);
        dict.Add(gattr.Name, new GlobalImport(() => (int) value, _ => field.SetValue(null, _)));
    }

    private static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
    {
        Func<Type[], Type> getType;
        var isAction = methodInfo.ReturnType == typeof(void);
        var types = methodInfo.GetParameters().Select(p => p.ParameterType);

        if (isAction)
        {
            getType = Expression.GetActionType;
        }
        else
        {
            getType = Expression.GetFuncType;
            types = types.Concat(new[] {methodInfo.ReturnType});
        }

        return methodInfo.IsStatic
            ? Delegate.CreateDelegate(getType(types.ToArray()), methodInfo)
            : Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
    }

    private static void ImportFunctions(Type type, IDictionary<string, RuntimeImport> dict)
    {
        foreach (var method in type.GetMethods())
        {
            var attr = method.GetCustomAttribute<WasmExportAttribute>();

            if (attr == null) continue;
            if (!method.IsStatic) continue;

            var del = CreateDelegate(method, null);

            dict.Add(attr.Name, new FunctionImport(del));
        }
    }
}