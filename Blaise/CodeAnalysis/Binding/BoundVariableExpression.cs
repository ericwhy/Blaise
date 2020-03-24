using System;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundVariableExpression : BoundExpression
    {
        public BoundVariableExpression(string variableName, Type boundType)
        {
            VariableName = variableName;
            BoundType = boundType;
        }

        public string VariableName { get; }

        public override Type BoundType { get; }

        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
    }
}