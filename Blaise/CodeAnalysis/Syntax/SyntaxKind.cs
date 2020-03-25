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
        ColonEqualsToken,
        ColonToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        EqualsToken,
        LtGtToken,
        OpenParensToken,
        CloseParensToken,
        LiteralAndToken,
        LiteralOrToken,
        LiteralNotToken,
        IdentifierToken,
        // Keywords
        TrueKeyword,
        FalseKeyword,
        // Expressions
        LiteralExpression,
        NameExpression,
        UnaryExpression,
        BinaryExpression,
        ParentheticalExpression,
        AssignmentExpression,
    }

}