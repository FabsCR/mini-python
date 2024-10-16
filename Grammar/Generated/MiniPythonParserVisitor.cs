//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:/projects/mini-python/Grammar/MiniPythonParser.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Generated {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="MiniPythonParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IMiniPythonParserVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProgram([NotNull] MiniPythonParser.ProgramContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.mainStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMainStatement([NotNull] MiniPythonParser.MainStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] MiniPythonParser.StatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.defStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDefStatement([NotNull] MiniPythonParser.DefStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.argList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgList([NotNull] MiniPythonParser.ArgListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfStatement([NotNull] MiniPythonParser.IfStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.whileStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhileStatement([NotNull] MiniPythonParser.WhileStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.forStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitForStatement([NotNull] MiniPythonParser.ForStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.returnStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReturnStatement([NotNull] MiniPythonParser.ReturnStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.printStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrintStatement([NotNull] MiniPythonParser.PrintStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.assignStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignStatement([NotNull] MiniPythonParser.AssignStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.functionCallStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionCallStatement([NotNull] MiniPythonParser.FunctionCallStatementContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.sequence"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSequence([NotNull] MiniPythonParser.SequenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] MiniPythonParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.comparison"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComparison([NotNull] MiniPythonParser.ComparisonContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.additionExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAdditionExpression([NotNull] MiniPythonParser.AdditionExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.multiplicationExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultiplicationExpression([NotNull] MiniPythonParser.MultiplicationExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.elementExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitElementExpression([NotNull] MiniPythonParser.ElementExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.expressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpressionList([NotNull] MiniPythonParser.ExpressionListContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>primitiveExpressionParenExprAST</c>
	/// labeled alternative in <see cref="MiniPythonParser.primitiveExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrimitiveExpressionParenExprAST([NotNull] MiniPythonParser.PrimitiveExpressionParenExprASTContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>primitiveExpressionLenAST</c>
	/// labeled alternative in <see cref="MiniPythonParser.primitiveExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrimitiveExpressionLenAST([NotNull] MiniPythonParser.PrimitiveExpressionLenASTContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>primitiveExpressionListExprAST</c>
	/// labeled alternative in <see cref="MiniPythonParser.primitiveExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrimitiveExpressionListExprAST([NotNull] MiniPythonParser.PrimitiveExpressionListExprASTContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>primitiveExpressionLiteralAST</c>
	/// labeled alternative in <see cref="MiniPythonParser.primitiveExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrimitiveExpressionLiteralAST([NotNull] MiniPythonParser.PrimitiveExpressionLiteralASTContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>primitiveExpressionExprListAST</c>
	/// labeled alternative in <see cref="MiniPythonParser.primitiveExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrimitiveExpressionExprListAST([NotNull] MiniPythonParser.PrimitiveExpressionExprListASTContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MiniPythonParser.listExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitListExpression([NotNull] MiniPythonParser.ListExpressionContext context);
}
} // namespace Generated