using Blaise.CodeAnalysis.Text;
using Xunit;

namespace Blaise.Tests.CodeAnalysis.Syntax.Text
{
    public class SourceTextTests
    {
        [Theory]
        [InlineData(".", 1)]
        [InlineData(".\r\n", 2)]
        [InlineData(".\r\n\r\n", 3)]
        public void SourceText_IncludesLastLine(string source, int lineCount)
        {
            var sourceText = SourceText.FromSource(source);
            Assert.Equal(lineCount, sourceText.Lines.Length);
        }
    }
}
