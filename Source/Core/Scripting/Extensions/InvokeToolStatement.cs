using System.Collections.Generic;
using System.Linq;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Extensions;

namespace Slithin.Core.Scripting.Extensions
{
    [CustomCodeFragment]
    public sealed class InvokeToolStatement : CodeNode
    {
        private readonly string _props;
        private string _tooldID;

        public InvokeToolStatement(string toolID, string props)
        {
            this._tooldID = toolID;
            _props = props;
        }

        public static CodeNode Parse(ParseInfo state, ref int position)
        {
            //invoke "backup" with backupProps

            if (!Parser.Validate(state.Code, "invoke", ref position))
                return null;

            while (char.IsWhiteSpace(state.Code, position))
                position++;

            var start = position;
            Parser.ValidateString(state.Code, ref position, false);

            var id = state.Code.Substring(start + 1, position - start - 2);

            if (state.Code[position] != ' ')
            {
                throw new JSException(new SyntaxError("Unexpected char at " + CodeCoordinates.FromTextPosition(state.Code, position, 0)));
            }

            while (char.IsWhiteSpace(state.Code, position))
                position++;

            if (!Parser.Validate(state.Code, "with", ref position))
                return null;

            while (char.IsWhiteSpace(state.Code, position))
                position++;

            start = position;
            if (!Parser.ValidateName(state.Code, ref position))
            {
                throw new JSException(new SyntaxError("Expected identifier name at " + CodeCoordinates.FromTextPosition(state.Code, position, 0)));
            }

            var props = state.Code.Substring(start, position - start);

            while (char.IsWhiteSpace(state.Code, position))
                position++;

            if (state.Code[position] != ';')
            {
                throw new JSException(new SyntaxError("Expected \";\" at " + CodeCoordinates.FromTextPosition(state.Code, position, 1)));
            }

            return new InvokeToolStatement(id, props);
        }

        public static bool Validate(string code, int position)
        {
            return Parser.Validate(code, "invoke", position);
        }

        public override void Decompose(ref CodeNode self)
        {
        }

        public override JSValue Evaluate(Context context)
        {
            var tool = Utils.Find<ITool>().FirstOrDefault(_ => _.Info.ID == _tooldID);

            if (tool != null)
            {
                var propsInstance = context.GetVariable(_props).As<ToolProperties>();

                tool.Invoke(propsInstance);
            }

            return null;
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> newVariables, int scopeBias)
        {
        }
    }
}
