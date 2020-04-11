using System;
using System.Collections.Immutable;

namespace Blaise.CodeAnalysis.Text
{
    public sealed class SourceText
    {
        private readonly string _text;

        private SourceText(string text)
        {
            Lines = ParseLines(this, text);
            _text = text;
        }
        public int GetLineIndex(int position)
        {
            int lowerIndex = 0;
            int nextIndex = 0;
            int upperIndex = Lines.Length;
            while (lowerIndex <= upperIndex)
            {
                nextIndex = lowerIndex + (upperIndex - lowerIndex) / 2;
                var line = Lines[nextIndex];
                if (line.Start <= position && position <= line.End) // did we find the line that contains position?
                {
                    break;
                }
                if (position < line.Start) // Is it in the lower half?
                {
                    upperIndex = nextIndex;
                }
                else // it must be in the upper half
                {
                    lowerIndex = nextIndex;
                }
            }
            return nextIndex;
        }
        private static ImmutableArray<TextLine> ParseLines(SourceText source, string text)
        {
            var result = ImmutableArray.CreateBuilder<TextLine>();
            var lineStart = 0;
            var position = 0;
            while (position < text.Length)
            {
                var lineBreakWidth = GetLineBreakWidth(text, position);
                if (lineBreakWidth == 0)
                {
                    position++;
                }
                else
                {
                    AddLine(lineBreakWidth);
                    position += lineBreakWidth;
                    lineStart = position;
                }
            }
            if (position > lineStart)
            {
                AddLine(0);
            }
            return result.ToImmutable();

            void AddLine(int lineBreakWidth)
            {
                int lineLength = position - lineStart;
                var line = new TextLine(source, lineStart, lineLength, lineLength + lineBreakWidth);
                result.Add(line);
            }
            int GetLineBreakWidth(string line, int pos)
            {
                var ch = line[pos];
                var ch2 = pos + 1 >= line.Length ? '\0' : line[pos + 1];
                if (ch == '\r' && ch2 == '\n')
                {
                    return 2;
                }
                if (ch == '\r' || ch == '\n')
                {
                    return 1;
                }
                return 0;
            }
        }
        public static SourceText FromSource(string text)
        {
            return new SourceText(text);
        }
        public char this[int index] => _text[index];
        public int Length => _text.Length;
        public ImmutableArray<TextLine> Lines { get; }
        public override string ToString() => _text;
        public string ToString(int start, int length) => _text.Substring(start, length);
        public string ToString(TextSpan textSpan) => _text.Substring(textSpan.Start, textSpan.Length);
        public string ToString(int lineNo) => _text.Substring(Lines[lineNo].Start, Lines[lineNo].Length);
    }
}