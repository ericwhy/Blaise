using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(string identifierName, BoundExpression boundExpression)
        {
            IdentifierName = identifierName;
            BoundExpression = boundExpression;
        }

        public override Type BoundType => BoundExpression.BoundType;

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;

        public string IdentifierName { get; }
        public BoundExpression BoundExpression { get; }
    }
}