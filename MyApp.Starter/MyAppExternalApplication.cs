using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Shared.Services;
using MyApp.Starter.Helpers;
using Serilog;

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
            LoggerInitHelper.Init();
            SampleServiceProvider.SetUp(uIControlledApplication);

            var logger = SampleServiceProvider
                .ServiceProvider.GetRequiredService<ILogger<MyAppExternalApplication>>();

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
