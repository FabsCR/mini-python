//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /home/fabs/RiderProjects/MiniPython/MiniPython/Parser/MiniPythonLexer.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

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
		INDENT=1, DEDENT=2, NEWLINE=3, COMMENT=4, MULTILINE_COMMENT=5, WS=6, PLUS=7, 
		MINUS=8, MUL=9, DIV=10, ASSIGN=11, EQ=12, NEQ=13, LT=14, GT=15, LTEQ=16, 
		GTEQ=17, LPAREN=18, RPAREN=19, LBRACKET=20, RBRACKET=21, COLON=22, COMMA=23, 
		DEF=24, IF=25, ELSE=26, WHILE=27, RETURN=28, PRINT=29, FOR=30, IN=31, 
		LEN=32, ID=33, INT=34, FLOAT=35, STRING=36;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"INDENT", "DEDENT", "NEWLINE", "COMMENT", "MULTILINE_COMMENT", "WS", "PLUS", 
		"MINUS", "MUL", "DIV", "ASSIGN", "EQ", "NEQ", "LT", "GT", "LTEQ", "GTEQ", 
		"LPAREN", "RPAREN", "LBRACKET", "RBRACKET", "COLON", "COMMA", "DEF", "IF", 
		"ELSE", "WHILE", "RETURN", "PRINT", "FOR", "IN", "LEN", "ID", "INT", "FLOAT", 
		"STRING"
	};


	public MiniPythonLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public MiniPythonLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, null, "'+'", "'-'", "'*'", "'/'", 
		"'='", "'=='", "'!='", "'<'", "'>'", "'<='", "'>='", "'('", "')'", "'['", 
		"']'", "':'", "','", "'def'", "'if'", "'else'", "'while'", "'return'", 
		"'print'", "'for'", "'in'", "'len'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "INDENT", "DEDENT", "NEWLINE", "COMMENT", "MULTILINE_COMMENT", "WS", 
		"PLUS", "MINUS", "MUL", "DIV", "ASSIGN", "EQ", "NEQ", "LT", "GT", "LTEQ", 
		"GTEQ", "LPAREN", "RPAREN", "LBRACKET", "RBRACKET", "COLON", "COMMA", 
		"DEF", "IF", "ELSE", "WHILE", "RETURN", "PRINT", "FOR", "IN", "LEN", "ID", 
		"INT", "FLOAT", "STRING"
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
		4,0,36,238,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,2,29,7,29,2,30,7,30,2,31,7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,
		7,35,1,0,4,0,75,8,0,11,0,12,0,76,1,0,1,0,1,1,4,1,82,8,1,11,1,12,1,83,1,
		1,1,1,1,2,4,2,89,8,2,11,2,12,2,90,1,2,1,2,1,3,1,3,5,3,97,8,3,10,3,12,3,
		100,9,3,1,3,1,3,1,4,1,4,1,4,1,4,1,4,5,4,109,8,4,10,4,12,4,112,9,4,1,4,
		1,4,1,4,1,4,1,4,1,4,1,5,4,5,121,8,5,11,5,12,5,122,1,5,1,5,1,6,1,6,1,7,
		1,7,1,8,1,8,1,9,1,9,1,10,1,10,1,11,1,11,1,11,1,12,1,12,1,12,1,13,1,13,
		1,14,1,14,1,15,1,15,1,15,1,16,1,16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,
		1,20,1,20,1,21,1,21,1,22,1,22,1,23,1,23,1,23,1,23,1,24,1,24,1,24,1,25,
		1,25,1,25,1,25,1,25,1,26,1,26,1,26,1,26,1,26,1,26,1,27,1,27,1,27,1,27,
		1,27,1,27,1,27,1,28,1,28,1,28,1,28,1,28,1,28,1,29,1,29,1,29,1,29,1,30,
		1,30,1,30,1,31,1,31,1,31,1,31,1,32,1,32,5,32,209,8,32,10,32,12,32,212,
		9,32,1,33,4,33,215,8,33,11,33,12,33,216,1,34,4,34,220,8,34,11,34,12,34,
		221,1,34,1,34,4,34,226,8,34,11,34,12,34,227,1,35,1,35,5,35,232,8,35,10,
		35,12,35,235,9,35,1,35,1,35,2,110,233,0,36,1,1,3,2,5,3,7,4,9,5,11,6,13,
		7,15,8,17,9,19,10,21,11,23,12,25,13,27,14,29,15,31,16,33,17,35,18,37,19,
		39,20,41,21,43,22,45,23,47,24,49,25,51,26,53,27,55,28,57,29,59,30,61,31,
		63,32,65,33,67,34,69,35,71,36,1,0,6,1,0,9,9,2,0,10,10,13,13,3,0,9,10,13,
		13,32,32,3,0,65,90,95,95,97,122,4,0,48,57,65,90,95,95,97,122,1,0,48,57,
		248,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,
		0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,0,0,21,1,0,0,0,
		0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,
		1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,
		0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,
		1,0,0,0,0,57,1,0,0,0,0,59,1,0,0,0,0,61,1,0,0,0,0,63,1,0,0,0,0,65,1,0,0,
		0,0,67,1,0,0,0,0,69,1,0,0,0,0,71,1,0,0,0,1,74,1,0,0,0,3,81,1,0,0,0,5,88,
		1,0,0,0,7,94,1,0,0,0,9,103,1,0,0,0,11,120,1,0,0,0,13,126,1,0,0,0,15,128,
		1,0,0,0,17,130,1,0,0,0,19,132,1,0,0,0,21,134,1,0,0,0,23,136,1,0,0,0,25,
		139,1,0,0,0,27,142,1,0,0,0,29,144,1,0,0,0,31,146,1,0,0,0,33,149,1,0,0,
		0,35,152,1,0,0,0,37,154,1,0,0,0,39,156,1,0,0,0,41,158,1,0,0,0,43,160,1,
		0,0,0,45,162,1,0,0,0,47,164,1,0,0,0,49,168,1,0,0,0,51,171,1,0,0,0,53,176,
		1,0,0,0,55,182,1,0,0,0,57,189,1,0,0,0,59,195,1,0,0,0,61,199,1,0,0,0,63,
		202,1,0,0,0,65,206,1,0,0,0,67,214,1,0,0,0,69,219,1,0,0,0,71,229,1,0,0,
		0,73,75,7,0,0,0,74,73,1,0,0,0,75,76,1,0,0,0,76,74,1,0,0,0,76,77,1,0,0,
		0,77,78,1,0,0,0,78,79,6,0,0,0,79,2,1,0,0,0,80,82,7,1,0,0,81,80,1,0,0,0,
		82,83,1,0,0,0,83,81,1,0,0,0,83,84,1,0,0,0,84,85,1,0,0,0,85,86,6,1,0,0,
		86,4,1,0,0,0,87,89,7,1,0,0,88,87,1,0,0,0,89,90,1,0,0,0,90,88,1,0,0,0,90,
		91,1,0,0,0,91,92,1,0,0,0,92,93,6,2,0,0,93,6,1,0,0,0,94,98,5,35,0,0,95,
		97,8,1,0,0,96,95,1,0,0,0,97,100,1,0,0,0,98,96,1,0,0,0,98,99,1,0,0,0,99,
		101,1,0,0,0,100,98,1,0,0,0,101,102,6,3,1,0,102,8,1,0,0,0,103,104,5,34,
		0,0,104,105,5,34,0,0,105,106,5,34,0,0,106,110,1,0,0,0,107,109,9,0,0,0,
		108,107,1,0,0,0,109,112,1,0,0,0,110,111,1,0,0,0,110,108,1,0,0,0,111,113,
		1,0,0,0,112,110,1,0,0,0,113,114,5,34,0,0,114,115,5,34,0,0,115,116,5,34,
		0,0,116,117,1,0,0,0,117,118,6,4,1,0,118,10,1,0,0,0,119,121,7,2,0,0,120,
		119,1,0,0,0,121,122,1,0,0,0,122,120,1,0,0,0,122,123,1,0,0,0,123,124,1,
		0,0,0,124,125,6,5,1,0,125,12,1,0,0,0,126,127,5,43,0,0,127,14,1,0,0,0,128,
		129,5,45,0,0,129,16,1,0,0,0,130,131,5,42,0,0,131,18,1,0,0,0,132,133,5,
		47,0,0,133,20,1,0,0,0,134,135,5,61,0,0,135,22,1,0,0,0,136,137,5,61,0,0,
		137,138,5,61,0,0,138,24,1,0,0,0,139,140,5,33,0,0,140,141,5,61,0,0,141,
		26,1,0,0,0,142,143,5,60,0,0,143,28,1,0,0,0,144,145,5,62,0,0,145,30,1,0,
		0,0,146,147,5,60,0,0,147,148,5,61,0,0,148,32,1,0,0,0,149,150,5,62,0,0,
		150,151,5,61,0,0,151,34,1,0,0,0,152,153,5,40,0,0,153,36,1,0,0,0,154,155,
		5,41,0,0,155,38,1,0,0,0,156,157,5,91,0,0,157,40,1,0,0,0,158,159,5,93,0,
		0,159,42,1,0,0,0,160,161,5,58,0,0,161,44,1,0,0,0,162,163,5,44,0,0,163,
		46,1,0,0,0,164,165,5,100,0,0,165,166,5,101,0,0,166,167,5,102,0,0,167,48,
		1,0,0,0,168,169,5,105,0,0,169,170,5,102,0,0,170,50,1,0,0,0,171,172,5,101,
		0,0,172,173,5,108,0,0,173,174,5,115,0,0,174,175,5,101,0,0,175,52,1,0,0,
		0,176,177,5,119,0,0,177,178,5,104,0,0,178,179,5,105,0,0,179,180,5,108,
		0,0,180,181,5,101,0,0,181,54,1,0,0,0,182,183,5,114,0,0,183,184,5,101,0,
		0,184,185,5,116,0,0,185,186,5,117,0,0,186,187,5,114,0,0,187,188,5,110,
		0,0,188,56,1,0,0,0,189,190,5,112,0,0,190,191,5,114,0,0,191,192,5,105,0,
		0,192,193,5,110,0,0,193,194,5,116,0,0,194,58,1,0,0,0,195,196,5,102,0,0,
		196,197,5,111,0,0,197,198,5,114,0,0,198,60,1,0,0,0,199,200,5,105,0,0,200,
		201,5,110,0,0,201,62,1,0,0,0,202,203,5,108,0,0,203,204,5,101,0,0,204,205,
		5,110,0,0,205,64,1,0,0,0,206,210,7,3,0,0,207,209,7,4,0,0,208,207,1,0,0,
		0,209,212,1,0,0,0,210,208,1,0,0,0,210,211,1,0,0,0,211,66,1,0,0,0,212,210,
		1,0,0,0,213,215,7,5,0,0,214,213,1,0,0,0,215,216,1,0,0,0,216,214,1,0,0,
		0,216,217,1,0,0,0,217,68,1,0,0,0,218,220,7,5,0,0,219,218,1,0,0,0,220,221,
		1,0,0,0,221,219,1,0,0,0,221,222,1,0,0,0,222,223,1,0,0,0,223,225,5,46,0,
		0,224,226,7,5,0,0,225,224,1,0,0,0,226,227,1,0,0,0,227,225,1,0,0,0,227,
		228,1,0,0,0,228,70,1,0,0,0,229,233,5,34,0,0,230,232,9,0,0,0,231,230,1,
		0,0,0,232,235,1,0,0,0,233,234,1,0,0,0,233,231,1,0,0,0,234,236,1,0,0,0,
		235,233,1,0,0,0,236,237,5,34,0,0,237,72,1,0,0,0,12,0,76,83,90,98,110,122,
		210,216,221,227,233,2,0,1,0,6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
