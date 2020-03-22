using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression leftExpression, BoundExpression rightExpression, BoundBinaryOperatorKind operatorKind)
        {
            LeftExpression = leftExpression;
            OperatorKind = operatorKind;
            RightExpression = rightExpression;
        }
        public BoundExpression LeftExpression { get; }
        public BoundBinaryOperatorKind OperatorKind { get; }
        public BoundExpression RightExpression { get; }
        public override Type BoundType => LeftExpression.BoundType;
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
    }
}