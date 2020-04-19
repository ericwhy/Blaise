namespace Blaise.CodeAnalysis.Syntax
{
    public abstract class StatementElement : SyntaxElement
    {
        public StatementElement(SyntaxToken semicolonToken)
        {
            SemicolonToken = semicolonToken;
        }

        public SyntaxToken SemicolonToken { get; }
    }
}