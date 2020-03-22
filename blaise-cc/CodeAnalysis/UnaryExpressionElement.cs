using System.Collections.Generic;

namespace Blaise.CodeAnalysis
{
    public sealed class UnaryExpressionElement : ExpressionElement
    {
        public UnaryExpressionElement(SyntaxToken operatorElement, ExpressionElement operandExpression)
        {
            OperatorElement = operatorElement;
            OperandExpression = operandExpression;
        }

        public SyntaxToken OperatorElement { get; }
        public ExpressionElement OperandExpression { get; }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

        public override IEnumerable<SyntaxElement> GetChildElements()
        {
            yield return OperatorElement;
            yield return OperandExpression;
        }
    }
}