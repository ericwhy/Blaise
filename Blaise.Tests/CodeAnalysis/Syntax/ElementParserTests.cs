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
            var expression = SyntaxTree.ParseTree(expressionText).Root;

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
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: c
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "c");
                    enumerator.AssertToken(secondOperatorKind, secondOperatorKindText); // op2 text
                    enumerator.AssertElement(SyntaxKind.BinaryExpression); // op1
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: b
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "b"); // 'b'
                    enumerator.AssertToken(firstOperatorKind, firstOperatorKindText); // op1 text
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: a
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "a"); // 'a'
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
                    enumerator.AssertElement(SyntaxKind.BinaryExpression); // op2
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: c
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "c"); // 'c'
                    enumerator.AssertToken(secondOperatorKind, secondOperatorKindText); // op2 text
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: b
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "b"); // 'b'
                    enumerator.AssertToken(firstOperatorKind, firstOperatorKindText); // op1 text
                    enumerator.AssertElement(SyntaxKind.NameExpression); // Name: a
                    enumerator.AssertToken(SyntaxKind.IdentifierToken, "a");
                }
            }

        }
        public static IEnumerable<object[]> BinaryOperatorPairsData => from firstOperatorKind in SyntaxFacts.BinaryOperatorKinds
                                                                       from secondOperatorKind in SyntaxFacts.BinaryOperatorKinds
                                                                       select new object[] { firstOperatorKind, secondOperatorKind };
    }
}
