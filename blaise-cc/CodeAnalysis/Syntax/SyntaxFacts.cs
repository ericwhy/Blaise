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
                    return 2;
                case SyntaxKind.PipePipeToken:
                    return 1;
                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string token)
        {
            switch (token)
            {
                case "True":
                    return SyntaxKind.TrueKeyword;
                case "False":
                    return SyntaxKind.FalseKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
    }

}