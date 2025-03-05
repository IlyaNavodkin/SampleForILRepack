using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Microsoft.Extensions.Logging;
using MyApp.Logging;
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
    {
        try
        {
            AppLogger.Info("Starting application");

            SampleServiceProvider.SetUp(uIControlledApplication);
            RevitComponentResolver.ConfigureRevitComponents();

            AppLogger.Info("Startup success");

            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            AppLogger.Error("Startup failed", ex);

            TaskDialog.Show("Ribbon Sample", ex.ToString());

            return Result.Failed;
        }
    }

}
