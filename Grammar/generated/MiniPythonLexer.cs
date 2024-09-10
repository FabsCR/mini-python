//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:/Master/TEC/2024 Semestre II/Compiladores e Interpretes/Proyectos/MiniPython/Grammar/MiniPythonLexer.g4 by ANTLR 4.13.1

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
		"NEWLINE", "COMMENT", "MULTILINE_COMMENT", "WS", "DEF", "IF", "ELSE", 
		"WHILE", "FOR", "RETURN", "PRINT", "IN", "LEN", "PLUS", "MINUS", "MULT", 
		"DIV", "LW", "GT", "LWEQ", "GTEQ", "EQEQ", "ASSIGN", "COMMA", "LPAREN", 
		"RPAREN", "LBRACKET", "RBRACKET", "LBRACE", "RBRACE", "DOSPUN", "INT", 
		"FLOAT", "CHARCONST", "STRING", "ID"
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
		4,0,38,232,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,1,0,3,0,75,8,0,1,0,1,0,5,0,79,8,0,10,0,12,0,82,9,0,1,1,1,1,5,1,86,
		8,1,10,1,12,1,89,9,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,5,2,98,8,2,10,2,12,2,
		101,9,2,1,2,1,2,1,2,1,2,1,2,1,2,1,3,4,3,110,8,3,11,3,12,3,111,1,3,1,3,
		1,4,1,4,1,4,1,4,1,5,1,5,1,5,1,6,1,6,1,6,1,6,1,6,1,7,1,7,1,7,1,7,1,7,1,
		7,1,8,1,8,1,8,1,8,1,9,1,9,1,9,1,9,1,9,1,9,1,9,1,10,1,10,1,10,1,10,1,10,
		1,10,1,11,1,11,1,11,1,12,1,12,1,12,1,12,1,13,1,13,1,14,1,14,1,15,1,15,
		1,16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,1,19,1,20,1,20,1,20,1,21,1,21,
		1,21,1,22,1,22,1,23,1,23,1,24,1,24,1,25,1,25,1,26,1,26,1,27,1,27,1,28,
		1,28,1,29,1,29,1,30,1,30,1,31,4,31,198,8,31,11,31,12,31,199,1,32,4,32,
		203,8,32,11,32,12,32,204,1,32,1,32,4,32,209,8,32,11,32,12,32,210,1,33,
		1,33,1,33,1,33,1,34,1,34,5,34,219,8,34,10,34,12,34,222,9,34,1,34,1,34,
		1,35,1,35,5,35,228,8,35,10,35,12,35,231,9,35,2,99,220,0,36,1,3,3,4,5,5,
		7,6,9,7,11,8,13,9,15,10,17,11,19,12,21,13,23,14,25,15,27,16,29,17,31,18,
		33,19,35,20,37,21,39,22,41,23,43,24,45,25,47,26,49,27,51,28,53,29,55,30,
		57,31,59,32,61,33,63,34,65,35,67,36,69,37,71,38,1,0,5,2,0,10,10,13,13,
		2,0,9,9,32,32,1,0,48,57,3,0,65,90,95,95,97,122,4,0,48,57,65,90,95,95,97,
		122,241,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,
		11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,
		0,0,0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,
		0,33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,
		1,0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,
		0,0,55,1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,
		1,0,0,0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,1,74,1,0,0,0,3,83,1,0,0,
		0,5,92,1,0,0,0,7,109,1,0,0,0,9,115,1,0,0,0,11,119,1,0,0,0,13,122,1,0,0,
		0,15,127,1,0,0,0,17,133,1,0,0,0,19,137,1,0,0,0,21,144,1,0,0,0,23,150,1,
		0,0,0,25,153,1,0,0,0,27,157,1,0,0,0,29,159,1,0,0,0,31,161,1,0,0,0,33,163,
		1,0,0,0,35,165,1,0,0,0,37,167,1,0,0,0,39,169,1,0,0,0,41,172,1,0,0,0,43,
		175,1,0,0,0,45,178,1,0,0,0,47,180,1,0,0,0,49,182,1,0,0,0,51,184,1,0,0,
		0,53,186,1,0,0,0,55,188,1,0,0,0,57,190,1,0,0,0,59,192,1,0,0,0,61,194,1,
		0,0,0,63,197,1,0,0,0,65,202,1,0,0,0,67,212,1,0,0,0,69,216,1,0,0,0,71,225,
		1,0,0,0,73,75,5,13,0,0,74,73,1,0,0,0,74,75,1,0,0,0,75,76,1,0,0,0,76,80,
		5,10,0,0,77,79,5,32,0,0,78,77,1,0,0,0,79,82,1,0,0,0,80,78,1,0,0,0,80,81,
		1,0,0,0,81,2,1,0,0,0,82,80,1,0,0,0,83,87,5,35,0,0,84,86,8,0,0,0,85,84,
		1,0,0,0,86,89,1,0,0,0,87,85,1,0,0,0,87,88,1,0,0,0,88,90,1,0,0,0,89,87,
		1,0,0,0,90,91,6,1,0,0,91,4,1,0,0,0,92,93,5,34,0,0,93,94,5,34,0,0,94,95,
		5,34,0,0,95,99,1,0,0,0,96,98,9,0,0,0,97,96,1,0,0,0,98,101,1,0,0,0,99,100,
		1,0,0,0,99,97,1,0,0,0,100,102,1,0,0,0,101,99,1,0,0,0,102,103,5,34,0,0,
		103,104,5,34,0,0,104,105,5,34,0,0,105,106,1,0,0,0,106,107,6,2,0,0,107,
		6,1,0,0,0,108,110,7,1,0,0,109,108,1,0,0,0,110,111,1,0,0,0,111,109,1,0,
		0,0,111,112,1,0,0,0,112,113,1,0,0,0,113,114,6,3,1,0,114,8,1,0,0,0,115,
		116,5,100,0,0,116,117,5,101,0,0,117,118,5,102,0,0,118,10,1,0,0,0,119,120,
		5,105,0,0,120,121,5,102,0,0,121,12,1,0,0,0,122,123,5,101,0,0,123,124,5,
		108,0,0,124,125,5,115,0,0,125,126,5,101,0,0,126,14,1,0,0,0,127,128,5,119,
		0,0,128,129,5,104,0,0,129,130,5,105,0,0,130,131,5,108,0,0,131,132,5,101,
		0,0,132,16,1,0,0,0,133,134,5,102,0,0,134,135,5,111,0,0,135,136,5,114,0,
		0,136,18,1,0,0,0,137,138,5,114,0,0,138,139,5,101,0,0,139,140,5,116,0,0,
		140,141,5,117,0,0,141,142,5,114,0,0,142,143,5,110,0,0,143,20,1,0,0,0,144,
		145,5,112,0,0,145,146,5,114,0,0,146,147,5,105,0,0,147,148,5,110,0,0,148,
		149,5,116,0,0,149,22,1,0,0,0,150,151,5,105,0,0,151,152,5,110,0,0,152,24,
		1,0,0,0,153,154,5,108,0,0,154,155,5,101,0,0,155,156,5,110,0,0,156,26,1,
		0,0,0,157,158,5,43,0,0,158,28,1,0,0,0,159,160,5,45,0,0,160,30,1,0,0,0,
		161,162,5,42,0,0,162,32,1,0,0,0,163,164,5,47,0,0,164,34,1,0,0,0,165,166,
		5,60,0,0,166,36,1,0,0,0,167,168,5,62,0,0,168,38,1,0,0,0,169,170,5,60,0,
		0,170,171,5,61,0,0,171,40,1,0,0,0,172,173,5,62,0,0,173,174,5,61,0,0,174,
		42,1,0,0,0,175,176,5,61,0,0,176,177,5,61,0,0,177,44,1,0,0,0,178,179,5,
		61,0,0,179,46,1,0,0,0,180,181,5,44,0,0,181,48,1,0,0,0,182,183,5,40,0,0,
		183,50,1,0,0,0,184,185,5,41,0,0,185,52,1,0,0,0,186,187,5,91,0,0,187,54,
		1,0,0,0,188,189,5,93,0,0,189,56,1,0,0,0,190,191,5,123,0,0,191,58,1,0,0,
		0,192,193,5,125,0,0,193,60,1,0,0,0,194,195,5,58,0,0,195,62,1,0,0,0,196,
		198,7,2,0,0,197,196,1,0,0,0,198,199,1,0,0,0,199,197,1,0,0,0,199,200,1,
		0,0,0,200,64,1,0,0,0,201,203,7,2,0,0,202,201,1,0,0,0,203,204,1,0,0,0,204,
		202,1,0,0,0,204,205,1,0,0,0,205,206,1,0,0,0,206,208,5,46,0,0,207,209,7,
		2,0,0,208,207,1,0,0,0,209,210,1,0,0,0,210,208,1,0,0,0,210,211,1,0,0,0,
		211,66,1,0,0,0,212,213,5,39,0,0,213,214,9,0,0,0,214,215,5,39,0,0,215,68,
		1,0,0,0,216,220,5,34,0,0,217,219,9,0,0,0,218,217,1,0,0,0,219,222,1,0,0,
		0,220,221,1,0,0,0,220,218,1,0,0,0,221,223,1,0,0,0,222,220,1,0,0,0,223,
		224,5,34,0,0,224,70,1,0,0,0,225,229,7,3,0,0,226,228,7,4,0,0,227,226,1,
		0,0,0,228,231,1,0,0,0,229,227,1,0,0,0,229,230,1,0,0,0,230,72,1,0,0,0,231,
		229,1,0,0,0,11,0,74,80,87,99,111,199,204,210,220,229,2,0,1,0,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace generated