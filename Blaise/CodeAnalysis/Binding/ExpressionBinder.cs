using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed partial class ExpressionBinder
    {
        private readonly Dictionary<SymbolEntry, object> _variableTable;
        private readonly DiagnosticBag _messages = new DiagnosticBag();

        public ExpressionBinder(Dictionary<SymbolEntry, object> variableTable)
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
            var symbol = _variableTable.Keys.FirstOrDefault(s => s.SymbolName == name);
            if (symbol == null)
            {
                _messages.ReportUndefinedName(expression.IdentifierToken.TextSpan, name);
                return new BoundLiteralExpression(0);
            }
            return new BoundVariableExpression(symbol);
        }
        private BoundExpression BindAssignmentExpression(AssignmentExpressionElement expression)
        {
            var identifierName = expression.IdentifierToken.Text;
            var boundExpression = BindExpression(expression.Expression);
            var currentSymbol = _variableTable.Keys.FirstOrDefault(s => s.SymbolName == identifierName);
            var symbol = new SymbolEntry(identifierName, boundExpression.BoundType);
            if (currentSymbol != null && currentSymbol.SymbolType != symbol.SymbolType)
            {
                _variableTable.Remove(currentSymbol);
                _variableTable[symbol] = null;
            }
            return new BoundAssignmentExpression(symbol, boundExpression);
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
                    case SyntaxKind.LiteralNotToken:
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
                    case SyntaxKind.LiteralAndToken:
                        return BoundBinaryOperatorKind.LogicalAnd;
                    case SyntaxKind.PipePipeToken:
                    case SyntaxKind.LiteralOrToken:
                        return BoundBinaryOperatorKind.LogicalOr;
                }
            }
            return null;
        }
    }
}