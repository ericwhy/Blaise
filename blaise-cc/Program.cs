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
            var variableTable = new Dictionary<SymbolEntry, object>();
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
    }

}
