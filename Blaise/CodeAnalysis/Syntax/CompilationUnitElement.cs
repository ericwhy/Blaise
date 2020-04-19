namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class CompilationUnitElement : SyntaxElement
    {
        public CompilationUnitElement(ExpressionElement expression, SyntaxToken endOfFileToken)
        {
            Expression = expression;
            EndOfFileToken = endOfFileToken;
        }

        public ExpressionElement Expression { get; }
        public SyntaxToken EndOfFileToken { get; }

        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
    }

}