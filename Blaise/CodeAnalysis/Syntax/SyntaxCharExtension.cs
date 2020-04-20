namespace Blaise.CodeAnalysis.Syntax
{
    public static class SyntaxCharExtension
    {
        public static bool IsIdentifierStart(this char aChar)
        {
            return char.IsLetter(aChar) || (aChar == '_');
        }
        public static bool IsIdentifier(this char aChar)
        {
            return char.IsLetterOrDigit(aChar) || (aChar == '_');
        }
    }

}