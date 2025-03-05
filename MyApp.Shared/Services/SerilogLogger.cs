using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MyApp.Shared.Services;

public class SerilogLogger : ILogger
{
    private readonly string _categoryName;

    public SerilogLogger(string categoryName)
    {
        _categoryName = categoryName;
    }

    public static void Init()
    {
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var logDirectory = Path.Combine(assemblyPath, "logs");

        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug() 
            .WriteTo.File(Path.Combine(logDirectory, "log-.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Serilog.Log.Information("Logger setup complete");

        var applicationName = Assembly.GetExecutingAssembly().GetFriendlyNameWithVersion();
        Serilog.Log.Information($"Starting application {applicationName}");
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return Serilog.Context.LogContext.PushProperty("Scope", state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        var serilogLevel = MapLogLevel(logLevel);
        return Serilog.Log.Logger.IsEnabled(serilogLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var serilogLevel = MapLogLevel(logLevel);
        var message = formatter(state, exception);

        if (exception != null)
        {
            Serilog.Log.Logger.Write(serilogLevel, exception, "[{Category}] {EventId} - {Message}", _categoryName, eventId.Id, message);
        }
        else
        {
            Serilog.Log.Logger.Write(serilogLevel, "[{Category}] {EventId} - {Message}", _categoryName, eventId.Id, message);
        }
    }

    private static LogEventLevel MapLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => LogEventLevel.Verbose,
            LogLevel.Debug => LogEventLevel.Debug,
            LogLevel.Information => LogEventLevel.Information,
            LogLevel.Warning => LogEventLevel.Warning,
            LogLevel.Error => LogEventLevel.Error,
            LogLevel.Critical => LogEventLevel.Fatal,
            LogLevel.None => LogEventLevel.Verbose,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), $"Unsupported log level: {logLevel}")
        };
    }
}
