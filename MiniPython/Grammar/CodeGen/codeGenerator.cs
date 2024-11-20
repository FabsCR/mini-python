using System.Text;
using Antlr4.Runtime.Tree;
 
namespace compilador.codeGen;
using Generated;

public class codeGenerator : MiniPythonParserBaseVisitor<object>
{
    private int currentLevel = 0;
    private List<Dictionary<string, string>> scopeStack = new List<Dictionary<string, string>>();

    private class Instruction{
        private string instr;
        private string value;
        public Instruction(string ins, string value)
        {
            instr = ins;
            value = value;
        }
        public Instruction(string ins)
        {
            instr = ins;
        }
        public string Value
        {
            set { this.value = value; }
        }
        public override string ToString() {
            return value != null ? instr + " " + value : instr;
           
        }
    }
    private List<Instruction> byteCode; 
 
    public codeGenerator() {
        byteCode = new List<Instruction>();
        scopeStack.Add(new Dictionary<string, string>());
        
    }
    
    public override object VisitProgram(MiniPythonParser.ProgramContext context)
    {
        foreach (var statement in context.mainStatement())
        {
            Visit(statement);
        }
        byteCode.Add(new Instruction("END"));
        return null;
    }

    public override object VisitMainStatement(MiniPythonParser.MainStatementContext context)
    {
        return base.VisitMainStatement(context);
    }

    public override object VisitStatement(MiniPythonParser.StatementContext context)
    {
        return base.VisitStatement(context);
    }

