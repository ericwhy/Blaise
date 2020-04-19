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
        BangEqualsToken,
        OpenParensToken,
        CloseParensToken,
        OpenBraceToken,
        CloseBraceToken,
        SemicolonToken,
        IdentifierToken,
        // Keywords
        AndKeyword,
        OrKeyword,
        NotKeyword,
        TrueKeyword,
        FalseKeyword,
        BeginKeyword,
        EndKeyword,
        // Expressions
        LiteralExpression,
        NameExpression,
        UnaryExpression,
        BinaryExpression,
        ParentheticalExpression,
        AssignmentExpression,
        // Units and statements
        CompilationUnit,
        BlockStatement,
        ExpressionStatement,
    }

}