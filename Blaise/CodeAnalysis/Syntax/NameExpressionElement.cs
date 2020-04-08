using System.Collections.Generic;

namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class NameExpressionElement : ExpressionElement
    {
        public NameExpressionElement(SyntaxToken identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public SyntaxToken IdentifierToken { get; }

        public override SyntaxKind Kind => SyntaxKind.NameExpression;
    }
}