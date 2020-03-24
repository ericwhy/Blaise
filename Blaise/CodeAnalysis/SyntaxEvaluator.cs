using System;
using System.Collections.Generic;
using Blaise.CodeAnalysis.Binding;

namespace Blaise.CodeAnalysis
{

    internal sealed class SyntaxEvaluator
    {
        private readonly BoundExpression _root;
        private readonly Dictionary<string, object> _variableTable;

        public SyntaxEvaluator(BoundExpression root, Dictionary<string, object> variableTable)
        {
            _root = root;
            _variableTable = variableTable;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression expression)
        {
            if (expression is BoundLiteralExpression literalExpression)
            {
                return literalExpression.BoundValue;
            }
            if (expression is BoundVariableExpression variableExpression)
            {
                return _variableTable[variableExpression.VariableName];
            }
            if (expression is BoundAssignmentExpression assignmentExpression)
            {
                var value = EvaluateExpression(assignmentExpression.BoundExpression);
                _variableTable[assignmentExpression.IdentifierName] = value;
                return value;
            }
            if (expression is BoundUnaryExpression unaryExpression)
            {
                var operand = EvaluateExpression(unaryExpression.OperandExpression);
                switch (unaryExpression.BoundOperator.OperatorKind)
                {
                    case BoundUnaryOperatorKind.ArithmeticIdentity:
                        return (int)operand;
                    case BoundUnaryOperatorKind.ArithmeticNegation:
                        return -(int)operand;
                    case BoundUnaryOperatorKind.LogicalNegation:
                        return !(bool)operand;
                    default:
                        throw new ArgumentException($"ERROR: Unexpected unary operator {unaryExpression.BoundOperator.OperatorKind}");
                }
            }
            if (expression is BoundBinaryExpression binaryExpression)
            {
                var leftOperand = EvaluateExpression(binaryExpression.LeftExpression);
                var rightOperand = EvaluateExpression(binaryExpression.RightExpression);

                switch (binaryExpression.BoundOperator.OperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (int)leftOperand + (int)rightOperand;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int)leftOperand - (int)rightOperand;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int)leftOperand * (int)rightOperand;
                    case BoundBinaryOperatorKind.Division:
                        return (int)leftOperand / (int)rightOperand;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool)leftOperand && (bool)rightOperand;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool)leftOperand || (bool)rightOperand;
                    case BoundBinaryOperatorKind.Equals:
                        return Equals(leftOperand, rightOperand);
                    case BoundBinaryOperatorKind.NotEquals:
                        return !Equals(leftOperand, rightOperand);
                    default:
                        throw new ArgumentException($"Unexpected binary operator {binaryExpression.BoundOperator.OperatorKind}.");
                }
            }
            throw new ArgumentException($"Unexpected expression type {expression.Kind}.");
        }

    }

}