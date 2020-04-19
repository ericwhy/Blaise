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
                case SyntaxKind.NotKeyword:
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
                case SyntaxKind.AndKeyword:
                    return 2;
                case SyntaxKind.PipePipeToken:
                case SyntaxKind.OrKeyword:
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
                    return SyntaxKind.AndKeyword;
                case "or":
                    return SyntaxKind.OrKeyword;
                case "not":
                    return SyntaxKind.NotKeyword;
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;
                case "begin":
                    return SyntaxKind.BeginKeyword;
                case "end":
                    return SyntaxKind.EndKeyword;
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
                case SyntaxKind.OpenBraceToken:
                    return "{";
                case SyntaxKind.CloseBraceToken:
                    return "}";
                case SyntaxKind.SemicolonToken:
                    return ";";
                case SyntaxKind.AndKeyword:
                    return "and";
                case SyntaxKind.OrKeyword:
                    return "or";
                case SyntaxKind.NotKeyword:
                    return "not";
                case SyntaxKind.BeginKeyword:
                    return "begin";
                case SyntaxKind.EndKeyword:
                    return "end";
                default:
                    return null;
            }
        }
    }

}