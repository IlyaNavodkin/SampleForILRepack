using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Shared;
using MyApp.Shared.Services;
using Serilog;
using System;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MyApp.Starter;

public class MyAppExternalApplication : IExternalApplication
{
    public Result OnShutdown(UIControlledApplication application)
    {
        Log.CloseAndFlush();

        return Result.Succeeded;
    }

    public Result OnStartup(UIControlledApplication uIControlledApplication)
    {
        try
        {
            SerilogLogger.Init();
            SampleServiceProvider.SetUp(uIControlledApplication);

            var logger = SampleServiceProvider
                .ServiceProvider.GetRequiredService<ILogger>();

            logger.LogInformation($"Load services GUID {SampleServiceProvider.Guid}");

            var uiService = SampleServiceProvider.ServiceProvider.GetRequiredService<RevitUiConfigurator>();

            uiService.ConfigureRevitUiComponents();

            logger.LogInformation("Startup complete");

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Startup failed");

            TaskDialog.Show("Ribbon Sample", ex.ToString());

            return Result.Failed;
        }
    }
}
