using System.Collections.Generic;
using Blaise.CodeAnalysis.Text;

namespace Blaise.CodeAnalysis.Syntax
{
    internal sealed class LexicalAnalyzer
    {
        private readonly SourceText _source;
        private int _scanPosition;
        private int _startToken;
        private string _currentToken;
        private SyntaxKind CurrentKind { get; set; }
        private object CurrentValue { get; set; }
        private DiagnosticBag _messages = new DiagnosticBag();

        public LexicalAnalyzer(SourceText source)
        {
            _source = source;
        }

        private char Current => Peek();
        private char Lookahead => Peek(1);

        private char Peek(int offset = 0)
        {
            var charOffset = _scanPosition + offset;
            if (charOffset >= _source.Length)
            {
                return '\0';
            }
            return _source[charOffset];
        }

        private int MoveNext(int charsToMove = 1)
        {
            var currentPosition = _scanPosition;
            _scanPosition += charsToMove;
            if (_scanPosition > _source.Length)
                _scanPosition = _source.Length;
            return currentPosition;
        }

        private int StartTextFragment()
        {
            _startToken = _scanPosition;
            _currentToken = string.Empty;
            return _startToken;
        }
        private string GetTextFragment()
        {
            if (string.IsNullOrEmpty(_currentToken))
            {
                _currentToken = _source.ToString(_startToken, _scanPosition - _startToken);
            }
            return _currentToken;
        }
        private bool IsEndOfFile => _scanPosition >= _source.Length;
        public SyntaxToken NextToken()
        {
            var startPosition = StartTextFragment();
            CurrentKind = SyntaxKind.BadToken;
            CurrentValue = null;
            switch (Current)
            {
                case '\0':
                    CurrentKind = SyntaxKind.EndOfFileToken;
                    _ = MoveNext();
                    break;
                case '+':
                    CurrentKind = SyntaxKind.PlusToken;
                    _ = MoveNext();
                    break;
                case '-':
                    CurrentKind = SyntaxKind.MinusToken;
                    _ = MoveNext();
                    break;
                case '*':
                    CurrentKind = SyntaxKind.SplatToken;
                    _ = MoveNext();
                    break;
                case '/':
                    CurrentKind = SyntaxKind.SlashToken;
                    _ = MoveNext();
                    break;
                case '(':
                    CurrentKind = SyntaxKind.OpenParensToken;
                    _ = MoveNext();
                    break;
                case ')':
                    CurrentKind = SyntaxKind.CloseParensToken;
                    _ = MoveNext();
                    break;
                case '!':
                    _ = MoveNext();
                    if (Current != '=')
                    {
                        CurrentKind = SyntaxKind.BangToken;
                    }
                    else
                    {
                        CurrentKind = SyntaxKind.BangEqualsToken;
                        _ = MoveNext();
                    }
                    break;
                case '{':
                    CurrentKind = SyntaxKind.OpenBraceToken;
                    _ = MoveNext();
                    break;
                case '}':
                    CurrentKind = SyntaxKind.CloseBraceToken;
                    _ = MoveNext();
                    break;
                case '=':
                    CurrentKind = SyntaxKind.EqualsToken;
                    _ = MoveNext();
                    break;
                case ':':
                    _ = MoveNext();
                    if (Current != '=')
                    {
                        CurrentKind = SyntaxKind.ColonToken;
                    }
                    else
                    {
                        CurrentKind = SyntaxKind.ColonEqualsToken;
                        _ = MoveNext();
                    }
                    break;
                case '&':
                    _ = MoveNext();
                    if (Current == '&')
                    {
                        CurrentKind = SyntaxKind.AmpersandAmpersandToken;
                        _ = MoveNext();
                    }
                    break;
                case '|':
                    _ = MoveNext();
                    if (Current == '|')
                    {
                        CurrentKind = SyntaxKind.PipePipeToken;
                        _ = MoveNext();
                    }
                    break;
                case '<':
                    _ = MoveNext();
                    if (Current == '>')
                    {
                        CurrentKind = SyntaxKind.LtGtToken;
                        _ = MoveNext();
                    }
                    break;
                case ';':
                    CurrentKind = SyntaxKind.SemicolonToken;
                    _ = MoveNext();
                    break;
                default:
                    if (char.IsDigit(Current))
                    {
                        ReadInteger(startPosition);
                    }
                    else if (char.IsWhiteSpace(Current))
                    {
                        ReadWhitespace();
                    }
                    else if (char.IsLetter(Current))
                    {
                        ReadLiteral();
                    }
                    else
                    {
                        _ = MoveNext();
                        string errorCh = GetTextFragment();
                        Messages.ReportInvalidCharacter(startPosition, errorCh);
                    }
                    break;
            }
            return new SyntaxToken(CurrentKind, startPosition, GetTextFragment(), CurrentValue);
        }

        private void ReadLiteral()
        {
            while (char.IsLetter(Current))
            {
                _ = MoveNext();
            }
            CurrentKind = SyntaxFacts.GetLiteralKind(GetTextFragment());
        }

        private void ReadWhitespace()
        {
            while (char.IsWhiteSpace(Current))
            {
                _ = MoveNext();
            }
            CurrentKind = SyntaxKind.WhitespaceToken;
        }

        private void ReadInteger(int startsAt)
        {
            while (char.IsDigit(Current))
            {
                _ = MoveNext();
            }
            var token = GetTextFragment();
            if (!int.TryParse(token, out var tokenValue))
            {
                Messages.ReportInvalidInteger(new TextSpan(startsAt, token.Length), token, typeof(int));
            }
            CurrentValue = tokenValue;
            CurrentKind = SyntaxKind.IntegerToken;
        }

        public DiagnosticBag Messages => _messages;
    }

}