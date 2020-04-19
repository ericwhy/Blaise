using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed partial class ExpressionBinder
    {
        private readonly DiagnosticBag _messages = new DiagnosticBag();
        private SymbolScope _symbolScope;

        public ExpressionBinder(SymbolScope outerScope)
        {
            _symbolScope = new SymbolScope(outerScope);
        }

        public static BoundGlobalScope BindGlobalScope(BoundGlobalScope previous, CompilationUnitElement compilation)
        {
            var outerScope = CreateOuterScope(previous);
            var binder = new ExpressionBinder(outerScope);
            var statement = binder.BindStatement(compilation.Statement);
            var symbols = binder._symbolScope.DeclaredSymbols;
            var messages = binder.Messages.ToImmutableArray();
            if (previous != null)
            {
                messages = messages.InsertRange(0, previous.Messages);
            }
            return new BoundGlobalScope(previous, messages, symbols, statement);
        }
        public static SymbolScope CreateOuterScope(BoundGlobalScope previous)
        {
            // We want to invert the global scope chain
            var scopeStack = new Stack<BoundGlobalScope>();
            while (previous != null)
            {
                scopeStack.Push(previous);
                previous = previous.Previous;
            }
            SymbolScope outerScope = null;
            while (scopeStack.Count > 0)
            {
                previous = scopeStack.Pop();
                var scope = new SymbolScope(outerScope);
                foreach (var s in previous.Symbols)
                {
                    scope.TryDeclare(s);
                }
                outerScope = scope;
            }
            return outerScope;
        }
        public DiagnosticBag Messages => _messages;
        public BoundStatement BindStatement(StatementElement statement)
        {
            switch (statement.Kind)
            {
                case SyntaxKind.BlockStatement:
                    return BindBlockStatement((BlockStatementElement)statement);
                case SyntaxKind.ExpressionStatement:
                    return BindExpressionStatement((ExpressionStatementElement)statement);
                default:
                    throw new ArgumentException($"ERROR: Unexpected statement expression type {statement.Kind}.");
            }
        }
        private BoundStatement BindExpressionStatement(ExpressionStatementElement element)
        {
            var expression = BindExpression(element.Expression);
            return new BoundExpressionStatement(expression);
        }
        private BoundStatement BindBlockStatement(BlockStatementElement element)
        {
            var boundStatements = ImmutableArray.CreateBuilder<BoundStatement>();
            foreach (var statementElement in element.Statements)
            {
                boundStatements.Add(BindStatement(statementElement));
            }
            return new BoundBlockStatement(boundStatements.ToImmutable());
        }

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
            if (!_symbolScope.TryLookup(name, out var symbol))
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
            if (!_symbolScope.TryLookup(identifierName, out SymbolEntry symbol))
            {
                symbol = new SymbolEntry(identifierName, boundExpression.BoundType);
                _symbolScope.TryDeclare(symbol);
            }
            if (boundExpression.BoundType != symbol.SymbolType)
            {
                _messages.ReportInvalidTypeConversion(expression.Expression.TextSpan, boundExpression.BoundType, symbol.SymbolType);
                return boundExpression;
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
                    case SyntaxKind.NotKeyword:
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
                    case SyntaxKind.AndKeyword:
                        return BoundBinaryOperatorKind.LogicalAnd;
                    case SyntaxKind.PipePipeToken:
                    case SyntaxKind.OrKeyword:
                        return BoundBinaryOperatorKind.LogicalOr;
                }
            }
            return null;
        }
    }
}