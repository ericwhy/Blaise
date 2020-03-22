using System.Collections.Generic;

namespace Blaise.CodeAnalysis
{
    sealed class ParentheticalExpressionElement : ExpressionElement
    {
        public ParentheticalExpressionElement(SyntaxElement openParensToken, ExpressionElement expression, SyntaxElement closeParensToken)
        {
            OpenParensToken = openParensToken;
            Expression = expression;
            CloseParensToken = closeParensToken;
        }

        public SyntaxElement OpenParensToken { get; }
        public ExpressionElement Expression { get; }
        public SyntaxElement CloseParensToken { get; }

        public override SyntaxKind Kind => SyntaxKind.ParentheticalExpression;

        public override IEnumerable<SyntaxElement> GetChildElements()
        {
            yield return OpenParensToken;
            yield return Expression;
            yield return CloseParensToken;
        }
    }

}