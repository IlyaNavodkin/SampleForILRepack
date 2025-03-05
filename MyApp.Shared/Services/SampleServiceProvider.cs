using Autodesk.Revit.UI;
using DnsClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.RevitUi.Services;
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

        services.AddSingleton(controlledApplication);
        services.AddSingleton<ILogger, SerilogLogger>();

        services.AddSingleton<UiComponentFactory>();
        services.AddSingleton<RevitUiConfigurator>();

        _serviceProvider = services.BuildServiceProvider();
    }
}
