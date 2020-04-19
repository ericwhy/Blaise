namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class CompilationUnitElement : SyntaxElement
    {
        public CompilationUnitElement(StatementElement statement, SyntaxToken endOfFileToken)
        {
            Statement = statement;
            EndOfFileToken = endOfFileToken;
        }

        public StatementElement Statement { get; }
        public SyntaxToken EndOfFileToken { get; }

        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
    }
}