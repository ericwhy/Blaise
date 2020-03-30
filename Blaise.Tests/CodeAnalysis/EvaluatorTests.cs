using System.Collections.Generic;
using Blaise.CodeAnalysis;
using Blaise.CodeAnalysis.Syntax;
using Xunit;

namespace Blaise.Tests.CodeAnalysis
{
    public class EvaluatorTests
    {
        [Theory]
        [InlineData("1", 1)]
        [InlineData("-1", -1)]
        [InlineData("1+2", 3)]
        [InlineData("12/3", 4)]
        [InlineData("-5*1", -5)]
        [InlineData("(10-2)/4", 2)]
        [InlineData("true", true)]
        [InlineData("!true", false)]
        [InlineData("not false", true)]
        [InlineData("2 = 2", true)]
        [InlineData("(2 <> 2)", false)]
        [InlineData("!(2 != 2)", true)]
        [InlineData("(1=1) or (1=2)", true)]
        [InlineData("(1=1) || (1=2)", true)]
        [InlineData("(1=1) and (1=2)", false)]
        [InlineData("(1=1) && (1=2)", false)]
        [InlineData("a := 10", 10)]
        [InlineData("(a := 10) + 10", 20)]
        public void SyntaxFact_GetText_RoundTrips(string text, object expectedResult)
        {
            var syntaxTree = SyntaxTree.ParseTree(text);
            var compiler = new Compilation(syntaxTree);
            Dictionary<SymbolEntry, object> symbolTable = new Dictionary<SymbolEntry, object>();
            var result = compiler.Evaluate(symbolTable);
            Assert.Empty(result.Messages);
            Assert.Equal(expectedResult, result.Value);
        }
    }
}