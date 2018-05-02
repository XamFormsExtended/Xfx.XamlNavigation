using System;
using System.Diagnostics;
using Prism.Logging;

namespace example.Logging
{
    internal class Logger
    {
        public static void Log(string message, Category category, Priority high)
        {
            Debug.WriteLine(message);
        }

        public static void Log(Exception exception, Category category, Priority high)
        {
            Debug.WriteLine(exception.Message);
            Debug.WriteLine(exception.StackTrace);
        }
    }
}