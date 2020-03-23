using System;
using System.Collections.Generic;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class ExpressionBinder
    {
        private readonly DiagnosticBag _messages = new DiagnosticBag();
        public DiagnosticBag Messages => _messages;
        public BoundExpression BindExpression(SyntaxElement expression)
        {
            switch (expression.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionElement)expression);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionElement)expression);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionElement)expression);
                case SyntaxKind.ParentheticalExpression:
                    return BindExpression(((ParentheticalExpressionElement)expression).Expression);
                default:
                    throw new ArgumentException($"ERROR: Unknown expression type {expression.Kind}.");
            }
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

        private BoundExpression BindLiteralExpression(LiteralExpressionElement expression)
        {
            var value = expression.Value ?? 0;
            return new BoundLiteralExpression(value);
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
    }
}