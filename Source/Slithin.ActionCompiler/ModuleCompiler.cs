using System.Collections.Generic;
using System.IO;
using System.Linq;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Arithmetic;
using MessagePack;
using Newtonsoft.Json;
using Slithin.ActionCompiler.Compiling;
using Slithin.ActionCompiler.Compiling.Passes;
using Slithin.ModuleSystem;
using WebAssembly;
using WebAssembly.Instructions;
using Block = Furesoft.Core.CodeDom.CodeDOM.Base.Block;

namespace Slithin.ActionCompiler;

public class ModuleCompiler
{
    public static Module Compile(string scriptFilename, string infoFilename, string uiFilename = null,
        string imageFilename = null)
    {
        var m = new Module();

        //serialize scriptinfo into data segment
        var info = JsonConvert.DeserializeObject<ScriptInfo>(File.ReadAllText(infoFilename));
        var infoBytes = MessagePackSerializer.Serialize(info);

        m.CustomSections.Add(new CustomSection {Name = ".info", Content = new List<byte>(infoBytes)});

        if (imageFilename != null)
            m.CustomSections.Add(new CustomSection
                {Name = ".image", Content = new List<byte>(File.ReadAllBytes(imageFilename))});

        if (uiFilename != null)
            m.CustomSections.Add(new CustomSection
                {Name = ".ui", Content = new List<byte>(File.ReadAllBytes(uiFilename))});

        m.Types.Add(new WebAssemblyType());

        m.Imports.Add(new Import.Memory("env", "memory", new Memory(1, 25)));

        m.Types.Add(new WebAssemblyType
        {
            Parameters = new List<WebAssemblyValueType> {WebAssemblyValueType.Int32, WebAssemblyValueType.Int32}
        });

        m.Imports.Add(new Import.Function("conversions", "intToString", 1));

        m.Functions.Add(new Function(0));

        Optimiser.AddPass<ConstantFoldingPass>();

        var tree = new Block(new Add(1, 2));
        tree = Optimiser.Process(tree);

        var exprBody = ExpressionEmitter.Emit(tree.GetChildren<Expression>().First(), null);
        exprBody.Clear();

        exprBody.Add(new Int32Constant(42));
        exprBody.Add(new Int32Constant(125));

        exprBody.Add(new Call(0));

        exprBody.Add(new End());

        m.Codes.Add(new FunctionBody(exprBody.ToArray()));

        m.Exports.Add(new Export("_start", 1));

        m.Globals.Add(new Global(WebAssemblyValueType.Int32)
            {IsMutable = false, InitializerExpression = new List<Instruction> {new Int32Constant(1024), new End()}});
        m.Exports.Add(new Export("_heap_base", 0, ExternalKind.Global));

        return m;
    }
}

//ScriptInfo als data hinzufügen
//falls ui-xaml vorhanden, laden und als serialized in custom section speichern
//compilation des scripts mit start funktion in module