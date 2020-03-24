using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(SymbolEntry symbol)
        {
            Symbol = symbol;
        }

        public SymbolEntry Symbol { get; }

        public override Type BoundType => Symbol.SymbolType;

        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
    }
}