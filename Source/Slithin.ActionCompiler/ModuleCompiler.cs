using CommandLine;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Arithmetic;
using Slithin.ActionCompiler.Compiling;
using Slithin.ActionCompiler.Compiling.Stages;
using Slithin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssembly;
using WebAssembly.Instructions;
using Block = Furesoft.Core.CodeDom.CodeDOM.Base.Block;

namespace Slithin.ActionCompiler;

public static class ModuleCompiler
{
    //https://github.com/benfoster/Flo
    public static Func<CompilerContext, Task<CompilerContext>> Pipeline;

    static ModuleCompiler()
    {
        Pipeline = Flo.Pipeline.Build<CompilerContext, CompilerContext>(
            cfg =>
            {
                cfg.When(_ => !_.IsLibray, cfg =>
                {
                    cfg.Add<ResourceStage>();
                });

                cfg.Add<ParsingStage>();
                cfg.Add<LowererStage>();

                cfg.Add<OptimizingStage>();
                cfg.Add<PostProcessStage>();

                cfg.Add<EmitModuleStage>();
            }
        );
    }

    public static void Compile()
    {
        Parser.Default.ParseArguments<CompilerContext>(Environment.GetCommandLineArgs())
                   .WithParsed(async o =>
                   {
                       _ = await Pipeline.Invoke(o);
                   });
    }

    [Obsolete]
    public static Module Compile(string scriptFilename, string infoFilename, string uiFilename = null,
        string imageFilename = null)
    {
        var m = new Module();

        m.Types.Add(new WebAssemblyType());

        m.Imports.Add(new Import.Memory("env", "memory", new Memory(1, 3)));
        m.AddData(1024, "Give me your name");

        m.Types.Add(new WebAssemblyType
        {
            Parameters = new List<WebAssemblyValueType> { WebAssemblyValueType.Int32 }
        });
        m.Types.Add(new WebAssemblyType
        {
            Parameters = new List<WebAssemblyValueType> { WebAssemblyValueType.Int32 },
            Returns = new List<WebAssemblyValueType>() { WebAssemblyValueType.Int32 }
        });

        m.Imports.Add(new Import.Function("notification", "show", 1));
        m.Imports.Add(new Import.Function("notification", "prompt", 2));

        m.Functions.Add(new Function(0));
        m.Functions.Add(new Function(0));

        var tree = new Block(new Add(1, 2));

        var exprBody = ExpressionEmitter.Emit(tree.GetChildren<Expression>().First(), null);
        exprBody.Clear();

        exprBody.Add(new Int32Constant(1024));

        exprBody.Add(new Call(1));
        exprBody.Add(new Call(0));

        exprBody.Add(new End());

        m.Codes.Add(new FunctionBody(exprBody.ToArray()));

        m.Codes.Add(new FunctionBody(exprBody.ToArray()));

        m.Exports.Add(new Export("_start", 3));
        m.Exports.Add(new Export("OnConnect", 2));

        m.Globals.Add(new Global(WebAssemblyValueType.Int32)
        { IsMutable = false, InitializerExpression = new List<Instruction> { new Int32Constant(1024), new End() } });
        m.Exports.Add(new Export("_heap_base", 0, ExternalKind.Global));

        return m;
    }
}