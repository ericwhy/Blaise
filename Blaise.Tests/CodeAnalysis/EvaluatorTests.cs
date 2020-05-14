using System;
using System.Collections.Generic;
using Blaise.CodeAnalysis;
using Blaise.CodeAnalysis.Syntax;
using Xunit;

namespace Blaise.Tests.CodeAnalysis
{
    public class EvaluatorTests
    {
        [Theory]
        [InlineData("1;", 1)]
        [InlineData("-1;", -1)]
        [InlineData("1+2;", 3)]
        [InlineData("12/3;", 4)]
        [InlineData("-5*1;", -5)]
        [InlineData("(10-2)/4;", 2)]
        [InlineData("true;", true)]
        [InlineData("!true;", false)]
        [InlineData("not false;", true)]
        [InlineData("2 = 2;", true)]
        [InlineData("(2 <> 2);", false)]
        [InlineData("!(2 != 2);", true)]
        [InlineData("(1=1) or (1=2);", true)]
        [InlineData("(1=1) || (1=2);", true)]
        [InlineData("(1=1) and (1=2);", false)]
        [InlineData("(1=1) && (1=2);", false)]
        [InlineData("begin var a:Int32; a := 10; end;", 10)]
        [InlineData("begin var a:Int32; (a := 10) + 10; end;", 20)]
        public void SyntaxFact_GetText_RoundTrips(string text, object expectedResult)
        {
            var syntaxTree = SyntaxTree.ParseTree(text);
            var compiler = new Compilation(syntaxTree);
            Dictionary<SymbolEntry, object> symbolTable = new Dictionary<SymbolEntry, object>();
            var result = compiler.Evaluate(symbolTable);
            Assert.Empty(result.Messages);
            Assert.Equal(expectedResult, result.Value);
        }

        [Fact]
        public void Evaluator_Reports_VariableRedeclaration()
        {
            string text = @"
            begin
                var a: Int32;
                begin
                    var a: Int32;
                end;
                var [a]: Int32;
            end;
            ";
            var expectedMessages = new List<string>();
            expectedMessages.Add("Variable 'a' is already declared.");
            AssertExpectedMessages(text, expectedMessages);
        }

        private void AssertExpectedMessages(string text, List<string> expectedMessages)
        {
            var annotatedText = AnnotatedText.ParseText(text);
            if (annotatedText.TextSpans.Length > expectedMessages.Count)
            {
                throw new ArgumentException("There are more marked spans than expected message.");
            }
            else if (annotatedText.TextSpans.Length < expectedMessages.Count)
            {
                throw new ArgumentException("There are more expected messages then marked spanse.");
            }
            var syntaxTree = SyntaxTree.ParseTree(annotatedText.Text);
            var compilation = new Compilation(syntaxTree);
            var result = compilation.Evaluate(new Dictionary<SymbolEntry, object>());
            Assert.Equal(expectedMessages.Count, result.Messages.Length);
            for (var i = 0; i < expectedMessages.Count; i++)
            {
                var expected = expectedMessages[i];
                var message = result.Messages[i];
                Assert.Equal(expected, message.Message);
                var span = annotatedText.TextSpans[i];
                Assert.Equal(span, message.Span);
            }
        }
    }
}