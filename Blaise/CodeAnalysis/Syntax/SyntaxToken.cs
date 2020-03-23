using System.Collections.Generic;
using System.Linq;

namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class SyntaxToken : SyntaxElement
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
        public TextSpan TextSpan => new TextSpan(Position, Text.Length);

        public override IEnumerable<SyntaxElement> GetChildElements()
        {
            return Enumerable.Empty<SyntaxElement>();
        }
    }

}