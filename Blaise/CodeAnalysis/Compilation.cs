using System;
using System.Linq;
using Blaise.CodeAnalysis.Binding;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis
{
    public class Compilation
    {
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public SyntaxTree Syntax { get; }

        public EvaluationResult Evaluate()
        {
            var binder = new ExpressionBinder();
            var boundExpression = binder.BindExpression(Syntax.Root);
            var messages = Syntax.Messages.Concat(binder.Messages).ToArray();
            if (messages.Any())
                return new EvaluationResult(messages, null);
            var evaluator = new SyntaxEvaluator(boundExpression);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(), value);
        }
    }
}
