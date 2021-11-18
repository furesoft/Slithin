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

        m.Types.Add(new WebAssemblyType {Returns = new List<WebAssemblyValueType> {WebAssemblyValueType.Int32}});

        m.Exports.Add(new Export("_start"));

        m.Functions.Add(new Function(0));

        PassManager.AddPass<ConstantFoldingPass>();

        var tree = new Block(new Add(1, 2));
        tree = PassManager.Process(tree);

        var exprBody = ExpressionEmitter.Emit(tree.GetChildren<Expression>().First(), null);
        exprBody.Add(new End());

        m.Codes.Add(new FunctionBody(exprBody.ToArray()));

        m.Imports.Add(new Import.Memory("env", "memory", new Memory(1, 25)));

        return m;
    }
}

//ScriptInfo als data hinzufügen
//falls ui-xaml vorhanden, laden und als serialized in custom section speichern
//compilation des scripts mit start funktion in module