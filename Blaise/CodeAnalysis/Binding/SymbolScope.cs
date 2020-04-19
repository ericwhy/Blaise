using System.Collections.Generic;
using System.Collections.Immutable;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class SymbolScope
    {
        private Dictionary<string, SymbolEntry> _symbols = new Dictionary<string, SymbolEntry>();

        public SymbolScope(SymbolScope outerScope)
        {
            OuterScope = outerScope;
        }
        public bool TryLookup(string name, out SymbolEntry symbol)
        {
            if(_symbols.TryGetValue(name, out symbol))
                return true;
            if(OuterScope == null)
                return false;
            return OuterScope.TryLookup(name, out symbol);
        }
        public bool TryDeclare(SymbolEntry symbol)
        {
            if(_symbols.ContainsKey(symbol.SymbolName))
            {
                return false;
            }
            _symbols.Add(symbol.SymbolName, symbol);
            return true;
        }
        public ImmutableArray<SymbolEntry> DeclaredSymbols => _symbols.Values.ToImmutableArray();

        public SymbolScope OuterScope { get; }
    }
}