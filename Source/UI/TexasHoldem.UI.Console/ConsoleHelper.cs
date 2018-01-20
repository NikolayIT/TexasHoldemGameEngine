namespace TexasHoldem.UI.Console
{
    using System;

    public static class ConsoleHelper
    {
        public static void WriteOnConsole(int row, int col, string text, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }

        public static string UserInput(int row, int col)
        {
            Console.SetCursorPosition(col, row);
            return Console.ReadLine();
        }
    }
}
