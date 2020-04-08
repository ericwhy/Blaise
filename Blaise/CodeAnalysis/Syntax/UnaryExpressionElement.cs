using System.Collections.Generic;

namespace Blaise.CodeAnalysis.Syntax
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
    }
}