using Antlr4.Runtime;
using System;

namespace MiniPython.Lexer
{
    public class CustomErrorListener : BaseErrorListener
    {
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            Console.WriteLine($"Error at line {line}:{charPositionInLine} - {msg}");
        }
    }
}