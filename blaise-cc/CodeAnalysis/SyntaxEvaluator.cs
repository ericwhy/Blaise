using System;
using Blaise.CodeAnalysis.Binding;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis
{

    internal sealed class SyntaxEvaluator
    {
        private readonly BoundExpression _root;

        public SyntaxEvaluator(BoundExpression root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(BoundExpression expression)
        {
            if (expression is BoundLiteralExpression literalExpression)
            {
                return (int)literalExpression.BoundValue;
            }
            if (expression is BoundUnaryExpression unaryExpression)
            {
                var operand = EvaluateExpression(unaryExpression.OperandExpression);
                switch (unaryExpression.OperatorKind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -operand;
                    default:
                        throw new ArgumentException($"ERROR: Unexpected unary operator {unaryExpression.OperatorKind}");
                }
            }
            if (expression is BoundBinaryExpression binaryExpression)
            {
                var leftOperand = EvaluateExpression(binaryExpression.LeftExpression);
                var rightOperand = EvaluateExpression(binaryExpression.RightExpression);

                switch (binaryExpression.OperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return leftOperand + rightOperand;
                    case BoundBinaryOperatorKind.Subtraction:
                        return leftOperand - rightOperand;
                    case BoundBinaryOperatorKind.Multiplication:
                        return leftOperand * rightOperand;
                    case BoundBinaryOperatorKind.Division:
                        return leftOperand / rightOperand;
                    default:
                        throw new ArgumentException($"Unexpected binary operator {binaryExpression.OperatorKind}.");
                }
            }
            throw new ArgumentException($"Unexpected expression type {expression.Kind}.");
        }

    }

}