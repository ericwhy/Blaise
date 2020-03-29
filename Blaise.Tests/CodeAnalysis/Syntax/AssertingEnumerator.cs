using System.Collections.Generic;
using Blaise.CodeAnalysis.Syntax;
using Xunit;
using System;

namespace Blaise.Tests.CodeAnalysis.Syntax
{
    public class AssertingEnumerator : IDisposable
    {
        IEnumerator<SyntaxElement> _elementEnumerator;
        private bool _hasErrors;
        private bool MarkFailed()
        {
            _hasErrors = true;
            return false;
        }
        public AssertingEnumerator(SyntaxElement element)
        {
            _elementEnumerator = FlattenElementTree(element).GetEnumerator();
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!_hasErrors)
                        Assert.False(_elementEnumerator.MoveNext());
                    _elementEnumerator.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
        private static IEnumerable<SyntaxElement> FlattenElementTree(SyntaxElement element)
        {
            var stack = new Stack<SyntaxElement>();
            stack.Push(element);
            while (stack.Count > 0)
            {
                var topElement = stack.Pop();
                yield return topElement;
                foreach (var childElement in topElement.GetChildElements())
                {
                    stack.Push(childElement);
                }
            }
        }
        public void AssertElement(SyntaxKind kind)
        {
            try
            {
                Assert.True(_elementEnumerator.MoveNext());
                Assert.Equal(kind, _elementEnumerator.Current.Kind);
                Assert.IsNotType<SyntaxToken>(_elementEnumerator.Current);
            }
            catch when (MarkFailed())
            {
                throw;
            }
        }
        public void AssertToken(SyntaxKind kind, string text)
        {
            try
            {
                Assert.True(_elementEnumerator.MoveNext());
                Assert.Equal(kind, _elementEnumerator.Current.Kind);
                var token = Assert.IsType<SyntaxToken>(_elementEnumerator.Current);
                Assert.Equal(text, token.Text);
            }
            catch when (MarkFailed())
            {
                throw;
            }
        }
    }
}
