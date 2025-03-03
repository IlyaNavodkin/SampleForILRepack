using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

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
            loggingBuilder.AddSerilog(Log.Logger);
        });

        services.AddSingleton<SimpleDimpleService>();

        _serviceProvider = services.BuildServiceProvider();
    }
}
