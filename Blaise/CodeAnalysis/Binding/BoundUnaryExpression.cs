using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression
    {
        public BoundUnaryExpression(BoundUnaryOperator boundOperator, BoundExpression operandExpression)
        {
            BoundOperator = boundOperator;
            OperandExpression = operandExpression;
        }
        public BoundUnaryOperator BoundOperator { get; }
        public BoundExpression OperandExpression { get; }
        public override Type BoundType => BoundOperator.ResultType;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    }
}