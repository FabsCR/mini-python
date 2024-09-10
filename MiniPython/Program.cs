using System;
using System.IO;
using Antlr4.Runtime;

namespace MiniPython
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MiniPython Interpreter");

            // Obtener el directorio donde se encuentra el código fuente (uno o dos niveles arriba de bin/Debug)
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            Console.WriteLine($"Carpeta del proyecto: {projectDirectory}");

            // Generar la ruta completa para 'test.txt'
            string filePath = Path.Combine(projectDirectory, "test.txt");

            // Comprobar si el archivo existe
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Error: No se encontró el archivo {filePath}");
                return;
            }

            // Leer el contenido del archivo test.txt
            string inputCode = File.ReadAllText(filePath);

            // Crear un lexer y parser
            var lexer = new MiniPythonLexer(new AntlrInputStream(inputCode));
            var tokens = new CommonTokenStream(lexer);
            var parser = new MiniPythonParser(tokens);

            // Parsear el archivo usando la regla inicial
            var tree = (ParserRuleContext)parser.program();
            Console.WriteLine("Parsing completed.");
        }
    }
}