using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blaise.CodeAnalysis;
using Blaise.CodeAnalysis.Binding;
using Blaise.CodeAnalysis.Syntax;
using Blaise.CodeAnalysis.Text;

namespace Blaise
{
    internal static class Program
    {
        private static void Main()
        {
            var variableTable = new Dictionary<SymbolEntry, object>();
            bool showTree = false;
            StringBuilder inputLines = new StringBuilder();
            while (true)
            {
                var isNew = inputLines.Length == 0;
                if (isNew)
                    Console.Write(": ");
                else
                    Console.Write("+ ");
                var inputLine = Console.ReadLine();
                var isEoi = string.IsNullOrWhiteSpace(inputLine);
                if (isNew)
                {
                    if (isEoi)
                        break;
                    if (inputLine.Equals("!showTree"))
                    {
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Display of tree is enabled." : "Display of tree is disabled.");
                        continue;
                    }
                    if (inputLine.Equals("!cls"))
                    {
                        Console.Clear();
                        continue;
                    }
                }
                inputLines.AppendLine(inputLine);
                var source = inputLines.ToString();
                var syntaxTree = SyntaxTree.ParseTree(source);
                if (!isEoi && syntaxTree.Messages.Any())
                    continue;
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variableTable);
                var messages = result.Messages;

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    syntaxTree.Root.WriteTo(Console.Out);
                    Console.ResetColor();
                }
                if (!messages.Any())
                {
                    Console.WriteLine($"Result := {result.Value}");
                }
                else
                {
                    foreach (var message in messages)
                    {
                        int lineIndex = syntaxTree.Source.GetLineIndex(message.Span.Start);
                        TextLine textLine = syntaxTree.Source.Lines[lineIndex];
                        int messageStartInLine = message.Span.Start - textLine.Start + 1;
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write($"({lineIndex + 1}, {messageStartInLine}): ");
                        Console.WriteLine(message);
                        Console.ResetColor();

                        var prefix = syntaxTree.Source.ToString(TextSpan.FromBounds(textLine.Start, message.Span.Start));
                        var error = syntaxTree.Source.ToString(message.Span);
                        var suffix = syntaxTree.Source.ToString(TextSpan.FromBounds(message.Span.End, textLine.End));

                        Console.Write("    ");
                        Console.Write(prefix);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();
                        Console.WriteLine(suffix);
                    }
                    Console.WriteLine();
                }
                inputLines.Clear();
            }
        }
    }

}
