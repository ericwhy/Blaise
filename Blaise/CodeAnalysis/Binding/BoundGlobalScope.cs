using System.Collections.Immutable;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundGlobalScope
    {
        public BoundGlobalScope(
            BoundGlobalScope previous,
            ImmutableArray<Diagnostic> messages,
            ImmutableArray<SymbolEntry> symbols,
            BoundStatement statement
        )
        {
            Previous = previous;
            Messages = messages;
            Symbols = symbols;
            Statement = statement;
        }

        public BoundGlobalScope Previous { get; }
        public ImmutableArray<Diagnostic> Messages { get; }
        public ImmutableArray<SymbolEntry> Symbols { get; }
        public BoundStatement Statement { get; }
    }
}