using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(SymbolEntry symbol, BoundExpression boundExpression)
        {
            Symbol = symbol;
            BoundExpression = boundExpression;
        }

        public override Type BoundType => BoundExpression.BoundType;

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;

        public SymbolEntry Symbol { get; }
        public BoundExpression BoundExpression { get; }
    }
}