using System.Collections.Generic;
using System.Linq;

namespace Blaise.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(IEnumerable<Diagnostic> messages, object value)
        {
            Messages = messages.ToArray();
            Value = value;
        }
        public IReadOnlyList<Diagnostic> Messages { get; }
        public object Value { get; }
    }

}