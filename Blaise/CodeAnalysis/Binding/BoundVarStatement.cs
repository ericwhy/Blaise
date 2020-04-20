namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundVarStatement : BoundStatement
    {
        public BoundVarStatement(SymbolEntry symbol)
        {
            Symbol = symbol;
        }

        public SymbolEntry Symbol { get; }

        public override BoundNodeKind Kind => BoundNodeKind.VarStatement;
    }
}