using System.Collections.Generic;

namespace Blaise.CodeAnalysis
{
    sealed class IntegerExpressionElement : ExpressionElement
    {
        public IntegerExpressionElement(SyntaxToken integerToken)
        {
            IntegerToken = integerToken;
        }

        public SyntaxToken IntegerToken { get; }

        public override SyntaxKind Kind => SyntaxKind.IntegerExpression;

        public override IEnumerable<SyntaxElement> GetChildElements()
        {
            yield return IntegerToken;
        }
    }

}