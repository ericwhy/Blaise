using System.Collections.Generic;

namespace Blaise.CodeAnalysis.Syntax
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
    }
}