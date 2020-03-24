using System;
using System.Collections.Generic;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed partial class ExpressionBinder
    {
        private readonly Dictionary<string, object> _variableTable;
        private readonly DiagnosticBag _messages = new DiagnosticBag();

        public ExpressionBinder(Dictionary<string, object> variableTable)
        {
            _variableTable = variableTable;
        }

        public DiagnosticBag Messages => _messages;
        public BoundExpression BindExpression(SyntaxElement expression)
        {
            switch (expression.Kind)
            {
                case SyntaxKind.ParentheticalExpression:
                    return BindParentheticalExpression((ParentheticalExpressionElement)expression);
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionElement)expression);
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameExpressionElement)expression);
                case SyntaxKind.AssignmentExpression:
                    return BindAssignmentExpression((AssignmentExpressionElement)expression);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionElement)expression);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionElement)expression);
                default:
                    throw new ArgumentException($"ERROR: Unknown expression type {expression.Kind}.");
            }
        }
        private BoundExpression BindParentheticalExpression(ParentheticalExpressionElement expression)
        {
            return BindExpression(expression.Expression);
        }
        private BoundExpression BindLiteralExpression(LiteralExpressionElement expression)
        {
            var value = expression.Value ?? 0;
            return new BoundLiteralExpression(value);
        }
        private BoundExpression BindNameExpression(NameExpressionElement expression)
        {
            var name = expression.IdentifierToken.Text;
            if (!_variableTable.TryGetValue(name, out var value))
            {
                _messages.ReportUndefinedName(expression.IdentifierToken.TextSpan, name);
                return new BoundLiteralExpression(0);
            }
            var variableType = value.GetType();
            return new BoundVariableExpression(name, variableType);
        }
        private BoundExpression BindAssignmentExpression(AssignmentExpressionElement expression)
        {
            var identifierName = expression.IdentifierToken.Text;
            var boundExpression = BindExpression(expression.Expression);
            var value = boundExpression.BoundType == typeof(int)
                ? (object)0
                : boundExpression.BoundType == typeof(bool)
                    ? (object)false
                    : null;
            _variableTable[identifierName] = value;
            return new BoundAssignmentExpression(identifierName, boundExpression);
        }
        private BoundExpression BindUnaryExpression(UnaryExpressionElement expression)
        {
            var boundOperandExpression = BindExpression(expression.OperandExpression);
            var boundOperator = BoundUnaryOperator.BindUnaryOperator(expression.OperatorElement.Kind, boundOperandExpression.BoundType);
            if (boundOperator == null)
            {
                _messages.ReportUndefindedUnaryOperator(expression.OperatorElement.TextSpan, expression.OperatorElement.Text, boundOperandExpression.BoundType);
                return boundOperandExpression;
            }
            return new BoundUnaryExpression(boundOperator, boundOperandExpression);
        }
        private BoundExpression BindBinaryExpression(BinaryExpressionElement expression)
        {
            var boundLeftOperandExpression = BindExpression(expression.LeftExpression);
            var boundRightOperandExpression = BindExpression(expression.RightExpression);
            var boundOperator = BoundBinaryOperator.BindBinaryOperator(expression.OperatorElement.Kind, boundLeftOperandExpression.BoundType, boundRightOperandExpression.BoundType);
            if (boundOperator == null)
            {
                _messages.ReportUndefinedBinaryOperator(expression.OperatorElement.TextSpan, expression.OperatorElement.Text, boundLeftOperandExpression.BoundType, boundRightOperandExpression.BoundType);
                return boundLeftOperandExpression;
            }
            return new BoundBinaryExpression(boundLeftOperandExpression, boundRightOperandExpression, boundOperator);
        }
        private BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind, Type boundOperandExpressionType)
        {
            if (boundOperandExpressionType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxKind.PlusToken:
                        return BoundUnaryOperatorKind.ArithmeticIdentity;
                    case SyntaxKind.MinusToken:
                        return BoundUnaryOperatorKind.ArithmeticNegation;
                }
            }
            if (boundOperandExpressionType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.BangToken:
                        return BoundUnaryOperatorKind.LogicalNegation;
                }
            }
            return null;
        }
        private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type boundLeftOperandExpressionType, Type boundRightOperandExpressionType)
        {
            if (boundLeftOperandExpressionType == typeof(int) && boundRightOperandExpressionType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxKind.PlusToken:
                        return BoundBinaryOperatorKind.Addition;
                    case SyntaxKind.MinusToken:
                        return BoundBinaryOperatorKind.Subtraction;
                    case SyntaxKind.SplatToken:
                        return BoundBinaryOperatorKind.Multiplication;
                    case SyntaxKind.SlashToken:
                        return BoundBinaryOperatorKind.Division;
                }
            }
            if (boundLeftOperandExpressionType == typeof(bool) && boundRightOperandExpressionType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.AmpersandAmpersandToken:
                        return BoundBinaryOperatorKind.LogicalAnd;
                    case SyntaxKind.PipePipeToken:
                        return BoundBinaryOperatorKind.LogicalOr;
                }
            }
            return null;
        }
    }
}