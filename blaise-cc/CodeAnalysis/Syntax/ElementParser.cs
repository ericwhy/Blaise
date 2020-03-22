using System.Collections.Generic;

namespace Blaise.CodeAnalysis.Syntax
{
    internal sealed class ElementParser
    {
        private readonly SyntaxToken[] _tokens;
        private int _tokenPosition;
        private List<string> _messages = new List<string>();
        public ElementParser(string text)
        {
            var tokens = new List<SyntaxToken>();
            var analyzer = new LexicalAnalyzer(text);
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
            _tokens = tokens.ToArray();

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
            Messages.Add($"ERROR: Unexpected token <{Current.Kind}> when expecting <{kind}>.");
            return new SyntaxToken(kind, Current.Position, string.Empty, null);
        }

        private ExpressionElement ParsePrimaryExpression()
        {
            if (Current.Kind == SyntaxKind.OpenParensToken)
            {
                var openParens = NextToken();
                var expression = ParseExpressionElement();
                var closeParens = MatchTokenKind(SyntaxKind.CloseParensToken);
                return new ParentheticalExpressionElement(openParens, expression, closeParens);
            }
            var primaryToken = MatchTokenKind(SyntaxKind.IntegerToken);
            return new LiteralExpressionElement(primaryToken);
        }

        private ExpressionElement ParseExpressionElement(int parentPrecedence = 0)
        {
            ExpressionElement nextExpression;
            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence > 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operandExpression = ParseExpressionElement(unaryOperatorPrecedence);
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
                var rightExpression = ParseExpressionElement(precedence);
                nextExpression = new BinaryExpressionElement(nextExpression, operatorToken, rightExpression);
            }
            return nextExpression;
        }

        public SyntaxTree ParseTree()
        {
            var element = ParseExpressionElement();
            var endOfFileToken = MatchTokenKind(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(element, endOfFileToken, Messages);
        }
        public List<string> Messages => _messages;
    }

}