    public override object VisitDefStatement(MiniPythonParser.DefStatementContext context)
    {
        byteCode.Add(new Instruction("DEF", context.ID().GetText()));
        currentLevel++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.argList());
        Visit(context.sequence());
        currentLevel--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        return null;
    }

    public override object VisitArgList(MiniPythonParser.ArgListContext context)
    {
        if (context.ID() != null)
        {
            foreach (var identifier in context.ID())
            {
                var name = identifier.GetText()+"_"+currentLevel;
                byteCode.Add(new Instruction("PUSH_LOCAL", name));
                scopeStack[currentLevel].Add(identifier.GetText(), name);
            }
        }
        return null;
    }

    public override object VisitIfStatement(MiniPythonParser.IfStatementContext context)
    {
        Visit(context.expression());
        byteCode.Add(new Instruction("JUMP_IF_FALSE", ""));
        int jumpIfFalsePos = byteCode.Count - 1;
        currentLevel++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.sequence(0));
        currentLevel--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        byteCode.Add(new Instruction("JUMP_ABSOLUTE", ""));
        int jumpAbsolutePos = byteCode.Count - 1;
        byteCode[jumpIfFalsePos].Value = byteCode.Count.ToString();
        currentLevel++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.sequence(1));
        currentLevel--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        byteCode[jumpAbsolutePos].Value = byteCode.Count.ToString();
        return null;
    }

    public override object VisitWhileStatement(MiniPythonParser.WhileStatementContext context)
    {
        int startPos = byteCode.Count;
        Visit(context.expression());
        byteCode.Add(new Instruction("JUMP_IF_FALSE", ""));
        int jumpIfFalsePos = byteCode.Count - 1;
        currentLevel++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.sequence());
        currentLevel--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        byteCode.Add(new Instruction("JUMP_ABSOLUTE", startPos.ToString()));
        byteCode[jumpIfFalsePos].Value = byteCode.Count.ToString();
        return null;
    }

    public override object VisitForStatement(MiniPythonParser.ForStatementContext context)
    {
        Visit(context.expressionList()); 
        string va = context.expression().GetText()+"_"+currentLevel;
        byteCode.Add(new Instruction("STORE_FAST", va));
        currentLevel++;
        scopeStack.Add(new Dictionary<string, string>());
        Visit(context.sequence());
        currentLevel--;
        scopeStack.RemoveAt(scopeStack.Count - 1);
        return null;
    }

    public override object VisitReturnStatement(MiniPythonParser.ReturnStatementContext context)
    {
        Visit(context.expression());
        byteCode.Add(new Instruction("RETURN_VALUE"));
        return null;
    }

    public override object VisitPrintStatement(MiniPythonParser.PrintStatementContext context)
    {
        int numArgs = 0;
        if (context.expression() != null)
        {
            foreach (var expr in context.expression().children)
            {
                Visit(expr);
                numArgs++;
            }
        }
        byteCode.Add(new Instruction("LOAD_GLOBAL", context.PRINT().GetText()));
        byteCode.Add(new Instruction("CALL_FUNCTION", numArgs.ToString()));
        return null;
    }

    public override object VisitAssignStatement(MiniPythonParser.AssignStatementContext context)
    {
        var va = context.ID().GetText()+"_"+currentLevel;
        if (currentLevel == 0) {
            scopeStack[currentLevel].Add(context.ID().GetText(), va);
            byteCode.Add(new Instruction("PUSH_GLOBAL", va));    
        }else{
            scopeStack[currentLevel].Add(context.ID().GetText(), va);
            byteCode.Add(new Instruction("PUSH_LOCAL", va));
        }
        Visit(context.expression());
        if (currentLevel == 0)
        {
            byteCode.Add(new Instruction("STORE_GLOBAL", va));
        }else
        {
            byteCode.Add(new Instruction("STORE_FAST", va));    
        }
        
        return null;
    }

    public override object VisitFunctionCallStatement(MiniPythonParser.FunctionCallStatementContext context)
    {
        var numArgs = 0;
        foreach (var expr in context.expressionList().expression())
        {
            Visit(expr);
            numArgs++;
        }
        byteCode.Add(new Instruction("LOAD_GLOBAL", context.ID().GetText()));
        byteCode.Add(new Instruction("CALL_FUNCTION", numArgs.ToString()));
        return null;
    }

    public override object VisitSequence(MiniPythonParser.SequenceContext context)
    {
        return base.VisitSequence(context);
    }

    public override object VisitExpression(MiniPythonParser.ExpressionContext context)
    {
        return base.VisitExpression(context);
    }

    public override object VisitComparison(MiniPythonParser.ComparisonContext context)
    {
        Visit(context.additionExpression());
        byteCode.Add(new Instruction("COMPARE_OP", context.GetChild(0).GetText()));
        return null;
    }

    public override object VisitAdditionExpression(MiniPythonParser.AdditionExpressionContext context)
    {
        Visit(context.multiplicationExpression(0));
        for (int i = 1; i < context.multiplicationExpression().Length; i++) {
            Visit(context.multiplicationExpression(i));
            var operatorToken = context.GetChild((i * 2) - 1).GetText();
            if (operatorToken == "+") {
                byteCode.Add(new Instruction("BINARY_ADD"));
            } else if (operatorToken == "-") {
                byteCode.Add(new Instruction("BINARY_SUBTRACT"));
            }
        }
        return null;
    }

    public override object VisitMultiplicationExpression(MiniPythonParser.MultiplicationExpressionContext context)
    {
        Visit(context.elementExpression(0));
        for (int i = 1; i < context.elementExpression().Length; i++) {
            Visit(context.elementExpression(i));
            var operatorToken = context.GetChild((i * 2) - 1).GetText();
            if (operatorToken == "*") {
                byteCode.Add(new Instruction("BINARY_MULTIPLY"));
            } else if (operatorToken == "/") {
                byteCode.Add(new Instruction("BINARY_DIVIDE"));
            }
        }
        return null;
    }

    public override object VisitElementExpression(MiniPythonParser.ElementExpressionContext context)
    {
        Visit(context.primitiveExpression());
        if (context.LBRACKET() != null && context.expression() != null)
        {
            Visit(context.expression());
            byteCode.Add(new Instruction("BINARY_SUBSCR"));
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
        Visit(context.expression());
        byteCode.Add(new Instruction("LOAD_GLOBAL", "len"));
        byteCode.Add(new Instruction("CALL_FUNCTION", "1"));
        return null;
    }

    public override object VisitPrimitiveExpressionListExprAST(MiniPythonParser.PrimitiveExpressionListExprASTContext context)
    {
        return base.VisitPrimitiveExpressionListExprAST(context);
    }

    public override object VisitPrimitiveExpressionLiteralAST(MiniPythonParser.PrimitiveExpressionLiteralASTContext context)
    {
        string literal = context.GetText();
        byteCode.Add(new Instruction("LOAD_CONST", literal));
        return null;
    }

    public override object VisitPrimitiveExpressionExprListAST(MiniPythonParser.PrimitiveExpressionExprListASTContext context)
    {
        var identifier = context.ID().GetText();
        string va = null;
        var cantEle = -1;
        for (int i = currentLevel; i >= 0; i--)
        {
            if (scopeStack[i].ContainsKey(identifier))
            {
                va = scopeStack[i][identifier];
                cantEle = i;       
                break;
            }
        } 
        if (context.expressionList() != null)
        {
            var numArgs = 0;
            foreach (var expr in context.expressionList().expression())
            {
                Visit(expr);
                numArgs++;
            }
            byteCode.Add(new Instruction("LOAD_GLOBAL", identifier));
            byteCode.Add(new Instruction("CALL_FUNCTION", numArgs.ToString()));
        }
        else
        {
            if (cantEle == 0) {
                byteCode.Add(new Instruction("LOAD_GLOBAL", va));
            }
            else
            {
                byteCode.Add(new Instruction("LOAD_FAST", va));
            }
        }
        return null;
    }

    public override object VisitListExpression(MiniPythonParser.ListExpressionContext context)
    {
        var cantEle = 0;
        foreach (var expr in context.expressionList().expression())
        {
            Visit(expr);
            cantEle++;
        }
        byteCode.Add(new Instruction("BUILD_LIST", cantEle.ToString()));
        return null;
    }

    public override string ToString() {
        var sb = new StringBuilder();
        int cont = 0;
        foreach (Instruction i in byteCode) {
            sb.AppendLine($"{cont++} {i}");
        }
        return sb.ToString();
    }
}