using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyApp.Shared.Services;
using MyApp.Starter.Helpers;
using Serilog;
using System;

namespace MyApp.Starter;

public class MyAppExternalApplication : IExternalApplication
{
    public Result OnShutdown(UIControlledApplication application)
    {
        Log.CloseAndFlush();

        return Result.Succeeded;
    }

    public Result OnStartup(UIControlledApplication uIControlledApplication)
    {\\pub\dev\00_Shared\01_Incoming\01_Rebels\Stage 2
        try
        {
            LoggerInitHelper.Init();
            SampleServiceProvider.SetUp(uIControlledApplication);

            var logger = SampleServiceProvider
                .ServiceProvider.GetRequiredService<ILogger<MyAppExternalApplication>>();

            logger.LogInformation($"Load services GUID {SampleServiceProvider.Guid}");
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
