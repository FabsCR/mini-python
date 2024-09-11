//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/Justin Martínez/Documentos/Escritorio/2do semestre 2024/Compi/proyecto/mini-python/Grammar/MiniPythonLexer.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace generated {

using AntlrDenter;

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class MiniPythonLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		INDENT=1, DEDENT=2, NEWLINE=3, COMMENT=4, MULTILINE_COMMENT=5, WS=6, DEF=7, 
		IF=8, ELSE=9, WHILE=10, FOR=11, RETURN=12, PRINT=13, IN=14, LEN=15, PLUS=16, 
		MINUS=17, MULT=18, DIV=19, LW=20, GT=21, LWEQ=22, GTEQ=23, EQEQ=24, ASSIGN=25, 
		COMMA=26, LPAREN=27, RPAREN=28, LBRACKET=29, RBRACKET=30, LBRACE=31, RBRACE=32, 
		DOSPUN=33, INT=34, FLOAT=35, CHARCONST=36, STRING=37, ID=38;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"ESC_SEQ", "NEWLINE", "COMMENT", "MULTILINE_COMMENT", "WS", "DEF", "IF", 
		"ELSE", "WHILE", "FOR", "RETURN", "PRINT", "IN", "LEN", "PLUS", "MINUS", 
		"MULT", "DIV", "LW", "GT", "LWEQ", "GTEQ", "EQEQ", "ASSIGN", "COMMA", 
		"LPAREN", "RPAREN", "LBRACKET", "RBRACKET", "LBRACE", "RBRACE", "DOSPUN", 
		"INT", "FLOAT", "CHARCONST", "STRING", "ID"
	};


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


	public MiniPythonLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public MiniPythonLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, null, "'def'", "'if'", "'else'", "'while'", 
		"'for'", "'return'", "'print'", "'in'", "'len'", "'+'", "'-'", "'*'", 
		"'/'", "'<'", "'>'", "'<='", "'>='", "'=='", "'='", "','", "'('", "')'", 
		"'['", "']'", "'{'", "'}'", "':'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "INDENT", "DEDENT", "NEWLINE", "COMMENT", "MULTILINE_COMMENT", "WS", 
		"DEF", "IF", "ELSE", "WHILE", "FOR", "RETURN", "PRINT", "IN", "LEN", "PLUS", 
		"MINUS", "MULT", "DIV", "LW", "GT", "LWEQ", "GTEQ", "EQEQ", "ASSIGN", 
		"COMMA", "LPAREN", "RPAREN", "LBRACKET", "RBRACKET", "LBRACE", "RBRACE", 
		"DOSPUN", "INT", "FLOAT", "CHARCONST", "STRING", "ID"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "MiniPythonLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static MiniPythonLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,38,240,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,2,36,7,36,1,0,1,0,1,0,1,1,3,1,80,8,1,1,1,1,1,5,1,84,8,1,10,1,12,1,
		87,9,1,1,2,1,2,5,2,91,8,2,10,2,12,2,94,9,2,1,2,1,2,1,3,1,3,1,3,1,3,1,3,
		5,3,103,8,3,10,3,12,3,106,9,3,1,3,1,3,1,3,1,3,1,3,1,3,1,4,4,4,115,8,4,
		11,4,12,4,116,1,4,1,4,1,5,1,5,1,5,1,5,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,
		1,8,1,8,1,8,1,8,1,8,1,8,1,9,1,9,1,9,1,9,1,10,1,10,1,10,1,10,1,10,1,10,
		1,10,1,11,1,11,1,11,1,11,1,11,1,11,1,12,1,12,1,12,1,13,1,13,1,13,1,13,
		1,14,1,14,1,15,1,15,1,16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,1,20,1,20,
		1,20,1,21,1,21,1,21,1,22,1,22,1,22,1,23,1,23,1,24,1,24,1,25,1,25,1,26,
		1,26,1,27,1,27,1,28,1,28,1,29,1,29,1,30,1,30,1,31,1,31,1,32,4,32,203,8,
		32,11,32,12,32,204,1,33,4,33,208,8,33,11,33,12,33,209,1,33,1,33,4,33,214,
		8,33,11,33,12,33,215,1,34,1,34,1,34,3,34,221,8,34,1,34,1,34,1,35,1,35,
		5,35,227,8,35,10,35,12,35,230,9,35,1,35,1,35,1,36,1,36,5,36,236,8,36,10,
		36,12,36,239,9,36,2,104,228,0,37,1,0,3,3,5,4,7,5,9,6,11,7,13,8,15,9,17,
		10,19,11,21,12,23,13,25,14,27,15,29,16,31,17,33,18,35,19,37,20,39,21,41,
		22,43,23,45,24,47,25,49,26,51,27,53,28,55,29,57,30,59,31,61,32,63,33,65,
		34,67,35,69,36,71,37,73,38,1,0,6,7,0,34,34,92,92,98,98,102,102,110,110,
		114,114,116,116,2,0,10,10,13,13,2,0,9,9,32,32,1,0,48,57,3,0,65,90,95,95,
		97,122,4,0,48,57,65,90,95,95,97,122,249,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,
		0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,
		0,19,1,0,0,0,0,21,1,0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,
		1,0,0,0,0,31,1,0,0,0,0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,
		0,0,41,1,0,0,0,0,43,1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,
		1,0,0,0,0,53,1,0,0,0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,
		0,0,63,1,0,0,0,0,65,1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,0,73,
		1,0,0,0,1,75,1,0,0,0,3,79,1,0,0,0,5,88,1,0,0,0,7,97,1,0,0,0,9,114,1,0,
		0,0,11,120,1,0,0,0,13,124,1,0,0,0,15,127,1,0,0,0,17,132,1,0,0,0,19,138,
		1,0,0,0,21,142,1,0,0,0,23,149,1,0,0,0,25,155,1,0,0,0,27,158,1,0,0,0,29,
		162,1,0,0,0,31,164,1,0,0,0,33,166,1,0,0,0,35,168,1,0,0,0,37,170,1,0,0,
		0,39,172,1,0,0,0,41,174,1,0,0,0,43,177,1,0,0,0,45,180,1,0,0,0,47,183,1,
		0,0,0,49,185,1,0,0,0,51,187,1,0,0,0,53,189,1,0,0,0,55,191,1,0,0,0,57,193,
		1,0,0,0,59,195,1,0,0,0,61,197,1,0,0,0,63,199,1,0,0,0,65,202,1,0,0,0,67,
		207,1,0,0,0,69,217,1,0,0,0,71,224,1,0,0,0,73,233,1,0,0,0,75,76,5,92,0,
		0,76,77,7,0,0,0,77,2,1,0,0,0,78,80,5,13,0,0,79,78,1,0,0,0,79,80,1,0,0,
		0,80,81,1,0,0,0,81,85,5,10,0,0,82,84,5,32,0,0,83,82,1,0,0,0,84,87,1,0,
		0,0,85,83,1,0,0,0,85,86,1,0,0,0,86,4,1,0,0,0,87,85,1,0,0,0,88,92,5,35,
		0,0,89,91,8,1,0,0,90,89,1,0,0,0,91,94,1,0,0,0,92,90,1,0,0,0,92,93,1,0,
		0,0,93,95,1,0,0,0,94,92,1,0,0,0,95,96,6,2,0,0,96,6,1,0,0,0,97,98,5,34,
		0,0,98,99,5,34,0,0,99,100,5,34,0,0,100,104,1,0,0,0,101,103,9,0,0,0,102,
		101,1,0,0,0,103,106,1,0,0,0,104,105,1,0,0,0,104,102,1,0,0,0,105,107,1,
		0,0,0,106,104,1,0,0,0,107,108,5,34,0,0,108,109,5,34,0,0,109,110,5,34,0,
		0,110,111,1,0,0,0,111,112,6,3,0,0,112,8,1,0,0,0,113,115,7,2,0,0,114,113,
		1,0,0,0,115,116,1,0,0,0,116,114,1,0,0,0,116,117,1,0,0,0,117,118,1,0,0,
		0,118,119,6,4,1,0,119,10,1,0,0,0,120,121,5,100,0,0,121,122,5,101,0,0,122,
		123,5,102,0,0,123,12,1,0,0,0,124,125,5,105,0,0,125,126,5,102,0,0,126,14,
		1,0,0,0,127,128,5,101,0,0,128,129,5,108,0,0,129,130,5,115,0,0,130,131,
		5,101,0,0,131,16,1,0,0,0,132,133,5,119,0,0,133,134,5,104,0,0,134,135,5,
		105,0,0,135,136,5,108,0,0,136,137,5,101,0,0,137,18,1,0,0,0,138,139,5,102,
		0,0,139,140,5,111,0,0,140,141,5,114,0,0,141,20,1,0,0,0,142,143,5,114,0,
		0,143,144,5,101,0,0,144,145,5,116,0,0,145,146,5,117,0,0,146,147,5,114,
		0,0,147,148,5,110,0,0,148,22,1,0,0,0,149,150,5,112,0,0,150,151,5,114,0,
		0,151,152,5,105,0,0,152,153,5,110,0,0,153,154,5,116,0,0,154,24,1,0,0,0,
		155,156,5,105,0,0,156,157,5,110,0,0,157,26,1,0,0,0,158,159,5,108,0,0,159,
		160,5,101,0,0,160,161,5,110,0,0,161,28,1,0,0,0,162,163,5,43,0,0,163,30,
		1,0,0,0,164,165,5,45,0,0,165,32,1,0,0,0,166,167,5,42,0,0,167,34,1,0,0,
		0,168,169,5,47,0,0,169,36,1,0,0,0,170,171,5,60,0,0,171,38,1,0,0,0,172,
		173,5,62,0,0,173,40,1,0,0,0,174,175,5,60,0,0,175,176,5,61,0,0,176,42,1,
		0,0,0,177,178,5,62,0,0,178,179,5,61,0,0,179,44,1,0,0,0,180,181,5,61,0,
		0,181,182,5,61,0,0,182,46,1,0,0,0,183,184,5,61,0,0,184,48,1,0,0,0,185,
		186,5,44,0,0,186,50,1,0,0,0,187,188,5,40,0,0,188,52,1,0,0,0,189,190,5,
		41,0,0,190,54,1,0,0,0,191,192,5,91,0,0,192,56,1,0,0,0,193,194,5,93,0,0,
		194,58,1,0,0,0,195,196,5,123,0,0,196,60,1,0,0,0,197,198,5,125,0,0,198,
		62,1,0,0,0,199,200,5,58,0,0,200,64,1,0,0,0,201,203,7,3,0,0,202,201,1,0,
		0,0,203,204,1,0,0,0,204,202,1,0,0,0,204,205,1,0,0,0,205,66,1,0,0,0,206,
		208,7,3,0,0,207,206,1,0,0,0,208,209,1,0,0,0,209,207,1,0,0,0,209,210,1,
		0,0,0,210,211,1,0,0,0,211,213,5,46,0,0,212,214,7,3,0,0,213,212,1,0,0,0,
		214,215,1,0,0,0,215,213,1,0,0,0,215,216,1,0,0,0,216,68,1,0,0,0,217,220,
		5,39,0,0,218,221,3,1,0,0,219,221,9,0,0,0,220,218,1,0,0,0,220,219,1,0,0,
		0,221,222,1,0,0,0,222,223,5,39,0,0,223,70,1,0,0,0,224,228,5,34,0,0,225,
		227,9,0,0,0,226,225,1,0,0,0,227,230,1,0,0,0,228,229,1,0,0,0,228,226,1,
		0,0,0,229,231,1,0,0,0,230,228,1,0,0,0,231,232,5,34,0,0,232,72,1,0,0,0,
		233,237,7,4,0,0,234,236,7,5,0,0,235,234,1,0,0,0,236,239,1,0,0,0,237,235,
		1,0,0,0,237,238,1,0,0,0,238,74,1,0,0,0,239,237,1,0,0,0,12,0,79,85,92,104,
		116,204,209,215,220,228,237,2,0,1,0,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace generated
