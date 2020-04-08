using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
    }
}