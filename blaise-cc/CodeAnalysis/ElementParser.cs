using System.Collections.Generic;

namespace Blaise.CodeAnalysis
{
    class ElementParser
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
                var expression = ParseExpression();
                var closeParens = MatchTokenKind(SyntaxKind.CloseParensToken);
                return new ParentheticalExpressionElement(openParens, expression, closeParens);
            }
            var primaryToken = MatchTokenKind(SyntaxKind.IntegerToken);
            return new IntegerExpressionElement(primaryToken);
        }

        private ExpressionElement ParseExpression()
        {
            return ParseTermElement();
        }
        private ExpressionElement ParseTermElement()
        {
            var left = ParseFactorElement();
            while (Current.Kind == SyntaxKind.PlusToken || Current.Kind == SyntaxKind.MinusToken)
            {
                var operatorToken = NextToken();
                var right = ParseFactorElement();
                left = new BinaryExpressionElement(left, operatorToken, right);
            }
            return left;
        }

        private ExpressionElement ParseFactorElement()
        {
            var left = ParsePrimaryExpression();
            while (Current.Kind == SyntaxKind.SplatToken || Current.Kind == SyntaxKind.SlashToken)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionElement(left, operatorToken, right);
            }
            return left;
        }

        public SyntaxTree ParseTree()
        {
            var element = ParseTermElement();
            var endOfFileToken = MatchTokenKind(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(element, endOfFileToken, Messages);
        }
        public List<string> Messages => _messages;
    }

}