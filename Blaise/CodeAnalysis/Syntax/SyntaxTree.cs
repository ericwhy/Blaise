using System.Collections.Generic;
using System.Linq;

namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(SyntaxElement root, SyntaxToken endOfFileToken, IEnumerable<Diagnostic> messages)
        {
            Root = root;
            EndOfFileToken = endOfFileToken;
            Messages = messages.ToArray();
        }

        public SyntaxElement Root { get; }
        public SyntaxToken EndOfFileToken { get; }
        public IReadOnlyList<Diagnostic> Messages { get; }

        public static SyntaxTree ParseTree(string source)
        {
            var parser = new ElementParser(source);
            return parser.ParseTree();
        }
    }

}