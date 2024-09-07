parser grammar MiniPython_Parser;

options { tokenVocab=MiniPython_Lexer; }

program
    : (mainStatement)* EOF ;

mainStatement
    : defStatement
    | assignStatement ;

statement
    : defStatement
    | ifStatement
    | returnStatement
    | printStatement
    | whileStatement
    | assignStatement
    | functionCallStatement ;

defStatement
    : DEF IDENTIFIER LPAREN argList RPAREN COLON sequence ;

argList
    : IDENTIFIER (COMMA IDENTIFIER)* | /* epsilon */ ;

ifStatement
    : IF expression COLON sequence ELSE COLON sequence ;

whileStatement
    : WHILE expression COLON sequence ;

forStatement
    : FOR expression IN expressionList COLON sequence ;

returnStatement
    : RETURN expression NEWLINE ;

printStatement
    : PRINT expression NEWLINE ;

assignStatement
    : IDENTIFIER ASSIGN expression NEWLINE ;

functionCallStatement
    : primitiveExpression LPAREN expressionList RPAREN NEWLINE ;

sequence
    : INDENT statement+ DEDENT ;

expression
    : additionExpression comparison ;

comparison
    : (LT | GT | LE | GE | EQ) additionExpression | /* epsilon */ ;

additionExpression
    : multiplicationExpression ((PLUS | MINUS) multiplicationExpression)* ;

multiplicationExpression
    : elementExpression ((MULT | DIV) elementExpression)* ;

elementExpression
    : primitiveExpression (LBRACK expression RBRACK)* ;

expressionList
    : expression (COMMA expression)* | /* epsilon */ ;

primitiveExpression
    : (MINUS)? (INTEGER | FLOAT | CHAR | STRING | IDENTIFIER | LPAREN expression RPAREN | listExpression | LEN LPAREN expression RPAREN) ;

listExpression
    : LBRACK expressionList RBRACK ;