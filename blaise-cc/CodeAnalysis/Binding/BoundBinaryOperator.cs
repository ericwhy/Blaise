using System;
using System.Linq;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind operatorKind, Type allType)
            : this(syntaxKind, operatorKind, allType, allType, allType)
        {

        }
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind operatorKind, Type operandType, Type resultType)
            : this(syntaxKind, operatorKind, operandType, operandType, resultType)
        {

        }
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind operatorKind, Type leftOperandType, Type rightOperandType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            OperatorKind = operatorKind;
            LeftOperandType = leftOperandType;
            RightOperandType = rightOperandType;
            ResultType = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind OperatorKind { get; }
        public Type LeftOperandType { get; }
        public Type RightOperandType { get; }
        public Type ResultType { get; }
        private static BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.SplatToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.EqualsToken, BoundBinaryOperatorKind.Equals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.LtGtToken, BoundBinaryOperatorKind.NotEquals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqualsToken, BoundBinaryOperatorKind.Equals, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.LtGtToken, BoundBinaryOperatorKind.NotEquals, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
        };

        public static BoundBinaryOperator BindBinaryOperator(SyntaxKind syntaxKind, Type leftOperandType, Type rightOperandType)
        {
            return _operators.Where(x => x.SyntaxKind == syntaxKind && x.LeftOperandType == leftOperandType && x.RightOperandType == rightOperandType).FirstOrDefault();
        }
    }
}