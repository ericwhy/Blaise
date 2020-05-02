using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Blaise.CodeAnalysis.Text;

namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        private readonly SourceText _source;

        private SyntaxTree(SourceText source)
        {
            var parser = new ElementParser(source);
            var root = parser.ParseCompilationUnitElement();
            _source = source;
            Root = root;
            Messages = parser.Messages.ToImmutableArray();
        }

        public CompilationUnitElement Root { get; }
        public ImmutableArray<Diagnostic> Messages { get; }
        public SourceText Source => _source;

        public static SyntaxTree ParseTree(string source)
        {
            return ParseTree(SourceText.FromSource(source));
        }
        public static SyntaxTree ParseTree(SourceText source)
        {
            return new SyntaxTree(source);
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