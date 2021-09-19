using System.Collections.Generic;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;

namespace Slithin.Core.Scripting.Extensions
{
    [CustomCodeFragment]
    public sealed class OnCallStatement : CodeNode
    {
        private string eventName;
        private string function;

        public OnCallStatement(string eventName, string function)
        {
            this.eventName = eventName;
            this.function = function;
        }

        public static CodeNode Parse(ParseInfo state, ref int position)
        {
            if (!Parser.Validate(state.Code, "on", ref position))
                return null;

            while (char.IsWhiteSpace(state.Code, position))
                position++;

            var start = position;
            Parser.ValidateName(state.Code, ref position);

            if (state.Code[position] != ' ')
            {
                throw new JSException(new SyntaxError("Unexpected char at " + CodeCoordinates.FromTextPosition(state.Code, position, 0)));
            }

            var eventName = state.Code[start..position];

            while (char.IsWhiteSpace(state.Code, position))
                position++;

            if (!Parser.Validate(state.Code, "call", ref position))
            {
                throw new JSException(new SyntaxError("Expected \"call\" at " + CodeCoordinates.FromTextPosition(state.Code, position, 2)));
            }

            while (char.IsWhiteSpace(state.Code, position))
                position++;

            start = position;
            if (!Parser.ValidateName(state.Code, ref position))
            {
                throw new JSException(new SyntaxError("Expected identifier name at " + CodeCoordinates.FromTextPosition(state.Code, position, 0)));
            }

            var function = state.Code.Substring(start, position - start);

            while (char.IsWhiteSpace(state.Code, position))
                position++;

            if (state.Code[position] != ';')
            {
                throw new JSException(new SyntaxError("Expected \";\" at " + CodeCoordinates.FromTextPosition(state.Code, position, 1)));
            }

            return new OnCallStatement(eventName, function);
        }

        public static bool Validate(string code, int position)
        {
            return Parser.Validate(code, "on", position);
        }

        public override void Decompose(ref CodeNode self)
        {
        }

        public override JSValue Evaluate(Context context)
        {
            context.Eval($"events.Subscribe(\"{eventName}\", {function});");

            return null;
        }

        public override void RebuildScope(FunctionInfo functionInfo, Dictionary<string, VariableDescriptor> newVariables, int scopeBias)
        {
        }
    }
}
