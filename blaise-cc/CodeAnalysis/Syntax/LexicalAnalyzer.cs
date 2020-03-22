using System.Collections.Generic;

namespace Blaise.CodeAnalysis.Syntax
{
    internal sealed class LexicalAnalyzer
    {
        private readonly string _scanText;
        private int _scanPosition;
        private int _startToken;
        private List<string> _messages = new List<string>();

        public LexicalAnalyzer(string scanText)
        {
            _scanText = scanText;
        }

        private char Current
        {
            get
            {
                if (_scanPosition >= _scanText.Length)
                {
                    return '\0';
                }
                return _scanText[_scanPosition];
            }
        }

        private int MoveNext()
        {
            return _scanPosition++;
        }

        private int StartTextFragment()
        {
            _startToken = _scanPosition;
            return _startToken;
        }
        private string GetTextFragment()
        {
            return _scanText.Substring(_startToken, _scanPosition - _startToken);
        }
        private bool IsEndOfFile => _scanPosition >= _scanText.Length;
        public SyntaxToken NextToken()
        {
            // Literals:
            //    Numbers
            //  Operators
            //     + - * / ( )
            //  Other
            //    <whitespace>
            //    <endoffile>
            if (IsEndOfFile)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, -1, string.Empty, null);
            }
            int test = 0;
            if (char.IsDigit(Current))
            {
                var startsAt = StartTextFragment();
                while (char.IsDigit(Current))
                {
                    test = MoveNext();
                }
                var token = GetTextFragment();
                if (!int.TryParse(token, out var tokenValue))
                {
                    Messages.Add($"ERROR: The value {token} is not a valid Integer value.");
                }
                return new SyntaxToken(SyntaxKind.IntegerToken, startsAt, token, tokenValue);
            }
            if (char.IsWhiteSpace(Current))
            {
                var startsAt = StartTextFragment();
                while (char.IsWhiteSpace(Current))
                {
                    MoveNext();
                }
                var token = GetTextFragment();
                return new SyntaxToken(SyntaxKind.WhitespaceToken, startsAt, token, null);
            }
            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, MoveNext(), "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, MoveNext(), "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.SplatToken, MoveNext(), "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, MoveNext(), "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParensToken, MoveNext(), "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParensToken, MoveNext(), ")", null);
            }
            // Unexpected token
            int errorPosition = StartTextFragment();
            MoveNext();
            string errorCh = GetTextFragment();
            Messages.Add($"ERROR: Unexpected character '{errorCh}' at position {errorPosition}.");
            return new SyntaxToken(SyntaxKind.BadToken, errorPosition, errorCh, null);
        }

        public List<string> Messages => _messages;
    }

}