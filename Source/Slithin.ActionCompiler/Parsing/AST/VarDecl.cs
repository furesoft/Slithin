using Furesoft.Core.CodeDom.CodeDOM.Base;
using Furesoft.Core.CodeDom.CodeDOM.Base.Interfaces;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.Base;
using Furesoft.Core.CodeDom.CodeDOM.Expressions.References.Base;
using Furesoft.Core.CodeDom.CodeDOM.Statements.Variables;
using Furesoft.Core.CodeDom.CodeDOM.Statements.Variables.Base;
using Furesoft.Core.CodeDom.Parsing;
using Furesoft.Core.CodeDom.Rendering;
using Slithin.ActionCompiler.Parsing.AST.References;

namespace Slithin.ActionCompiler.Parsing.AST
{
    public class VarDecl : VariableDecl
    {
      

        public override string Category => "local variable";

        public override bool IsConst
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public override bool IsStatic
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public override Expression Type
        {
            set
            {
                SetField(ref _type, value, format: true);
            }
        }

        public VarDecl(string name, Expression type, Expression initialization)
            : base(name, type, initialization)
        {
        }

        public VarDecl(string name, Expression type)
            : base(name, type, null)
        {
        }

        public VarDecl(Parser parser, CodeObject parent)
            : base(parser, parent)
        {
            parser.NextToken();

            ParseName(parser, parent);

            if(parser.TokenText == ":")
            {
                parser.NextToken();

                ParseType(parser);
            }

            ParseInitialization(parser, parent);

            ParseTerminator(parser);
        }

        public static void AddParsePoints()
        {
            Parser.AddParsePoint("var", 1, Parse); //, typeof(IBlock));
        }

        public static VarDecl Parse(Parser parser, CodeObject parent, ParseFlags flags)
        {
            //var identifier (: <typename>)? (= <expression>)
            return new VarDecl(parser, parent);
        }

        public static bool PeekLocalDecl(Parser parser)
        {
            bool result = false;
            if (TypeRefBase.PeekType(parser, parser.Token, nonArrayType: false, ParseFlags.Type) && (parser.LastPeekedToken?.IsIdentifier ?? false) && ";=),".Contains(parser.PeekNextTokenText()))
            {
                result = true;
            }

            return result;
        }

        public override SymbolicRef CreateRef(bool isFirstOnLine)
        {
            return new VarRef(this, isFirstOnLine);
        }

        protected internal void SetTypeFromParentMulti(Expression type)
        {
            SetField(ref _type, type, format: true);
        }

        protected override void AsTextStatement(CodeWriter writer, RenderFlags flags)
        {
            writer.Write("var ");

            writer.WriteIdentifier(_name, flags);
            writer.Write(" ");

            RenderFlags flags2 = (RenderFlags)((uint)flags & 4286578688u);
            bool flag = flags.HasFlag(RenderFlags.Description);
            if (!(_parent is MultiLocalDecl) || (flag && !flags.HasFlag(RenderFlags.NoEOLComments)))
            {
                writer.Write(" : ");
                AsTextType(writer, flags);
            }

            UpdateLineCol(writer, flags);
            
            if (_initialization != null)
            {
                AsTextInitialization(writer, flags2);
            }
        }

        protected void ParseName(Parser parser, CodeObject parent)
        {
            _name = parser.GetIdentifierText();
            if (_name != null)
            {
                MoveLocationAndComment(parser.LastToken);
            }
        }
    }
}
