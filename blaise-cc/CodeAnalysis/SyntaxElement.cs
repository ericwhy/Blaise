using System.Collections.Generic;

namespace Blaise.CodeAnalysis
{
    abstract class SyntaxElement
    {
        public abstract SyntaxKind Kind { get; }
        public abstract IEnumerable<SyntaxElement> GetChildElements();
    }
}