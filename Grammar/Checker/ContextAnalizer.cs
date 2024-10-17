namespace MiniPython.Grammar.Checker;

using Generated;
using Antlr4.Runtime;
using System.Collections.Generic;
using System.Text;
public class ContextAnalizer : MiniPythonParserBaseVisitor<object> {

    private SymbolsTable currentTable;
    public List<string> errorList ;
    public ContextAnalizer() {
        currentTable = new SymbolsTable();
        errorList =  new List<string>();
    }    
    private void reportError(string error, IToken offendingToken) {
        var err = new StringBuilder();
        err.Append("CONTEXT ERROR - line ")
            .Append(offendingToken.Line)
            .Append(":")
            .Append(offendingToken.Column + 1)
            .Append(" ")
            .Append(error);
        errorList.Add(err.ToString());
    }
    public override object VisitProgram(MiniPythonParser.ProgramContext context)
    {
        currentTable.OpenScope();
        return base.VisitProgram(context);
    }
    public override object VisitMainStatement(MiniPythonParser.MainStatementContext context)
    {
        return base.VisitMainStatement(context);
    }
 public override object VisitStatement(MiniPythonParser.StatementContext context) {
     return base.VisitStatement(context);;
 }
    public override object VisitDefStatement(MiniPythonParser.DefStatementContext context) {
        string methodName = context.ID().GetText();
        if (currentTable.SearchInCurrentLevel(methodName) != null) {
            reportError($"The function '{methodName}' is already defined in this scope.", context.ID().Symbol);
        } else {
            var nivel = currentTable.GetCurrentLevel();
            if (nivel != 0 && currentTable.SearchInSpecificLevel(nivel - 1, methodName) != null) {
                var symbol = currentTable.SearchInSpecificLevel(nivel - 1, methodName);
                if (symbol.Type == SymbolType.Function) {
                    var methodIdent = symbol as SymbolsTable.MethodIdent;
                    if (methodIdent != null) {
                        int parametersCount = methodIdent.Params.Count;
                        int newParametersCount = 0;
                        foreach (var param in context.argList().ID()) {
                            newParametersCount++;
                        }
                        if (parametersCount == newParametersCount) {
                            reportError($"The function '{methodName}' is being redefined with the same {parametersCount} parameters.", context.ID().Symbol);
                            return null;
                        }
                    }
                }
            }
            List<string> parameters = new List<string>();
            if (context.argList() != null) {
                foreach (var param in context.argList().ID()) {
                    parameters.Add(param.GetText());
                }
            }
            currentTable.InsertFunction(context.ID().Symbol, SymbolType.Function, parameters);
            currentTable.OpenScope();
            if (context.argList() != null) {
                foreach (var param in context.argList().ID()) {
                    currentTable.InsertVariable(param.Symbol, SymbolType.Parameter);
                }
            }
            Visit(context.DOSPUN());
            Visit(context.sequence());
            currentTable.Print();
            currentTable.CloseScope();
        }
        return null;
    }
    public override object VisitArgList(MiniPythonParser.ArgListContext context)
    {
        return base.VisitArgList(context);
    }
    public override object VisitIfStatement(MiniPythonParser.IfStatementContext context)
    {
        Visit(context.expression());
        Visit(context.DOSPUN(0));
        currentTable.OpenScope();
        Visit(context.sequence(0)); 
        currentTable.Print();
        currentTable.CloseScope();
        Visit(context.ELSE());
        Visit(context.DOSPUN(1));
        currentTable.OpenScope();
        Visit(context.sequence(1)); 
        currentTable.Print();
        currentTable.CloseScope();
        return null;

    }
    public override object VisitWhileStatement(MiniPythonParser.WhileStatementContext context)
    {
        Visit(context.expression());
        Visit(context.DOSPUN());
        currentTable.OpenScope();
        Visit(context.sequence());
        currentTable.Print();
        currentTable.CloseScope();

        return null;
    }
    public override object VisitReturnStatement(MiniPythonParser.ReturnStatementContext context)
    {
        var expressionText = context.expression().GetText();
        if (!string.IsNullOrEmpty(expressionText) && !expressionText.Contains("("))
        {
           Visit(context.expression());
        }
        return null;
    }

    public override object VisitForStatement(MiniPythonParser.ForStatementContext context)
    {
            Visit(context.expression());
            Visit(context.expressionList()); 
            Visit(context.DOSPUN());
            currentTable.OpenScope();
            Visit(context.sequence()); 
            currentTable.Print();
            currentTable.CloseScope();

            return null;
    }

