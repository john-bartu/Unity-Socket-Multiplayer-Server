using System;
using System.Collections.Generic;
using System.Text;

namespace UnitySocketMultiplayerServer
{
    static class Debug
    {
        private static bool silentMode;

        public static void SetSilent(bool silentEnabled)
        {
            silentMode = silentEnabled;
        }

        public static void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        public static void Log(string tag, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine('[' + tag + "]: " + message);
        }
        public static void LogColor(string tag, string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine('[' + tag + "]: " + message);
        }

        public static void LogInfo(string message)
        {
            LogColor("INFO", message, ConsoleColor.Green);
        }
        public static void LogData(string message)
        {
            if (!silentMode) LogColor("DATA", message, ConsoleColor.Blue);
        }
        public static void LogDB(string message)
        {
            if (!silentMode) LogColor("DB", message, ConsoleColor.Yellow);
        }


        public static void LogError(string message)
        {
            LogColor("ERROR", message, ConsoleColor.Red);
        }

    }
}
