using System;

namespace SmartHome.Clients.WebApp.Services.Logger
{
    public static class Logger
    {
        public static void LogWarning(Exception ex, string message)
        {
            Console.WriteLine($"Warning: {message} | Exception thrown: {ex.Message} | Stack trace: {ex.StackTrace}");
        }

        public static void LogWarning(string message)
        {
            Console.WriteLine($"Warning: {message}");
        }
    }
}