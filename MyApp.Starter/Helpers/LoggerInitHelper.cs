using MyApp.Starter.Extensions;
using Serilog;
using System.Reflection;

namespace MyApp.Starter.Helpers;

public static class LoggerInitHelper
{
    public static void Init()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var logDirectory = Path.Combine(assemblyPath, "logs");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(logDirectory, "log-.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("Logger setup complete");

        var applicationName = typeof(MyAppExternalApplication).Assembly
            .GetFriendlyNameWithVersion();

        Log.Information($"Starting application {applicationName}");
    }

}


