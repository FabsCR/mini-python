using System.IO;
using Antlr4.Runtime;
using Generated;

namespace MiniPython.Grammar
{
    public class CustomErrorListener : BaseErrorListener, IAntlrErrorListener<IToken>, IAntlrErrorListener<int>
    {
        public List<string> ErrorMsgs { get; }

        public CustomErrorListener()
        {
            ErrorMsgs = new List<string>();
        }
        
        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            if (recognizer is MiniPythonParser) 
            {
                ErrorMsgs.Add($"PARSER ERROR - line {line}:{charPositionInLine + 1} {msg}");
            }
            else if (recognizer is MiniPythonLexer) 
            {
                ErrorMsgs.Add($"SCANNER ERROR - line {line}:{charPositionInLine + 1} {msg}");
            }
            else
            {
                ErrorMsgs.Add("Other Error");
            }
        }
        
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            ErrorMsgs.Add($"LEXER ERROR - line {line}:{charPositionInLine + 1} {msg}");
        }

        public void AddContextError(string msg)
        {
            ErrorMsgs.Add($"CONTEXT ERROR - {msg}");
        }

        public bool HasErrors()
        {
            return ErrorMsgs.Count > 0;
        }

        public override string ToString()
        {
            if (!HasErrors())
            {
                return "0 errors";
            }
            else
            {
                var builder = new System.Text.StringBuilder();
                foreach (var error in ErrorMsgs)
                {
                    builder.AppendLine(error);
                }
                return builder.ToString();
            }
        }
    }
}