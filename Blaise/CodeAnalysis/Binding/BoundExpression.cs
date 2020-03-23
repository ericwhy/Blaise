using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type BoundType { get; }
    }
}