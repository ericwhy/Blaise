using System.Collections.Generic;
using System.Linq;

namespace Blaise.CodeAnalysis
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(SyntaxElement root, SyntaxToken endOfFileToken, IEnumerable<string> messages)
        {
            Root = root;
            EndOfFileToken = endOfFileToken;
            Messages = messages.ToArray();
        }

        public SyntaxElement Root { get; }
        public SyntaxToken EndOfFileToken { get; }
        public IReadOnlyList<string> Messages { get; }

        public static SyntaxTree ParseTree(string source)
        {
            var parser = new ElementParser(source);
            return parser.ParseTree();
        }
    }

}