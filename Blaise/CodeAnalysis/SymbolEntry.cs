using System;

namespace Blaise.CodeAnalysis
{
    public sealed class SymbolEntry
    {
        internal SymbolEntry(string symbolName, Type symbolType)
        {
            SymbolName = symbolName;
            SymbolType = symbolType;
        }

        public string SymbolName { get; }
        public Type SymbolType { get; }
    }

}