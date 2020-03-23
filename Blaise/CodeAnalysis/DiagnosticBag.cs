using System;
using System.Collections;
using System.Collections.Generic;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis
{
    internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _diagnostics.GetEnumerator();
        private void Report(TextSpan textSpan, string message)
        {
            _diagnostics.Add(new Diagnostic(textSpan, message));
        }
        internal void AddRange(DiagnosticBag messages)
        {
            _diagnostics.AddRange(messages);
        }

        internal void ReportInvalidInteger(TextSpan textSpan, string token, Type type)
        {
            var message = $"The value {textSpan} isn't a valid {type}.";
            Report(textSpan, message);
        }
        internal void ReportInvalidCharacter(int errorPosition, string errorCh)
        {
            var message = $"Invalid character in input: '{errorCh}'.";
            Report(new TextSpan(errorPosition, 1), message);
        }

        internal void ReportUnexpectedToken(TextSpan textSpan, SyntaxKind actualKind, SyntaxKind expectedKind)
        {
            var message = $"Found unexpected token <{actualKind}> when expecting <{expectedKind}>.";
            Report(textSpan, message);
        }

        internal void ReportUndefindedUnaryOperator(TextSpan textSpan, string operatorText, Type operandType)
        {
            var message = $"Unary operator {operatorText} is not defined for type {operandType}.";
            Report(textSpan, message);
        }

        internal void ReportUndefinedBinaryOperator(TextSpan textSpan, string operatorText, Type leftExpressionBoundType, Type rightExpressionBoundType)
        {
            var message = $"Binary operator {operatorText} is not defined for types {leftExpressionBoundType} and {rightExpressionBoundType}.";
            Report(textSpan, message);
        }
    }
}