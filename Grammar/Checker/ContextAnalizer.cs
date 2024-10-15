using Generated;

namespace MiniPython.Grammar.Checker;

using Antlr4.Runtime;
using System.Collections.Generic;
using System.Text;

public class ContextAnalizer : MiniPythonParserBaseVisitor<object>
{
    private SymbolsTable symbolsTable;
    private CustomErrorListener errorListener;

    public ContextAnalizer(CustomErrorListener errorListener)
    {
        symbolsTable = new SymbolsTable();
        this.errorListener = errorListener;
    }

    private void ReportError(string error, IToken offendingToken)
    {
        var err = new StringBuilder();
        err.Append(error)
           .Append(" line ")
           .Append(offendingToken.Line)
           .Append(":")
           .Append(offendingToken.Column + 1);
        errorListener.AddContextError(err.ToString());
    }

    public override object VisitProgram(MiniPythonParser.ProgramContext context)
    {
        symbolsTable.OpenScope();
        return base.VisitProgram(context);
    }

    public override object VisitDefStatement(MiniPythonParser.DefStatementContext context)
    {
        string functionName = context.ID().GetText();
        if (symbolsTable.SearchInCurrentLevel(functionName) != null)
        {
            ReportError($"The function '{functionName}' is already defined in this scope.", context.ID().Symbol);
        }
        else
        {
            var level = symbolsTable.GetCurrentLevel();
            if (level != 0 && symbolsTable.SearchInSpecificLevel(level - 1, functionName) != null)
            {
                var symbol = symbolsTable.SearchInSpecificLevel(level - 1, functionName);
                if (symbol.Type == SymbolType.Function)
                {
                    var methodIdent = symbol as SymbolsTable.MethodIdent;
                    if (methodIdent != null)
                    {
                        int paramCount = methodIdent.Params.Count;
                        int newParamCount = context.argList()?.ID().Length ?? 0;
                        if (paramCount == newParamCount)
                        {
                            ReportError($"The function '{functionName}' is being redefined with the same {paramCount} parameters.", context.ID().Symbol);
                        }
                    }
                }
            }
            if (errorListener.HasErrors())
            {
                return null;
            }

            List<string> parameters = new List<string>();
            if (context.argList() != null)
            {
                foreach (var param in context.argList().ID())
                {
                    parameters.Add(param.GetText());
                }
            }

            symbolsTable.InsertFunction(context.ID().Symbol, SymbolType.Function, parameters);
            symbolsTable.OpenScope();
            if (context.argList() != null)
            {
                foreach (var param in context.argList().ID())
                {
                    symbolsTable.InsertVariable(param.Symbol, SymbolType.Parameter);
                }
            }
            Visit(context.sequence());
            symbolsTable.Print();
            symbolsTable.CloseScope();
        }
        return null;
    }

    public override object VisitIfStatement(MiniPythonParser.IfStatementContext context)
    {
        Visit(context.expression());
        symbolsTable.OpenScope();
        Visit(context.sequence(0));
        symbolsTable.Print();
        symbolsTable.CloseScope();

        if (context.ELSE() != null)
        {
            symbolsTable.OpenScope();
            Visit(context.sequence(1));
            symbolsTable.Print();
            symbolsTable.CloseScope();
        }
        return null;
    }

    public override object VisitWhileStatement(MiniPythonParser.WhileStatementContext context)
    {
        Visit(context.expression());
        symbolsTable.OpenScope();
        Visit(context.sequence());
        symbolsTable.Print();
        symbolsTable.CloseScope();
        return null;
    }

    public override object VisitForStatement(MiniPythonParser.ForStatementContext context)
    {
        Visit(context.expression());
        Visit(context.expressionList());
        symbolsTable.OpenScope();
        Visit(context.sequence());
        symbolsTable.Print();
        symbolsTable.CloseScope();
        return null;
    }

    public override object VisitReturnStatement(MiniPythonParser.ReturnStatementContext context)
    {
        var expressionText = context.expression().GetText();
        if (!string.IsNullOrEmpty(expressionText) && !expressionText.Contains("("))
        {
            var expressionSymbol = symbolsTable.SearchInCurrentLevel(expressionText);
            if (expressionSymbol == null)
            {
                ReportError($"The variable '{expressionText}' is not defined.", context.expression().Start);
            }
        }
        return null;
    }

    public override object VisitPrintStatement(MiniPythonParser.PrintStatementContext context)
    {
        var expressionText = context.expression().GetText();
        if (!string.IsNullOrEmpty(expressionText) && !expressionText.Contains("("))
        {
            var expressionSymbol = symbolsTable.SearchInCurrentLevel(expressionText);
            if (expressionSymbol == null)
            {
                ReportError($"The variable '{expressionText}' is not defined.", context.expression().Start);
            }
        }
        return null;
    }

    public override object VisitAssignStatement(MiniPythonParser.AssignStatementContext context)
    {
        string varName = context.ID().GetText();
        if (symbolsTable.SearchInCurrentLevel(varName) != null)
        {
            ReportError($"The variable '{varName}' is already defined in this scope.", context.ID().Symbol);
        }
        else
        {
            Visit(context.expression());
            if (errorListener.HasErrors())
            {
                return null;
            }
            symbolsTable.InsertVariable(context.ID().Symbol, SymbolType.Variable);
        }
        return null;
    }

    public override object VisitFunctionCallStatement(MiniPythonParser.FunctionCallStatementContext context)
    {
        var functionName = context.ID().GetText();
        var functionSymbol = symbolsTable.Search(functionName);
        if (functionSymbol == null || functionSymbol.Type != SymbolType.Function)
        {
            ReportError($"The function '{functionName}' is not defined.", context.ID().Symbol);
        }
        else
        {
            var methodSymbol = functionSymbol as SymbolsTable.MethodIdent;
            int numArguments = context.expressionList()?.expression().Length ?? 0;
            int numParameters = methodSymbol.Params.Count;

            if (numArguments != numParameters)
            {
                ReportError($"The function '{functionName}' expects {numParameters} arguments, but {numArguments} were passed.", context.ID().Symbol);
            }
        }
        return null;
    }

    public override string ToString()
    {
        return errorListener.ToString();
    }
}