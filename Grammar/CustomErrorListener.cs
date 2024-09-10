using Antlr4.Runtime;

namespace MiniPython.Grammar
{
    public class CustomErrorListener : BaseErrorListener
    {
        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Console.WriteLine($"Error at line {line}:{charPositionInLine} - {msg}");
        }
    }
}