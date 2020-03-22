using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperatorKind operatorKind, BoundExpression operandExpression)
        {
            OperatorKind = operatorKind;
            OperandExpression = operandExpression;
        }
        public BoundUnaryOperatorKind OperatorKind { get; }
        public BoundExpression OperandExpression { get; }
        public override Type BoundType => OperandExpression.BoundType;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    }
}