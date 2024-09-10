parser grammar MiniPythonParser;

options { tokenVocab = MiniPythonLexer; }

// Definición de un programa
program: statement+ EOF;

// Declaraciones de diferentes tipos de statements
statement: defStatement
         | ifStatement
         | whileStatement
         | forStatement
         | returnStatement
         | printStatement
         | assignStatement;

// Definición de funciones
defStatement: DEF ID LPAREN argList? RPAREN COLON block;

// Estructura de control 'if-else'
ifStatement: IF expression COLON block (ELSE COLON block)?;

// Ciclo while
whileStatement: WHILE expression COLON block;

// Ciclo for
forStatement: FOR ID IN expression COLON block;

// Return statement
returnStatement: RETURN expression;

// Print statement
printStatement: PRINT expression;

// Asignación de variables
assignStatement: ID ASSIGN expression;

// Definición de bloques
block: INDENT statement+ DEDENT;

// Expresiones y operadores
expression: additionExpression ((LT | GT | EQ | NEQ | LTEQ | GTEQ) additionExpression)?;

additionExpression: multiplicationExpression ((PLUS | MINUS) multiplicationExpression)*;

multiplicationExpression: primaryExpression ((MUL | DIV) primaryExpression)*;

primaryExpression: INT | FLOAT | STRING | ID | LPAREN expression RPAREN;

// Argumentos para funciones
argList: ID (COMMA ID)*;