    public override object VisitPrintStatement(MiniPythonParser.PrintStatementContext context)
    {
        var expressionText = context.expression().GetText();
        if (!string.IsNullOrEmpty(expressionText)) 
        {
            Visit(context.expression());
        }
        return null;
    }
    public override object VisitAssignStatement(MiniPythonParser.AssignStatementContext context)
    {
        string varName = context.ID().GetText();
        Visit(context.ID());
        if (currentTable.SearchInCurrentLevel(varName) != null) {
            reportError($"The variable '{varName}' is already defined in this scope.", context.ID().Symbol);
        } else {
            Visit(context.ASSIGN());
            int initialErrorCount = errorList.Count;
            Visit(context.expression());
            
            if (errorList.Count > initialErrorCount){
                return null;
            }
            currentTable.InsertVariable(context.ID().Symbol, SymbolType.Variable);
            Visit(context.NEWLINE());
        }
        return null;
    }
    public override object VisitFunctionCallStatement(MiniPythonParser.FunctionCallStatementContext context)
    {
        var methodName = context.ID().GetText();
        var functionSymbol = currentTable.Search(methodName);
        if (functionSymbol == null || functionSymbol.Type != SymbolType.Function)
        {
            reportError($"The function '{methodName}' is not defined.", context.ID().Symbol);
        }
        else
        {
            var methodSymbol = functionSymbol as SymbolsTable.MethodIdent;
            int numArguments = context.expressionList()?.expression().Length ?? 0;
            int numParameters = methodSymbol.Params.Count;

            if (numArguments != numParameters)
            {
                reportError($"The function '{methodName}' expects {numParameters} arguments, but {numArguments} were passed.", context.ID().Symbol);
            }
        } 
        
        return null;
    }
    public override object VisitSequence(MiniPythonParser.SequenceContext context) {
        
        return base.VisitSequence(context) ;
    }
    public override object VisitExpression(MiniPythonParser.ExpressionContext context) {
        var ex1 = Visit(context.additionExpression());
        var additionExpr = context.additionExpression();
        foreach (var multExpr in additionExpr.multiplicationExpression()) {
            foreach (var elemExpr in multExpr.elementExpression()) {
                if (elemExpr.primitiveExpression() is MiniPythonParser.PrimitiveExpressionExprListASTContext identifierContext) {
                    string identifier = identifierContext.ID().GetText();
                    if (identifierContext.LPAREN() != null) {
                        var functionSymbol = currentTable.Search(identifier);
                        if (functionSymbol == null || functionSymbol.Type != SymbolType.Function) {
                            reportError($"The function '{identifier}' is not defined.", identifierContext.ID().Symbol);
                        } else {
                            var methodSymbol = functionSymbol as SymbolsTable.MethodIdent;
                            int numArguments = identifierContext.expressionList()?.expression().Length ?? 0;
                            int numParameters = methodSymbol.Params.Count;

                            if (numArguments != numParameters) {
                                reportError($"The function '{identifier}' expects {numParameters} arguments, but {numArguments} were passed.", identifierContext.ID().Symbol);
                            }
                        }
                    } else {
                        var symbol = currentTable.SearchInCurrentLevel(identifier);
                        if (symbol == null) {
                            symbol= currentTable.Search(identifier);
                            if (symbol == null) {
                                reportError($"The variable '{identifier}' is not defined.", identifierContext.ID().Symbol);
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
                    reportError($"The expression '{primitiveText}' is not indexable.", context.primitiveExpression().Start);
                    return null;
                }
                if (primitiveText.StartsWith("\"") && primitiveText.EndsWith("\""))
                {
                    reportError($"The expression '{primitiveText}' is not indexable.", context.primitiveExpression().Start);
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
        var symbol = currentTable.SearchInCurrentLevel(identifier);
        return symbol;
    }

    public override object VisitListExpression(MiniPythonParser.ListExpressionContext context)
    {
        return base.VisitListExpression(context);
    }
    public bool hasErrors() {
        return errorList.Count > 0;
    }

    public override string ToString() {
        if (!hasErrors()) return "0 type/scope errors";
        var builder = new StringBuilder();
        foreach (var error in errorList) {
            builder.AppendLine(error);
        }
        return builder.ToString();
    }
    
}