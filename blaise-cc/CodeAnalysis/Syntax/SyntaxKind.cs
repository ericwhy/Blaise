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
        BangToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        EqualsToken,
        LtGtToken,
        OpenParensToken,
        CloseParensToken,
        IdentifierToken,
        // Keywords
        TrueKeyword,
        FalseKeyword,
        // Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParentheticalExpression,
    }

}