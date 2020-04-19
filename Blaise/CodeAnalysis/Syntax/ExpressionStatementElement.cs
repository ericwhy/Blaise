namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class ExpressionStatementElement : StatementElement
    {
        public ExpressionStatementElement(ExpressionElement expression, SyntaxToken semicolonToken)
            : base(semicolonToken)
        {
            Expression = expression;
        }

        public ExpressionElement Expression { get; }
        public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;
    }
}