using System.Collections.Immutable;

namespace Blaise.CodeAnalysis.Syntax
{
    public sealed class BlockStatementElement : StatementElement
    {
        public BlockStatementElement(SyntaxToken beginKeywordToken, ImmutableArray<StatementElement> statements, SyntaxToken endKeywordToken, SyntaxToken semicolonToken)
            : base(semicolonToken)
        {
            BeginKeywordToken = beginKeywordToken;
            Statements = statements;
            EndKeywordToken = endKeywordToken;
        }

        public SyntaxToken BeginKeywordToken { get; }
        public ImmutableArray<StatementElement> Statements { get; }
        public SyntaxToken EndKeywordToken { get; }

        public override SyntaxKind Kind => SyntaxKind.BlockStatement;
    }
}