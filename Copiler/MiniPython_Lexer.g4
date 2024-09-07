lexer grammar MiniPython_Lexer;

WS              : [ \t\r\n]+ -> skip ;
COMMENT         : '#' ~[\r\n]* -> skip ;
MULTILINE_COMMENT: '"""' .*? '"""' -> skip;

DEF             : 'def' ;
RETURN          : 'return' ;
IF              : 'if' ;
ELSE            : 'else' ;
WHILE           : 'while' ;
FOR             : 'for' ;
IN              : 'in' ;
PRINT           : 'print' ;
LEN             : 'len' ;

PLUS            : '+' ;
MINUS           : '-' ;
MULT            : '*' ;
DIV             : '/' ;
EQ              : '==' ;
NE              : '!=' ;
LT              : '<' ;
LE              : '<=' ;
GT              : '>' ;
GE              : '>=' ;
ASSIGN          : '=' ;

LBRACK          : '[' ;
RBRACK          : ']' ;
LPAREN          : '(' ;
RPAREN          : ')' ;
COLON           : ':' ;
COMMA           : ',' ;

INTEGER         : [0-9]+ ;
FLOAT           : [0-9]+ '.' [0-9]+ ;
STRING          : '"' .*? '"' ;
CHAR            : '\'' . '\'' ;

IDENTIFIER      : [a-zA-Z_][a-zA-Z_0-9]* ;

INDENT          : '    ' -> channel(HIDDEN) ;
DEDENT          : '<DEDENT>' -> channel(HIDDEN);
NEWLINE         : '\r'? '\n' ;