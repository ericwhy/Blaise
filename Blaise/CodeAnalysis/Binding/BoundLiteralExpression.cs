using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(object boundValue)
        {
            BoundValue = boundValue;
        }
        public object BoundValue { get; }
        public override Type BoundType => BoundValue.GetType();
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
    }
}