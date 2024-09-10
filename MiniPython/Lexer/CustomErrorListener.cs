using Antlr4.Runtime;
using System;
using System.Collections.Generic;

namespace MiniPython
{
    public class CustomErrorListener : BaseErrorListener
    {
        // Lista de mensajes de error para almacenar errores léxicos y sintácticos
        public List<string> ErrorMessages { get; } = new List<string>();

        // Método para manejar errores de sintaxis
        public void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            // Formatear el mensaje de error con la ubicación del error
            string errorMessage = $"Error en la línea {line}:{charPositionInLine} - {msg}";
            ErrorMessages.Add(errorMessage);
            Console.WriteLine(errorMessage);
        }

        // Verificar si hay errores
        public bool HasErrors => ErrorMessages.Count > 0;

        // Método para convertir la lista de errores a una cadena
        public override string ToString() => string.Join(Environment.NewLine, ErrorMessages);
    }
}