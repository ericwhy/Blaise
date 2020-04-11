using System.Collections.Generic;
using System.Collections.Immutable;
using Blaise.CodeAnalysis.Text;

namespace Blaise.CodeAnalysis.Syntax
{
    internal sealed class ElementParser
    {
        private readonly DiagnosticBag _messages = new DiagnosticBag();
        private readonly ImmutableArray<SyntaxToken> _tokens;
        private int _tokenPosition;
        public DiagnosticBag Messages => _messages;
        public ElementParser(SourceText source)
        {
            var tokens = new List<SyntaxToken>();
            var analyzer = new LexicalAnalyzer(source);
            SyntaxToken token;
            do
            {
                token = analyzer.NextToken();
                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);
            _messages.AddRange(analyzer.Messages);
            _tokens = tokens.ToImmutableArray();

        }

        private SyntaxToken PeekToken(int offset)
        {
            var index = _tokenPosition + offset;
            if (index >= _tokens.Length)
            {
                return _tokens[_tokens.Length - 1];
            }
            return _tokens[index];
        }

        private SyntaxToken Current => PeekToken(0);
        private SyntaxToken LookAhead => PeekToken(1);
        private SyntaxToken NextToken()
        {
            var current = Current;
            _tokenPosition++;
            return current;
        }
        private SyntaxToken MatchTokenKind(SyntaxKind kind)
        {
            if (Current.Kind == kind)
            {
                return NextToken();
            }
            Messages.ReportUnexpectedToken(Current.TextSpan, Current.Kind, kind);
            return new SyntaxToken(kind, Current.Position, string.Empty, null);
        }

        private ExpressionElement ParseExpressionElement()
        {
            return ParseAssignmentExpressionElement();
        }
        private ExpressionElement ParseAssignmentExpressionElement()
        {
            if (Current.Kind == SyntaxKind.IdentifierToken &&
                LookAhead.Kind == SyntaxKind.ColonEqualsToken)
            {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var rightExpression = ParseBinaryExpressionElement();
                return new AssignmentExpressionElement(identifierToken, operatorToken, rightExpression);
            }
            return ParseBinaryExpressionElement();
        }
        private ExpressionElement ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpenParensToken:
                    return ParseParentheticalExpression();
                case SyntaxKind.FalseKeyword:
                case SyntaxKind.TrueKeyword:
                    return ParseBooleanLiteral();
                case SyntaxKind.IntegerToken:
                    return ParseIntegerLiteral();
                case SyntaxKind.IdentifierToken:
                default:
                    return ParseNameExpression();
            }
        }

        private ExpressionElement ParseIntegerLiteral()
        {
            var primaryToken = MatchTokenKind(SyntaxKind.IntegerToken);
            return new LiteralExpressionElement(primaryToken);
        }

        private ExpressionElement ParseParentheticalExpression()
        {
            var openParens = MatchTokenKind(SyntaxKind.OpenParensToken);
            var expression = ParseExpressionElement();
            var closeParens = MatchTokenKind(SyntaxKind.CloseParensToken);
            return new ParentheticalExpressionElement(openParens, expression, closeParens);
        }

        private ExpressionElement ParseBooleanLiteral()
        {
            var isTrue = Current.Kind == SyntaxKind.TrueKeyword;
            var keywordToken = isTrue ? MatchTokenKind(SyntaxKind.TrueKeyword)
                : MatchTokenKind(SyntaxKind.FalseKeyword);
            return new LiteralExpressionElement(keywordToken, isTrue);
        }

        private ExpressionElement ParseNameExpression()
        {
            var identifierToken = MatchTokenKind(SyntaxKind.IdentifierToken);
            return new NameExpressionElement(identifierToken);
        }

        private ExpressionElement ParseBinaryExpressionElement(int parentPrecedence = 0)
        {
            ExpressionElement nextExpression;
            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence > 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operandExpression = ParseBinaryExpressionElement(unaryOperatorPrecedence);
                nextExpression = new UnaryExpressionElement(operatorToken, operandExpression);
            }
            else
            {
                nextExpression = ParsePrimaryExpression();

            }
            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                    break;
                var operatorToken = NextToken();
                var rightExpression = ParseBinaryExpressionElement(precedence);
                nextExpression = new BinaryExpressionElement(nextExpression, operatorToken, rightExpression);
            }
            return nextExpression;
        }

        public SyntaxTree ParseTree()
        {
            var element = ParseExpressionElement();
            var endOfFileToken = MatchTokenKind(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(element, endOfFileToken, Messages.ToImmutableArray());
        }
    }

}