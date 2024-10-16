namespace MiniPython.Grammar.Checker;

using System;
using System.Collections.Generic;
using Antlr4.Runtime;

public class SymbolsTable
{
    private List<Dictionary<string, Ident>> table;
    private int currentLevel;

    public abstract class Ident
    {
        public IToken Tok { get; }
        public SymbolType Type { get; }
        public int Level { get; }
        public int Value { get; set; }

        public Ident(IToken tok, SymbolType type, int level)
        {
            Tok = tok;
            Type = type;
            Level = level;
            Value = 0; 
        }

        public abstract void PrintIdent();
    }

    public class VarIdent : Ident
    {
        public bool IsConstant { get; }

        public VarIdent(IToken tok, SymbolType type, int level, bool isConstant)
            : base(tok, type, level)
        {
            IsConstant = isConstant;
        }

        public override void PrintIdent()
        {
            Console.WriteLine($"Variable: {Tok.Text}, Tipo: {Type}, Nivel: {Level}, Constante: {IsConstant}");
        }
    }

    public class MethodIdent : Ident
    {
        public List<string> Params { get; }

        public MethodIdent(IToken tok, SymbolType type, int level, List<string> parameters)
            : base(tok, type, level)
        {
            Params = parameters;
        }

        public override void PrintIdent()
        {
            Console.WriteLine($"Funcion: {Tok.Text}, Tipo: {Type}, Nivel: {Level}, Parametros: {Params.Count}");
        }
    }

    public SymbolsTable()
    {
        table = new List<Dictionary<string, Ident>>();
        currentLevel = -1;
    }

    public void OpenScope()
    {
        currentLevel++;
        if (currentLevel >= table.Count)
        {
            table.Add(new Dictionary<string, Ident>());
        }
    }

    public void CloseScope()
    {
        if (currentLevel >= 0)
        {
            table[currentLevel].Clear();
            currentLevel--;
        }
    }

    public void InsertVariable(IToken id, SymbolType type)
    {
        if (currentLevel >= 0 && currentLevel < table.Count)
        {
            VarIdent varIdent = new VarIdent(id, type, currentLevel, false);
            table[currentLevel][id.Text] = varIdent;
        }
    }

    public void InsertFunction(IToken id, SymbolType type, List<string> paramsList)
    {
        if (currentLevel >= 0 && currentLevel < table.Count)
        {
            Ident methodIdent = new MethodIdent(id, type, currentLevel, paramsList);
            table[currentLevel][id.Text] = methodIdent;
        }
    }

    public Ident Search(string name)
    {
        for (int i = currentLevel; i >= 0; i--)
        {
            if (table[i].ContainsKey(name))
            {
                return table[i][name];
            }
        }

        return null;
    }

    public Ident SearchInCurrentLevel(string name)
    {
        if (currentLevel >= 0 && currentLevel < table.Count && table[currentLevel].ContainsKey(name))
        {
            return table[currentLevel][name];
        }

        return null;
    }

    public Ident SearchInSpecificLevel(int level, string name)
    {
        if (level >= 0 && level < table.Count && table[level].ContainsKey(name))
        {
            return table[level][name];
        }

        return null;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void Print()
    {
        Console.WriteLine("////////// INICIO TABLA \\\\\\\\\\");
        for (int i = 0; i <= currentLevel; i++)
        {
            Console.WriteLine($"Nivel {i}:");
            foreach (var id in table[i].Values)
            {
                id.PrintIdent();
            }
        }

        Console.WriteLine("////////// FINAL TABLA \\\\\\\\\\\\\\\\\\\\");
    }
}

public enum SymbolType
{
    Variable,
    Function,
    Parameter
}