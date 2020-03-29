using System;
using System.Collections.Generic;
using Blaise.CodeAnalysis.Syntax;
using Xunit;

namespace Blaise.Tests.CodeAnalysis.Syntax
{
    public class SyntaxFactTests
    {
        [Theory]
        [MemberData(nameof(SyntaxTextData))]
        public void SyntaxFact_GetSyntaxText_RoundTrips(SyntaxKind syntaxKind)
        {
            var syntaxText = SyntaxFacts.GetSyntaxText(syntaxKind);
            if (syntaxText == null)
                return;
            var tokens = SyntaxTree.ParseTokens(syntaxText);
            var token = Assert.Single(tokens);
            Assert.Equal(syntaxKind, token.Kind);
            Assert.Equal(syntaxText, token.Text);
        }

        public static IEnumerable<object[]> SyntaxTextData
        {
            get
            {
                var syntaxKinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));
                foreach (var syntaxKind in syntaxKinds)
                    yield return new object[] { syntaxKind };
            }
        }
    }
}
