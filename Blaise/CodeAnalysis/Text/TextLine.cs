namespace Blaise.CodeAnalysis.Text
{
    public sealed class TextLine
    {
        public TextLine(SourceText text, int start, int length, int lengthWithLineBreaks)
        {
            Text = text;
            Start = start;
            Length = length;
            LengthWithLineBreaks = lengthWithLineBreaks;
        }

        public SourceText Text { get; }
        public int Start { get; }
        public int Length { get; }
        public int LengthWithLineBreaks { get; }
        public int End => Start + Length;
        public TextSpan TextSpan => new TextSpan(Start, Length);
        public TextSpan TextSpanWithLineBreaks => new TextSpan(Start, LengthWithLineBreaks);
        public override string ToString() => Text.ToString(TextSpan);
    }
}