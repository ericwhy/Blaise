namespace Blaise.CodeAnalysis
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
        BinaryExpression,
        ParentheticalExpression
    }

}