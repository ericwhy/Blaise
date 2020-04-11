using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Blaise.CodeAnalysis.Text;

namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        private readonly SourceText _source;

        public SyntaxTree(SourceText source, SyntaxElement root, SyntaxToken endOfFileToken, ImmutableArray<Diagnostic> messages)
        {
            _source = source;
            Root = root;
            EndOfFileToken = endOfFileToken;
            Messages = messages;
        }

        public SyntaxElement Root { get; }
        public SyntaxToken EndOfFileToken { get; }
        public ImmutableArray<Diagnostic> Messages { get; }
        public SourceText Source => _source;

        public static SyntaxTree ParseTree(string source)
        {
            return ParseTree(SourceText.FromSource(source));
        }
        public static SyntaxTree ParseTree(SourceText source)
        {
            var parser = new ElementParser(source);
            return parser.ParseTree();
        }

        // Added for testing support
        public static IEnumerable<SyntaxToken> ParseTokens(string source)
        {
            return ParseTokens(SourceText.FromSource(source));
        }
        public static IEnumerable<SyntaxToken> ParseTokens(SourceText source)
        {
            var analyzer = new LexicalAnalyzer(source);
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