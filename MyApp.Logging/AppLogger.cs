using System;
using System.IO;
using System.Reflection;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.WithCaller;

namespace MyApp.Logging;
public static class AppLogger
{
    private static readonly Logger Logger;

    static AppLogger()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var logDirectory = Path.Combine(assemblyPath, "logs");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(logDirectory, "log-.txt"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Log.Information("Logger initialized");

        var applicationName = Assembly.GetExecutingAssembly().GetName().Name;
        Log.Information($"Starting application: {applicationName}");
    }

    public static void Info(string message) => Logger.Information(message);

    public static void Warn(string message) => Logger.Warning(message);

    public static void Error(string message, Exception? ex = null) =>
        Logger.Error(ex, message);

    public static void Error(Exception ex) =>
        Logger.Error(ex, "Unknow error");

    public static void Debug(string message) => Logger.Debug(message);

    public static void Fatal(string message, Exception? ex = null) =>
        Logger.Fatal(ex, message);

    public static void Close() => Log.CloseAndFlush();
}
