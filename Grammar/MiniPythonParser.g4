parser grammar MiniPythonParser;

options { tokenVocab = MiniPythonLexer; }

program: mainStatement* EOF;

mainStatement: defStatement| assignStatement ;

statement: defStatement
         | ifStatement
         | returnStatement
         | printStatement
         | whileStatement
         | assignStatement
         | forStatement
         | functionCallStatement
         ;

defStatement: DEF ID LPAREN argList RPAREN DOSPUN sequence ;
argList: (ID (COMMA ID)*)?;
ifStatement: IF expression DOSPUN sequence  ELSE DOSPUN sequence;
whileStatement: WHILE expression DOSPUN  sequence;
forStatement: FOR expression IN expressionList DOSPUN sequence; 
returnStatement: RETURN expression NEWLINE;
printStatement: PRINT expression NEWLINE;
assignStatement: ID ASSIGN expression NEWLINE;
functionCallStatement: ID LPAREN expressionList RPAREN NEWLINE?;
sequence:  INDENT statement+ DEDENT ;
expression: additionExpression comparison?;
comparison: (LW | GT | LWEQ | GTEQ | EQEQ) additionExpression;
additionExpression: multiplicationExpression ((PLUS | MINUS) multiplicationExpression)*;
multiplicationExpression: elementExpression ((MULT | DIV) elementExpression)*;
elementExpression: primitiveExpression (LBRACKET expression RBRACKET)?;
expressionList: (expression (COMMA expression)*)?;
primitiveExpression : LPAREN expression RPAREN
                    | LEN LPAREN expression RPAREN
                    | listExpression
                    | (PLUS | MINUS)? (INT | FLOAT | CHARCONST | STRING)
                    | ID (LPAREN expressionList RPAREN)?
                    ;
listExpression: LBRACKET expressionList RBRACKET;