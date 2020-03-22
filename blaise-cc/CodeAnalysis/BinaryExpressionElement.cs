using System.Collections.Generic;

namespace Blaise.CodeAnalysis
{
    public sealed class BinaryExpressionElement : ExpressionElement
    {
        public BinaryExpressionElement(ExpressionElement leftExpression, SyntaxToken operatorElement, ExpressionElement rightExpression)
        {
            LeftExpression = leftExpression;
            OperatorElement = operatorElement;
            RightExpression = rightExpression;
        }

        public ExpressionElement LeftExpression { get; }
        public SyntaxToken OperatorElement { get; }
        public ExpressionElement RightExpression { get; }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public override IEnumerable<SyntaxElement> GetChildElements()
        {
            yield return LeftExpression;
            yield return OperatorElement;
            yield return RightExpression;
        }
    }
}