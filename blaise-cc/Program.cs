using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.CodeAnalysis;
using Blaise.CodeAnalysis.Binding;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise
{
    internal static class Program
    {
        private static void Main()
        {
            bool showTree = false;
            while (true)
            {
                Console.Write(": ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;
                if (line.Equals("!showTree"))
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Display of tree is enabled." : "Display of tree is disabled.");
                    continue;
                }
                if (line.Equals("!cls"))
                {
                    Console.Clear();
                    continue;
                }
                var syntaxTree = SyntaxTree.ParseTree(line);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate();
                var messages = result.Messages;

                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    PrettyPrint(syntaxTree.Root);
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
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(message);
                        Console.ResetColor();

                        var prefix = line.Substring(0, message.Span.Start);
                        var error = line.Substring(message.Span.Start, message.Span.Length);
                        var suffix = line.Substring(message.Span.End);

                        Console.Write("    ");
                        Console.Write(prefix);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();
                        Console.WriteLine(suffix);
                    }
                    Console.WriteLine();
                }
            }
        }
        private static void PrettyPrint(SyntaxElement element, string indent = "", bool isLast = true)
        {
            // ├──
            // │  
            // └──
            string marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(element.Kind);
            if (element is SyntaxToken t && t.Value != null)
            {
                Console.Write($" {t.Value}");
            }
            Console.WriteLine();

            indent += isLast ? "   " : "│  ";
            var lastChild = element.GetChildElements().LastOrDefault();
            foreach (var child in element.GetChildElements())
            {
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }

}
