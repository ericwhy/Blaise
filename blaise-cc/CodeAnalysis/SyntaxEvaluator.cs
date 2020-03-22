using System;

namespace Blaise.CodeAnalysis
{

    public sealed class SyntaxEvaluator
    {
        private readonly SyntaxElement _root;

        public SyntaxEvaluator(SyntaxElement root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(SyntaxElement expression)
        {
            if (expression is LiteralExpressionElement integerExpression)
            {
                return (int)integerExpression.LiteralToken.Value;
            }
            if (expression is BinaryExpressionElement binaryExpression)
            {
                var leftOperand = EvaluateExpression(binaryExpression.LeftExpression);
                var rightOperand = EvaluateExpression(binaryExpression.RightExpression);

                switch (binaryExpression.OperatorElement.Kind)
                {
                    case SyntaxKind.PlusToken:
                        return leftOperand + rightOperand;
                    case SyntaxKind.MinusToken:
                        return leftOperand - rightOperand;
                    case SyntaxKind.SplatToken:
                        return leftOperand * rightOperand;
                    case SyntaxKind.SlashToken:
                        return leftOperand / rightOperand;
                    default:
                        throw new ArgumentException($"Unexpected binary operator {binaryExpression.OperatorElement.Kind}.");
                }
            }
            if (expression is ParentheticalExpressionElement parentheticalExpression)
            {
                return EvaluateExpression(parentheticalExpression.Expression);
            }
            throw new ArgumentException($"Unexpected expression type {expression.Kind}.");
        }

    }

}