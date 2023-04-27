﻿using Furesoft.Core.CodeDom.CodeDOM;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Arithmetic;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Assignments;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Bitwise;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Conditional;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Binary.Relational;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Other;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Operators.Unary;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Other;
using Slithin.ActionCompiler.Parsing.AST;
using Slithin.ActionCompiler.Parsing.AST.References;

namespace Slithin.ActionCompiler
{
    public class OodParser
    {
        static OodParser()
        {
            Negative.AddParsePoints();
            Literal.AddParsePoints();

            Add.AddParsePoints();
            Multiply.AddParsePoints();
            Divide.AddParsePoints();
            Subtract.AddParsePoints();
            Call.AddParsePoints();
            Expression.AddParsePoints();

            GreaterThan.AddParsePoints();
            LessThan.AddParsePoints();
            GreaterThanEqual.AddParsePoints();
            LessThanEqual.AddParsePoints();
            NotEqual.AddParsePoints();
            Equal.AddParsePoints();

            And.AddParsePoints();
            Or.AddParsePoints();
            BitwiseXor.AddParsePoints();
            BitwiseOr.AddParsePoints();
            BitwiseAnd.AddParsePoints();

            AddAssign.AddParsePoints();
            SubtractAssign.AddParsePoints();

            PostIncrement.AddParsePoints();
            PostDecrement.AddParsePoints();

            VarDecl.AddParsePoints();

            Mod.AddParsePoints();

            Dot.AddParsePoints();

            GenericTypeRef.AddParsePoints();
            ArrayTypeRef.AddParsePoints();

            //NewArray.AddParsePoints();
        }

        public static CodeUnit ParseFile(string filename)
        {
            var tree = CodeUnit.Load(filename);

            return tree;
        }
    }
}