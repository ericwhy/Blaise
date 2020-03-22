namespace Blaise.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        IntegerToken,
        PlusToken,
        MinusToken,
        SplatToken,
        SlashToken,
        OpenParensToken,
        CloseParensToken,

        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParentheticalExpression
    }

}