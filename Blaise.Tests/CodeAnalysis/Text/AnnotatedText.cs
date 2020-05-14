using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Blaise.CodeAnalysis.Text;

internal sealed class AnnotatedText
{
    private AnnotatedText(string text, ImmutableArray<TextSpan> textSpans)
    {
        Text = text;
        TextSpans = textSpans;
    }

    public string Text { get; }
    public ImmutableArray<TextSpan> TextSpans { get; }

    public static AnnotatedText ParseText(string text)
    {
        text = text.Unindent(true);
        var textBuilder = new StringBuilder();
        var spanBuilder = ImmutableArray.CreateBuilder<TextSpan>();
        var stack = new Stack<int>();
        var position = 0;
        foreach (var ch in text)
        {
            if (ch == '[')
            {
                stack.Push(position);
            }
            else if (ch == ']')
            {
                if (stack.Count == 0)
                    throw new ArgumentException("Unmatched ']' in input.", nameof(text));
                var start = stack.Pop();
                var textSpan = TextSpan.FromBounds(start, position - 1);
                spanBuilder.Add(textSpan);
            }
            else
            {
                textBuilder.Append(ch);
            }
            _ = position++;
        }
        if (stack.Count > 0)
            throw new ArgumentException("Unmatched '[' in text.", nameof(text));
        return new AnnotatedText(textBuilder.ToString(), spanBuilder.ToImmutable());
    }
}