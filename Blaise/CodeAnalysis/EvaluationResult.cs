using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Blaise.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(ImmutableArray<Diagnostic> messages, object value)
        {
            Messages = messages;
            Value = value;
        }
        public ImmutableArray<Diagnostic> Messages { get; }
        public object Value { get; }
    }

}