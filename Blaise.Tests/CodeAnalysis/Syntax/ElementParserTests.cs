using System.Collections.Generic;
using Blaise.CodeAnalysis.Syntax;
using Xunit;
using System.Linq;

namespace Blaise.Tests.CodeAnalysis.Syntax
{
    public class ElementParserTests
    {
        [Theory]
        [MemberData(nameof(BinaryOperatorPairsData))]
        public void Parser_BinaryExpression_HonorsPrecedences(SyntaxKind firstOperatorKind, SyntaxKind secondOperatorKind)
        {
            var firstOperatorKindPrecedence = SyntaxFacts.GetBinaryOperatorPrecedence(firstOperatorKind);
            var secondOperatorKindPrecedence = SyntaxFacts.GetBinaryOperatorPrecedence(secondOperatorKind);
            var firstOperatorKindText = SyntaxFacts.GetSyntaxText(firstOperatorKind);
            var secondOperatorKindText = SyntaxFacts.GetSyntaxText(secondOperatorKind);
            var expressionText = $"a {firstOperatorKindText} b {secondOperatorKindText} c";
            var expression = ParseExpression(expressionText);

            if (firstOperatorKindPrecedence >= secondOperatorKindPrecedence)
            {
                //     op2
                //     / \
                //   op1  c
                //   / \
                //  a   b
                using (var enumerator = new AssertingEnumerator(expression))
                {
                    enumerator.AssertElement(SyntaxKind.BinaryExpression); // op2
                    enumerator.AssertElement(SyntaxKind.BinaryExpression); // op1
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: a
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "a"); // 'a'
                    enumerator.AssertToken(firstOperatorKind, firstOperatorKindText); // op1 text
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: b
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "b"); // 'b'
                    enumerator.AssertToken(secondOperatorKind, secondOperatorKindText); // op2 text
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: c
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "c");
                }
            }
            else
            {
                //     op1
                //     / \
                //    a  op2
                //       / \
                //      b   c
                using (var enumerator = new AssertingEnumerator(expression))
                {
                    enumerator.AssertElement(SyntaxKind.BinaryExpression); // op1
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: a
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "a");
                    enumerator.AssertToken(firstOperatorKind, firstOperatorKindText); // op1 text
                    enumerator.AssertElement(SyntaxKind.BinaryExpression); // op2
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: b
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "b"); // 'b'
                    enumerator.AssertToken(secondOperatorKind, secondOperatorKindText); // op2 text
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: c
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "c"); // 'c'
                }
            }
        }
        [Theory]
        [MemberData(nameof(UnaryOperatorPairsData))]
        public void Parser_UnaryExpression_HonorsPrecedences(SyntaxKind unaryOperatorKind, SyntaxKind binaryOperatorKind)
        {
            var unaryOperatorKindPrecedence = SyntaxFacts.GetUnaryOperatorPrecedence(unaryOperatorKind);
            var binaryOperatorKindPrecedence = SyntaxFacts.GetBinaryOperatorPrecedence(binaryOperatorKind);
            var unaryOperatorKindText = SyntaxFacts.GetSyntaxText(unaryOperatorKind);
            var binaryOperatorKindText = SyntaxFacts.GetSyntaxText(binaryOperatorKind);
            var expressionText = $"{unaryOperatorKindText} a {binaryOperatorKindText} b";
            var expression = ParseExpression(expressionText);

            if (unaryOperatorKindPrecedence >= binaryOperatorKindPrecedence)
            {
                //     bOp
                //     / \
                //   uOp  b
                //    |
                //    a
                using (var enumerator = new AssertingEnumerator(expression))
                {
                    enumerator.AssertElement(SyntaxKind.BinaryExpression); // op2
                    enumerator.AssertElement(SyntaxKind.UnaryExpression); // op1
                    enumerator.AssertToken(unaryOperatorKind, unaryOperatorKindText); // op1 text
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: a
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "a"); // 'a'
                    enumerator.AssertToken(binaryOperatorKind, binaryOperatorKindText); // op2 text
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: b
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "b"); // 'b'
                }
            }
            else
            {
                //     uOp
                //      |
                //    bOp
                //    / \
                //   a   b
                using (var enumerator = new AssertingEnumerator(expression))
                {
                    enumerator.AssertElement(SyntaxKind.UnaryExpression); // op1
                    enumerator.AssertToken(unaryOperatorKind, unaryOperatorKindText); // op1 text
                    enumerator.AssertElement(SyntaxKind.BinaryExpression); // op2
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: a
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "a");
                    enumerator.AssertToken(binaryOperatorKind, binaryOperatorKindText); // op2 text
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: b
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "b"); // 'b'
                }
            }
        }

        private static ExpressionElement ParseExpression(string expressionText)
        {
            var syntaxTree = SyntaxTree.ParseTree(expressionText);
            var root = syntaxTree.Root;
            var expression = root.Expression;
            return expression;
        }

        public static IEnumerable<object[]> BinaryOperatorPairsData => from firstOperatorKind in SyntaxFacts.BinaryOperatorKinds
                                                                       from secondOperatorKind in SyntaxFacts.BinaryOperatorKinds
                                                                       select new object[] { firstOperatorKind, secondOperatorKind };
        public static IEnumerable<object[]> UnaryOperatorPairsData => from unaryOperatorKind in SyntaxFacts.UnaryOperatorKinds
                                                                      from binaryOperatorKind in SyntaxFacts.BinaryOperatorKinds
                                                                      select new object[] { unaryOperatorKind, binaryOperatorKind };
    }
}
