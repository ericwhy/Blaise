using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundBinaryExpression(BoundExpression leftExpression, BoundExpression rightExpression, BoundBinaryOperator boundOperator)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
            BoundOperator = boundOperator;
        }
        public BoundExpression LeftExpression { get; }
        public BoundExpression RightExpression { get; }
        public BoundBinaryOperator BoundOperator { get; }

        public override Type BoundType => LeftExpression.BoundType;
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
    }
}