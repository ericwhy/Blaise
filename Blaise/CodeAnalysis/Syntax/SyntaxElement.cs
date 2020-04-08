using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Blaise.CodeAnalysis.Text;

namespace Blaise.CodeAnalysis.Syntax
{
    public abstract class SyntaxElement
    {
        public abstract SyntaxKind Kind { get; }
        public virtual TextSpan TextSpan
        {
            get
            {
                return TextSpan.FromBounds(GetChildElements().First().TextSpan.Start,
                    GetChildElements().Last().TextSpan.End);
            }
        }
        public IEnumerable<SyntaxElement> GetChildElements()
        {
            var instanceProperties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var instanceProperty in instanceProperties)
            {
                if (typeof(SyntaxElement).IsAssignableFrom(instanceProperty.PropertyType))
                {
                    var childElement = (SyntaxElement)instanceProperty.GetValue(this);
                    yield return childElement;
                }
                else if (typeof(IEnumerable<SyntaxElement>).IsAssignableFrom(instanceProperty.PropertyType))
                {
                    var childElements = (IEnumerable<SyntaxElement>)instanceProperty.GetValue(this);
                    foreach (var childElement in childElements)
                    {
                        yield return childElement;
                    }
                }
            }
        }

        public void WriteTo(TextWriter writer)
        {
            PrettyPrint(writer, this);
        }

        private static void PrettyPrint(TextWriter writer, SyntaxElement element, string indent = "", bool isLast = true)
        {
            // ├──
            // │  
            // └──
            string marker = isLast ? "└──" : "├──";

            writer.Write(indent);
            writer.Write(marker);
            writer.Write(element.Kind);
            if (element is SyntaxToken t && t.Value != null)
            {
                writer.Write($" {t.Value}");
            }
            writer.WriteLine();

            indent += isLast ? "   " : "│  ";
            var lastChild = element.GetChildElements().LastOrDefault();
            foreach (var child in element.GetChildElements())
            {
                PrettyPrint(writer, child, indent, child == lastChild);
            }
        }

        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                PrettyPrint(writer, this);
                return writer.ToString();
            }
        }
    }
}