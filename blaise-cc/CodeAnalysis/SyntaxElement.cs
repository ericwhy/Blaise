using System.Collections.Generic;

namespace Blaise.CodeAnalysis
{
    public abstract class SyntaxElement
    {
        public abstract SyntaxKind Kind { get; }
        public abstract IEnumerable<SyntaxElement> GetChildElements();
    }
}