using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.CodeAnalysis.Binding;

namespace Blaise.CodeAnalysis
{

    internal sealed class SyntaxEvaluator
    {
        private readonly BoundStatement _root;
        private readonly Dictionary<SymbolEntry, object> _variableTable;
        private object _lastValue;

        public SyntaxEvaluator(BoundStatement root, Dictionary<SymbolEntry, object> variableTable)
        {
            _root = root;
            _variableTable = variableTable;
        }

        public object Evaluate()
        {
            EvaluateStatement(_root);
            return _lastValue;
        }

        private void EvaluateStatement(BoundStatement statement)
        {
            switch (statement.Kind)
            {
                case BoundNodeKind.VarStatement:
                    EvaluateVarStatement((BoundVarStatement)statement);
                    break;
                case BoundNodeKind.BlockStatement:
                    EvaluateBlockStatement((BoundBlockStatement)statement);
                    break;
                case BoundNodeKind.ExpressionStatement:
                    EvaluateExpressionStatement((BoundExpressionStatement)statement);
                    break;
                default:
                    throw new ArgumentException($"Unexpected statement type {statement.Kind}.");
            }
        }

        private void EvaluateVarStatement(BoundVarStatement statement)
        {
            _variableTable[statement.Symbol] = null;
        }

        private void EvaluateBlockStatement(BoundBlockStatement statement)
        {
            foreach (var expressionStatement in statement.Statements)
            {
                EvaluateStatement(expressionStatement);
            }
        }

        private void EvaluateExpressionStatement(BoundExpressionStatement statement)
        {
            _lastValue = EvaluateExpression(statement.Expression);
        }

        private object EvaluateExpression(BoundExpression expression)
        {
            switch (expression.Kind)
            {
                case BoundNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((BoundLiteralExpression)expression);
                case BoundNodeKind.VariableExpression:
                    return EvaluateVariableExpression((BoundVariableExpression)expression);
                case BoundNodeKind.AssignmentExpression:
                    return EvaluateAssignmentExpression((BoundAssignmentExpression)expression);
                case BoundNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((BoundUnaryExpression)expression);
                case BoundNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((BoundBinaryExpression)expression);
                default:
                    throw new ArgumentException($"Unexpected expression type {expression.Kind}.");
            }
        }
        private static object EvaluateLiteralExpression(BoundLiteralExpression literalExpression)
        {
            return literalExpression.BoundValue;
        }
        private object EvaluateVariableExpression(BoundVariableExpression variableExpression)
        {
            var symbolMatch = _variableTable.Keys.First(s => s.SymbolName == variableExpression.Symbol.SymbolName);
            return _variableTable[symbolMatch];
        }
        private object EvaluateAssignmentExpression(BoundAssignmentExpression assignmentExpression)
        {
            var value = EvaluateExpression(assignmentExpression.BoundExpression);
            _variableTable[assignmentExpression.Symbol] = value;
            return value;
        }
        private object EvaluateUnaryExpression(BoundUnaryExpression unaryExpression)
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
        private object EvaluateBinaryExpression(BoundBinaryExpression binaryExpression)
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
    }

}