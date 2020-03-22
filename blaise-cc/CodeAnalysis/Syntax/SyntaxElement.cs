using System.Collections.Generic;

namespace Blaise.CodeAnalysis.Syntax
{
    public abstract class SyntaxElement
    {
        public abstract SyntaxKind Kind { get; }
        public abstract IEnumerable<SyntaxElement> GetChildElements();
    }
}