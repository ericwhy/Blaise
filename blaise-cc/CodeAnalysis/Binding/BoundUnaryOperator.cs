using System;
using System.Linq;
using Blaise.CodeAnalysis.Syntax;

namespace Blaise.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator
    {
        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind operatorKind, Type operatorType)
            :this(syntaxKind, operatorKind, operatorType, operatorType) 
        {

        }
        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind operatorKind, Type operandType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            OperatorKind = operatorKind;
            OperandType = operandType;
            ResultType = resultType;
        }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind OperatorKind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }
        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.ArithmeticIdentity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(int)),
        };

        public static BoundUnaryOperator BindUnaryOperator(SyntaxKind syntaxKind, Type operandType)
        {
            return _operators.Where(x => x.SyntaxKind == syntaxKind && x.OperandType == operandType).FirstOrDefault();
        }
    }
}