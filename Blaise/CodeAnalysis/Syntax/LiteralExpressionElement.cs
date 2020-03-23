using System.Collections.Generic;

namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class LiteralExpressionElement : ExpressionElement
    {
        public LiteralExpressionElement(SyntaxToken literalToken)
            : this(literalToken, literalToken.Value)
        {
        }
        public LiteralExpressionElement(SyntaxToken literalToken, object value)
        {
            LiteralToken = literalToken;
            Value = value;
        }

        public SyntaxToken LiteralToken { get; }
        public object Value { get; }

        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;

        public override IEnumerable<SyntaxElement> GetChildElements()
        {
            yield return LiteralToken;
        }
    }

}