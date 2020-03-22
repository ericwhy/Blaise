using System.Collections.Generic;

namespace Blaise.CodeAnalysis
{
    public sealed class LiteralExpressionElement : ExpressionElement
    {
        public LiteralExpressionElement(SyntaxToken literalToken)
        {
            LiteralToken = literalToken;
        }

        public SyntaxToken LiteralToken { get; }

        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

        public override IEnumerable<SyntaxElement> GetChildElements()
        {
            yield return LiteralToken;
        }
    }

}