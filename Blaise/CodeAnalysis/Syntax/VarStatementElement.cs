namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class VarStatementElement : StatementElement
    {
        public VarStatementElement(SyntaxToken keyword, SyntaxToken identifier, SyntaxToken colonToken, SyntaxToken typeToken, SyntaxToken semicolonToken)
        : base(semicolonToken)
        {
            Keyword = keyword;
            Identifier = identifier;
            ColonToken = colonToken;
            TypeToken = typeToken;
        }
        public override SyntaxKind Kind => SyntaxKind.VarStatement;

        public SyntaxToken Keyword { get; }
        public SyntaxToken Identifier { get; }
        public SyntaxToken ColonToken { get; }
        public SyntaxToken TypeToken { get; }
    }
}