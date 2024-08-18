using GrapeNetwork.AccessLevel.Common;
using GrapNetwork.LogWriter;
using System;
using System.Globalization;

namespace GrapeNetwork.Console.Common
{
    public static class ConsoleManager
    {
        static System.ConsoleColor standartColor = System.ConsoleColor.White;
        /// <summary>
        /// Выводит текст, перекрашивает его и приводит Foreground к цвету по умолчанию
        /// </summary>
        /// <param name="message"><Текст/param>
        /// <param name="oldColor">Старый цвет для перекраски текста</param>
        /// <param name="newColor">Цвет для перекраски текста</param>
        public static void WriteAndEditColor(string message, System.ConsoleColor oldColor, System.ConsoleColor newColor)
        {
            System.Console.ForegroundColor = newColor;
            System.Console.Write(message);
            System.Console.ForegroundColor = oldColor;
        }
        /// <summary>
        /// Написание и перекраска текста
        /// </summary>
        /// <param name="message">Текст</param>
        /// <param name="newColor">Цвет для перекраски текста</param>
        public static void WriteAndEditColor(string message, System.ConsoleColor newColor)
        {
            System.Console.ForegroundColor = newColor;
            System.Console.Write(message);
            System.Console.ForegroundColor = standartColor;
        }
        /// <summary>
        /// Выводит текст на следущею строку, перекрашивает его и приводит Foreground к цвету по умолчанию
        /// </summary>
        /// <param name="message"><Текст/param>
        /// <param name="oldColor">Старый цвет для перекраски текста</param>
        /// <param name="newColor">Цвет для перекраски текста</param>
        public static void WriteLineAndEditColor(string message, System.ConsoleColor oldColor, System.ConsoleColor newColor)
        {
            System.Console.ForegroundColor = newColor;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = oldColor;
        }
        /// <summary>
        /// Написание текст на следущей строке и его перекраска 
        /// </summary>
        /// <param name="message">Текст</param>
        /// <param name="newColor">Цвет для перекраски текста</param>
        public static void WriteLineAndEditColor(string message, ConsoleColor newColor)
        {
            System.Console.ForegroundColor = newColor;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = standartColor;
        }
        /// <summary>
        /// Выводит текст на текущею строку
        /// </summary>
        /// <param name="message">Текст</param>
        public static void Write(string message)
        {
            System.Console.Write(message);
        }
        /// <summary>
        /// Выводит текст на следующию строку
        /// </summary>
        /// <param name="message">Текст</param>
        public static void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }
        /// <summary>
        /// Выводит текст на следующию строку
        /// </summary>
        /// <param name="message">Текст</param>
        public static void WriteLine()
        {
            System.Console.WriteLine();
        }
        /// <summary>
        /// Ждет вашего нажатия кнопки
        /// </summary>
        public static void ReadKey()
        {
            System.Console.ReadKey();
        }

        public static void SkipLine(int countSkipLine)
        {
            for (int i = 0; i < countSkipLine; i++)
            {
                System.Console.WriteLine();
            }
        }
        /// <summary>
        /// Очистка окна консоли
        /// </summary>
        public static void Clear()
        {
            System.Console.Clear();
        }

        private static CultureInfo culture = new CultureInfo("en-US", false);
        private static AccessRole accessLevel = new AccessRole();

        public static void Info(string message)
        {
            CultureInfo.CurrentCulture = culture;
            WriteLineAndEditColor($"\n{GetInfoAboutRecord()} [INFO]: {message}", ConsoleColor.White);
        }

        public static void Debug(string message)
        {
            CultureInfo.CurrentCulture = culture;
            WriteLineAndEditColor($"\n{GetInfoAboutRecord()} [DEBUG]: {message}", ConsoleColor.Green);
        }

        public static void Warning(string message)
        {
            CultureInfo.CurrentCulture = culture;
            WriteLineAndEditColor($"\n{GetInfoAboutRecord()} [WARNING]: {message}", ConsoleColor.Yellow);
        }

        public static void Error(Exception exception)
        {
            CultureInfo.CurrentCulture = culture;
            WriteLineAndEditColor($"\n{GetInfoAboutRecord()} [ERROR]: {exception.TargetSite.DeclaringType} {exception.TargetSite.Name} {exception.Message}", ConsoleColor.DarkRed);
        }

        public static void Fatal(Exception exception)
        {
            CultureInfo.CurrentCulture = culture;
            WriteLineAndEditColor($"\n{GetInfoAboutRecord()} [FATAL]: {exception.TargetSite.DeclaringType} {exception.TargetSite.Name} {exception.Message}", ConsoleColor.Red);
        }

        public static void Log(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Info:
                    Info(message);
                    break;
                case LogLevel.Debug:
                    Debug(message);
                    break;
                case LogLevel.Warning:
                    Warning(message);
                    break;
                default:
                    break;
            }
        }

        public static void Log(LogLevel logLevel, Exception exception)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    Error(exception);
                    break;
                case LogLevel.Fatal:
                    Fatal(exception);
                    break;
                default:
                    break;
            }
        }

        private static string GetInfoAboutRecord()
        {
            return $"{DateTime.Now.ToString("MMM dd.MM.yyyy HH:mm:ss K")} {accessLevel.ToString()}:";
        }
    }
}