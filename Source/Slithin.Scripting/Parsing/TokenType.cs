namespace Slithin.Scripting.Parsing;

public enum TokenType
{
    Invalid,
    EOF,
    Identifier,
    StringLiteral,
    Number,
    As,
    Remember,
    Dot,
    Plus,
    Minus,
    Slash,
    Star,
    OpenParen,
    CloseParen,
    Not,
    Colon,
    At,
    TrueLiteral,
    FalseLiteral,
}