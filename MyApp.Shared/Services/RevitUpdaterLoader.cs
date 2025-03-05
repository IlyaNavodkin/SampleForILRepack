using Autodesk.Revit.DB;
using Microsoft.Extensions.Logging;
using MyApp.Logging;
using System.Collections.Generic;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace MyApp.Shared.Services;

public class RevitUpdaterLoader
{
    private List<IUpdater> registeredUpdaters = new List<IUpdater>();

    public void RegisterUpdaters(List<IUpdater> updaters)
    {
        AppLogger.Info("Starting register updaters");

        foreach (IUpdater updater in updaters)
        {
            UpdaterRegistry.RegisterUpdater(updater);

            registeredUpdaters.Add(updater);

            AppLogger.Info($"Registered updater with id {updater.GetUpdaterId()} and name {updater.GetUpdaterName()}");
        }
    }

    public void RegisterUpdaters()
    {
        AppLogger.Info("Starting unregister updaters");

        foreach (IUpdater updater in registeredUpdaters)
        {
            UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId());

            registeredUpdaters.Remove(updater);

            AppLogger.Info($"Unregistered updater with id {updater.GetUpdaterId()} and name {updater.GetUpdaterName()}");
        }
    }
}
