using System.Collections.Generic;

namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class AssignmentExpressionElement : ExpressionElement
    {
        public AssignmentExpressionElement(SyntaxToken identifierToken, SyntaxToken equalsToken, ExpressionElement expression)
        {
            IdentifierToken = identifierToken;
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public SyntaxToken IdentifierToken { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionElement Expression { get; }

        public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;

        public override IEnumerable<SyntaxElement> GetChildElements()
        {
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return Expression;
        }
    }
}