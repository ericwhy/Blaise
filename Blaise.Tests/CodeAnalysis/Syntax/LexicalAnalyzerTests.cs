using System;
using System.Collections.Generic;
using Blaise.CodeAnalysis.Syntax;
using Xunit;
using System.Linq;

namespace Blaise.Tests.CodeAnalysis.Syntax
{
    public class LexicalAnalyzerTests
    {
        [Theory]
        [MemberData(nameof(TokensData))]
        public void Lexer_Analyzes_Token(SyntaxKind kind, string text)
        {
            var tokens = SyntaxTree.ParseTokens(text);
            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }
        [Theory]
        [MemberData(nameof(GetTokenPairsData))]
        public void Lexer_Analyzes_TokenPair(SyntaxKind firstKind, string firstText, SyntaxKind secondKind, string secondText)
        {
            var text = firstText + secondText;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();
            Assert.Equal(2, tokens.Length);
            Assert.Equal(tokens[0].Kind, firstKind);
            Assert.Equal(tokens[0].Text, firstText);
            Assert.Equal(tokens[1].Kind, secondKind);
            Assert.Equal(tokens[1].Text, secondText);
        }
        [Theory]
        [MemberData(nameof(GetTokenPairsWithSeparatorsData))]
        public void Lexer_Analyzes_TokenPairWithSeparator(SyntaxKind firstKind, string firstText,
                                                          SyntaxKind secondKind, string secondText,
                                                          SyntaxKind separatorKind, string separatorText)
        {
            var text = firstText + separatorText + secondText;
            var tokens = SyntaxTree.ParseTokens(text).ToArray();
            Assert.Equal(3, tokens.Length);
            Assert.Equal(tokens[0].Kind, firstKind);
            Assert.Equal(tokens[0].Text, firstText);
            Assert.Equal(tokens[1].Kind, separatorKind);
            Assert.Equal(tokens[1].Text, separatorText);
            Assert.Equal(tokens[2].Kind, secondKind);
            Assert.Equal(tokens[2].Text, secondText);
        }
        public static IEnumerable<object[]> TokensData => (from t in Tokens
                                                           select new object[] { t.kind, t.text })
                                                          .Union
                                                          (from t in Separators
                                                           select new object[] { t.kind, t.text });
        public static IEnumerable<object[]> GetTokenPairsData =>
            from tokenPair in GetTokenPairs()
            select new object[] { tokenPair.firstKind, tokenPair.firstText, tokenPair.secondKind, tokenPair.secondText };
        public static IEnumerable<object[]> GetTokenPairsWithSeparatorsData =>
            from tokenPair in GetTokenPairsWithSeparators()
            select new object[] { tokenPair.firstKind, tokenPair.firstText, tokenPair.secondKind, tokenPair.secondText, tokenPair.separatorKind, tokenPair.separatorText };
        private static IEnumerable<(SyntaxKind firstKind, string firstText, SyntaxKind secondKind, string secondText)> GetTokenPairs()
        {
            foreach (var firstToken in Tokens)
            {
                foreach (var secondToken in Tokens)
                {
                    if (!RequiresSeparator(firstToken.kind, secondToken.kind))
                    {
                        yield return (firstToken.kind, firstToken.text, secondToken.kind, secondToken.text);
                    }
                }
            }
        }
        private static IEnumerable<(SyntaxKind firstKind, string firstText,
                                    SyntaxKind secondKind, string secondText,
                                    SyntaxKind separatorKind, string separatorText)>
                                    GetTokenPairsWithSeparators()
        {
            foreach (var firstToken in Tokens)
            {
                foreach (var secondToken in Tokens)
                {
                    if (RequiresSeparator(firstToken.kind, secondToken.kind))
                    {
                        foreach (var separatorToken in Separators)
                        {
                            yield return (firstToken.kind, firstToken.text, secondToken.kind, secondToken.text, separatorToken.kind, separatorToken.text);
                        }
                    }
                }
            }
        }
        private static IEnumerable<(SyntaxKind kind, string text)> Tokens => new[]
        {
            (SyntaxKind.IntegerToken, "1"),
            (SyntaxKind.IntegerToken, "12"),
            (SyntaxKind.IntegerToken, "123"),

            (SyntaxKind.PlusToken, "+"),
            (SyntaxKind.MinusToken, "-"),
            (SyntaxKind.SplatToken, "*"),
            (SyntaxKind.SlashToken, "/"),
            (SyntaxKind.BangToken, "!"),
            (SyntaxKind.ColonEqualsToken, ":="),
            (SyntaxKind.ColonToken, ":"),
            (SyntaxKind.AmpersandAmpersandToken, "&&"),
            (SyntaxKind.PipePipeToken, "||"),
            (SyntaxKind.EqualsToken, "="),
            (SyntaxKind.LtGtToken, "<>"),
            (SyntaxKind.BangEqualsToken, "!="),
            (SyntaxKind.OpenParensToken, "("),
            (SyntaxKind.CloseParensToken, ")"),
            (SyntaxKind.LiteralAndToken, "and"),
            (SyntaxKind.LiteralOrToken, "or"),
            (SyntaxKind.LiteralNotToken, "not"),

            (SyntaxKind.IdentifierToken, "a"),
            (SyntaxKind.IdentifierToken, "cat"),
            (SyntaxKind.IdentifierToken, "dog"),

            (SyntaxKind.TrueKeyword, "true"),
            (SyntaxKind.FalseKeyword, "false"),
        };
        private static IEnumerable<(SyntaxKind kind, string text)> Separators => new[]
        {
            (SyntaxKind.WhitespaceToken, " "),
            (SyntaxKind.WhitespaceToken, "  "),
            (SyntaxKind.WhitespaceToken, "\r"),
            (SyntaxKind.WhitespaceToken, "\n"),
            (SyntaxKind.WhitespaceToken, "\r\n"),
            (SyntaxKind.WhitespaceToken, "\t"),
            (SyntaxKind.WhitespaceToken, "\t\r\n"),
        };
        private static bool RequiresSeparator(SyntaxKind firstKind, SyntaxKind secondKind)
        {
            var firstKindIsKeyword = firstKind.ToString().EndsWith("Keyword");
            var secondKindIsKeyword = secondKind.ToString().EndsWith("Keyword");
            var firstKindIsLiteralOperator = firstKind.ToString().StartsWith("Literal");
            var secondKindIsLiteralOperator = secondKind.ToString().StartsWith("Literal");

            if (firstKind == SyntaxKind.IdentifierToken && secondKind == SyntaxKind.IdentifierToken)
                return true;
            if (firstKindIsKeyword && secondKindIsKeyword)
                return true;
            if (firstKindIsKeyword && secondKind == SyntaxKind.IdentifierToken)
                return true;
            if (firstKind == SyntaxKind.IdentifierToken && secondKindIsKeyword)
                return true;
            if (firstKindIsLiteralOperator && secondKindIsLiteralOperator)
                return true;
            if (firstKindIsLiteralOperator && secondKind == SyntaxKind.IdentifierToken)
                return true;
            if (firstKind == SyntaxKind.IdentifierToken && secondKindIsLiteralOperator)
                return true;
            if (firstKindIsLiteralOperator && secondKindIsKeyword)
                return true;
            if (firstKindIsKeyword && secondKindIsLiteralOperator)
                return true;
            if (firstKind == SyntaxKind.IntegerToken && secondKind == SyntaxKind.IntegerToken)
                return true;
            if (firstKind == SyntaxKind.ColonToken && secondKind == SyntaxKind.EqualsToken)
                return true;
            if (firstKind == SyntaxKind.BangToken && secondKind == SyntaxKind.EqualsToken)
                return true;
            return false;
        }
    }
}
