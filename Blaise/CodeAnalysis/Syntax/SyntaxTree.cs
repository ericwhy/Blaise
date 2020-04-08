using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(SyntaxElement root, SyntaxToken endOfFileToken, ImmutableArray<Diagnostic> messages)
        {
            Root = root;
            EndOfFileToken = endOfFileToken;
            Messages = messages;
        }

        public SyntaxElement Root { get; }
        public SyntaxToken EndOfFileToken { get; }
        public ImmutableArray<Diagnostic> Messages { get; }

        public static SyntaxTree ParseTree(string source)
        {
            var parser = new ElementParser(source);
            return parser.ParseTree();
        }

        // Added for testing support
        public static IEnumerable<SyntaxToken> ParseTokens(string text)
        {
            var analyzer = new LexicalAnalyzer(text);
            while (true)
            {
                var token = analyzer.NextToken();
                if (token.Kind == SyntaxKind.EndOfFileToken)
                    break;
                yield return token;
            }
        }
    }

}