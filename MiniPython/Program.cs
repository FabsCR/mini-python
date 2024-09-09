using System;
using Antlr4.Runtime;

namespace MiniPython
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MiniPython Interpreter");
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: MiniPython <source file>");
                return;
            }

            // Cargar el archivo fuente de Python Mini
            string filePath = args[0];
            string inputCode = System.IO.File.ReadAllText(filePath);

            // Crear un lexer y parser
            var lexer = new MiniPythonLexer(new AntlrInputStream(inputCode));
            var tokens = new CommonTokenStream(lexer);
            var parser = new MiniPythonParser(tokens);

            // Ejecutar la interpretación
            var tree = parser.file_input(); // Asumiendo que `file_input` es la regla inicial
            Console.WriteLine("Parsing completed.");
        }
    }
}