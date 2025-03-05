using Autodesk.Revit.UI;
using DnsClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MyApp.Shared.Services;

public static class SampleServiceProvider
{
    public static Guid Guid = Guid.NewGuid();

    private static IServiceProvider _serviceProvider;

    public static IServiceProvider ServiceProvider => _serviceProvider ??
        throw new InvalidOperationException($"{nameof(SampleServiceProvider)} has not been initialized. " +
            $"Use {nameof(SetUp)} method to set up the service provider.");

    public static void SetUp(UIControlledApplication controlledApplication)
    {
        if (_serviceProvider != null)
            throw new InvalidOperationException($"{nameof(SampleServiceProvider)} can only be initialized once.");
        var services = new ServiceCollection();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders(); 
            loggingBuilder.AddProvider(new SerilogLoggerProvider());
        });

        services.AddSingleton<UiComponentFactory>();
        services.AddSingleton<RevitUiConfigurator>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public static ILogger GetLogger(string categoryName)
    {
        var loggerFactory = ServiceProvider.GetService<ILoggerFactory>();
        if (loggerFactory == null)
            throw new InvalidOperationException("ILoggerFactory is not registered in the service provider.");

        return loggerFactory.CreateLogger(categoryName);
    }
}
