using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.CodeAnalysis;
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
                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }
                if (!syntaxTree.Messages.Any())
                {
                    var evaluator = new SyntaxEvaluator(syntaxTree.Root);
                    var result = evaluator.Evaluate();
                    Console.WriteLine($"Result := {result}");

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach (var message in syntaxTree.Messages)
                    {
                        Console.WriteLine(message);
                    }
                    Console.ResetColor();
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
