parser grammar MiniPythonParser;

options { tokenVocab = MiniPythonLexer; }

program: statement+ EOF;

statement: defStatement
         | ifStatement
         | whileStatement
         | forStatement
         | returnStatement
         | printStatement
         | assignStatement;

defStatement: DEF ID LPAREN argList? RPAREN COLON block;
ifStatement: IF expression COLON block ELSE COLON block;
whileStatement: WHILE expression COLON block;
forStatement: FOR ID IN expression COLON block;
returnStatement: RETURN expression;
printStatement: PRINT expression;
assignStatement: ID ASSIGN expression;

block: INDENT statement+ DEDENT;

expression: additionExpression ((LT|GT|EQ|NEQ|LTEQ|GTEQ) additionExpression)?;

additionExpression: multiplicationExpression ((PLUS|MINUS) multiplicationExpression)*;
multiplicationExpression: primaryExpression ((MUL|DIV) primaryExpression)*;
primaryExpression: INT | FLOAT | STRING | ID | LPAREN expression RPAREN;

argList: ID (COMMA ID)*;