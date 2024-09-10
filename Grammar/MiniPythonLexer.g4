lexer grammar MiniPythonLexer;

tokens { INDENT, DEDENT }

@lexer::header {
using AntlrDenter;
}

@lexer::members {
private DenterHelper denter;
  
public override IToken NextToken()
{
    if (denter == null)
    {
        denter = DenterHelper.Builder()
            .Nl(NEWLINE)
            .Indent(MiniPythonParser.INDENT)
            .Dedent(MiniPythonParser.DEDENT)
            .PullToken(base.NextToken);
    }

    return denter.NextToken();
}
}

// El token de nueva línea debe ser visible para DenterHelper
NEWLINE: ('\r'? '\n' ' '*);

// Comentarios
COMMENT: '#' ~[\r\n]* -> channel(HIDDEN) ;
MULTILINE_COMMENT: '"""' .*? '"""' -> channel(HIDDEN) ;

// Espacios y caracteres ignorados
WS: [ \t]+ -> skip; // Solo ignoramos espacios y tabulaciones, no saltos de línea

DEF:            'def';
IF:             'if';
ELSE:           'else';
WHILE:          'while';
FOR:            'for';
RETURN:         'return';
PRINT:          'print';
IN:             'in';
LEN:            'len';

PLUS:           '+';
MINUS:          '-';
MULT:           '*';
DIV:            '/';
LW:             '<';
GT:              '>';
LWEQ:           '<=';
GTEQ:           '>=';
EQEQ:           '==';
ASSIGN:         '=';
COMMA:          ',';
LPAREN:         '(';
RPAREN:         ')';
LBRACKET:       '[';
RBRACKET:       ']';
LBRACE:         '{';
RBRACE:         '}';
DOSPUN:         ':';

INT:            [0-9]+;
FLOAT:          [0-9]+ '.' [0-9]+;
CHARCONST:      '\'' . '\'';
STRING:         '"' .*? '"';
ID:             [a-zA-Z_][a-zA-Z_0-9]*;