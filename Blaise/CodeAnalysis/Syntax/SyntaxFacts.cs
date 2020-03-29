using System;
using System.Collections.Generic;
using System.Linq;

namespace Blaise.CodeAnalysis.Syntax
{
    public static class SyntaxFacts
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
                case SyntaxKind.BangEqualsToken:
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
        public static IEnumerable<SyntaxKind> UnaryOperatorKinds
        {
            get
            {
                var syntaxKinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));
                return from syntaxKind in syntaxKinds
                       where GetUnaryOperatorPrecedence(syntaxKind) > 0
                       select syntaxKind;
            }
        }
        public static IEnumerable<SyntaxKind> BinaryOperatorKinds
        {
            get
            {
                var syntaxKinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));
                return from syntaxKind in syntaxKinds
                       where GetBinaryOperatorPrecedence(syntaxKind) > 0
                       select syntaxKind;
            }
        }
        public static string GetSyntaxText(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                    return "+";
                case SyntaxKind.MinusToken:
                    return "-";
                case SyntaxKind.SplatToken:
                    return "*";
                case SyntaxKind.SlashToken:
                    return "/";
                case SyntaxKind.BangToken:
                    return "!";
                case SyntaxKind.ColonEqualsToken:
                    return ":=";
                case SyntaxKind.ColonToken:
                    return ":";
                case SyntaxKind.AmpersandAmpersandToken:
                    return "&&";
                case SyntaxKind.PipePipeToken:
                    return "||";
                case SyntaxKind.EqualsToken:
                    return "=";
                case SyntaxKind.LtGtToken:
                    return "<>";
                case SyntaxKind.BangEqualsToken:
                    return "!=";
                case SyntaxKind.OpenParensToken:
                    return "(";
                case SyntaxKind.CloseParensToken:
                    return ")";
                case SyntaxKind.LiteralAndToken:
                    return "and";
                case SyntaxKind.LiteralOrToken:
                    return "or";
                case SyntaxKind.LiteralNotToken:
                    return "not";
                default:
                    return null;
            }
        }
    }

}