using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Logging;
using MyApp.MEP.Services;
using MyApp.MEP.Updaters;
using MyApp.Shared.Services;
using System.Collections.Generic;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MyApp.Starter.Helpers;

public static class RevitComponentResolver
{
    public static void ConfigureRevitComponents()
    {
        ConfigureUi();
        ConfigureUpdaters();

        AppLogger.Info("Revit components is configured");
    }

    private static void ConfigureUpdaters()
    {
        var revitUpdaterLoader = SampleServiceProvider.ServiceProvider.GetRequiredService<RevitUpdaterLoader>();
        var uiControlledApplication = SampleServiceProvider.ServiceProvider.GetRequiredService<UIControlledApplication>();
        var simpleDimpleService = new SimpleDimpleService();

        var updaters = new List<IUpdater>
        {
            new ElementChangeLoggerUpdater(uiControlledApplication.ActiveAddInId, simpleDimpleService)
        };

        revitUpdaterLoader.RegisterUpdaters(updaters);
    }

    private static void ConfigureUi()
    {
        var uiService = SampleServiceProvider.ServiceProvider.GetRequiredService<RevitUiConfigurator>();
        uiService.ConfigureRevitUiComponents();
    }
}