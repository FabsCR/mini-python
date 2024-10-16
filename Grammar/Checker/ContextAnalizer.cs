namespace MiniPython.Grammar.Checker;

using Generated;
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
                            return null;
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

    public override object VisitSequence(MiniPythonParser.SequenceContext context)
    {
        return base.VisitSequence(context);
    }

    public override object VisitExpression(MiniPythonParser.ExpressionContext context)
    {
        var ex1 = Visit(context.additionExpression());
        var additionExpr = context.additionExpression();
        foreach (var multExpr in additionExpr.multiplicationExpression())
        {
            foreach (var elemExpr in multExpr.elementExpression())
            {
                if (elemExpr.primitiveExpression() is MiniPythonParser.PrimitiveExpressionExprListASTContext identifierContext)
                {
                    string identifier = identifierContext.ID().GetText();
                    if (identifierContext.LPAREN() != null)
                    {
                        var functionSymbol = symbolsTable.Search(identifier);
                        if (functionSymbol == null || functionSymbol.Type != SymbolType.Function)
                        {
                            ReportError($"The function '{identifier}' is not defined.", identifierContext.ID().Symbol);
                        }
                        else
                        {
                            var methodSymbol = functionSymbol as SymbolsTable.MethodIdent;
                            int numArguments = identifierContext.expressionList()?.expression().Length ?? 0;
                            int numParameters = methodSymbol.Params.Count;

                            if (numArguments != numParameters)
                            {
                                ReportError($"The function '{identifier}' expects {numParameters} arguments, but {numArguments} were passed.", identifierContext.ID().Symbol);
                            }
                        }
                    }
                    else
                    {
                        var symbol = symbolsTable.SearchInCurrentLevel(identifier);
                        if (symbol == null)
                        {
                            symbol = symbolsTable.Search(identifier);
                            if (symbol == null)
                            {
                                ReportError($"The variable '{identifier}' is not defined.", identifierContext.ID().Symbol);
                            }
                        }
                    }
                }
            }
        }
        return ex1;
    }

    public override object VisitComparison(MiniPythonParser.ComparisonContext context)
    {
        return base.VisitComparison(context);
    }

    public override object VisitAdditionExpression(MiniPythonParser.AdditionExpressionContext context)
    {
        return base.VisitAdditionExpression(context);
    }

    public override object VisitMultiplicationExpression(MiniPythonParser.MultiplicationExpressionContext context)
    {
        return base.VisitMultiplicationExpression(context);
    }

    public override object VisitElementExpression(MiniPythonParser.ElementExpressionContext context)
    {
        var primitiveExpr = context.primitiveExpression();
        if (primitiveExpr != null)
        {
            var result = Visit(primitiveExpr);
            if (context.LBRACKET() != null && context.expression() != null)
            {
                string primitiveText = primitiveExpr.GetText();
                if (primitiveText.All(char.IsDigit))
                {
                    ReportError($"The expression '{primitiveText}' is not indexable.", context.primitiveExpression().Start);
                    return null;
                }
                if (primitiveText.StartsWith("\"") && primitiveText.EndsWith("\""))
                {
                    ReportError($"The expression '{primitiveText}' is not indexable.", context.primitiveExpression().Start);
                    return null;
                }
                Visit(context.expression());
            }
            return result;
        }
        return null;
    }

    public override object VisitExpressionList(MiniPythonParser.ExpressionListContext context)
    {
        return base.VisitExpressionList(context);
    }

    public override object VisitPrimitiveExpressionParenExprAST(MiniPythonParser.PrimitiveExpressionParenExprASTContext context)
    {
        return base.VisitPrimitiveExpressionParenExprAST(context);
    }

    public override object VisitPrimitiveExpressionLenAST(MiniPythonParser.PrimitiveExpressionLenASTContext context)
    {
        return base.VisitPrimitiveExpressionLenAST(context);
    }

    public override object VisitPrimitiveExpressionListExprAST(MiniPythonParser.PrimitiveExpressionListExprASTContext context)
    {
        return base.VisitPrimitiveExpressionListExprAST(context);
    }

    public override object VisitPrimitiveExpressionLiteralAST(MiniPythonParser.PrimitiveExpressionLiteralASTContext context)
    {
        return base.VisitPrimitiveExpressionLiteralAST(context);
    }

    public override object VisitPrimitiveExpressionExprListAST(MiniPythonParser.PrimitiveExpressionExprListASTContext context)
    {
        string identifier = context.ID().GetText();
        var symbol = symbolsTable.SearchInCurrentLevel(identifier);
        return symbol;
    }

    public override object VisitListExpression(MiniPythonParser.ListExpressionContext context)
    {
        return base.VisitListExpression(context);
    }

    public bool HasErrors()
    {
        return errorListener.HasErrors();
    }

    public override string ToString()
    {
        return errorListener.ToString();
    }
}