using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Blaise.CodeAnalysis.Binding;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis
{
    public class Compilation
    {
        private BoundGlobalScope _globalScope;
        public Compilation(SyntaxTree syntax)
            : this(null, syntax)
        {
        }

        private Compilation(Compilation previous, SyntaxTree syntax)
        {
            Previous = previous;
            Syntax = syntax;
        }
        private BoundGlobalScope GlobalScope
        {
            get
            {
                if (_globalScope == null)
                {
                    var globalScope = ExpressionBinder.BindGlobalScope(Previous?.GlobalScope, Syntax.Root);
                    Interlocked.CompareExchange(ref _globalScope, globalScope, null);
                }
                return _globalScope;
            }
        }
        public Compilation ContinueWith(SyntaxTree syntax)
        {
            return new Compilation(this, syntax);
        }
        public SyntaxTree Syntax { get; }
        public Compilation Previous { get; }

        public EvaluationResult Evaluate(Dictionary<SymbolEntry, object> variableTable)
        {
            var messages = Syntax.Messages.Concat(GlobalScope.Messages).ToImmutableArray();
            if (messages.Any())
                return new EvaluationResult(messages, null);
            var evaluator = new SyntaxEvaluator(GlobalScope.Expression, variableTable);
            var value = evaluator.Evaluate();
            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }
    }
}
