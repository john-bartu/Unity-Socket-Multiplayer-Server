using System;
using System.Collections.Generic;
using System.Text;

namespace UnitySocketMultiplayerServer
{
    static class Debug
    {
        private static bool silentMode;

        /// <summary>
        /// Set silent mode with turns of some logs
        /// </summary>
        /// <param name="silentEnabled">Status of Silent</param>
        public static void SetSilent(bool silentEnabled)
        {
            silentMode = silentEnabled;
        }

        /// <summary>
        /// Log line of text
        /// </summary>
        /// <param name="message">Message</param>
        public static void Log(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }

        /// <summary>
        /// Log line of text with tag
        /// [TAG]: Message
        /// </summary>
        /// <param name="tag">[TAG]</param>
        /// <param name="message">Message</param>
        public static void Log(string tag, string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine('[' + tag + "]: " + message);
        }

        /// <summary>
        /// Log line of text with tag and color
        /// [TAG]: Message (colorized)
        /// <param name="tag">[TAG]</param>
        /// <param name="message">Message</param>
        /// <param name="color">Color of text</param>
        public static void LogColor(string tag, string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine('[' + tag + "]: " + message);
        }

        /// <summary>
        /// Log text with INFO tag + unique color
        /// Log information messages
        /// </summary>
        /// <param name="message">Info Message</param>
        public static void LogInfo(string message)
        {
            LogColor("INFO", message, ConsoleColor.Green);
        }

        /// <summary>
        /// Log text with DATA tag + unique color
        /// Use when this is data transfer (TCP/Files...)
        /// DOESNT PRINT ON SILENT MODE
        /// </summary>
        /// <param name="message">Data Message</param>
        public static void LogData(string message)
        {
            if (!silentMode) LogColor("DATA", message, ConsoleColor.Blue);
        }

        /// <summary>
        /// Log text with DB tag
        /// Use when something is Database operation
        /// DOESNT PRINT ON SILENT MODE
        /// </summary>
        /// <param name="message">Database Message</param>
        public static void LogDB(string message)
        {
            if (!silentMode) LogColor("DB", message, ConsoleColor.Yellow);
        }

        /// <summary>
        /// Log text with ERROR tag + unique color
        /// Use when something is error and wants to highlight it
        /// </summary>
        /// <param name="message">Error Message</param>
        public static void LogError(string message)
        {
            LogColor("ERROR", message, ConsoleColor.Red);
        }

    }
}
