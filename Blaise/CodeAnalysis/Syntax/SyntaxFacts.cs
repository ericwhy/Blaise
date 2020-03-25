using System;

namespace Blaise.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                case SyntaxKind.LiteralNotToken:
                    return 6;
                default:
                    return 0;
            }
        }
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.SplatToken:
                case SyntaxKind.SlashToken:
                    return 5;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;
                case SyntaxKind.EqualsToken:
                case SyntaxKind.LtGtToken:
                    return 3;
                case SyntaxKind.AmpersandAmpersandToken:
                case SyntaxKind.LiteralAndToken:
                    return 2;
                case SyntaxKind.PipePipeToken:
                case SyntaxKind.LiteralOrToken:
                    return 1;
                default:
                    return 0;
            }
        }

        public static SyntaxKind GetLiteralKind(string token)
        {
            switch (token.ToLower())
            {
                case "and":
                    return SyntaxKind.LiteralAndToken;
                case "or":
                    return SyntaxKind.LiteralOrToken;
                case "not":
                    return SyntaxKind.LiteralNotToken;
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
    }

}