lexer grammar MiniPythonLexer;

INDENT: [\t]+ -> channel(HIDDEN);
DEDENT: [\n\r]+ -> channel(HIDDEN);
NEWLINE: [\r\n]+ -> channel(HIDDEN);

// Comentarios
COMMENT: '#' ~[\r\n]* -> skip;
MULTILINE_COMMENT: '"""' .*? '"""' -> skip;

// Espacios y caracteres ignorados
WS: [ \t\r\n]+ -> skip;

// Operadores
PLUS: '+';
MINUS: '-';
MUL: '*';
DIV: '/';
ASSIGN: '=';
EQ: '==';
NEQ: '!=';
LT: '<';
GT: '>';
LTEQ: '<=';
GTEQ: '>=';

// Delimitadores
LPAREN: '(';
RPAREN: ')';
LBRACKET: '[';
RBRACKET: ']';
COLON: ':';
COMMA: ',';

// Palabras reservadas
DEF: 'def';
IF: 'if';
ELSE: 'else';
WHILE: 'while';
RETURN: 'return';
PRINT: 'print';
FOR: 'for';
IN: 'in';
LEN: 'len';

// Tipos de datos
ID: [a-zA-Z_][a-zA-Z0-9_]*;
INT: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
STRING: '"' .*? '"';